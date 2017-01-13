<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    Title="产品组装拆分" CodeBehind="CombineSplitEdit.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.CombineSplitEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, grid;
        var enum_OperateType = { 组装: '组装', 拆分: '拆分' };
        var id = $.getQueryString({ "ID": "id" });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            InitEditTable();
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateName").val(AimState.UserInfo.Name);
                $("#CreateTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
                $("#CombineSplitNo").val(AimState["CombineSplitNo"]);
            }
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }
        //验证成功执行保存方法
        function SuccessSubmit() {
            if ($("#OperateType").val() == "拆分") {
                var val1 = parseInt($("#StockQuantity").val());
                var val2 = parseInt($("#ProductQuantity").val());
                if (val1 < val2) {
                    AimDlg.show("待拆分产品的本仓库存不能小于拆分数量！");
                    return;
                }
            }
            else {
                var allow = true;
                $.each(store.getRange(), function() {
                    var val3 = parseInt(this.get("ProductQuantity"));
                    var val4 = parseInt(this.get("StockQuantity"));
                    if (val4 < val3) {
                        allow = false;
                        return false;
                    }
                }); if (!allow) {
                    AimDlg.show("组装产品详细的本仓库存不能小于组装数量！");
                    return;
                }
            }
            var recs = store.getRange();
            var dt = store.getModifiedDataStringArr(recs) || [];
            AimFrm.submit(pgAction, { data: dt }, null, SubFinish);
        }
        function SubFinish(args) {
            RefreshClose();
        }
        function InitEditTable() {
            // 表格数据
            myData = {
                records: AimState["DetailDataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailDataList',
                data: myData,
                fields: [
                { name: 'Id' }, { name: 'PId' }, { name: 'ProductCode' }, { name: 'ProductName' }, { name: 'RawValue' },
			    { name: 'ProductId' }, { name: 'ProductQuantity' }, { name: 'StockQuantity' }, { name: 'ProductPcn'}]
            });
            // 表格面板
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                columns: [
                { id: 'Id', dataIndex: 'Id', hidden: true },
                { id: 'RawValue', dataIndex: 'RawValue', hidden: true, header: 'RawValue' },
                { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 150 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode', width: 200 },
                { id: 'ProductPcn', header: 'PCN', dataIndex: 'ProductPcn', width: 100 },
                { id: 'StockQuantity', header: '本仓库存', dataIndex: 'StockQuantity', width: 70 },
                { id: 'ProductQuantity', header: '操作数量', dataIndex: 'ProductQuantity', width: 70 }
                ],
                renderTo: "StandardSub",
                columnLines: true,
                width: Ext.get("StandardSub").getWidth(),
                autoHeight: true,
                plugins: new Ext.ux.grid.GridSummary(),
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">组装拆分明细：</label>', '->'
                   ]
                }),
                autoExpandColumn: 'ProductCode'
            });
            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
        function LoadProductDetail(rtn) {
            $("#StockQuantity").val(rtn.StockQuantity);
            $("#ProductQuantity").val('1');
            jQuery.ajaxExec("LoadProductDetail", { ProductId: $("#ProductId").val(), WarehouseId: $("#WarehouseId").val() }, function(rtn) {
                if (rtn.data.DetailDataList) {
                    store.removeAll();
                    $.each(rtn.data.DetailDataList, function() {
                        var recType = store.recordType;
                        var index = store.data.length;
                        var rec = new recType(this);
                        store.insert(index, rec);
                    });
                }
            });
        }
        function QuantityChange() {
            $.each(store.getRange(), function() {
                var temp = parseFloat($("#ProductQuantity").val()) * parseFloat(this.get("RawValue"));
                this.set("ProductQuantity", temp);
            })
            store.commitChanges();
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                //                case "RawValue":      
                //                    rtn = record.get("ProductQuantity");      
                //                    break;      
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
            销售管理=》组装拆分</h1>
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
                        组装拆分编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CombineSplitNo" name="CombineSplitNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        操作类型
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="OperateType" name="OperateType" aimctrl='select' style="width: 152px;"
                            class="validate[required]" enumdata="enum_OperateType">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        产品名称
                    </td>
                    <td>
                        <input aimctrl='popup' id="ProductName" name="ProductName" class="validate[required]"
                            readonly="readonly" popurl="/PurchaseManagement/CombineSplitProductSelect.aspx"
                            popparam="ProductId:ProductId;ProductName:ProductName;ProductCode:ProductCode;ProductPcn:Pcn;WarehouseId:WarehouseId;WarehouseName:WarehouseName;StockQuantity:StockQuantity"
                            popstyle="width=800,height=550" afterpopup="LoadProductDetail" />
                        <input id="ProductId" name="ProductId" type="hidden" />
                    </td>
                    <td>
                        PCN
                    </td>
                    <td>
                        <input id="ProductPcn" name="ProductPcn" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        操作仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="WarehouseName" name="WarehouseName" readonly="readonly" />
                        <input id="WarehouseId" name="WarehouseId" type="hidden" />
                    </td>
                    <td class="aim-ui-td-caption">
                        本仓库存
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="StockQuantity" name="StockQuantity" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        操作数量
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ProductQuantity" name="ProductQuantity" onchange="QuantityChange()" class="validate[required,custom[onlyInteger]]" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        产品型号
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <input id="ProductCode" name="ProductCode" readonly="readonly" style="width: 83%" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 83%"></textarea>
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
