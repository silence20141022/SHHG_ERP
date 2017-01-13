using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;
using System.Data;

namespace Aim.Examining.Web.StockManagement
{
    public partial class InWarehouseProductList : ExamListPage
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
            string sql = string.Empty;
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
            }
            string inWarehouseId = RequestData.Get<string>("InWarehouseId");//入库单id有可能为空 比如
            if (!string.IsNullOrEmpty(inWarehouseId))
            {
                InWarehouse iwEnt = InWarehouse.Find(inWarehouseId);
                if (iwEnt.InWarehouseType == "采购入库")
                {
                    sql = @"select A.ProductId,A.IQuantity as Quantity,B.Name,A.InWarehouseState,B.Code
                    from SHHG_AimExamine..InWarehouseDetail as A
                    left join SHHG_AimExamine..Products as B on A.ProductId=B.Id  
                    where InWarehouseId='" + inWarehouseId + "'" + where;
                }
                else
                {
                    sql = @"select A.ProductId,A.Quantity,A.InWarehouseState,B.Name,B.Code
                    from SHHG_AimExamine..OtherInWarehouseDetail as A
                    left join SHHG_AimExamine..Products as B on A.ProductId=B.Id  
                    where InWarehouseId='" + inWarehouseId + "'" + where;
                }
                PageState.Add("InWarehouseDetail", GetPageData(sql, SearchCriterion));
                PageState.Add("InWarehouseNo", iwEnt.InWarehouseNo);
            }
//            else
//            {
//                DataTable tbl = new DataTable();
//                DataColumn col = new DataColumn("ProductId", typeof(string)); tbl.Columns.Add(col);
//                col = new DataColumn("Quantity", typeof(int)); tbl.Columns.Add(col);
//                col = new DataColumn("Name", typeof(string)); tbl.Columns.Add(col);
//                col = new DataColumn("Code", typeof(string)); tbl.Columns.Add(col);
//                col = new DataColumn("Isbn", typeof(string)); tbl.Columns.Add(col);
//                col = new DataColumn("InWarehouseNo", typeof(string)); tbl.Columns.Add(col);
//                col = new DataColumn("InWarehouseState", typeof(string)); tbl.Columns.Add(col);
//                sql = @"select A.ProductId,A.IQuantity as Quantity,A.InWarehouseState,B.Name,B.Code,B.Isbn,C.InWarehouseNo 
//                from SHHG_AimExamine..InWarehouseDetail as A
//                left join SHHG_AimExamine..Products as B on A.ProductId=B.Id 
//                left join SHHG_AimExamine..InWarehouse as C on A.InWarehouseId=C.Id where 1=1 " + where;
//                IList<EasyDictionary> dics1 = DataHelper.QueryDictList(sql);
//                foreach (EasyDictionary ea in dics1)
//                {
//                    DataRow dr = tbl.NewRow();
//                    dr["ProductId"] = ea.Get<string>("ProductId");
//                    dr["Quantity"] = ea.Get<int>("Quantity");
//                    dr["Name"] = ea.Get<string>("Name");
//                    dr["Code"] = ea.Get<string>("Code");
//                    dr["Isbn"] = ea.Get<string>("Isbn");
//                    dr["InWarehouseNo"] = ea.Get<string>("InWarehouseNo");
//                    dr["InWarehouseState"] = ea.Get<string>("InWarehouseState");
//                    tbl.Rows.Add(dr);
//                }
//                sql = @"select A.ProductId,A.Quantity,A.InWarehouseState,B.Name,B.Code,B.Isbn,C.InWarehouseNo 
//                from SHHG_AimExamine..OtherInWarehouseDetail as A
//                left join SHHG_AimExamine..Products as B on A.ProductId=B.Id 
//                left join SHHG_AimExamine..InWarehouse as C on A.InWarehouseId=C.Id where 1=1 " + where;
//                IList<EasyDictionary> dics2 = DataHelper.QueryDictList(sql);
//                foreach (EasyDictionary ea in dics2)
//                {
//                    DataRow dr = tbl.NewRow();
//                    dr["ProductId"] = ea.Get<string>("ProductId");
//                    dr["Quantity"] = ea.Get<int>("Quantity");
//                    dr["Name"] = ea.Get<string>("Name");
//                    dr["Code"] = ea.Get<string>("Code");
//                    dr["Isbn"] = ea.Get<string>("Isbn");
//                    dr["InWarehouseNo"] = ea.Get<string>("InWarehouseNo");
//                    dr["InWarehouseState"] = ea.Get<string>("InWarehouseState");
//                    tbl.Rows.Add(dr);
//                }
//                PageState.Add("InWarehouseDetail", tbl);
//            }
        }
        //sql 分页
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Code";
            string asc = search.Orders.Count <= 0 || search.Orders[0].Ascending ? " asc" : " desc";
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
