<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    Title="库存盘点产品选择" CodeBehind="StockCheckProductSelect.aspx.cs" Inherits="Aim.Examining.Web.SaleManagement.StockCheckProductSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var QualificationEnum, OpPropertyEnum, QualificationGradeEnum;
        var myData, store, viewport, pgBar, titPanel;
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
			        { name: 'Id' }, { name: 'ProductName' }, { name: 'ProductCode' }, { name: 'ProductPcn' }, { name: 'StockQuantity' }
			    ],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data.WarehouseId = warehouseId;
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
                    collapsed: false,
                    columns: 4,
                    items: [
                     { fieldLabel: '产品名称', id: 'ProductName', schopts: { qryopts: "{ mode: 'Like', field: 'ProductName' }"} },
                     { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                     { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} },
                      { fieldLabel: '按钮', xtype: 'button', iconCls: 'aim-icon-search', width: 60, margins: '2 0 0 0', text: '查 询', handler: function() {
                          Ext.ux.AimDoSearch(Ext.getCmp("ProductCode"));
                      }
}]
                });
                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    //tbar: tlBar,
                    items: [schBar]
                });

                // 表格面板
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    title: '库存盘点产品选择',
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'ProductCode',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'StockCheckId', dataIndex: 'StockCheckId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
                    { id: 'ProductName', dataIndex: 'ProductName', header: '产品名称', width: 120, sortable: true },
					{ id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', width: 170, sortable: true, renderer: ExtGridpperCase },
					{ id: 'ProductPcn', dataIndex: 'ProductPcn', header: 'PCN', width: 130, sortable: true, renderer: ExtGridpperCase },
					{ id: 'StockQuantity', dataIndex: 'StockQuantity', header: '本仓库存', width: 70 }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
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
            标题</h1>
    </div>
</asp:Content>
