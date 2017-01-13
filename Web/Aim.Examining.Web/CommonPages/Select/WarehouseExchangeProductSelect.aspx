<%@ Page Title="仓库调拨产品选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master"
    AutoEventWireup="true" CodeBehind="WarehouseExchangeProductSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.WarehouseExchangeProductSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, AimSelGrid, viewport;
        var warehouseId = $.getQueryString({ "ID": "WarehouseId" });
        function onSelPgLoad() {
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
			    { name: 'Id' }, { name: 'ProductId' }, { name: 'ProductCode' }, { name: 'ProductName' }, { name: 'ProductIsbn' }, { name: 'ProductType' },
			    { name: 'ProductPcn' }, { name: 'WarehouseId' }, { name: 'WarehouseName' }, { name: 'StockQuantity' }, { name: 'Remark' }
			],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || {};
                    options.data.WarehouseId = warehouseId;
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
                columns: 4,
                collapsed: false,
                items: [{ fieldLabel: '产品名称', id: 'ProductName', schopts: { qryopts: "{ mode: 'Like', field: 'ProductName' }"} },
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"} },
                { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} }
                ]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: ['<img src="../../images/shared/arrow_right1.png" /><font color=red>说明：双击记录也可以完成选择</font>', '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("ProductCode"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            setTimeout("viewport.doLayout()", 50);
                        }
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                var buttonPanel = new Ext.form.FormPanel({
                    region: 'south',
                    frame: true,
                    buttonAlign: 'center',
                    buttons: [{ text: '确定', handler: function() {
                        AimGridSelect();
                    }
                    }, { text: '取消', handler: function() {
                        window.close();
                    } }]
                    });
                    // 表格面板
                    AimSelGrid = new Ext.ux.grid.AimGridPanel({
                        store: store,
                        region: 'center',
                        autoExpandColumn: 'ProductCode',
                        columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'ProductId', dataIndex: 'ProductId', header: 'ProductId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
                    { id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 110, sortable: true },
                    { id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 70, sortable: true },
					{ id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', sortable: true },
					{ id: 'ProductIsbn', dataIndex: 'ProductIsbn', header: '条形码', width: 150, sortable: true },
                    { id: 'ProductPcn', dataIndex: 'ProductPcn', header: 'PCN', width: 150, sortable: true },
					{ id: 'StockQuantity', dataIndex: 'StockQuantity', header: '库存量', width: 70 }
                    ],
                        tbar: titPanel,
                        bbar: pgBar
                    });
                    // 页面视图
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
