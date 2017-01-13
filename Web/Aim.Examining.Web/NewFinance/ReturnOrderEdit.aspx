<%@ Page Title="退货单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="ReturnOrderEdit.aspx.cs" Inherits="Aim.Examining.Web.ReturnOrderEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .aim-ui-td-caption
        {
            text-align: right;
        }
        fieldset
        {
            margin: 15px;
            width: 100%;
            padding: 5px;
        }
        fieldset legend
        {
            font-size: 12px;
            font-weight: bold;
        }
        .righttxt
        {
            text-align: right;
        }
        input
        {
            width: 90%;
        }
        select
        {
            width: 90%;
        }
        .x-superboxselect-display-btns
        {
            width: 90% !important;
        }
        .x-form-field-trigger-wrap
        {
            width: 100% !important;
        }
    </style>

    <script type="text/javascript">
        var id = $.getQueryString({ ID: "id" });
        var mdls, tabPanel, grid, store;
        function onPgLoad() {
            setPgUI();
            getPrice();
        }
        function setPgUI() {
            InitGrid();
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }
        function SuccessSubmit() {
            $("#btnSubmit").hide();
            if (store.getRange().length == 0) {
                alert("退货单的明细不能为空！");
                return;
            }
            var warehousename = $("#WarehouseId").find("option:selected").text();
            $("#WarehouseName").val(warehousename);
            var dt = store.getModifiedDataStringArr(store.getRange()) || []
            AimFrm.submit(pgAction, { data: dt }, null, SubFinish);
        }
        function SubFinish(args) {
            RefreshClose();
        }
        function InitGrid() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                data: myData,
                fields: [
                { name: 'Id' }, { name: 'ReturnOrderId' }, { name: 'OrderPartId' }, { name: 'ProductId' }, { name: 'ProductCode' },
			    { name: 'ProductName' }, { name: 'Isbn' }, { name: 'Count' }, { name: 'Unit' }, { name: 'ReturnPrice' },
			    { name: 'Amount' }, { name: 'Reason' }, { name: 'Remark' }, { name: 'OriginalCount' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, hidden: true },
                    { id: 'OrderPartId', header: 'OrderPartId', dataIndex: 'OrderPartId', width: 80, hidden: true },
                    { id: 'ProductId', header: 'ProductId', dataIndex: 'ProductId', width: 80, hidden: true },
                    { id: 'Isbn', header: 'Isbn', dataIndex: 'Isbn', width: 80, hidden: true },
                    { id: 'OriginalCount', header: 'OriginalCount', dataIndex: 'OriginalCount', width: 80, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode', width: 150, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 120 },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60 },
                    { id: 'ReturnPrice', header: '退货价', dataIndex: 'ReturnPrice', renderer: filterValue, width: 100 }, //, editor: new Ext.form.NumberField({ allowBlank: false, minValue: 0 }) 
                    {id: 'Count', header: '<label style="color:red;">退货数量</label>', dataIndex: 'Count', summaryType: 'sum', width: 100, editor: new Ext.form.NumberField({ id: 'ReturnCount', allowBlank: false, minValue: 1 }) },
                    { id: 'Amount', dataIndex: 'Amount', header: '退货金额', summaryType: 'sum', width: 100, renderer: function(val) {
                        return filterValue(Math.round(val * 100) / 100);
                    }
                    },
                    { id: 'Remark', header: '<label style="color:red;">备注</label>', dataIndex: 'Remark', width: 100, editor: new Ext.form.TextArea({}) }
                ]
            });
            grid = new Ext.ux.grid.AimEditorGridPanel({
                title: '退货单明细',
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                width: Ext.get("StandardSub").getWidth(),
                autoHeight: true,
                columnLines: true,
                viewConfig: {
                    forceFit: true
                },
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: [{
                        text: '删除',
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要删除的记录！");
                                return;
                            }
                            store.remove(recs);
                            getPrice();
                        }
                    }
]
                }),
                autoExpandColumn: 'ProductCode',
                listeners: { afteredit: function(val) {
                    if (val.field == "ReturnPrice" || val.field == "Count") {
                        val.record.set("Amount", (val.record.get("ReturnPrice")) * (val.record.get("Count")));
                        getPrice();
                    }
                }, beforeedit: function(e) {
                    if (e.field == "Count") {
                        Ext.getCmp("ReturnCount").setMaxValue(e.record.get("OriginalCount"));
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
            var count = store.getCount();
            var result = 0.0;
            var temp;
            for (var i = 0; i < count; i++) {
                temp = store.getAt(i);
                result += parseFloat(temp.get("ReturnPrice")) * parseFloat(temp.get("Count"));
            }
            if (result) {
                $("#ReturnMoney").val(result);
            }
            else {
                $("#ReturnMoney").val(0);
            }
        }
        function AfterSelect() {
            $.ajaxExec("GetSaleOrderPart", { OrderNumber: $("#OrderNumber").val() }, function(rtn) {
                if (rtn.data.Result) {
                    var data = rtn.data.Result;
                    var recType = store.recordType;
                    $.each(data, function() {
                        var rec = new recType({ OrderPartId: this.Id, ProductId: this.PId, ProductCode: this.PCode, ProductName: this.PName, Unit: this.Unit,
                            ReturnPrice: this.SalePrice, Count: parseFloat(this.Count) - parseFloat(this.ReturnCount ? this.ReturnCount : 0),
                            Amount: this.Amount, OriginalCount: parseFloat(this.Count) - parseFloat(this.ReturnCount ? this.ReturnCount : 0)
                        });
                        store.insert(store.data.length, rec);
                    })
                    getPrice();
                }
            })
        }
        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            退货单</h1>
    </div>
    <table class="aim-ui-table-edit" width="100%">
        <tr style="display: none">
            <td>
                <input id="CId" name="CId" />
                <input id="Id" name="Id" />
            </td>
        </tr>
        <tr>
            <td class="aim-ui-td-caption" style="width: 20%">
                退货单号
            </td>
            <td class="aim-ui-td-data" style="width: 30%">
                <input id="Number" name="Number" disabled="disabled" value="自动生成" />
            </td>
            <td class="aim-ui-td-caption" style="width: 20%">
                销售单号
            </td>
            <td class="aim-ui-td-data" style="width: 30%">
                <input id="OrderNumber" name="OrderNumber" class="validate[required]" aimctrl='popup'
                    readonly="readonly" style="width: 80%" popurl="/NewFinance/SaleOrderSelect.aspx?seltype=single"
                    popparam="CId:CId;CName:CName;OrderNumber:Number" popstyle="width=900,height=550"
                    afterpopup="AfterSelect" />
            </td>
        </tr>
        <tr>
            <td class="aim-ui-td-caption">
                客户名称
            </td>
            <td class="aim-ui-td-data">
                <input id="CName" name="CName" readonly="readonly" />
            </td>
            <td class="aim-ui-td-caption">
                退款金额
            </td>
            <td class="aim-ui-td-data">
                <input id="ReturnMoney" disabled="disabled" name="ReturnMoney" />
            </td>
        </tr>
        <tr>
            <td class="aim-ui-td-caption">
                收货仓库
            </td>
            <td class="aim-ui-td-data">
                <select id="WarehouseId" name="WarehouseId" aimctrl="select" enum="AimState['Warehouse']"
                    class="validate[required]">
                </select>
                <input type="hidden" id="WarehouseName" name="WarehouseName" />
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="aim-ui-td-caption">
                备注
            </td>
            <td class="aim-ui-td-data" colspan="3">
                <textarea id="Remark" name="Remark" style="width: 97%" cols="" rows=""></textarea>
            </td>
        </tr>
    </table>
    <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
    </div>
    <table class="aim-ui-table-edit">
        <tr>
            <td class="aim-ui-button-panel">
                <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">
                    取消</a>
            </td>
        </tr>
    </table>
</asp:Content>
