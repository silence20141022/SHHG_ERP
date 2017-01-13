<%@ Page Title="组装拆分产品选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master"
    AutoEventWireup="true" CodeBehind="CombineSplitProductSelect.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.CombineSplitProductSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onSelPgLoad() {
            setPgUI();
        } 
        function setPgUI() { 
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            }; 
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'ProductName' }, { name: 'ProductCode' }, { name: 'Isbn' }, { name: 'Pcn' }, { name: 'StockQuantity', type: 'int' },
			    { name: 'ProductId' }, { name: 'WarehouseId' }, { name: 'WarehouseName' }
			]
            }); 
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
                schBar = new Ext.ux.AimSchPanel({
                    store: store,
                    collapsed: false,
                    columns: 4,
                    items: [
                    { fieldLabel: '产品名称', id: 'ProductName', schopts: { qryopts: "{ mode: 'Like', field: 'ProductName' }"} },
                    { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                    { fieldLabel: 'PCN', id: 'PCN', schopts: { qryopts: "{ mode: 'Like', field: 'PCN' }"} }                     
                ]
                }); 
                titPanel = new Ext.ux.AimPanel({ 
                    items: [schBar]
                }); 
                grid = new Ext.ux.grid.AimGridPanel({
                    title: '组装拆分产品选择',
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'ProductCode',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'WarehouseId', dataIndex: 'WarehouseId', header: 'WarehouseId', hidden: true },
                    { id: 'ProductId', dataIndex: 'ProductId', header: 'ProductId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
                    { id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 100, sortable: true },
					{ id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', renderer: ExtGridpperCase, width: 200, sortable: true },
					{ id: 'Pcn', dataIndex: 'Pcn', header: 'PCN', sortable: true, width: 130 },
					{ id: 'WarehouseName', dataIndex: 'WarehouseName', header: '仓库名称', width: 100, sortable: true },
					{ id: 'StockQuantity', dataIndex: 'StockQuantity', header: '本仓库存', width: 80 }
					],
                    bbar: pgBar,
                    tbar: titPanel
                });
                AimSelGrid = grid; 
                viewport = new Ext.ux.AimViewport({
                    items: [grid, buttonPanel]
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
