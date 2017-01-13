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
    public partial class NoInvoiceCustomerPayEdit : ExamListPage
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
                    ent.BillType = "收据";
                    ent.CreateId = UserInfo.UserID;
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = DateTime.Now;
                    ent.DoCreate();
                    UpdateSaleOrder();
                    break;
                case "AutoCorrespond":
                    sql = @"select sum(TotalMoney-isnull(DiscountAmount,0)-isnull(ReturnAmount,0)-isnull(ReceiptAmount,0)) 
                    from SHHG_AimExamine..SaleOrders where  CId='" + CId + "' and State is null and InvoiceType='收据' and (PayState is null or PayState='部分付款')";
                    decimal TotalArrearage = DataHelper.QueryValue<decimal>(sql);
                    if (TotalArrearage >= RequestData.Get<decimal>("PayAmount"))
                    {
                        PageState.Add("Result", "T");
                    }
                    else
                    {
                        PageState.Add("Result", "F");
                    }
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
        private void UpdateSaleOrder()//销售付款创建完毕后  如果该客户有应付款记录   开始执行对应操作
        {
            if (ent.Name == "自动销账")
            {
                sql = @"select * from SHHG_AimExamine..SaleOrders  where InvoiceType='收据' and 
                CId='" + CId + "' and State is null and (PayState is null or PayState='部分付款') order by CreateTime asc";
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                decimal payamount = ent.Money.Value;
                ent.CorrespondInvoice = "";
                foreach (EasyDictionary dic in dics)
                {
                    if (payamount > 0)
                    {
                        SaleOrder oiEnt = SaleOrder.Find(dic.Get<string>("Id"));
                        decimal unpayAmount = oiEnt.TotalMoney.Value - (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) -
                            (oiEnt.DiscountAmount.HasValue ? oiEnt.DiscountAmount.Value : 0) - (oiEnt.ReturnAmount.HasValue ? oiEnt.ReturnAmount.Value : 0);
                        if (payamount >= unpayAmount)
                        {
                            payamount = payamount - unpayAmount;
                            oiEnt.ReceiptAmount = (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) + unpayAmount;
                            oiEnt.PayState = "已全部付款";
                            ent.CorrespondInvoice += oiEnt.Number + "_" + unpayAmount + ",";
                        }
                        else
                        {
                            oiEnt.ReceiptAmount = (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) + payamount;
                            ent.CorrespondInvoice += oiEnt.Number + "_" + payamount + ",";
                            payamount = 0;
                        }
                        oiEnt.DoUpdate();
                    }
                }
                ent.CorrespondState = "已对应";
                ent.DoUpdate();
            }
            if (ent.Name == "手动销账")
            {
                string[] SaleOrderArray = ent.CorrespondInvoice.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                ent.CorrespondInvoice = "";
                foreach (string str in SaleOrderArray)
                {
                    SaleOrder oiEnt = SaleOrder.FindAllByProperty(SaleOrder.Prop_Number, str).FirstOrDefault<SaleOrder>();
                    decimal unpayAmount = oiEnt.TotalMoney.Value - (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) -
                                (oiEnt.DiscountAmount.HasValue ? oiEnt.DiscountAmount.Value : 0) - (oiEnt.ReturnAmount.HasValue ? oiEnt.ReturnAmount.Value : 0);
                    ent.CorrespondInvoice += oiEnt.Number + "_" + unpayAmount + ",";
                    oiEnt.ReceiptAmount = (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) + unpayAmount;
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