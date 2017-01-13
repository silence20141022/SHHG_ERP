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

namespace Aim.Examining.Web.StockManagement
{
    public partial class StockCheckCheck : ExamBasePage
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
            StockCheckDetail scdEnt = null;
            foreach (string str in strList)
            {
                JObject json = JsonHelper.GetObject<JObject>(str);
                scdEnt = StockCheckDetail.Find(json.Value<string>("Id"));
                if (json.Value<int>("StockCheckQuantity") >= 0)
                {
                    scdEnt.StockCheckQuantity = json.Value<int>("StockCheckQuantity");
                    if (scdEnt.StockQuantity > scdEnt.StockCheckQuantity)
                    {
                        scdEnt.StockCheckResult = "盘亏";
                    }
                    if (scdEnt.StockQuantity < scdEnt.StockCheckQuantity)
                    {
                        scdEnt.StockCheckResult = "盘赢";
                    }
                    if (scdEnt.StockQuantity == scdEnt.StockCheckQuantity)
                    {
                        scdEnt.StockCheckResult = "正常";
                        scdEnt.StockCheckState = "盘点结束";
                    }                   
                    scdEnt.DoUpdate();
                }
            }
            //1  如果所有的盘点明细都正常 则把盘点单的状态改为已结束
            SearchCriterion.AddSearch("StockCheckResult", "正常", SearchModeEnum.NotEqual);
            IList<StockCheckDetail> scdEnts = StockCheckDetail.FindAll(SearchCriterion);
            if (scdEnts.Count == 0)
            {
                ent.State = "已结束"; ent.Result = "正常";
            }
            else
            {
                ent.Result = "异常";
            }
            ent.DoUpdate();
        }
    }
}

