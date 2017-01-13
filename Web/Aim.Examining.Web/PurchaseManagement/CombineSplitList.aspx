<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="CombineSplitList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.CombineSplitList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <%-- <style type="text/css">
        .x-grid3-cell-inner, .x-grid3-hd-inner
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
        }
    </style>--%>
    <style type="text/css">
        .x-grid3-row-expanded
        {
            border-width: 1px;
            border-color: Red;
        }
    </style>

    <script type="text/javascript" src="/js/ext/ux/RowExpander.js"></script>

    <script type="text/javascript">
        var CombineSplitId = '';
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
                dsname: 'DetailDataList',
                data: myData,
                id: 'store_' + record.get("Id"),
                idProperty: 'Id',
                fields: [{ name: 'Id' }, { name: 'ProductId' }, { name: 'ProductName' }, { name: 'CombineSplitId' },
                    	    { name: 'ProductCode' }, { name: 'ProductPcn' }, { name: 'ProductQuantity'}],

                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || [];
                    options.data.CombineSplitId = CombineSplitId;
                    options.data.optype = "getChildData";
                }
                }
            });
            var gridX = new Ext.ux.grid.AimGridPanel({
                store: childstore,
                id: 'grid_' + record.get("Id"),
                autoExpandColumn: 'ProductCode',
                columnLines: true,
                columns: [new Ext.ux.grid.AimRowNumberer(),
                  { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                  { id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 180 },
                  { id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号' },
                  { id: 'ProductPcn', dataIndex: 'ProductPcn', header: 'PCN', width: 100 },
                  { id: 'ProductQuantity', dataIndex: 'ProductQuantity', header: '操作数量', width: 80}],
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
            CombineSplitId = record.get("Id");
            childstore.reload();
            this.loaded = this.loaded || [];
            this.loaded[rowIndex] = gridX;
        });
        function expandAllRows() {
            var nRows = store.getCount();
            for (i = 0; i < nRows; i++)
                expander.expandRow(i);
        }
        var EditWinStyle = CenterWin("width=950,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl = "CombineSplitEdit.aspx";
        var store, myData
        var State = "";
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
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
			{ name: 'Id' }, { name: 'CombineSplitNo' }, { name: 'OperateType' }, { name: 'ProductId' }, { name: 'ProductName' },
			{ name: 'ProductCode' }, { name: 'ProductQuantity' }, { name: 'WarehouseId' }, { name: 'WarehouseName' },
		    { name: 'CombineSplitUserId' }, { name: 'CombineSplitUserName' }, { name: 'CreateId' }, { name: 'ProductPcn' },
			{ name: 'CreateName' }, { name: 'Remark' }, { name: 'CreateTime' }
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
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: 'PCN', id: 'PCN', schopts: { qryopts: "{ mode: 'Like', field: 'PCN' }"} },
                { fieldLabel: '产品名称', id: 'ProductName', schopts: { qryopts: "{ mode: 'Like', field: 'ProductName' }"} }
              ]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '添加',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                    }
                },
                '-',
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
                            ExtBatchOperate('batchdelete', recs, null, null, function(rtn) {
                                AimDlg.show("删除成功！"); store.reload();
                            });
                        }
                    }
                }, '-', {
                    text: '打印',
                    iconCls: 'aim-icon-printer',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择组装拆分单！");
                            return;
                        }
                        window.open('CombineSplitPrint.aspx?Id=' + recs[0].get("Id"), 'print', 'width=50,height=20,scrollbars=yes resizable=yes');
                        //    window.open('PrintOrder.aspx?PurchaseOrderId=' + recs[0].get("Id"), 'newwindow', EditWinStyle);
                    }
                },
             '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("ProductCode"));
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
                    autoExpandColumn: 'ProductCode',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'ProductId', dataIndex: 'ProductId', header: 'ProductId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                     expander,
					{ id: 'CombineSplitNo', dataIndex: 'CombineSplitNo', header: '组装拆分编号', width: 130, sortable: true, renderer: RowRender },
					{ id: 'OperateType', dataIndex: 'OperateType', header: '操作类型', sortable: true, width: 60 },
					{ id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 120 },
					{ id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', width: 200, renderer: RowRender },
					{ id: 'ProductPcn', dataIndex: 'ProductPcn', header: 'PCN', width: 100 },
					{ id: 'ProductQuantity', dataIndex: 'ProductQuantity', header: '操作数量', width: 60 },
                	{ id: 'WarehouseName', dataIndex: 'WarehouseName', header: '操作仓库', width: 80 },
                  	{ id: 'CreateName', dataIndex: 'CreateName', header: '创建人', width: 60, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建日期', width: 80, sortable: true, renderer: ExtGridDateOnlyRender },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 100}],
                    bbar: pgBar,
                    tbar: titPanel,
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
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "CombineSplitNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"CombineSplitView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "ProductCode":
                        if (value) {
                            value = value || "";
                            cellmeta.attr = 'ext:qtitle =""' + ' ext:qtip ="' + value + '"';
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
