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
    public partial class CombineSplitView : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            CombineSplit csEnt = null;
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
                    csEnt = CombineSplit.Find(id);
                    SetFormData(csEnt);
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                IList<CombineSplitDetail> csdEnts = CombineSplitDetail.FindAll(new Order("ProductCode", true), Expression.Eq("CombineSplitId", id));
                PageState.Add("DetailDataList", csdEnts);
            }
            else
            {
                PageState.Add("CombineSplitNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getCombineSplitNo()").ToString());
            }
        }
    }
}

