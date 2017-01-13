<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="InvalidOrderList.aspx.cs" Inherits="Aim.Examining.Web.SaleManagement.InvalidOrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/ext/ux/RowExpander.js" type="text/javascript"></script>

    <script type="text/javascript">
        var OId = "";

        var expander = new Ext.grid.RowExpander({
            tpl: new Ext.Template('<div id="myrow_{Id}" class="row-expander-box" style="margin-left: 62px"></div>'),
            lazyRender: false,
            enableCaching: false,
            preserveRowsOnRefresh: true
        });
        expander.on("expand", function(obj, record, body, rowIndex) {
            var rowid = "myrow_" + record.get("Id");
            var gridid = "grid_" + record.get("Id");
            if (this.loaded && this.loaded[rowIndex]) {
                this.loaded[rowIndex].getEl().swallowEvent(['mousedown', 'mouseup', 'click', 'contextmenu', 'mouseover', 'mouseout', 'dblclick', 'mousemove']);
                return;
            }
            var myData = {
                total: 0,
                records: []
            };
            var childstore = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                id: 'store_' + record.get("Id"),
                idProperty: 'Id',
                fields: [
                    { name: 'Id' },
			        { name: 'Isbn' },
			        { name: 'PCode' },
			        { name: 'PName' },
			        { name: 'MinSalePrice' },
			        { name: 'SalePrice' },
			        { name: 'Unit' },
			        { name: 'Count' },
			        { name: 'OutCount' },
			        { name: 'Amount' },
			        { name: 'Remark' }
			    ], listeners: { "aimbeforeload": function(proxy, options) {
			        options.data.OId = OId;
			        options.data.optype = "getChildData";
			    }
			    }
            });

            var gridX = new Ext.ux.grid.AimGridPanel({
                store: childstore,
                id: 'grid_' + record.get("Id"),
                columnLines: true,
                columns: [new Ext.ux.grid.AimRowNumberer(),
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, resizable: true, hidden: true },
                    { id: 'Isbn', header: '条形码', dataIndex: 'Isbn', width: 120, sortable: false },
                    { id: 'PName', header: '商品名称', dataIndex: 'PName', width: 150, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'PCode', header: '规格型号', dataIndex: 'PCode', width: 150, resizable: true, renderer: ExtGridpperCase },

                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 100, resizable: true },
                    { id: 'MinSalePrice', header: '最低售价', dataIndex: 'MinSalePrice', width: 100, resizable: true, hidden: true },

                    { id: 'SalePrice', header: '售价', dataIndex: 'SalePrice', width: 100, renderer: filterValue, resizable: true },
                    { id: 'Count', header: '购买量', dataIndex: 'Count', width: 100, resizable: true, summaryType: 'sum', allowBlank: false },
                    { id: 'OutCount', header: '已出库量', dataIndex: 'OutCount', width: 100, resizable: true, summaryType: 'sum' },

                    { id: 'Amount', dataIndex: 'Amount', header: '总金额', summaryType: 'sum', width: 100, renderer: filterValue },
                    { id: 'Remark', header: '备注', dataIndex: 'Remark', width: 100, resizable: true }
                ], viewConfig: {
                    forceFit: true
                },
                autoHeight: true,
                plugins: new Ext.ux.grid.GridSummary()
            });
            gridX.render(rowid);
            var rowView = grid.getView().getRow(rowIndex);
            var rowViewObject = Ext.get(rowView);
            var expanderContent = rowViewObject.child('.row-expander-box');
            var gridEl = expanderContent.child('.x-grid-panel');
            if (gridEl) {
                gridEl.swallowEvent(['mousedown', 'mouseup', 'click', 'contextmenu', 'mouseover', 'mouseout', 'dblclick', 'mousemove']);
            }
            //            childstore.baseParams = { 'url': 'ChildList.aspx' };
            //            childstore.reload({ "PurchaseOrderId": record.get("Id") });
            //            this.loaded = this.loaded || [];
            //            this.loaded[rowIndex] = gridX;

            OId = record.get("Id");
            childstore.reload();
            this.loaded = this.loaded || [];
            this.loaded[rowIndex] = gridX;
        });

        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }

        var EditWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var EditPageUrl = "FrmOrdersEdit.aspx";

        var ViewWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var ViewPageUrl = "SaleOrderView.aspx";



        var store, mydata;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        };

        function setPgUI() {
            //表格数据
            mydata = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OrderList"] || []
            };

            //表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OrderList',
                idProperty: 'Id',
                data: mydata,
                fields: [
                { name: 'Id' },
			    { name: 'PId' },
			    { name: 'Number' },
			    { name: 'CId' },
			    { name: 'CCode' },
			    { name: 'CName' },
			    { name: 'ExpectedTime' },

			    { name: 'WarehouseId' },
			    { name: 'WarehouseName' },
			    { name: 'CalculateManner' },
			    { name: 'InvoiceType' },
			    { name: 'PayType' },
			    { name: 'TotalMoney' },
			    { name: 'PreDeposit' },
			    { name: 'ApprovalState' },
			    { name: 'TotalMoneyHis' },
			    { name: 'InvoiceNumber' },

			    { name: 'DeliveryMode' },
			    { name: 'InvoiceState' },
			    { name: 'PANumber' },
			    { name: 'PAState' },
			    { name: 'Child' },
			    { name: 'Reason' },
			    { name: 'Remark' },
			    { name: 'State' },
			    { name: 'Salesman' },
			    { name: 'SalesmanId' },
			    { name: 'DeState' },
			    { name: 'DeliveryState' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
			    { name: 'CreateTime'}]
            });

            //分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            //搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 5,
                collapsed: false,
                padding: '2 0 0 0',
                items: [
                { fieldLabel: '单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '规格型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '创建时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]

            });

            //工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
                        }
                    }
                }, '->',
                {
                    text: '复杂查询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        schBar.toggleCollapse(false);

                        setTimeout("viewport.doLayout()", 50);
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });

                //表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(), expander,
					{ id: 'Number', dataIndex: 'Number', header: '单号', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 130, sortable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 250, sortable: true },
					{ id: 'TotalMoney', dataIndex: 'TotalMoney', header: '总金额', width: 70, sortable: true, summaryType: 'sum', renderer: function(val, r, p) {
					    val = String(val);
					    var whole = val;
					    var r = /(\d+)(\d{3})/;
					    while (r.test(whole)) {
					        whole = whole.replace(r, '$1' + ',' + '$2');
					    }
					    if (p.get("InvoiceType") == "发票") {
					        return '<label style="color:red;">￥' + (whole == "null" || whole == null ? "" : whole) + '</label>';
					    }
					    else {
					        return '￥' + (whole == "null" || whole == null ? "" : whole);
					    }
					}
					},
                    //{ id: 'State', dataIndex: 'State', header: '流程状态', width: 70, enumdata: enumState, sortable: true },
                    {id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 80, renderer: ExtGridDateOnlyRender, sortable: true },

					{ id: 'InvoiceType', dataIndex: 'InvoiceType', header: '开票类型', width: 80, sortable: true, hidden: true },
					{ id: 'DeliveryState', dataIndex: 'DeliveryState', header: '生成出库单状态', width: 120, sortable: true, renderer: function(val) {
					    if (val == "已作废")
					        return "<label style='color:Red;'>" + val + "</label>";
					    return val;
					}
					},
					{ id: 'DeState', dataIndex: 'DeState', header: '出库状态', width: 100, sortable: true, renderer: function(val) {
					    if (val == "已全部出库")
					        return "<label style='color:green;'>" + val + "</label>";
					    else
					        return "<label style='color:red;'>" + val + "</label>";
					}
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 80, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'InvoiceState', dataIndex: 'InvoiceState', header: '开票状态', width: 80, sortable: true },

					{ id: 'InvoiceNumber', dataIndex: 'InvoiceNumber', header: '发票号', width: 80, sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    plugins: expander
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid]
                });

            };

            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
