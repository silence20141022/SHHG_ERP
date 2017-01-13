using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using Aim.Data;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.Examining.Model;
using System.Web.Script.Serialization;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class InWarehouseEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            InWarehouse ent = null;
            IList<string> entStrList = RequestData.GetList<string>("data");
            switch (RequestActionString)
            {
                case "create":
                    ent = this.GetPostedData<InWarehouse>();
                    ent.State = "未入库";
                    ent.DoCreate();
                    ProcessDetail(entStrList, ent);
                    break;
                case "update":
                    ent = this.GetMergedData<InWarehouse>();
                    ent.DoUpdate();
                    InWarehouseDetail.DeleteAll("InWarehouseId='" + ent.Id + "'");
                    ProcessDetail(entStrList, ent);
                    break;
                case "batchdelete":
                    if (entStrList.Count > 0)
                    {
                        for (int i = 0; i < entStrList.Count; i++)
                        {
                            Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[i]);
                            DataHelper.ExecSql("delete SHHG_AimExamine..InWarehouseDetail where Id='" + objL.Value<string>("Id") + "'");
                        }
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    //修改的时候要带入供应商的相关信息 所以不能简单 ent = InWarehouse.Find(id);
                    string sql = @"select A.*,B.Symbo,C.Name from SHHG_AimExamine..InWarehouse as A 
                    left join SHHG_AimExamine..Supplier as B on A.SupplierId=B.Id
                    left join SHHG_AimExamine..Warehouse as C on A.WarehouseId=C.Id where A.Id='{0}'";
                    sql = string.Format(sql, id);
                    IList<EasyDictionary> Dics = DataHelper.QueryDictList(sql);
                    if (Dics.Count > 0)
                    {
                        this.SetFormData(Dics[0]);
                    }
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.*,C.PurchaseOrderNo,C.PurchaseOrderId,C.BuyPrice,A.IQuantity as Raw,
               (C.BuyPrice*A.IQuantity) as Amount,B.Name,B.Code,SHHG_AimExamine.dbo.fun_NoInQuantity(A.PurchaseOrderDetailId) as NoIn
               from SHHG_AimExamine..InWarehouseDetail as A   
               left join SHHG_AimExamine..Products as B on A.ProductId=B.Id  
               left join SHHG_AimExamine..PurchaseOrderDetail as C  on A.PurchaseOrderDetailId=C.Id             
               where A.InWarehouseId='{0}' order by C.PurchaseOrderId asc";
                sql = string.Format(sql, id);
                IList<EasyDictionary> dics = FormatData(DataHelper.QueryDictList(sql));
                this.PageState.Add("DetailList", dics);
            }
            else
            {
                PageState.Add("InWarehouseNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getInWarehouseNo()").ToString());
            }
        }
        private IList<EasyDictionary> FormatData(IList<EasyDictionary> dics)
        {
            string same = string.Empty;
            for (int i = 0; i < dics.Count; i++)
            {
                if (dics[i].Get<string>("PurchaseOrderId") == same)
                {
                    dics[i].Set("PurchaseOrderNo", "");
                }
                else
                {
                    same = dics[i].Get<string>("PurchaseOrderId");
                }
            }
            return dics;
        }
        private void ProcessDetail(IList<string> entStrList, InWarehouse iwEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    //添加入库单详细
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    InWarehouseDetail iwdEnt = new InWarehouseDetail();
                    iwdEnt.InWarehouseId = iwEnt.Id;
                    iwdEnt.PurchaseOrderDetailId = objL.Value<string>("PurchaseOrderDetailId");
                    iwdEnt.ProductId = objL.Value<string>("ProductId");
                    iwdEnt.ProductCode = objL.Value<string>("Code");
                    iwdEnt.InWarehouseState = "未入库";
                    iwdEnt.IQuantity = Convert.ToInt32(objL.Value<string>("IQuantity"));
                    iwdEnt.DoCreate();
                }
            }
        }
    }
}

