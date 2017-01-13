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
    public partial class FaPiaoYingShou : System.Web.UI.Page
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
                        where += " and (DATEPART(yyyy, A.CreateTime) = '" + Request["year"] + "')";
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
                    sql = @"select A.*,B.MagId,B.MagUser from SHHG_AimExamine..OrderInvoice A                    
                    left join SHHG_AimExamine..Customers B on B.Id=A.CId
                    where (A.PayState is null or A.PayState<>'已全部付款') and B.MagId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                    and A.Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8') " + where;
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