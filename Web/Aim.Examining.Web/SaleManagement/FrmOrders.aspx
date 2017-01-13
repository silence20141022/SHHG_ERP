<%@ Page Title="销售订单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmOrders.aspx.cs" Inherits="Aim.Examining.Web.FrmOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        /*.x-grid3-cell-inner, .x-grid3-hd-inner
        {
            white-space: normal !important;
        }
        .x-grid3-cell-inner
        {
        }
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
        }*/.x-grid-back-red
        {
            background: #FFE4E1;
        }
    </style>

    <script type="text/javascript" src="/js/ext/ux/RowExpander.js"></script>

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
                    { name: 'Id' }, { name: 'CustomerOrderNo' }, { name: 'Isbn' }, { name: 'PCode' }, { name: 'PName' },
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
                //{ id: 'Isbn', header: '条形码', dataIndex: 'Isbn', width: 120, sortable: false },
                    {id: 'PName', header: '产品名称', dataIndex: 'PName', width: 150, resizable: true },
                    { id: 'PCode', header: '产品型号', dataIndex: 'PCode', width: 180, resizable: true, renderer: ExtGridpperCase },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 100, resizable: true },
                    { id: 'MinSalePrice', header: '最低售价', dataIndex: 'MinSalePrice', width: 100, resizable: true, hidden: true },
                    { id: 'SalePrice', header: '销售价', dataIndex: 'SalePrice', width: 100, renderer: filterValue, resizable: true,
                        summaryRenderer: function(v, params, data) { return "汇总:" }
                    },
                    { id: 'Count', header: '销售数量', dataIndex: 'Count', width: 70, resizable: true, summaryType: 'sum', allowBlank: false },
                    { id: 'OutCount', header: '生成出库单数量', dataIndex: 'OutCount', width: 90, resizable: true, summaryType: 'sum' },
                    { id: 'Amount', dataIndex: 'Amount', header: '销售金额', summaryType: 'sum', width: 100, renderer: filterValue },
                    { id: 'CustomerOrderNo', header: '客户订单号', dataIndex: 'CustomerOrderNo', width: 120, resizable: true },
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


        var EditWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var EditPageUrl = "FrmOrdersEdit.aspx";

        var ViewWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var ViewPageUrl = "SaleOrderView.aspx";

        var enumState = { '': '新建', 'null': '新建', 'Flowing': '流程中', 'End': '流程结束' };

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OrderList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OrderList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'PId' }, { name: 'Number' }, { name: 'CId' }, { name: 'CCode' }, { name: 'CName' },
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

			    { name: 'CorrespondState' },
			    { name: 'CorrespondInvoice' },
			    { name: 'CorrespondAmount' },
			    { name: 'EndDate' },

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
			    { name: 'CreateTime' }
			    ], listeners: { "aimbeforeload": function(proxy, options) {
			        options.data.ftype = getQueryString("ftype");
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
                collapsed: false,
                items: [
                { fieldLabel: '销售单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '创建时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '添加',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                    }
                }, {
                    text: '修改',
                    iconCls: 'aim-icon-edit',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要修改的记录！");
                            return;
                        }
                        if (recs[0].get("State") == "Flowing") {
                            alert("已经提交流程的记录不能修改!"); return;
                        }

                        //if (recs[0].get("DeliveryState")) {
                        //    alert("此订单已到下一环节或已作废不能修改!"); return;
                        //}

                        if (recs[0].get("DeState") != "未出库" && recs[0].get("DeState") != null) {
                            alert("此订单" + recs[0].get("DeState") + "不能修改!"); return;
                        }

                        //if (recs[0].get("PAState") == "不同意" || recs[0].get("PAState") == "待审核") {
                        if (recs[0].get("PAState") != "不同意" && recs[0].get("PAState") != "同意" && recs[0].get("PAState")) {
                            alert("订单对应的价格申请单审核未完成，不能修改!"); return;
                        }


                        if (recs[0].get("DeliveryState") && recs[0].get("DeliveryState") != "") {
                            if (confirm("此订单" + recs[0].get("DeliveryState") + "，修改会删除生成的出库单，确定修改!")) {

                                ExtOpenGridEditWin(grid, EditPageUrl + "?del=yes", "u", EditWinStyle);
                            }
                        }
                        else {
                            ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                        }
                    }
                }, { text: '修改订单金额',
                    iconCls: 'aim-icon-edit',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要修改的记录！");
                            return;
                        }

                        if (confirm("修改订单金额可能会影响财务收款信息,确定?")) {

                            ExtOpenGridEditWin(grid, EditPageUrl + "?updatemoney=yes", "u", EditWinStyle);
                        }
                    }
                }, {
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        for (var i = 0; i < recs.length; i++) {
                            if (recs[i].get("State") == "Flowing") {
                                alert("已经提交流程的记录不能删除!"); return;
                            }

                            //if (recs[i].get("PAState") == "不同意" || recs[0].get("PAState") == "待审核") {
                            if (recs[i].get("PAState") != "不同意" && recs[i].get("PAState") != "同意" && recs[i].get("PAState")) {
                                alert("订单对应的价格申请单审核未完成，不能删除!"); return;
                            }

                            if (recs[i].get("DeliveryState") && recs[i].get("DeliveryState") != "已作废") {
                                alert("此订单" + recs[i].get("DeliveryState") + "不能删除!"); return;
                            }

                        }

                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
                        }
                    }
                }, '-', {
                    text: '提交审批',
                    id: 'btnSubmit',
                    iconCls: 'aim-icon-execute',
                    hidden: true,
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要审批的记录！");
                            return;
                        }
                        if (recs[0].get("State") == "Flowing") {
                            alert("已经提交流程的记录不能提交审批!"); return;
                        }

                        if (recs[0].get("PAState") == "不同意" || recs[0].get("PAState") == "待审核") {
                            alert("订单对应的价格申请单审核未通过，不能提交审批!"); return;
                        }

                        if (recs[0].get("DeliveryState")) {
                            alert("此订单已到下一环节或已作废不能再提交审批!"); return;
                        }

                        if (!confirm("确定要提交审批吗?提交后不能再修改!")) return;
                        Ext.getBody().mask("提交中,请稍后...");
                        var flowKey = "Orders";
                        jQuery.ajaxExec('submit', { state: "Flowing", Id: recs[0].get("Id"), FlowKey: flowKey }, function(rtn) {
                            window.setTimeout("AutoExecuteFlow('" + rtn.data.FlowId + "')", 500);
                        });
                    }
                }, {
                    text: '流程跟踪',
                    hidden: true,
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要跟踪的记录！");
                            return;
                        }
                        ExtOpenGridEditWin(grid, "/workflow/flowtrace.aspx?FormId=" + recs[0].get("Id"), "c", CenterWin("width=900,height=600,scrollbars=yes"));
                    }
                }, {
                    text: '生成出库单',
                    iconCls: 'aim-icon-execute',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }

                        //for (var i = 0; i < recs.length; i++) {
                        if (recs[0].get("DeliveryState") == "已全部生成出库单") {
                            alert("该订单已全部生成出库单");
                            return;
                        }
                        if (recs[0].get("PAState") == "不同意" || recs[0].get("PAState") == "待审核" || recs[0].get("DeliveryState") == "已作废") {
                            alert("订单对应的价格申请单审核未通过或已作废，不能生成出库单!"); return;
                        }
                        window.open("FrmDeliveryOrderEdit3.aspx?paids=" + recs[0].get("Id") + "&op=c", "Delivery", "width=1000,height=600,scrollbars=yes");
                    }
                },
                {
                    text: '详细查看',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        window.open("SaleOrderDetailReport.aspx", "view", CenterWin("width=1050,height=600,resizable=yes"));
                    }
                }, '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("CName"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            setTimeout("viewport.doLayout()", 50);
                        }
                    }
}]
                });
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'CName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(), expander,
					{ id: 'Number', dataIndex: 'Number', header: '销售单号', linkparams: { url: ViewPageUrl, style: ViewWinStyle },
					    width: 130, sortable: true, summaryRenderer: function(v, params, data) { return "汇总:" }
					},
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 150, sortable: true },
					{ id: 'TotalMoney', dataIndex: 'TotalMoney', header: '总金额', width: 100, sortable: true, summaryType: 'sum', renderer: function(val, r, p) {
					    val = String(val);
					    var whole = val;
					    var r = /(\d+)(\d{3})/;
					    while (r.test(whole)) {
					        whole = whole.replace(r, '$1' + ',' + '$2');
					    }
					    if (p.data.InvoiceType == "发票") {
					        return '<label style="color:red;">￥' + (whole == "null" || whole == null ? "" : whole) + '</label>';
					    }
					    else {
					        return '￥' + (whole == "null" || whole == null ? "" : whole);
					    }
					}
					},
					{ id: 'InvoiceType', dataIndex: 'InvoiceType', header: '开票类型', width: 80, sortable: true, hidden: true },
					{ id: 'DeliveryState', dataIndex: 'DeliveryState', header: '生成出库单状态', width: 100, sortable: true, renderer: function(val) {
					    if (val == "已作废")
					        return "<label style='color:Red;'>" + val + "</label>";
					    else if (val == "已全部退货")
					        return "已全部生成出库单";
					    return val;
					}
					},
					{ id: 'DeState', dataIndex: 'DeState', header: '出库状态', width: 100, sortable: true, renderer: function(val) {
					    if (val == "已全部出库")
					        return "<label style='color:green;'>" + val + "</label>";
					    else if (!val) {
					        return "<label style='color:red;'>未出库</label>";
					    }
					    else {
					        return "<label style='color:red;'>" + val + "</label>";
					    }
					}
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 80, renderer: ExtGridDateOnlyRender, sortable: true },
                    { id: 'InvoiceState', dataIndex: 'InvoiceState', header: '开票状态', width: 80, sortable: true },
                    {id: 'CreateName', dataIndex: 'CreateName', header: '开单员', width: 80, sortable: true },
                    //{ id: 'InvoiceNumber', dataIndex: 'InvoiceNumber', header: '发票号', width: 80, sortable: true },
					{id: 'PAState', dataIndex: 'PAState', header: '价格申请单审核状态', width: 120, sortable: true, renderer: function(val) {
					    if (val == "")
					        return "不需要审核";
					    else if (val == "同意") {
					        return "<label style='color:green;'>通过</label>";
					    }
					    else if (val == "不同意") {
					        return "<label style='color:red;'>未通过</label>";
					    }
					    else {
					        return val;
					    }
					}
	},
					{ id: 'PANumber', dataIndex: 'PANumber', header: '价格申请单编号', width: 100, sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    plugins: [new Ext.ux.grid.GridSummary(), expander]
                });
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }
            function onExecuted() {
                store.reload();
            }

            function AutoExecuteFlow(flowid) {
                jQuery.ajaxExec('AutoExecuteFlow', { "FlowId": flowid }, function(rtn) {
                    AimDlg.show("提交成功！");
                    onExecuted();
                    Ext.getBody().unmask();
                });
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
