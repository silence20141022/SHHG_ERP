<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    Title="对账单" CodeBehind="NoInvoiceCorrespondBillList.aspx.cs" Inherits="Aim.Examining.Web.NoInvoiceCorrespondBillList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var CC = $.getQueryString({ ID: "CC" });
        function onPgLoad() {
            setPgUI();
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
			    { name: 'Id' }, { name: 'CId' }, { name: 'CName' }, { name: 'CustomerId' }, { name: 'BorrowMoney' }, { name: 'TotalArrearage' },
			    { name: 'PayMoney' }, { name: 'CreateTime' }, { name: 'MoneyType' }, { name: 'Remark' }, { name: 'CreateName' }
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
                columns: 4,
                items: [
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                // { fieldLabel: '发票编号', id: 'InvoiceNo', schopts: { qryopts: "{ mode: 'Like', field: 'InvoiceNo' }"} },
                {fieldLabel: '开始时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '结束时间', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
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
                    { id: 'CId', dataIndex: 'CId', header: 'CId', hidden: true },
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'CreateTime', dataIndex: 'CreateTime', header: '借/付款日期', width: 100, sortable: true, renderer: ExtGridDateOnlyRender },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 220, sortable: true },
					{ id: 'MoneyType', dataIndex: 'MoneyType', header: '对账类型', width: 80, sortable: true },
				    { id: 'BorrowMoney', dataIndex: 'BorrowMoney', header: '借款金额', width: 100, sortable: true, renderer: RowRender,
				        summaryType: 'sum', summaryRenderer: function(v) { return AmountFormat(v); }
				    },
				    { id: 'PayMoney', dataIndex: 'PayMoney', header: '还款金额', width: 100, sortable: true, renderer: RowRender,
				        summaryType: 'sum', summaryRenderer: function(v) { return AmountFormat(v); }
				    },
            //    { id: 'TotalArrearage', dataIndex: 'TotalArrearage', header: '合计欠款', width: 100, sortable: true, renderer: RowRender },
				    {id: 'CreateName', dataIndex: 'CreateName', header: '操作人', width: 80 },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 180 }
					 ];
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'center',
                autoExpandColumn: 'CName',
                columns: columnarray,
                bbar: pgBar,
                tbar: titPanel,
                plugins: new Ext.ux.grid.GridSummary()
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
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
                case "BorrowMoney":
                case "PayMoney":
                    if (value) {
                        rtn = AmountFormat(value);
                    }
                    break;
                case "TotalArrearage":
                    var v1 = (record.get("BorrowMoney") ? record.get("BorrowMoney") : 0) - (record.get("PayMoney") ? record.get("PayMoney") : 0);
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
