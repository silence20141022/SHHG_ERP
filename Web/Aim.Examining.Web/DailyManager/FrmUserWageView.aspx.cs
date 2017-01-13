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
using System.Configuration;
using System.Web.Script.Serialization;

namespace Aim.Examining.Web
{
    public partial class FrmUserWageView : ExamListPage
    {
        string db = ConfigurationManager.AppSettings["ExamineDB"];
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string Stage = RequestData.Get<string>("Stage");
            string where = " where Stage='" + Stage + "' ";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
            }

            string sql = @"select Id,UserID,UserName,WorkNo,LoginName,DeptName,Wage,Bonus,Total,CreateTime,Remark,Stage from " + db + "..UserWage" + where;
            this.PageState.Add("UserList", GetPageData(sql, SearchCriterion));
        }


        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "DeptName";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " desc" : " asc";
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * search.PageSize + 1, search.CurrentPageIndex * search.PageSize);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }
    }
}
