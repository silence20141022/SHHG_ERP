<%@ Page Title="采购产品选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master"
    AutoEventWireup="true" CodeBehind="PurchaseProductSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.PurchaseProductSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        Ext.override(Ext.grid.CheckboxSelectionModel, {
            handleMouseDown: function(g, rowIndex, e) {
                if (e.button !== 0 || this.isLocked()) {
                    return;
                }
                var view = this.grid.getView();
                if (e.shiftKey && !this.singleSelect && this.last !== false) {
                    var last = this.last;
                    this.selectRange(last, rowIndex, e.ctrlKey);
                    this.last = last; // reset the last     
                    view.focusRow(rowIndex);
                } else {
                    var isSelected = this.isSelected(rowIndex);
                    if (isSelected) {
                        this.deselectRow(rowIndex);
                    } else if (!isSelected || this.getCount() > 1) {
                        this.selectRow(rowIndex, true);
                        view.focusRow(rowIndex);
                    }
                }
            }
        });
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, AimSelGrid, viewport;
        var supplierId = $.getQueryString({ "ID": "SupplierId" });
        var supplierName = unescape($.getQueryString({ "ID": "SupplierName" }));
        var productType = unescape($.getQueryString({ "ID": "ProductType" }));
        var priceType = $.getQueryString({ "ID": "PriceType" });
        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() { 
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["ProductList"] || []
            }; 
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'ProductList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'Isbn' }, { name: 'Code' }, { name: 'Name' }, { name: 'Unit' }, { name: 'SupplierName' },
			    { name: 'StockQuantity' }, { name: 'Price' }, { name: 'Remark' }, { name: 'Pcn' }, { name: 'Isbn' }, { name: 'ProductType' }
			],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || {};
                    options.data.SupplierId = supplierId;
                    options.data.ProductType = productType;
                    options.data.PriceType = priceType;
                }
                }
            }); 
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 4,
                collapsed: false,
                items: [{ fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"} },
                { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} } 
                ]
            }); 
            titPanel = new Ext.ux.AimPanel({
                items: [schBar]
            });
            var buttonPanel = new Ext.form.FormPanel({
                region: 'south',
                frame: true,
                buttonAlign: 'center',
                buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                    window.close();
                } }]
                }); 
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    title: '供应商：『' + supplierName + '』',
                    store: store,
                    region: 'center', 
                    autoExpandColumn: 'Code',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'Price', dataIndex: 'Price', header: 'Price', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
                    { id: 'Name', dataIndex: 'Name', header: '产品名称', width: 110, sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '产品型号', sortable: true },
					{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 70, sortable: true },
                    { id: 'PCN', dataIndex: 'Pcn', header: 'PCN', width: 110, sortable: true },
                    { id: 'ISBN', dataIndex: 'Isbn', header: '条形码', width: 110, sortable: true },
					{ id: 'StockQuantity', dataIndex: 'StockQuantity', header: '库存量', width: 70 },
                  	{ id: 'Unit', dataIndex: 'Unit', header: '单位', width: 70 }
                    ],
                    tbar: titPanel,
                    bbar: pgBar
                }); 
                viewport = new Ext.ux.AimViewport({
                    items: [AimSelGrid, buttonPanel]
                });
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            采购产品选择</h1>
    </div>
</asp:Content>
