<%@ Page Title="流程审批" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="TaskExecute.aspx.cs" Inherits="Aim.Portal.Web.WorkFlow.TaskExecute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/ext/ux/TabScrollerMenu.js" type="text/javascript"></script>

    <script src="TaskExecute.js" type="text/javascript"></script>

    <style type="text/css">
        body
        {
            color: #003399;
            background-color: #ddd;
        }
        .flow
        {
            background-image: url(/images/shared/arrow_cross.gif) !important;
        }
        .task
        {
            background-image: url(/images/shared/arrow_turnround1.gif) !important;
        }
        .iconop
        {
            background-image: url(/images/shared/message_edit.png) !important;
        }
        .app-header
        {
        }
        .app-header
        {
            font-family: verdana,arial,sans-serif;
            font-size: 20px;
            color: #15428B;
        }
        .ToolBar
        {
            background-color: red;
        }
    </style>

    <script type="text/javascript">
        var taskId = $.getQueryString({ ID: "TaskId", DefaultValue: "" });
        var EnumType = { '1': '已定', '0': '待定' };
        var routeData=[<%=NextStep %>];
        var flowInstanceId="<%=FlowInstanceId %>";
        var formUrl="<%=FormUrl %>";
        var flowDefineId="<%=FlowDefineId %>";
        
        
        function SubmitTask() {
            ShowMask();
            if ($("#id_SubmitState").val() == "") {
                Ext.MessageBox.alert('提示框', '请选择路由!');
                return;
            }
            SaveTask();
            if($("#id_SubmitState").val()=="结束"||AimState["NextNodeName"]=="结束")//流程结束
            {
                if(frameContent.onFinish)
                {
                    frameContent.onFinish(AimState["Task"]);
                }
                $.ajaxExec('submitTask', { FlowDefineId:flowDefineId,TaskId: taskId, Route: $("#id_SubmitStateH").val(),RouteName: $("#id_SubmitState").val(),UserIds:userIds,UserNames:userNames,NextNodeName:AimState["NextNodeName"] }, function() {ShowMessageAndClose("流程已结束!");});
                return;
            }
            else
            {
                if(frameContent.onSubmit)
                    frameContent.onSubmit(AimState["Task"]);
            }
            if(frameContent.onGiveUsers&&frameContent.onGiveUsers($("#id_SubmitState").val()).UserIds!="")//先取表单上的人员
            {
                var users = frameContent.onGiveUsers($("#id_SubmitState").val());
                userIds = users.UserIds;
                userNames = users.UserNames;
                var text = $("#id_SubmitState").val();
                $.ajaxExec('submitTask', { FlowDefineId:flowDefineId,TaskId: taskId, Route: $("#id_SubmitStateH").val(),RouteName: text,UserIds:userIds,UserNames:userNames,NextNodeName:AimState["NextNodeName"] }, function() {ShowMessageAndClose("提交成功!");});
            }
            else if(AimState["NextUserIds"]!=null&&AimState["NextUserIds"]!="")//流程配置里的人员和打回情况的人员,有打回(执行过的环节)优先取执行过的人提交
            {
                var text = $("#id_SubmitState").val();
                //if(frameContent.StartUserId=="")return; 选人就不能提交了
                if(frameContent.StartUserId&&frameContent.StartUserId!="")
                {
                    $.ajaxExec('submitTask', { FlowDefineId:flowDefineId,TaskId: taskId, Route: $("#id_SubmitStateH").val(),RouteName: text,UserIds:AimState["NextUserIds"],UserNames:AimState["NextUserNames"] ,UserType:AimState["UserType"],NextNodeName:AimState["NextNodeName"],StartUserId:frameContent.StartUserId,StartUserName:frameContent.StartUserName}, function() {ShowMessageAndClose("提交成功!");});
                }
                else
                {
                    $.ajaxExec('submitTask', { FlowDefineId:flowDefineId,TaskId: taskId, Route: $("#id_SubmitStateH").val(),RouteName: text,UserIds:AimState["NextUserIds"],UserNames:AimState["NextUserNames"] ,UserType:AimState["UserType"],NextNodeName:AimState["NextNodeName"]}, function() {ShowMessageAndClose("提交成功!");});
                }
            }
            else//手动选人
            {
                SelectUsers("/CommonPages/Select/UsrSelect/MUsrSelect.aspx?rtntype=array&seltype=multi");
            }
        }
        var userIds="";var userNames="";
        function SelectUsers(url, op, style) {
            userIds="";userNames = "";
            op = op || "r";
            style = style || "dialogWidth:750px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";

            var params = [];
            params[params.length] = "op=" + op;

            url = $.combineQueryUrl(url, params)
            rtn = window.showModalDialog(url, window, style);

            if (rtn && rtn.result) {
                if (rtn.result == 'success') {
                    var uids = [];
                    var usrs = rtn.data;
                    $.each(usrs, function() {
                        if (this.UserID) {
                            userIds = userIds+this.UserID+",";
                            userNames = userNames+this.Name+",";
                        }
                    });
                    if(userIds==""){HideMask();return;}
                    var text = $("#id_SubmitState").val().replace("("+$("#id_SubmitStateH").val()+")","")
                    $.ajaxExec('submitTask', { FlowDefineId:flowDefineId,TaskId: taskId, Route: $("#id_SubmitStateH").val(),RouteName: text,UserIds:userIds,UserNames:userNames ,NextNodeName:AimState["NextNodeName"]}, function() {ShowMessageAndClose("提交成功!");});
                }
            }
            else
                HideMask();
        }
        function SaveTask(tag)
        {
            ShowMask();
            if(frameContent.onSave)
            {
                //传入当前环节数据
                frameContent.onSave(AimState["Task"]);
            }
            $.ajaxExec('saveTask', { TaskId: taskId,Opinion:$("#textOpinion").val()}, function() { if(tag)ShowMessage("保存成功!");});
        }
        //表单控制下一环节路由
        //第一个参数为下一步路由,第二个参数为是否禁止重新选择路由
        function SetRoute(routeName,disabled)
        {
            var comb = Ext.getCmp("id_SubmitState");
            comb.setValue(routeName);
            if(disabled)comb.disabled=disabled;
        }
        function ShowMessage(msg)
        {
            HideMask();
            Ext.MessageBox.alert("操作",msg);
        }
        function ShowMessageAndClose(msg)
        {
            HideMask();
            Ext.MessageBox.alert("操作",msg,RefreshWindow);
        }
        function RefreshWindow()
        {
            if(window.opener&&window.opener.reloadPage)
            {
                window.opener.reloadPage();
            }
            else if(window.opener)
            {
                window.opener.location.reload();
            }
            window.close();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="divNorth" class="x-hide-display">
    </div>
    <div id="west" class="x-hide-display">
    </div>
</asp:Content>
