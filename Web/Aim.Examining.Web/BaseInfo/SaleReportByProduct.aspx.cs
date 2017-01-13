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
using NHibernate.Criterion;

namespace Aim.Examining.Web
{
    public partial class SaleReportByProduct : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = "";
            string productId = RequestData.Get<string>("ProductId");
            string beginDate = RequestData.Get<string>("BeginDate");
            string endDate = RequestData.Get<string>("EndDate");
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.PropertyName))
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            if (!string.IsNullOrEmpty(beginDate))
            {
                where += " and B.CreateTime >='" + Convert.ToDateTime(beginDate) + "'";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                where += " and B.CreateTime <= '" + Convert.ToDateTime(endDate) + "'";
            }
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(productId))
            {
                //                sql = @"select sum(A.Count) as Total,B.CName from SHHG_AimExamine..OrdersPart as  A
                //                left join (Select Id,CName,CId,CreateTime,DeliveryState from SHHG_AimExamine..SaleOrders) as B on A.OId=B.Id  where A.PId='{0}' and B.DeliveryState<> '已作废'" + where + " group by CName";
                sql = @"select sum(isnull(Count,0)-isnull(ReturnCount,0)) Total,B.CId,B.CName from SHHG_AimExamine..OrdersPart A
                left join SHHG_AimExamine..SaleOrders B on A.OId=B.Id  where A.PId='{0}' group by B.Cid,B.CName";
                sql = string.Format(sql, productId);
                PageState.Add("OrdersPartList", GetPageData(sql, SearchCriterion));
            }
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Total";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " desc" : " asc";
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * 120 + 1, search.CurrentPageIndex * 120);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }
    }
}
