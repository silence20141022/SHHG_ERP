<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.master" AutoEventWireup="true"
    CodeBehind="FrmPortalEdit.aspx.cs" Inherits="Aim.Examining.Web.Message.FrmPortalEdit"
    Title="�Ż�ά��" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        var TypeEnum = { '��˾�Ż�': '��˾�Ż�', '�����Ż�': '�����Ż�' };

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateName").val(AimState.UserInfo.Name);
                $("#CreateId").val(AimState.UserInfo.UserID);
                $("#CreateTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
            }

            //�󶨰�ť��֤
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //��֤�ɹ�ִ�б��淽��
        function SuccessSubmit() {
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            alert("OK");
            //RefreshClose();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>�Ż�ά��</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td colspan="3">
                        <input id="CreateId" name="CreateId" />
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        ����
                    </td>
                    <td class="aim-ui-td-data" style="width: 30%;">
                        <input id="Name" name="Name" class="validate[required]" style="width:200px;" />
                    </td>
                    <td class="aim-ui-td-caption">
                        ���
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="Type" name="Type" aimctrl='select' enum="TypeEnum" class="validate[required]" style="width:200px;" ></select>
                    </td>
                </tr>
                <tr width="100%">
                    <td class="aim-ui-td-caption">
                        ����˳��
                    </td>
                    <td class="aim-ui-td-data" colspan="3" >
                        <input id="Sort" name="Sort" class="validate[required custom[onlyInteger]]" style="width:200px;" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">����</td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Description" name="Description" style="width:80%; height:80px;" ></textarea>
                    </td>
                </tr>
                <tr width="100%" style="display:none;">
                    <td class="aim-ui-td-caption">
                        ������
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateName" name="CreateName" />
                    </td>
                    <td class="aim-ui-td-caption">
                        ��������
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateTime" name="CreateTime" dateonly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnSubmit" class="aim-ui-button submit">�ύ</a> <a id="btnCancel" class="aim-ui-button cancel">
                            ȡ��</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
