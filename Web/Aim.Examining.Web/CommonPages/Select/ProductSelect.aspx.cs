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
    public partial class ProductSelect : BaseListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }
        private void DoSelect()
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string WarehouseId = RequestData.Get<string>("WarehouseId");
            string where = "";
            if (!string.IsNullOrEmpty(WarehouseId))
            {
                where += " and Id in (select ProductId from " + db + "..StockInfo where WarehouseId='" + WarehouseId + "') ";
            }
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
            }
            string PId = Request.QueryString["PId"];//订单ID
          //  string sql = @"select *," + db + ".dbo.fun_getProQuantity(Id) as StockQuantity, " + db + ".dbo.fun_getDestinePro(Id,'" + PId + "') as DestineCount from " + db + "..Products where 1=1 " + where;
            string sql = @"select a.*,(select isnull(sum(StockQuantity),0) from SHHG_AimExamine..StockInfo where ProductId=a.Id) as StockQuantity, 
                         ((select isnull(sum(isnull(Count,0)-isnull(OutCount,0)),0) from SHHG_AimExamine..DelieryOrderPart t where t.ProductId=a.Id and isnull(State,'')<>'已出库')+
                         (select isnull(sum(isnull(Count,0)-isnull(OutCount,0)),0) from SHHG_AimExamine..OrdersPart n where n.PId=a.Id and n.IsValid=1 and isnull(OutCount,0)<>isnull(Count,0))) as DestineCount
                         from SHHG_AimExamine..Products a  where 1=1 " + where;
            this.PageState.Add("ProductList", GetPageData(sql, SearchCriterion));
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


