<%@ Page Title="编辑" Language="C#" MasterPageFile="~/Masters/Ext/formpage.master"
    AutoEventWireup="true" CodeBehind="FrmSetPrintCount.aspx.cs" Inherits="Aim.Examining.Web.FrmSetPrintCount" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            $("#txtcount").val(AimState["count"]);
            
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            AimFrm.submit("setcount", { "count": $("#txtcount").val() }, null, SubFinish);
        }

        function SubFinish(args) {
            window.close();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            每页出库单打印数量设置</h1>
    </div>
    <div align="center">
    <br />
        <table class="aim-ui-table-edit">
            <tr width="100%">
                <td class="aim-ui-td-caption" style="width:150px;">
                    每页出库单打印数量
                </td>
                <td class="aim-ui-td-data">
                    <input id="txtcount" name="txtcount" class="validate[required custom[onlyInteger]]" />
                </td>
            </tr>
        </table>
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-button-panel">
                    <a id="btnSubmit" style="height: 20px; width: 50px; background-color: #7f99be; line-height: 20px;
                        cursor: pointer; color: #FFFFFF;">提 交</a> <a id="btnCancel" style="height: 20px;
                            width: 50px; background-color: #7f99be; line-height: 20px; cursor: pointer; color: #FFFFFF;">
                            取 消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
