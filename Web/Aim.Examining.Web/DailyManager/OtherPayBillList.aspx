<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="OtherPayBillList.aspx.cs" Inherits="Aim.Examining.Web.DailyManager.OtherPayBillList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=1000,height=600,scrollbars=yes,resizable=yes");
        var ViewPageUrl = "OtherPayBillEdit.aspx";
        var EditWinStyle = CenterWin("width=900,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl = "OtherPayBillEdit.aspx";
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
                records: AimState["DataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'PayBillNo' }, { name: 'LogisticsCompanyName' }, { name: 'PayType' }, { name: 'ShouldPayAmount' },
                { name: 'AcctualPayAmount' }, { name: 'InterfaceArray' }, { name: 'ModifyUserId' }, { name: 'InvoiceNo' }, { name: 'PayState' },
                { name: 'InvoiceAmount' }, { name: 'PayUserId' }, { name: 'PayUserName' }, { name: 'PayTime' }, { name: 'ModifyTime' },
                { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime'}]
            });

            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 5,
                items: [
                { fieldLabel: '付款单号', id: 'PayBillNo', schopts: { qryopts: "{ mode: 'Like', field: 'PayBillNo' }"} },
                { fieldLabel: '申请人', id: 'CreateName', schopts: { qryopts: "{ mode: 'Like', field: 'CreateName' }"} },
                { fieldLabel: '付款状态', id: 'PayState', schopts: { qryopts: "{ mode: 'Like', field: 'PayState' }"} },
                { fieldLabel: '申请时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
                ]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                {
                    text: '添加',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        window.open("OtherPayBillEdit.aspx?&op=c", "OtherPayBillEdit", EditWinStyle);
                    }
                },
                 {
                     text: '修改',
                     iconCls: 'aim-icon-edit',
                     hidden: true,
                     handler: function() {
                         var recs = grid.getSelectionModel().getSelections();
                         if (!recs || recs.length <= 0) {
                             AimDlg.show("请先选择要修改的记录！");
                             return;
                         }
                         ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                     }
                 },
                {
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        if (confirm("确定删除所选记录？")) {
                            var allow = true;
                            $.each(recs, function() {
                                if (this.get("PayState") == "已付款") {
                                    allow = false;
                                    return false;
                                }
                            });
                            if (!allow) {
                                AimDlg.show("已付款的付款单不允许删除！");
                                return;
                            }
                            ExtBatchOperate('batchdelete', recs, null, null, function(rtn) {
                                AimDlg.show(rtn.data.Message); onExecuted();
                            });
                        }
                    }
                },
                 {
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
                            Ext.ux.AimDoSearch(Ext.getCmp("BeginDate"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            viewport.doLayout();
                        }
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
                    autoExpandColumn: 'Remark',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'PayBillNo', dataIndex: 'PayBillNo', header: '付款单号', width: 130, renderer: RowRender },
                  	{ id: 'PayType', dataIndex: 'PayType', header: '付款类型', width: 100 },
                  	{ id: 'CreateName', dataIndex: 'CreateName', header: '申请人', sortable: true, width: 80, summaryRenderer: function(v, params, data) { return "汇总:" } },
                  	{ id: 'ShouldPayAmount', dataIndex: 'ShouldPayAmount', header: '申请金额', width: 70, summaryType: 'sum',
                  	    summaryRenderer: function(v, params, data) { return filterValue(v) }, renderer: RowRender
                  	},
                  	{ id: 'Remark', dataIndex: 'Remark', header: '申请事由', width: 100 },
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 60 },
					{ id: 'InvoiceNo', dataIndex: 'InvoiceNo', header: '发票号', width: 100 },
					{ id: 'InvoiceAmount', dataIndex: 'InvoiceAmount', header: '发票金额', width: 70, renderer: RowRender,
					    summaryType: 'sum',
					    summaryRenderer: function(v, params, data) { return filterValue(v) }
					},
					{ id: 'AcctualPayAmount', dataIndex: 'AcctualPayAmount', header: '已付金额', width: 70, summaryType: 'sum',
					    summaryRenderer: function(v, params, data) { return filterValue(v) }, renderer: RowRender
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '申请时间', width: 80, renderer: ExtGridDateOnlyRender }
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    plugins: new Ext.ux.grid.GridSummary()
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }
            // 提交数据成功后
            function onExecuted() {
                store.reload();
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
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "PayBillNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../FinanceManagement/OtherPayBillView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + ViewWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "AcctualPayAmount":
                        if (value) {//因为有null的现象
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = "￥" + whole;
                        }
                    case "ShouldPayAmount":
                        if (value) {//因为有null的现象
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = "￥" + whole;
                        }
                        break;
                    case "InvoiceAmount":
                        if (value) {//因为有null的现象
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = "￥" + whole;
                        }
                        break;
                }
                return rtn;
            }      
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
    </div>
</asp:Content>
