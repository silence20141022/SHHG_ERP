<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="ProductInWarehouse.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.ProductInWarehouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=950,height=600,scrollbars=yes ,resizable=yes");
        var EditWinStyle2 = CenterWin("width=630,height=550,scrollbars=yes ,resizable=yes");
        var Index = $.getQueryString({ "ID": "Index" });
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport, State;
        function onPgLoad() {
            setPgUI();
            viewport.doLayout();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["InWarehouse"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'InWarehouse',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'InWarehouseNo' }, { name: 'EstimatedArrivalDate' }, { name: 'PurchaseOrderId' }, { name: 'SupplierName' },
			    { name: 'State' }, { name: 'InWarehouseType' }, { name: 'CreateName' }, { name: 'CreateTime' }
			    ],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data = options.data || {};
                    options.data.Index = Index;
                }, load: function(p, v) {
                    if (grid && v.data) {
                        if (v.data.Result) {
                            grid.setTitle("【合计入库数量：" + v.data.Result.InTotal + "】【序列号数量：" + v.data.Result.SeriesTotal + "】【退换货数量：" + v.data.Result.ReturnTotal + "】【未出库序列号：" + v.data.Result.UnOutQuan + "】【已出库数量：" + v.data.Result.OutQuan + "】【库存：" + v.data.Result.StockQuan + "】");
                        }
                    }
                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: 120,
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 4,
                collapsed: false,
                items: [
                { fieldLabel: '入库编号', id: 'InWarehouseNo', schopts: { qryopts: "{ mode: 'Like', field: 'InWarehouseNo' }"} },
                { fieldLabel: '供应商', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: '按钮', xtype: 'button', iconCls: 'aim-icon-search', width: 60, margins: '2 30 0 0', text: '查 询', handler: function() {
                    Ext.ux.AimDoSearch(Ext.getCmp("ProductCode"));
                }
}]

                });
                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: [
                                 '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        schBar.toggleCollapse(false);
                        setTimeout("viewport.doLayout()", 50);
                    } }]
                });
                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    //tbar: tlBar,
                    items: [schBar]
                });
                // 表格面板
                var columnarray = [{ id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'InWarehouseNo', dataIndex: 'InWarehouseNo', header: '入库编号', width: 120, sortable: true, renderer: RowRender },
                   	{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商' },
                   	{ id: 'InWarehouseType', dataIndex: 'InWarehouseType', header: '入库类型', width: 60, sortable: true },
                    { id: 'CreateName', dataIndex: 'CreateName', header: '创建人 ', width: 50 },
                    { id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间 ', width: 80, renderer: ExtGridDateOnlyRender, sortable: true }
                   ];
                if (Index == 0) {
                    columnarray.push({ id: 'Operate', dataIndex: 'Id', header: '操作', width: 80, renderer: RowRender });
                }
                grid = new Ext.ux.grid.AimGridPanel({
                    title: '入库信息',
                    store: store,
                    region: 'west',
                    width: 700,
                    split: true,
                    autoExpandColumn: 'SupplierName',
                    columns: columnarray,
                    bbar: pgBar,
                    tbar: titPanel,
                    listeners: { rowdblclick: function(grid, rowindex, e) {
                        var url = "";
                        if (store.getAt(rowindex).get("InWarehouseType") == "采购入库") {
                            url = "/PurchaseManagement/InWarehouseView.aspx?id="
                        }
                        else {
                            url = "/PurchaseManagement/OtherInWarehouseView.aspx?id="
                        }
                        window.open(url + store.getAt(rowindex).get("Id"), "viewinfo", EditWinStyle);
                    }, rowclick: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (recs || recs.length > 0) {
                            frameContent.location.href = "InWarehouseProductList.aspx?InWarehouseId=" + recs[0].get("Id");
                        }
                    }
                    }
                });
                viewport = new Ext.ux.AimViewport({
                    items: [grid
                    , {
                        id: 'frmcon',
                        region: 'center',
                        margins: '-1 0 -2 0',
                        html: '<iframe width="100%" height="100%" id="frameContent" src="InWarehouseProductList.aspx" name="frameContent" frameborder="0"></iframe>'
                    }
                        ]
                });
                if (store.data.length > 0) {
                    frameContent.location.href = "InWarehouseProductList.aspx?InWarehouseId=" + store.getAt(0).get("Id");
                }
            }
            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
            function OpenInWarehouseBill(val, type) {
                if (type == "调拨入库") {
                    jQuery.ajaxExec("Judge", { id: val }, function(rtn) {
                        if (rtn.data.Allow) {
                            window.open("AcctualInWarehouse.aspx?id=" + val + "&op=u", "newwin", EditWinStyle);
                        }
                        else {
                            AimDlg.show("调拨入库的单据必须先出库！");
                            return;
                        }
                    });
                }
                else {
                    window.open("AcctualInWarehouse.aspx?id=" + val + "&op=u", "newwin", EditWinStyle);
                }
            }

            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Operate":
                        if (value) {
                            if (record.get("State") == "未入库") {
                                rtn = " <img src='../images/shared/device-add.gif' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='OpenInWarehouseBill(\"" + value + "\",\"" + record.get("InWarehouseType") + "\")'>产品入库</label>";
                                //                                rtn = " <img src='../images/shared/device-add.gif' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"AcctualInWarehouse.aspx?id=" +
                                //                       value + "&op=u\",\"wind\",\"" + EditWinStyle + "\")'>产品入库</label>";
                            }
                        }
                        break;
                    case "InWarehouseNo":
                        if (record.get("InWarehouseType") == "采购入库") {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../PurchaseManagement/InWarehouseEdit_New.aspx?op=view&id=" +
                        record.get("Id") + "\",\"wind\",\"" + EditWinStyle2 + "\")'>" + value + "</label>";
                        }
                        else {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../PurchaseManagement/OtherInWarehouseView.aspx?id=" +
                        record.get("Id") + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                }
                return rtn;
            }      
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
