<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SaleReportLeft.aspx.cs" Inherits="Aim.Examining.Web.SaleReportLeft" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, fieldsarray, searcharray, columnarray;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var typeEnum = { '压缩机': '压缩机', '配件': '配件' };
        var beginDate = "";
        var endDate = "";
        var Index = $.getQueryString({ "ID": "Index" });
        function onPgLoad() {
            setPgUI();
            if (store.data.length > 0) {
                if (Ext.getCmp("BeginDate").getValue()) {
                    beginDate = Ext.getCmp("BeginDate").getValue().toLocaleDateString();
                }
                if (Ext.getCmp("EndDate").getValue()) {
                    endDate = Ext.getCmp("EndDate").getValue().toLocaleDateString();
                }
                if (Index == 0) {

                    frameContent.location.href = "SaleReportByProduct.aspx?ProductId=" + store.getAt(0).get("PId") + "&BeginDate=" + beginDate + "&EndDate=" + endDate;
                }
                else {
                    frameContent.location.href = "SaleReportByCustomer.aspx?CustomerId=" + store.getAt(0).get("Id") + "&BeginDate=" + beginDate + "&EndDate=" + endDate;
                }
            }
            viewport.doLayout();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DetailList"] || []
            };
            if (Index == 0) {
                fieldsarray = [{ name: 'Id' }, { name: 'Code' }, { name: 'Name' }, { name: 'Pcn' },
                { name: 'SaleCount' }, { name: 'SaleAmount' }, { name: 'StockQuantity'}];
            }
            else {
                fieldsarray = [{ name: 'Id' }, { name: 'Name' }, { name: 'SaleAmount' }, { name: 'MagUser' }, { name: 'LastExchangeDate'}];
            }
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                idProperty: 'Id',
                data: myData,
                fields: fieldsarray,
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || {};
                    options.data.Index = Index;
                },
                    load: function(store, records, options) {
                        if (records.length > 0 && document.getElementById("frameContent")) {
                            beginDate = ""; endDate = "";
                            if (Ext.getCmp("BeginDate").getValue()) {
                                beginDate = Ext.getCmp("BeginDate").getValue().toLocaleDateString();
                            }
                            if (Ext.getCmp("EndDate").getValue()) {
                                endDate = Ext.getCmp("EndDate").getValue().toLocaleDateString();
                            }
                            if (Index == 0) {
                                frameContent.location.href = "SaleReportByProduct.aspx?ProductId=" + records[0].get("PId") + "&BeginDate=" + beginDate + "&EndDate=" + endDate;
                            }
                            else {
                                frameContent.location.href = "SaleReportByCustomer.aspx?CustomerId=" + records[0].get("Id") + "&BeginDate=" + beginDate + "&EndDate=" + endDate;
                            }
                        }
                    }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            // 搜索栏
            if (Index == 0) {
                searcharray = [
                { fieldLabel: '产品类型', xtype: 'aimcombo', style: { marginTop: '-1px' }, id: 'ProductType', enumdata: typeEnum, schopts: { qryopts: "{ mode: 'Equal', field: 'ProductType' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '交易日期', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}];
            }
            else {
                searcharray = [
                { fieldLabel: '客户名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '交易日期', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}];

            }
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 3,
                items: searcharray
            });
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
}]
                });
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });

                // 表格面板
                if (Index == 0) {
                    columnarray = [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Name', dataIndex: 'Name', header: '产品名称', width: 100, sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '产品型号', width: 120, sortable: true },
                    { id: 'SaleCount', dataIndex: 'SaleCount', header: '销售数量', width: 60, sortable: true },
				    { id: 'StockQuantity', dataIndex: 'StockQuantity', header: '库存', width: 50, sortable: true}];
                }
                else {
                    columnarray = [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Name', dataIndex: 'Name', header: '客户名称', width: 230, sortable: true },
                    { id: 'MagUser', dataIndex: 'MagUser', header: '负责人', width: 60, sortable: true },
                    { id: 'LastExchangeDate', dataIndex: 'LastExchangeDate', header: '最近交易日期', width: 100, sortable: true },
				    { id: 'SaleAmount', dataIndex: 'SaleAmount', header: '销售金额', width: 100, sortable: true, renderer: RowRender }
				 ];
                }
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'west',
                    margins: '-2 0 0 0',
                    width: 630,
                    columns: columnarray,
                    autoExpandColumn: Index == 0 ? "Code" : "Name",
                    bbar: pgBar,
                    tbar: titPanel,
                    listeners: { rowclick: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (recs || recs.length > 0) {
                            beginDate = ""; endDate = "";
                            if (Ext.getCmp("BeginDate").getValue()) {
                                beginDate = Ext.getCmp("BeginDate").getValue().toLocaleDateString();
                            }
                            if (Ext.getCmp("EndDate").getValue()) {
                                endDate = Ext.getCmp("EndDate").getValue().toLocaleDateString();
                            }
                            if (Index == 0) {
                                frameContent.location.href = "SaleReportByProduct.aspx?ProductId=" + recs[0].get("Id") + "&BeginDate=" + beginDate + "&EndDate" + endDate;
                            }
                            else {
                                frameContent.location.href = "SaleReportByCustomer.aspx?CustomerId=" + recs[0].get("Id") + "&BeginDate=" + beginDate + "&EndDate" + endDate;
                            }
                        }
                    }
                    }
                });
                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [grid,
                    { region: 'center',
                        cls: 'empty',
                        bodyStyle: 'background:#f1f1f1',
                        html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src="SaleDetailByProductId.aspx"></iframe>'}]
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
                                      record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                            }
                            break;
                        case "SaleAmount":
                            if (value) {
                                val = String(value);
                                var whole = val;
                                var r = /(\d+)(\d{3})/;
                                while (r.test(whole)) {
                                    whole = whole.replace(r, '$1' + ',' + '$2');
                                }
                                rtn = '￥' + whole;
                            }
                            else {
                                rtn = '';
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
            标题</h1>
    </div>
</asp:Content>
