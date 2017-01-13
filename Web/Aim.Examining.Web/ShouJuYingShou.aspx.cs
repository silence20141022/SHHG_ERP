using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aim.Data;
using Aim.Examining.Model;

namespace Aim.Examining.Web
{
    public partial class ShouJuYingShou : System.Web.UI.Page
    {
        int totalProperty = 0;
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string month = Request["month"];
            DataTable dt = null;
            string where = "";
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["year"]))
                    {
                        where += " and (DATEPART(yyyy, CreateTime) = '" + Request["year"] + "')";
                    }
                    //if (!string.IsNullOrEmpty(Request["SupplierName"]))
                    //{
                    //    where += " and SupplierName like '%" + Request["SupplierName"].Trim() + "%'";
                    //}
                    //if (!string.IsNullOrEmpty(Request["InWarehouseState"]))
                    //{
                    //    where += " and InWarehouseState = '" + Request["InWarehouseState"] + "'";
                    //}
                    //if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    //{
                    //    where += " and Id in (select distinct PurchaseOrderId from SHHG_AimExamine..PurchaseOrderDetail where Code like '%" + Request["ProductCode"] + "%')";
                    //}
                    sql = @"select * from  SHHG_AimExamine..SaleOrders                                                
                          where InvoiceType='收据' and  State is null and 
                          DeState='已全部出库' and (TotalMoney-isnull(ReceiptAmount,0)-isnull(ReturnAmount,0)-isnull(DiscountAmount,0))> 0" + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
            }
        }
        private string GetPageSql(string tempsql)
        {
            int start = Convert.ToInt32(Request["start"]);
            int limit = Convert.ToInt32(Request["limit"]);
            totalProperty = DataHelper.QueryValue<int>("select count(1) from (" + tempsql + ") t");
            string order = "CreateTime";
            string asc = " desc";
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, tempsql, start + 1, limit + start);
            return pageSql;
        }
    }
}