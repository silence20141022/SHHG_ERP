<%@ Page Title="销售开票" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="SaleOrderInvoiceEdit.aspx.cs" Inherits="Aim.Examining.Web.SaleOrderInvoiceEdit" %>

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
    </style>

    <script type="text/javascript">
        var grid, store, tlBar, myData;
        var orderids = $.getQueryString({ ID: 'orderids' });
        function onPgLoad() {
            setPgUI();
            if (pgOperation == "c") {
                getInvoiceAmount();
            }
        }
        function setPgUI() {
            InitGrid();
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }
        function SuccessSubmit() {
            if (store.getCount() == 0) {
                AimDlg.show("开票明细不能为空！");
                return;
            }
            $.ajaxExec("JudgeRepeat", { InvoiceNo: $("#Number").val() }, function(rtn) {
                if (rtn.data.IsRepeat == "F") {
                    var recs = store.getRange();
                    var dt = store.getModifiedDataStringArr(recs) || [];
                    AimFrm.submit(pgAction, { data: dt, orderids: orderids }, null, SubFinish);
                }
                else {
                    AimDlg.show("同样的发票号码已经存在,请检查是否输入错误！");
                    return;
                }
            })
        }
        function SubFinish(args) {
            // RefreshClose();
            window.opener.store.reload();
            window.close();
        }
        function MultiAddPros() {
            var style = "dialogWidth:550px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/ProductSelect.aspx?seltype=multi&rtntype=array&PId=" + $("#Id").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ "Id": users[i].Id, "PCode": users[i].Code, "PName": users[i].Name, "SalePrice": users[i].SalePrice, "Count": 1, "Amount": users[i].SalePrice, "Unit": users[i].Unit });
                    insRowIdx = store.data.length;
                    store.insert(insRowIdx, p);
                    //var rec = store.getAt(insRowIdx);
                    grid.startEditing(insRowIdx, 3);
                }
                getInvoiceAmount();
            });
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
                { name: 'Id' }, { name: 'OrderInvoiceId' }, { name: 'OrderDetailId' }, { name: 'SaleOrderId' }, { name: 'ProductId' },
                { name: 'ProductCode' }, { name: 'Unit' }, { name: 'ProductName' }, { name: 'SalePrice' }, { name: 'Count' },
                { name: 'NowAmount' }, { name: 'InvoiceCount' }, { name: 'HaveInvoiceCount' }, { name: 'Remark' }, { name: 'ReturnCount' },
                { name: 'Number' }
			    ]
            });
            tlBar = new Ext.ux.AimToolbar({
                items: [
                {
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        if (confirm("确定删除所选记录？")) {
                            $.each(recs, function() {
                                store.remove(this);
                            });
                            getInvoiceAmount();
                        }
                    }
}]
                });
                grid = new Ext.ux.grid.AimEditorGridPanel({
                    title: '发票明细',
                    store: store,
                    columns: [
                    { id: 'Id', header: 'Id', dataIndex: 'Id', hidden: true },
                    { id: 'OrderInvoiceId', header: 'OrderInvoiceId', dataIndex: 'OrderInvoiceId', hidden: true },
                    { id: 'OrderDetailId', header: 'OrderDetailId', dataIndex: 'OrderDetailId', hidden: true },
                    { id: 'SaleOrderId', header: 'SaleOrderId', dataIndex: 'SaleOrderId', hidden: true },
                    { id: 'ProductId', header: 'ProductId', dataIndex: 'ProductId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Number', header: '销售单号', dataIndex: 'Number', width: 130 },
                    { id: 'ProductCode', header: '产品型号', renderer: ExtGridpperCase, dataIndex: 'ProductCode', width: 150, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'ProductName', header: ' 产品名称', dataIndex: 'ProductName', width: 150 },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60 },
                    { id: 'SalePrice', header: '售价', dataIndex: 'SalePrice', width: 80, renderer: filterValue },
                    { id: 'Count', header: '总数量', dataIndex: 'Count', width: 60 },
                    { id: 'InvoiceCount', header: '<label style="color:red;">本次开票数量</label>', dataIndex: 'InvoiceCount', width: 80,
                        editor: new Ext.form.NumberField({ id: 'NowInvoiceCount', allowBlank: false, minValue: 1 })
                    },
                    { id: 'HaveInvoiceCount', header: '已开票数量', dataIndex: 'HaveInvoiceCount', width: 80 },
                    { id: 'ReturnCount', header: '退货数量', dataIndex: 'ReturnCount', width: 80 },
                    { id: 'NowAmount', dataIndex: 'NowAmount', header: '本次开票金额', summaryType: 'sum', width: 100,
                        renderer: RowRender, summaryRenderer: function(v) { return filterValue(v); }
                    }
                ],
                    renderTo: "StandardSub",
                    autoHeight: true,
                    columnLines: true,
                    plugins: new Ext.ux.grid.GridSummary(),
                    tbar: tlBar,
                    autoExpandColumn: 'ProductCode',
                    listeners: { beforeedit: function(e) {
                        if (e.field == "InvoiceCount") {
                            Ext.getCmp("NowInvoiceCount").setMaxValue(parseFloat(e.record.get("Count")) - parseFloat(e.record.get("ReturnCount")) -
                            parseFloat(e.record.get("HaveInvoiceCount")));
                        }
                    }, afteredit: function(val) {
                        if (val.field == "InvoiceCount") {
                            val.record.set("NowAmount", parseFloat(val.record.get("SalePrice")) * parseFloat(val.record.get("InvoiceCount")));
                            getInvoiceAmount();
                        }
                    }
                    }
                });
                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                };
            }
            function getInvoiceAmount() {
                var count = store.getCount();
                var result = 0.0;
                $.each(store.getRange(), function() {
                    result += parseFloat(this.get("SalePrice") ? this.get("SalePrice") : 0) * parseFloat(this.get("InvoiceCount"));
                })
                if ($("#DiscountAmount").val()) {
                    result = result - parseFloat($("#DiscountAmount").val());
                }
                $("#Amount").val(result);
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
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn = "";
                switch (this.id) {
                    case "Title":
                        rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='showwin(\"" +
                                                     record.get('Id') + "\")'>" + value + "</label>";
                        break;
                    case "NowAmount":
                        if (value) {
                            rtn = filterValue(value);
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
            财务开票</h1>
    </div>
    <fieldset>
        <legend>基本信息</legend>
        <table class="aim-ui-table-edit" width="100%" style="border: none">
            <tr style="display: none">
                <td>
                    <input id="Id" name="Id" />
                    <input id="CId" name="CId" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption" style="width: 20%">
                    发票号
                </td>
                <td class="aim-ui-td-data" style="width: 30%">
                    <input id="Number" name="Number" class="validate[required]" />
                </td>
                <td class="aim-ui-td-caption" style="width: 20%">
                    客户名称
                </td>
                <td class="aim-ui-td-data" style="width: 30%">
                    <input id="CName" name="CName" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    发票金额(￥)
                </td>
                <td class="aim-ui-td-data">
                    <input id="Amount" name="Amount" />
                </td>
                <td class="aim-ui-td-caption">
                    折扣金额(￥)
                </td>
                <td class="aim-ui-td-data">
                    <input id="DiscountAmount" name="DiscountAmount" onchange="getInvoiceAmount()" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    开票日期
                </td>
                <td class="aim-ui-td-data">
                    <input id="InvoiceDate" aimctrl="date" name="InvoiceDate" class="validate[required]" />
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
                    <textarea id="Remark" name="Remark" cols="" rows="3" style="width: 96.5%"></textarea>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <div id="StandardSub">
        </div>
    </fieldset>
    <table class="aim-ui-table-edit">
        <tr>
            <td class="aim-ui-button-panel">
                <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">
                    取消</a>
            </td>
        </tr>
    </table>
</asp:Content>
