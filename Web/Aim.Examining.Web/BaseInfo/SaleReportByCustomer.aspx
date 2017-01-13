<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SaleReportByCustomer.aspx.cs" Inherits="Aim.Examining.Web.SaleReportByCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var customerId = $.getQueryString({ "ID": "CustomerId" });
        var beginDate = $.getQueryString({ "ID": "BeginDate" });
        var endDate = $.getQueryString({ "ID": "EndDate" });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DeliveryOrderPartList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DeliveryOrderPartList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'PCode' }, { name: 'PName' }, { name: 'Count' }, { name: 'CreateTime' },
			    { name: 'SalePrice' }, { name: 'SaleAmount' }
			    ],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data.CustomerId = customerId;
                }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: 120,
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 3,
                items: [
                //{ fieldLabel: '序列号', schopts: { qryopts: "{ mode: 'Like', field: 'SeriesNo' }"} },
                    {fieldLabel: '型号', schopts: { qryopts: "{ mode: 'Like', field: 'ModelNo' }"} },
                    { fieldLabel: '销售时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                    { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }

                ]
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
                grid = new Ext.ux.grid.AimEditorGridPanel({
                    store: store,
                    region: 'center',
                    border: false,
                    autoExpandColumn: 'PCode',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
					{ id: 'PCode', dataIndex: 'PCode', header: '产品型号', width: 120 },
					{ id: 'PName', dataIndex: 'PName', header: '产品名称', width: 80 },
					{ id: 'Count', dataIndex: 'Count', header: '数量', width: 45 },
					{ id: 'SalePrice', dataIndex: 'SalePrice', header: '销售价格', width: 70, renderer: RowRender },
					{ id: 'SaleAmount', dataIndex: 'SaleAmount', header: '销售金额', width: 70, renderer: RowRender },
                    { id: 'CreateTime', dataIndex: 'CreateTime', header: '销售时间', width: 80 }
					],
                    bbar: pgBar,
                    tbar: titPanel
                });
                viewport = new Ext.Viewport({
                    layout: 'border',
                    items: [grid]
                });
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
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
                    case "SalePrice":
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
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
