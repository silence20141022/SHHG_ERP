<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="PayBillView.aspx.cs" Inherits="Aim.Examining.Web.FinanceManagement.PayBillView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, grid, PayData, paystore, paygrid;
        var id = $.getQueryString({ "ID": "id" });
        var WinStyle = CenterWin("width=950,height=600,scrollbars=yes,resizable=no");
        function onPgLoad() {
            setPgUI();
            $("#label").text("(" + $("#Symbo").val() + ")");
            var arg1 = $("#ActuallyPayAmount").val();
            var arg2 = $("#PAmount").val();
            var arg3 = $("#DiscountAmount").val();
            var result = accDiv(parseFloat(arg1) + parseFloat(arg3 ? arg3 : 0), parseFloat(arg2));
            $("#ExchangeRate").val(Math.round(result * 100) / 100);
        }
        function setPgUI() {
            // 表格数据
            myData = {
                records: AimState["DetailList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                fields: [{ name: 'Id' }, { name: 'PayBillId' }, { name: 'PurchaseOrderDetailId' }, { name: 'PurchaseOrderNo' }, { name: 'PurchaseOrderId' },
              { name: 'ProductCode' }, { name: 'ProductName' }, { name: 'BuyPrice' }, { name: 'PayQuantity' }, { name: 'Amount'}]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                { id: 'Id', dataIndex: 'Id', hidden: true },
                { id: 'PurchaseOrderId', dataIndex: 'PurchaseOrderId', hidden: true },
                { id: 'PurchaseOrderDetailId', dataIndex: 'PurchaseOrderDetailId', hidden: true },
                { id: 'PayBillId', dataIndex: 'PayBillId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'PurchaseOrderNo', header: '采购单号', dataIndex: 'PurchaseOrderNo', width: 130, renderer: RowRender },
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 130 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode', width: 200 },
                { id: 'BuyPrice', header: '采购价格', dataIndex: 'BuyPrice', width: 100, renderer: RowRender,
                    summaryRenderer: function(v, params, data) { return "汇总:" }
                },
                { id: 'PayQuantity', header: '数量', dataIndex: 'PayQuantity', width: 100, summaryType: 'sum' },
                { id: 'Amount', header: '金额', dataIndex: 'Amount', width: 100, summaryType: 'sum', renderer: RowRender,
                    summaryRenderer: function(v, params, data) { return $("#Symbo").val() + filterValue(v); } }]
                });

                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    cm: cm,
                    renderTo: "StandardSub",
                    columnLines: true,
                    width: Ext.get("StandardSub").getWidth(),
                    autoHeight: true,
                    plugins: new Ext.ux.grid.GridSummary(),
                    forceLayout: true,
                    tbar: new Ext.Toolbar({
                        items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">付款明细：</label>']
                    }),
                    autoExpandColumn: 'ProductCode'
                });
                var PayData = {
                    records: AimState["PayDetailList"] || []
                };
                var paystore = new Ext.ux.data.AimJsonStore({
                    dsname: 'PayDetailList',
                    data: PayData,
                    fields: [{ name: 'Id' }, { name: 'PayBillId' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'PayBillNo' },
              { name: 'CreateTime' }, { name: 'Remark' }, { name: 'ActualPayAmount'}]
                });
                var cm2 = new Ext.grid.ColumnModel({
                    defaults: {
                        resizable: true
                    },
                    columns: [
                { id: 'Id', dataIndex: 'Id', hidden: true },
                { id: 'PayBillId', dataIndex: 'PayBillId', hidden: true },
                { id: 'CreateId', dataIndex: 'CreateId', hidden: true },
                 new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'PayBillNo', header: '付款单号', dataIndex: 'PayBillNo', width: 130, summaryRenderer: function(v, params, data) { return "汇总:" } },
                 { id: 'Remark', header: '备注', dataIndex: 'Remark' },
                 { id: 'ActualPayAmount', header: '付款金额', dataIndex: 'ActualPayAmount', width: 100, summaryType: 'sum',
                     summaryRenderer: function(v, params, data) { return '￥' + filterValue(v); }, renderer: RowRender
                 },
                { id: 'CreateTime', header: '付款日期', dataIndex: 'CreateTime', width: 100, renderer: ExtGridDateOnlyRender },
                { id: 'CreateName', header: '付款人', dataIndex: 'CreateName', width: 100, renderer: RowRender }

                ]
                });
                var paygrid = new Ext.ux.grid.AimGridPanel({
                    store: paystore,
                    cm: cm2,
                    renderTo: "sub2",
                    columnLines: true,
                    width: Ext.get("sub2").getWidth(),
                    autoHeight: true,
                    plugins: new Ext.ux.grid.GridSummary(),
                    forceLayout: true,
                    tbar: new Ext.Toolbar({
                        items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">已付款信息：</label>']
                    }),
                    autoExpandColumn: 'Remark'
                });
                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                    paygrid.setWidth(0);
                    paygrid.setWidth(Ext.get("sub2").getWidth());
                };
            }
            function filterValue(val) {
                val = String(val);
                var whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return (whole == "null" || whole == null ? "" : whole);
            }
            function FloatAdd(arg1, arg2) {
                var r1, r2, m;
                try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
                try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
                m = Math.pow(10, Math.max(r1, r2))
                return (arg1 * m + arg2 * m) / m
            }
            function accDiv(arg1, arg2) {
                var t1 = 0, t2 = 0, r1, r2;
                try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
                try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
                with (Math) {
                    r1 = Number(arg1.toString().replace(".", ""))
                    r2 = Number(arg2.toString().replace(".", ""))
                    return (r1 / r2) * pow(10, t2 - t1);
                }
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "PurchaseOrderNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../PurchaseManagement/PurchaseOrderView.aspx?id=" +
                       record.get('PurchaseOrderId') + "\",\"childwind\",\"" + WinStyle + "\")'>" + value + "</label>";

                        }
                        break;
                    case "BuyPrice":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = $("#Symbo").val() + whole;
                        break;
                    case "Amount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = $("#Symbo").val() + whole;
                        break;
                    case "ActualPayAmount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = "￥" + whole;
                        break;
                    default: //因为有汇总插件存在 所以存在第三种情形
                        rtn = value;
                        break;
                }
                return rtn;
            }
       
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            付款单信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-td-caption">
                    付款单编号：
                </td>
                <td class="aim-ui-td-data">
                    <input id="PayBillNo" name="PayBillNo" />
                </td>
                <td class="aim-ui-td-caption">
                    供应商名称：
                </td>
                <td class="aim-ui-td-data">
                    <input id="SupplierName" name="SupplierName" readonly="readonly" style="width: 250px" />
                    <input id="Symbo" name="Symbo" type="hidden" />
                    <input id="State" name="State" type="hidden" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    交易币种：
                </td>
                <td>
                    <input id="MoneyType" name="MoneyType" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    应付金额<label id="label"></label>：
                </td>
                <td class="aim-ui-td-data">
                    <input id="PAmount" name="PAmount" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    申请人：
                </td>
                <td class="aim-ui-td-data">
                    <input id="CreateName" name="CreateName" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    申请日期：
                </td>
                <td class="aim-ui-td-data">
                    <input id="CreateTime" name="CreateTime" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td>
                    折扣金额（￥）
                </td>
                <td>
                    <input id="DiscountAmount" name="DiscountAmount" readonly="readonly" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    实付金额(￥)：
                </td>
                <td>
                    <input id="ActuallyPayAmount" name="ActuallyPayAmount" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    折算汇率：
                </td>
                <td class="aim-ui-td-data">
                    <input id="ExchangeRate" name="ExchangeRate" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    备注：
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <textarea id="Remark" name="Remark" style="width: 73.5%" readonly="readonly"></textarea>
                </td>
            </tr>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <div id="sub2" name="sub2" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
