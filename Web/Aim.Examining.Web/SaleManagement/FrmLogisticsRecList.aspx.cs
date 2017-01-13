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
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmLogisticsRecList : ExamListPage
    {
        private IList<Logistic> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "mark")
            {
                string db = ConfigurationManager.AppSettings["ExamineDB"];
                string ids = RequestData.Get<string>("ids");
                DataHelper.ExecSql("update " + db + "..Logistics set PayState=1 where id in ('" + ids.Replace(",", "','") + "')");
            }
            else
            {
                DoSelect();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
                SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));

            string paystate = RequestData.Get<string>("paystate");
            if (paystate == "1")
            {
                SearchCriterion.AddSearch("PayState", "1");
                ents = Logistic.FindAll(SearchCriterion);
            }
            else
            {
                ents = Logistic.FindAll(SearchCriterion, Expression.Sql(" isnull(PayState,0) <> '1' "));
            }

            this.PageState.Add("LogisticList", ents);

        }
    }
}
