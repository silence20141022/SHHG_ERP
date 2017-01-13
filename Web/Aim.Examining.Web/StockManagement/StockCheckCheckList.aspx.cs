using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;
using Aim.Portal.Web.UI;
using Aim.Utilities;
using System.Data.SqlClient;
using Aim.WorkFlow;

namespace Aim.Examining.Web.StockManagement
{
    public partial class StockCheckCheckList : ExamListPage
    {
        private IList<StockCheck> ents = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            StockCheck ent = null;
            switch (RequestActionString)
            {
                case "delete":
                    ent = this.GetTargetData<StockCheck>();
                    ent.DoDelete();
                    break;
                case "batchdelete":
                    DoBatchDelete();
                    break;
                case "submit":
                    StartFlow();
                    break;
                case "AutoExecuteFlow":
                    Task task = Task.FindAllByProperties(Task.Prop_WorkflowInstanceID, this.RequestData.Get<string>("FlowId"))[0];
                    //自动执行,关键代码
                    Aim.WorkFlow.WorkFlow.AutoExecute(task);
                    this.PageState.Add("TaskId", task.ID);
                    break;
                default:
                    DoSelect();
                    break;
            }

        }
        private void DoSelect()
        {
            if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
            {
                SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));
            }
            string state = "未结束";
            string temp = RequestData.Get<string>("State");
            if (!string.IsNullOrEmpty(temp))
            {
                state = temp;

            }
            if (state == "未结束")
            {
                SearchCriterion.AddSearch("State", "已结束", SearchModeEnum.NotEqual);
            }
            else
            {
                SearchCriterion.AddSearch("State", state);
            }
            ents = StockCheck.FindAll(SearchCriterion);
            PageState.Add("DataList", ents);
        }
        /// <summary>
        /// 根据拼音首字母查询
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="pinyinIndex"></param>
        /// <returns></returns>
        public string GetPinyinWhereString(string fieldName, string pinyinIndex)
        {
            string[,] hz = Tool.GetHanziScope(pinyinIndex);
            string whereString = "(";
            for (int i = 0; i < hz.GetLength(0); i++)
            {
                whereString += "(SUBSTRING(" + fieldName + ", " + (i + 1) + ", 1) >= '" + hz[i, 0] + "' AND SUBSTRING(" + fieldName + ", " + (i + 1) + ", 1) <= '" + hz[i, 1] + "') AND ";
            }
            if (whereString.Substring(whereString.Length - 4, 4) == "AND ")
                return whereString.Substring(0, whereString.Length - 4) + ")";
            else
                return "(1=1)";
        }


        public void StartFlow()
        {
            string state = this.RequestData.Get<string>("state");
            string id = this.RequestData.Get<string>("Id");
            StockCheck pb = StockCheck.Find(id);
            pb.State = state;
            pb.Save();
            string code = this.RequestData.Get<string>("FlowKey");

            //启动流程
            //表单路径,后面加上参数传入
            string formUrl = "/StockManagement/FrmInventoryView.aspx?op=u&&id=" + id;
            Guid guid = Aim.WorkFlow.WorkFlow.StartWorkFlow(id, formUrl, "仓库盘点异常审批(" + pb.StockCheckNo + ")[" + pb.CreateName + "]", code, this.UserInfo.UserID, this.UserInfo.Name);

            this.PageState.Add("FlowId", guid.ToString());
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
                StockCheck.DoBatchDelete(idList.ToArray());
            }
        }
    }
}
