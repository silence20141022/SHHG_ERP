using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using System.Data;
using Aim.Portal.Model;
using System.IO;

namespace Aim.Examining.Web.BaseInfo
{
    public partial class FileList : System.Web.UI.Page
    {
        int totalProperty = 0;
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request["action"])
            {
                case "load":
                    sql = @"select * from FileItem";
                    DataTable dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "delete":
                    string id = Request["id"];
                    FileItem fiEnt = FileItem.Find(id);
                    File.Delete(@"D:\RW\Files\AppFiles\Portal\Default\" + fiEnt.Name);
                    fiEnt.DoDelete();
                    Response.Write("{success:true}");
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