<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="TaskList.aspx.cs" Inherits="Aim.Portal.Web.WorkFlow.TaskList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
        var EditPageUrl = "TaskEdit.aspx";
        var status = $.getQueryString({ ID: "Status", DefaultValue: "" });
        var EnumType = { '4': '已审批', '0': '待审批' };
        var comboxData = [[3, '三天内'], [7, '一周内'], [14, '二周内'], [30, '一个月内'], [31, '一个月以上']];
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["SysWorkFlowTaskList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SysWorkFlowTaskList',
                idProperty: 'ID',
                data: myData,
                fields: [
			{ name: 'ID' },
			{ name: 'Title' },
			{ name: 'Description' },
			{ name: 'OwnerId' },
			{ name: 'OwnerName' },
			{ name: 'Action' },
			{ name: 'WorkFlowInstanceId' },
			{ name: 'WorkFlowName' },
			{ name: 'EFormName' },
			{ name: 'ApprovalNodeName' },
			{ name: 'GroupId' },
			{ name: 'ApprovalNodeMathConditionType' },
			{ name: 'BookmarkName' },
			{ name: 'CreatedTime' },
			{ name: 'FinishTime' },
			{ name: 'Status' },
			{ name: 'Context' },
			{ name: 'Result' }
			],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data = options.data || {};
                    options.data.Status = status;
                    options.data.Date = $("#id_SubmitStateH").val();
                }
                },
                sortInfo: {
                    field: 'CreatedTime',
                    direction: 'Desc'
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
                items: [
                { fieldLabel: '标题', id: 'Title', schopts: { qryopts: "{ mode: 'Like', field: 'Title' }"} },
                { fieldLabel: '流程名', id: 'WorkFlowName', schopts: { qryopts: "{ mode: 'Like', field: 'WorkFlowName' }"} },
                { fieldLabel: '环节名', id: 'ApprovalNodeName', schopts: { qryopts: "{ mode: 'Like', field: 'ApprovalNodeName' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [new Ext.Toolbar.TextItem('查看：'), {
                    xtype: 'combo',
                    name: 'submitState',
                    id: 'id_SubmitState',
                    hiddenName: 'id_SubmitStateH',
                    triggerAction: 'all',
                    forceSelection: true,
                    lazyInit: false,
                    editable: false,
                    allowBlank: false,
                    width: 90,
                    store: new Ext.data.SimpleStore({
                        fields: ["retrunValue", "displayText"],
                        data: comboxData
                    }),
                    mode: 'local',
                    value: '3',
                    valueField: "retrunValue",
                    displayField: "displayText",
                    listeners: {
                        select: function() {
                            reloadPage(null);
                            try {
                            } catch (ex) {

                            }
                        }
                    }, anchor: '99%'
                }, {
                    text: '启动测试流程',
                    hidden: true,
                    iconCls: 'aim-icon-execute',
                    handler: SubmitFlow
                }, '->', {
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
                    autoExpandColumn: 'Title',
                    columns: [
                    { id: 'ID', header: '标识', dataIndex: 'ID', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'execute', header: '执行', renderer: RenderImg, width: 50, sortable: true, fixed: true, menuDisabled: true },
					{ id: 'Title', dataIndex: 'Title', header: '标题', width: 150, sortable: true },
					{ id: 'WorkFlowName', dataIndex: 'WorkFlowName', header: '流程名称', width: 200, sortable: true },
					{ id: 'ApprovalNodeName', dataIndex: 'ApprovalNodeName', header: '环节名称', width: 150, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreatedTime', header: '分发时间', width: 120, sortable: true },
					{ id: 'FinishTime', dataIndex: 'FinishTime', header: '完成时间', width: 120, sortable: true },
					{ id: 'Status', dataIndex: 'Status', header: '状态', width: 120, sortable: true, enumdata: EnumType }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    layout: 'border',
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid]
                });
            }
            function RenderImg(val, p, rec) {
                switch (this.id) {
                    case "execute":
                        return "<img src='/images/shared/arrow_turnback.gif' style='cursor:hand' onclick=\"ExecuteTask('" + rec.id + "')\"/>";
                        break;
                }
            }
            var WinStyle = CenterWin("width=800,height=600,scrollbars=yes");
            function ExecuteTask(taskId) {
                if (status == "1") {
                    alert("环节已审批!"); return;
                }
                OpenWin("/WorkFlow/TaskExecute.aspx?TaskId=" + taskId, "_blank", WinStyle);
            }
            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }

            function reloadPage(args) {
                // 重新加载页面
                status = args ? args.cid : status;
                store.reload();
            }

            function SubmitFlow() {
                Ext.getBody().mask("流程启动中,请稍后...");
                $.ajaxExec('startflow', { id: "1", tid: "2" },
                function(args) {
                    Ext.getBody().unmask();
                    reloadPage();
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
