using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Aim.Examining.Model;
using NHibernate.Criterion;
using Aim.Portal.Model;
using Aim.Data;
using Aim.Examining.Web;
using Aim.WorkFlow;

namespace Aim.Examining.Web
{
    public partial class FrmPriceApply : ExamListPage
    {
        private IList<PriceApply> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "submit")
            {
                StartFlow();
            }
            else if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else if (RequestActionString == "AutoExecuteFlow")
            {
                Task[] tasks = Task.FindAllByProperties(Task.Prop_WorkflowInstanceID, this.RequestData.Get<string>("FlowId"));

                if (tasks.Length == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    tasks = Task.FindAllByProperties(Task.Prop_WorkflowInstanceID, this.RequestData.Get<string>("FlowId"));
                }

                if (tasks.Length > 0)
                {
                    //自动执行,关键代码
                    Aim.WorkFlow.WorkFlow.AutoExecute(tasks[0]);
                    this.PageState.Add("TaskId", tasks[0].ID);
                }
                else
                {
                    PageState.Add("error", "自动提交申请人失败，请手动提交");
                }
            }
            //else if (RequestActionString == "GenerateOrder")
            //{
            //    //生成销售单
            //    string Id = RequestData.Get<string>("Id");

            //    PageState.Add("result", Id);
            //}
            else
            {
                DoSelect();
            }
        }


        public void StartFlow()
        {
            string id = this.RequestData.Get<string>("Id");
            string state = this.RequestData.Get<string>("state");
            PriceApply pb = PriceApply.Find(id);
            pb.State = state;
            pb.Save();
            string code = this.RequestData.Get<string>("FlowKey");

            //启动流程
            //表单路径,后面加上参数传入
            string formUrl = "/SaleManagement/FrmPriceApplyView.aspx?op=u&&id=" + id;
            Guid guid = Aim.WorkFlow.WorkFlow.StartWorkFlow(id, formUrl, "销售价格申请单(" + pb.Number + ")[" + pb.CreateName + "]", code, this.UserInfo.UserID, this.UserInfo.Name);

            this.PageState.Add("FlowId", guid.ToString());
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
                SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));
            ents = PriceApply.FindAll(SearchCriterion);
            this.PageState.Add("PriceApplyList", ents);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");

            if (idList != null && idList.Count > 0)
            {
                PriceApply.DoBatchDelete(idList.ToArray());
            }
        }
    }
}
