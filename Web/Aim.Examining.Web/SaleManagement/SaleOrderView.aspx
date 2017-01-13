<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleOrderView.aspx.cs"
    Inherits="Aim.Examining.Web.SaleManagement.SaleOrderView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
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
                <img src="../images/shared/arrow_down.gif" alt="" />基本信息</legend>
            <table width="100%" style="margin-top: 5px; margin-bottom: 5px; font-family: Verdana,Arial, Helvetica, sans-serif;
                font-size: 12px; color: Black; background-color: #FFFFFF; border-collapse: collapse;">
                <tr style="height: 30px">
                    <td align="right">
                        销售编号：
                    </td>
                    <td>
                        <asp:Label ID="lbSaleOrderNo" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        客户名称：
                    </td>
                    <td>
                        <asp:Label ID="lbCustomerName" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        开票类型：
                    </td>
                    <td>
                        <asp:Label ID="lbInvoiceType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td align="right">
                        总金额：
                    </td>
                    <td>
                        <asp:Label ID="lbTotalMoney" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        销售负责人：
                    </td>
                    <td>
                        <asp:Label ID="lbSalesman" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        开单人：
                    </td>
                    <td>
                        <asp:Label ID="lbCreateName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td align="right">
                        支付方式：
                    </td>
                    <td>
                        <asp:Label ID="lbPayType" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        提货方式：
                    </td>
                    <td>
                        <asp:Label ID="lbDeliveryMode" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        出库状态：
                    </td>
                    <td>
                        <asp:Label ID="lbDeState" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td align="right">
                        开票状态：
                    </td>
                    <td>
                        <asp:Label ID="lbInvoiceState" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        退货金额：
                    </td>
                    <td>
                        <asp:Label ID="lbReturnAmount" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        创建时间：
                    </td>
                    <td>
                        <asp:Label ID="lbCreateTime" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td align="right">
                        备注：
                    </td>
                    <td colspan="5">
                        <asp:Label ID="lbRemark" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div style="border-bottom: 2px solid gray; text-align: left; padding-left: 20px;
            margin-bottom: 10px; font-family: Verdana,Arial, Helvetica, sans-serif; font-size: 13px;
            color: Blue; font-weight: bolder">
            <img src="../images/shared/arrow_down.gif" alt="" />
            销售明细</div>
        <telerik:RadGrid ID="rgSaleOrderDetail" runat="server" Skin="Office2007" AutoGenerateColumns="False">
            <ClientSettings EnableRowHoverStyle="True">
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="销售编号" DataField="Number" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="产品名称" DataField="PName" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="产品型号" DataField="PCode" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="销售价格" DataField="SalePrice" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="销售数量" DataField="Count" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="金额" DataField="Amount">
                    </telerik:GridBoundColumn>
                </Columns>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <EditFormSettings>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
            </MasterTableView>
        </telerik:RadGrid>
        <div style="border-bottom: 2px solid gray; text-align: left; padding-left: 20px;
            margin-bottom: 10px; font-family: Verdana,Arial, Helvetica, sans-serif; font-size: 13px;
            color: Blue; font-weight: bolder">
            <img src="../images/shared/arrow_down.gif" alt="" />
            出库明细</div>
        <telerik:RadGrid ID="rgDeliveryOrderDetail" runat="server" Skin="Office2007" AutoGenerateColumns="False">
            <ClientSettings EnableRowHoverStyle="True">
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="出库编号" DataField="Number" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="产品名称" DataField="PName" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="产品型号" DataField="PCode" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="出库数量" DataField="Count" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="状态" DataField="State" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="备注" DataField="Remark">
                    </telerik:GridBoundColumn>
                </Columns>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <EditFormSettings>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
            </MasterTableView>
        </telerik:RadGrid>
        <div style="border-bottom: 2px solid gray; text-align: left; padding-left: 20px;
            margin-bottom: 10px; font-family: Verdana,Arial, Helvetica, sans-serif; font-size: 13px;
            color: Blue; font-weight: bolder">
            <img src="../images/shared/arrow_down.gif" alt="" />
            收款明细</div>
        <telerik:RadGrid ID="rgReceiveMoney" runat="server" Skin="Office2007" AutoGenerateColumns="False">
            <ClientSettings EnableRowHoverStyle="True">
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="收款编号" DataField="Number" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="收款金额" DataField="Money" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="是否对应销售明细" DataField="CorrespondState" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="收款时间" DataField="CreateTime" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="备注" DataField="Remark">
                    </telerik:GridBoundColumn>
                </Columns>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <EditFormSettings>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
            </MasterTableView>
        </telerik:RadGrid>
        <div style="border-bottom: 2px solid gray; text-align: left; padding-left: 20px;
            margin-bottom: 10px; font-family: Verdana,Arial, Helvetica, sans-serif; font-size: 13px;
            color: Blue; font-weight: bolder">
            <img src="../images/shared/arrow_down.gif" alt="" />
            发票明细</div>
        <telerik:RadGrid ID="rgSaleInvoice" runat="server" Skin="Office2007" AutoGenerateColumns="False">
            <ClientSettings EnableRowHoverStyle="True">
            </ClientSettings>
            <MasterTableView DataKeyNames="Id">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="发票编号" DataField="Number" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="发票金额" DataField="Amount" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="折扣金额" DataField="DiscountAmount" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="收款时间" DataField="CreateTime" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="是否作废" DataField="Invalid" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="备注" DataField="Remark">
                    </telerik:GridBoundColumn>
                </Columns>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <EditFormSettings>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
            </MasterTableView>
        </telerik:RadGrid>
        <div style="border-bottom: 2px solid gray; text-align: left; padding-left: 20px;
            margin-bottom: 10px; font-family: Verdana,Arial, Helvetica, sans-serif; font-size: 13px;
            color: Blue; font-weight: bolder">
            <img src="../images/shared/arrow_down.gif" alt="" />
            退货信息</div>
        <telerik:RadGrid ID="rgReturnOrder" runat="server" Skin="Office2007" AutoGenerateColumns="False">
            <ClientSettings EnableRowHoverStyle="True">
            </ClientSettings>
            <MasterTableView DataKeyNames="ProductId">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="退货单号" DataField="Number" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="产品名称" DataField="ProductName" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="产品型号" DataField="ProductCode" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="数量" DataField="Count" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="退货价格" DataField="ReturnPrice" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="退货金额" DataField="Amount">
                    </telerik:GridBoundColumn>
                </Columns>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <EditFormSettings>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
            </MasterTableView>
        </telerik:RadGrid>
        <div style="border-bottom: 2px solid gray; text-align: left; padding-left: 20px;
            margin-bottom: 10px; font-family: Verdana,Arial, Helvetica, sans-serif; font-size: 13px;
            color: Blue; font-weight: bolder">
            <img src="../images/shared/arrow_down.gif" alt="" />
            物流信息</div>
        <telerik:RadGrid ID="rgLogistics" runat="server" Skin="Office2007" AutoGenerateColumns="False">
            <ClientSettings EnableRowHoverStyle="True">
            </ClientSettings>
            <MasterTableView DataKeyNames="Number">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="运单号" DataField="Number" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="物流公司" DataField="Name" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="重量" DataField="Weight" HeaderStyle-Width="120px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="运费" DataField="Price" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="保价费" DataField="Insured" HeaderStyle-Width="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="送货费" DataField="Delivery" HeaderStyle-Width="70px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="合计" DataField="Total" HeaderStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="收货人" DataField="Receiver" HeaderStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <%--  <telerik:GridBoundColumn HeaderText="收货人电话" DataField="Tel">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="收货人手机" DataField="MobilePhone">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn HeaderText="付款方式" DataField="PayType" HeaderStyle-Width="70px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="备注" DataField="Remark">
                    </telerik:GridBoundColumn>
                </Columns>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <EditFormSettings>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    </form>
</body>
</html>
