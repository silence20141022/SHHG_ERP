<%@ Page Title="产品信息" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductList.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.ProductList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
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
    </style>

    <script type="text/javascript">

        var EditWinStyle = CenterWin("width=1000,height=600,scrollbars=yes");
        var EditPageUrl = "ProductEdit.aspx";
        var productType = '';
        var store, myData, pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
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
			    { name: 'Id' }, { name: 'Code' }, { name: 'Name' }, { name: 'Pcn' }, { name: 'Unit' }, { name: 'FirstSkinIsbn' },
		        { name: 'FirstSkinCapacity' }, { name: 'SecondSkinIsbn' }, { name: 'SecondSkinCapacity' }, { name: 'IsProxy' },
			    { name: 'ProductType' }, { name: 'SalePrice' }, { name: 'Remark' }, { name: 'Isbn' }, { name: 'SupplierId' },
			    { name: 'SupplierName' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }
			    ],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || [];
                }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                 collapsed: false,
                columns: 5,
                items: [
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"} },
                { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} },
                //{ fieldLabel: '产品类型', id: 'ProductType', xtype: 'aimcombo', enumdata: AimState["ProductTypeEnum"], schopts: { qryopts: "{ mode: 'Like', field: 'ProductType' }"} },
                { fieldLabel: '供应商', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"}}
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
                }, '-',
                 {
                     text: '修改',
                     iconCls: 'aim-icon-edit',
                     handler: function() {
                         ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                     }
                 },
                '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, '->']
            });
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]                
            });
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'center',
                autoExpandColumn: 'SupplierName',
                forceFit: true,
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Name', dataIndex: 'Name', header: '产品名称', width: 180, sortable: true },
					{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 70, sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '产品型号', renderer: ExtGridpperCase, width: 150, sortable: true },
					{ id: 'Isbn', dataIndex: 'Isbn', header: '条形码', width: 130, sortable: true },
					{ id: 'PCN', dataIndex: 'Pcn', header: 'PCN', width: 100 },
					{ id: 'IsProxy', dataIndex: 'IsProxy', width: 60, header: '宏谷代理' },
					{ id: 'FirstSkinIsbn', dataIndex: 'FirstSkinIsbn', header: '包装箱条形码', width: 120 },
					{ id: 'FirstSkinCapacity', dataIndex: 'FirstSkinCapacity', header: '包装容量', width: 60 },
					{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商', width: 150, sortable: true },
                    { id: 'CreateTime', dataIndex: 'CreateTime', header: '登记时间', renderer: ExtGridDateOnlyRender, width: 80, sortable: true }
                    ],
                bbar: pgBar,
                tbar: titPanel,
                listeners: { "rowdblclick": function() {
                    ExtOpenGridEditWin(grid, EditPageUrl, "r", EditWinStyle);
                }
                }
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function onExecuted() {
            store.reload();
        } 
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
