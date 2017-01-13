using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aim.Data;
using Aim.Examining.Model;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class InWarehouseEdit_New : System.Web.UI.Page
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
            InWarehouse iwEnt = null;
            IList<InWarehouseDetail> iwdEnts = null;
            PurchaseOrderDetail podEnt = null;
            string PurchaseOrderIds = Request["PurchaseOrderIds"];
            string id = Request["id"];
            if (!string.IsNullOrEmpty(id))
            {
                iwEnt = InWarehouse.Find(id);
            }
            switch (action)
            {
                case "loadwarehouse":
                    sql = "select Id ,Name from SHHG_AimExamine..Warehouse";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    if (!string.IsNullOrEmpty(PurchaseOrderIds))
                    {
                        sql = @"select a.Id as PurchaseOrderDetailId, a.ProductId,a.Code as ProductCode,                                            
                        (a.Quantity-isnull(a.RuKuDanQuan,0)) as IQuantity, (a.Quantity-isnull(a.RuKuDanQuan,0)) as MaxQuan,b.PurchaseOrderNo
                        from SHHG_AimExamine..PurchaseOrderDetail a left join SHHG_AimExamine..PurchaseOrder b on a.PurchaseOrderId=b.Id 
                        where '" + PurchaseOrderIds + "' like '%'+PurchaseOrderId+'%' and  (a.Quantity-isnull(a.RuKuDanQuan,0))>0 order by a.Code asc";
                    }
                    if (!string.IsNullOrEmpty(id))
                    {
                        sql = @"select a.Id,a.PurchaseOrderDetailId, a.ProductId,a.ProductCode,a.IQuantity, a.YiRuQuan, 
                        c.PurchaseOrderNo,a.InWarehouseState, a.Remark,(b.Quantity-isnull(b.RuKuDanQuan,0)+a.IQuantity) as MaxQuan   
                        from SHHG_AimExamine..InWarehouseDetail a left join SHHG_AimExamine..PurchaseOrderDetail b
                        on a.PurchaseOrderDetailId=b.Id left join SHHG_AimExamine..PurchaseOrder c on c.Id=b.PurchaseOrderId  
                        where a.InWarehouseId='" + id + "' order by a.ProductCode asc";
                    }
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadform":
                    if (!string.IsNullOrEmpty(PurchaseOrderIds))
                    {
                        iwEnt = new InWarehouse();
                        iwEnt.InWarehouseNo = DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getInWarehouseNo()").ToString();
                        iwEnt.InWarehouseType = "采购入库";
                        string[] idarray = PurchaseOrderIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        PurchaseOrder poEnt = PurchaseOrder.Find(idarray[0]);
                        iwEnt.SupplierId = poEnt.SupplierId;
                        iwEnt.SupplierName = poEnt.SupplierName;
                        Response.Write("{success:true,data:" + JsonHelper.GetJsonString(iwEnt) + "}");
                    }
                    if (!string.IsNullOrEmpty(id))
                    {
                        Response.Write("{success:true,data:" + JsonHelper.GetJsonString(iwEnt) + "}");
                    }
                    Response.End();
                    break;
                case "create":
                    string formdata = Request["formdata"];
                    string detaildata = Request["detaildata"];
                    iwEnt = JsonHelper.GetObject<InWarehouse>(Request["formdata"]);
                    iwEnt.CreateId = Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID;
                    iwEnt.CreateName = Aim.Portal.Web.WebPortalService.CurrentUserInfo.Name;
                    iwEnt.CreateTime = DateTime.Now;
                    iwEnt.State = "未入库";
                    iwEnt.DoCreate();
                    iwdEnts = JsonHelper.GetObject<IList<InWarehouseDetail>>(Request["detaildata"]);
                    foreach (InWarehouseDetail iwdEnt in iwdEnts)
                    {
                        iwdEnt.InWarehouseId = iwEnt.Id;
                        iwdEnt.InWarehouseState = "未入库";
                        iwdEnt.DoCreate();
                        //创建完入库单明细后更新采购单明细的生成入库单数量
                        sql = "select sum(isnull(IQuantity,0)) from SHHG_AimExamine..InWarehouseDetail where PurchaseOrderDetailId='" + iwdEnt.PurchaseOrderDetailId + "'";
                        podEnt = PurchaseOrderDetail.Find(iwdEnt.PurchaseOrderDetailId);
                        podEnt.RuKuDanQuan = DataHelper.QueryValue<Int32>(sql);
                        podEnt.DoUpdate();
                    }
                    Response.Write("{success:true}");
                    Response.End();
                    break;
                case "update":
                    InWarehouse tempEnt = JsonHelper.GetObject<InWarehouse>(Request["formdata"]);
                    EasyDictionary dic = JsonHelper.GetObject<EasyDictionary>(Request["formdata"]);
                    iwEnt = DataHelper.MergeData<InWarehouse>(iwEnt, tempEnt, dic.Keys);
                    iwEnt.DoUpdate();
                    sql = "delete SHHG_AimExamine..InWarehouseDetail where InWarehouseId='"+iwEnt.Id+"'";
                    DataHelper.ExecSql(sql);
                    iwdEnts = JsonHelper.GetObject<IList<InWarehouseDetail>>(Request["detaildata"]);
                    foreach (InWarehouseDetail iwdEnt in iwdEnts)
                    {
                        iwdEnt.InWarehouseId = iwEnt.Id;
                        iwdEnt.InWarehouseState = "未入库";
                        iwdEnt.DoCreate();
                        //创建完入库单明细后更新采购单明细的生成入库单数量
                        sql = "select sum(isnull(IQuantity,0)) from SHHG_AimExamine..InWarehouseDetail where PurchaseOrderDetailId='" + iwdEnt.PurchaseOrderDetailId + "'";
                        podEnt = PurchaseOrderDetail.Find(iwdEnt.PurchaseOrderDetailId);
                        podEnt.RuKuDanQuan = DataHelper.QueryValue<Int32>(sql);
                        podEnt.DoUpdate();
                    }
                    break;
            }
        }
    }
}