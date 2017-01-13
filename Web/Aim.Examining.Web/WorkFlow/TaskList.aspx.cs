using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Aim.Data;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.WorkFlow;
using Aim.WorkFlow.WFService;

namespace Aim.Portal.Web.WorkFlow
{
    public partial class TaskList : BaseListPage
    {
        #region 属性

        #endregion

        #region 变量

        private IList<Aim.WorkFlow.Task> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            Aim.WorkFlow.Task ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Aim.WorkFlow.Task>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                default:
                    if (RequestActionString == "batchdelete")
                    {
                        IList<object> idList = RequestData.GetList<object>("IdList");

                        if (idList != null && idList.Count > 0)
                        {
                            Aim.WorkFlow.Task.DoBatchDelete(idList.ToArray());
                        }
                    }
                    else if (this.RequestActionString.ToLower() == "startflow")
                    {
                        //启动流程
                        /*string key = "FirstFlow";
                        //表单路径,后面加上参数传入
                        string formUrl = "/EPC/PrjBasic/PrjBasicEdit.aspx?op=u";
                        Aim.WorkFlow.WorkFlow.StartWorkFlow("", formUrl, "流程的标题", key, this.UserInfo.UserID, this.UserInfo.Name);*/
                        Aim.WorkFlow.WorkflowTemplate ne = Aim.WorkFlow.WorkflowTemplate.FindAllByProperty("Code","audit")[0];
                        //启动流程
                        string key = "audit";
                        //表单路径,后面加上参数传入
                        string formUrl = "/EPC/PrjBasic/PrjBasicEdit.aspx?op=u";
                        Aim.WorkFlow.WorkFlow.StartWorkFlow(ne.ID, formUrl, ne.TemplateName, key, this.UserInfo.UserID, this.UserInfo.Name);
                        PageState.Add("message", "启动成功");
                    }
                    else
                    {
                        SearchCriterion.SetSearch("Status", int.Parse(this.RequestData["Status"].ToString()));
                        SearchCriterion.SetSearch("OwnerId", this.UserInfo.UserID);
                        SearchCriterion.SetOrder("CreatedTime", false);
                        string dateFlag = this.RequestData["Date"] == null ? "3" : this.RequestData["Date"].ToString();
                        switch (dateFlag)
                        {
                            case "3":
                                SearchCriterion.SetSearch("CreatedTime", DateTime.Now.AddDays(-3), SearchModeEnum.GreaterThanEqual);
                                break;
                            case "7":
                                SearchCriterion.SetSearch("CreatedTime", DateTime.Now.AddDays(-7), SearchModeEnum.GreaterThanEqual);
                                break;
                            case "14":
                                SearchCriterion.SetSearch("CreatedTime", DateTime.Now.AddDays(-14), SearchModeEnum.GreaterThanEqual);
                                break;
                            case "30":
                                SearchCriterion.SetSearch("CreatedTime", DateTime.Now.AddMonths(-1), SearchModeEnum.GreaterThanEqual);
                                break;
                            case "31":
                                SearchCriterion.SetSearch("CreatedTime", DateTime.Now.AddMonths(-1), SearchModeEnum.LessThanEqual);
                                break;
                        }
                        ents = Aim.WorkFlow.TaskRule.FindAll(SearchCriterion);
                        this.PageState.Add("SysWorkFlowTaskList", ents);
                    }
                    break;
            }

        }

        #endregion

        #region 私有方法

        #endregion
    }
}

