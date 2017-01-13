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

namespace Aim.Examining.Web.SaleManagement
{
    public partial class StockCheckView : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        StockCheck ent = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            ent = StockCheck.Find(id);
            switch (RequestActionString)
            {
                case "WorkFlowEnd":
                    ent.WorkFlowState = RequestData.Get<string>("state");
                    ent.ExamineResult = RequestData.Get<string>("ApprovalState");
                    ent.State = "已结束";
                    ent.DoUpdate();
                    IList<StockCheckDetail> scdEnts = StockCheckDetail.FindAllByProperty("StockCheckId", id);
                    foreach (StockCheckDetail scdEnt in scdEnts)
                    {
                        if (scdEnt.StockCheckResult != "正常")
                        {
                            scdEnt.StockCheckState = "盘点结束";
                            scdEnt.DoUpdate();
                        }
                    }
                    if (ent.ExamineResult == "同意")
                    {
                        foreach (StockCheckDetail scdEnt in scdEnts)
                        {
                            if (scdEnt.StockCheckResult != "正常")//创建库存变更日志  并更改库存
                            {
                                StockLog slEnt = new StockLog();
                                slEnt.InOrOutDetailId = scdEnt.Id;
                                StockCheck scEnt = StockCheck.Find(scdEnt.StockCheckId);
                                slEnt.InOrOutBillNo = scEnt.StockCheckNo;
                                slEnt.OperateType = "库存盘点";
                                slEnt.WarehouseId = scEnt.WarehouseId;
                                slEnt.WarehouseName = scEnt.WarehouseName;
                                slEnt.StockQuantity = scdEnt.StockQuantity;
                                slEnt.Quantity = scdEnt.StockCheckQuantity - scdEnt.StockQuantity;
                                slEnt.ProductId = scdEnt.ProductId;
                                Product pEnt = Product.Find(scdEnt.ProductId);
                                slEnt.ProductName = pEnt.Name;
                                slEnt.ProductCode = pEnt.Code;
                                slEnt.ProductIsbn = pEnt.Isbn;
                                slEnt.ProductPcn = pEnt.Pcn;
                                slEnt.CreateId = UserInfo.UserID;
                                slEnt.CreateName = UserInfo.Name;
                                slEnt.CreateTime = System.DateTime.Now;
                                slEnt.DoCreate();
                                IList<StockInfo> siEnts = StockInfo.FindAllByProperties("ProductId", scdEnt.ProductId, "WarehouseId", ent.WarehouseId);
                                if (siEnts.Count > 0)
                                {
                                    siEnts[0].StockQuantity = scdEnt.StockCheckQuantity;
                                    siEnts[0].DoUpdate();
                                }
                            }
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
                    SetFormData(ent);
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                IList<StockCheckDetail> scdEnts = StockCheckDetail.FindAllByProperty("StockCheckId", id);
                this.PageState.Add("DataList", scdEnts);
            }
        }
    }
}

