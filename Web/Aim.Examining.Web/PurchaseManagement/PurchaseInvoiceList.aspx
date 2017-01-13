<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="PurchaseInvoiceList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseInvoiceList" %>

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

    <script type="text/javascript" src="/js/ext/ux/RowExpander.js"></script>

    <script type="text/javascript">
        var PurchaseInvoiceId = '';
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
                fields: [{ name: 'Id' }, { name: 'ProductId' }, { name: 'ProductName' },
                    	    { name: 'ProductCode' }, { name: 'BuyPrice' }, { name: 'InvoiceQuantity' }, { name: 'InvoiceAmount'}],

                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || [];
                    options.data.PurchaseInvoiceId = PurchaseInvoiceId;
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
                  { id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 150 },
                  { id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号' },
                  { id: 'BuyPrice', dataIndex: 'BuyPrice', header: '价格', width: 100,
                      renderer: function(value, cellmeta, record2, rowIndex, columnIndex, store) {
                          return record.get("Symbo") + filterValue(value);
                      }
                  , summaryRenderer: function(v, params, data) { return "汇总:" }
                  },
	              { id: 'InvoiceQuantity', dataIndex: 'InvoiceQuantity', header: '数量', width: 100, summaryType: 'sum' },
			      { id: 'InvoiceAmount2', dataIndex: 'InvoiceAmount', header: '金额', width: 100, summaryType: 'sum', renderer:
			      function(value, cellmeta, record2, rowIndex, columnIndex, store) {
			          return record.get("Symbo") + filterValue(value);
			      },
			          summaryRenderer: function(v, params, data) {
			              return record.get("Symbo") + filterValue(v);
			          }
			      }
],
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
            PurchaseInvoiceId = record.get("Id");
            childstore.reload();
            this.loaded = this.loaded || [];
            this.loaded[rowIndex] = gridX;
        });
        function expandAllRows() {
            var nRows = store.getCount();
            for (i = 0; i < nRows; i++)
                expander.expandRow(i);
        }
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var EditPageUrl = "PurchaseInvoiceEdit.aspx";
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
                records: AimState["PurchaseInvoiceList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'PurchaseInvoiceList',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' }, { name: 'SupplierName' }, { name: 'MoneyType' }, { name: 'Symbo' },
			{ name: 'PurchaseInvoiceNo' }, { name: 'InvoiceNo' },
		    { name: 'InvoiceAmount' }, { name: 'SupplierId' }, { name: 'State' },
			{ name: 'CreateId' }, { name: 'CreateName' }, { name: 'Remark' }, { name: 'CreateTime' }
			],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || {};
                    options.data.State = State;
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
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: '发票号', id: 'InvoiceNo', schopts: { qryopts: "{ mode: 'Like', field: 'InvoiceNo' }"} },
                { fieldLabel: '供应商', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '开始时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '结束时间', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]

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
                                if (rtn.data.Allow == false) {
                                    AimDlg.show("该采购发票已关联有采购详细，拒绝删除！");
                                }
                                else {
                                    AimDlg.show("删除成功！"); store.reload();
                                }
                            });
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
                    //autoExpandColumn: 'SupplierName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'Symbo', dataIndex: 'Symbo', header: 'Symbo', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                     expander,
					{ id: 'PurchaseInvoiceNo', dataIndex: 'PurchaseInvoiceNo', header: '发票编号', width: 130, sortable: true, renderer: RowRender },
					{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商名称', sortable: true, width: 240 },
					{ id: 'InvoiceNo', dataIndex: 'InvoiceNo', header: '发票号', width: 120 },
					{ id: 'MoneyType', dataIndex: 'MoneyType', header: '交易币种', width: 80 },
					{ id: 'InvoiceAmount', dataIndex: 'InvoiceAmount', header: '发票金额', width: 100, sortable: true, renderer: RowRender },
                	{ id: 'State', dataIndex: 'State', header: '发票状态', width: 80 },
					{ id: 'CreateName', dataIndex: 'CreateName', header: '录入人', width: 80, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '录入日期', width: 120, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 120}],

                    bbar: pgBar,
                    tbar: titPanel,
                    plugins: expander,
                    listeners: { rowdblclick: function(grid, rowindex, e) {
                        window.open("PurchaseInvoiceView.aspx?id=" + store.getAt(rowindex).get("Id"), "viewinfo", EditWinStyle);
                    }
                    }
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
                if (val == null || val == "") {
                    return null;
                }
                else {
                    val = String(val);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    return whole;
                }
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "PurchaseInvoiceNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PurchaseInvoiceView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "InvoiceAmount":
                        if (value) {
                            rtn = record.get("Symbo") + filterValue(value);
                        }
                        break;
                }
                return rtn;
            }      
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
