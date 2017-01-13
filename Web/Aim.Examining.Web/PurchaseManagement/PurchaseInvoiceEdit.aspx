<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="PurchaseInvoiceEdit.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseInvoiceEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, grid;
        var id = $.getQueryString({ "ID": "id" });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            InitEditTable();
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateName").val(AimState.UserInfo.Name);
                $("#CreateTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
                $("#PurchaseInvoiceNo").val(AimState["PurchaseInvoiceNo"]);
            }
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }
        //验证成功执行保存方法
        function SuccessSubmit() {
            var recs = store.getRange();
            var dt = store.getModifiedDataStringArr(recs) || [];
            //如果存在明细的情形下  如果明细的金额和大于发票的金额。不允许提交
            var total = 0.00;
            if (store.getCount() > 0) {
                for (var i = 0; i < store.getCount(); i++) {
                    total = FloatAdd(total, recs[i].get("InvoiceAmount"));
                }
                if (total > $("#InvoiceAmount").val()) {
                    AimDlg.show("明细的金额大于发票金额，请重新调整明细！");
                    return;
                }
            }
            AimFrm.submit(pgAction, { "data": dt }, null, SubFinish);
        }
        function SubFinish(args) {
            RefreshClose();
        }
        //选择订单
        function MultiAddData() {
            var style = "dialogWidth:800px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/InvoiceSelect.aspx?seltype=multi&rtntype=array&SupplierId=" + $("#SupplierId").val() +
            "&MoneyType=" + $("#MoneyType").val() + "&SupplierName=" + $("#SupplierName").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var index = store.find("PurchaseOrderDetailId", users[i].Id); //判断该订单是否已经存在于store中
                    if (index < 0) {
                        var p = new EntRecord({ PurchaseOrderDetailId: users[i].Id, PurchaseOrderNo: users[i].PurchaseOrderNo,
                            ProductCode: users[i].Code, ProductName: users[i].Name,
                            ProductId: users[i].ProductId, BuyPrice: users[i].BuyPrice, InvoiceQuantity: users[i].NoInvoice,
                            InvoiceAmount: accMul(users[i].BuyPrice, users[i].NoInvoice), NoInvoice: users[i].NoInvoice
                        });
                        insRowIdx = store.data.length;
                        store.insert(insRowIdx, p);
                    }
                }
                getPrice();
            });
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
			     { name: 'InvoiceAmount' }, { name: 'Raw' }, { name: 'NoInvoice' }
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
                { id: 'PurchaseOrderNo', header: '采购编号', dataIndex: 'PurchaseOrderNo', width: 120 }, new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 120 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode', width: 200 },
                { id: 'BuyPrice', header: '采购价格', dataIndex: 'BuyPrice', width: 80, renderer: RowRender,
                    summaryRenderer: function(v, params, data) { return "汇总:" }
                },
                { id: 'InvoiceQuantity', header: '<label style="color:red;">数量</label>', dataIndex: 'InvoiceQuantity', renderer: RowRender,
                    width: 80, summaryType: 'sum', editor: { xtype: 'numberfield', id: 'acctual', minValue: 1, decimalPrecision: 0 }
                },
                { id: 'InvoiceAmount', header: '金额', dataIndex: 'InvoiceAmount', width: 80, summaryType: 'sum', renderer: RowRender}]
            });
            // 表格面板
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
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">采购发票详细信息：</label>', '-', {
                        text: '添加发票明细',
                        iconCls: 'aim-icon-add',
                        handler: function() {
                            if ($("#SupplierId").val() == "") {
                                AimDlg.show("请先选择供应商！");
                                return;
                            }
                            MultiAddData();
                            return;
                        }
                    }, {
                        text: '删除',
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要删除的记录！");
                                return;
                            }
                            if (confirm("确定删除所选记录？")) {
                                if (pgOperation == "c" || pgOperation == "cs") {
                                    $.each(recs, function() {
                                        store.remove(this);
                                        getPrice();
                                    })
                                }
                            }
                        }
}]
                    }),
                    autoExpandColumn: 'ProductCode',
                    listeners: { "afteredit": function(val) {
                        if (val.field == "BuyPrice" || val.field == "InvoiceQuantity") {
                            val.record.set("InvoiceAmount", accMul(val.record.get("BuyPrice"), val.record.get("InvoiceQuantity")));
                            getPrice();
                        }
                    }, "beforeedit": function(e) {
                        if (e.record.get("Id")) {//添加的记录
                            Ext.getCmp("acctual").setMaxValue(FloatAdd(e.record.get("NoInvoice"), e.record.get("Raw")));
                        }
                        else {//修改的记录
                            Ext.getCmp("acctual").setMaxValue(e.record.get("NoInvoice"));
                        }
                    }
                    }
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
                $("#InvoiceAmount").val(result);
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
                <tr style="display: none">
                    <td colspan="4">
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        发票编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PurchaseInvoiceNo" name="PurchaseInvoiceNo" class="validate[required]" />
                    </td>
                    <td class="aim-ui-td-caption">
                        供应商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="SupplierName" name="SupplierName" relateid="txtuserid"
                            style="width: 200px;" popurl="/CommonPages/Select/SupplierSelect.aspx?seltype=single"
                            popparam="SupplierId:Id;SupplierName:SupplierName;MoneyType:MoneyType;Symbo:Symbo"
                            popstyle="width=800,height=400" />
                        <input id="SupplierId" name="SupplierId" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td>
                        交易币种
                    </td>
                    <td>
                        <input id="MoneyType" name="MoneyType" readonly="readonly" />
                        <input id="Symbo" name="Symbo" readonly="readonly" type="hidden" />
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
                        <input id="InvoiceNo" name="InvoiceNo" />
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
                        <textarea id="Remark" name="Remark" style="width: 83.5%"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <table>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnSubmit" class="aim-ui-button submit">提交</a> <a id="btnCancel" class="aim-ui-button cancel">
                            取消</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
