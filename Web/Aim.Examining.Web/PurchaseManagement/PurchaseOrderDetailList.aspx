<%@ Page Title="采购明细" Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="PurchaseOrderDetailList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseOrderDetailList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=1050,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl = "PurchaseOrderEdit.aspx";
        var store, myData;
        var productType = "";
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
            $("#label1").text("当前显示的产品类型：全部");
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
			    { name: 'Id' }, { name: 'PriceType' }, { name: 'PurchaseOrderNo' }, { name: 'RMBAmount' }, { name: 'Symbo' },
                { name: 'SupplierName' }, { name: 'BuyPrice' }, { name: 'Name' }, { name: 'Code' }, { name: 'ProductType' },
                { name: 'Pcn' }, { name: 'Quantity' }, { name: 'InvoiceState' }, { name: 'MoneyType' }, { name: 'Amount' }, { name: 'PurchaseOrderId' },
                { name: 'PayState' }, { name: 'InWarehouseState' }, { name: 'CreateId' }, { name: 'CreateTime' }, { name: 'CreateName'}],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data = options.data || {};
                    options.data.op = pgOperation || null;
                    options.data.ProductType = productType;
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
                columns: 5,
                items: [
                { fieldLabel: '采购单号', id: 'PurchaseOrderNo', schopts: { qryopts: "{ mode: 'Like', field: 'PurchaseOrderNo' }"} },
                { fieldLabel: '供应商', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} },
                { fieldLabel: '下单人', id: 'CreateName', schopts: { qryopts: "{ mode: 'Like', field: 'CreateName' }"} },
                { fieldLabel: '下单时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '销售单' });
                    }
                }, '-',
                                           {
                                               text: '压缩机',
                                               iconCls: 'aim-icon-search',
                                               handler: function() {
                                                   productType = "压缩机"; store.reload();
                                                   productType = "";
                                               }
                                           }, '-', {
                                               text: '配件',
                                               iconCls: 'aim-icon-search',
                                               handler: function() {
                                                   productType = "配件"; store.reload();
                                                   productType = "";
                                               }
                                           },
                                            '-', {
                                                text: '全部',
                                                iconCls: 'aim-icon-search',
                                                handler: function() {
                                                    productType = ""; store.reload();
                                                }
                                            },
                                            '<img src="../images/shared/arrow_right.gif" /><label id="label1" style="color:#FF8C69;font-weight:bolder"></label>', '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("Code"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            viewport.doLayout();
                        }
                    } }]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    title: '采购详细报表',
                    store: store,
                    region: 'center',
                    //  forceFit: true,
                    forceLayout: true,
                    plugins: new Ext.ux.grid.GridSummary(),
                    //autoExpandColumn: 'SupplierName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'PurchaseOrderId', dataIndex: 'PurchaseOrderId', header: '标识', hidden: true },
                    { id: 'Symbo', dataIndex: 'Symbo', header: 'Symbo', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'PurchaseOrderNo', dataIndex: 'PurchaseOrderNo', header: '采购编号', width: 130, sortable: true, renderer: RowRender },
					{ id: 'Name', dataIndex: 'Name', header: '产品名称', width: 120, sortable: true },
					{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 60, sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '产品型号', width: 180, sortable: true },
					{ id: 'Pcn', dataIndex: 'Pcn', header: 'PCN', width: 150, sortable: true },
					{ id: 'BuyPrice', dataIndex: 'BuyPrice', header: '价格', width: 100, summaryRenderer: function(v, params, data) { return "汇总:" }, renderer: RowRender },
					{ id: 'Quantity', dataIndex: 'Quantity', header: '数量', width: 50, summaryType: 'sum' },
					{ id: 'Amount', dataIndex: 'Amount', header: '金额', width: 90, renderer: RowRender },
                    { id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 65, sortable: true },
				    { id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 65, sortable: true },
                    { id: 'InvoiceState', dataIndex: 'InvoiceState', header: '发票状态', width: 65, sortable: true },
			    	{ id: 'CreateName', dataIndex: 'CreateName', header: '下单人 ', width: 60, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '下单时间 ', width: 70, sortable: true, renderer: ExtGridDateOnlyRender },
              		{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商', sortable: true, width: 150}],
                    bbar: pgBar,
                    tbar: titPanel
                });
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }
            function onExecuted() {
                store.reload();
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "PurchaseOrderNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PurchaseOrderView.aspx?id=" +
                                      record.get('PurchaseOrderId') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "Amount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = record.get("Symbo") + whole;
                        break;
                    case "BuyPrice":
                        if (value) {
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = record.get("Symbo") + whole;
                        }
                        break;
                    default:
                        rtn = value;
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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
