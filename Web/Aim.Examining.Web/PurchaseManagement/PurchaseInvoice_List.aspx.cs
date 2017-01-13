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
    public partial class PurchaseInvoice_List : System.Web.UI.Page
    {
        int totalProperty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string PurchaseInvoiceId = Request["PurchaseInvoiceId"];
            string where = "";
            string sql = "";
            PurchaseInvoice Pient = null;
            DataTable dt;
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["InvoiceNo"]))
                    {
                        where += " and InvoiceNo like '%" + Request["InvoiceNo"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["SupplierName"]))
                    {
                        where += " and SupplierName like '%" + Request["SupplierName"].Trim() + "%'";
                    }

                    if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    {
                        where += " and Id in (select distinct PurchaseInvoiceId from SHHG_AimExamine..PurchaseInvoiceDetail where  ProductCode like '%" + Request["ProductCode"] + "%')";
                    }
                    sql = @"select * from SHHG_AimExamine..PurchaseInvoice where 1=1 " + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;

                case "loaddetail":
                    sql = @"select *  from SHHG_AimExamine..PurchaseInvoiceDetail 
                             where PurchaseInvoiceId='" + PurchaseInvoiceId + "' order by ProductCode asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "delete":
                    string Id = Request["Id"];
                    Pient = PurchaseInvoice.Find(Id);
                    IList<PurchaseInvoiceDetail> PidEnts = PurchaseInvoiceDetail.FindAllByProperty("PurchaseInvoiceId", Id);
                    foreach (PurchaseInvoiceDetail PidEnt in PidEnts)
                    {
                        sql = @"select sum(isnull(InvoiceQuantity,0)) from SHHG_AimExamine..PurchaseInvoiceDetail where InWarehouseDetailId='" + PidEnt.InWarehouseDetailId + "'";
                        PidEnt.DoDelete();
                        if (!string.IsNullOrEmpty(PidEnt.InWarehouseDetailId))
                        {
                            InWarehouseDetail iwdEnt = InWarehouseDetail.Find(PidEnt.InWarehouseDetailId);
                            iwdEnt.KaiPiaoQuan = DataHelper.QueryValue<Int32>(sql);
                            iwdEnt.DoUpdate();
                        }
                    }
                    Pient.DoDelete();
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