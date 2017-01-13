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
namespace Aim.Examining.Web.CommonPages.Select
{
    public partial class PurchaseProductSelect : BaseListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }

        private void DoSelect()
        {
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
            }
            string supplierId = RequestData.Get<string>("SupplierId");
            string productType = Server.UrlDecode(RequestData.Get<string>("ProductType"));
            string priceType = Server.UrlDecode(RequestData.Get<string>("PriceType"));
            string sql = @"select A.*,(select Price from SHHG_AimExamine..ProductPriceDetail where PriceTypeName='{0}' and
            ProductId=A.Id) as Price,SHHG_AimExamine.dbo.fun_getProQuantity(A.Id) as StockQuantity
            from SHHG_AimExamine..Products as A 
            where A.SupplierId='{1}' and  A.ProductType='{2}'" + where;
            sql = string.Format(sql, priceType, supplierId, productType);
            PageState.Add("ProductList", GetPageData(sql, SearchCriterion));
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
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * search.PageSize + 1, search.CurrentPageIndex * search.PageSize);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }
    }
}


