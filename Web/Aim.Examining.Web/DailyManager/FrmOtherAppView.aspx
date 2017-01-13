<%@ Page Title="其他费用申请" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmOtherAppView.aspx.cs" Inherits="Aim.Examining.Web.FrmOtherAppView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=800,height=600,scrollbars=yes");
        var EditPageUrl = "FrmOtherAppEdit.aspx";
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
                records: AimState["OtherCostList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OtherCostList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'Number' },
			    { name: 'DeptName' },
			    { name: 'Child' },
			    { name: 'Reason' },
			    { name: 'Amount' },
			    { name: 'LeaveUser' },
			    { name: 'AppState' },
			    { name: 'State' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
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
                collapsed: false,
                items: [
                { fieldLabel: '申请人', id: 'LeaveUser', schopts: { qryopts: "{ mode: 'Like', field: 'LeaveUser' }"} },
                { fieldLabel: '申请时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'CreateTime' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'CreateTime' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '查看',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "r", EditWinStyle);
                    }
                }, '-', {
                    text: '流程跟踪',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要跟踪的记录！");
                            return;
                        }
                        ExtOpenGridEditWin(grid, "/workflow/flowtrace.aspx?FormId=" + recs[0].get("Id"), "c", CenterWin("width=900,height=600,scrollbars=yes"));
                    }
                }, '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
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
                    autoExpandColumn: 'Reason',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '编号', width: 130, sortable: true, linkparams: { url: EditPageUrl, style: EditWinStyle }, summaryRenderer: function(v, params, data) { return "汇总:" } },
					{ id: 'LeaveUser', dataIndex: 'LeaveUser', header: '申请人', width: 100, sortable: true },
					{ id: 'DeptName', dataIndex: 'DeptName', header: '部门', width: 100, sortable: true },
					{ id: 'Amount', dataIndex: 'Amount', header: '报销金额', width: 100, sortable: true, renderer: filterValue, summaryType: 'sum' },
					{ id: 'State', dataIndex: 'State', header: '状态', width: 100, enumdata: enumState, sortable: true },
					{ id: 'AppState', dataIndex: 'AppState', header: '审核状态', width: 100, sortable: true },
					{ id: 'Reason', dataIndex: 'Reason', header: '明细', width: 100, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '申请日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
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
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
