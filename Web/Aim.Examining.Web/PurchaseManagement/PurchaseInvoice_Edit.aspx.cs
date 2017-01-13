using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;
using System.Data;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseInvoice_Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            DataTable dt;
            string sql = "";
            PurchaseInvoice Pient = null;
            //   PurchaseInvoiceDetail Pident = null;
            InWarehouseDetail IwdEnt = null;
            IList<PurchaseInvoiceDetail> Pidents = null;
            string inwarehouseids = Request["inwarehouseids"];
            string id = Request["id"];
            switch (action)
            {
                case "loadform":
                    if (!string.IsNullOrEmpty(inwarehouseids))
                    {
                        Pient = new PurchaseInvoice(); 
                        string[] idarray = inwarehouseids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        InWarehouse iwEnt = InWarehouse.Find(idarray[0]);
                        Pient.SupplierId = iwEnt.SupplierId;
                        Pient.SupplierName = iwEnt.SupplierName;
                        Supplier sEnt = Supplier.Find(iwEnt.SupplierId);
                        Pient.Symbo = sEnt.Symbo;
                        Response.Write("{success:true,data:" + JsonHelper.GetJsonString(Pient) + "}");
                    }
                    if (!string.IsNullOrEmpty(id))
                    {
                        Response.Write("{success:true,data:" + JsonHelper.GetJsonString(PurchaseInvoice.Find(id)) + "}");
                    }
                    Response.End();
                    break;
                case "loaddetail":
                    if (!string.IsNullOrEmpty(id))
                    {
                        sql = @"select a.Id,a.PurchaseOrderDetailId, a.ProductId,a.ProductCode,a.InvoiceQuantity,a.InvoiceAmount,a.ProductName,a.BuyPrice,a.Remark,
                        (d.IQuantity-isnull(d.KaiPiaoQuan,0)+a.InvoiceQuantity) as MaxQuan,a.InWarehouseDetailId  
                        from SHHG_AimExamine..PurchaseInvoiceDetail   a 
                        left join SHHG_AimExamine..InWarehouseDetail d on d.Id=a.InWarehouseDetailId               
                        left join SHHG_AimExamine..PurchaseOrderDetail b on a.PurchaseOrderDetailId=b.Id                      
                        left join SHHG_AimExamine..PurchaseOrder c on c.Id=b.PurchaseOrderId  
                        where a.PurchaseInvoiceId='" + id + "' order by a.ProductCode asc";
                    }
                    if (!string.IsNullOrEmpty(inwarehouseids))
                    {
                        sql = @"select a.Id InWarehouseDetailId, a.ProductId,a.PurchaseOrderDetailId, a.ProductCode,b.Name as ProductName, b.BuyPrice, c.PurchaseOrderNo,                                       
                        (a.IQuantity-isnull(a.KaiPiaoQuan,0)) as InvoiceQuantity, (a.IQuantity-isnull(a.KaiPiaoQuan,0)) as MaxQuan,(a.IQuantity-isnull(a.KaiPiaoQuan,0))*(b.BuyPrice) as InvoiceAmount
                        from SHHG_AimExamine..InWarehouseDetail a 
                        left join SHHG_AimExamine..PurchaseOrderDetail b on a.PurchaseOrderDetailId=b.Id 
                        left join SHHG_AimExamine..PurchaseOrder c on c.Id=b.PurchaseOrderId
                        where (a.IQuantity-isnull(a.KaiPiaoQuan,0))>0 and ('" + inwarehouseids + "' like '%'+a.InWarehouseId+'%') order by a.ProductCode asc";
                    }
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "create":
                    Pient = JsonHelper.GetObject<PurchaseInvoice>(Request["formdata"]);
                    //Pient.CreateId = Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID;
                    //Pient.CreateName = Aim.Portal.Web.WebPortalService.CurrentUserInfo.Name;
                    //Pient.CreateTime = DateTime.Now;
                    //Pient.InvoiceNo
                    Pient.DoCreate();
                    Pidents = JsonHelper.GetObject<IList<PurchaseInvoiceDetail>>(Request["detaildata"]);
                    foreach (PurchaseInvoiceDetail Pident in Pidents)
                    {
                        Pident.PurchaseInvoiceId = Pient.Id;
                        Pident.DoCreate();
                        //创建完付款单明细后更新入库单明细的生成付款单数量
                        sql = "select sum(isnull(InvoiceQuantity,0)) from SHHG_AimExamine..PurchaseInvoiceDetail where InWarehouseDetailId='" + Pident.InWarehouseDetailId + "'";
                        IwdEnt = InWarehouseDetail.Find(Pident.InWarehouseDetailId);
                        IwdEnt.KaiPiaoQuan = DataHelper.QueryValue<Int32>(sql);
                        IwdEnt.DoUpdate();
                    }
                    Response.Write("{success:true}");
                    Response.End();
                    break;
                case "update":
                    Pient = JsonHelper.GetObject<PurchaseInvoice>(Request["formdata"]);
                    PurchaseInvoice oEnt = PurchaseInvoice.Find(Pient.Id);
                    EasyDictionary dic = JsonHelper.GetObject<EasyDictionary>(Request["formdata"]);
                    Pient = DataHelper.MergeData<PurchaseInvoice>(oEnt, Pient, dic.Keys);

                    Pient.DoUpdate();
                    sql = "delete SHHG_AimExamine..PurchaseInvoiceDetail where PurchaseInvoiceId='" + Pient.Id + "'";
                    DataHelper.ExecSql(sql);
                    Pidents = JsonHelper.GetObject<IList<PurchaseInvoiceDetail>>(Request["detaildata"]);
                    foreach (PurchaseInvoiceDetail Pident in Pidents)
                    {
                        Pident.PurchaseInvoiceId = Pient.Id;
                        Pident.DoCreate();
                        //创建完付款单明细后更新入库单明细的生成付款单数量
                        sql = "select sum(isnull(InvoiceQuantity,0)) from SHHG_AimExamine..PurchaseInvoiceDetail where InWarehouseDetailId='" + Pident.InWarehouseDetailId + "'";
                        IwdEnt = InWarehouseDetail.Find(Pident.InWarehouseDetailId);
                        IwdEnt.KaiPiaoQuan = DataHelper.QueryValue<Int32>(sql);
                        IwdEnt.DoUpdate();
                    }
                    Response.Write("{success:true}");
                    Response.End();
                    break;
            }
        }
    }
}