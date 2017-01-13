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
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmLeaveApp : ExamListPage
    {
        private IList<Leave> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "submit")
            {
                StartFlow();
            }
            else if (RequestActionString == "AutoExecuteFlow")
            {
                Task task = Task.FindAllByProperties(Task.Prop_WorkflowInstanceID, this.RequestData.Get<string>("FlowId"))[0];
                //自动执行,关键代码
                Aim.WorkFlow.WorkFlow.AutoExecute(task);
                this.PageState.Add("TaskId", task.ID);
            }
            else if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else
            {
                DoSelect();
            }
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
                Leave.DoBatchDelete(idList.ToArray());
            }
        }

        public void StartFlow()
        {
            string state = this.RequestData.Get<string>("state");
            string id = this.RequestData.Get<string>("Id");
            Leave pb = Leave.Find(id);
            pb.State = state;
            pb.Save();
            string code = this.RequestData.Get<string>("FlowKey");

            //启动流程
            //表单路径,后面加上参数传入
            string formUrl = "/DailyManager/FrmLeaveAppWFView.aspx?op=u&id=" + id;
            Guid guid = Aim.WorkFlow.WorkFlow.StartWorkFlow(id, formUrl, "请假申请(" + pb.LeaveUser + ")", code, this.UserInfo.UserID, this.UserInfo.Name);

            this.PageState.Add("FlowId", guid.ToString());
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string where = " where (CreateName='" + UserInfo.Name + "' or LeaveUser='" + UserInfo.Name + "') ";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
            }
            string sql = @"select * from " + db + "..Leave" + where;
            this.PageState.Add("LeaveList", GetPageData(sql, SearchCriterion));
        }

        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " desc" : " asc";
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * search.PageSize + 1, search.CurrentPageIndex * search.PageSize);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }

    }
}
