<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="InWarehouseSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.InWarehouseSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var symbo = $.getQueryString({ "ID": "Symbo" });
        var supplierName = $.getQueryString({ "ID": "SupplierName" });
        var supplierId = $.getQueryString({ "ID": "SupplierId" });
        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["PurchaseOrderList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'PurchaseOrderList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'PurchaseOrderNo' }, { name: 'PurchaseOrderId' }, { name: 'BuyPrice' }, { name: 'Amount' },
			    { name: 'Name' }, { name: 'Code' }, { name: 'PCN' }, { name: 'Quantity' },
			    { name: 'HaveIn' }, { name: 'NoIn' }
			],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || [];
                    options.data.SupplierId = supplierId;
                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            var buttonPanel = new Ext.form.FormPanel({
                region: 'south',
                frame: true,
                buttonAlign: 'center',
                buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                    window.close();
                } }]
                });
                // 搜索栏
                schBar = new Ext.ux.AimSchPanel({
                    store: store,
                    columns: 2,
                    items: [{ fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} }
               ]
                });

                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: ['<img src="../../images/shared/arrow_right1.png" /><font color=red>说明： 双击行可以直接完成选择</font>', '->',
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
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    title: '供应商：『' + supplierName + '』待入库采购单详细',
                    store: store,
                    bbar: pgBar,
                    plugins: new Ext.ux.grid.GridSummary(),
                    region: 'center',
                    autoExpandColumn: 'Code',
                    columns: [new Ext.ux.grid.AimRowNumberer(), AimSelCheckModel,
                    { id: 'Id', dataIndex: 'Id', hidden: true },
                    { id: 'PurchaseOrderId', dataIndex: 'PurchaseOrderId', hidden: true },
                    { id: 'PurchaseOrderNo', dataIndex: 'PurchaseOrderNo', header: '采购编号', width: 120 },
                    { id: 'Name', dataIndex: 'Name', header: '产品名称', width: 100 },
					{ id: 'Code', dataIndex: 'Code', header: '产品型号', width: 120 },
					{ id: 'PCN', dataIndex: 'PCN', header: 'PCN', summaryRenderer: function(v, params, data) { return "汇总:" }, width: 80 },
                	{ id: 'Quantity', dataIndex: 'Quantity', header: '采购数量', width: 70, summaryType: 'sum' },
                	{ id: 'BuyPrice', dataIndex: 'BuyPrice', header: '采购价格', renderer: RowRender, width: 60 },
                	{ id: 'Amount', dataIndex: 'Amount', header: '金额', renderer: RowRender, summaryType: 'sum', width: 60 },
                	{ id: 'HaveIn', dataIndex: 'HaveIn', header: '已入库数量', width: 70, summaryType: 'sum' },
                	{ id: 'NoIn', dataIndex: 'NoIn', header: '未入库数量', width: 70, summaryType: 'sum'}],
                    tbar: titPanel
                });
                viewport = new Ext.ux.AimViewport({
                    items: [AimSelGrid, buttonPanel]
                });
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "BuyPrice":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = symbo + whole;
                        break;
                    case "PayQuantity":
                        rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
                        break;
                    case "Amount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = symbo + whole;
                        break;
                    default: //因为有汇总插件存在 所以存在第三种情形
                        rtn = value;
                        break;
                }
                return rtn;
            }         
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            入库产品选择</h1>
    </div>
</asp:Content>
