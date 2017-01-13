<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    Title="对账单" CodeBehind="CorrespondBillParent.aspx.cs" Inherits="Aim.Examining.Web.CorrespondBillParent" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var CC = $.getQueryString({ ID: "CC" });
        function onPgLoad() {
            setPgUI();
            schBar.doLayout();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                data: myData,
                fields: [
			   { name: 'Id' }, { name: 'Name' }, { name: 'MagUser' }, { name: 'SaleAmount' }, { name: 'InvoiceAmount' }, { name: 'PrepayAmount' }
			    ],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || [];
                    options.data.CC = CC;
                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 2,
                items: [
                { fieldLabel: '客户名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '负责人', id: 'MagUser', schopts: { qryopts: "{ mode: 'Like', field: 'MagUser' }"} }
                //                {fieldLabel: '开始时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                //                { fieldLabel: '结束时间', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
               ]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
               {
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
            var columnarray = [
                    { id: 'Id', dataIndex: 'Id', header: 'Id', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Name', dataIndex: 'Name', header: '客户名称', width: 220, sortable: true },
					{ id: 'MagUser', dataIndex: 'MagUser', header: '负责人', width: 60, sortable: true },
                    { id: 'SaleAmount', dataIndex: 'SaleAmount', header: '待开票', width: 110, sortable: true, renderer: RowRender,
                        summaryType: 'sum', summaryRenderer: function(v) { return AmountFormat(Math.round(v * 100) / 100); }
                    },
                   { id: 'InvoiceAmount', dataIndex: 'InvoiceAmount', header: '应收款', width: 110, sortable: true, renderer: RowRender,
                       summaryType: 'sum', summaryRenderer: function(v) { return AmountFormat(Math.round(v * 100) / 100); }
                   },
                    { id: 'PrepayAmount', dataIndex: 'PrepayAmount', header: '预付款', width: 110, sortable: true, renderer: RowRender,
                        summaryType: 'sum', summaryRenderer: function(v) { return AmountFormat(Math.round(v * 100) / 100); }
                    }
					 ];
            grid = new Ext.ux.grid.AimGridPanel({
                title: '客户对账信息',
                store: store,
                region: 'west',
                autoExpandColumn: 'Name',
                width: 600,
                columns: columnarray,
                bbar: pgBar,
                tbar: titPanel,
                plugins: new Ext.ux.grid.GridSummary(),
                listeners: { rowclick: function(tgrid, rowIndex, e) {
                    var rec = store.getAt(rowIndex);
                    frameContent.location.href = "CorrespondBillList.aspx?CId=" + rec.get("Id") + "&CName=" + escape(rec.get("Name"));
                }
                }
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid, {
                    id: 'frmcon',
                    region: 'center',
                    margins: '-1 0 -2 0',
                    html: '<iframe width="100%" height="100%" id="frameContent"  name="frameContent" frameborder="0"></iframe>'}]
                });
                if (store.data.length > 0) {
                    frameContent.location.href = "CorrespondBillList.aspx?CId=" + store.getAt(0).get("Id") + "&CName=" + escape(store.getAt(0).get("Name"));
                }
            }
            function onExecuted() {
                store.reload();
            }
            function opencenterwin(url, name, iWidth, iHeight) {
                var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
                var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
                window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
            }
            function OpenModule(val) {

            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn = "";
                switch (this.id) {
                    case "CorrespondBillNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../FinanceManagement/OtherPayBillView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "SaleAmount":
                    case "InvoiceAmount":
                    case "PrepayAmount":
                        rtn = AmountFormat(value);
                        break;
                }
                return rtn;
            }
            function AmountFormat(value) {
                val = String(value);
                var whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return "￥" + whole;
            }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
