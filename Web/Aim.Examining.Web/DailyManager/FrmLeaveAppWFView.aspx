<%@ Page Title="请假申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmLeaveAppWFView.aspx.cs" Inherits="Aim.Examining.Web.FrmLeaveAppWFView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, subContentPanel, sngrid;

        function onPgLoad() {

            setPgUI();
        }

        function setPgUI() {
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }
    </script>

    <script language="javascript">
        /**********************************************WorkFlow Function Start**************************/
        var permission = {};
        //这里依次统一添加各环节的控件权限
        permission.初审 = { ReadOnly: "", Hidden: "BeginTime" };
        permission.复审 = { ReadOnly: "", Hidden: "EndTime" };

        var StartUserId = "";
        var StartUserName = "";
        function InitUIForFlow() {

            StartUserId = $("#RequestUserId").val();
            StartUserName = $("#RequestUserName").val();
            if (window.parent.AimState["Task"])
                var taskName = window.parent.AimState["Task"].ApprovalNodeName;

            $("#btnSubmit").hide();
            $("#btnCancel").hide();

            ///控制下一步路由
            if (taskName == "确认发布内容") {
                //SetRoute("公司领导",true);//第一个参数为下一步路由,第二个参数为是否禁止重新选择路由
            }

            if (eval("permission." + taskName)) {
                //只读
                var read = eval("permission." + taskName).ReadOnly;
                for (var i = 0; i < read.split(',').length; i++) {
                    var id = read.split(',')[i];
                    if (document.getElementById(id))
                        document.getElementById(id).readOnly = true;
                }
                //隐藏
                var vis = eval("permission." + taskName).Hidden;
                for (var i = 0; i < vis.split(',').length; i++) {
                    var id = vis.split(',')[i];
                    if (document.getElementById(id))
                        document.getElementById(id).style.display = "none";
                }
            }
        }
        //保存流程和提交流程时触发
        function onSave(task) {
            SuccessSubmit();
            //AimFrm.submit(pgAction, { param: "test" }, null, function() { });
        }
        //提交流程时触发
        function onSubmit(task) {

        }
        //获取下一环节用户
        function onGiveUsers(nextName) {
            var users = { UserIds: "", UserNames: "" };
            switch (nextName) {
                case "提交审批":
                    //users.UserIds = $("#PostUserId").val();
                    //users.UserNames = $("#PostUserName").val();
                    break;
            }
            return users;
        }
        //流程结束时触发
        function onFinish(task) {
            jQuery.ajaxExec('submitfinish', { "state": "End", "id": id, "ApprovalState": window.parent.document.getElementById("id_SubmitState").value }, function() {
                RefreshClose();
            });
            //AimFrm.submit(pgAction, { param: "finish" }, null, function() { });
        }
        //第一个参数为下一步路由,第二个参数为是否禁止重新选择路由
        function SetRoute(name, flag) {
            window.parent.SetRoute("公司领导", flag);
        }
        /*****************************************************WorkFlow Function End****************************/
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            请假申请</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tr style="display: none">
                <td>
                    <input id="Id" name="Id" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    编号
                </td>
                <td class="aim-ui-td-data">
                    <input id="Number" name="Number" disabled="disabled" />
                </td>
                <td class="aim-ui-td-caption">
                    请假类型
                </td>
                <td class="aim-ui-td-data">
                    <input id="LeaveType" name="LeaveType" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    开始时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="BeginTime" name="BeginTime" disabled="disabled" />
                </td>
                <td class="aim-ui-td-caption">
                    结束时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="EndTime" name="EndTime" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    请假天数
                </td>
                <td class="aim-ui-td-data">
                    <input id="LeaveDays" disabled="disabled" name="LeaveDays" />天
                </td>
                <td class="aim-ui-td-caption">
                    请假人
                </td>
                <td class="aim-ui-td-data">
                    <input disabled="disabled" id="LeaveUser" name="LeaveUser" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    部门
                </td>
                <td class="aim-ui-td-data">
                    <input disabled="disabled" id="DeptName" name="DeptName" />
                </td>
                <td class="aim-ui-td-caption">
                    申请日期
                </td>
                <td class="aim-ui-td-data">
                    <input id="LeaveTime" name="LeaveTime" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    请假原因
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <textarea id="Reason" name="Reason" cols="65" rows="5" disabled="disabled" ></textarea>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
