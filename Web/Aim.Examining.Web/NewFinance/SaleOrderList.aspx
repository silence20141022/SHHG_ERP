<%@ Page Title="财务销售单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="SaleOrderList.aspx.cs" Inherits="Aim.Examining.Web.SaleOrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        var CC = $.getQueryString({ ID: "CC" }); //统一的前台接收页面参数
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
			    { name: 'InvoiceType' }, { name: 'PayType' }, { name: 'TotalMoney' }, { name: 'PreDeposit' },
			    { name: 'ApprovalState' }, { name: 'TotalMoneyHis' },
			    { name: 'InvoiceNumber' }, { name: 'CorrespondState' },
			    { name: 'CorrespondInvoice' },
			    { name: 'CorrespondAmount' },
			    { name: 'EndDate' },
			    { name: 'DeliveryMode' },
			    { name: 'InvoiceState' },
			    { name: 'PANumber' },
			    { name: 'PAState' },
			    { name: 'Child' },
			    { name: 'Reason' }, { name: 'State' },
			    { name: 'Salesman' },
			    { name: 'SalesmanId' },
			    { name: 'DeState' },
			    { name: 'DeliveryState' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
			    { name: 'CreateTime' }, { name: 'DiscountAmount' }, { name: 'ReturnAmount' }, { name: 'InvoiceAmount' }, { name: 'Remark' }
			    ],
                listeners: { "aimbeforeload": function (proxy, options) {
                    options.data.CC = CC;
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
                { fieldLabel: '销售单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '负责人', id: 'Salesman', schopts: { qryopts: "{ mode: 'Like', field: 'Salesman' }"} },
                { fieldLabel: '下单时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                {
                    text: '开发票',
                    iconCls: 'aim-icon-invoice',
                    handler: function () {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要开具发票的销售订单！");
                            return;
                        }
                        var cid = recs[0].get("CId");
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
                        var orderids = "";
                        $.each(recs, function () {
                            orderids = orderids + this.get("Id") + ",";
                        });
                        opencenterwin("SaleOrderInvoiceEdit.aspx?orderids=" + orderids + "&op=c&CustomerId=" + recs[0].get("CId") + "&CustomerName=" + escape(recs[0].get("CName")), "", 1100, 600);
                    }
                }, '-',
                  {
                      text: '导出Excel',
                      iconCls: 'aim-icon-xls',
                      handler: function () {
                          ExtGridExportExcel(grid, { store: null, title: '标题' });
                      }

                  }, '-', {
                      text: '重置开票明细',
                      iconCls: 'aim-icon-undo',
                      handler: function () {
                          var recs = grid.getSelectionModel().getSelections();
                          if (!recs || recs.length <= 0) {
                              AimDlg.show("请先选择要重置的销售订单！");
                              return;
                          }
                          var orderids = "";
                          $.each(recs, function () {
                              orderids = orderids + this.get("Id") + ",";
                          });
                          $.ajaxExec("ResetSaleOrderDetail", { ids: orderids }, function (rtn) {
                              store.reload();
                          })
                      }
                  }, '-', { text: '设置为收据模式', iconCls: 'aim-icon-swf',
                      handler: function () {
                          var recs = grid.getSelectionModel().getSelections();
                          if (!recs || recs.length <= 0) {
                              AimDlg.show("请先选择要设置为收据的销售订单！");
                              return;
                          }
                          var orderids = "";
                          $.each(recs, function () {
                              orderids = orderids + this.get("Id") + ",";
                          });
                          $.ajaxExec("SetReceiptModel", { ids: orderids }, function (rtn) {
                              store.reload();
                          })
                      }
                  }, '-', { text: '暂缓开票', iconCls: 'aim-icon-pause',
                      handler: function () {
                          var recs = grid.getSelectionModel().getSelections();
                          if (!recs || recs.length <= 0) {
                              AimDlg.show("请先选择要暂缓开票的销售订单！");
                              return;
                          }
                          var orderids = "";
                          $.each(recs, function () {
                              orderids = orderids + this.get("Id") + ",";
                          });
                          $.ajaxExec("DelayInvoice", { ids: orderids }, function (rtn) {
                              store.reload();
                          }) 
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
					{ id: 'TotalMoney', dataIndex: 'TotalMoney', header: '总金额', width: 110, sortable: true, summaryType: 'sum', renderer: RowRender,
					    summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'DiscountAmount', dataIndex: 'DiscountAmount', header: '折扣金额', width: 90, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'ReturnAmount', dataIndex: 'ReturnAmount', header: '退货金额', width: 90, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'InvoiceAmount', dataIndex: 'InvoiceAmount', header: '开票金额', width: 90, sortable: true, summaryType: 'sum',
					    renderer: RowRender, summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); }
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 80, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'InvoiceState', dataIndex: 'InvoiceState', header: '开票状态', width: 80, sortable: true },
                    { id: 'Salesman', dataIndex: 'Salesman', header: '销售负责人', width: 80, sortable: true },
                    { id: 'Remark', dataIndex: 'Remark', header: '备注', width: 130, renderer: RowRender }
                    ],
                bbar: pgBar,
                tbar: titPanel,
                plugins: [new Ext.ux.grid.GridSummary()]
            });
            viewport = new Ext.ux.AimViewport({
                layout: 'border',
                items: [grid]//body     
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
                case "InvoiceAmount":
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
