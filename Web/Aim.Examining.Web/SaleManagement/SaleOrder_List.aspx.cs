using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aim.Data;
using Aim.Examining.Model;
using Aim.Portal.Model;

namespace Aim.Examining.Web.SaleManagement
{
    public partial class SaleOrder_List : System.Web.UI.Page
    {
        int totalProperty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Aim.Portal.Web.WebPortalService.CheckLogon();
            }
            catch
            {
                Response.Write("<script> window.location.href = '/Login.aspx';</script>");
                Response.End();
            }
            string sql = "";
            string where = "";
            DataTable dt;
            string action = Request["action"];
            string id = Request["id"];
            switch (action)
            {
                case "load":
                    string Number = Request["Number"];
                    if (!string.IsNullOrEmpty(Number))
                    {
                        where += " and number like '%" + Number + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    {
                        where += " and Id in (select distinct OId from SHHG_AimExamine..OrdersPart where PCode like '%" + Request["ProductCode"] + "%')";
                    }
                    sql = @"select * from SHHG_AimExamine..SaleOrders where    
                          CId  in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')" + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + ",total:" + totalProperty + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    sql = @"select a.* from SHHG_AimExamine..OrdersPart a  
                          left join SHHG_AimExamine..SaleOrders b on  b.Id=a.OId  where a.OId= '" + id + "'";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "delete":
                    IList<OrderDetail_FenGongSi> odEnts = OrderDetail_FenGongSi.FindAllByProperty(OrderDetail_FenGongSi.Prop_Order_FenGongSi_Id, id);
                    foreach (OrderDetail_FenGongSi odEnt in odEnts)
                    {
                        sql = "select isnull(sum(Quantity),0) from SHHG_AimExamine..OrderDetail_FenGongSi where OrderPart_Id='" + odEnt.OrderPart_Id + "'";
                        OrdersPart opEnt = OrdersPart.Find(odEnt.OrderPart_Id);
                        opEnt.SaleQuan = DataHelper.QueryValue<Int32>(sql);
                        opEnt.DoUpdate();
                        odEnt.Delete();
                    }
                    Order_FenGongSi oEnt = Order_FenGongSi.Find(id);
                    oEnt.DoDelete();
                    Response.Write("{success:true}");
                    Response.End();
                    break;
                case "loadsecond":
                    sql = @"select a.*,b.CustomerName,b.CreateTime from SHHG_AimExamine..OrderDetail_FenGongSi a  
                          left join SHHG_AimExamine..Order_FenGongSi b on  b.Id=a.Order_FenGongSi_Id  where a.OrderPart_Id= '" + Request["orderpartid"] + "' order by b.CreateTime asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
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