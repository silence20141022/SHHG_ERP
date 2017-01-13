using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aim.Data;

namespace Aim.Examining.Web.NewFinance
{
    public partial class BadDebtList : System.Web.UI.Page
    {
        int totalProperty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string where = "";
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["CName"]))
                    {
                        where += " and CName like '%" + Request["CName"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["Number"]))
                    {
                        where += " and Number like '%" + Request["Number"].Trim() + "%'";
                    }
                    string sql = @"select Id,Number,CName,Amount,Remark,PayAmount,InvoiceDate,PayState,CreateTime,(Amount-isnull(PayAmount,0)) as BadDebtAmount from SHHG_AimExamine..OrderInvoice
                    where (Amount-isnull(PayAmount,0))>0 and PayState = '已全部付款' " + where;
                    DataTable dt = DataHelper.QueryDataTable(GetPageSql(sql));
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