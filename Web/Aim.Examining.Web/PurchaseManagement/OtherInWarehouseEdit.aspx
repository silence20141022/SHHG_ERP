<%@ Page Title="其他入库" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="OtherInWarehouseEdit.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.OtherInWarehouseEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            InitEditTable();
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#InWarehouseNo").val(AimState["InWarehouseNo"]);
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
                AimDlg.show("未选择任何产品的入库单拒绝提交！"); return;
            }
        }
        function SubFinish(args) {
            RefreshClose();
        }
        //选择订单
        function MultiAddData() {
            var style = "dialogWidth:850px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/OtherInWarehouseSelect.aspx?seltype=multi&rtntype=array";
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = store.recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var index = store.findExact("ProductId", users[i].Id); //判断该产品是否已经添加
                    if (index < 0) {
                        var p = new EntRecord({ ProductCode: users[i].Code, ProductName: users[i].Name, ProductId: users[i].Id,
                            Quantity: 1, ProductISBN: users[i].Isbn, ProductPCN: users[i].Pcn, InWarehouseState: '未入库',
                            Remark: "", ProductType: users[i].ProductType
                        });
                        insRowIdx = store.data.length;
                        store.insert(insRowIdx, p);
                    }
                    else {
                        AimDlg.show("你已经选择了该型号的产品！");
                        return;
                    }
                }
            });
        }
        function InitEditTable() {
            // 表格数据  &SupplierId=" + $("#SupplierId").val()+"&SupplierName=" + escape($("#SupplierName").val())
            myData = {
                records: AimState["DetailList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                fields: [
			    { name: 'InWarehouseId' }, { name: 'Remark' }, { name: 'ProductISBN' },
			    { name: 'ProductPCN' }, { name: 'ProductCode' }, { name: 'Quantity' }, { name: 'ProductType' }, { name: 'InWarehouseState' },
			    { name: 'ProductName' }, { name: 'Id' }, { name: 'ProductId' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                 { id: 'Id', dataIndex: 'Id', hidden: true },
                 { id: 'InWarehouseId', dataIndex: 'InWarehouseId', hidden: true },
                 { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 130 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode', width: 130 },
                { id: 'ProductPCN', header: 'PCN', dataIndex: 'ProductPCN', width: 100 },
                { id: 'ProductISBN', header: '条形码', dataIndex: 'ProductISBN', width: 150, summaryRenderer: function(v, params, data) { return "汇总:" } },
                { id: 'Quantity', dataIndex: 'Quantity', header: '<label style="color:red;">数量</label>', width: 70,
                    editor: { xtype: 'numberfield', minValue: 1, decimalPrecision: 0 }, summaryType: 'sum', renderer: RowRender
                },
            	{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 80 },
                { id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 70 },
   		        { id: 'Remark', dataIndex: 'Remark', header: '<label style="color:red;">备注</label>', width: 90, editor: { xtype: 'textarea' }, renderer: RowRender}]
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
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">入库单产品明细：</label>', '-', {
                        text: '添加产品',
                        iconCls: 'aim-icon-add',
                        handler: function() {
                            //                            if ($("#SupplierId").val() == "") {
                            //                                AimDlg.show("请先选择供应商！"); return;
                            //                            }
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
                                    $.each(recs, function() { store.remove(this); })
                                }
                                else {
                                    var dt = store.getModifiedDataStringArr(recs) || [];
                                    jQuery.ajaxExec("batchdelete", { "data": dt }, function() {
                                        $.each(recs, function() { store.remove(this); })
                                    });
                                }
                            }
                        }
}]
                    }),
                    autoExpandColumn: 'ProductCode'
                });
                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                };
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Quantity":
                        rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
                        break;
                    case "Remark":
                        rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
                        break;
                    default: //因为有汇总插件存在 所以存在第三种情形
                        rtn = value;
                        break;
                }
                return rtn;
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
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            入库单基本信息</h1>
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
                        入库编号
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
                            popparam="SupplierId:Id;SupplierName:SupplierName;MoneyType:MoneyType" popstyle="width=800,height=400" />
                        <input id="SupplierId" name="SupplierId" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        入库类型
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="InWarehouseType" name="InWarehouseType" aimctrl='select' style="width: 152px;"
                            class="validate[required]" enumdata="AimState['InWarehouseType']">
                        </select>
                    </td>
                    <td class="aim-ui-td-caption">
                        预计到货时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="EstimatedArrivalDate" name="EstimatedArrivalDate" aimctrl='date' />
                    </td>
                </tr>
                <tr>
                    <td>
                        入库仓库
                    </td>
                    <td>
                        <input id="Name" name="Name" aimctrl='popup' style="width: 152px" popurl="/CommonPages/Select/WarehouseSelect.aspx?seltype=single"
                            popparam="Name:Name;WarehouseId:Id" class="validate[required]" />
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
