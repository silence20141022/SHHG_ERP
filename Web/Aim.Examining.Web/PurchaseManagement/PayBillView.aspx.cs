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

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PayBillView : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            IList<string> entStrList = RequestData.GetList<string>("data");
            switch (RequestActionString)
            {
                case "WorkFlowEnd":
                    PayBill ent = PayBill.Find(id);
                    ent.WorkFlowState = RequestData.Get<string>("state");
                    ent.ExamineResult = RequestData.Get<string>("ApprovalState");
                    ent.DoUpdate();
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
                        SetFormData(dics[0]);
                    }
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.*,c.PurchaseOrderNo,B.PurchaseOrderId
                       from SHHG_AimExamine..PayBillDetail as A                        
                       left join SHHG_AimExamine..PurchaseOrderDetail as B on A.PurchaseOrderDetailId=B.Id   
                       left join SHHG_AimExamine..PurchaseOrder c on c.Id=B.PurchaseOrderId                  
                       where A.PayBillId='{0}' order by B.PurchaseOrderId asc";
                sql = string.Format(sql, id);
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", dics);
            }
            else
            {
                PageState.Add("PayBillNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getPayBillNo()").ToString());
            }
        }
        private IList<EasyDictionary> FormatData(IList<EasyDictionary> dics)
        {
            string same = string.Empty;
            for (int i = 0; i < dics.Count; i++)
            {
                if (dics[i].Get<string>("PurchaseOrderId") == same)
                {
                    dics[i].Set("PurchaseOrderNo", "");
                }
                else
                {
                    same = dics[i].Get<string>("PurchaseOrderId");
                }
            }
            return dics;
        }
    }
}

