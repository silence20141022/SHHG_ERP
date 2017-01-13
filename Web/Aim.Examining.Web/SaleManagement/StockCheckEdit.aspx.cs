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
using System.Data.OleDb;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace Aim.Examining.Web.SaleManagement
{
    public partial class StockCheckEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            StockCheck scEnt = null;
            IList<String> strList = RequestData.GetList<string>("data");
            switch (RequestActionString)
            {
                case "update":
                    scEnt = this.GetMergedData<StockCheck>();
                    scEnt.DoUpdate();
                    ProcessDetail(strList, scEnt);
                    break;
                case "create":
                    scEnt = this.GetPostedData<StockCheck>();
                    scEnt.State = "未结束";
                    scEnt.DoCreate();
                    ProcessDetail(strList, scEnt);
                    break;
                case "delete":
                    scEnt = this.GetTargetData<StockCheck>();
                    scEnt.DoDelete();
                    return;
                default:
                    if (op != "c" && op != "cs")
                    {
                        if (!String.IsNullOrEmpty(id))
                        {
                            scEnt = StockCheck.Find(id);
                            SetFormData(scEnt);
                            IList<StockCheckDetail> scdEnts = StockCheckDetail.FindAllByProperty("StockCheckId", id);
                            PageState.Add("DataList", scdEnts);
                        }
                    }
                    else
                    {
                        PageState.Add("StockCheckNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getStockCheckNo()"));
                        this.PageState.Add("FlowEnum", SysEnumeration.GetEnumDictList("WorkFlow.Simple"));
                        if (RequestActionString == "submitfinish")
                        {
                            StockCheck pc = StockCheck.Find(this.RequestData.Get<string>("id"));
                            pc.State = "End";
                            // pc.InventoryState = this.RequestData.Get<string>("ApprovalState");
                            pc.Save();
                        }
                    }
                    break;
            }
        }
        private void ProcessDetail(IList<string> strList, StockCheck ent)
        {
            IList<StockCheckDetail> scEnts = StockCheckDetail.FindAllByProperty("StockCheckId", ent.Id);
            foreach (StockCheckDetail tent in scEnts)
            {
                tent.DoDelete();
            }
            StockCheckDetail scdEnt = null;
            foreach (string str in strList)
            {
                JObject json = JsonHelper.GetObject<JObject>(str);
                scdEnt = new StockCheckDetail();
                scdEnt.StockCheckId = ent.Id;
                scdEnt.ProductId = json.Value<string>("ProductId");
                scdEnt.ProductName = json.Value<string>("ProductName");
                scdEnt.ProductCode = json.Value<string>("ProductCode");
                scdEnt.ProductPcn = json.Value<string>("ProductPcn");
                scdEnt.StockQuantity = json.Value<int>("StockQuantity");
                scdEnt.StockCheckState = json.Value<string>("StockCheckState");
                scdEnt.DoCreate();
            }
        }
    }
}

