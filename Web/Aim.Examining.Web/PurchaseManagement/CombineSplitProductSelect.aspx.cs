using System;
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
using System.Configuration;
namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class CombineSplitProductSelect : BaseListPage
    {
        private IList<Product> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }

        private void DoSelect()
        {

            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            string sql = @"select A.*,B.Pcn from SHHG_AimExamine..StockInfo as A 
            left join (select Id,Pcn from  SHHG_AimExamine..Products) as B 
            on A.ProductId=B.Id where (select count(Id) from SHHG_AimExamine..ProductsPart where PId=A.ProductId)>0" + where;
            this.PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }

        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "ProductCode";
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


