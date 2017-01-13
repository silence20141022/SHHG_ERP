<%@ Page Title="销售单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmOrdersEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmOrdersEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        var label = '<label style="color:red;">';
        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });
        var index = 0;
        var mdls, tabPanel, grid, store;
        function onPgLoad() {
            setPgUI();
            $("#btninsertexcel").val("导入Excel");
        }
        function setPgUI() {
            if (getQueryString("updatemoney") == "yes") {
                label = '<label>';
            }
            if (pgOperation == "c") {
                $("#CreateName").val(AimState.UserInfo.Name);
            }
            InitEditTable();
            if (pgOperation == "r") {
                $("#tdexcel").css("display", "none");
            }
            //绑定导入excel数据
            $('#btninsertexcel').click(function () {
                if ($("#excel").val().length == 0) {
                    alert("请先上传Excel文件");
                    return;
                }

                var recs = grid.store.getRange();
                var subdata = [];
                $.each(recs, function () {
                    subdata.push(this.data);
                });

                jQuery.ajaxExec('inputexcel', { "path": $("#excel").val().substring(0, $("#excel").val().length - 1), "json": $.getJsonString(subdata) }, function (rtn) {
                    if (rtn.data.error) {
                        alert(rtn.data.error);
                    }
                    else {
                        $("#StandardSub").html("");
                        $("#Child").val(rtn.data.result);
                        InitEditTable();
                    }
                });
            });
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function () {
                window.close();
            });

            //设置金额可写
            //if (getQueryString("updatemoney") == "yes") {
            //    $("#TotalMoney").removeAttr("disabled");
            //}
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            //隐藏提交按钮
            $("#btnSubmit").hide();
            if (getQueryString("updatemoney") == "yes") {
                var reg = new RegExp("^\\d+(\\.\\d+)?$");
                if (reg.test($("#TotalMoney").val())) {
                    //保存
                    jQuery.ajaxExec('updateAmount', { "id": $("#Id").val(), "TotalMoney": $("#TotalMoney").val(), "DiscountAmount": $("#DiscountAmount").val() }, function () {
                        try {
                            window.opener.location = window.opener.location;
                            window.close();
                        } catch (e) {
                            window.close();
                        }
                    });
                }
                else {
                    alert("总金额填写错误");
                }
                return;
            }
            //判断是否填写销售人员
            if (!$("#Salesman").val()) {
                alert("请填写销售人员");
                return;
            }
            ProcGridData();
            if ($("#Child").val() == "[]") {
                alert("未添加产品信息");
                return;
            }
            var msg = "";
            var url = "";
            if (!getQueryString("paid")) {
                var count = store.getCount();
                for (var i = 0; i < count; i++) {
                    temp = store.getAt(i);
                    if (temp.get("Price") < temp.get("MinSalePrice")) {
                        msg = "商品：" + temp.get("Name") + " 售价低于最低售价，需审批";
                        url = "FrmPriceApplyEdit.aspx?op=c&optype=undefined?op=c";
                        break;
                    }
                }
            }
            if (msg != "" && url != "") {
                if (!confirm(msg + "，是否继续？"))
                    return;
            }
            var recs = store.getRange();
            var dt = store.getModifiedDataStringArr(recs) || [];
            AimFrm.submit(pgAction, { "PAState": msg != "" && url != "" ? "Yes" : "No", "data": dt }, null, function (rtn) {
                try {
                    window.opener.location = window.opener.location;
                    window.close();
                } catch (e) {
                    window.close();
                }
            });
        }
        function SubFinish(args) {
            RefreshClose();
        }
        function MultiAddPros() {
            var style = "dialogWidth:700px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/ProductSelect.aspx?seltype=multi&rtntype=array&PId=" + $("#Id").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function () {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ "PId": users[i].Id, "Isbn": users[i].Isbn, "Code": users[i].Code, "Name": users[i].Name, "MinSalePrice": users[i].MinSalePrice, "Count": "", "Amount": "", "Unit": users[i].Unit });
                    insRowIdx = store.data.length;
                    if (store.findExact('PId', users[i].Id) == -1) {
                        store.insert(insRowIdx, p);
                        grid.startEditing(insRowIdx, 3);
                    }
                }
                getPrice();
            });
        }
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function () {
                subdata.push(this.data);
            });
            var jsonstr = $.getJsonString(subdata);
            $("#Child").val(jsonstr);
        }
        function InitEditTable() {
            myData = {
                records: AimState["DetailList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
                { name: 'Id' }, { name: 'CustomerOrderNo' }, { name: 'PId' }, { name: 'Isbn' }, { name: 'Code' },
			    { name: 'Name' },
			    { name: 'MinSalePrice' },
			    { name: 'Price' },
			    { name: 'Unit' },
			    { name: 'OutCount' },
			    { name: 'Amount' },
			    { name: 'Count' },
			    { name: 'BillingCount' },
			    { name: 'Remark' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, resizable: true, hidden: true },
                    { id: 'Isbn', header: 'Isbn', dataIndex: 'Isbn', width: 80, resizable: true, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 150, resizable: true, summaryRenderer: function (v, params, data) { return "汇总:" } },
                    { id: 'Code', header: '产品型号', dataIndex: 'Code', width: 150, resizable: true, renderer: ExtGridpperCase },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 100, resizable: true },
                    { id: 'MinSalePrice', header: '最低售价', dataIndex: 'MinSalePrice', width: 100, resizable: true, hidden: true },
                    { id: 'Price', header: label + '销售价格</label>', dataIndex: 'Price', width: 100, resizable: true,
                        editor: new Ext.form.NumberField({ allowBlank: false })
                    },
                    { id: 'Count', header: label + '销售数量</label>', dataIndex: 'Count', width: 100, resizable: true, summaryType: 'sum', allowBlank: false, editor: new Ext.form.NumberField({ allowBlank: false }) },
                    { id: 'OutCount', header: '已出库量', dataIndex: 'OutCount', width: 80, resizable: true, summaryType: 'sum' },
                    { id: 'BillingCount', header: '已开票数量', dataIndex: 'BillingCount', width: 80, resizable: true, summaryType: 'sum', allowBlank: false },
                    { id: 'Amount', dataIndex: 'Amount', header: '销售金额', summaryType: 'sum', width: 100, renderer: function (val) {
                        return filterValue(Math.round(val * 100) / 100);
                    }
                    },
                    { id: 'CustomerOrderNo', header: label + '客户订单号</label>', dataIndex: 'CustomerOrderNo', width: 120, resizable: true, editor: { xtype: 'textfield'} },
                    { id: 'Remark', header: label + '备注</label>', dataIndex: 'Remark', width: 100, resizable: true, editor: new Ext.form.TextArea({}) }
                ]
            });
            grid = new Ext.ux.grid.AimEditorGridPanel({
                title: '【销售单明细】',
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                width: Ext.get("StandardSub").getWidth(),
                forceLayout: true,
                columnLines: true,
                viewConfig: {
                    forceFit: true
                },
                autoHeight: true,
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: [{
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function () {
                            MultiAddPros();
                            return;
                        }
                    }, {
                        text: '删除',
                        iconCls: 'aim-icon-delete',
                        handler: function () {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要删除的记录！");
                                return;
                            }
                            if (confirm("确定删除所选记录？")) {
                                $.each(recs, function () {
                                    store.remove(this);
                                    getPrice();
                                })
                            }
                        }
                    }, {
                        text: '清空',
                        iconCls: 'aim-icon-delete',
                        handler: function () {
                            if (confirm("确定清空所有记录？")) {
                                store.removeAll();
                                getPrice();
                            }
                        }
                    }]
                }),
                autoExpandColumn: 'Remark',
                listeners: { "afteredit": function (val) {
                    if (val.field == "Price" || val.field == "Count") {
                        val.record.set("Amount", parseFloat(val.record.get("Price")) * parseFloat(val.record.get("Count")));
                        getPrice();
                    }
                    else if (val.field == "Guids" || val.field == "Remark") {
                        //val.record.set(val.field, val.record.get(val.field).replace("\r\n", ","));
                    }
                }
                }
            });
            grid.on('beforeedit', function (obj) {
                if (getQueryString("updatemoney") == "yes") {
                    return false;
                }
            });
            window.onresize = function () {
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
                if (temp.get("Price") && temp.get("Count")) {
                    result += temp.get("Price") * temp.get("Count");
                }
            }
            if (parseFloat($("#DiscountAmount").val())) {
                result = result - parseFloat($("#DiscountAmount").val());
            }
            $("#TotalMoney").val(result);
            $("#TotalMoneyHis").val(result);
        }
        function filterValue(val) {
            if (val) {
                val = String(val);
                var whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return '￥' + (whole == "null" || whole == null ? "" : whole);
            }
        }
        var temp = 0;
        function valChange(val) {
            temp++;
            if (temp == 1) return;
            if (!val || val == 0) {
                $("#CalculateManner option[text='月结']").remove();
                $("#ReceivablesDate").val("");
            }
            else {
                if (!$("#CalculateManner option[text='月结']"))
                    $("#CalculateManner").append("<option value='月结'>月结</option>");
                var date = new Date();
                date.setDate(date.getDate() + parseInt(val));
                $("#ReceivablesDate").val(date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate());
            }
        }
        function getSalesman(val) {
            index++;
            var op = getQueryString("op");
            if ((op == "c" && val && index > 1) || (op == "u" && val && index > 2)) {
                jQuery.ajaxExec("getSalesman", { "CId": val }, function (rtn) {
                    $("#CCode").val(rtn.data.Code);
                    $("#SalesmanId").val(rtn.data.SaleUserId);
                    $("#Salesman").dataBind(rtn.data.SaleUser);
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            销售单</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tr style="display: none">
                <td>
                    <input id="InvoiceState" name="InvoiceState" />
                    <input id="PANumber" name="PANumber" />
                    <input id="PId" name="PId" />
                    <input id="PAState" name="PAState" />
                    <input id="CCode" name="CCode" />
                    <input id="Id" name="Id" />
                    <input id="AccountValidity" name="AccountValidity" onpropertychange="valChange(this.value)" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    销售单号
                </td>
                <td class="aim-ui-td-data">
                    <input id="Number" name="Number" disabled="disabled" value="自动生成" />
                </td>
                <td class="aim-ui-td-caption">
                    客户名称
                </td>
                <td class="aim-ui-td-data">
                    <input aimctrl='customer' required="true" id="CName" name="CName" relateid="CId" />
                    <input type="hidden" name="CId" id="CId" onpropertychange="getSalesman(this.value);" />
                </td>
                <td class="aim-ui-td-caption">
                    开票类型
                </td>
                <td class="aim-ui-td-data">
                    <select id="InvoiceType" name="InvoiceType" aimctrl='select' style="width: 152px;"
                        class="validate[required]" enumdata="AimState['InvoiceType']">
                    </select>
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    结算方式
                </td>
                <td class="aim-ui-td-data">
                    <select id="CalculateManner" name="CalculateManner" aimctrl='select' style="width: 152px;"
                        class="validate[required]" enumdata="AimState['CalculateManner']">
                    </select>
                </td>
                <td class="aim-ui-td-caption">
                    支付方式
                </td>
                <td class="aim-ui-td-data">
                    <select id="PayType" name="PayType" aimctrl='select' style="width: 152px;" class="validate[required]"
                        enumdata="AimState['PayType']">
                    </select>
                </td>
                <td class="aim-ui-td-caption">
                    预存款
                </td>
                <td class="aim-ui-td-data">
                    <input id="PreDeposit" name="PreDeposit" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    总金额
                </td>
                <td class="aim-ui-td-data" width="120px">
                    <input id="TotalMoney" disabled="disabled" name="TotalMoney" />
                    <input id="TotalMoneyHis" type="hidden" name="TotalMoneyHis" />
                </td>
                <td class="aim-ui-td-caption">
                    折扣金额(￥)
                </td>
                <td class="aim-ui-td-data">
                    <input id="DiscountAmount" name="DiscountAmount" onblur="getPrice()" />
                </td>
                <td class="aim-ui-td-caption">
                    要求到货时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="ExpectedTime" aimctrl="date" name="ExpectedTime" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    提货方式
                </td>
                <td class="aim-ui-td-data">
                    <select id="DeliveryMode" aimctrl='select' style="width: 152px;" enumdata="AimState['DeliveryMode']"
                        name="DeliveryMode">
                    </select>
                </td>
                <td>
                    预计收款日期
                </td>
                <td>
                    <input id="ReceivablesDate" aimctrl="date" name="ReceivablesDate" />
                </td>
                <td>
                    销售人员
                </td>
                <td>
                    <input type="hidden" id="SalesmanId" name="SalesmanId" />
                    <input aimctrl="user" id="Salesman" name="Salesman" relateid="SalesmanId" />
                </td>
            </tr>
            <tr style="display: none;">
                <td>
                    创建人
                </td>
                <td colspan="5">
                    <input id="CreateName" disabled="disabled" name="CreateName" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    备注
                </td>
                <td class="aim-ui-td-data" colspan="5">
                    <textarea id="Remark" name="Remark" style="width: 100%" rows="5"></textarea>
                </td>
            </tr>
        </table>
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
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
    </div>
</asp:Content>
