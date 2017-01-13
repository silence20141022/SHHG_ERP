using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using System.Data;

namespace Aim.Examining.Web.NewFinance
{
    public partial class UnInvoiceOrderList : System.Web.UI.Page
    {
        int totalProperty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string where = "";
            string sql = "";
            DataTable dt = null;
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["Number"]))
                    {
                        where += " and Number like '%" + Request["Number"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["CName"]))
                    {
                        where += " and CName like '%" + Request["CName"].Trim() + "%'";
                    } 
                    sql = @"select * from  SHHG_AimExamine..SaleOrders t where InvoiceType='发票' and  State='暂缓开票' and 
                            DeState='已全部出库' and (InvoiceState is null or InvoiceState<>'已全部开发票') and (ISNULL(TotalMoney, 0) > ISNULL(ReturnAmount, 0)) " + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    sql = @"select * from  SHHG_AimExamine..OrdersPart  where oid='" + Request["id"] + "'";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "rtninvoice":
                    sql = @"update SHHG_AimExamine..SaleOrders set State=null  where id='" + Request["id"] + "'";
                    DataHelper.ExecSql(sql);
                    break;
            }
        }
        private string GetPageSql(string tempsql)
        {
            int start = Convert.ToInt32(Request["start"]);
            int limit = Convert.ToInt32(Request["limit"]);
            totalProperty = DataHelper.QueryValue<int>("select count(1) from (" + tempsql + ") t");
            string order = "CREATETIME";
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