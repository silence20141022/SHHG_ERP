<%@ Page Title="采购订单选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master"
    AutoEventWireup="true" CodeBehind="InvoiceSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.InvoiceSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var moneyType = $.getQueryString({ "ID": "MoneyType" });
        var supplierName = $.getQueryString({ "ID": "SupplierName" });
        var supplierId = $.getQueryString({ "ID": "SupplierId" });
        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["PurchaseOrderDetailList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'PurchaseOrderDetailList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'PurchaseOrderNo' }, { name: 'HaveInvoice' }, { name: 'NoInvoice' }, { name: 'ProductId' },
			    { name: 'Id' }, { name: 'PurchaseOrderId' }, { name: 'Code' }, { name: 'Name' }, { name: 'BuyPrice' }, { name: 'NoInvoiceAmount' },
			     { name: 'Quantity' }, { name: 'Amount' }
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

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 2,
                items: [
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"}}]
            });
            var buttonPanel = new Ext.form.FormPanel({
                region: 'south',
                frame: true,
                buttonAlign: 'center',
                buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                    window.close();
                } }]
                });
                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: ['<font color=red>说明： 双击行可以直接完成选择</font>', '->',
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
                    title: "『" + supplierName + "』" + "待关联发票采购单",
                    store: store,
                    bbar: pgBar,
                    region: 'center',
                    checkOnly: true,
                    autoExpandColumn: 'Code',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', hidden: true },
                    { id: 'PurchaseOrderId', dataIndex: 'PurchaseOrderId', hidden: true },
                    { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                    { id: 'PurchaseOrderNo', dataIndex: 'PurchaseOrderNo', header: '采购编号', width: 120, sortable: true },
                     new Ext.ux.grid.AimRowNumberer(), AimSelCheckModel,
                    { id: 'Name', dataIndex: 'Name', header: '产品名称', sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '型号', width: 100, sortable: true },
					{ id: 'BuyPrice', dataIndex: 'BuyPrice', header: '采购价', width: 70, sortable: true, renderer: filterValue },
					{ id: 'Quantity', dataIndex: 'Quantity', header: '采购数量', width: 70 },
					{ id: 'Amount', dataIndex: 'Amount', header: '采购金额', width: 70, sortable: true, renderer: filterValue },
					{ id: 'HaveInvoice', dataIndex: 'HaveInvoice', header: '已开发票数量', width: 90 },
					{ id: 'NoInvoice', dataIndex: 'NoInvoice', header: '未开发票数量', width: 90 },
					{ id: 'NoInvoiceAmount', dataIndex: 'NoInvoiceAmount', header: '未开金额', width: 70, sortable: true, renderer: filterValue }
                    ],
                    tbar: titPanel
                });
                // 页面视图{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 },
                viewport = new Ext.ux.AimViewport({
                    items: [AimSelGrid, buttonPanel]
                });
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
                    switch (moneyType) {
                        case '美元':
                            return '$' + whole;
                            break;
                        default:
                            return '￥' + whole;
                            break;
                    }
                }
            }            
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            采购订单选择</h1>
    </div>
</asp:Content>
