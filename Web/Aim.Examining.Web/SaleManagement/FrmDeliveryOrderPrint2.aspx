<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmDeliveryOrderPrint2.aspx.cs"
    Inherits="Aim.Examining.Web.FrmDeliveryOrderPrint2" %>

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
                        <b style="font-size: 20px;">上海宏谷冷冻机有限公司 发货单</b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px;">
                        购货单位：
                    </td>
                    <td style="width: 175px; border-bottom: solid #000 1px;">
                        <label id="lblghdw" runat="server">
                        </label>
                    </td>
                    <td style="width: 75px;">
                        日 期：
                    </td>
                    <td style="width: 175px; border-bottom: solid #000 1px;">
                        <label id="lblriqi" runat="server">
                        </label>
                    </td>
                    <td style="width: 75px;">
                        出库单号：
                    </td>
                    <td style="width: 175px; border-bottom: solid #000 1px;">
                        <label id="lblbianhao" runat="server">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        地 址：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbldizhi" runat="server">
                        </label>
                    </td>
                    <td>
                        联系电话：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbllxdh" runat="server">
                        </label>
                    </td>
                    <td>
                        客户订单号：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lblCustomerOrderNo" runat="server">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        摘 要：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lblzhaiyao" runat="server">
                        </label>
                    </td>
                    <td>
                        交货方式：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lbljhfs" runat="server">
                        </label>
                    </td>
                    <td>
                        发票号码：
                    </td>
                    <td style="border-bottom: solid #000 1px;">
                        <label id="lblfphm" runat="server">
                        </label>
                    </td>
                </tr>
            </table>
            <br />
            <table id="dtmain" width="100%" style="border-collapse: collapse; border: none;">
                <tr align="center">
                    <td style="width: 133px;">
                        产品名称
                    </td>
                    <td style="width: 216px;">
                        规格型号
                    </td>
                    <td style="width: 45px;">
                        单位
                    </td>
                    <td style="width: 45px;">
                        数量
                    </td>
                    <td style="width: 62px;">
                        含税价格
                    </td>
                    <td style="width: 62px;">
                        价税合计
                    </td>
                    <td>
                        备注
                    </td>
                    <td style="width: 75px;">
                        交货日期
                    </td>
                </tr>
                <asp:Literal runat="server" ID="lit" />
                <tr align="center">
                    <td>
                        汇总价税合计：
                    </td>
                    <td colspan="7">
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%">
                <tr>
                    <td style="width: 75px;">
                        开单人：
                    </td>
                    <td style="width: 200px; border-bottom: solid #000 1px;">
                        <label id="lblkdr" runat="server">
                        </label>
                    </td>
                    <td style="width: 75px;">
                        业务员：
                    </td>
                    <td style="width: 200px; border-bottom: solid #000 1px;">
                        <label id="lblywy" runat="server">
                        </label>
                    </td>
                    <td style="width: 100px;">
                        客户签收：
                    </td>
                    <td style="width: 200px; border-bottom: solid #000 1px;">
                        <label id="lblxsjl" runat="server">
                        </label>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    </form>
</body>
</html>
