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
namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseOrderEdit : ExamBasePage
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
            PurchaseOrder ent = null;
            switch (RequestActionString)
            {
                case "create":
                    ent = this.GetPostedData<PurchaseOrder>();
                    ent.OrderState = "未结束";
                    ent.PayState = "未付款";
                    ent.InWarehouseState = "未入库";
                    ent.InvoiceState = "未关联";
                    //  ent.PurchaseOrderNo = DataHelper.QueryValue("select  SHHG_AimExamine.dbo.fun_getPurchaseOrderNumber()").ToString();
                    ent.PurchaseType = "生产商采购";
                    ent.CreateId = UserInfo.UserID;
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = System.DateTime.Now;
                    ent.DoCreate();
                    ProcessDetail(entStrList, ent);//添加采购单详情
                    break;
                case "update":
                    ent = this.GetMergedData<PurchaseOrder>();
                    PurchaseOrderDetail.DeleteAll("PurchaseOrderId='" + ent.Id + "'");
                    ProcessDetail(entStrList, ent);
                    ent.PurchaseType = "生产商采购";
                    ent.DoUpdate();
                    break;
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
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
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    string sql = @"select A.*,B.SupplierName,B.MoneyType,B.Symbo from SHHG_AimExamine..PurchaseOrder as A 
                    left join SHHG_AimExamine..Supplier as B on A.SupplierId=B.Id where A.Id='" + id + "'";
                    IList<EasyDictionary> ents = DataHelper.QueryDictList(sql);
                    SetFormData(ents[0]);
                }
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
        private void ProcessDetail(IList<string> entStrList, PurchaseOrder poEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    PurchaseOrderDetail podEnt = new PurchaseOrderDetail();
                    podEnt.PurchaseOrderId = poEnt.Id;                   
                    podEnt.ProductId = objL.Value<string>("ProductId");
                    podEnt.Name = objL.Value<string>("Name");
                    podEnt.Code = objL.Value<string>("Code");
                    podEnt.PCN = objL.Value<string>("PCN");
                    podEnt.BuyPrice = objL.Value<decimal>("BuyPrice");
                    podEnt.Quantity = objL.Value<int>("Quantity");
                    podEnt.Amount = objL.Value<decimal>("Amount");
                    podEnt.PayState = "未付款";
                    podEnt.InWarehouseState = "未入库";
                    podEnt.InvoiceState = "未关联"; 
                    if (!string.IsNullOrEmpty(objL.Value<string>("ExpectedArrivalDate")))
                    {
                        podEnt.ExpectedArrivalDate = objL.Value<DateTime>("ExpectedArrivalDate");
                    } 
                    podEnt.DoCreate();
                }
            }
        }
    }
}

