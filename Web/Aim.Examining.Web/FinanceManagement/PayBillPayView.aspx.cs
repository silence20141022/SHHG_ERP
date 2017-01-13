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
    public partial class PayBillPayView : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        #region ASP.NET 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            IList<string> entStrList = RequestData.GetList<string>("data");
            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    string tempsql = @"select A.* ,B.SupplierName,B.MoneyType,B.Symbo from SHHG_AimExamine..PayBill as A 
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
        #endregion
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.*,B.PurchaseOrderNo,B.Name,B.Code,B.Quantity,B.Amount,B.BuyPrice,B.PurchaseOrderId
                       from SHHG_AimExamine..PayBillDetail as A                        
                       left join SHHG_AimExamine..PurchaseOrderDetail as B on A.PurchaseOrderDetailId=B.Id                      
                       where A.PayBillId='{0}' order by B.PurchaseOrderId asc";
                sql = string.Format(sql, id);
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                this.PageState.Add("DetailList", dics);
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
        //private IList<EasyDictionary> FormatData(IList<EasyDictionary> dics)
        //{
        //    string same = string.Empty;
        //    for (int i = 0; i < dics.Count; i++)
        //    {
        //        if (dics[i].Get<string>("PurchaseOrderId") == same)
        //        {
        //            dics[i].Set("PurchaseOrderNo", "");
        //        }
        //        else
        //        {
        //            same = dics[i].Get<string>("PurchaseOrderId");
        //        }
        //    }
        //    return dics;
        //}
    }
}

