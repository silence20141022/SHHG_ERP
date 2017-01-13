<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="PurchaseOrderList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseOrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .x-grid3-summary-row
        {
            background-color: #FFFFC0;
        }
        .x-grid3-summary-row .x-grid3-td-Ext2
        {
            background-color: #FFFFC0;
        }
        .x-grid3-td-Ext2
        {
            background-color: #FAFAD1;
        }
        .x-grid3-row-expanded
        {
            border-width: 1px;
            border-color: Red;
        }
    </style>
    <script type="text/javascript" src="/js/ext/ux/RowExpander.js"></script>
    <script type="text/javascript">
        var PurchaseOrderId = '';
        Ext.override(Ext.grid.GridView, {
            afterRender: Ext.grid.GridView.prototype.afterRender.createSequence(function () {
                this.fireEvent("viewready", this); //new event 
            })
        });
        var beforeRowExpand = function (expander, record, body, rowIndex) {
            if (this.loaded && this.loaded[rowIndex]) {
                Ext.getCmp("grid_" + record.get("Id")).getEl().swallowEvent(['mousedown', 'mouseup', 'click', 'contextmenu', 'mouseover', 'mouseout', 'dblclick', 'mousemove']);
                Ext.get("grid_" + record.get("Id")).swallowEvent(['mousedown', 'mouseup', 'click', 'contextmenu', 'mouseover', 'mouseout', 'dblclick', 'mousemove']);
            }
            var rowView = this.grid.getView().getRow(rowIndex);
            var rowViewObject = Ext.get(rowView);
            var expanderContent = rowViewObject.child('.row-expander-box');
            var gridEl = expanderContent.child('.x-grid-panel');
            if (gridEl) {
                gridEl.swallowEvent(['mousedown', 'mouseup', 'click', 'contextmenu', 'mouseover', 'mouseout', 'dblclick', 'mousemove']);
                gridEl.onclick = function (e) {
                    event.cancelBubble = true;
                    if (e && e.stopPropagation) {
                        //因此它支持W3C的stopPropagation()方法
                        e.stopPropagation();
                        e.preventDefault();
                    }
                    return false
                };
            }
        }
        var expander = new Ext.grid.RowExpander({
            tpl: new Ext.Template('<div id="myrow_{Id}" class="row-expander-box" style="margin-left: 62px"></div>'),
            lazyRender: false,
            enableCaching: false,
            preserveRowsOnRefresh: true
        });
        expander.on("expand", function (obj, record, body, rowIndex) {
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
                fields: [{ name: 'Id' }, { name: 'PurchaseOrderId' }, { name: 'PurchaseOrderNo' }, { name: 'ProductId' }, { name: 'Name' },
                    	    { name: 'Code' }, { name: 'PCN' }, { name: 'BuyPrice' }, { name: 'Quantity' }, { name: 'Amount' }, { name: 'PayState' },
                    	    { name: 'InvoiceState' }, { name: 'InWarehouseState' }, { name: 'Symbo'}],
                listeners: { aimbeforeload: function (proxy, options) {
                    options.data = options.data || [];
                    options.data.PurchaseOrderId = PurchaseOrderId;
                    options.data.optype = "getChildData";
                }
                }
            });
            var gridX = new Ext.ux.grid.AimGridPanel({
                store: childstore,
                id: 'grid_' + record.get("Id"),
                columnLines: true,
                columns: [new Ext.ux.grid.AimRowNumberer(),
                  { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                  { id: 'Symbo', dataIndex: 'Symbo', header: '货币符号', hidden: true },
                  { id: 'Name', dataIndex: 'Name', header: '产品名称', sortable: false },
                  { id: 'Code', dataIndex: 'Code', header: '产品型号', sortable: false },
                  { id: 'BuyPrice', dataIndex: 'BuyPrice', header: '价格', width: 60,
                      summaryRenderer: function (v, params, data) { return "汇总:" }
                  },
	              { id: 'Quantity', dataIndex: 'Quantity', header: '数量', width: 50, summaryType: 'sum' },
			      { id: 'Amount', dataIndex: 'Amount', header: '金额', width: 70, summaryType: 'sum', renderer: RowRender,
			          summaryRenderer: function (v, params, data) {
			              return record.get("Symbo") + filterValue(Math.round(parseFloat(v) * 100) / 100);
			          }
			      },
                  { id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 65 },
	              { id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 65 },
                  { id: 'InvoiceState', dataIndex: 'InvoiceState', header: '发票状态', width: 65}],
                viewConfig: {
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
            PurchaseOrderId = record.get("Id");
            childstore.reload();
            this.loaded = this.loaded || [];
            this.loaded[rowIndex] = gridX;
        });
        function expandAllRows() {
            var nRows = store.getCount();
            for (i = 0; i < nRows; i++)
                expander.expandRow(i);
        }
        var WinStyle = CenterWin("width=1200,height=768,scrollbars=yes,resizable=yes");
        var EditWinStyle = CenterWin("width=950,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl = "PurchaseOrderEdit.aspx";
        var EditWinStyle2 = CenterWin("width=950,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl2 = "OtherPurchaseOrderEdit.aspx";
        var Index = $.getQueryString({ "ID": "Index" });
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport, PayState, InWarehouseState, OrderState;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["PurchaseOrderList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'PurchaseOrderList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'PriceType' }, { name: 'PurchaseOrderNo' }, { name: 'RMBAmount' }, { name: 'Symbo' }, { name: 'PurchaseType' },
                { name: 'SupplierName' }, { name: 'TransportationMode' }, { name: 'OrderDate' }, { name: 'RequestDeliveryDate' }, { name: 'Remark' },
                { name: 'OrderState' }, { name: 'PurchaseOrderAmount' }, { name: 'ProductType' }, { name: 'InvoiceState' }, { name: 'MoneyType' },
                { name: 'PayState' }, { name: 'InWarehouseState' }, { name: 'CreateId' }, { name: 'CreateTime' }, { name: 'CreateName'}],
                listeners: { "aimbeforeload": function (proxy, options) {
                    options.data = options.data || {};
                    options.data.op = pgOperation || null;
                    options.data.Index = Index;
                    options.data.PayState = PayState;
                    options.data.InWarehouseState = InWarehouseState;
                    options.data.OrderState = OrderState;
                    PayState = ""; InWarehouseState = ""; OrderState = "";
                }, load: function () {

                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 5,
                animCollapse: true,
                collapsed: false,
                items: [
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '采购编号', id: 'PurchaseOrderNo', schopts: { qryopts: "{ mode: 'Like', field: 'PurchaseOrderNo' }"} },
                { fieldLabel: '供应商', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '采购类型', id: 'PurchaseType', schopts: { qryopts: "{ mode: 'Like', field: 'PurchaseType' }"} },
                { fieldLabel: '采购人', id: 'CreateName', schopts: { qryopts: "{ mode: 'Like', field: 'CreateName' }"} },
                { fieldLabel: '下单时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
               ]
            });
            // 工具栏
            if (Index == '0') {
                tlBar = new Ext.ux.AimToolbar({
                    items: [
                    {
                        text: '添加生产商采购',
                        iconCls: 'aim-icon-add',
                        handler: function () {
                            ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                        }
                    }, '-',
                    {
                        text: '添加中间商采购',
                        iconCls: 'aim-icon-add',
                        handler: function () {
                            ExtOpenGridEditWin(grid, EditPageUrl2, "c", EditWinStyle2);
                        }
                    }, '-',
                    {
                        text: '修改',
                        iconCls: 'aim-icon-edit',
                        hidden: true,
                        handler: function () {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要修改的记录！");
                                return;
                            }
                            jQuery.ajaxExec("Modify", { id: recs[0].get("Id") }, function (rtn) {
                                if (rtn.data.result && rtn.data.result == "true") {
                                    if (recs[0].get("PurchaseType") == "生产商采购") {
                                        ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                                    }
                                    else {
                                        ExtOpenGridEditWin(grid, EditPageUrl2, "u", EditWinStyle2);
                                    }
                                }
                                else {
                                    AimDlg.show("已创建入库单或者付款单的订单不允许修改！"); return;
                                }
                            });
                        }
                    }, '-', {
                        text: '删除',
                        iconCls: 'aim-icon-delete',
                        handler: function () {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要删除的记录！");
                                return;
                            }
                            if (confirm("确定删除所选记录？")) {
                                ExtBatchOperate('batchdelete', recs, null, null, function (rtn) {
                                    AimDlg.show(rtn.data.Message); onExecuted();
                                });
                            }
                        }
                    },
                    //                     "-", {
                    //                        text: '已付款的采购单',
                    //                        iconCls: 'aim-icon-search',
                    //                        handler: function() {
                    //                            PayState = "已付款"; InWarehouseState = ""; OrderState = "";
                    //                            store.reload();
                    //                        }
                    //                    }, "-", {
                    //                        text: '已入库的采购单',
                    //                        iconCls: 'aim-icon-search',
                    //                        handler: function() {
                    //                            InWarehouseState = "已入库"; PayState = ""; OrderState = "";
                    //                            store.reload();
                    //                        }
                    //                    }, "-", {
                    //                        text: '显示全部',
                    //                        iconCls: 'aim-icon-search',
                    //                        handler: function() {
                    //                            OrderState = "未结束"; PayState = ""; InWarehouseState = "";
                    //                            store.reload();
                    //                        }
                    //                    },
                     "-", {
                         text: '打印订货通知书',
                         iconCls: 'aim-icon-printer',
                         handler: function () {
                             var recs = grid.getSelectionModel().getSelections();
                             if (!recs || recs.length <= 0) {
                                 AimDlg.show("请先选择打印的采购订单！");
                                 return;
                             }
                             window.open('PrintOrder.aspx?PurchaseOrderId=' + recs[0].get("Id"), 'newwindow', EditWinStyle);
                         }
                     },
                     "-", {
                         text: '采购详细报表',
                         iconCls: 'aim-icon-search',
                         handler: function () {
                             window.open('PurchaseOrderDetailList.aspx', 'newwindow', WinStyle);
                         }
                     },
                 '->']
                });
            }
            // 工具标题栏
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar ? tlBar : '',
                items: [schBar]
            });
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                id: 'mainGrid',
                region: 'center',
                forceFit: true,
                // autoExpandColumn: 'Remark',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'TransportationMode', dataIndex: 'TransportationMode', header: 'TransportationMode', hidden: true },
                    { id: 'OrderDate', dataIndex: 'OrderDate', header: 'OrderDate', hidden: true },
                    { id: 'Symbo', dataIndex: 'Symbo', header: 'Symbo', hidden: true },
                    { id: 'RequestDeliveryDate', dataIndex: 'RequestDeliveryDate', header: 'RequestDeliveryDate', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    expander,
                    { id: 'PurchaseOrderNo', dataIndex: 'PurchaseOrderNo', header: '采购编号', width: 120, sortable: true, renderer: RowRender },
                    { id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商', sortable: true, width: 240 },
					{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 70, sortable: true },
					{ id: 'PurchaseType', dataIndex: 'PurchaseType', header: '采购类型', width: 100, sortable: true },
					{ id: 'PurchaseOrderAmount', dataIndex: 'PurchaseOrderAmount', header: '采购单金额', width: 80, renderer: RowRender },
					{ id: 'MoneyType', dataIndex: 'MoneyType', header: '交易币种', width: 70 },
				    { id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 70, sortable: true },
				    { id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 70, sortable: true },
                    { id: 'InvoiceState', dataIndex: 'InvoiceState', header: '发票状态', width: 70, sortable: true },
			    	{ id: 'CreateName', dataIndex: 'CreateName', header: '采购人 ', width: 60, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '下单时间 ', width: 70, sortable: true, renderer: ExtGridDateOnlyRender },
                    { id: 'Remark', dataIndex: 'Remark', header: '备注 ' }
              		],
                bbar: pgBar,
                tbar: titPanel,
                plugins: expander
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function onExecuted() {
            store.reload();
        }
        function filterValue(val) {
            if (val == null || val == "") {
                return null;
            }
            else {
                val = String(val);
                var whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return whole;
            }
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "PurchaseOrderNo":
                    if (value) {
                        rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PurchaseOrderView.aspx?id=" +
                                      record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                    }
                    break;
                case "PurchaseOrderAmount":
                    val = String(value);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    rtn = record.get("Symbo") + whole;
                    break;
                case "SupplierName":
                    if (value) {
                        value = value || "";
                        cellmeta.attr = 'ext:qtitle =""' + ' ext:qtip ="' + value + '"';
                        rtn = value;
                    }
                    break;
                case "Amount":
                    if (value) {
                        rtn = record.get("Symbo") + filterValue(value);
                    }
                    break;
            }
            return rtn;
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
        function accMul(arg1, arg2) {
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
        }      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
