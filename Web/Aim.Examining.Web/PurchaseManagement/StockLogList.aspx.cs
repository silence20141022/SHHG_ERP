using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class StockLogList : ExamListPage
    {
        string sql = "";
        string productId = "";
        string warehouseId = "";
        string operateType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            productId = RequestData.Get<string>("ProductId");
            warehouseId = RequestData.Get<string>("WarehouseId");
            operateType = RequestData.Get<string>("OperateType");
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
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.Value.ToString() != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
            if (string.IsNullOrEmpty(operateType))
            {
                sql = @"select  * from SHHG_AimExamine..StockLog where ProductId='{0}' and WarehouseId='{1}' " + where;
                sql = string.Format(sql, productId, warehouseId, operateType);
            }
            else
            {
                sql = @"select  * from SHHG_AimExamine..StockLog where ProductId='{0}' and WarehouseId='{1}' and OperateType='{2}' " + where;
                sql = string.Format(sql, productId, warehouseId, operateType);
            }
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || search.Orders[0].Ascending ? " desc" : " asc";
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
