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
    public partial class OtherInWarehouseEdit : ExamBasePage
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
            InWarehouse ent = null;
            switch (RequestActionString)
            {
                case "create":
                    ent = this.GetPostedData<InWarehouse>();
                    ent.State = "未入库";
                    ent.InWarehouseNo = DataHelper.QueryValue("select  SHHG_AimExamine.dbo.fun_getInWarehouseNo()").ToString();
                    ent.CreateId = UserInfo.UserID;
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = System.DateTime.Now;
                    ent.DoCreate();
                    ProcessDetail(entStrList, ent);//添加其他入库详情
                    break;
                case "update":
                    ent = this.GetMergedData<InWarehouse>();
                    ent.DoUpdate();
                    OtherInWarehouseDetail.DeleteAll("InWarehouseId='" + ent.Id + "'");
                    ProcessDetail(entStrList, ent);//添加其他入库详情
                    break;
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
                    string sql = @"select A.*,C.Name from SHHG_AimExamine..InWarehouse as A 
                    left join SHHG_AimExamine..Warehouse as C on A.WarehouseId=C.Id where A.Id='{0}'";
                    sql = string.Format(sql, id);
                    IList<EasyDictionary> Dics = DataHelper.QueryDictList(sql);
                    if (Dics.Count > 0)
                    {
                        this.SetFormData(Dics[0]);
                    }
                }
            }
            else
            {
                //添加的时候自动生成入库单流水号
                PageState.Add("InWarehouseNo", DataHelper.QueryValue("select  SHHG_AimExamine.dbo.fun_getInWarehouseNo()"));
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.* from SHHG_AimExamine..OtherInWarehouseDetail as A
                where A.InWarehouseId='{0}' order by ProductCode";
                sql = string.Format(sql, id);
                IList<EasyDictionary> podDics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", podDics);
            }
            PageState.Add("InWarehouseType", SysEnumeration.GetEnumDict("InWarehouseType"));
        }
        private void DoBatchDelete()
        {
            IList<string> entStrList = RequestData.GetList<string>("data");
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    //这个地方不用判断    能进入修改的话。肯定没有实际入库记录。直接删除就可以
                    OtherInWarehouseDetail.DeleteAll("Id='" + objL.Value<string>("Id") + "'");
                }
            }
        }
        private void ProcessDetail(IList<string> entStrList, InWarehouse iwEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    OtherInWarehouseDetail oidEnt = new OtherInWarehouseDetail();
                    oidEnt.InWarehouseId = iwEnt.Id;
                    oidEnt.ProductId = objL.Value<string>("ProductId");
                    oidEnt.ProductName = objL.Value<string>("ProductName");
                    oidEnt.ProductCode = objL.Value<string>("ProductCode");
                    oidEnt.ProductPCN = objL.Value<string>("ProductPCN");
                    oidEnt.ProductISBN = objL.Value<string>("ProductISBN");
                    oidEnt.ProductType = objL.Value<string>("ProductType");
                    oidEnt.Quantity = objL.Value<int>("Quantity");
                    oidEnt.InWarehouseState = "未入库";
                    oidEnt.Remark = objL.Value<string>("Remark");
                    oidEnt.DoCreate();
                }
            }
        }
    }
}

