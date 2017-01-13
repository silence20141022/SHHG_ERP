<%@ Page Title="成本核算明细" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="CostCheckList.aspx.cs" Inherits="Aim.Examining.Web.FinanceManagement.CostCheckList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, productType = '';
        var ViewWinStyle = CenterWin("width=1050,height=600,scrollbars=yes,resizable=yes");
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
            $("#label1").text("当前显示的产品类型：全部");
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
			    { name: 'Id' }, { name: 'Number' }, { name: 'CName' }, { name: 'PName' }, { name: 'PCode' }, { name: 'Isbn' }, { name: 'ProductType' },
			    { name: 'Pcn' }, { name: 'Count' }, { name: 'Amount' }, { name: 'Unit' }, { name: 'SalePrice' }, { name: 'OutCount' }, { name: 'CostAmount' },
			    { name: 'CreateTime' }, { name: 'Salesman' }, { name: 'BuyPrice' }, { name: 'EndDate' }, { name: 'Rate' }, { name: 'CostPrice' }, { name: 'OId' },
			    { name: 'InvoiceNumber'}],
                listeners: { aimbeforeload: function(proxy, options) {
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
                columns: 4,
                collapsed: false,
                items: [
                { fieldLabel: '销售单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '产品名称', id: 'PName', schopts: { qryopts: "{ mode: 'Like', field: 'PName' }"} },
                { fieldLabel: '产品型号', id: 'PCode', schopts: { qryopts: "{ mode: 'Like', field: 'PCode' }"} },
                { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} },
                { fieldLabel: '负责人', id: 'Salesman', schopts: { qryopts: "{ mode: 'Like', field: 'Salesman' }"} },
                { fieldLabel: '下单时间', id: 'BeginDate1', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate1', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate1' }"} },
                { fieldLabel: '至', id: 'EndDate1', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate1', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate1' }"} },
                { fieldLabel: '结束时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
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
                { text: '压缩机', iconCls: 'aim-icon-search',
                    handler: function() {
                        productType = '压缩机'; $("#label1").text("当前显示的产品类型：" + productType);
                        store.reload();
                    }

                }, '-',
                 { text: '配件', iconCls: 'aim-icon-search',
                     handler: function() {
                         productType = '配件'; $("#label1").text("当前显示的产品类型：" + productType);
                         store.reload();
                     }

                 },
                  { text: '全部', iconCls: 'aim-icon-search',
                      handler: function() {
                          productType = ''; $("#label1").text("当前显示的产品类型：全部");
                          store.reload();
                      }
                  }, '<img src="../images/shared/arrow_right.gif" /><label id="label1" style="color:#FF8C69;font-weight:bolder"></label>',
                 '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("PCode"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            viewport.doLayout();
                        }
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                var columnsarray = [
                    { id: 'Id', dataIndex: 'Id', header: '标识1', hidden: true },
                    { id: 'OId', dataIndex: 'OId', header: '标识2', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Number', dataIndex: 'Number', header: '销售单号', width: 130, sortable: true, renderer: RowRender },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 200, sortable: true },
					{ id: 'Salesman', dataIndex: 'Salesman', header: '销售负责人', width: 80, sortable: true },
					{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 80 },
					{ id: 'PName', dataIndex: 'PName', header: '产品名称', width: 100, sortable: true },
					{ id: 'PCode', dataIndex: 'PCode', header: '产品型号', width: 170, sortable: true },
					{ id: 'Pcn', dataIndex: 'Pcn', header: 'PCN', width: 130, sortable: true },
					{ id: 'SalePrice', dataIndex: 'SalePrice', header: '销售价格', width: 70, sortable: true, renderer: RowRender,
					    summaryRenderer: function(v, params, data) { return "汇总:" }
					},
					{ id: 'Count', dataIndex: 'Count', header: '数量', width: 60, sortable: true, summaryType: 'sum' },
					{ id: 'CostPrice', dataIndex: 'CostPrice', header: '成本价格', sortable: true, width: 70, renderer: RowRender },
					{ id: 'CostAmount', dataIndex: 'CostAmount', header: '成本金额', width: 100, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function(v, params, data) {
					        var temp = Math.round(parseFloat(v) * 100) / 100;
					        return '￥' + filterValue(temp);
					    }
					},
					{ id: 'Amount', dataIndex: 'Amount', header: '销售金额', width: 100, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function(v, params, data) {
					        var temp = Math.round(parseFloat(v) * 100) / 100;
					        return '￥' + filterValue(temp);
					    }
					},
					{ id: 'InvoiceNumber', dataIndex: 'InvoiceNumber', header: '发票号', width: 80, sortable: true },
					{ id: 'BuyPrice', dataIndex: 'BuyPrice', header: '采购价', hidden: true },
					{ id: 'Rate', dataIndex: 'Rate', header: '汇率', hidden: true },
                    { id: 'CreateTime', dataIndex: 'CreateTime', header: '下单时间', width: 100, sortable: true, renderer: ExtGridDateOnlyRender },
					{ id: 'EndDate', dataIndex: 'EndDate', header: '结束时间', width: 100, sortable: true, renderer: ExtGridDateOnlyRender }
                    ];
                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    plugins: new Ext.ux.grid.GridSummary(),
                    //autoExpandColumn: 'PCode',
                    gridLine: true,
                    columns: columnsarray,
                    bbar: pgBar,
                    cls: 'grid-row-span',
                    tbar: titPanel
                });

                // 页面视图EDS
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }

            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
            function filterValue(val) {
                if (val) {
                    val = String(val);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    return whole;
                }
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Number":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../SaleManagement/SaleOrderView.aspx?id=" +
                                      record.get('OId') + "\",\"wind\",\"" + ViewWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "ProfitRate":
                        if (value) {
                            rtn = Math.round(parseFloat(value) * 10000) / 100;
                            if (rtn >= 100) {
                                rtn = '<font color="green">' + rtn + '%' + '</font>';
                            }
                            else {
                                rtn = '<font color="red">' + rtn + '%' + '</font>';
                            }
                        }
                        break;
                    case "SalePrice":
                        if (value) {
                            rtn = '￥' + filterValue(value);
                        }
                        break;
                    case "Amount":
                        if (value) {
                            rtn = '￥' + filterValue(value);
                        }
                        break;
                    case "CostPrice":
                        if (value) {
                            rtn = '￥' + filterValue(value);
                        }
                        break;
                    case "CostAmount":
                        if (value) {
                            rtn = '￥' + filterValue(value);
                        }
                        break;
                }
                return rtn;
            }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            销售单</h1>
    </div>
</asp:Content>
