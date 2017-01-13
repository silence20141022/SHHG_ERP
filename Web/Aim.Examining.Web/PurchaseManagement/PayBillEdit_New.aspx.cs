using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aim.Examining.Model;
using Aim.Data;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PayBillEdit_New : System.Web.UI.Page
    {
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
            string sql = "";
            DataTable dt = null;
            PayBill pbEnt = null;
            IList<PayBillDetail> pbdEnts = null;
            InWarehouseDetail iwdEnt = null;
            string id = Request["id"];
            string inwarehouseids = Request["inwarehouseids"];
            if (!string.IsNullOrEmpty(id))
            {
                pbEnt = PayBill.Find(id);
            }
            switch (action)
            {
                case "loaddetail":
                    if (!string.IsNullOrEmpty(id))
                    {
                        sql = @"select a.Id,a.PurchaseOrderDetailId, a.ProductId,a.ProductCode,a.PayQuantity,a.BuyPrice,a.Amount,a.ProductName,
                        c.PurchaseOrderNo,(d.IQuantity-isnull(d.FuKuanDanQuan,0)+a.PayQuantity) as MaxQuan,a.InWarehouseDetailId  
                        from SHHG_AimExamine..PayBillDetail a 
                        left join SHHG_AimExamine..InWarehouseDetail d on d.Id=a.InWarehouseDetailId               
                        left join SHHG_AimExamine..PurchaseOrderDetail b on a.PurchaseOrderDetailId=b.Id                      
                        left join SHHG_AimExamine..PurchaseOrder c on c.Id=b.PurchaseOrderId  
                        where a.PayBillId='" + id + "' order by a.ProductCode asc";
                    }
                    if (!string.IsNullOrEmpty(inwarehouseids))
                    {
                        sql = @"select a.Id InWarehouseDetailId, a.ProductId,a.PurchaseOrderDetailId, a.ProductCode,b.Name as ProductName, b.BuyPrice,c.PurchaseOrderNo,                                        
                        (a.IQuantity-isnull(a.FuKuanDanQuan,0)) as PayQuantity, (a.IQuantity-isnull(a.FuKuanDanQuan,0)) as MaxQuan,(a.IQuantity-isnull(a.FuKuanDanQuan,0))*(b.BuyPrice) as Amount
                        from SHHG_AimExamine..InWarehouseDetail a 
                        left join SHHG_AimExamine..PurchaseOrderDetail b on a.PurchaseOrderDetailId=b.Id 
                        left join SHHG_AimExamine..PurchaseOrder c on c.Id=b.PurchaseOrderId
                        where (a.IQuantity-isnull(a.FuKuanDanQuan,0))>0 and ('" + inwarehouseids + "' like '%'+a.InWarehouseId+'%') order by a.ProductCode asc";
                    }
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadform":
                    if (!string.IsNullOrEmpty(inwarehouseids))
                    {
                        pbEnt = new PayBill();
                        pbEnt.PayBillNo = DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getPayBillNo()").ToString();
                        string[] idarray = inwarehouseids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        InWarehouse iwEnt = InWarehouse.Find(idarray[0]);
                        pbEnt.SupplierId = iwEnt.SupplierId;
                        pbEnt.SupplierName = iwEnt.SupplierName;
                        Supplier sEnt = Supplier.Find(iwEnt.SupplierId);
                        pbEnt.Symbo = sEnt.Symbo;
                        Response.Write("{success:true,data:" + JsonHelper.GetJsonString(pbEnt) + "}");
                    }
                    if (!string.IsNullOrEmpty(id))
                    {
                        Response.Write("{success:true,data:" + JsonHelper.GetJsonString(pbEnt) + "}");
                    }
                    Response.End();
                    break;
                case "create":
                    pbEnt = JsonHelper.GetObject<PayBill>(Request["formdata"]);
                    pbEnt.CreateId = Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID;
                    pbEnt.CreateName = Aim.Portal.Web.WebPortalService.CurrentUserInfo.Name;
                    pbEnt.CreateTime = DateTime.Now;
                    pbEnt.State = "未付款";
                    pbEnt.DoCreate();
                    pbdEnts = JsonHelper.GetObject<IList<PayBillDetail>>(Request["detaildata"]);
                    foreach (PayBillDetail pbdEnt in pbdEnts)
                    {
                        pbdEnt.PayBillId = pbEnt.Id;
                        pbdEnt.DoCreate();
                        //创建完付款单明细后更新入库单明细的生成付款单数量
                        sql = "select sum(isnull(PayQuantity,0)) from SHHG_AimExamine..PayBillDetail where InWarehouseDetailId='" + pbdEnt.InWarehouseDetailId + "'";
                        iwdEnt = InWarehouseDetail.Find(pbdEnt.InWarehouseDetailId);
                        iwdEnt.FuKuanDanQuan = DataHelper.QueryValue<Int32>(sql);
                        iwdEnt.DoUpdate();
                    }
                    Response.Write("{success:true}");
                    Response.End();
                    break;
                case "update":
                    PayBill tempEnt = JsonHelper.GetObject<PayBill>(Request["formdata"]);
                    EasyDictionary dic = JsonHelper.GetObject<EasyDictionary>(Request["formdata"]);
                    pbEnt = DataHelper.MergeData<PayBill>(pbEnt, tempEnt, dic.Keys);
                    pbEnt.DoUpdate();
                    sql = "delete SHHG_AimExamine..PayBillDetail where PayBillId='" + pbEnt.Id + "'";
                    DataHelper.ExecSql(sql);
                    pbdEnts = JsonHelper.GetObject<IList<PayBillDetail>>(Request["detaildata"]);
                    foreach (PayBillDetail pbdEnt in pbdEnts)
                    {
                        pbdEnt.PayBillId = pbEnt.Id;
                        pbdEnt.DoCreate();
                        //创建完付款单明细后更新入库单明细的生成付款单数量
                        sql = "select sum(isnull(PayQuantity,0)) from SHHG_AimExamine..PayBillDetail where InWarehouseDetailId='" + pbdEnt.InWarehouseDetailId + "'";
                        iwdEnt = InWarehouseDetail.Find(pbdEnt.InWarehouseDetailId);
                        iwdEnt.FuKuanDanQuan = DataHelper.QueryValue<Int32>(sql);
                        iwdEnt.DoUpdate();
                    }
                    Response.Write("{success:true}");
                    Response.End();
                    break;
            }
        }
    }
}