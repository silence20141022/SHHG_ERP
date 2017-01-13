using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Newtonsoft.Json.Linq;
using System.Data;
using Aim.Examining.Model;

namespace Aim.Examining.Web
{
    public partial class ProductPrice : System.Web.UI.Page
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
                case "updateprice":
                    string productid = Request["productid"];
                    Product pEnt = Product.Find(productid);
                    pEnt.BuyPrice = Convert.ToDecimal(Request["BuyPrice"]);
                    pEnt.DoUpdate();
                    break;
                case "select":
                    string product_s = Request["Product"];
                    if (!string.IsNullOrEmpty(product_s))
                    {
                        where += " and a.Name like '%" + product_s.Trim() + "%' or a.Code like '%" + product_s.Trim() + "%' or a.Pcn like '%" + product_s.Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["ProductType"]+""))
                    {
                        where += " and a.ProductType='" + Request["ProductType"] + "'";
                    }
                    sql = @"select a.*,b.Symbo from SHHG_AimExamine..Products a 
                          left join SHHG_AimExamine..Supplier b on a.SupplierId=b.Id  where 1=1 " + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + ",total:" + totalProperty + "}");
                    Response.End();
                    break;
                case "loadproducttype":
                    sql = "select name from SysEnumeration where ParentId='cb614448-cea2-4546-acad-1031b863f7d6' order by SortIndex asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
            }
        }
        private string GetPageSql(string tempsql)
        {
            int start = Convert.ToInt32(Request["start"]);
            int limit = Convert.ToInt32(Request["limit"]);
            totalProperty = DataHelper.QueryValue<int>("select count(1) from (" + tempsql + ") t");
            string order = "CreateTime desc";
            //  string order2 = Request["sort"];//[{"property":"MajorName","direction":"ASC"}]
            if (!string.IsNullOrEmpty(Request["sort"]))
            {
                JArray jarray = JsonHelper.GetObject<JArray>(Request["sort"]);
                order = jarray[0].Value<string>("property") + " " + jarray[0].Value<string>("direction");
            }
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0})as RowNumber
		    FROM ({1}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {2} and {3}";
            pageSql = string.Format(pageSql, order, tempsql, start + 1, limit + start);
            return pageSql;
        }
    }
}