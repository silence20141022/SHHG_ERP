<%@ Page Title="ϵͳͼ��" Language="C#" MasterPageFile="~/Masters/Ext/formpage.master" AutoEventWireup="true" CodeBehind="SysGraphEdit.aspx.cs" Inherits="Aim.Portal.Web.Modules.SiteMag.SysGraphEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EnumType = { 1: "����", 0: "ͣ��" };
        
        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            if (pgOperation == "c") {
                $("#CreaterName").val(AimState.CreaterName);
                $("#CreatedDate").val(AimState.CreatedDate);
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
            RefreshClose();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header"><h1>ϵͳͼ��ά��</h1></div>
    
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="GraphID" name="GraphID" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        ����
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Code" style=" width:200"  name="Code" class="validate[required]" />
                    </td>
                    <td class="aim-ui-td-caption">
                        ����
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Title" style=" width:200"  name="Title" class="validate[required]" />
                    </td>
                </tr>
               <tr>
                    <td class="aim-ui-td-caption">
                        �ļ�
                    </td>
                    <td colspan=3 class="aim-ui-td-data">
                        <input id="FileID" name="FileID" aimctrl='file' mode="single" value="" class="validate[required]" />
                    </td>
                    </tr>
                <tr>
                      <td class="aim-ui-td-caption">
                        ��ϸ˵��
                    </td>
                    <td  colspan=3 class="aim-ui-td-data">
                        <textarea id="Description" rows="5"  style=" width:98%"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                       ������
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreaterName" disabled name="CreaterName" class="text ui-widget-content" />
                    </td>
                     <td class="aim-ui-td-caption">
                       ��������
                    </td>
                     <td class="aim-ui-td-data">
                        <input id="CreatedDate" disabled name="CreatedDate" class="text ui-widget-content" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnSubmit" class="aim-ui-button submit">�ύ</a>
                        <a id="btnCancel" class="aim-ui-button cancel">ȡ��</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
