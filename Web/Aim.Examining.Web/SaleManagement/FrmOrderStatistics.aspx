<%@ Page Title="销售订单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmOrderStatistics.aspx.cs" Inherits="Aim.Examining.Web.FrmOrderStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .x-grid-back-red
        {
            background: #FFE4E1;
        }
    </style>
    <style type="text/css">
        .x-grid3-cell-inner, .x-grid3-hd-inner
        {
            white-space: normal !important;
        }
        /* */.grid-row-span .x-grid3-row
        {
            border-bottom: 0;
        }
        .grid-row-span .x-grid3-col
        {
            border-bottom: 1px solid gray;
        }
        .grid-row-span .row-span
        {
            border-bottom: 1px solid #fff;
        }
        .grid-row-span .row-span-first
        {
            position: relative;
        }
        .grid-row-span .row-span-first .x-grid3-cell-inner
        {
            position: absolute;
            border-right: 1px solid gray;
            border-bottom: 1px solid gray;
        }
    </style>

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var EditPageUrl = "FrmOrdersEdit.aspx";

        var ViewWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmOrdersEdit.aspx";

        var enumState = { '': '新建', 'null': '新建', 'Flowing': '流程中', 'End': '流程结束' };

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
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
			    { name: 'Id' },
			    { name: 'Number' },
			    { name: 'CName' },
			    { name: 'PName' },
			    { name: 'PCode' },
			    { name: 'Isbn' },
			    { name: 'Count' },
			    { name: 'Amount' },

			    { name: 'Unit' },
			    { name: 'SalePrice' },
			    { name: 'OutCount' },
			    { name: 'CreateTime' }
			    ]
            });

            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 4,
                collapsed: false,
                              items: [
                { fieldLabel: '单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '商品名称', id: 'PName', schopts: { qryopts: "{ mode: 'Like', field: 'PName' }"} },
                { fieldLabel: '规格型号', id: 'PCode', schopts: { qryopts: "{ mode: 'Like', field: 'PCode' }"} },
                { fieldLabel: '销售时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '销售单' });
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

                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    plugins: new Ext.ux.grid.GridSummary(),
                    autoExpandColumn: 'CName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 120, sortable: true, renderer: renderRowCName },
					{ id: 'Number', dataIndex: 'Number', header: '销售单号', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 150, sortable: true, renderer: renderRowNumber },
					{ id: 'PName', dataIndex: 'PName', header: '商品名称', width: 120, sortable: true },
					{ id: 'PCode', dataIndex: 'PCode', header: '规格型号', width: 170, sortable: true },
					{ id: 'SalePrice', dataIndex: 'SalePrice', header: '售价', width: 100, sortable: true, renderer: filterValue, summaryRenderer: function(v, params, data) { return "汇总:" } },
					{ id: 'Count', dataIndex: 'Count', header: '数量', width: 100, sortable: true, summaryType: 'sum' },
					{ id: 'Amount', dataIndex: 'Amount', header: '金额', width: 100, sortable: true, summaryType: 'sum', renderer: filterValue },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 150, sortable: true }
                    ],
                    bbar: pgBar,
                    cls: 'grid-row-span',
                    tbar: titPanel,
                    viewConfig: {
                        //forceFit: true,
                        getRowClass: function(record, rowIndex, rowParams, store) {
                            if (record.data.InvoiceType == "发票") {
                                return 'x-grid-back-red';
                            } else {
                                return '';
                            }
                        }
                    },
                    listeners: { "rowdblclick": function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            return;
                        }
                        //查看
                        ExtOpenGridEditWin(grid, EditPageUrl, "r", EditWinStyle);
                    }
                    }
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid]
                });
            }

            // 提交数据成功后
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

            function renderRowNumber(value, meta, record, rowIndex, colIndex, store) {
                var first = !rowIndex || value !== store.getAt(rowIndex - 1).get("Number"),
                last = rowIndex >= store.getCount() - 1 || value !== store.getAt(rowIndex + 1).get("DeptName");
                meta.css += 'row-span' + (first ? ' row-span-first' : '') + (last ? ' row-span-last' : '');
                if (first) {
                    var i = rowIndex + 1;
                    while (i < store.getCount() && value === store.getAt(i).get("Number")) {
                        i++;
                    }
                    var rowHeight = 23.5, padding = 0, height = (rowHeight * (i - rowIndex) - padding) + 'px';
                    meta.attr = 'style="height:' + height + ';line-height:' + height + ';"';
                }
                return first ? '<b>' + value + '</b>' : '';
            }

            function renderRowCName(value, meta, record, rowIndex, colIndex, store) {
                var first = !rowIndex || value !== store.getAt(rowIndex - 1).get("CName"),
                last = rowIndex >= store.getCount() - 1 || value !== store.getAt(rowIndex + 1).get("DeptName");
                meta.css += 'row-span' + (first ? ' row-span-first' : '') + (last ? ' row-span-last' : '');
                if (first) {
                    var i = rowIndex + 1;
                    while (i < store.getCount() && value === store.getAt(i).get("CName")) {
                        i++;
                    }
                    var rowHeight = 23.5, padding = 0, height = (rowHeight * (i - rowIndex) - padding) + 'px';
                    meta.attr = 'style="height:' + height + ';line-height:' + height + ';"';
                }
                return first ? '<b>' + value + '</b>' : '';
            }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            销售单</h1>
    </div>
</asp:Content>
