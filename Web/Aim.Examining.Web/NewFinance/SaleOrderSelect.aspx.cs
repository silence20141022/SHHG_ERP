using System;
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
using Aim.Examining.Web;
using NHibernate.Criterion;

namespace Aim.Examining.Web
{
    public partial class SaleOrderSelect : ExamBasePage
    {
        private SaleOrder[] ents = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value + ""))
                {
                    switch (item.PropertyName)
                    {
                        case "BeginDate":
                            where += " and CreateTime>'" + item.Value + "' ";
                            break;
                        case "EndDate":
                            where += " and CreateTime<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                            break;
                        default:
                            where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                    }
                }
            }
            string sql = "select * from SHHG_AimExamine..SaleOrders where State is null and DeState='已全部出库'" + where;
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(1) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";//当没有在前台点击排序列的时候 默认的排序字段
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " desc" : " asc";//默认降序  
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
