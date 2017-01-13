<%@ Page Title="请假申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmLeaveAppEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmLeaveAppEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, subContentPanel, sngrid;

        function onPgLoad() {

            setPgUI();

            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateTime").val(new Date().format("Y-m-d"));
                $("#WriteUser").val(AimState.UserInfo.Name);
                $("#DeptName").val(AimState["ReleDepartment"]);
            }
        }

        function setPgUI() {
            if (pgOperation == "c") {
                $("#CreaterName").val(AimState.UserInfo.Name);
                $("#CreatedDate").val(AimState.SystemInfo.Name);
            }

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }
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
                    <input id="Number" name="Number" disabled="disabled" value="自动生成" />
                </td>
                <td class="aim-ui-td-caption">
                    请假类型
                </td>
                <td class="aim-ui-td-data">
                    <select id="LeaveType" name="LeaveType" aimctrl='select' enumdata="AimState['LeaveType']"
                        style="width: 152px;" class="validate[required]">
                    </select>
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    开始时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="BeginTime" name="BeginTime" aimctrl="date" class="validate[required]" />
                </td>
                <td class="aim-ui-td-caption">
                    结束时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="EndTime" name="EndTime" aimctrl="date" class="validate[required]" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    请假天数
                </td>
                <td class="aim-ui-td-data">
                    <input id="LeaveDays" name="LeaveDays" class="validate[required custom[onlyNumber]]" />天
                </td>
                <td class="aim-ui-td-caption">
                    请假人
                </td>
                <td class="aim-ui-td-data">
                    <%--<input disabled="disabled" id="LeaveUser" name="LeaveUser" />--%>
                    <input aimctrl='user' required="true" id="LeaveUser" name="LeaveUser" />
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
                    填单人
                </td>
                <td class="aim-ui-td-data">
                    <input disabled="disabled" id="WriteUser" name="WriteUser" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    申请日期
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <input id="CreateTime" name="CreateTime" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    请假原因
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <textarea id="Reason" name="Reason" cols="65" rows="5"></textarea>
                </td>
            </tr>
        </table>
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-button-panel">
                    <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">
                        取消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
