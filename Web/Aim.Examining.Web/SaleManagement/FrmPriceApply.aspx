<%@ Page Title="销售价格申请单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmPriceApply.aspx.cs" Inherits="Aim.Examining.Web.FrmPriceApply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var EditPageUrl = "FrmPriceApplyEdit.aspx";

        var ViewWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmPriceApplyEdit.aspx";

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
                records: AimState["PriceApplyList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'PriceApplyList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'Number' },
			    { name: 'CId' },
			    { name: 'CCode' },
			    { name: 'CName' },
			    { name: 'ExpectedTime' },
			    { name: 'Child' },
			    { name: 'Reason' },
			    { name: 'Remark' },
			    { name: 'State' },
			    { name: 'ApprovalState' },
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
                padding: '2 0 0 0',
                items: [
                { fieldLabel: '单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
//                {
//                    text: '添加',
//                    hidden: true,
//                    iconCls: 'aim-icon-add',
//                    handler: function() {
//                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
//                    }
//                }, {
//                    text: '修改',
//                    hidden: true,
//                    iconCls: 'aim-icon-edit',
//                    handler: function() {
//                        var recs = grid.getSelectionModel().getSelections();
//                        if (!recs || recs.length <= 0) {
//                            AimDlg.show("请先选择要修改的记录！");
//                            return;
//                        }
//                        if (recs[0].get("State") == "Flowing") {
//                            alert("已经提交流程的记录不能修改!"); return;
//                        }
//                        ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
//                    }
//                }, {
//                    text: '删除',
//                    hidden: true,
//                    iconCls: 'aim-icon-delete',
//                    handler: function() {
//                        var recs = grid.getSelectionModel().getSelections();
//                        if (!recs || recs.length <= 0) {
//                            AimDlg.show("请先选择要删除的记录！");
//                            return;
//                        }
//                        for (var i = 0; i < recs.length; i++) {
//                            if (recs[i].get("State") == "Flowing") {
//                                alert("已经提交流程的记录不能删除!"); return;
//                            }
//                        }
//                        if (confirm("确定删除所选记录？")) {
//                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
//                        }
//                    }
//                },
                { text: '提交审批', id: 'btnSubmit', iconCls: 'aim-icon-execute',
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
                        var flowKey = "PriceApply";

                        //                        jQuery.ajaxExec('submit', { "state": "Flowing", "Id": recs[0].get("Id"), "FlowKey": flowKey }, function() {
                        //                            onExecuted();
                        //                            AimDlg.show("提交成功！");
                        //                            Ext.getBody().unmask();
                        //                        });

                        jQuery.ajaxExec('submit', { state: "Flowing", Id: recs[0].get("Id"), FlowKey: flowKey }, function(rtn) {
                            window.setTimeout("AutoExecuteFlow('" + rtn.data.FlowId + "')", 1000);
                        });
                    }
                }, {
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
                }, {
                    text: '生成销售单',
                    hidden: true,
                    iconCls: 'aim-icon-execute',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }
                        if (recs[0].get("ApprovalState") != "同意") {
                            alert("您选择的价格申请单未审核通过，不能生成销售单");
                            return;
                        }

                        ExtOpenGridEditWin(grid, "FrmOrdersEdit.aspx?paid=" + recs[0].get("Id"), "c", "width=1000,height=600,scrollbars=yes");
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
					{ id: 'Number', dataIndex: 'Number', header: '单号', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 150, sortable: true },
                    //{ id: 'CCode', dataIndex: 'CCode', header: '客户编号', width: 130, sortable: true },
					{id: 'CName', dataIndex: 'CName', header: '客户名称', width: 300, sortable: true },
					{ id: 'ExpectedTime', dataIndex: 'ExpectedTime', header: '预计采购时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '状态', width: 100, enumdata: enumState, sortable: true },
					{ id: 'ApprovalState', dataIndex: 'ApprovalState', header: '审核状态', width: 100, sortable: true,
					    renderer: function(val) {
					        if (val == "同意") {
					            return "<label style='color:green;'>同意</label>";
					        }
					        else if (val == "不同意") {
					            return "<label style='color:red;'>不同意</label>";
					        }
					        else {
					            return val;
					        }
					    }
					},
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
                    if (rtn.data.error) {
                        AimDlg.show(rtn.data.error);
                    }
                    else {
                        AimDlg.show("提交成功！");
                    }
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
