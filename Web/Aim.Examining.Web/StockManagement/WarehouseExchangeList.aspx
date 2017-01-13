<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="WarehouseExchangeList.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.WarehouseExchangeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmLogisticsEdit.aspx";
        var EditWinStyle = CenterWin("width=950,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl = "WarehouseExchangeEdit.aspx";
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var payType = '';
        var ids = '';
        function onPgLoad() {
            setPgUI();
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
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
			    { name: 'Id' }, { name: 'FromWarehouseId' }, { name: 'FromWarehouseName' }, { name: 'ToWarehouseId' },
			    { name: 'ToWarehouseName' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' },
			    { name: 'Remark' }, { name: 'OutWarehouseState' }, { name: 'InWarehouseState' }, { name: 'ExchangeState' },
			    { name: 'EndTime' }, { name: 'ExchangeNo'}],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || [];
                    options.data.PayType = payType;
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
                columns: 4,
                items: [
                { fieldLabel: '调拨单号', id: 'ExchangeNo', schopts: { qryopts: "{ mode: 'Like', field: 'ExchangeNo' }"} },
                { fieldLabel: '调出仓库', id: 'FromWarehouseName', schopts: { qryopts: "{ mode: 'Like', field: 'FromWarehouseName' }"} },
                { fieldLabel: '调入仓库', id: 'ToWarehouseName', schopts: { qryopts: "{ mode: 'Like', field: 'ToWarehouseName' }"} },
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} }
                ]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '添加仓库调拨单',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                    }
                }, '-', {
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        var allow = true;
                        $.each(recs, function() {
                            if (this.get("InWarehouseState") == "已入库" || this.get("OutWarehouseState") == "已出库" || this.get("ExchangeState") == "已结束") {
                                allow = false;
                                return false;
                            }
                        })
                        if (!allow) {
                            AimDlg.show("已出库或者已入库或者已调拨结束的调拨单不能删除！");
                            return;
                        }
                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, function(rtn) {
                                AimDlg.show(rtn.data.Message); onExecuted();
                            });
                        }
                    }
                }, '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("ExchangeNo"));
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

                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Remark',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'ExchangeNo', dataIndex: 'ExchangeNo', header: '调拨单号', width: 130, sortable: true, renderer: RowRender },
					{ id: 'FromWarehouseName', dataIndex: 'FromWarehouseName', header: '调出仓库', width: 120, sortable: true },
					{ id: 'ToWarehouseName', dataIndex: 'ToWarehouseName', header: '调入仓库', width: 120 },
					{ id: 'OutWarehouseState', dataIndex: 'OutWarehouseState', header: '出库状态', width: 80 },
					{ id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 80 },
					{ id: 'ExchangeState', dataIndex: 'ExchangeState', header: '调拨状态', width: 80 },
					{ id: 'CreateName', dataIndex: 'CreateName', header: '创建人', width: 80 },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '开始时间', width: 80, renderer: ExtGridDateOnlyRender },
					{ id: 'EndTime', dataIndex: 'EndTime', header: '结束时间', width: 80, renderer: ExtGridDateOnlyRender },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 80 }
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    plugins: new Ext.ux.grid.GridSummary()
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }
            function filterValue(val) {
                val = String(val);
                var whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return '￥' + (whole == "null" || whole == null ? "" : whole);
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "ExchangeNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"WarehouseExchangeView.aspx?id=" +
                                      record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "PurchaseOrderAmount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = record.get("Symbo") + whole;
                        break;
                }
                return rtn;
            }
            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
    </div>
</asp:Content>
