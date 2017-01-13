<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CombineSplitPrintContent.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.CombineSplitPrintContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>出库单</title>
    <style type="text/css">
        #dtmain td
        {
            border: solid #000 1px;
        }
        td
        {
            font-size: 14px;
            height: 22px;
        }
        b
        {
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <br />
        <div id="divcontent" style="width: 750px; text-align: left;">
            <table width="100%">
                <tr>
                    <td colspan="6" align="center" style="height: 30px;">
                        <b style="font-size: 20px;">上海宏谷冷冻机有限公司组装拆分单</b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 105px;">
                        组装拆分编号：
                    </td>
                    <td style="width: 145px; border-bottom: solid #000 1px;">
                        <label id="lbCombineSplitNo" runat="server">
                        </label>
                    </td>
                    <td style="width: 95px;">
                        操作类型：
                    </td>
                    <td style="width: 155px; border-bottom: solid #000 1px;">
                        <label id="lbOperateType" runat="server">
                        </label>
                    </td>
                    <td style="width: 95px;">
                        产品名称：
                    </td>
                    <td style="width: 155px; border-bottom: solid #000 1px;">
                        <label id="lbProductName" runat="server">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        PCN：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbProductPcn" runat="server">
                        </label>
                    </td>
                    <td>
                        操作仓库：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbWarehouseName" runat="server">
                        </label>
                    </td>
                    <td>
                        本仓库存：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbStockQuantity" runat="server">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        操作数量：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbProductQuantity" runat="server">
                        </label>
                    </td>
                    <td>
                        产品型号：
                    </td>
                    <td style="border-bottom: solid #000 1px;" colspan="3">
                        <label id="lbProductCode" runat="server">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        备注：
                    </td>
                    <td style="border-bottom: solid #000 1px;" colspan="5">
                        <label id="lbRemark" runat="server">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        创建人：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbCreateName" runat="server">
                        </label>
                    </td>
                    <td>
                        创建时间：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbCreateTime" runat="server">
                        </label>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
            <label>
                组装拆分明细：</label>
            <table id="dtmain" width="100%" style="border-collapse: collapse; border: none;">
                <tr align="center">
                    <td style="width: 185px;">
                        产品名称
                    </td>
                    <td style="width: 360px;">
                        产品型号
                    </td>
                    <td style="width: 75px;">
                        PCN
                    </td>
                    <td style="width: 65px;">
                        本仓库存
                    </td>
                    <td style="width: 65px;">
                        数量
                    </td>
                </tr>
                <asp:Literal runat="server" ID="lit" />
            </table>
        </div>
    </center>
    </form>
</body>
</html>
