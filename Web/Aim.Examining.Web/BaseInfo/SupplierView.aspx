<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="SupplierView.aspx.cs" Inherits="Aim.Examining.Web.SupplierView" %>

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
            //FormValidationBind('btnSubmit', SuccessSubmit);

            //            $("#btnCancel").click(function() {
            //                window.close();
            //            });
        }

        //验证成功执行保存方法
        //        function SuccessSubmit() {
        //            AimFrm.submit(pgAction, { "IsDefault": $("#IsDefault").attr("checked") ? "on" : "off" }, null, SubFinish);
        //        }

        //        function SubFinish(args) {
        //            RefreshClose();
        //        }
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
                <tr>
                    <td class="aim-ui-td-caption" style="width: 30%;">
                        供应商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierName" name="SupplierName" readonly="readonly" />
                    </td>
                    <td style="width: 30%">
                        供应商地址
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierAddress" name="SupplierAddress" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        固定电话
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="FixedTelephone" name="FixedTelephone" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        手机号码
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Mobile" name="Mobile" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        传真
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Fax" name="Fax" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        联系人
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ContactPerson" name="ContactPerson" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        E_Mail
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Email" name="Email" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        默认供应商
                    </td>
                    <td class="aim-ui-td-data">
                        <input type="checkbox" id="IsDefault" name="IsDefault" readonly="readonly" />是
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        开户行
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Bank" name="Bank" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        账号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Account" name="Account" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        交易币种
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="MoneyType" name="MoneyType" readonly="readonly" />
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
                        <input id="MailAddress" name="MailAddress" style="width: 100%" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 100%" readonly="readonly"></textarea>
                    </td>
                </tr>
                <tr width="100%">
                    <td class="aim-ui-td-caption">
                        创建人
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateName" name="CreateName" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        创建时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateTime" name="CreateTime" readonly="readonly" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
