<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="StockInfoList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.StockInfoList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["StockList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'StockList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'Name' }, { name: 'ProductCode' }, { name: 'Isbn' }, { name: 'Pcn' }, { name: 'NoInQuan' }, { name: 'ProductId' },
			    { name: 'MinCount' }, { name: 'WarehouseId' }, { name: 'WarehouseName' }, { name: 'StockQuantity' }, { name: 'BuyPrice' }, { name: 'Rate' },
                { name: 'StockQuan' }, { name: 'NoOutQuan' }, { name: 'ProductType' }, { name: 'StockAmount'}],
                listeners: { "aimbeforeload": function (proxy, options) {
                    options.data = options.data || {};
                    options.data.op = pgOperation || null;
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
                columns: 6,
                items: [
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: 'PCN', id: 'PCN', schopts: { qryopts: "{ mode: 'Like', field: 'PCN' }"} },
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"} },
                { fieldLabel: '产品类型', id: 'ProductType', xtype: 'aimcombo', enumdata: AimState["ProductTypeEnum"], schopts: { qryopts: "{ mode: 'Like', field: 'ProductType' }"} },
                { fieldLabel: '仓库名称', id: 'WarehouseName', schopts: { qryopts: "{ mode: 'Like', field: 'WarehouseName' }"} }
                ]
            });
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '导出<label style=" font-family:@宋体">Excel</label>',
                    iconCls: 'aim-icon-xls',
                    handler: function () {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, '-', {
                    text: '查看出入库记录',
                    iconCls: 'aim-icon-chart',
                    handler: function () {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要查看出入库日志的记录！");
                            return;
                        }
                        ShowStockLog(recs[0].get("ProductId"), recs[0].get("WarehouseId"));
                    }
                }, '->'
                ]
            });
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            var columns = [{ id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'Rate', dataIndex: 'Rate', header: '汇率', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Name', dataIndex: 'Name', header: '产品名称', width: 110, sortable: true },
                    { id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', sortable: true, width: 200 },
                    { id: 'Isbn', dataIndex: 'Isbn', header: '条形码', width: 160, sortable: true },
                    { id: 'WarehouseName', dataIndex: 'WarehouseName', header: '仓库名称', width: 80, sortable: true },
                    { id: 'Pcn', dataIndex: 'Pcn', header: 'PCN', width: 160, sortable: true, summaryRenderer: function (v, params, data) { return "汇总:" } },
					{ id: 'StockQuantity', dataIndex: 'StockQuantity', header: '本仓库存', width: 80, sortable: true, summaryType: 'sum',
					    summaryRenderer: function (v, params, data) { return v }
					},

					{ id: 'StockQuan', dataIndex: 'StockQuan', header: '总库存', width: 80, sortable: true, renderer: RowRender },
                    { id: 'NoInQuan', dataIndex: 'NoInQuan', header: '待入库数量', width: 80, sortable: true, summaryType: 'sum' },
                    { id: 'NoOutQuan', dataIndex: 'NoOutQuan', header: '待出库数量', width: 80, sortable: true, summaryType: 'sum'}];
            if (AimState["UserInfo"].Name == '叶敏' || AimState["UserInfo"].Name == '陈燕萍') {
                columns.push({ id: 'StockAmount', dataIndex: 'StockAmount', header: '库存金额', width: 100, sortable: true, summaryType: 'sum', hidden: true,
                    summaryRenderer: function (v, params, data) {
                        var temp = Math.round(parseFloat(v) * 100) / 100;
                        return '￥' + filterValue(temp.toString());
                    }, renderer: RowRender
                });
            }
            // 表格面板
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'center',
                forceFit: true,
                autoExpandColumn: 'ProductCode',
                plugins: new Ext.ux.grid.GridSummary(),
                columns: columns,
                bbar: pgBar,
                tbar: titPanel
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        // 提交数据成功后
        function onExecuted() {
            store.reload();
        }
        function opencenterwin(url, name, iWidth, iHeight) {
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;ExamineResultView
            window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
        }
        function ShowStockLog(val1, val2) {
            var task = new Ext.util.DelayedTask();
            task.delay(100, function () {
                opencenterwin("StockLogList.aspx?ProductId=" + val1 + "&WarehouseId=" + val2, "", 1200, 650);
            });
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "StockQuan":
                    if (record.get("MinCount") >= value) {
                        rtn = "<label style='color:red;font-weight:bolder'>" + value + "</label>";
                    }
                    else {
                        rtn = "<label style='color:green;font-weight:bolder'>" + value + "</label>";
                    }
                    break;
                case "StockAmount":
                    if (value) {
                        temp = Math.round(value * 100) / 100;
                        return '￥' + filterValue(temp.toString());
                    }
                    break;
            }
            return rtn;
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
        function accMul(arg1, arg2) {
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
        }
        function filterValue(val) {
            if (val == null || val == "") {
                return null;
            }
            else {
                val = String(val);
                var whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return whole;
            }
        }  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
