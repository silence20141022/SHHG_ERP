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
namespace Aim.Examining.Web.SaleManagement
{
    public partial class StockCheckProductSelect : BaseListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }
        private void DoSelect()
        {
            string warehouseId = RequestData.Get<string>("WarehouseId");
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            string sql = @"select A.*,B.Pcn as ProductPcn from SHHG_AimExamine..StockInfo as A left join SHHG_AimExamine..Products
            as B on A.ProductId=B.Id where A.WarehouseId='{0}'" + where;
            if (!string.IsNullOrEmpty(warehouseId))
            {
                sql = string.Format(sql, warehouseId);
                PageState.Add("DataList", GetPageData(sql, SearchCriterion));
            }
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "ProductCode";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " asc" : " desc";
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


