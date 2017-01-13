using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Portal.Model;
using System.Data;
using Aim.Data;
using Aim.Examining.Model;
using System.Configuration;

namespace Aim.Examining.Web.SaleManagement
{
    public partial class FenGongSi_Order_Edit : System.Web.UI.Page
    {
        int totalProperty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            Order_FenGongSi oEnt = null;
            string where = "";
            string sql = "";
            DataTable dt;
            string obj = "";
            string json = "";
            IList<OrderDetail_FenGongSi> odEnts;
            string id = Request["id"];
            if (!string.IsNullOrEmpty(id))
            {
                oEnt = Order_FenGongSi.Find(id);
            }
            switch (action)
            {
                case "loadorderpart":
                    string FenGongSiId = Request["fengongsiid"];
                    string ProductCode = Request["ProductCode"];
                    if (!string.IsNullOrEmpty(ProductCode))
                    {
                        where += " and PCode like '%" + ProductCode + "%'";
                    }
                    sql = @"select a.Id,a.PId,a.PName,a.PCode,a.SalePrice,a.Count,isnull(a.SaleQuan,0) SaleQuan ,a.CreateTime,b.Number from SHHG_AimExamine..orderspart a  
                    left join SHHG_AimExamine..SaleOrders b on a.OId=b.Id where a.Count>isnull(a.SaleQuan,0) and cid='" + FenGongSiId + "'" + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonString(dt) + "}");
                    Response.End();
                    break;
                case "loadinvoicetype":
                    sql = "select name from SHHG_AimPortal..SysEnumeration where ParentID='f6a3af59-85c0-4847-a2ff-aebdeadf1be7' order by SortIndex asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonString(dt) + "}");
                    Response.End();
                    break;
                case "loadpaytype":
                    sql = "select name from SHHG_AimPortal..SysEnumeration where ParentID='10404568-1e4f-4aab-95ff-86eeed6d955e' order by SortIndex asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonString(dt) + "}");
                    Response.End();
                    break;
                case "loadcustomer":
                    if (!string.IsNullOrEmpty(Request["name"]))
                    {
                        where = " and Name like '%" + Request["name"] + "%'";
                    }
                    sql = "select id,name from SHHG_AimExamine..Customers where 1=1 "+where;
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "create":
                    obj = Request["data"];
                    oEnt = JsonHelper.GetObject<Order_FenGongSi>(obj);
                    oEnt.DoCreate();
                    json = Request["json"];
                    odEnts = JsonHelper.GetObject<IList<OrderDetail_FenGongSi>>(json);
                    foreach (OrderDetail_FenGongSi odEnt in odEnts)
                    {
                        odEnt.Order_FenGongSi_Id = oEnt.Id;
                        odEnt.DoCreate();
                        sql = "select sum(Quantity) from SHHG_AimExamine..OrderDetail_FenGongSi where OrderPart_Id='" + odEnt.OrderPart_Id + "'";
                        OrdersPart opEnt = OrdersPart.Find(odEnt.OrderPart_Id);
                        opEnt.SaleQuan = DataHelper.QueryValue<Int32>(sql);
                        opEnt.DoUpdate();
                    }
                    break;
                case "loadform":
                    if (string.IsNullOrEmpty(id))
                    {
                        oEnt = new Order_FenGongSi();
                        oEnt.Number = DataHelper.QueryValue<string>("select SHHG_AimExamine.dbo.fun_getOrderNumber()");
                        SysUser suEnt = SysUser.Find(Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID);
                        Customer cEnt = Customer.Find(suEnt.LastLogIP);
                        oEnt.FenGongSiId = cEnt.Id;
                        oEnt.FenGongSiName = cEnt.Name;
                    }
                    Response.Write("{success: true  ,data:" + JsonHelper.GetJsonString(oEnt) + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    if (!string.IsNullOrEmpty(id))
                    {
                        sql = @"select a.Id,a.ProductId,a.OrderPart_Id,a.Name,a.Code,a.PurchasePrice,a.SecondPrice,
                        a.Quantity,a.Amount,a.Remark,(b.Count-isnull(b.SaleQuan,0)+a.Quantity) as MaxQuan
                        from SHHG_AimExamine..Orderdetail_FenGongSi a 
                        left join SHHG_AimExamine..OrdersPart b on a.OrderPart_Id=b.Id where a.Order_FenGongSi_Id='" + id + "'";
                        dt = DataHelper.QueryDataTable(sql);
                        Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                        Response.End();
                    }
                    break;
                case "update":
                    obj = Request["data"];
                    Order_FenGongSi tempEnt = JsonHelper.GetObject<Order_FenGongSi>(obj);
                    EasyDictionary dic = JsonHelper.GetObject<EasyDictionary>(obj);
                    oEnt = DataHelper.MergeData<Order_FenGongSi>(oEnt, tempEnt, dic.Keys);
                    oEnt.DoUpdate();
                    sql = "delete from SHHG_AimExamine..Orderdetail_FenGongSi where Order_FenGongSi_Id='" + id + "'";
                    DataHelper.ExecSql(sql);
                    odEnts = JsonHelper.GetObject<IList<OrderDetail_FenGongSi>>(Request["json"]);
                    foreach (OrderDetail_FenGongSi odEnt in odEnts)
                    {
                        odEnt.Order_FenGongSi_Id = oEnt.Id;
                        odEnt.DoCreate();
                        OrdersPart opEnt = OrdersPart.Find(odEnt.OrderPart_Id);
                        sql = "select isnull(sum(Quantity),0) from SHHG_AimExamine..Orderdetail_FenGongSi where OrderPart_Id='" + odEnt.OrderPart_Id + "'";
                        opEnt.SaleQuan = DataHelper.QueryValue<Int32>(sql);
                        opEnt.DoUpdate();
                    }
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