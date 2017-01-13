<%@ Page Title="收据销售单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="NoInvoiceSaleOrderList.aspx.cs" Inherits="Aim.Examining.Web.NoInvoiceSaleOrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        var Index = $.getQueryString({ ID: "Index" });
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
			    { name: 'Id' }, { name: 'PId' }, { name: 'Number' }, { name: 'CId' }, { name: 'CCode' }, { name: 'CName' },
			    { name: 'ExpectedTime' }, { name: 'WarehouseId' }, { name: 'WarehouseName' }, { name: 'CalculateManner' },
			    { name: 'InvoiceType' }, { name: 'PayType' }, { name: 'TotalMoney' }, { name: 'PreDeposit' }, { name: 'ApprovalState' },
			    { name: 'TotalMoneyHis' },
			    { name: 'InvoiceNumber' },
			    { name: 'CorrespondState' },
			    { name: 'CorrespondInvoice' },
			    { name: 'CorrespondAmount' },
			    { name: 'EndDate' },
			    { name: 'DeliveryMode' },
			    { name: 'InvoiceState' },
			    { name: 'PANumber' },
			    { name: 'PAState' },
			    { name: 'Reason' }, { name: 'State' },
			    { name: 'Salesman' },
			    { name: 'SalesmanId' },
			    { name: 'DeState' },
			    { name: 'DeliveryState' },
			    { name: 'CreateId' }, { name: 'CreateName' },
			    { name: 'CreateTime' }, { name: 'DiscountAmount' }, { name: 'ReturnAmount' }, { name: 'ReceiptAmount' }, { name: 'Remark' }, { name: 'PayState' }
			    ],
                listeners: { "aimbeforeload": function (proxy, options) {
                    options.data.Index = Index;
                }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 4,
                collapsed: false,
                items: [
                { fieldLabel: '销售单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '下单时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                {
                    text: '添加销售收款',
                    iconCls: 'aim-icon-invoice',
                    handler: function () {
                        var saleorderno = "";
                        var saleamount = 0;
                        var cid = "";
                        var cname = "";
                        var recs = grid.getSelectionModel().getSelections();
                        if (recs.length > 0) {
                            cid = recs[0].get("CId");
                            cname = recs[0].get("CName");
                            var allow = true;
                            $.each(recs, function () {
                                if (this.get("CId") != cid) {
                                    allow = false;
                                    return false;
                                }
                            })
                            if (!allow) {
                                AimDlg.show("选择的记录不是同一个客户的销售单！");
                                return;
                            }
                            $.each(recs, function () {
                                saleorderno += this.get("Number") + ",";
                                saleamount += parseFloat(this.get("TotalMoney")) - parseFloat(this.get("ReturnAmount") ? this.get("ReturnAmount") : 0) - parseFloat(this.get("ReceiptAmount") ? this.get("ReceiptAmount") : 0);
                            });
                        }
                        opencenterwin("NoInvoiceCustomerPayEdit.aspx?&op=c&CustomerId=" + cid + "&CustomerName=" + escape(cname) +
                          "&SaleAmount=" + saleamount + "&SaleOrderNo=" + saleorderno, "", 1000, 600);
                    }
                }, '-', {
                    text: '添加其他收款',
                    iconCls: 'aim-icon-invoice',
                    handler: function () {
                        opencenterwin("CustomerOtherPayEdit.aspx?op=c", "", 900, 400);
                    }
                }, '-',
                 {
                     text: '标记已作废',
                     iconCls: 'aim-icon-stop',
                     handler: function () {
                         var recs = grid.getSelectionModel().getSelections();
                         if (!recs || recs.length <= 0) {
                             AimDlg.show("请先选择要操作的记录！");
                             return;
                         }
                         var orderids = "";
                         $.each(recs, function () {
                             orderids = orderids + this.get("Id") + ",";
                         });
                         $.ajaxExec("CancelSaleOrder", { ids: orderids }, function () {
                             store.reload();
                         })
                     }
                 }, '-', {
                     text: '导出Excel',
                     iconCls: 'aim-icon-xls',
                     handler: function () {
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
                autoExpandColumn: 'CName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '销售单号', renderer: RowRender,
					    width: 130, sortable: true, summaryRenderer: function (v, params, data) { return "汇总:" }
					},
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 250, sortable: true },
					{ id: 'TotalMoney', dataIndex: 'TotalMoney', header: '总金额', width: 150, sortable: true, summaryType: 'sum', renderer: RowRender,
					    summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'DiscountAmount', dataIndex: 'DiscountAmount', header: '折扣金额', width: 100, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'ReturnAmount', dataIndex: 'ReturnAmount', header: '退货金额', width: 100, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'ReceiptAmount', dataIndex: 'ReceiptAmount', header: '收款金额', width: 100, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 80, renderer: ExtGridDateOnlyRender, sortable: true },
                    { id: 'CreateName', dataIndex: 'CreateName', header: '开单员', width: 80, sortable: true },
                    { id: 'Remark', dataIndex: 'Remark', header: '备注', width: 130, renderer: RowRender }
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
                case "TotalMoney":
                case "DiscountAmount":
                case "ReturnAmount":
                case "ReceiptAmount":
                    if (value) {
                        rtn = AmountFormat(value);
                    }
                    break;
                case "Remark":
                    if (value) {
                        cellmeta.attr = 'ext:qtitle="" ext:qtip="' + value + '"';
                        rtn = value;
                    }
                    break;
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
            task.delay(50, function () {
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
