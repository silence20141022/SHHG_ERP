<%@ Page Title="其他付款申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.master"
    AutoEventWireup="true" CodeBehind="OtherPayBillEdit.aspx.cs" Inherits="Aim.Examining.Web.DailyManager.OtherPayBillEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var op = "";
        function onPgLoad() {
            setPgUI();
            op = $.getQueryString({ "ID": "op" });
            if (op == "c") {
                $("#PayBillNo").val(AimState["PayBillNo"]);
                $("#PayType").val("其他付款");
                $("#CreateName").val(AimState["UserInfo"].Name);
            }
        }
        function setPgUI() {
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }
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
            日常管理=》 其他付款申请</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr style="height: 35px">
                    <td class="aim-ui-td-caption">
                        付款编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PayBillNo" name="PayBillNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        付款类型
                    </td>
                    <td class="aim-ui-td-data" style="width: 31%;">
                        <input id="PayType" name="PayType" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        发票号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InvoiceNo" name="InvoiceNo" />
                    </td>
                    <td class="aim-ui-td-caption">
                        发票金额(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InvoiceAmount" name="InvoiceAmount" />
                    </td>
                </tr>
                <tr style="height: 35px">
                    <td class="aim-ui-td-caption">
                        申请金额（￥）
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ShouldPayAmount" name="ShouldPayAmount" />
                    </td>
                    <td class="aim-ui-td-caption">
                        申请人
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateName" name="CreateName" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        申请事由
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 86.5%; height: 50px"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-button-panel" colspan="4">
                    <a id="btnSubmit" class="aim-ui-button submit">提交</a> <a id="btnCancel" class="aim-ui-button cancel">
                        取消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
