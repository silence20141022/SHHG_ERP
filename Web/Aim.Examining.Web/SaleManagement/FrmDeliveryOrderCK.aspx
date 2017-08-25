<%@ Page Title="出库单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmDeliveryOrderCK.aspx.cs" Inherits="Aim.Examining.Web.FrmDeliveryOrderCK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=1200,height=600,scrollbars=yes");
        var EditPageUrl = "FrmDeliveryOrderEdit.aspx";

        var ViewWinStyle = CenterWin("width=1200,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmDeliveryOrderEdit.aspx";

        var store, myData, type;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var index = 0;

        function onPgLoad() {
            setPgUI();

            if (store.getCount() > 0) {
                frameContent.location.href = "/StockManagement/OutWarehouseProductList.aspx?Id=" + store.getAt(0).get("Id");
            }

            if (grid) {
                grid.setTitle("出库产品数量：" + (AimState["quantity"] || "0"));
            }
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OrderList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OrderList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'PId' },
			    { name: 'Number' },
			    { name: 'CId' },
			    { name: 'CCode' },
			    { name: 'CName' },
			    { name: 'ExpectedTime' },
			    { name: 'CorrespondState' },
			    { name: 'CorrespondInvoice' },
			    { name: 'DeliveryOrder' },
			    { name: 'SalesmanId' },

			    { name: 'DeliveryType' },
			    { name: 'WarehouseId' },
			    { name: 'WarehouseName' },
			    { name: 'TotalMoneyHis' },
			    { name: 'TotalMoney' },
			    { name: 'Salesman' },
			    { name: 'Address' },
			    { name: 'Tel' },
			    { name: 'DeliveryMode' },

			    { name: 'Child' },
			    { name: 'Remark' },
			    { name: 'State' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
			    { name: 'CreateTime' }
			    ], listeners: { "aimbeforeload": function(proxy, options) {
			        options.data.type = type;
			    }, "load": function(p, v) {
			        if (grid && v.data) {
			            grid.setTitle("出库产品数量：" + (v.data.quantity || "0"));
			        }
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
                //collapsed: false,
                items: [
                { fieldLabel: '出库单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '出库类型', id: 'DeliveryType', schopts: { qryopts: "{ mode: 'Like', field: 'DeliveryType' }"} },
                { fieldLabel: '出库仓库', id: 'WarehouseName', schopts: { qryopts: "{ mode: 'Like', field: 'WarehouseName' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                {
                    text: '出库',
                    iconCls: 'aim-icon-edit',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要出库的记录！");
                            return;
                        }
                        if (recs[0].get("State") == "已出库") {
                            alert("选择的记录已出库!"); return;
                        }
                        ExtOpenGridEditWin(grid, EditPageUrl + "?type=ck", "u", EditWinStyle);
                    }
                }, '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, '-', {
                    text: '待出库',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        type = "wei";
                        grid.getColumnModel().setHidden(8, false);
                        store.reload();
                    }
                }, {
                    text: '已出库',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        type = "yi";
                        grid.getColumnModel().setHidden(8, true);
                        store.reload();
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
                    title: "产品数量",
                    store: store,
                    region: 'west',
                    split: true,
                    border: false,
                    width: 650,
                    autoExpandColumn: 'CName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '出库单号', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 120, sortable: true },
                    //{ id: 'CCode', dataIndex: 'CCode', header: '客户编号', width: 100, sortable: true },
					{id: 'CName', dataIndex: 'CName', header: '客户名称', width: 100, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '出库状态', width: 60, sortable: true, renderer: function(val) {
					    if (val == "已出库")
					        return '<label style="color:green;">已出库</label>';
					    if (!val)
					        return '<label style="color:red;">待出库</label>';
					    else
					        return '<label style="color:red;">' + val + '</label>';
					}
					},
					{ id: 'DeliveryType', dataIndex: 'DeliveryType', header: '出库类型', width: 60, sortable: true },
					{ id: 'WarehouseName', dataIndex: 'WarehouseName', header: '出库仓库', width: 80, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 70, renderer: ExtGridDateOnlyRender, sortable: true },
					{ dataIndex: 'Id', header: '出库 ', width: 60, sortable: true, renderer: function(val) {
					    return "<img src='../images/shared/device-add.gif' /><label style='color:blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"" + EditPageUrl + "?op=u&type=ck&id=" + val + "\",\"wind\",\"" + EditWinStyle + "\");'>出库</label>";
					}
					}
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    listeners: { "rowdblclick": function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            return;
                        }

                        ExtOpenGridEditWin(grid, EditPageUrl, "r", EditWinStyle);
                    }, "rowclick": function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            return;
                        }

                        //查看的分详细
                        frameContent.location.href = "/StockManagement/OutWarehouseProductList.aspx?Id=" + recs[0].get("Id");
                    }
                    }
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid, {
                        id: 'frmcon',
                        region: 'center',
                        margins: '0 0 0 0',
                        cls: 'empty',
                        bodyStyle: 'background:#f1f1f1',
                        html: '<iframe width="100%" height="100%" id="frameContent" src="" name="frameContent" frameborder="0"></iframe>'}]
                    });

                    schBar.toggleCollapse(false);
                    viewport.doLayout();
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
