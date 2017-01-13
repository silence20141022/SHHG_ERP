<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="LogisticsSettlement.aspx.cs" Inherits="Aim.Examining.Web.LogisticsManagement.LogisticsSettlement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=1000,height=600,scrollbars=yes,resizable=yes");
        var ViewPageUrl = "FrmLogisticsEdit.aspx";
        var EditWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var EditPageUrl = "FrmLogisticsEdit.aspx";
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
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
			    { name: 'Id' }, { name: 'PayBillNo' }, { name: 'LogisticsCompanyName' }, { name: 'PayType' }, { name: 'ShouldPayAmount' },
                { name: 'AcctualPayAmount' }, { name: 'InterfaceArray' }, { name: 'ModifyUserId' }, { name: 'InvoiceNo' }, { name: 'PayState' },
                { name: 'InvoiceAmount' }, { name: 'PayUserId' }, { name: 'PayUserName' }, { name: 'PayTime' }, { name: 'ModifyTime' },
                { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime'}]
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
                { fieldLabel: '付款单号', id: 'PayBillNo', schopts: { qryopts: "{ mode: 'Like', field: 'PayBillNo' }"} },
                { fieldLabel: '物流公司', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '申请时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
                ]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        if (confirm("确定删除所选记录？")) {
                            var allow = true;
                            $.each(recs, function() {
                                if (this.get("PayState") == "已付款") {
                                    allow = false;
                                    return false;
                                }
                            });
                            if (!allow) {
                                AimDlg.show("已付款的付款单不允许删除！");
                                return;
                            }
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

                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Remark',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'PayBillNo', dataIndex: 'PayBillNo', header: '付款单号', width: 130, renderer: RowRender },
                  	{ id: 'LogisticsCompanyName', dataIndex: 'LogisticsCompanyName', header: '物流公司', width: 100 },
					{ id: 'InvoiceNo', dataIndex: 'InvoiceNo', header: '发票号', width: 100, summaryRenderer: function(v, params, data) { return "汇总:" } },
					{ id: 'InvoiceAmount', dataIndex: 'InvoiceAmount', header: '发票金额', width: 70, renderer: RowRender,
					    summaryType: 'sum',
					    summaryRenderer: function(v, params, data) { return filterValue(v) }
					},
					{ id: 'ShouldPayAmount', dataIndex: 'ShouldPayAmount', header: '申请金额', width: 70, summaryType: 'sum',
					    summaryRenderer: function(v, params, data) { return filterValue(v) }, renderer: RowRender
					},
					{ id: 'AcctualPayAmount', dataIndex: 'AcctualPayAmount', header: '已付金额', width: 70, summaryType: 'sum',
					    summaryRenderer: function(v, params, data) { return filterValue(v) }, renderer: RowRender
					},
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 80 },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '申请时间', width: 80, renderer: ExtGridDateOnlyRender },
				    { id: 'Remark', dataIndex: 'Remark', header: '申请事由', width: 100 }
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
            // 提交数据成功后
            function onExecuted() {
                store.reload();
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
                    case "PayBillNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../FinanceManagement/OtherPayBillView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + ViewWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "AcctualPayAmount":
                        if (value) {//因为有null的现象
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
                    case "ShouldPayAmount":
                        if (value) {//因为有null的现象
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
    <div id="header" style="display: none;">
    </div>
</asp:Content>
