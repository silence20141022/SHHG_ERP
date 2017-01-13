using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Aim.Portal.Model;
using Aim.Data;
using System.Data.SqlClient;
using System.Data;


namespace Aim.Examining.Web
{
    public partial class CustomerPayEdit : ExamListPage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id 
        string sql = "";
        string CId = "";
        PaymentInvoice ent = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            CId = RequestData.Get<string>("CId");
            switch (RequestActionString)
            {
                case "create":
                    ent = this.GetPostedData<PaymentInvoice>();
                    ent.BillType = "发票";
                    ent.CreateId = UserInfo.UserID;
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = DateTime.Now;
                    ent.CollectionType = "销售收款";
                    ent.DoCreate();
                    UpdateOrderInvoice();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = PaymentInvoice.Find(id);
                }
            }
            else
            {
                ent = new PaymentInvoice();
            }
            SetFormData(ent);
            PageState.Add("PayType", SysEnumeration.GetEnumDict("PayType"));
        }
        private void UpdateOrderInvoice()//销售付款创建完毕后  如果该客户有应付款记录   开始执行对应操作
        {
            if (ent.Name == "自动销账")
            {
                PaymentInvoice piEnt = ent;
                piEnt.CorrespondInvoice = "";
                sql = @"select sum(Amount-isnull(PayAmount,0)) from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and CId='" + piEnt.CId + "'";
                decimal shouldpayAmount = DataHelper.QueryValue<decimal>(sql);//合计应付金额

                if (shouldpayAmount > 0)
                {
                    decimal validAmount = piEnt.Money.Value - (piEnt.CorrespondAmount.HasValue ? piEnt.CorrespondAmount.Value : 0);//有效金额
                    if (shouldpayAmount >= validAmount)//如果有效金额小于等于应付款总额 
                    {
                        sql = @"select * from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and CId='" + piEnt.CId + "' order by InvoiceDate asc";
                        IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                        foreach (EasyDictionary dic in dics)
                        {
                            if (validAmount > 0)
                            {
                                OrderInvoice oiEnt = OrderInvoice.Find(dic.Get<string>("Id"));
                                if (validAmount >= (oiEnt.Amount.Value - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0)))//大于等于该发票的未付金额
                                {
                                    validAmount = validAmount - (oiEnt.Amount.Value - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0));
                                    oiEnt.PayState = "已全部付款";
                                    piEnt.CorrespondInvoice += (string.IsNullOrEmpty(piEnt.CorrespondInvoice) ? "" : ",") + oiEnt.Number + "_" + (oiEnt.Amount - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0));
                                    oiEnt.PayAmount = oiEnt.Amount;
                                }
                                else
                                {
                                    oiEnt.PayAmount = (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0) + validAmount;
                                    oiEnt.PayState = "部分付款";
                                    piEnt.CorrespondInvoice += (string.IsNullOrEmpty(piEnt.CorrespondInvoice) ? "" : ",") + oiEnt.Number + "_" + validAmount;
                                    validAmount = 0;
                                }
                                oiEnt.DoUpdate();
                            }
                        }
                        piEnt.CorrespondAmount = piEnt.Money;
                        piEnt.CorrespondState = "已对应";
                        piEnt.Name = "自动销账";
                        piEnt.DoUpdate();
                    }
                    else //如果付款金额大于应付款总金额
                    {
                        sql = @"select * from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and CId='" + piEnt.CId + "' order by InvoiceDate asc";
                        IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                        foreach (EasyDictionary dic in dics)
                        {
                            OrderInvoice oiEnt = OrderInvoice.Find(dic.Get<string>("Id"));
                            piEnt.CorrespondInvoice += (string.IsNullOrEmpty(piEnt.CorrespondInvoice) ? "" : ",") + oiEnt.Number + "_" + (oiEnt.Amount - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0));
                            piEnt.CorrespondState = "部分对应";
                            piEnt.CorrespondAmount = (piEnt.CorrespondAmount.HasValue ? piEnt.CorrespondAmount.Value : 0) + shouldpayAmount;
                            piEnt.DoUpdate();
                            oiEnt.PayState = "已全部付款";
                            oiEnt.PayAmount = oiEnt.Amount;
                            oiEnt.DoUpdate();
                        }
                    }
                }
            }
            if (ent.Name == "手动销账")
            {
                string[] orderinvoiceArray = ent.CorrespondInvoice.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                ent.CorrespondInvoice = "";
                foreach (string str in orderinvoiceArray)
                {
                    OrderInvoice oiEnt = OrderInvoice.FindAllByProperty(OrderInvoice.Prop_Number, str).FirstOrDefault<OrderInvoice>();
                    ent.CorrespondInvoice += oiEnt.Number + "_" + (oiEnt.Amount - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0)) + ",";
                    oiEnt.PayAmount = oiEnt.Amount;
                    oiEnt.PayState = "已全部付款";
                    oiEnt.DoUpdate();
                }
                ent.CorrespondState = "已对应";
                ent.DoUpdate();
            }
            if (ent.Name == "暂不销账")
            {
                ent.CorrespondInvoice = "";
                ent.DoUpdate();
            }
        }
    }
}