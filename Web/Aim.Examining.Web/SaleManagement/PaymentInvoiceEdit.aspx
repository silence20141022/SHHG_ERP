<%@ Page Title="应收款" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="PaymentInvoiceEdit.aspx.cs" Inherits="Aim.Examining.Web.PaymentInvoiceEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var cid;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            if (pgOperation == "c" || pgOperation == "cs") {
                $("#ReceivablesTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
            }

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            //if (confirm("收款信息保存后不能修改及删除，确定？")) {
            AimFrm.submit(pgAction, {}, null, SubFinish);
            //}
        }

        function SubFinish(args) {
            //RefreshClose();
            window.opener.location = window.opener.location;
            window.close();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            应收款</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="CId" name="CId" />
                        <input id="CorrespondState" name="CorrespondState" />
                        <input id="CorrespondInvoice" name="CorrespondInvoice" />
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        收款编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Number" name="Number" class="validate[required]" />
                    </td>
                    <td class="aim-ui-td-caption">
                        收款名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Name" name="Name" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        收款金额
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Money" name="Money" value="0" class="validate[required custom[onlyNumber]]" />元
                    </td>
                    <td class="aim-ui-td-caption">
                        收款方式
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="PayType" name="PayType" aimctrl='select' style="width: 152px;" class="validate[required]"
                            enumdata="AimState['PayType']">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        收款时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ReceivablesTime" aimctrl="date" name="ReceivablesTime" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 90%; height: 60px"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnSubmit" class="aim-ui-button submit">提交</a> <a id="btnCancel" class="aim-ui-button cancel">
                            取消</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
