<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="WarehouseExchangeView.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.WarehouseExchangeView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            InitEditTable();
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#ExchangeNo").val(AimState["ExchangeNo"]);
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
                AimDlg.show("未选择任何产品的采购单拒绝提交！"); return;
            }
        }
        function SubFinish(args) {
            RefreshClose();
        }
        //选择订单
        function MultiAddData() {
            var style = "dialogWidth:850px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/WarehouseExchangeProductSelect.aspx?seltype=multi&rtntype=array&WarehouseId=" + $("#FromWarehouseId").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = store.recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var index = store.find("ProductId", users[i].Id); //判断该产品是否已经添加
                    if (index < 0) {
                        var p = new EntRecord({ ProductId: users[i].ProductId, ProductName: users[i].ProductName, ProductCode: users[i].ProductCode,
                            ProductIsbn: users[i].ProductIsbn, ProductPcn: users[i].ProductPcn, ExchangeQuantity: 1,
                            StockQuantity: users[i].StockQuantity
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
                records: AimState["DataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'WarehouseExchangeId' }, { name: 'ProductId' }, { name: 'ProductName' },
			    { name: 'ProductCode' }, { name: 'ProductIsbn' }, { name: 'ProductPcn' }, { name: 'ExchangeQuantity' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                 { id: 'Id', dataIndex: 'Id', hidden: true },
                 { id: 'WarehouseExchangeId', dataIndex: 'WarehouseExchangeId', hidden: true },
                 { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 100 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode' },
                { id: 'ProductIsbn', header: '条形码', dataIndex: 'ProductIsbn', width: 150 },
                { id: 'ProductPcn', header: 'PCN', dataIndex: 'ProductPcn', width: 120, resizable: true },
                { id: 'ExchangeQuantity', dataIndex: 'ExchangeQuantity', header: '调拨数量', width: 60
}]

                });

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
                        items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">调拨产品详细信息：</label>', '-']
                    }),
                    autoExpandColumn: 'ProductCode',
                    listeners: { "afteredit": function(val) {
                    }
                    }
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

            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Quantity":
                        rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
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
            仓库管理--》调拨单信息</h1>
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
                        调拨单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ExchangeNo" name="ExchangeNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        调出仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="FromWarehouseName" name="FromWarehouseName" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        调入仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ToWarehouseName" name="ToWarehouseName" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        出库状态
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="OutWarehouseState" name="OutWarehouseState" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        入库状态
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InWarehouseState" name="InWarehouseState" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        结束时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="EndTime" name="EndTime" readonly="readonly" />
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
