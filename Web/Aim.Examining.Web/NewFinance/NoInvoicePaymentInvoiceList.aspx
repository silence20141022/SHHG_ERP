<%@ Page Title="收款记录" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="NoInvoicePaymentInvoiceList.aspx.cs" Inherits="Aim.Examining.Web.NoInvoicePaymentInvoiceList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var CC = $.getQueryString({ ID: "CC" });
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
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
			    { name: 'Id' }, { name: 'CorrespondState' }, { name: 'CorrespondInvoice' }, { name: 'Name' }, { name: 'BillType' },
			    { name: 'Money' }, { name: 'Remark' }, { name: 'CreateTime' }, { name: 'CId' }, { name: 'CName' }, { name: 'PayType' },
			    { name: 'ReceivablesTime' },
			    ],
                listeners: { aimbeforeload: function(proxy, options) {
                    //  options.data.CC = CC;
                }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 5,
                collapsed: false,
                items: [
                { fieldLabel: '收款方式', id: 'PayType', schopts: { qryopts: "{ mode: 'Like', field: 'PayType' }"} },
                { fieldLabel: '收据号码', id: 'Remark', schopts: { qryopts: "{ mode: 'Like', field: 'Remark' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '收款时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                { text: '撤销对应',
                    iconCls: 'aim-icon-undo',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }
                        var allow = true;
                        $.each(recs, function() {
                            if (this.get("CorrespondState") != "已对应") {
                                allow = false;
                                return false;
                            }
                        })
                        if (!allow) {
                            AimDlg.show("选择的记录中包含未对应的收款单！");
                            return;
                        }
                        var ids = "";
                        $.each(recs, function() {
                            ids = ids + this.get("Id") + ",";
                        });
                        $.ajaxExec("CancelCorrespond", { ids: ids }, function() { store.reload(); })
                    }
                }, '-', { text: '自动销账', iconCls: 'aim-icon-connect',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }
                        if (recs[0].get("CorrespondState") == "已对应") {
                            AimDlg.show("已对应的收款单不能再次销账！");
                            return;
                        }
                        $.ajaxExec("AutoCorrespond", { id: recs[0].get("Id") }, function(rtn) {
                            if (rtn.data.Result == "T") {
                                AimDlg.show("销账成功！");
                                store.reload();
                            }
                            else {
                                AimDlg.show("销账失败！该客户无欠款或者欠款金额小于本笔付款，请核对该客户欠款总金额！");
                            }
                        })
                    }
                }, '-',
                { text: '删除还款', iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }
                        var allow = true;
                        $.each(recs, function() {
                            if (this.get("CorrespondState") == "已对应") {
                                allow = false;
                                return false;
                            }
                        })
                        if (!allow) {
                            AimDlg.show("删除的记录中包含已对应的收款单！");
                            return;
                        }
                        var ids = "";
                        $.each(recs, function() {
                            ids = ids + this.get("Id") + ",";
                        });
                        $.ajaxExec("delete", { ids: ids }, function(rtn) {
                            store.reload();
                        })
                    }
                }, '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }

                }, '->'
                ]
            });
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'center',
                autoExpandColumn: 'CorrespondInvoice',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'ReceivablesTime', dataIndex: 'ReceivablesTime', header: '收款日期' },
					{ id: 'Remark', dataIndex: 'Remark', header: '收据号码', width: 100 },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 250, sortable: true },
					{ id: 'Money', dataIndex: 'Money', header: '收款金额', width: 100, sortable: true, summaryType: 'sum', renderer: RowRender,
					    summaryRenderer: function(v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'CorrespondState', dataIndex: 'CorrespondState', header: '对应状态', sortable: true, width: 80 },
					{ id: 'CorrespondInvoice', dataIndex: 'CorrespondInvoice', header: '对应销售单及金额', sortable: true, width: 160, renderer: RowRender },
					{ id: 'PayType', dataIndex: 'PayType', header: '收款方式', width: 80, sortable: true },
                    { id: 'Name', dataIndex: 'Name', header: '销账类型', width: 80, sortable: true }
                    ],
                bbar: pgBar,
                tbar: titPanel,
                plugins: [new Ext.ux.grid.GridSummary()]
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function AmountFormat(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn = "";
            switch (this.id) {
                case "Number":
                    rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='showwin(\"" +
                                                     record.get('Id') + "\")'>" + value + "</label>";
                    break;
                case "Money":
                    rtn = AmountFormat(value);
                    break;
                case "CorrespondInvoice":
                    cellmeta.attr = 'ext:qtitle="" ext:qtip="' + value + '"';
                    rtn = value;
                    break
            }
            return rtn;
        }
        function opencenterwin(url, name, iWidth, iHeight) {
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
            window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
        }
        function showwin(val) {
            var task = new Ext.util.DelayedTask();
            task.delay(50, function() {
                opencenterwin("/SaleManagement/FrmOrdersEdit.aspx?op=v&id=" + val, "", 1000, 500);
            });
        }
        function onExecuted() {
            store.reload();
        }     
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
