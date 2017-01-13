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
using Castle.ActiveRecord;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Data.OleDb;
using System.Data;
namespace Aim.Examining.Web.StockManagement
{
    public partial class WarehouseExchangeEdit : ExamBasePage
    {

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型        
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            IList<string> entStrList = RequestData.GetList<string>("data");
            WarehouseExchange ent = null;
            switch (RequestActionString)
            {
                case "create":
                    ent = GetPostedData<WarehouseExchange>();
                    ent.ExchangeState = "未结束";
                    ent.OutWarehouseState = "未出库";
                    ent.InWarehouseState = "未入库";
                    ent.CreateId = UserInfo.UserID;
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = System.DateTime.Now;
                    ent.DoCreate();
                    ProcessDetail(entStrList, ent);
                    DeliveryOrder doEnt = new DeliveryOrder();
                    doEnt.Number = DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getDeliveryNumber()").ToString();
                    doEnt.DeliveryType = "调拨出库";
                    doEnt.PId = ent.Id;
                    doEnt.WarehouseId = ent.FromWarehouseId;
                    doEnt.WarehouseName = ent.FromWarehouseName;
                    doEnt.DoCreate();
                    ProcessDeliveryOrderDetail(entStrList, doEnt);
                    InWarehouse iwEnt = new InWarehouse();
                    iwEnt.WarehouseId = ent.ToWarehouseId;
                    iwEnt.WarehouseName = ent.ToWarehouseName;
                    iwEnt.InWarehouseNo = DataHelper.QueryValue<string>("select SHHG_AimExamine.dbo.fun_getInWarehouseNo()");
                    iwEnt.InWarehouseType = "调拨入库";
                    iwEnt.State = "未入库";
                    iwEnt.CreateId = UserInfo.UserID;
                    iwEnt.CreateName = UserInfo.Name;
                    iwEnt.CreateTime = System.DateTime.Now;
                    iwEnt.PublicInterface = ent.Id;
                    iwEnt.DoCreate();
                    ProcessInWarehouseDetail(entStrList, iwEnt);
                    break;
                //case "update":
                //    ent = this.GetMergedData<PurchaseOrder>();
                //    PurchaseOrderDetail.DeleteAll("PurchaseOrderId='" + ent.Id + "'");
                //    ProcessDetail(entStrList, ent);
                //    ent.PurchaseType = "生产商采购";
                //    ent.DoUpdate();
                //    break;
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    string sql = @"select A.*,B.SupplierName,B.MoneyType,B.Symbo from SHHG_AimExamine..PurchaseOrder as A 
                    left join SHHG_AimExamine..Supplier as B on A.SupplierId=B.Id where A.Id='" + id + "'";
                    IList<EasyDictionary> ents = DataHelper.QueryDictList(sql);
                    this.SetFormData(ents[0]);
                }
            }
            else//添加的时候找到默认供应商
            {
                IList<EasyDictionary> dics = DataHelper.QueryDictList("select Id as SupplierId,SupplierName,MoneyType from SHHG_AimExamine..Supplier where IsDefault='on'");
                if (dics.Count > 0)
                {
                    this.SetFormData(dics[0]);
                }
                //添加的时候自动生成采购单流水号
                PageState.Add("ExchangeNo", DataHelper.QueryValue("select  SHHG_AimExamine.dbo.fun_GetWarehouseExchangeNo()"));
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.* from SHHG_AimExamine..PurchaseOrderDetail as A
                where A.PurchaseOrderId='{0}' order by A.Code Asc";
                sql = string.Format(sql, id);
                IList<EasyDictionary> podDics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", podDics);
            }
            PageState.Add("ProductType", SysEnumeration.GetEnumDict("ProductType"));
            PageState.Add("PriceType", SysEnumeration.GetEnumDict("PriceType"));
        }
        private void DoBatchDelete()
        {
            IList<string> entStrList = RequestData.GetList<string>("data");
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);//能进入修改界面说明没有与该详细关联的入库或者付款
                    DataHelper.ExecSql("delete SHHG_AimExamine..PurchaseOrderDetail where Id='" + objL.Value<string>("Id") + "'");
                }
            }
        }
        private void ProcessDetail(IList<string> entStrList, WarehouseExchange weEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    WarehouseExchangeDetail wedEnt = new WarehouseExchangeDetail();
                    wedEnt.WarehouseExchangeId = weEnt.Id;
                    wedEnt.ProductId = objL.Value<string>("ProductId");
                    wedEnt.ProductName = objL.Value<string>("ProductName");
                    wedEnt.ProductCode = objL.Value<string>("ProductCode");
                    wedEnt.ProductPcn = objL.Value<string>("ProductPcn");
                    wedEnt.ProductIsbn = objL.Value<string>("ProductIsbn");
                    wedEnt.ExchangeQuantity = objL.Value<int>("ExchangeQuantity");
                    wedEnt.DoCreate();
                }
            }
        }
        private void ProcessDeliveryOrderDetail(IList<string> entStrList, DeliveryOrder doEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    JObject json = JsonHelper.GetObject<JObject>(entStrList[j]);
                    DelieryOrderPart dopEnt = new DelieryOrderPart();
                    dopEnt.DId = doEnt.Id;
                    dopEnt.Isbn = json.Value<string>("ProductIsbn");
                    dopEnt.PCode = json.Value<string>("ProductCode");
                    dopEnt.PName = json.Value<string>("ProductName");
                    dopEnt.ProductId = json.Value<string>("ProductId");
                    dopEnt.Count = json.Value<int>("ExchangeQuantity");
                    dopEnt.State = "未出库";
                    dopEnt.CreateId = UserInfo.UserID;
                    dopEnt.CreateName = UserInfo.Name;
                    dopEnt.CreateTime = System.DateTime.Now;
                    dopEnt.DoCreate();
                }
            }
        }
        private void ProcessInWarehouseDetail(IList<string> entStrList, InWarehouse iwEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    JObject json = JsonHelper.GetObject<JObject>(entStrList[j]);
                    OtherInWarehouseDetail oiwdEnt = new OtherInWarehouseDetail();
                    oiwdEnt.InWarehouseId = iwEnt.Id;
                    oiwdEnt.ProductId = json.Value<string>("ProductId");
                    oiwdEnt.ProductName = json.Value<string>("ProductName");
                    oiwdEnt.ProductCode = json.Value<string>("ProductCode");
                    oiwdEnt.ProductPCN = json.Value<string>("ProductPcn");
                    oiwdEnt.ProductISBN = json.Value<string>("ProductIsbn");
                    oiwdEnt.InWarehouseState = "未入库";
                    oiwdEnt.Quantity = json.Value<int>("ExchangeQuantity");
                    oiwdEnt.DoCreate();
                }
            }
        }
    }
}

