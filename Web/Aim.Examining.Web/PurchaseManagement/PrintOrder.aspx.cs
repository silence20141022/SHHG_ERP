using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Aim.Portal;
using Aim.Portal.Model;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Castle.ActiveRecord;
using System.Web.Script.Serialization;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PrintOrder : ExamBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string purchaseOrderId = RequestData.Get<string>("PurchaseOrderId");
            if (!string.IsNullOrEmpty(purchaseOrderId))
            {
                string sql = @"select A.*,B.ProductType from SHHG_AimExamine..PurchaseOrderDetail as A 
                left join SHHG_AimExamine..PurchaseOrder  as B on A.PurchaseOrderId=B.Id where B.Id='{0}'";
                sql = string.Format(sql, purchaseOrderId);
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", dics);
                PurchaseOrder poEnt = PurchaseOrder.Find(purchaseOrderId);
                PageState.Add("PurchaseOrder", poEnt);
            }
        }
    }
}

