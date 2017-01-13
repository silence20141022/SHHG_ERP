using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Aim.Data;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.WorkFlow;
using Aim.WorkFlow.WFService;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Aim.Portal.Web.WorkFlow
{
    public partial class TaskExecute : BasePage
    {
        public string NextStep = "";
        public string FlowInstanceId = "";
        public string FormUrl = "";
        public string FlowDefineId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.RequestActionString.ToLower())
            {
                case "submittask":
                    Aim.WorkFlow.WFService.Task task = Aim.WorkFlow.WorkFlow.ServiceClient.GetTaskByTaskId(this.RequestData["TaskId"].ToString());
                    Aim.WorkFlow.WorkFlow.SubmitTask("", task.WorkflowInstanceID, task.BookmarkName, GetApprovalResult(task));
                    PageState.Add("message", "提交成功!");
                    break;
                case "savetask":
                    Aim.WorkFlow.Task tas = Aim.WorkFlow.Task.Find(this.RequestData["TaskId"].ToString());
                    tas.Description = this.RequestData["Opinion"].ToString();
                    tas.SaveAndFlush();
                    break;
                case "getusers":
                    GetNextUsers(this.RequestData["TemplateId"].ToString(), this.RequestData["FlowInstanceId"].ToString(), this.RequestData["Name"].ToString(), this.RequestData["CurrentName"].ToString());
                    break;
                case "getbackusers":
                    Aim.WorkFlow.Task[] tks = Aim.WorkFlow.Task.FindAllByProperties(Aim.WorkFlow.Task.Prop_WorkflowInstanceID, this.RequestData["FlowInstanceId"].ToString(), Aim.WorkFlow.Task.Prop_ApprovalNodeName, this.RequestData["TaskName"].ToString());
                    if (tks != null && tks.Length == 1)//打回情况一个人的时候有效,多人的话,还是从之前配置里取
                    {
                        this.PageState.Add("NextUserIds", tks[0].OwnerId);
                        this.PageState.Add("NextUserNames", tks[0].Owner);
                    }
                    break;
                default:
                    if (this.RequestData["TaskId"] != null && !string.IsNullOrEmpty(this.RequestData["TaskId"].ToString()))
                    {
                        Aim.WorkFlow.Task fTask = Aim.WorkFlow.Task.Find(this.RequestData["TaskId"].ToString());
                        Aim.WorkFlow.WorkflowInstance instance = WorkflowInstance.Find(fTask.WorkflowInstanceID);
                        this.PageState.Add("InstanceId", fTask.WorkflowInstanceID);
                        this.PageState.Add("TemplateId", instance.WorkflowTemplateID);
                        FlowInstanceId = instance.ID;
                        FormUrl = instance.RelateUrl;
                        FlowDefineId = instance.WorkflowTemplateID;
                        Title = fTask.WorkFlowName + "->" + fTask.ApprovalNodeName;
                        XmlSerializer xs = new XmlSerializer(typeof(TaskContext));
                        if (!string.IsNullOrEmpty(fTask.Context))
                        {
                            /*Aim.WorkFlow.WorkflowTemplate temp = Aim.WorkFlow.WorkflowTemplate.Find(instance.WorkflowTemplateID);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(temp.XAML);
                            XmlElement root = doc.DocumentElement;
                            string nameSpace = root.NamespaceURI;
                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                            nsmgr.AddNamespace("ns", nameSpace);
                            nsmgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                            nsmgr.AddNamespace("bwa", "clr-namespace:BPM.WF.Activities;assembly=BPM.WF");
                            XmlNode currentNode = root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + fTask.ApprovalNodeName + "')]", nsmgr);*/
                            StringReader sr = new StringReader(fTask.Context);
                            TaskContext content = xs.Deserialize(sr) as TaskContext;
                            if (content.SwitchRules.Length > 0)
                            {
                                TaskContextSwitchRuleNextAction[] arrs = content.SwitchRules[0].NextActions;
                                string comboxdataText = "['{0}','{1}'],";
                                if (arrs.Length > 0)
                                {
                                    int first = 0;
                                    foreach (TaskContextSwitchRuleNextAction ar in arrs)
                                    {
                                        //GetNextRoute(currentNode, nsmgr, ar.Name)
                                        NextStep += string.Format(comboxdataText, ar.Name, ar.Name);
                                        if (first == 0)
                                            GetNextUsers(instance.WorkflowTemplateID, fTask.WorkflowInstanceID, ar.Name, fTask.ApprovalNodeName);
                                        first++;
                                    }
                                }
                                else
                                    NextStep += string.Format("['','{0}'],", "结束");
                            }
                            else
                                NextStep += string.Format("['','{0}'],", "结束");
                        }
                        else
                            NextStep += string.Format("['','{0}'],", "结束");
                        NextStep = NextStep.TrimEnd(',');
                        Aim.WorkFlow.Task[] tasks = Aim.WorkFlow.Task.FindAllByProperty("CreatedTime", "WorkflowInstanceID", fTask.WorkflowInstanceID);
                        this.PageState.Add("Tasks", JsonHelper.GetJsonString(tasks));
                        this.PageState.Add("Task", fTask);
                    }
                    break;
            }

        }

        public string GetNextRoute(XmlNode node, XmlNamespaceManager nsmgr, string routeString)
        {
            XmlNode nodeOr = node.NextSibling.SelectSingleNode("ns:FlowSwitch/ns:FlowStep[@x:Key='" + routeString + "']", nsmgr);
            if (nodeOr != null)
            {
                node = nodeOr.SelectSingleNode("bwa:Approval_Node", nsmgr);
                string config = System.Web.HttpUtility.HtmlDecode(node.Attributes["ApprovalNodeConfig"].InnerXml);
                XmlDocument docC = new XmlDocument();
                docC.LoadXml(config);
                return docC.DocumentElement.Attributes["Name"].InnerText.ToString();
            }
            else
                return routeString;

        }

        public void GetNextUsers(string templateId, string instanctId, string nextName, string currentName)
        {
            Aim.WorkFlow.WorkflowTemplate temp = Aim.WorkFlow.WorkflowTemplate.Find(templateId);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(temp.XAML);
            XmlElement root = doc.DocumentElement;
            string nameSpace = root.NamespaceURI;
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", nameSpace);
            nsmgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            nsmgr.AddNamespace("bwa", "clr-namespace:BPM.WF.Activities;assembly=BPM.WF");

            string current = "ApprovalNode Name=\"" + currentName + "\"";
            XmlNode currentNode = root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + current + "')]", nsmgr);
            //XmlNode node = root.SelectSingleNode("//*[@x:Key='" + nextName + "']", nsmgr);
            XmlNode node = currentNode.NextSibling.SelectSingleNode("ns:FlowSwitch/ns:FlowStep[@x:Key='" + nextName + "']", nsmgr);
            string nextUserIds = "";
            string nextUserNames = "";
            string nextUserAccountType = "";
            string nextNodeName = "";
            string content = "ApprovalNode Name=\"" + nextName + "\"";
            if (node != null)//switch路由
            {
                node = node.SelectSingleNode("bwa:Approval_Node", nsmgr);
                if (node != null)
                {
                    string config = System.Web.HttpUtility.HtmlDecode(node.Attributes["ApprovalNodeConfig"].InnerXml);
                    SetNextUsers(config, ref nextUserIds, ref nextUserNames, ref nextUserAccountType, ref nextNodeName);
                }
            }
            else if (root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + content + "')]", nsmgr) != null)//直接路由
            {
                string config = System.Web.HttpUtility.HtmlDecode(root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + content + "')]", nsmgr).Attributes["ApprovalNodeConfig"].InnerXml);
                SetNextUsers(config, ref nextUserIds, ref nextUserNames, ref nextUserAccountType, ref nextNodeName);
            }
            //如果是打回的情况
            XmlNode nextNode = root.SelectSingleNode("//*[contains(@ApprovalNodeConfig,'" + nextName + "')]", nsmgr);
            if (currentNode.ParentNode.SelectSingleNode("ns:FlowStep.Next/ns:FlowSwitch/x:Reference/x:Key", nsmgr) != null)//switch情况的打回
            {
                if (currentNode.ParentNode.SelectSingleNode("ns:FlowStep.Next/ns:FlowSwitch/x:Reference[x:Key/text()='" + nextName + "']", nsmgr) != null)
                {
                    string reference = currentNode.ParentNode.SelectSingleNode("ns:FlowStep.Next/ns:FlowSwitch/x:Reference[x:Key/text()='" + nextName + "']", nsmgr).ChildNodes[0].InnerText;
                    nextNode = root.SelectSingleNode("//*[@x:Name='" + reference + "']", nsmgr);
                    string config = System.Web.HttpUtility.HtmlDecode(nextNode.SelectSingleNode("bwa:Approval_Node", nsmgr).Attributes["ApprovalNodeConfig"].InnerXml);
                    XmlDocument docC = new XmlDocument();
                    docC.LoadXml(config);
                    nextNodeName = docC.DocumentElement.Attributes["Name"].InnerText.ToString();
                    SetNextUsers(instanctId, nextNodeName, ref nextUserIds, ref nextUserNames, ref nextUserAccountType);
                }
            }
            else if (currentNode.SelectSingleNode("parent::*/ns:FlowStep.Next/x:Reference", nsmgr) != null)//正常路由打回
            {
                string refer = currentNode.SelectSingleNode("parent::*/ns:FlowStep.Next/x:Reference", nsmgr).InnerText;
                if (refer == nextNode.ParentNode.Attributes["x:Name"].Value)
                {
                    SetNextUsers(instanctId, nextName, ref nextUserIds, ref nextUserNames, ref nextUserAccountType);
                }
                nextNodeName = nextName;
            }
            this.PageState.Add("NextUserIds", nextUserIds.TrimEnd(','));
            this.PageState.Add("NextUserNames", nextUserNames.TrimEnd(','));
            this.PageState.Add("NextUserType", nextUserAccountType);
            this.PageState.Add("NextNodeName", nextNodeName);
        }
        public void SetNextUsers(string instanceId, string NodeName, ref string userIds, ref string userNames, ref string accountType)
        {
            Aim.WorkFlow.Task[] tks = Aim.WorkFlow.Task.FindAllByProperties(Aim.WorkFlow.Task.Prop_WorkflowInstanceID, instanceId, Aim.WorkFlow.Task.Prop_ApprovalNodeName, NodeName);
            foreach (Aim.WorkFlow.Task tk in tks)
            {
                if (userIds.IndexOf(tk.OwnerId) >= 0) continue;
                userIds += tk.OwnerId + ",";
                userNames += tk.Owner + ",";
                accountType = "ADAccount";
            }
        }
        public void SetNextUsers(string config, ref string userIds, ref string userNames, ref string accountType, ref string nextNodeName)
        {
            XmlDocument docC = new XmlDocument();
            docC.LoadXml(config);
            nextNodeName = docC.DocumentElement.Attributes["Name"].InnerText.ToString();
            if (docC.DocumentElement.SelectSingleNode("ApprovalUnits") != null && docC.DocumentElement.SelectSingleNode("ApprovalUnits").ChildNodes.Count > 0)
            {
                XmlNodeList list = docC.DocumentElement.SelectSingleNode("ApprovalUnits").ChildNodes;
                foreach (XmlNode chd in list)
                {
                    userIds += chd.ChildNodes[0].Attributes["Value"].InnerText + ",";
                    userNames += chd.ChildNodes[0].Attributes["Name"].InnerText + ",";
                    accountType = chd.ChildNodes[0].Attributes["Type"].InnerText;
                }
            }
        }

        public ApprovalResult GetApprovalResult(Aim.WorkFlow.WFService.Task task)
        {
            ApprovalResult result = new ApprovalResult()
            {
                Task = task,
                TaskId = this.RequestData["TaskId"].ToString(),

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
            if (this.RequestData["Route"] != null && this.RequestData["Route"].ToString() != "")
            {
                result.SwitchRules = new KeyValuePair_V2[] 
                    { 
                        new KeyValuePair_V2() 
                        { 
                            Key = task.ApprovalNodeName, 
                            Value = this.RequestData["Route"].ToString() 
                        } 
                    };
                /// 设定指定流转节点的审批人员的信息. 
                List<ApprovalNodeContext> approvalNodeContexts = new List<ApprovalNodeContext>();
                ApprovalNodeContext specifiedApprovalNodeContext = new ApprovalNodeContext();
                specifiedApprovalNodeContext.Name = this.RequestData["NextNodeName"] == null ? "" : this.RequestData["NextNodeName"].ToString();

                if (this.RequestData["UserType"] != null)
                {
                    if (this.RequestData["UserType"].ToString() != "ADAccount" && this.RequestData["UserIds"].ToString() != "")//如果是组或者角色
                    {
                        string[] grpIds = this.RequestData["UserIds"].ToString().Split(',');
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
                                if (this.RequestData.Get("StartUserId") != null && this.RequestData.Get<string>("StartUserId") != "")
                                    sql = string.Format(sql, groupId, this.RequestData.Get("StartUserId"));
                                else
                                    sql = string.Format(sql, groupId, this.UserInfo.UserID);
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
                                throw new Exception("缺少角色" + this.RequestData["UserNames"] + "的人员!");
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
                }
                else if (this.RequestData["UserIds"] != null && this.RequestData["UserIds"].ToString().Trim() != "")
                {
                    string userIds = this.RequestData["UserIds"].ToString().TrimEnd(',');
                    string userNames = this.RequestData["UserNames"].ToString().TrimEnd(',');
                    LoadFromConfigString(specifiedApprovalNodeContext, userIds, userNames);
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

        private ApprovalNodeContext LoadFromConfigString(ApprovalNodeContext approvalNodeContext, string userIds, string userNames)
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
