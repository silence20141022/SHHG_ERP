<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="PurchaseInvoiceView.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseInvoiceView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, grid;
        var id = $.getQueryString({ "ID": "id" });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            InitEditTable();
        }
        function InitEditTable() {
            // 表格数据
            myData = {
                records: AimState["DetailList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                fields: [
                { name: 'Id' }, { name: 'PurchaseInvoiceId' }, { name: 'ProductCode' }, { name: 'ProductName' }, { name: 'BuyPrice' },
			    { name: 'Remark' }, { name: 'ProductId' }, { name: 'PurchaseOrderDetailId' }, { name: 'InvoiceQuantity' },
			     { name: 'InvoiceAmount' }, { name: 'Raw' }, { name: 'NoInvoice' }, { name: 'PurchaseOrderNo' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                { id: 'Id', dataIndex: 'Id', hidden: true }, { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                { id: 'Raw', dataIndex: 'Raw', hidden: true },
                  { id: 'NoInvoice', dataIndex: 'NoInvoice', hidden: true },
                { id: 'PurchaseOrderDetailId', dataIndex: 'PurchaseOrderDetailId', hidden: true },
                { id: 'PurchaseInvoiceId', dataIndex: 'PurchaseInvoiceId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'PurchaseOrderNo', header: '采购编号', dataIndex: 'PurchaseOrderNo', width: 120 },
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 120 },
                { id: 'ProductCode', header: '型号', dataIndex: 'ProductCode', width: 200 },
                { id: 'BuyPrice', header: '采购价格', dataIndex: 'BuyPrice', width: 80, renderer: RowRender,
                    summaryRenderer: function(v, params, data) { return "汇总:" }
                },
                { id: 'InvoiceQuantity', header: '<label style="color:red;">数量</label>', dataIndex: 'InvoiceQuantity', width: 80,
                    summaryType: 'sum', editor: { xtype: 'numberfield', id: 'acctual', minValue: 1, decimalPrecision: 0 }
                },
                { id: 'InvoiceAmount', header: '金额', dataIndex: 'InvoiceAmount', width: 80, summaryType: 'sum', renderer: RowRender}]
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
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">采购发票详细信息：</label>']
                }),
                autoExpandColumn: 'ProductCode'
            });
            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
        function getPrice() {
            var result = 0.0;
            for (var i = 0; i < store.getCount(); i++) {
                var rec = store.getAt(i);
                result = FloatAdd(result, rec.get("InvoiceAmount"));
            }
            $("#PAmount").val(result);
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
        function accMul(arg1, arg2) {
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "BuyPrice":
                    val = String(value);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    rtn = $("#Symbo").val() + whole;
                    break;
                case "InvoiceQuantity":
                    rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
                    break;
                case "InvoiceAmount":
                    val = String(value);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    rtn = $("#Symbo").val() + whole;
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
            采购发票信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-td-caption">
                        发票编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PurchaseInvoiceNo" name="PurchaseInvoiceNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        供货商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierName" name="SupplierName" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        交易币种
                    </td>
                    <td>
                        <input id="MoneyType" name="MoneyType" readonly="readonly" />
                        <input id="Symbo" name="Symbo" type="hidden" />
                    </td>
                    <td class="aim-ui-td-caption">
                        发票金额
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InvoiceAmount" name="InvoiceAmount" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        发票号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InvoiceNo" name="InvoiceNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                    </td>
                    <td class="aim-ui-td-data">
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 83.5%" readonly="readonly"></textarea>
                    </td>
                </tr>
                <tr width="100%" style="display: none">
                    <td class="aim-ui-td-caption">
                        录入人
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateName" name="CreateName" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        录入日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateDate" name="CreateDate" dateonly="true" readonly="readonly" />
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
