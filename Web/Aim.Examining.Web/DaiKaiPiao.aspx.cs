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
    public partial class DaiKaiPiao : System.Web.UI.Page
    {
        int totalProperty = 0;
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string month = Request["month"];
            DataTable dt = null;
            string where = "";
            // string id = Request["id"];
            //PurchaseOrder poEnt = null;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    poEnt = PurchaseOrder.Find(id);
            //}
            // IList<PurchaseOrderDetail> podEnts = null;
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["year"]))
                    {
                        where += " and (DATEPART(yyyy, CreateTime) = '" + Request["year"] + "')";
                    }
                    if (!string.IsNullOrEmpty(Request["SupplierName"]))
                    {
                        where += " and SupplierName like '%" + Request["SupplierName"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["InWarehouseState"]))
                    {
                        where += " and InWarehouseState = '" + Request["InWarehouseState"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    {
                        where += " and Id in (select distinct PurchaseOrderId from SHHG_AimExamine..PurchaseOrderDetail where Code like '%" + Request["ProductCode"] + "%')";
                    }
                    //                  sql = @"select *,(select sum(b.RuKuDanQuan) from SHHG_AimExamine..PurchaseOrderDetail b where b.PurchaseOrderId=PurchaseOrder.Id) as RuKuDanQuan,
                    //                          (select sum(b.Quantity) from SHHG_AimExamine..PurchaseOrderDetail b where b.PurchaseOrderId=PurchaseOrder.Id) as DetailQuan
                    //                          from SHHG_AimExamine..PurchaseOrder where GoodsResource='GCYSJ' " + where;
                    sql = @"select * from  SHHG_AimExamine..SaleOrders t where InvoiceType='发票' and  State is null and 
                                 DeState='已全部出库' and (InvoiceState is null or InvoiceState<>'已全部开发票') and 
                                 SalesmanId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' and (ISNULL(TotalMoney, 0) > ISNULL(ReturnAmount, 0)) and 
                                Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')" + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    //sql = "select * from SHHG_AimExamine..PurchaseOrderDetail where PurchaseOrderId='" + id + "' order by Code asc";
                    //dt = DataHelper.QueryDataTable(sql);
                    //Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    //Response.End();
                    break;
                case "loadsupplier":
                    //string supplierName = Request["SupplierName"];
                    //sql = "select * from SHHG_AimExamine..Supplier where SupplierName like '%" + supplierName + "%'";
                    //dt = DataHelper.QueryDataTable(sql);
                    //Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    //Response.End();
                    break;
                case "loadproduct":
                    //                    if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    //                    {
                    //                        where += " and Code like '%" + Request["ProductCode"].Trim() + "%'";
                    //                    }
                    //                    if (!string.IsNullOrEmpty(Request["ProductPcn"]))
                    //                    {
                    //                        where += " and Pcn like '%" + Request["ProductPcn"].Trim() + "%'";
                    //                    }
                    //                    sql = @"select top 25 a.Id as ProductId,Name,Code,Pcn as PCN,SalePrice as BuyPrice,
                    //                         (select sum(StockQuantity) from SHHG_AimExamine..StockInfo where ProductId=a.Id ) as StockQuantity 
                    //                         from SHHG_AimExamine..Products a  where ProductType='压缩机' " + where;
                    //                    dt = DataHelper.QueryDataTable(sql);
                    //                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    //                    Response.End();
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