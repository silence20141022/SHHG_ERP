using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Aim.Examining.Model;
using NHibernate.Criterion;
using Aim.Portal.Model;
using Aim.Data;
using Aim.Examining.Web;
using Aim.WorkFlow;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmOtherAppView : ExamListPage
    {
        private IList<OtherCost> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else
            {
                DoSelect();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");

            if (idList != null && idList.Count > 0)
            {
                OtherCost.DoBatchDelete(idList.ToArray());
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
                SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));

            ents = OtherCost.FindAll(SearchCriterion);
            this.PageState.Add("OtherCostList", ents);
        }
    }
}
