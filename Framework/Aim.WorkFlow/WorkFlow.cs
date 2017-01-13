using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Aim.WorkFlow.WFService;
using Aim.Data;

namespace Aim.WorkFlow
{
    public class WorkFlow
    {
        private static WFService.Service_ReceivedDocumentSoapClient _client;

        public static WFService.Service_ReceivedDocumentSoapClient ServiceClient
        {
            get
            {
                if (_client == null)
                {
                    _client = new WFService.Service_ReceivedDocumentSoapClient();
                }

                return _client;
            }
        }
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="formUrl">表单URL全路径</param>
        /// <param name="title">任务标题,存放表单关键字等,以区分任务列表中的任务</param>
        /// <param name="flowDefineKey">流程定义Key</param>
        /// <param name="userId">第一环节执行人ID</param>
        /// <param name="userName">第一环节执行人</param>
        /// <returns>流程实例ID</returns>
        public static Guid StartWorkFlow(string formUrl, string flowDefineKey, string userId, string userName)
        {
            Guid guid = ServiceClient.StartFlow(Guid.Empty, formUrl, "", flowDefineKey, userId, userName);
            return guid;
        }
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="formId">表单实例ID</param>
        /// <param name="formUrl">表单URL全路径</param>
        /// <param name="flowDefineKey">流程定义Key</param>
        /// <param name="userId">第一环节执行人ID</param>
        /// <param name="userName">第一环节执行人</param>
        /// <returns>流程实例ID</returns>
        public static Guid StartWorkFlow(string formId, string formUrl, string flowDefineKey, string userId, string userName)
        {
            Guid guid = ServiceClient.StartFlow(new Guid(formId), formUrl, "", flowDefineKey, userId, userName);
            return guid;
        }
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="formId">表单实例ID</param>
        /// <param name="formUrl">表单URL全路径</param>
        /// <param name="title">任务标题,存放表单关键字等,以区分任务列表中的任务</param>
        /// <param name="flowDefineKey">流程定义Key</param>
        /// <param name="userId">第一环节执行人ID</param>
        /// <param name="userName">第一环节执行人</param>
        /// <returns>流程实例ID</returns>
        public static Guid StartWorkFlow(string formId, string formUrl, string title, string flowDefineKey, string userId, string userName)
        {
            if (formId.Trim() == "") formId = Guid.Empty.ToString();
            Guid guid = ServiceClient.StartFlow(new Guid(formId), formUrl, title, flowDefineKey, userId, userName);
            return guid;
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="defineKey">流程定义Key</param>
        /// <param name="instanseId">流程实例ID</param>
        /// <param name="bookmarkName">任务bookmarkid</param>
        /// <param name="result">审批结果</param>
        public static void SubmitTask(string defineId, string instanseId, string bookmarkName, WFService.ApprovalResult result)
        {
            ServiceClient.SubmitApprovalResult(new Guid(instanseId), bookmarkName, result);
        }

        /// <summary>
        /// 适用于流程第二步已指定人员的,自动执行
        /// </summary>
        /// <param name="fTask"></param>
        public static void AutoExecute(Task fTask)
        {
            XmlSerializer xs = new XmlSerializer(typeof(TaskContext));
            if (!string.IsNullOrEmpty(fTask.Context))
            {
                StringReader sr = new StringReader(fTask.Context);
                TaskContext content = xs.Deserialize(sr) as TaskContext;
                if (content.SwitchRules.Length > 0)
                {
                    TaskContextSwitchRuleNextAction[] arrs = content.SwitchRules[0].NextActions;
                    if (arrs.Length > 0)
                    {
                        string route = arrs[0].Name;
                        Aim.WorkFlow.WFService.Task taskS = Aim.WorkFlow.WorkFlow.ServiceClient.GetTaskByTaskId(fTask.ID);
                        SubmitTask("", fTask.WorkflowInstanceID, fTask.BookmarkName, GetApprovalResult(taskS, fTask.ID, fTask.WorkflowInstanceID, route)); ;
                    }
                }
            }
        }
        public static ApprovalResult GetApprovalResult(Aim.WorkFlow.WFService.Task task, string taskId, string winstanceId, string route)
        {
            ApprovalResult result = new ApprovalResult()
            {
                Task = task,
                TaskId = task.ID,

                ApprovalDateTime = DateTime.Now,

                Opinion = ApprovalOpinion.同意,

                //Comment = ""
            };

            /// 设定跳过后续哪些节点. 
            /*List<ApprovalNodeSkipInfo> approvalNodeSkipInfos = new List<ApprovalNodeSkipInfo>();

            if (checkBox1.IsChecked.HasValue && checkBox1.IsChecked.Value)
                approvalNodeSkipInfos.Add(new ApprovalNodeSkipInfo() { ApprovalNodeContextName = "经理审批", CanBeSkipped = true });

            if (checkBox2.IsChecked.HasValue && checkBox2.IsChecked.Value)
                approvalNodeSkipInfos.Add(new ApprovalNodeSkipInfo() { ApprovalNodeContextName = "主管审批", CanBeSkipped = true });

            result.ApprovalNodeSkipInfoList = approvalNodeSkipInfos.ToArray();
            */
            /// 设定选中的流转节点
            if (route != "")
            {
                result.SwitchRules = new KeyValuePair_V2[] 
                    { 
                        new KeyValuePair_V2() 
                        { 
                            Key = task.ApprovalNodeName, 
                            Value = route 
                        } 
                    };
                WorkflowInstance ins = WorkflowInstance.Find(winstanceId);
                Aim.WorkFlow.WorkflowTemplate temp = Aim.WorkFlow.WorkflowTemplate.Find(ins.WorkflowTemplateID);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(temp.XAML);
                XmlElement root = doc.DocumentElement;
                string nameSpace = root.NamespaceURI;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ns", nameSpace);
                nsmgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                nsmgr.AddNamespace("bwa", "clr-namespace:BPM.WF.Activities;assembly=BPM.WF");

                string current = "ApprovalNode Name=\"" + task.ApprovalNodeName + "\"";
                XmlNode currentNode = root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + current + "')]", nsmgr);
                //XmlNode node = root.SelectSingleNode("//*[@x:Key='" + nextName + "']", nsmgr);
                XmlNode node = currentNode.NextSibling.SelectSingleNode("ns:FlowSwitch/ns:FlowStep[@x:Key='" + route + "']", nsmgr);
                string nextUserIds = "";
                string nextUserNames = "";
                string nextUserAccountType = "";
                string nextNodeName = "";
                string content = "ApprovalNode Name=\"" + route + "\"";
                if (root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + content + "')]", nsmgr) != null)//直接路由
                {
                    string config = System.Web.HttpUtility.HtmlDecode(root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + content + "')]", nsmgr).Attributes["ApprovalNodeConfig"].InnerXml);
                    XmlDocument docC = new XmlDocument();
                    docC.LoadXml(config);
                    nextNodeName = docC.DocumentElement.Attributes["Name"].InnerText.ToString();
                    if (docC.DocumentElement.SelectSingleNode("ApprovalUnits") != null && docC.DocumentElement.SelectSingleNode("ApprovalUnits").ChildNodes.Count > 0)
                    {
                        XmlNodeList list = docC.DocumentElement.SelectSingleNode("ApprovalUnits").ChildNodes;
                        foreach (XmlNode chd in list)
                        {
                            nextUserIds += chd.ChildNodes[0].Attributes["Value"].InnerText + ",";
                            nextUserNames += chd.ChildNodes[0].Attributes["Name"].InnerText + ",";
                            nextUserAccountType = chd.ChildNodes[0].Attributes["Type"].InnerText;
                        }
                    }
                }
                nextUserIds = nextUserIds.TrimEnd(',');
                nextUserNames = nextUserNames.TrimEnd(',');
                /// 设定指定流转节点的审批人员的信息. 
                List<ApprovalNodeContext> approvalNodeContexts = new List<ApprovalNodeContext>();
                ApprovalNodeContext specifiedApprovalNodeContext = new ApprovalNodeContext();
                specifiedApprovalNodeContext.Name = nextNodeName;

                if (nextUserAccountType != "ADAccount" && nextUserIds != "")//如果是组或者角色
                {
                    string[] grpIds = nextUserIds.Split(',') ;
                    List<ApprovalUnitContext> approvalUnitContexts = new List<ApprovalUnitContext>();
                    foreach (string groupId in grpIds)
                    {
                        string cou = @"select count(*) from (select distinct ParentDeptName from View_SysUserGroup where 
ChildDeptName=(Select Name from SysRole where RoleID='{0}')) a";
                        string sql = "";
                        int count = DataHelper.QueryValue<int>(string.Format(cou, groupId));
                        IList<EasyDictionary> lists = null;
                        //判断角色的唯一性,多部门角色需要对应到部门
                        if (count > 1)
                        {
                            sql = @"select distinct UserID,UserName Name from View_SysUserGroup where ChildDeptName in (Select Name from SysRole where RoleID='{0}') 
and (select top 1 Path+'.'+DeptId from View_SysUserGroup where UserID='{1}') like '%'+Path+'%'";
                            sql = string.Format(sql, groupId, task.OwnerId);
                            lists = DataHelper.QueryDictList(sql);
                        }
                        else if (count == 1)
                        {
                            sql = "select UserId,UserName Name from View_SysUserGroup where ChildDeptName=(Select Name from SysRole where RoleID='{0}')";
                            sql = string.Format(sql, groupId);
                            lists = DataHelper.QueryDictList(sql);
                        }
                        if (lists.Count == 0)
                        {
                            throw new Exception("缺少角色" + nextUserNames + "的人员!");
                        }
                        foreach (EasyDictionary ed in lists)
                        {
                            approvalUnitContexts.Add(new ApprovalUnitContext() { Approver = new Approver() { Value = ed["UserID"].ToString(), Name = ed["Name"].ToString() } });
                        }
                    }
                    specifiedApprovalNodeContext.ApprovalUnitContexts = approvalUnitContexts.ToArray();
                    approvalNodeContexts.Add(specifiedApprovalNodeContext);
                    result.SpecifiedApprovalNodeContexts = approvalNodeContexts.ToArray();
                }
                else if (nextUserIds.Trim() != "")
                {
                    LoadFromConfigString(specifiedApprovalNodeContext, nextUserIds, nextUserNames);
                    approvalNodeContexts.Add(specifiedApprovalNodeContext);
                    result.SpecifiedApprovalNodeContexts = approvalNodeContexts.ToArray();
                }
                /*List<ApprovalNodeContext> approvalNodeContexts = new List<ApprovalNodeContext>();

                ApprovalNodeContext specifiedApprovalNodeContext = new ApprovalNodeContext();
                specifiedApprovalNodeContext.Name = this.RequestData["RouteName"].ToString();

                string userIds = this.RequestData["UserIds"].ToString().TrimEnd(',');
                string userNames = this.RequestData["UserNames"].ToString().TrimEnd(',');
                LoadFromConfigString(specifiedApprovalNodeContext, userIds,userNames);
                approvalNodeContexts.Add(specifiedApprovalNodeContext);
                result.SpecifiedApprovalNodeContexts = approvalNodeContexts.ToArray();*/
            }

            return result;
        }

        private static ApprovalNodeContext LoadFromConfigString(ApprovalNodeContext approvalNodeContext, string userIds, string userNames)
        {
            //TODO: 需要扩展, 将config扩展为更复杂的格式.

            string[] accounts = userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] accountNames = userNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<ApprovalUnitContext> approvalUnitContexts = new List<ApprovalUnitContext>();


            for (int i = 0; i < accounts.Length; i++)
                approvalUnitContexts.Add(new ApprovalUnitContext() { Approver = new Approver() { Value = accounts[i], Name = accountNames[i] } });

            approvalNodeContext.ApprovalUnitContexts = approvalUnitContexts.ToArray();


            return approvalNodeContext;
        }
    }






    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     Runtime Version:4.0.30319.1
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------


    // 
    // This source code was auto-generated by xsd, Version=4.0.30319.1.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TaskContext
    {

        private TaskContextSwitchRule[] switchRulesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SwitchRule", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TaskContextSwitchRule[] SwitchRules
        {
            get
            {
                return this.switchRulesField;
            }
            set
            {
                this.switchRulesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TaskContextSwitchRule
    {

        private TaskContextSwitchRuleNextAction[] nextActionsField;

        private string nodeNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("NextAction", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TaskContextSwitchRuleNextAction[] NextActions
        {
            get
            {
                return this.nextActionsField;
            }
            set
            {
                this.nextActionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NodeName
        {
            get
            {
                return this.nodeNameField;
            }
            set
            {
                this.nodeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TaskContextSwitchRuleNextAction
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }
}
