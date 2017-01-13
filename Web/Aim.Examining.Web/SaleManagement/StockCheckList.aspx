<%@ Page Title="库存盘点" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="StockCheckList.aspx.cs" Inherits="Aim.Examining.Web.SaleManagement.StockCheckList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
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
        var StockCheckId = '';
        Ext.override(Ext.grid.GridView, {
            afterRender: Ext.grid.GridView.prototype.afterRender.createSequence(function() {
                this.fireEvent("viewready", this); //new event 
            })
        });
        var beforeRowExpand = function(expander, record, body, rowIndex) {
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
                gridEl.onclick = function(e) {
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
                fields: [{ name: 'Id' }, { name: 'StockCheckId' }, { name: 'StockCheckNo' }, { name: 'ProductId' }, { name: 'ProductName' },
                    	 { name: 'ProductCode' }, { name: 'ProductPcn' }, { name: 'StockQuantity' }, { name: 'StockCheckQuantity' },
                    	 { name: 'StockCheckState' }, { name: 'StockCheckResult'}],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || [];
                    options.data.StockCheckId = StockCheckId;
                    options.data.optype = "getChildData";
                }
                }
            });
            var gridX = new Ext.ux.grid.AimGridPanel({
                store: childstore,
                id: 'grid_' + record.get("Id"),
                columnLines: true,
                autoExpandColumn: 'ProductCode',
                columns: [
                  new Ext.ux.grid.AimRowNumberer(),
                  { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                  { id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 130 },
                  { id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', width: 200 },
                  { id: 'ProductPcn', dataIndex: 'ProductPcn', header: 'PCN', width: 130 },
	              { id: 'StockQuantity', dataIndex: 'StockQuantity', header: '本仓库存', width: 70 },
			      { id: 'StockCheckQuantity', dataIndex: 'StockCheckQuantity', header: '盘点库存', width: 70 },
                  { id: 'StockCheckState', dataIndex: 'StockCheckState', header: '盘点状态', width: 70 },
	              { id: 'StockCheckResult', dataIndex: 'StockCheckResult', header: '盘点结果', width: 70 }
	              ],
                viewConfig: {
                    forceFit: true
                },
                autoHeight: true
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
            StockCheckId = record.get("Id");
            childstore.reload();
            this.loaded = this.loaded || [];
            this.loaded[rowIndex] = gridX;
        });
        function expandAllRows() {
            var nRows = store.getCount();
            for (i = 0; i < nRows; i++)
                expander.expandRow(i);
        }
        var EditWinStyle = CenterWin("width=820,height=500,scrollbars=yes");
        var EditPageUrl = "StockCheckEdit.aspx";
        var enumState = { '': '新建', 'null': '新建', 'Flowing': '流程中', 'End': '流程结束' };
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var index = $.getQueryString({ "ID": "Index" });
        function onPgLoad() {
            setPgUI();
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
			    { name: 'Id' }, { name: 'StockCheckNo' }, { name: 'WarehouseName' }, { name: 'WarehouseId' }, { name: 'WorkFlowState' }, { name: 'StockCheckUserName' },
			    { name: 'State' }, { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'Result' },
			    { name: 'ExamineResult' }
			    ],
                listeners: { aimbeforeload: function(proxy, opitions) {
                    opitions.data = opitions.data || [];
                    opitions.data.Index = index;
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
                collapsed: false,
                columns: 5,
                items: [
                { fieldLabel: '盘点编号', id: 'StockCheckNo', schopts: { qryopts: "{ mode: 'Like', field: 'StockCheckNo' }"} },
                { fieldLabel: '盘点仓库', id: 'WarehouseName', schopts: { qryopts: "{ mode: 'Like', field: 'WarehouseName' }"} },
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: '按钮', xtype: 'button', iconCls: 'aim-icon-search', width: 60, margins: '2 0 0 0', text: '查 询', handler: function() {
                    Ext.ux.AimDoSearch(Ext.getCmp("StockCheckNo"));
                }
                }
                ]
            });

            // 工具栏
            if (index == '0') {
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

                            if (confirm("确定删除所选记录？")) {
                                ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
                            }
                        }
                    }, '-', { text: '提交审核', iconCls: 'aim-icon-execute',
                        handler: function() {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要审核的记录！");
                                return;
                            }
                            if (recs[0].get("WorkFlowState") == "Flowing") {
                                alert("流程中的记录不允许重复提交审核!"); return;
                            }
                            if (recs[0].get("Result") != "异常") {
                                alert("只有盘点结果异常的记录才能提交审批!"); return;
                            }
                            if (!confirm("确定要提交审核吗?提交后不能再修改!")) return;
                            Ext.getBody().mask("提交中,请稍后...");
                            var flowKey = "StockCheck";
                            jQuery.ajaxExec('SubmitExamine', { state: "Flowing", Id: recs[0].get("Id"), FlowKey: flowKey, ApprovalState: "已提交" }, function(rtn) {
                                //                            Ext.getBody().unmask(); //关键在这里,自动执行流程的脚步,见第二步,稍延迟以等待Task生成.
                                window.setTimeout("AutoExecuteFlow('" + rtn.data.FlowId + "')", 1000);
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
                    }, '->']
                });
            }
            // 工具标题栏
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar ? tlBar : '',
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
                     expander,
					{ id: 'StockCheckNo', dataIndex: 'StockCheckNo', header: '盘点编号', width: 130, sortable: true },
					{ id: 'WarehouseName', dataIndex: 'WarehouseName', header: '盘点仓库', width: 150, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '盘点状态', width: 80, sortable: true },
					{ id: 'WorkFlowState', dataIndex: 'WorkFlowState', header: '流程状态', width: 80, enumdata: enumState, sortable: true },
					{ id: 'ExamineResult', dataIndex: 'ExamineResult', header: '审核结果', width: 80, sortable: true },
					{ id: 'StockCheckUserName', dataIndex: 'StockCheckUserName', header: '盘点人', width: 80, sortable: true },
					{ id: 'Result', dataIndex: 'Result', header: '盘点结果', width: 80, sortable: true },
					{ id: 'CreateName', dataIndex: 'CreateName', header: '创建人', width: 80, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 200, renderer: RowRender }
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
                },
                plugins: expander
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
        function AutoExecuteFlow(flowid) {
            jQuery.ajaxExec('AutoExecuteFlow', { "FlowId": flowid }, function(rtn) {
                AimDlg.show("提交成功！");
                onExecuted();
                Ext.getBody().unmask();
            });
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "Operation":
                    if (value) {
                        rtn = " <img src='../images/shared/application_view_list.png' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PurchaseOrderView.aspx?id=" +
                       value + "\",\"wind\",\"" + EditWinStyle + "\")'>查看详细</label>";
                    }
                    break;
                case "Remark":
                    if (value) {
                        cellmeta.attr = "ext:qtitle=''" + " ext:qtip='" + value + "'";
                        rtn = value;
                    }
                    break;
            }
            return rtn;
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
