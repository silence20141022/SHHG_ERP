<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    Title="产品组装拆分" CodeBehind="CombineSplitView.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.CombineSplitView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, grid;
        var enum_OperateType = { 组装: '组装', 拆分: '拆分' };
        var id = $.getQueryString({ "ID": "id" });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            InitEditTable();
            $("#btnCancel").click(function() {
                window.close();
            });
        }
        function InitEditTable() {
            // 表格数据
            myData = {
                records: AimState["DetailDataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailDataList',
                data: myData,
                fields: [
                { name: 'Id' }, { name: 'PId' }, { name: 'ProductCode' }, { name: 'ProductName' },
			    { name: 'ProductId' }, { name: 'ProductQuantity' }, { name: 'StockQuantity' }, { name: 'ProductPcn'}]
            });
            // 表格面板
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                columns: [
                { id: 'Id', dataIndex: 'Id', hidden: true },
                { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 180 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode', width: 200 },
                { id: 'ProductPcn', header: 'PCN', dataIndex: 'ProductPcn', width: 100 },
                { id: 'StockQuantity', header: '本仓库存', dataIndex: 'StockQuantity', width: 70 },
                { id: 'ProductQuantity', header: '操作数量', dataIndex: 'ProductQuantity', width: 70 }
                ],
                renderTo: "StandardSub",
                columnLines: true,
                width: Ext.get("StandardSub").getWidth(),
                autoHeight: true,
                plugins: new Ext.ux.grid.GridSummary(),
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">组装拆分明细：</label>', '->'
                   ]
                }),
                autoExpandColumn: 'ProductCode'
            });
            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "BuyPrice":
                    val = String(value);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    rtn = $("#Symbo").val() + whole;
                    break;
                case "InvoiceQuantity":
                    rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
                    break;
                case "InvoiceAmount":
                    val = String(value);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    rtn = $("#Symbo").val() + whole;
                    break;
                default: //因为有汇总插件存在 所以存在第三种情形
                    rtn = value;
                    break;
            }
            return rtn;
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            销售管理=》组装拆分</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td colspan="4">
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        组装拆分编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CombineSplitNo" name="CombineSplitNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        操作类型
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="OperateType" name="OperateType" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td>
                        产品名称
                    </td>
                    <td>
                        <input id="ProductName" name="ProductName" readonly="readonly" />
                    </td>
                    <td>
                        PCN
                    </td>
                    <td>
                        <input id="ProductPcn" name="ProductPcn" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        操作仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="WarehouseName" name="WarehouseName" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        本仓库存
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="StockQuantity" name="StockQuantity" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        操作数量
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ProductQuantity" name="ProductQuantity" readonly="readonly" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        创建人
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateName" name="CreateName" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        创建时间
                    </td>
                    <td>
                        <input id="CreateTime" name="CreateTime" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        产品型号
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <input id="ProductCode" name="ProductCode" readonly="readonly" style="width: 80%" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 80%" readonly="readonly"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnCancel" class="aim-ui-button cancel">取消</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
