using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;
using System.Collections;
using System.Data;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseOrderDetailList : ExamListPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = RequestData.Get<string>("id");
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
            string productType = RequestData.Get<string>("ProductType");
            if (!string.IsNullOrEmpty(productType))
            {
                SearchCriterion.AddSearch("ProductType", productType);
            }
            string sql = string.Empty;
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.Value.ToString() != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
            sql = @"select A.Id,A.PurchaseOrderId,C.PurchaseOrderNo,A.BuyPrice,A.Quantity,A.Amount,A.PayState,A.InWarehouseState,
                A.InvoiceState,B.ProductType,B.Name,B.Code,B.Pcn,C.PriceType,C.SupplierName,C.CreateName,C.CreateTime,D.Symbo
                from (Select Id,PurchaseOrderId,BuyPrice,Quantity,Amount,
                PayState,InWarehouseState,InvoiceState,ProductId from SHHG_AimExamine..PurchaseOrderDetail) as A 
                left join (select Id,Name,Code,ProductType,Isbn,Pcn from SHHG_AimExamine..Products) as B on A.ProductId=B.Id
                left join (Select Id,PurchaseOrderNo,PriceType,SupplierId,SupplierName,CreateName,CreateTime from SHHG_AimExamine..PurchaseOrder) as C on A.PurchaseOrderId=C.Id
                left join (Select Id,Symbo from SHHG_AimExamine..Supplier) as D on D.Id=C.SupplierId
                where 1=1" + where;
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        //sql 分页
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "PurchaseOrderNo";
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
