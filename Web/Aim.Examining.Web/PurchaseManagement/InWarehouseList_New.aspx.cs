using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using System.Data;
using Aim.Examining.Model;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class InWarehouseList_New : System.Web.UI.Page
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
            string where = "";
            DataTable dt = null;
            string id = Request["id"];
            InWarehouse iwEnt = null;
            IList<InWarehouseDetail> iwdEnts = null;
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["InWarehouseNo"]))
                    {
                        where += " and InWarehouseNo like '%" + Request["InWarehouseNo"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["SupplierName"]))
                    {
                        where += " and SupplierName like '%" + Request["SupplierName"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["State"]))
                    {
                        where += " and State = '" + Request["State"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    {
                        where += " and Id in (select distinct InWarehouseId from SHHG_AimExamine..InWarehouseDetail where  ProductCode like '%" + Request["ProductCode"] + "%')";
                    }
                    sql = "select * from SHHG_AimExamine..InWarehouse where InWarehouseType ='采购入库' " + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;  
                case "delete":
                    if (!string.IsNullOrEmpty(id))
                    {
                        iwEnt = InWarehouse.Find(id);
                        iwdEnts = InWarehouseDetail.FindAllByProperty("InWarehouseId", id);
                        foreach (InWarehouseDetail tempEnt in iwdEnts)
                        {
                            sql = "select sum(isnull(IQuantity,0)) from SHHG_AimExamine..InWarehouseDetail where PurchaseOrderDetailId='" + tempEnt.PurchaseOrderDetailId + "'";
                            tempEnt.DoDelete();
                           //删除完入库单明细后更新采购单明细的生成入库单数量
                            PurchaseOrderDetail podEnt = PurchaseOrderDetail.Find(tempEnt.PurchaseOrderDetailId);
                            podEnt.RuKuDanQuan = DataHelper.QueryValue<Int32>(sql);
                            podEnt.DoUpdate();
                        }
                        iwEnt.DoDelete();
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