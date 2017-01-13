<%@ Page Title="出差申请" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmTravelApp.aspx.cs" Inherits="Aim.Examining.Web.FrmTravelApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=800,height=600,scrollbars=yes");
        var EditPageUrl = "FrmTravelAppEdit.aspx";
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
                records: AimState["TravelList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'TravelList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'Number' },
			    { name: 'DeptName' },
			    { name: 'Days' },
			    { name: 'Child' },
			    { name: 'Address' },
			    { name: 'Reason' },
			    { name: 'BeginTime' },
			    { name: 'EndTime' },
			    { name: 'LeaveUser' },
			    { name: 'WriteUser' },
			    { name: 'AppState' },
			    { name: 'LeaveTime' },
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
                { fieldLabel: '出差人', id: 'LeaveUser', schopts: { qryopts: "{ mode: 'Like', field: 'LeaveUser' }"}}]
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
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        if (recs[0].get("State") != null && recs[0].get("State") != "") {
                            alert("已经提交流程的记录不能修改!"); return;
                        }
                        ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
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
                            if (recs[i].get("State") != null && recs[i].get("State") != "") {
                                alert("已经提交流程的记录不能删除!"); return;
                            }
                        }
                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
                        }
                    }
                }, { text: '提交审批', id: 'btnSubmit', iconCls: 'aim-icon-execute',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要审批的记录！");
                            return;
                        }
                        if (recs[0].get("State") == "Flowing") {
                            alert("已经提交流程的记录不能修改!"); return;
                        }
                        if (!confirm("确定要提交审批吗?提交后不能再修改!")) return;
                        Ext.getBody().mask("提交中,请稍后...");
                        var flowKey = "TravelApp";

                        //                        jQuery.ajaxExec('submit', { "state": "Flowing", "Id": recs[0].get("Id"), "FlowKey": flowKey }, function() {
                        //                            onExecuted();
                        //                            AimDlg.show("提交成功！");
                        //                            Ext.getBody().unmask();
                        //                        });

                        jQuery.ajaxExec('submit', { state: "Flowing", Id: recs[0].get("Id"), FlowKey: flowKey }, function(rtn) {
                            window.setTimeout("AutoExecuteFlow('" + rtn.data.FlowId + "')", 500);
                        });
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
					{ id: 'LeaveUser', dataIndex: 'LeaveUser', header: '出差人', width: 100, sortable: true },
					{ id: 'BeginTime', dataIndex: 'BeginTime', header: '开始时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'Days', dataIndex: 'Days', header: '预计天数', width: 100, sortable: true },
					{ id: 'DeptName', dataIndex: 'DeptName', header: '部门', width: 100, sortable: true },
					{ id: 'Address', dataIndex: 'Address', header: '出差地点', width: 100, sortable: true },
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
