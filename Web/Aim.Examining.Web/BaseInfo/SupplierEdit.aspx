<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="SupplierEdit.aspx.cs" Inherits="Aim.Examining.Web.SupplierEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateName").val(AimState.UserInfo.Name);
                $("#CreateTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
            }

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            AimFrm.submit(pgAction, { "IsDefault": $("#IsDefault").attr("checked") ? "on" : "off", MoneyType: $("#Money").val() }, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            供应商信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption" style="width: 30%;">
                        供应商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierName" name="SupplierName" class="validate[required]" />
                    </td>
                    <td style="width: 30%">
                        供应商地址
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierAddress" name="SupplierAddress" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        固定电话
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="FixedTelephone" name="FixedTelephone" />
                    </td>
                    <td class="aim-ui-td-caption">
                        手机号码
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Mobile" name="Mobile" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        传真
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Fax" name="Fax" />
                    </td>
                    <td class="aim-ui-td-caption">
                        联系人
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ContactPerson" name="ContactPerson" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        E_Mail
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Email" name="Email" />
                    </td>
                    <td class="aim-ui-td-caption">
                        默认供应商
                    </td>
                    <td class="aim-ui-td-data">
                        <input type="checkbox" id="IsDefault" name="IsDefault" />是
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        开户行
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Bank" name="Bank" />
                    </td>
                    <td class="aim-ui-td-caption">
                        账号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Account" name="Account" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        交易币种
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="MoneyType" name="MoneyType" relateid="txtuserid" style="width: 127px;"
                            class="validate[required]" popurl="/CommonPages/Select/ExchangeRateSelect.aspx?seltype=single"
                            popparam="ExchangeRateId:Id;MoneyType:MoneyType;Symbo:Symbo" popstyle="width=800,height=400" />
                        <%--<input aimctrl='moneytype' required="true" id="MoneyType" name="MoneyType" relateid="ExchangeRateId"
                            symbo="Symbo" class="validate[required]" />--%>
                        <input id="ExchangeRateId" name="ExchangeRateId" type="hidden" />
                    </td>
                    <td class="aim-ui-td-caption">
                        币种符号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Symbo" name="Symbo" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        邮寄地址
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <input id="MailAddress" name="MailAddress" style="width: 100%" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 100%"></textarea>
                    </td>
                </tr>
                <tr width="100%">
                    <td class="aim-ui-td-caption">
                        录入人
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateName" name="CreateName" />
                    </td>
                    <td class="aim-ui-td-caption">
                        录入日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateTime" name="CreateTime" dateonly="true" />
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
