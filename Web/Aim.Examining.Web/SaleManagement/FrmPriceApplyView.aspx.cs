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
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmPriceApplyView : ExamBasePage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string oid = String.Empty;   // 对象id
        string paid = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            paid = RequestData.Get<string>("paid");
            oid = RequestData.Get<string>("oid");
            type = RequestData.Get<string>("type");

            PriceApply ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<PriceApply>();
                    ent.State = "";
                    ent.ApprovalState = "";
                    ent.DoUpdate();
                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<PriceApply>();
                    if (oid != "")
                    {
                        ent.OId = oid;
                    }

                    //自动生成流水号
                    ent.Number = DataHelper.QueryValue("select " + db + ".dbo.fun_getPriceAppNumber()") + "";

                    ent.DoCreate();
                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<PriceApply>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
                default:
                    if (RequestActionString == "submitfinish")
                    {
                        PriceApply pc = PriceApply.Find(this.RequestData.Get<string>("id"));
                        pc.State = "End";
                        pc.ApprovalState = this.RequestData.Get<string>("ApprovalState");
                        pc.Save();

                        //更新销售单对应的状态
                        if (!string.IsNullOrEmpty(pc.OId))
                        {
                            SaleOrder order = SaleOrder.TryFind(pc.OId);
                            if (order != null)
                            {
                                order.PAState = pc.ApprovalState;
                                order.Save();
                            }
                        }
                    }
                    break;
            }
            if (RequestActionString == "getSalesman")
            {
                string cid = RequestData.Get<string>("CId");
                Customer customer = Customer.Find(cid);
                if (customer != null)
                {
                    PageState.Add("Code", customer.Code);
                }
            }
            else if (op == "c" && !string.IsNullOrEmpty(oid))
            {
                if (!string.IsNullOrEmpty(oid))
                {
                    SaleOrder order = SaleOrder.TryFind(oid);
                    if (order != null)
                    {
                        order.Id = "";
                        order.Reason = "";
                        order.Number = "";
                        order.ApprovalState = "";
                        order.Remark = "";
                        order.State = "";
                        this.SetFormData(order);
                    }
                }
            }
            else if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = PriceApply.Find(id);
                    ent.CCode = Customer.Find(ent.CId).Importance;//2017-08-22客户要求在价格申请单中显示客户等级，客户编号没有什么用，就用次属性代替客户等级
                }

                this.SetFormData(ent);
                this.PageState.Add("State", ent.State);
            }

            this.PageState.Add("FlowEnum", SysEnumeration.GetEnumDictList("WorkFlow.Simple"));
        }

        #endregion

    }
}

