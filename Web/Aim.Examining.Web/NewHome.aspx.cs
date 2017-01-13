using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aim.Data;

namespace Aim.Examining.Web
{
    public partial class NewHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string sql = "";
            string where = "";
            string year = "";
            DataTable dt = null;
            DataTable temp_Tabel = null;
            DataColumn dc = null;
            IList<EasyDictionary> dics = null;
            try
            {
                Aim.Portal.Web.WebPortalService.CheckLogon();
            }
            catch
            {
                Response.Write("<script> window.location.href = '/Login.aspx';</script>");
                Response.End();
            }
            switch (action)
            {
                case "loadyingshou":
                    if (!string.IsNullOrEmpty(Request["CustomerId"]))
                    {
                        where += " and CId='" + Request["CustomerId"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["UserId"]))
                    {
                        where += " and B.MagId='" + Request["UserId"] + "'";
                    }
                    temp_Tabel = new DataTable();
                    dc = new DataColumn("year");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("daikaipiao");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("fapiaoyingshou");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("shoujuyingshou");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("total");
                    temp_Tabel.Columns.Add(dc);
                    sql = "select Name from SysEnumeration where ParentId='058fbee9-0a9a-4b25-b343-ea8c05396632' order by SortIndex asc";
                    dics = DataHelper.QueryDictList(sql);
                    foreach (EasyDictionary dic in dics)
                    {
                        DataRow dr = temp_Tabel.NewRow();
                        dr["year"] = dic.Get<string>("Name");
                        //待开票 不包含张勇和各分公司
                        sql = @"select sum(TotalMoney)  from  SHHG_AimExamine..SaleOrders t
                              left join SHHG_AimExamine..Customers B on B.Id=t.CId  where t.InvoiceType='发票' and  t.State is null and 
                              t.DeState='已全部出库' and (t.InvoiceState is null or t.InvoiceState<>'已全部开发票') and 
                              t.SalesmanId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' and (ISNULL(t.TotalMoney, 0) > ISNULL(t.ReturnAmount, 0)) and 
                              t.CId not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')
                               and DATEPART(yyyy, t.CreateTime)='" + dic.Get<string>("Name") + "'" + where;
                        //发票应收
                        decimal daikaipiao = DataHelper.QueryValue<decimal>(sql);
                        dr["daikaipiao"] = daikaipiao;
                        sql = @"select sum(Amount- ISNULL(A.PayAmount, 0))   from SHHG_AimExamine..OrderInvoice A                    
                        left join SHHG_AimExamine..Customers B on B.Id=A.CId 
                        where (A.PayState is null or A.PayState<>'已全部付款') and B.MagId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                        and A.Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8') and DATEPART(yyyy, A.CreateTime)='" + dic.Get<string>("Name") + "'" + where;
                        decimal fapiaoyingshou = DataHelper.QueryValue<decimal>(sql);
                        dr["fapiaoyingshou"] = fapiaoyingshou;
                        //收据应收
                        sql = @"select sum(TotalMoney) from  SHHG_AimExamine..SaleOrders t 
                        left join SHHG_AimExamine..Customers B on B.Id=t.CId                         
                        where t.InvoiceType='收据' and  t.State is null and 
                        t.DeState='已全部出库' and (t.TotalMoney-isnull(t.ReceiptAmount,0)-isnull(t.ReturnAmount,0)-isnull(t.DiscountAmount,0))> 0
                        and DATEPART(yyyy, t.CreateTime)='" + dic.Get<string>("Name") + "'" + where;
                        decimal shoujuyingshou = DataHelper.QueryValue<decimal>(sql);
                        dr["shoujuyingshou"] = shoujuyingshou;
                        //合计
                        dr["total"] = daikaipiao + fapiaoyingshou + shoujuyingshou;
                        temp_Tabel.Rows.Add(dr);
                    }
                    Response.Write("{success:true,data:" + JsonHelper.GetJsonStringFromDataTable(temp_Tabel) + "}");
                    Response.End();
                    break;
                case "loadyingshou_fengongsi":
                    if (!string.IsNullOrEmpty(Request["CustomerId"]))
                    {
                        where += " and CId='" + Request["CustomerId"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["UserId"]))
                    {
                        where += " and B.MagId='" + Request["UserId"] + "'";
                    }
                    temp_Tabel = new DataTable();
                    dc = new DataColumn("year");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("daikaipiao");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("fapiaoyingshou");
                    temp_Tabel.Columns.Add(dc);
                    sql = "select Name from SysEnumeration where ParentId='058fbee9-0a9a-4b25-b343-ea8c05396632' order by SortIndex asc";
                    dics = DataHelper.QueryDictList(sql);
                    foreach (EasyDictionary dic in dics)
                    {
                        DataRow dr = temp_Tabel.NewRow();
                        dr["year"] = dic.Get<string>("Name");
                        //待开票包含张勇和各分公司
                        sql = @"select sum(TotalMoney)  from  SHHG_AimExamine..SaleOrders t
                              left join SHHG_AimExamine..Customers B on B.Id=t.CId  where t.InvoiceType='发票' and  t.State is null and 
                              t.DeState='已全部出库' and (t.InvoiceState is null or t.InvoiceState<>'已全部开发票')  
                              and (ISNULL(t.TotalMoney, 0) > ISNULL(t.ReturnAmount, 0)) and  (t.SalesmanId ='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' or
                              t.CId in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8'))
                               and DATEPART(yyyy, t.CreateTime)='" + dic.Get<string>("Name") + "'" + where;
                        //发票应收
                        dr["daikaipiao"] = DataHelper.QueryValue(sql);
                        sql = @"select sum(Amount- ISNULL(A.PayAmount, 0))   from SHHG_AimExamine..OrderInvoice A                    
                        left join SHHG_AimExamine..Customers B on B.Id=A.CId 
                        where (A.PayState is null or A.PayState<>'已全部付款') and (B.MagId ='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                        or A.Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')) and DATEPART(yyyy, A.CreateTime)='" + dic.Get<string>("Name") + "'" + where;
                        dr["fapiaoyingshou"] = DataHelper.QueryValue(sql);
                        temp_Tabel.Rows.Add(dr);
                    }
                    Response.Write("{success:true,data:" + JsonHelper.GetJsonStringFromDataTable(temp_Tabel) + "}");
                    Response.End();
                    break;
                case "loadweiruku":
                    if (!string.IsNullOrEmpty(Request["SupplierId"]))
                    {
                        where += " and SupplierId='" + Request["SupplierId"] + "'";
                    }
                    sql = @"select * from SHHG_AimExamine..InWarehouse where State='未入库'" + where + " order by CreateTime asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{success:true,data:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadsupplier":
                    string supplierName = Request["SupplierName"];
                    sql = "select Id as SupplierId,SupplierName from SHHG_AimExamine..Supplier where SupplierName like '%" + supplierName + "%'";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadcustomer":
                    string customerName = Request["CustomerName"];
                    sql = "select Id as CustomerId,Name as CustomerName from SHHG_AimExamine..Customers where Name like '%" + customerName + "%'";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadfuzeren":
                    string userName = Request["UserName"];
                    sql = "select UserID as UserId ,Name as UserName from SysUser where Name like '%" + userName + "%'";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadtask":
                    sql = "select ID as Id,Title,CreatedTime from Task where Status=0 and OwnerId='" + Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID + "' order by CreatedTime desc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    if (!string.IsNullOrEmpty(Request["CustomerId"]))
                    {
                        where += " and CId='" + Request["CustomerId"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["UserId"]))
                    {
                        where += " and B.MagId='" + Request["UserId"] + "'";
                    }
                    year = Request["year"];
                    temp_Tabel = new DataTable();
                    dc = new DataColumn("month");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("daikaipiao");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("fapiaoyingshou");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("shoujuyingshou");
                    temp_Tabel.Columns.Add(dc);
                    sql = "select Value from SysEnumeration where ParentId='b25e537b-34e3-4437-87af-692e00facd73' order by SortIndex asc";
                    dics = DataHelper.QueryDictList(sql);
                    foreach (EasyDictionary dic in dics)
                    {
                        DataRow dr = temp_Tabel.NewRow();
                        dr["month"] = dic.Get<string>("Value");
                        //待开票 不包含张勇和各分公司
                        sql = @"select sum(TotalMoney)  from  SHHG_AimExamine..SaleOrders t
                              left join SHHG_AimExamine..Customers B on B.Id=t.CId  where t.InvoiceType='发票' and  t.State is null and 
                              t.DeState='已全部出库' and (t.InvoiceState is null or t.InvoiceState<>'已全部开发票') and 
                              t.SalesmanId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' and (ISNULL(t.TotalMoney, 0) > ISNULL(t.ReturnAmount, 0)) and 
                              t.CId not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')
                               and DATEPART(m, t.CreateTime)='" + dic.Get<string>("Value") + "' and DatePart(yyyy,t.CreateTime)='" + year + "' " + where;
                        //发票应收
                        dr["daikaipiao"] = DataHelper.QueryValue(sql);
                        sql = @"select sum(Amount- ISNULL(A.PayAmount, 0))   from SHHG_AimExamine..OrderInvoice A                    
                        left join SHHG_AimExamine..Customers B on B.Id=A.CId 
                        where (A.PayState is null or A.PayState<>'已全部付款') and B.MagId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                        and A.Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8') 
                        and DATEPART(yyyy, A.CreateTime)='" + year + "' and DatePart(m,A.CreateTime)='" + dic.Get<string>("Value") + "'" + where;
                        dr["fapiaoyingshou"] = DataHelper.QueryValue(sql);
                        //收据应收
                        sql = @"select sum(TotalMoney) from  SHHG_AimExamine..SaleOrders t 
                        left join SHHG_AimExamine..Customers B on B.Id=t.CId                         
                        where t.InvoiceType='收据' and  t.State is null and 
                        t.DeState='已全部出库' and (t.TotalMoney-isnull(t.ReceiptAmount,0)-isnull(t.ReturnAmount,0)-isnull(t.DiscountAmount,0))> 0
                        and DATEPART(yyyy, t.CreateTime)='" + year + "' and DatePart(m,t.CreateTime)='" + dic.Get<string>("Value") + "'" + where;
                        dr["shoujuyingshou"] = DataHelper.QueryValue(sql);
                        temp_Tabel.Rows.Add(dr);
                    }
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(temp_Tabel) + "}");
                    Response.End();
                    break;
                case "loaddetail_f":
                    if (!string.IsNullOrEmpty(Request["CustomerId"]))
                    {
                        where += " and CId='" + Request["CustomerId"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["UserId"]))
                    {
                        where += " and B.MagId='" + Request["UserId"] + "'";
                    }
                    year = Request["year"];
                    temp_Tabel = new DataTable();
                    dc = new DataColumn("month");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("daikaipiao");
                    temp_Tabel.Columns.Add(dc);
                    dc = new DataColumn("fapiaoyingshou");
                    temp_Tabel.Columns.Add(dc);
                    sql = "select Value from SysEnumeration where ParentId='b25e537b-34e3-4437-87af-692e00facd73' order by SortIndex asc";
                    dics = DataHelper.QueryDictList(sql);
                    foreach (EasyDictionary dic in dics)
                    {
                        DataRow dr = temp_Tabel.NewRow();
                        dr["month"] = dic.Get<string>("Value");
                        //待开票包含张勇和各分公司
                        sql = @"select sum(TotalMoney)  from  SHHG_AimExamine..SaleOrders t
                              left join SHHG_AimExamine..Customers B on B.Id=t.CId  where t.InvoiceType='发票' and  t.State is null and 
                              t.DeState='已全部出库' and (t.InvoiceState is null or t.InvoiceState<>'已全部开发票') 
                             and (ISNULL(t.TotalMoney, 0) > ISNULL(t.ReturnAmount, 0)) and  (t.SalesmanId ='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' or
                              t.CId in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8'))
                               and DATEPART(m, t.CreateTime)='" + dic.Get<string>("Value") + "' and DatePart(yyyy,t.CreateTime)='" + year + "' " + where;
                        //发票应收
                        dr["daikaipiao"] = DataHelper.QueryValue(sql);
                        sql = @"select sum(Amount- ISNULL(A.PayAmount, 0))   from SHHG_AimExamine..OrderInvoice A                    
                        left join SHHG_AimExamine..Customers B on B.Id=A.CId 
                        where (A.PayState is null or A.PayState<>'已全部付款') and (B.MagId ='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                        or A.Cid in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')) 
                        and DATEPART(yyyy, A.CreateTime)='" + year + "' and DatePart(m,A.CreateTime)='" + dic.Get<string>("Value") + "'" + where;
                        dr["fapiaoyingshou"] = DataHelper.QueryValue(sql);
                        temp_Tabel.Rows.Add(dr);
                    }
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(temp_Tabel) + "}");
                    Response.End();
                    break;
            }
        }
    }
}