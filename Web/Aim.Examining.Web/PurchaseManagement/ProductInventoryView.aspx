<%@ Page Title="商品库存" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductInventoryView.aspx.cs" Inherits="Aim.Examining.Web.ProductInventoryView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
        var EditPageUrl = "ProductInventoryEdit.aspx";

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["ProductList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'ProductList',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' },
			{ name: 'WarehouseId' },
			{ name: 'WarehouseName' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'Pcn' },
			{ name: 'Unit' },
			{ name: 'ProductType' },
			{ name: 'SalePrice' },
			{ name: 'Remark' },
			{ name: 'Isbn' },
			{ name: 'Unit' },
			{ name: 'SupplierId' },
			{ name: 'SupplierName' },
			{ name: 'StockQuantity' },
			{ name: 'CreateId' },
			{ name: 'CreateName' },
			{ name: 'CreateTime' }
			]
            });
            //store.sort('BeRoleName', 'ASC');

            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                items: [
                { fieldLabel: '规格型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '商品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '添加库存信息',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
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
                    autoExpandColumn: 'Code',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Name', dataIndex: 'Name', header: '产品名称', width: 100, sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '型号', width: 100, sortable: true },
					{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 100, sortable: true },
				    { id: 'PCN', dataIndex: 'Pcn', header: 'PCN', width: 100 },
				    { id: 'StockQuantity', dataIndex: 'StockQuantity', header: '库存', width: 100 },
				    { id: 'Unit', dataIndex: 'Unit', header: '单位', width: 60 },
					{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商', width: 120, sortable: true },
					{ id: 'Isbn', dataIndex: 'Isbn', header: '条码', width: 100, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '登记时间', renderer: ExtGridDateOnlyRender, width: 100, sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
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
