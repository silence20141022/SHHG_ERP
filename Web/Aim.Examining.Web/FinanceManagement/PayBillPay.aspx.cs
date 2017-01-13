using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using Aim.Data;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.Examining.Model;
using System.Web.Script.Serialization;

namespace Aim.Examining.Web.FinanceManagement
{
    public partial class PayBillPay : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型        
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            PayBill ent = null;
            switch (RequestActionString)
            {
                case "PayBillPay":
                    ent = PayBill.Find(id);
                    ActualPayDetail apdEnt = new ActualPayDetail();
                    if (!string.IsNullOrEmpty(RequestData.Get<string>("PayAmount")))
                    {
                        ent.ActuallyPayAmount = ent.ActuallyPayAmount + RequestData.Get<decimal>("PayAmount");
                        apdEnt.ActualPayAmount = RequestData.Get<decimal>("PayAmount");
                    }
                    apdEnt.PayBillId = ent.Id;
                    apdEnt.CreateId = UserInfo.UserID;
                    apdEnt.CreateName = UserInfo.Name;
                    apdEnt.CreateTime = System.DateTime.Now;
                    apdEnt.Remark = RequestData.Get<string>("Remark");
                    ent.DoUpdate();
                    apdEnt.DoCreate();
                    IList<PayBillDetail> pbdEnts = PayBillDetail.FindAll("from PayBillDetail where PayBillId='" + ent.Id + "'");
                    Supplier supplierEnt = Supplier.Find(ent.SupplierId);
                    if (supplierEnt.Symbo == "￥")//如果付款单的币种为本币
                    {
                        if (ent.PAmount == ent.ActuallyPayAmount + (ent.DiscountAmount.HasValue ? ent.DiscountAmount.Value : 0))
                        {
                            ent.State = "已付款"; ent.DoUpdate();
                        }
                    }
                    //付款成功后 如果付款单为已付款状态 遍历采购单详细 
                    if (ent.State == "已付款")
                    {
                        string tempsql = @"select * from SHHG_AimExamine..PayBillDetail where PayBillId='{0}'";
                        tempsql = string.Format(tempsql, ent.Id);
                        IList<EasyDictionary> dicPayDetail = DataHelper.QueryDictList(tempsql);
                        PurchaseOrderDetail podEnt = null;
                        ArrayList idarray = new ArrayList();//搜集涉及的采购单ID
                        string poId = string.Empty;
                        string podId = string.Empty;
                        int nopay = 0;
                        foreach (EasyDictionary ea in dicPayDetail)
                        {
                            podId = ea.Get<string>("PurchaseOrderDetailId");
                            podEnt = PurchaseOrderDetail.TryFind(podId);
                            //取得采购详细的实际未付款数目
                            nopay = Convert.ToInt32(DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_ActuallNoPayQuan('" + podId + "')"));
                            if (nopay == 0)
                            {
                                podEnt.PayState = "已付款";
                                podEnt.DoUpdate();
                            }
                            if (podEnt.PurchaseOrderId != poId)
                            {
                                poId = podEnt.PurchaseOrderId;
                                idarray.Add(poId);
                            }
                        }
                        //付款单涉及的采购详细表状态遍历完后。再遍历付款单涉及的采购单
                        PurchaseOrder poEnt = null;
                        foreach (object ob in idarray)
                        {
                            IList<PurchaseOrderDetail> podEnts = PurchaseOrderDetail.FindAll("from PurchaseOrderDetail where PayState='未付款' and PurchaseOrderId='" + ob.ToString() + "'");
                            if (podEnts.Count == 0)
                            {
                                poEnt = PurchaseOrder.TryFind(ob.ToString());
                                poEnt.PayState = "已付款";
                                if (poEnt.InvoiceState == "已关联" && poEnt.InWarehouseState == "已入库")
                                {
                                    poEnt.OrderState = "已结束";
                                }
                                poEnt.DoUpdate();
                            }
                        }
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    string tempsql = @"select A.* ,B.MoneyType,B.Symbo from SHHG_AimExamine..PayBill as A 
                    left join SHHG_AimExamine..Supplier as B on A.SupplierId=B.Id where A.Id='{0}'";
                    tempsql = string.Format(tempsql, id);
                    IList<EasyDictionary> dics = DataHelper.QueryDictList(tempsql);
                    if (dics.Count > 0)
                    {
                        this.SetFormData(dics[0]);
                    }
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.*,B.PayBillNo,d.PurchaseOrderNo,C.PurchaseOrderId from SHHG_AimExamine..PayBillDetail as A  
                left join SHHG_AimExamine..PurchaseOrderDetail as C  on A.PurchaseOrderDetailId=C.Id    
                left join SHHG_AimExamine..PurchaseOrder d on C.PurchaseOrderId=d.Id              
                left join SHHG_AimExamine..PayBill as B on A.PayBillId=B.Id  where A.PayBillId='{0}'";
                sql = string.Format(sql, id);
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", dics);
                string sql2 = @"select A.*,B.PayBillNo from SHHG_AimExamine..ActualPayDetail as A 
                left join SHHG_AimExamine..PayBill as B on A.PayBillId=B.Id where A.PayBillId='{0}'";
                sql2 = string.Format(sql2, id);
                IList<EasyDictionary> dics2 = DataHelper.QueryDictList(sql2);
                PageState.Add("PayDetailList", dics2);
            }
            else
            {
                PageState.Add("PayBillNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getPayBillNo()").ToString());
            }
        }
    }
}

