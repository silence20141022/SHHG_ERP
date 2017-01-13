<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    Title="采购单查看" CodeBehind="PurchaseOrderView.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseOrderView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, grid;
        function onPgLoad() {
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
			    { name: 'PurchaseOrderNo' }, { name: 'PurchaseOrderId' }, { name: 'PCN' }, { name: 'Code' },
			    { name: 'Name' }, { name: 'Id' }, { name: 'BuyPrice' }, { name: 'Quantity' }, { name: 'Amount' },
			    { name: 'ExpectedArrivalDate' }, { name: 'ConfirmOutFactoryDate' }, { name: 'DelieveGoodsDate' },
                { name: 'InvoiceNo' }, { name: 'PayState' }, { name: 'InvoiceState' }, { name: 'InWarehouseState' }, { name: 'ProductId' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                 { id: 'Id', dataIndex: 'Id', hidden: true },
                 { id: 'PurchaseOrderId', dataIndex: 'PurchaseOrderId', hidden: true },
                 { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 100 },
                { id: 'Code', header: '产品型号', dataIndex: 'Code' },
                { id: 'PCN', header: 'PCN', dataIndex: 'PCN', width: 160, summaryRenderer: function(v, params, data) { return "汇总:" } },
                { id: 'BuyPrice', header: '单价', dataIndex: 'BuyPrice', width: 100, resizable: true, renderer: RowRender },
                { id: 'Quantity', dataIndex: 'Quantity', header: '数量', width: 50, summaryType: 'sum' },
		        { id: 'Amount', dataIndex: 'Amount', header: '金额', width: 100, resizable: true, summaryType: 'sum', renderer: RowRender,
		            summaryRenderer: function(v, params, data) {
		                return $("#Symbo").val() + filterValue(v);
		            }
		        },
                // { id: 'ConfirmOutFactoryDate', dataIndex: 'ConfirmOutFactoryDate', header: '出厂时间', width: 70, renderer: ExtGridDateOnlyRender },
                // { id: 'ExpectedArrivalDate', dataIndex: 'ExpectedArrivalDate', header: '到货时间', width: 70, renderer: ExtGridDateOnlyRender },
		        {id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 60 },
		        { id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 60 },
		        { id: 'InvoiceState', dataIndex: 'InvoiceState', header: '发票状态', width: 60 }
		        ]
            });

            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                columnLines: true,
                width: Ext.get("StandardSub").getWidth(),
                autoHeight: true,
                plugins: new Ext.ux.grid.GridSummary(),
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">采购单产品详细信息：</label>']
                }),
                autoExpandColumn: 'Code'
            });

            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
        function accMul(arg1, arg2) {
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
        function filterValue(value) {
            if (value) {
                value = String(value);
                var whole = value;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return whole;
            }
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "BuyPrice":
                    if (value) {
                        rtn = $("#Symbo").val() + filterValue(value);
                    }
                    break;
                case "Amount":
                    if (value) {
                        rtn = $("#Symbo").val() + filterValue(value);
                    }
                    break;
            }
            return rtn;
        }
        
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            采购管理=》采购信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-td-caption">
                        采购编号：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PurchaseOrderNo" name="PurchaseOrderNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        供应商名称：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierName" name="SupplierName" readonly="readonly" style="width: 230px" />
                        <input id="Symbo" name="Symbo" style="display: none" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        产品类型：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ProductType" name="ProductType" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        价格类型：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PriceType" name="PriceType" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        金额合计：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PurchaseOrderAmount" name="PurchaseOrderAmount" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        订单号：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierBillNo" name="SupplierBillNo" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        运货方式
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="TransportationMode" name="TransportationMode" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-data">
                        订货日期
                    </td>
                    <td>
                        <input id="OrderDate" name="OrderDate" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        要求发货日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="RequestDeliveryDate" name="RequestDeliveryDate" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-data">
                        交易币种：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="MoneyType" name="MoneyType" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        下单人：
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateName" name="CreateName" readonly="readonly" style="background-color: ActiveBorder" />
                    </td>
                    <td>
                        <input id="Symbo" name="Symbo" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注：
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 83.5%" readonly="readonly"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
