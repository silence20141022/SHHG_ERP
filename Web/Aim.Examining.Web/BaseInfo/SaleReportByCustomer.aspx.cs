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
    public partial class SaleReportByCustomer : ExamListPage
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
            string customerId = RequestData.Get<string>("CustomerId");
            string beginDate = RequestData.Get<string>("BeginDate");
            string endDate = RequestData.Get<string>("EndDate");
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = "1900-01-01 00:00:00";
            }
            if (string.IsNullOrEmpty(endDate))
            {
                endDate = "2099-12-30 00:00:00";
            }
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(customerId))
            { 
                sql = @"select A.PCode,A.PName,(isnull(A.Count,0)-isnull(ReturnCount,0)) Count,Convert(varchar(10),A.CreateTime,120) CreateTime ,
                A.SalePrice,(isnull(A.Count,0)-isnull(ReturnCount,0))*A.SalePrice SaleAmount from SHHG_AimExamine..OrdersPart A                 
                left join SHHG_AimExamine..SaleOrders B on A.OId=B.Id  where B.CId='{0}'";
                sql = string.Format(sql, customerId);
                this.PageState.Add("DeliveryOrderPartList", GetPageData(sql, SearchCriterion));
            }
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
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
