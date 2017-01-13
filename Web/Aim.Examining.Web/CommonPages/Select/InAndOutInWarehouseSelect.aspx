<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="InAndOutInWarehouseSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.InAndOutInWarehouseSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, AimSelGrid, viewport;
        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["InWarehouseList"] || []
            };
            var buttonPanel = new Ext.form.FormPanel({
                region: 'south',
                frame: true,
                buttonAlign: 'center',
                buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                    window.close();
                } }]
                });
                // 表格数据源
                store = new Ext.ux.data.AimJsonStore({
                    dsname: 'InWarehouseList',
                    idProperty: 'Id',
                    data: myData,
                    fields: [
                			{ name: 'Id' }, { name: 'InWarehouseNo' }, { name: 'InWarehouseType' }, { name: 'State' }, { name: 'SupplierName' },
                    { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'Remark' }, { name: 'SupplierId' },
                    { name: 'EstimatedArrivalDate' }, { name: 'PublicInterface' }
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
                    items: [
                { fieldLabel: '名称', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '联系人', id: 'ContactPerson', schopts: { qryopts: "{ mode: 'Like', field: 'ContactPerson' }"}}]
                });

                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: ['<label style="color:red;">请选择供应商对其产品价格进行维护：</label>', '->',
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
                    store: store,
                    region: 'center',
                    //                    plugins: new Ext.ux.grid.GridSummary(),
                    autoExpandColumn: 'SupplierName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'InWarehouseNo', dataIndex: 'InWarehouseNo', header: '入库编号', width: 120 },
  					{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商名称' },
  					{ id: 'InWarehouseType', dataIndex: 'InWarehouseType', header: '入库类型', width: 80 },
				    { id: 'State', dataIndex: 'State', header: '状态', width: 70 },
   				    { id: 'CreateName', dataIndex: 'CreateName', header: '创建人', width: 80 },
  					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
                });
                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [AimSelGrid, buttonPanel]
                })
            }    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            入库单选择</h1>
    </div>
</asp:Content>
