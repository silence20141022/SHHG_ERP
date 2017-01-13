using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Aim.Portal;
using Aim.Portal.Model;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class ReturnOrderEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id 
        ReturnOrder ent = null;
        IList<ReturnOrderPart> ropEnts = null;
        SaleOrder soent = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            if (!string.IsNullOrEmpty(id))
            {
                ent = ReturnOrder.Find(id);
            }
            switch (RequestActionString)
            {
                case "update":
                    ent = GetMergedData<ReturnOrder>();
                    ent.DoUpdate();
                    break;
                case "create":
                    ent = this.GetPostedData<ReturnOrder>();
                    ent.Number = DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getReturnOrderNumber()") + ""; //自动生成流水号
                    ent.State = "已生成";
                    ent.DoCreate();
                    soent = SaleOrder.FindAllByProperty("Number", ent.OrderNumber).FirstOrDefault<SaleOrder>();
                    soent.ReturnAmount = (soent.ReturnAmount.HasValue ? soent.ReturnAmount.Value : 0) + ent.ReturnMoney;
                    soent.DoUpdate();//将退货金额反写到销售单
                    if (soent.InvoiceState == "已全部开发票")
                    {
                        ent.IsDiscount = "T";
                        ent.DoUpdate();
                    }
                    InWarehouse inwh = new InWarehouse
                                   {
                                       PublicInterface = ent.Id,//退货单Id
                                       InWarehouseNo = DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getInWarehouseNo()").ToString(),
                                       InWarehouseType = "退货入库",
                                       WarehouseId = ent.WarehouseId,
                                       WarehouseName = ent.WarehouseName,
                                       State = "未入库"
                                   };
                    inwh.DoCreate();
                    ProcessDetail(inwh);
                    break;
                case "GetSaleOrderPart":
                    string orderNumber = RequestData.Get<string>("OrderNumber");
                    soent = SaleOrder.FindAllByProperty("Number", orderNumber).FirstOrDefault<SaleOrder>();
                    PageState.Add("Result", OrdersPart.FindAllByProperty(OrdersPart.Prop_OId, soent.Id));
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            SetFormData(ent);
            PageState.Add("DataList", ReturnOrderPart.FindAllByProperty(ReturnOrderPart.Prop_ReturnOrderId, id));
            PageState.Add("Warehouse", DataHelper.QueryDict("select * from SHHG_AimExamine..Warehouse", "Id", "Name"));
        }
        private void ProcessDetail(InWarehouse iwEnt)
        {
            ropEnts = ReturnOrderPart.FindAllByProperty(ReturnOrderPart.Prop_ReturnOrderId, id);
            foreach (ReturnOrderPart ropEnt in ropEnts)
            {
                ropEnt.DoDelete();
            }
            IList<string> entStrList = RequestData.GetList<string>("data");
            ropEnts = entStrList.Select(tent => JsonHelper.GetObject<ReturnOrderPart>(tent) as ReturnOrderPart).ToList();
            foreach (ReturnOrderPart ropEnt in ropEnts)
            {
                ropEnt.ReturnOrderId = ent.Id;
                ropEnt.DoCreate();
                OrdersPart opEnt = OrdersPart.Find(ropEnt.OrderPartId);//更新销售单明细的退货数量
                opEnt.ReturnCount = (opEnt.ReturnCount.HasValue ? opEnt.ReturnCount : 0) + ropEnt.Count;
                opEnt.DoUpdate();
                OtherInWarehouseDetail oidEnt = new OtherInWarehouseDetail();//创建入库单明细
                oidEnt.InWarehouseId = iwEnt.Id;
                oidEnt.ProductId = ropEnt.ProductId;
                oidEnt.ProductName = ropEnt.ProductName;
                oidEnt.ProductCode = ropEnt.ProductCode;
                oidEnt.ProductISBN = ropEnt.Isbn;
                oidEnt.ProductType = Product.Find(ropEnt.ProductId).ProductType;
                oidEnt.Quantity = ropEnt.Count;
                oidEnt.InWarehouseState = "未入库";
                oidEnt.Remark = ropEnt.Remark;
                oidEnt.DoCreate();
            }
        }
    }
}

