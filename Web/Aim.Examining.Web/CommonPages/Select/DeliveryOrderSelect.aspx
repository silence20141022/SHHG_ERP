<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="DeliveryOrderSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.DeliveryOrderSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        //        var moneyType = $.getQueryString({ "ID": "MoneyType" });
        //        var supplierName = $.getQueryString({ "ID": "SupplierName" });
        //        var supplierId = $.getQueryString({ "ID": "SupplierId" });
        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DeliveryOrderList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DeliveryOrderList',
                idProperty: 'Id',
                data: myData,
                fields: [
                //ExpectedTime
                //WarehouseId
                //WarehouseName
                //Remark
                //CreateId
                //CreateName
                //CreateTime
                //TotalMoneyHis
                //TotalMoney
                //CorrespondState
                //CorrespondInvoice
                //DeliveryType
			    {name: 'PId' }, { name: 'Id' }, { name: 'Number' }, { name: 'Child' }, { name: 'State' }, { name: 'DeliveryType' },
			    { name: 'CId' }, { name: 'CCode' }, { name: 'CName' }, { name: 'ExpectedTime' }
			],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || [];
                    // options.data.SupplierId = supplierId;
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
                collapsed: false,
                columns: 2,
                items: [
                { fieldLabel: '客户名称', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"}}]
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
                var buttonPanel = new Ext.form.FormPanel({
                    region: 'south',
                    frame: true,
                    buttonAlign: 'center',
                    buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                        window.close();
                    } }]
                    });
                    // 工具标题栏"" +
                    titPanel = new Ext.ux.AimPanel({
                        tbar: tlBar,
                        items: [schBar]
                    });
                    // 表格面板
                    AimSelGrid = new Ext.ux.grid.AimGridPanel({
                        title: "『待出库的销售单』",
                        store: store,
                        bbar: pgBar,
                        region: 'center',
                        autoExpandColumn: 'CName',
                        columns: [
                    { id: 'Id', dataIndex: 'Id', header: 'Id', hidden: true },
                     new Ext.ux.grid.AimRowNumberer(), AimSelCheckModel,
                    { id: 'Number', dataIndex: 'Number', header: '出库编号', width: 120 },
                    { id: 'CCode', dataIndex: 'CCode', header: '客户编号' },
                    { id: 'CName', dataIndex: 'CName', header: '客户名称', sortable: true, width: 180 },
                 	{ id: 'Child', dataIndex: 'Child', header: '详细', hidden: true },
                 	{ id: 'DeliveryType', dataIndex: 'DeliveryType', header: '提货方式' },
                 	{ id: 'ExpectedTime', dataIndex: 'ExpectedTime', header: '期待到货时间', width: 80 }
                                      ],
                        tbar: titPanel
                    });
                    // 页面视图{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 },
                    viewport = new Ext.ux.AimViewport({
                        items: [AimSelGrid, buttonPanel]
                    });
                }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            采购订单选择</h1>
    </div>
</asp:Content>
