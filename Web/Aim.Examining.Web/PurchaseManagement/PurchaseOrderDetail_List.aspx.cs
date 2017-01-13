using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aim.Data;
using Aim.Examining.Model;
using Aspose.Cells;
using Aim.Portal.Model;
namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseOrderDetail_List : System.Web.UI.Page
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
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["PurchaseOrderNo"]))
                    {
                        where += " and b.PurchaseOrderNo like '%" + Request["PurchaseOrderNo"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["SupplierName"]))
                    {
                        where += " and SupplierName like '%" + Request["SupplierName"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["Code"]))
                    {
                        where += " and Code like  '%" + Request["Code"] + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["PCN"]))
                    {
                        where += " and PCN like  '%" + Request["PCN"] + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["StartTime"]))
                    {
                        where += " and CreateTime>='" + Request["StartTime"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["EndTime"]))
                    {
                        where += " and CreateTime<='" + (Request["EndTime"] + "").Replace("00:00:00", "23:59:59") + "'";
                    }
                    sql = @"select a.Name,a.Code,a.PCN,a.BuyPrice,a.Quantity,a.Amount,a.RuKuDanQuan,b.SupplierName,
                          b.PurchaseOrderNo,b.Symbo,b.CreateTime,b.CreateName from SHHG_AimExamine..PurchaseOrderDetail a 
                          left join SHHG_AimExamine..PurchaseOrder b on a.PurchaseOrderId=b.Id where 1=1 " + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadsupplier":
                    string supplierName = Request["SupplierName"];
                    sql = "select * from SHHG_AimExamine..Supplier where SupplierName like '%" + supplierName + "%'";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "exportexcel":
                    if (!string.IsNullOrEmpty(Request["PurchaseOrderNo"]))
                    {
                        where += " and b.PurchaseOrderNo like '%" + Request["PurchaseOrderNo"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["SupplierName"]))
                    {
                        where += " and SupplierName like '%" + Request["SupplierName"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["Code"]))
                    {
                        where += " and Code like  '%" + Request["Code"] + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["PCN"]))
                    {
                        where += " and PCN like  '%" + Request["PCN"] + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["StartTime"]))
                    {
                        where += " and CreateTime>='" + Request["StartTime"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["EndTime"]))
                    {
                        where += " and CreateTime<='" + (Request["EndTime"] + "").Replace("00:00:00", "23:59:59") + "'";
                    }
                    try
                    {
                        Workbook wb = new Workbook();
                        // book.Open(tempfilename);这里我们暂时不用模板  
                        wb.Worksheets.Clear();
                        wb.Worksheets.Add("New Worksheet1");//New Worksheet1是Worksheet的name
                        Worksheet ws = wb.Worksheets[0];
                        sql = @"select a.Name,a.Code,a.BuyPrice,a.Quantity,a.Amount,b.SupplierName,
                        b.PurchaseOrderNo,b.Symbo,b.CreateTime,b.CreateName from SHHG_AimExamine..PurchaseOrderDetail a 
                        left join SHHG_AimExamine..PurchaseOrder b on a.PurchaseOrderId=b.Id where 1=1 " + where + " order by b.CreateTime asc";
                        dt = DataHelper.QueryDataTable(sql);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Cell cell = ws.Cells[i, j];
                                cell.PutValue(dt.Rows[i][j] + ""); //必须用PutValue方法赋值
                                //cell.ForegroundColor = Color.Yellow;
                                //cell.Style.Pattern = BackgroundType.Solid;
                                //cell.Style.Font.Size = 10;
                                //cell.Style.Font.Color = Color.Blue;
                            }
                        }
                        string fileName = DateTime.Now.Ticks + "_采购明细.xls";
                        wb.Save("D:\\RW\\Files\\AppFiles\\Portal\\Default\\" + fileName);
                        sql = @"insert fileitem (id,name,creatorid,creatorname,createtime) values(newid(),'" + fileName + "','"
                            + Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID + "','" + Aim.Portal.Web.WebPortalService.CurrentUserInfo.Name + "','" + DateTime.Now + "')";
                        DataHelper.ExecSql(sql);
                        Response.Write("{success:true}");
                    }
                    catch
                    {
                        Response.Write("{success:false}");
                    }
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