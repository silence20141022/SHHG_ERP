<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="OtherPayBillPayList.aspx.cs" Inherits="Aim.Examining.Web.FinanceManagement.OtherPayBillPayList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=1000,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl = "LogisticsPayPay.aspx";
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var index = $.getQueryString({ "ID": 'Index' });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OtherPayBillList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OtherPayBillList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'PayBillNo' }, { name: 'PayState' }, { name: 'PayType' }, { name: 'LogisticsCompanyName' },
			    { name: 'ShouldPayAmount' }, { name: 'AcctualPayAmount' }, { name: 'InterfaceArray' }, { name: 'ModifyUserId' },
			    { name: 'InvoiceNo' }, { name: 'ModifyTime' },
			    { name: 'InvoiceAmount' }, { name: 'PayUserId' }, { name: 'PayUserName' }, { name: 'PayTime' },
			    { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }
			    ],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || [];
                    options.data.Index = index;
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
                { fieldLabel: '付款编号', id: 'PayBillNo', schopts: { qryopts: "{ mode: 'Like', field: 'PayBillNo' }"} },
                { fieldLabel: '申请人', id: 'CreateName', schopts: { qryopts: "{ mode: 'Like', field: 'CreateName' }"} },
                { fieldLabel: '申请时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
               ]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: ['->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("PayBillNo"));
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
                var columnarray = [{ id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'PayBillNo', dataIndex: 'PayBillNo', header: '付款编号', width: 130, sortable: true, renderer: RowRender },
					{ id: 'LogisticsCompanyName', dataIndex: 'LogisticsCompanyName', header: '物流公司', width: 100, sortable: true },
					{ id: 'PayType', dataIndex: 'PayType', header: '付款类型', width: 70, sortable: true },
				    { id: 'ShouldPayAmount', dataIndex: 'ShouldPayAmount', header: '申请金额', width: 70, sortable: true, renderer: RowRender },
					{ id: 'InvoiceAmount', dataIndex: 'InvoiceAmount', header: '发票金额', width: 70, sortable: true, renderer: RowRender },
					{ id: 'InvoiceNo', dataIndex: 'InvoiceNo', header: '发票号', width: 70, sortable: true },
					{ id: 'CreateName', dataIndex: 'CreateName', header: '申请人', width: 70, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '申请日期', width: 80, sortable: true, renderer: ExtGridDateOnlyRender },
					{ id: 'AcctualPayAmount', dataIndex: 'AcctualPayAmount', header: '已付金额', width: 70, renderer: RowRender },
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 80, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '申请事由', width: 80}];
                if (index == 0) {
                    columnarray.push({ id: 'Operation', dataIndex: 'Id', header: '付款操作', width: 80, renderer: RowRender });
                }
                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Remark',
                    columns: columnarray,
                    bbar: pgBar,
                    tbar: titPanel
                });
                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }
            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Operation":
                        if (value) {
                            rtn = "<img   src='../images/shared/key.png'/><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"OtherPayBillPay.aspx?id=" +
                       value + "&op=u" + "\",\"wind\",\"" + EditWinStyle + "\")'>付款操作</label>";
                        }
                        break;
                    case "PayBillNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../FinanceManagement/OtherPayBillView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "ShouldPayAmount":
                        if (value) {
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = "￥" + whole;
                        }
                        break;
                    case "InvoiceAmount":
                        if (value) {
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = "￥" + whole;
                        }
                        break;
                    case "AcctualPayAmount":
                        if (value) {
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = "￥" + whole;
                        }
                        break;
                }
                return rtn;
            }      
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
