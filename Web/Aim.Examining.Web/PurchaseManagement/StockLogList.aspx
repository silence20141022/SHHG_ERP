<%@ Page Title="出入库记录" Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="StockLogList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.StockLogList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport, ProductType;
        var ProductId = $.getQueryString({ ID: "ProductId" });
        var WarehouseId = $.getQueryString({ ID: "WarehouseId" });
        var OperateType = "";
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'InOrOutDetailId' }, { name: 'InOrOutBillNo' }, { name: 'OperateType' }, { name: 'WarehouseId' },
			    { name: 'WarehouseName' }, { name: 'StockQuantity' }, { name: 'Quantity' }, { name: 'ProductId' }, { name: 'ProductName' },
			    { name: 'ProductCode' }, { name: 'ProductPcn' }, { name: 'ProductIsbn' }, { name: 'CreateId' }, { name: 'CreateName' },
                { name: 'CreateTime'}],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data.ProductId = ProductId;
                    options.data.WarehouseId = WarehouseId;
                    options.data.OperateType = OperateType;
                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 5,
                items: [
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: 'PCN', id: 'PCN', schopts: { qryopts: "{ mode: 'Like', field: 'PCN' }"} },
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"} },
                { fieldLabel: '仓库名称', id: 'WarehouseName', schopts: { qryopts: "{ mode: 'Like', field: 'WarehouseName' }"} }
                ]
            });
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '数据过滤',
                    iconCls: 'aim-icon-search',
                    menu: [{ text: '产品入库', iconCls: 'aim-icon-arrow-right1', handler: function() {
                        OperateType = "产品入库";
                        store.reload();
                    }
                    }, {
                        text: '产品出库',
                        iconCls: 'aim-icon-arrow-left1',
                        handler: function() {
                            OperateType = "产品出库";
                            store.reload();
                        }
                    }, { text: '库存盘点', iconCls: 'aim-icon-chart', handler: function() {
                        OperateType = "库存盘点";
                        store.reload();
                    }
                    }, {
                        text: '显示全部',
                        iconCls: 'aim-icon-trans',
                        handler: function() {
                            OperateType = "";
                            store.reload();
                        }
}]
                    }, '-', {
                        text: '导出<label style=" font-family:@宋体">Excel</label>',
                        iconCls: 'aim-icon-xls',
                        handler: function() {
                            ExtGridExportExcel(grid, { store: null, title: '标题' });
                        }
                    },
                 '->'
                ]
                });
                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    title: '库存操作日志',
                    store: store,
                    region: 'center',
                    forceFit: true,
                    autoExpandColumn: 'ProductCode',
                    plugins: new Ext.ux.grid.GridSummary(),
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'InOrOutDetailId', dataIndex: 'InOrOutDetailId', header: 'InOrOutDetailId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 110, sortable: true },
                    { id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', sortable: true, width: 200, renderer: RowRender },
                    { id: 'WarehouseName', dataIndex: 'WarehouseName', header: '仓库名称', width: 80, sortable: true },
                    { id: 'ProductPcn', dataIndex: 'ProductPcn', header: 'PCN', width: 160, sortable: true },
					{ id: 'StockQuantity', dataIndex: 'StockQuantity', header: '操作前库存', width: 90, sortable: true,
					    summaryRenderer: function(v, paras, data) { return '合计'; }
					},
   					{ id: 'Quantity', dataIndex: 'Quantity', header: '操作数量', width: 80, sortable: true,
   					    summaryType: 'sum', summaryRenderer: function(v, paras, data) { return v; }
   					},
   					{ id: 'OperateType', dataIndex: 'OperateType', header: '操作类型', width: 80, sortable: true, renderer: RowRender },
   					{ id: 'InOrOutBillNo', dataIndex: 'InOrOutBillNo', header: '出/入库单号', width: 120, sortable: true },
   					{ id: 'CreateName', dataIndex: 'CreateName', header: '操作人', width: 80, sortable: true },
   					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '操作时间', width: 130, sortable: true }
   					],
                    bbar: pgBar,
                    tbar: tlBar
                });
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }
            function onExecuted() {
                store.reload();
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "ProductCode":
                        value = value || "";
                        cellmeta.attr = 'ext:qtitle =""' + ' ext:qtip ="' + value + '"';
                        rtn = value;
                        break;
                    case "Operation":
                        if (value) {
                            rtn = " <img src='../images/shared/application_view_list.png' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PurchaseOrderView.aspx?id=" +
                       value + "\",\"wind\",\"" + EditWinStyle + "\")'>查看详细</label>";

                        }
                        break;
                    case "OperateType":
                        if (value == "产品入库") {
                            rtn = "<label style='color:blue;'>" + value + "</label>";
                        }
                        if (value == "产品出库") {
                            rtn = "<label style='color:red;'>" + value + "</label>";
                        }
                        if (value == "库存盘点") {
                            rtn = value;
                        }
                        break;
                    case "StockAmount":
                        if (value) {
                            temp = Math.round(value * 100) / 100;
                            return '￥' + filterValue(temp.toString());
                        }
                        break;
                }
                return rtn;
            } 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
