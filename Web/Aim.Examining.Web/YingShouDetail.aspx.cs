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
    public partial class YingShouDetail : System.Web.UI.Page
    {
        int totalProperty = 0;
        string sql = "";
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
            string action = Request["action"];
            DataTable dt = null;
            string where = "";
            string id = Request["id"];
            PurchaseOrder poEnt = null;
            if (!string.IsNullOrEmpty(id))
            {
                poEnt = PurchaseOrder.Find(id);
            }
            IList<PurchaseOrderDetail> podEnts = null;
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["PurchaseOrderNo"]))
                    {
                        where += " and PurchaseOrderNo like '%" + Request["PurchaseOrderNo"].Trim() + "%'";
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
                    sql = @"select *,(select sum(b.RuKuDanQuan) from SHHG_AimExamine..PurchaseOrderDetail b where b.PurchaseOrderId=PurchaseOrder.Id) as RuKuDanQuan,
                          (select sum(b.Quantity) from SHHG_AimExamine..PurchaseOrderDetail b where b.PurchaseOrderId=PurchaseOrder.Id) as DetailQuan
                          from SHHG_AimExamine..PurchaseOrder where GoodsResource='GCPJ' " + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    sql = "select * from SHHG_AimExamine..PurchaseOrderDetail where PurchaseOrderId='" + id + "' order by Code asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadsupplier":
                    string supplierName = Request["SupplierName"];
                    sql = "select * from SHHG_AimExamine..Supplier where SupplierName like '%" + supplierName + "%'";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadproduct":
                    if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    {
                        where += " and Code like '%" + Request["ProductCode"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["ProductPcn"]))
                    {
                        where += " and Pcn like '%" + Request["ProductPcn"].Trim() + "%'";
                    }
                    sql = @"select top 25 a.Id as ProductId,Name,Code,Pcn as PCN,SalePrice as BuyPrice,
                         (select sum(StockQuantity) from SHHG_AimExamine..StockInfo where ProductId=a.Id ) as StockQuantity 
                         from SHHG_AimExamine..Products a  where ProductType='配件' " + where;
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "create":
                    string formdata = Request["formdata"];
                    string detaildata = Request["detaildata"];
                    poEnt = JsonHelper.GetObject<PurchaseOrder>(Request["formdata"]);
                    poEnt.CreateId = Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID;
                    poEnt.CreateName = Aim.Portal.Web.WebPortalService.CurrentUserInfo.Name;
                    poEnt.CreateTime = DateTime.Now;
                    poEnt.GoodsResource = "GCPJ";
                    poEnt.ProductType = "配件";
                    poEnt.OrderState = "未结束";
                    poEnt.PayState = "未付款";
                    poEnt.InWarehouseState = "未入库";
                    poEnt.InvoiceState = "未关联";
                    poEnt.DoCreate();
                    podEnts = JsonHelper.GetObject<IList<PurchaseOrderDetail>>(Request["detaildata"]);
                    foreach (PurchaseOrderDetail podEnt in podEnts)
                    {
                        podEnt.PurchaseOrderId = poEnt.Id;
                        podEnt.InWarehouseState = "未入库";
                        podEnt.DoCreate();
                    }
                    Response.Write("{success:true}");
                    Response.End();
                    break;
                case "loadform":
                    Response.Write("{success:true,data:" + JsonHelper.GetJsonString(poEnt) + "}");
                    Response.End();
                    break;
                case "update":
                    PurchaseOrder newEnt = JsonHelper.GetObject<PurchaseOrder>(Request["formdata"]);
                    EasyDictionary dic = JsonHelper.GetObject<EasyDictionary>(Request["formdata"]);
                    poEnt = DataHelper.MergeData<PurchaseOrder>(poEnt, newEnt, dic.Keys);
                    poEnt.DoUpdate();
                    sql = "delete from SHHG_AimExamine..PurchaseOrderDetail where PurchaseOrderId='" + id + "'";
                    DataHelper.ExecSql(sql);
                    podEnts = JsonHelper.GetObject<IList<PurchaseOrderDetail>>(Request["detaildata"]);
                    foreach (PurchaseOrderDetail podEnt in podEnts)
                    {
                        podEnt.PurchaseOrderId = poEnt.Id;
                        podEnt.InWarehouseState = "未入库";
                        podEnt.DoCreate();
                    }
                    Response.Write("{success:true}");
                    Response.End();
                    break;
                case "delete":
                    PurchaseOrderDetail.DeleteAll("PurchaseOrderId='" + id + "'");//删除采购订单详细
                    poEnt.DoDelete();
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