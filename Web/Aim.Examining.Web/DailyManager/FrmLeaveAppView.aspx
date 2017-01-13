<%@ Page Title="请假申请" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmLeaveAppView.aspx.cs" Inherits="Aim.Examining.Web.FrmLeaveAppView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=800,height=600,scrollbars=yes");
        var EditPageUrl = "FrmLeaveAppEdit.aspx";
        var enumState = { '': '新建', 'null': '新建', 'Flowing': '流程中', 'End': '流程结束' };

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["LeaveList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'LeaveList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'Reason' },
			    { name: 'BeginTime' },
			    { name: 'EndTime' },
			    { name: 'LeaveUser' },
			    { name: 'AppState' },
			    { name: 'LeaveTime' },
			    { name: 'Number' },
			    { name: 'DeptName' },
			    { name: 'LeaveType' },
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
                { fieldLabel: '申请人', id: 'LeaveUser', schopts: { qryopts: "{ mode: 'Like', field: 'LeaveUser' }"}}]
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
                    autoExpandColumn: 'Reason',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '编号', width: 130, sortable: true, linkparams: { url: EditPageUrl, style: EditWinStyle} },
					{ id: 'LeaveUser', dataIndex: 'LeaveUser', header: '申请人', width: 100, sortable: true },
					{ id: 'BeginTime', dataIndex: 'BeginTime', header: '开始时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'EndTime', dataIndex: 'EndTime', header: '结束时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'DeptName', dataIndex: 'DeptName', header: '部门', width: 100, sortable: true },
					{ id: 'LeaveType', dataIndex: 'LeaveType', header: '请假类型', width: 100, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '状态', width: 100, enumdata: enumState, sortable: true },
					{ id: 'AppState', dataIndex: 'AppState', header: '审核状态', width: 100, sortable: true },
					{ id: 'Reason', dataIndex: 'Reason', header: '申请原因', width: 100, sortable: true },
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
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
