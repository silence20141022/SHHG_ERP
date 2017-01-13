<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="InWarehouseEdit.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.InWarehouseEdit" %>

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
                $("#InWarehouseNo").val(AimState["InWarehouseNo"]);
                $("#InWarehouseType").val("采购入库");
            }
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }
        //验证成功执行保存方法
        function SuccessSubmit() {
            if (store.getCount() > 0) {
                var recs = store.getRange();
                var dt = store.getModifiedDataStringArr(recs) || [];
                AimFrm.submit(pgAction, { "data": dt }, null, SubFinish);
            }
            else {
                AimDlg.show("请选择采购明细,否则不能生成入库单！");
                return;
            }
        }
        function SubFinish(args) {
            RefreshClose();
        }
        //选择订单
        function MultiAddData() {
            var style = "dialogWidth:850px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/InWarehouseSelect.aspx?seltype=multi&rtntype=array&SupplierId=" + $("#SupplierId").val() +
            "&Symbo=" + $("#Symbo").val() + "&SupplierName=" + $("#SupplierName").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var index = store.find("PurchaseOrderDetailId", users[i].Id); //判断该订单是否已经存在于store中
                    //光判断明细是否存在于入库单还不行。还得加 型号的限制
                    if (index < 0) {
                        var index2 = store.findExact("Code", users[i].Code);
                        if (index2 < 0) {
                            var p = new EntRecord({ PurchaseOrderDetailId: users[i].Id, PurchaseOrderNo: users[i].PurchaseOrderNo,
                                Code: users[i].Code, Name: users[i].Name,
                                ProductId: users[i].ProductId, BuyPrice: users[i].BuyPrice, IQuantity: users[i].NoIn,
                                Amount: accMul(users[i].BuyPrice, users[i].NoIn), NoIn: users[i].NoIn
                            });
                            insRowIdx = store.data.length;
                            store.insert(insRowIdx, p);
                        }
                        else {
                            AimDlg.show("同型号的产品不要放到一个入库单中！");
                            return;
                        }
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
                { name: 'Id' }, { name: 'PurchaseOrderNo' }, { name: 'Code' }, { name: 'Name' },
                { name: 'BuyPrice' }, { name: 'ProductId' }, { name: 'InWarehouseId' },
			    { name: 'PurchaseOrderDetailId' }, { name: 'IQuantity' }, { name: 'Amount' }, { name: 'Raw' }, { name: 'NoIn' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                { id: 'Id', dataIndex: 'Id', hidden: true }, { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                { id: 'NoIn', dataIndex: 'NoIn', hidden: true }, { id: 'Raw', dataIndex: 'Raw', hidden: true },
                { id: 'PurchaseOrderDetailId', dataIndex: 'PurchaseOrderDetailId', hidden: true },
                { id: 'InWarehouseId', dataIndex: 'InWarehouseId', hidden: true },
                { id: 'PurchaseOrderNo', header: '采购编号', dataIndex: 'PurchaseOrderNo', width: 120 },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 120 },
                { id: 'Code', header: '型号', dataIndex: 'Code' },
                { id: 'BuyPrice', header: '采购价格', dataIndex: 'BuyPrice', width: 80,
                    summaryRenderer: function(v, params, data) { return "汇总:" }, renderer: RowRender
                },
                { id: 'IQuantity', header: '<label style="color:red;">数量</label>', dataIndex: 'IQuantity', width: 80, renderer: RowRender,
                    summaryType: 'sum', editor: { xtype: 'numberfield', id: 'acctual', minValue: 1, decimalPrecision: 0 }
                },
                { id: 'Amount', header: '金额', dataIndex: 'Amount', width: 80, summaryType: 'sum', renderer: RowRender}]
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
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">入库单详细信息：</label>', '-', {
                        text: '添加入库明细',
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
                                else {//如果有入库记录根本进不了修改界面。所以这里的删除只需要删除入库详细就可以了 可以确定是没有实际入库记录的
                                    var dt = store.getModifiedDataStringArr(recs) || [];
                                    jQuery.ajaxExec("batchdelete", { data: dt }, function() {
                                        $.each(recs, function() {
                                            store.remove(this);
                                            getPrice();
                                        })
                                    });
                                }
                            }
                        }
}]
                    }),
                    autoExpandColumn: 'Code',
                    listeners: { "afteredit": function(val) {
                        if (val.field == "BuyPrice" || val.field == "IQuantity") {
                            val.record.set("Amount", accMul(val.record.get("BuyPrice"), val.record.get("IQuantity")));
                            getPrice();
                        }
                    }, "beforeedit": function(e) {
                        if (e.record.get("Id")) {//修改的记录
                            Ext.getCmp("acctual").setMaxValue(FloatAdd(e.record.get("NoIn"), e.record.get("Raw")));
                        }
                        else {//添加的记录
                            Ext.getCmp("acctual").setMaxValue(e.record.get("NoIn"));
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
                    result = FloatAdd(result, rec.get("Amount"));
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
                    case "IQuantity":
                        rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
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
            入库单信息</h1>
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
                        入库单编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InWarehouseNo" name="InWarehouseNo" class="validate[required]" />
                    </td>
                    <td class="aim-ui-td-caption">
                        供应商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="SupplierName" name="SupplierName" relateid="txtuserid"
                            style="width: 200px;" popurl="/CommonPages/Select/SupplierSelect.aspx?seltype=single"
                            popparam="SupplierId:Id;SupplierName:SupplierName;Symbo:Symbo" popstyle="width=800,height=400" />
                        <input id="SupplierId" name="SupplierId" type="hidden" />
                        <input id="Symbo" name="Symbo" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td>
                        入库类型
                    </td>
                    <td>
                        <input id="InWarehouseType" name="InWarehouseType" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        预计到货时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="EstimatedArrivalDate" name="EstimatedArrivalDate" aimctrl="date" />
                    </td>
                </tr>
                <tr>
                    <td>
                        入库仓库
                    </td>
                    <td>
                        <input id="WarehouseName" name="WarehouseName" aimctrl='popup' style="width: 152px"
                            popurl="/CommonPages/Select/WarehouseSelect.aspx?seltype=single" popparam="WarehouseName:Name;WarehouseId:Id"
                            class="validate[required]" />
                        <input id="WarehouseId" name="WarehouseId" type="hidden" />
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
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tbody>
                <tr width="100%" style="display: none">
                    <td class="aim-ui-td-caption">
                        录入人
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateName" name="CreateName" />
                    </td>
                    <td class="aim-ui-td-caption">
                        录入日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateDate" name="CreateDate" dateonly="true" />
                    </td>
                </tr>
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
