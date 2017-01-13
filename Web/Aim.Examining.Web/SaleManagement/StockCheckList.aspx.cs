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

namespace Aim.Examining.Web.SaleManagement
{
    public partial class StockCheckList : ExamListPage
    {
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
                case "SubmitExamine":
                    string id = this.RequestData.Get<string>("Id");
                    ent = StockCheck.Find(id);
                    ent.WorkFlowState = RequestData.Get<string>("state");
                    ent.ExamineResult = RequestData.Get<string>("ApprovalState");
                    ent.DoUpdate();
                    StartFlow(id);
                    break;
                case "AutoExecuteFlow":
                    Task task = Task.FindAllByProperties(Task.Prop_WorkflowInstanceID, this.RequestData.Get<string>("FlowId"))[0];
                    //自动执行,关键代码
                    Aim.WorkFlow.WorkFlow.AutoExecute(task);
                    this.PageState.Add("TaskId", task.ID);
                    break;
                default:
                    if (RequestData.Get<string>("optype") == "getChildData")
                    {
                        string stockCheckId = RequestData.Get<string>("StockCheckId");
                        IList<StockCheckDetail> scdEnt = StockCheckDetail.FindAllByProperty("StockCheckId", stockCheckId);
                        PageState.Add("DetailList", scdEnt);
                    }
                    else
                    {
                        DoSelect();
                    }
                    break;
            }
        }
        public void StartFlow(string formId)
        {
            string code = this.RequestData.Get<string>("FlowKey");
            Aim.WorkFlow.WorkflowTemplate ne = Aim.WorkFlow.WorkflowTemplate.FindAllByProperties(Aim.WorkFlow.WorkflowTemplate.Prop_Code, code)[0];
            //启动流程
            //表单路径,后面加上参数传入
            StockCheck scEnt = StockCheck.Find(formId);
            string formUrl = "/SaleManagement/StockCheckView.aspx?id=" + formId;
            Guid guid = Aim.WorkFlow.WorkFlow.StartWorkFlow(formId, formUrl, "盘点单【" + scEnt.StockCheckNo + "】申请人【" + scEnt.CreateName + "】", code, this.UserInfo.UserID, this.UserInfo.Name);
            //返回流程的Id
            this.PageState.Add("FlowId", guid.ToString());
        }

        private void DoSelect()
        {
            string where = "";
            string detailwhere = "";
            string index = RequestData.Get<string>("Index");
            string sql = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.Value.ToString() != "")
                {
                    if (item.PropertyName != "ProductCode")
                    {
                        where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                    else
                    {
                        detailwhere = "where " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                }
            }
            if (index == "0")
            {
                sql = @"select * from SHHG_AimExamine..StockCheck   where State='未结束' and Id in 
                (select StockCheckId from SHHG_AimExamine..StockCheckDetail " + detailwhere + ")" + where;
            }
            else
            {
                sql = @"select * from SHHG_AimExamine..StockCheck   where State='已结束' and Id in 
                (select StockCheckId from SHHG_AimExamine..StockCheckDetail " + detailwhere + ")" + where;
            }
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
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
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || search.Orders[0].Ascending ? " desc" : " asc";
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
