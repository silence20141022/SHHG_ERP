<%@ Page Title="销售发票" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="SaleOrderInvoiceList.aspx.cs" Inherits="Aim.Examining.Web.SaleOrderInvoiceList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=1100,height=600,scrollbars=yes");
        var ViewPageUrl = "SaleOrderInvoiceEdit.aspx";
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var CC = $.getQueryString({ ID: "CC" });
        var Index = $.getQueryString({ ID: "Index" });
        var IsQuery = $.getQueryString({ ID: "IsQuery" });
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
			    { name: 'Id' }, { name: 'Number' }, { name: 'CId' }, { name: 'CCode' }, { name: 'CName' }, { name: 'Amount' },
			    { name: 'Invalid' }, { name: 'Child' }, { name: 'Remark' }, { name: 'InvoiceDate' }, { name: 'CreateId' },
			    { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'PayAmount' }, { name: 'PayState' }, { name: 'MagUser' }
			    ],
                listeners: { aimbeforeload: function (proxy, options) {
                    options.data.CC = CC;
                    options.data.Index = Index;
                }
                }
            });
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
                { fieldLabel: '发票号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '负责人', id: 'MagUser', schopts: { qryopts: "{ mode: 'Like', field: 'MagUser' }"} },
                { fieldLabel: '开票时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });
            // 工具栏
            tlBar = new Ext.Toolbar({
                items: [
            { text: '添加销售收款', iconCls: 'aim-icon-add', hidden: Index == "2" || !Index || IsQuery == "T",
                handler: function () {
                    var recs = grid.getSelectionModel().getSelections();
                    var correspondAmount = 0;
                    var invoiceNo = "";
                    if (recs.length > 0) {
                        $.each(recs, function () {
                            correspondAmount += parseFloat(this.get("Amount")) - parseFloat(this.get("PayAmount") ? this.get("PayAmount") : 0);
                            invoiceNo += this.get("Number") + ",";
                        })
                    }
                    var cid = (recs.length > 0 ? recs[0].get("CId") : "");
                    var CName = (recs.length > 0 ? escape(recs[0].get("CName")) : "");
                    var url = "CustomerPayEdit.aspx?op=c&CId=" + cid + "&CName=" + CName + "&CorrespondAmount=" + correspondAmount + "&InvoiceNo=" + invoiceNo;
                    opencenterwin(url, "", 900, 400);
                }
            }, {
                text: '删除',
                iconCls: 'aim-icon-delete',
                hidden: Index == "2" || !Index || IsQuery == "T",
                handler: function () {
                    var recs = grid.getSelectionModel().getSelections();
                    if (!recs || recs.length <= 0) {
                        AimDlg.show("请先选择要删除的记录！");
                        return;
                    }
                    var allow = true;
                    $.each(recs, function () {
                        if (this.get("PayAmount")) {
                            allow = false;
                            return false;
                        }
                    })
                    if (!allow) {
                        AimDlg.show("不能删除有付款记录的发票,如果确定要删除，请先删除付款记录！");
                        return;
                    }
                    if (confirm("确定删除所选发票？")) {
                        var ids = "";
                        for (var i = 0; i < recs.length; i++) {
                            ids += recs[i].get("Id") + ",";
                        }
                        jQuery.ajaxExec('delete', { ids: ids }, onExecuted);
                    }
                }
            }, { text: '发票状态还原', iconCls: 'aim-icon-undo', hidden: !Index || IsQuery == "T",
                handler: function () {
                    var recs = grid.getSelectionModel().getSelections();
                    if (!recs || recs.length <= 0) {
                        AimDlg.show("请先选择要还原的记录！");
                        return;
                    }
                    var ids = "";
                    for (var i = 0; i < recs.length; i++) {
                        ids += (ids ? "," : "") + recs[i].get("Id");
                    }
                    if (confirm("确定还原所选发票？")) {
                        jQuery.ajaxExec('RollBack', { ids: ids }, function () {
                            alert("发票还原成功！");
                            store.reload();
                        });
                    }
                }
            },
                //             {
                //                 text: '更新开票明细',
                //                 iconCls: 'aim-icon-undo',
                //                 handler: function () {
                //                     $.ajaxExec("updateorderinvoicedetail", {}, function () {
                //                         store.reload();
                //                     });
                //                 }
                //             },
            {
            text: '标记已全部付款',
            iconCls: 'aim-icon-undo',
            handler: function () {
                var recs = grid.getSelectionModel().getSelections();
                if (!recs || recs.length <= 0) {
                    AimDlg.show("请先选择要操作的记录！");
                    return;
                }
                opencenterwin("ManualPayoffInvoice.aspx?invoiceid=" + recs[0].get("Id"), "", 900, 500);
            }
        }, '->']
            });
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'center',
                autoExpandColumn: 'CName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '发票号', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 180, sortable: true },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 200, sortable: true },
					{ id: 'MagUser', dataIndex: 'MagUser', header: '销售负责人', width: 80, sortable: true },
					{ id: 'Amount', dataIndex: 'Amount', header: '发票金额', width: 100, sortable: true, renderer: RowRender, summaryType: 'sum',
					    summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
                    { id: 'PayAmount', dataIndex: 'PayAmount', header: '已付款金额', width: 100, sortable: true, renderer: RowRender, summaryType: 'sum',
                        summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
                    },
                    { dataIndex: 'BadDebt', header: '坏账金额', width: 100 },
                    { id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 80, sortable: true },
					{ id: 'InvoiceDate', dataIndex: 'InvoiceDate', header: '开票日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '录入日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 150 }
                    ],
                bbar: pgBar,
                tbar: titPanel,
                plugins: [new Ext.ux.grid.GridSummary()]
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function opencenterwin(url, name, iWidth, iHeight) {
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
            window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
        }
        function onExecuted() {
            store.reload();
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
                case "Title":
                    rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='showwin(\"" +
                                                     record.get('Id') + "\")'>" + value + "</label>";
                    break;
                case "Amount":
                case "PayAmount":
                    if (value) {
                        rtn = AmountFormat(value);
                    }
                    break;
            }
            return rtn;
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
