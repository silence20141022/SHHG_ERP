<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherPayBillEdit.aspx.cs"
    Inherits="Aim.Examining.Web.LogisticsManagement.OtherPayBillEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script type="text/javascript">
        function RefreshClose() {
            if (window.opener) {
                window.opener.location.reload();
            }
            window.close();
        }</script>

    <title></title>
    <style type="text/css">
        .ButtonTd
        {
            text-align: center;
            background: url(../Images/buttonBg.gif) no-repeat;
            height: 22px;
            width: 85px;
        }
        .table_paddingleft
        {
            border-collapse: collapse;
            border-bottom: 0;
            border-right: 0;
        }
        .table_paddingleft td
        {
            border: 1px solid #ccc;
            padding-left: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager">
    </asp:ScriptManager>
    <div>
        <fieldset style="margin-bottom: 10px">
            <legend style="margin-left: 20px; font-family: Verdana,Arial, Helvetica, sans-serif;
                font-size: 13px; color: Blue; font-weight: bolder">
                <img src="../images/shared/arrow_down.gif" alt="" />物流付款单</legend>
            <table width="100%" style="margin-top: 5px; margin-bottom: 5px; font-family: Verdana,Arial, Helvetica, sans-serif;
                font-size: 12px; color: Black; background-color: #FFFFFF; border-collapse: collapse;">
                <tr style="height: 30px">
                    <td align="right" style="width: 25%">
                        付款编号：
                    </td>
                    <td style="width: 25%">
                        <telerik:RadTextBox ID="rtbPayBillNo" runat="server" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td align="right" style="width: 25%">
                        物流公司：
                    </td>
                    <td>
                        <telerik:RadTextBox ID="rtbLogisticsCompanyName" runat="server">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td align="right" style="width: 25%">
                        付款类型：
                    </td>
                    <td style="width: 25%">
                        <telerik:RadComboBox ID="rcbPayType" runat="server" Font-Size="Small" Width="128px"
                            Skin="Web20">
                            <Items>
                                <telerik:RadComboBoxItem Text="物流付款" Value="物流付款" />
                                <telerik:RadComboBoxItem Text="报销付款" Value="报销付款" />
                            </Items>
                            <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                        </telerik:RadComboBox>
                    </td>
                    <td align="right" style="width: 25%">
                        应付金额：
                    </td>
                    <td style="width: 25%">
                        <telerik:RadNumericTextBox ID="rntbShouldPayAmount" runat="server" ReadOnly="true">
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td align="right">
                        物流发票号：
                    </td>
                    <td>
                        <telerik:RadTextBox ID="rtbInvoiceNo" runat="server" MaxLength="8">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rtbInvoiceNo">*</asp:RequiredFieldValidator>
                    </td>
                    <td align="right">
                        发票金额：
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="rntbInvoiceAmount" runat="server">
                        </telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="rntbInvoiceAmount">*</asp:RequiredFieldValidator>
                        <%--  <asp:CompareValidator ID="cv1" runat="server" ControlToCompare="rntbShouldPayAmount"
                            ControlToValidate="rntbInvoiceAmount"></asp:CompareValidator>--%>
                    </td>
                </tr>
                <tr style="height: 30px; display: none">
                    <td align="right">
                        物流单IDS：
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="rtbInterfaceArray" runat="server" MaxLength="360" Width="86%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td align="right">
                        备注 ：
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="rtbRemark" runat="server" TextMode="MultiLine" Height="35px" Width="86%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div style="text-align: right; margin-top: 10px; margin-bottom: 10px">
        <table width="100%">
            <tr>
                <td style="padding-right: 25px">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="SingleParagraph"
                        HeaderText="带*号项为必填项" />
                </td>
                <td class="ButtonTd">
                    <asp:LinkButton Width="85px" Font-Underline="false" Height="16px" ID="lbtSave" runat="server"
                        Font-Bold="true" Font-Size="11" OnClick="lbtSave_Click">保  存</asp:LinkButton>
                </td>
                <td class="ButtonTd">
                    <asp:LinkButton Width="85px" Font-Underline="false" Height="16px" ID="lbtClose" runat="server"
                        OnClientClick="window.close();" Font-Bold="true" Font-Size="11">关  闭</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
