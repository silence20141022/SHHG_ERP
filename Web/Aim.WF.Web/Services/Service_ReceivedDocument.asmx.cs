using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Activities;
using System.Data.Objects.DataClasses;
using BPM.WF.Model;
using BPM.WF.Activities;
using BPM.WF.EDM;
using BPM.WF.Core;
using System.Xml.Serialization;
using BPM.WF.Exceptions;
using System.IO;
using System.Activities.Statements;


namespace BusinessDemo_收发文_UI.Web.Services
{
    /// <summary>
    /// Summary description for Service_WF
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service_ReceivedDocument : System.Web.Services.WebService
    {

        [WebMethod]
        public WorkflowTemplate[] GetWorkflowTemplatesBindingToEForm(Guid eFormID)
        {
            List<WorkflowTemplate> result = new List<WorkflowTemplate>();

            var dataContext = DBContext.GetDataContext_ServerLayer();
            var mappings = dataContext.Mapping_EFormAndWorkflowTemplate.Where(m => m.EFormID == eFormID);

            foreach (var mapping in mappings)
            {
                string workflowTemplateName = mapping.WorkflowTemplateName;

                result.AddRange(dataContext.WorkflowTemplate.Where(template => template.TemplateName == workflowTemplateName));
            }

            return result.ToArray();
        }

        [WebMethod]
        public Guid SubmitNewEForm(BPM.WF.EDM.ReceivedDocument eFormEntity, Guid workflowTemplateID)
        {

            string entitySetName = eFormEntity.GetType().Name;

            //TODO: 需要错误处理
            WorkflowTemplateModel workflowTemplate = WFEngine.Singletion.GetWorkflowTemplateByWorkflowTemplateID(workflowTemplateID);

            var dataContext = DBContext.GetDataContext_ServerLayer();


            dataContext.AddObject(eFormEntity.GetType().Name, eFormEntity);


            Dictionary<string, Object> inputs = new Dictionary<string, object>();

            HumanWorkflowContext humanWorkflowContext = new HumanWorkflowContext();

            if (!string.IsNullOrEmpty(workflowTemplate.Config))
                humanWorkflowContext.LoadConfig(workflowTemplate.Config);

            humanWorkflowContext.ApprovalNodeContexts = workflowTemplate.GetApprovalNodeContexts(humanWorkflowContext);


            //ApprovalNodeContext approvalNodeContext = new ApprovalNodeContext(humanWorkflowContext, "院办主任审批", ApproverMode.并行);
            //approvalNodeContext.MatchConditionType = ApprovalNodeMatchConditionType.必须全部同意;

            //approvalNodeContext.ApprovalUnitContexts.Add(new ApprovalUnitContext(approvalNodeContext, new Approver() { Value = "user1" }));
            ////approvalNodeContext.ApprovalUnitContexts.Add(new ApprovalUnitContext(approvalNodeContext, new Approver() { Account = "user2" }));
            ////approvalNodeContext.ApprovalUnitContexts.Add(new ApprovalUnitContext(approvalNodeContext, new Approver() { Account = "user3" }));

            //humanWorkflowContext.AddApprovalNodeContext(approvalNodeContext);
            humanWorkflowContext.EFormID = (eFormEntity as IEForm).ID;
            humanWorkflowContext.EFormEntitySetName = entitySetName;
            humanWorkflowContext.EFormName = (eFormEntity as IEForm).Title;

            inputs.Add("HumanWorkflowContext", humanWorkflowContext);

            Guid newWorkflowInstanceID = WFEngine.Singletion.CreateNewWorkflowInstance(workflowTemplate.Activity, inputs).Id;

            humanWorkflowContext.WorkflowInstanceID = newWorkflowInstanceID;


            dataContext.WorkflowInstance.AddObject(
                new WorkflowInstance()
                {
                    ID = newWorkflowInstanceID.ToString(),
                    EFormInstanceID = (eFormEntity as IEForm).ID.ToString(),
                    EFormMetaDataID = dataContext.MetaData_EForm.FirstOrDefault(meta => meta.EntitySetName == entitySetName).ID.ToString(),
                    Status = WorkflowInstanceStatus.Processing.ToString(),
                    WorkflowTemplateID = workflowTemplate.ID.ToString()
                }
                );

            dataContext.SaveChanges();

            WFEngine.Singletion.RunWorkflowInstance(newWorkflowInstanceID);

            return newWorkflowInstanceID;
        }

        [WebMethod]
        public Guid StartFlow(Guid formInstanceId, string formUrl, string title, string workflowTemplateKey, string userId, string userNames)
        {
            var dataContext = DBContext.GetDataContext_ServerLayer();
            WorkflowTemplateModel workflowTemplate = WFEngine.Singletion.GetWorkflowTemplateByWorkflowTemplateKey(workflowTemplateKey);
            string StartNodeName = "";
            System.Collections.Generic.IEnumerator<Activity> list = WorkflowInspectionServices.GetActivities(workflowTemplate.Activity).GetEnumerator();
            list.MoveNext();
            Flowchart flowChart = list.Current as Flowchart;
            StartNodeName = (flowChart.StartNode as FlowStep).Action.DisplayName;
            Dictionary<string, Object> inputs = new Dictionary<string, object>();
            HumanWorkflowContext humanWorkflowContext = new HumanWorkflowContext();
            if (!string.IsNullOrEmpty(workflowTemplate.Config))
                humanWorkflowContext.LoadConfig(workflowTemplate.Config);

            humanWorkflowContext.ApprovalNodeContexts = workflowTemplate.GetApprovalNodeContexts(humanWorkflowContext);
            ApprovalNodeContext approvalNodeContext = new ApprovalNodeContext(humanWorkflowContext, StartNodeName, ApproverMode.并行);
            if (humanWorkflowContext.ApprovalNodeContexts.ContainsKey(StartNodeName))
            {
                approvalNodeContext = humanWorkflowContext.ApprovalNodeContexts[StartNodeName];
                approvalNodeContext.ApprovalUnitContexts.Clear();
            }
            approvalNodeContext.ApprovalUnitContexts.Add(new ApprovalUnitContext(approvalNodeContext, new Approver() { Value = userId, Name = userNames }));
            
            //humanWorkflowContext.AddApprovalNodeContext(approvalNodeContext);
            humanWorkflowContext.EFormID = formInstanceId;
            humanWorkflowContext.EFormName = formUrl;
            humanWorkflowContext.WorkflowTemplateName = workflowTemplate.TemplateName;
            inputs.Add("HumanWorkflowContext", humanWorkflowContext);
            //扩展信息中加入节点关系,以便过程中访问
            humanWorkflowContext.ExtendedProperties.Add("FlowTitle", title);
            Guid newWorkflowInstanceID = WFEngine.Singletion.CreateNewWorkflowInstance(workflowTemplate.Activity, inputs).Id;

            humanWorkflowContext.WorkflowInstanceID = newWorkflowInstanceID;
            dataContext.WorkflowInstance.AddObject(
                new WorkflowInstance()
                {
                    StartTime = DateTime.Now,
                    WorkflowTemplateID = workflowTemplate.ID.ToString(),
                    WorkFlowName = workflowTemplate.TemplateName,
                    RelateUrl = formUrl,
                    RelateId = formInstanceId.ToString(),
                    ID = newWorkflowInstanceID.ToString(),
                    EFormInstanceID = formInstanceId.ToString(),
                    //EFormMetaDataId = dataContext.MetaData_EForm.FirstOrDefault(meta => meta.QualifiedEntitySetName == qualifiedEntitySetName).ID,
                    Status = WorkflowInstanceStatus.Processing.ToString()
                }
                );

            dataContext.SaveChanges();

            WFEngine.Singletion.RunWorkflowInstance(newWorkflowInstanceID);

            return newWorkflowInstanceID;
        }

        [WebMethod]
        [XmlInclude(typeof(KeyValuePair<string, Object>))]
        public void SubmitApprovalResult(Guid workflowInstanceID, string bookmarkName, ApprovalResult approvalResult)
        {
            var dataContext = DBContext.GetDataContext_ServerLayer();
            string id = workflowInstanceID.ToString();
            var workflowInstance = dataContext.WorkflowInstance.FirstOrDefault(instance => instance.ID == id);

            if (workflowInstance == null)
            {
                //TODO: 细化Exception
                throw new Exception_WFCore("");
            }

            if ( // 常规情况: 必须等待所有审批人都提交了审批意见, 流程才能继续往下执行.
                !approvalResult.Task.ApprovalNodeMatchConditionType.HasValue ||
                approvalResult.Task.ApprovalNodeMatchConditionType == (int)ApprovalNodeMatchConditionType.必须全部同意
                )
                WFEngine.Singletion.ResumeBookmark(WFEngine.Singletion.GetWorkflowTemplateByWorkflowTemplateID(new Guid(workflowInstance.WorkflowTemplateID)).Activity, workflowInstanceID, bookmarkName, approvalResult);
            else if (
                approvalResult.Task.ApprovalNodeMatchConditionType.Value == (int)ApprovalNodeMatchConditionType.一个同意即可 ||
                approvalResult.Task.ApprovalNodeMatchConditionType.Value == (int)ApprovalNodeMatchConditionType.有一个不同意则退出
                )
            {


                var tasksInGroup = dataContext.Task.Where(task => task.GroupID == approvalResult.Task.GroupID);

                foreach (var task in tasksInGroup)
                    if (task.ID == approvalResult.Task.ID)
                        WFEngine.Singletion.ResumeBookmark(WFEngine.Singletion.GetWorkflowTemplateByWorkflowTemplateID(new Guid(workflowInstance.WorkflowTemplateID)).Activity, workflowInstanceID, bookmarkName, approvalResult);
                    else
                    {
                        WFEngine.Singletion.ResumeBookmark(WFEngine.Singletion.GetWorkflowTemplateByWorkflowTemplateID(new Guid(workflowInstance.WorkflowTemplateID)).Activity, workflowInstanceID, task.BookmarkName, new ApprovalResult() { ApprovalDateTime = DateTime.Now, Opinion = ApprovalOpinion.系统取消 });
                    }
            }
        }


        [WebMethod]
        public Task[] GetTaskListByOwnerAndStatus(string owner, WorkItemStatus workItemStatus)
        {
            var result = WFEngine.Singletion.GetTaskListByOwnerAndStatus(owner, workItemStatus);

            XmlSerializer xs = new XmlSerializer(typeof(TaskContext));

            foreach (var task in result)
            {
                if (string.IsNullOrEmpty(task.Context))
                    continue;

                StringReader sr = new StringReader(task.Context);

                task.TaskContext = xs.Deserialize(sr) as TaskContext;
            }

            return result;
        }
        [WebMethod]
        public Task GetTaskByTaskId(string taskId)
        {
            var result = WFEngine.Singletion.GetTaskByTaskId(taskId);
            return result;
        }

        [WebMethod]
        public EntityObject GetEFormEntityByWorkflowInstanceID(Guid workflowInstanceID)
        {
            return WFEngine.Singletion.GetEFormEntityByWorkflowInstanceID(workflowInstanceID);
        }

        [WebMethod]
        public MetaData_EForm[] GetAllEFormMetaDatas()
        {
            return WFEngine.Singletion.GetAllEFormMetaDatas();
        }

        [WebMethod]
        public WorkflowTemplate[] GetAllWorkflowTemplates()
        {
            return WFEngine.Singletion.GetAllWorkflowTemplates();
        }

        [WebMethod]
        public void UpdateEFormWorkflowTemplateBinding(Guid eformTemplateID, List<string> boundWorkflowTemplateNames)
        {
            WFEngine.Singletion.UpdateEFormWorkflowTemplateBinding(eformTemplateID,boundWorkflowTemplateNames);
        }


    }

 
}
