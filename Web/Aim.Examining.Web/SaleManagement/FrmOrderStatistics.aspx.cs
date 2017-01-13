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
using Aim.WorkFlow;

namespace Aim.Examining.Web
{
    public partial class FrmOrderStatistics : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }


        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string where = " where (DeliveryState is null or DeliveryState<>'已作废') ";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate" && item.Value + "" != "")
                {
                    where += " and p.CreateTime>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate" && item.Value + "" != "")
                {
                    where += " and p.CreateTime<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.Value + "" != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }

            string sql = @"select c.Id,p.Number,p.CName,PName,PCode,Isbn,[Count],[Count]*SalePrice as Amount,Unit,SalePrice,OutCount,p.CreateTime from " + db + "..SaleOrders p inner join " + db + "..OrdersPart c on p.id=c.oid" + where;
            this.PageState.Add("OrderList", GetPageData(sql, SearchCriterion));
        }


        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Number";
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
