<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="PurchaseOrderEdit.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseOrderEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        var store;
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        function onPgLoad() {
            setPgUI();
            getPrice();
        }
        function setPgUI() {
            InitEditTable();
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateName").val(AimState.UserInfo.Name);
                $("#CreateTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
                $("#PurchaseOrderNo").val(AimState["PurchaseOrderNo"]);
            }
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function () {
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
            var url = "/CommonPages/Select/PurchaseProductSelect.aspx?seltype=multi&rtntype=array&SupplierId=" + $("#SupplierId").val() + "&ProductType=" + escape($("#ProductType").val())
            + "&PriceType=" + escape($("#PriceType").val()) + "&SupplierName=" + escape($("#SupplierName").val());
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function () {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = store.recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var index = store.find("ProductId", users[i].Id); //判断该产品是否已经添加
                    if (index < 0) {
                        var p = new EntRecord({ "Code": users[i].Code, "Name": users[i].Name, "ProductId": users[i].Id,
                            "BuyPrice": users[i].Price == null ? 0.00 : users[i].Price, "Quantity": 1,
                            "Amount": users[i].Price == null ? 0.00 : users[i].Price, "PCN": users[i].Pcn
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
			    { name: 'PurchaseOrderNo' }, { name: 'PurchaseOrderId' }, { name: 'Amount' }, { name: 'ExpectedArrivalDate' },
			    { name: 'PCN' }, { name: 'Code' }, { name: 'Quantity' }, { name: 'DelieveGoodsDate' }, { name: 'InvoiceNo' },
			    { name: 'Name' }, { name: 'Id' }, { name: 'BuyPrice' }, { name: 'ConfirmOutFactoryDate' }, { name: 'ProductId' }
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
                { id: 'Code', header: '型号', dataIndex: 'Code' },
                { id: 'PCN', header: 'PCN', dataIndex: 'PCN', width: 120 },
                { id: 'BuyPrice', header: '单价', dataIndex: 'BuyPrice', width: 60, resizable: true, renderer: rowRender,
                    summaryRenderer: function (v, params, data) { return "汇总:" }
                },
                { id: 'Quantity', dataIndex: 'Quantity', header: '<label style="color:red;">数量</label>', width: 60,
                    editor: { xtype: 'numberfield', minValue: 1, decimalPrecision: 0 }, summaryType: 'sum', renderer: rowRender
                },
		        { id: 'Amount', dataIndex: 'Amount', header: '金额', width: 110, resizable: true, summaryType: 'sum',
		            summaryRenderer: function (v, params, data) { return filterValue(Math.round(parseFloat(v) * 100) / 100); }, renderer: rowRender
		        },
		        { id: 'ConfirmOutFactoryDate', dataIndex: 'ConfirmOutFactoryDate', header: '<label style="color:red;">出厂时间</label>', width: 100, editor: { xtype: 'datefield' }, renderer: ExtGridDateOnlyRender },
		        { id: 'ExpectedArrivalDate', dataIndex: 'ExpectedArrivalDate', header: '<label style="color:red;">到货时间</label>', width: 100, editor: { xtype: 'datefield' }, renderer: ExtGridDateOnlyRender }
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
                    hidden: pgOperation == 'r',
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">采购单产品详细信息：</label>', '-', {
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function () {
                            if ($("#SupplierId").val() == "") {
                                AimDlg.show("请先选择供应商！"); return;
                            }
                            if ($("#PriceType").val() == "") {
                                AimDlg.show("请选择该供应商的价格类型！"); return;
                            }
                            MultiAddData();
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
                                if (pgOperation == "c" || pgOperation == "cs") {
                                    $.each(recs, function () { store.remove(this); });
                                }
                                else {
                                    var dt = store.getModifiedDataStringArr(recs) || [];
                                    jQuery.ajaxExec("batchdelete", { "data": dt }, function (rtn) {
                                        $.each(recs, function () {
                                            store.remove(this);
                                        })
                                    });
                                }
                                getPrice();
                            }
                        }
                    }, '-', '<input id="excelfile" style="width:200px"></input>', { text: '导入Excel', iconCls: 'aim-icon-xls',
                        handler: function () {
                            if ($("#PriceType").val() == "") {
                                AimDlg.show("请选择该供应商的价格类型！");
                                return;
                            }
                            var UploadStyle = "dialogHeight:405px; dialogWidth:465px; help:0; resizable:0; status:0;scroll=0;";
                            var uploadurl = '../CommonPages/File/Upload.aspx';
                            var rtn = window.showModalDialog(uploadurl, window, UploadStyle);
                            if (rtn != undefined) {
                                $("#excelfile").val(rtn.substring(37, rtn.length - 1));
                                jQuery.ajaxExec('ImportExcel', { "path": rtn.substring(0, rtn.length - 1), "PriceType": $("#PriceType").val() }, function (rtn2) {
                                    if (rtn2.data.error) {
                                        alert(rtn2.data.error);
                                    }
                                    else {
                                        store.removeAll();
                                        var resultData = eval(rtn2.data.result);
                                        for (var i = 0; i < resultData.length; i++) {
                                            var recType = store.recordType;
                                            var p = new recType(resultData[i]);
                                            var rowIdx = store.data.length;
                                            store.insert(rowIdx, p);
                                        }
                                        getPrice();
                                    }
                                });
                            }
                        }
                    }]
                }),
                autoExpandColumn: 'Code',
                listeners: { "afteredit": function (val) {
                    if (val.field == "BuyPrice" || val.field == "Quantity") {
                        var result = accMul(val.record.get("BuyPrice"), val.record.get("Quantity"));
                        val.record.set("Amount", result);
                        getPrice();
                    }
                }
                }
            });

            window.onresize = function () {
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
        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return $("#Symbo").val() + (whole == "null" || whole == null ? "" : whole);
        }
        function getPrice() {
            var result = 0.00;
            for (var i = 0; i < store.getCount(); i++) {
                var rec = store.getAt(i);
                result = parseFloat(result) + parseFloat(rec.get("Amount"));
            }
            $("#PurchaseOrderAmount").val(result);
        }
        function rowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
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
            采购订单信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit" width="100%">
            <tr style="display: none">
                <td colspan="4">
                    <input id="Id" name="Id" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    采购编号
                </td>
                <td class="aim-ui-td-data">
                    <input id="PurchaseOrderNo" name="PurchaseOrderNo" class="validate[required]" />
                </td>
                <td class="aim-ui-td-caption">
                    供应商名称
                </td>
                <td class="aim-ui-td-data">
                    <input aimctrl='popup' id="SupplierName" name="SupplierName" relateid="txtuserid"
                        class="validate[required]" style="width: 200px;" popurl="/CommonPages/Select/SupplierSelect.aspx"
                        popparam="SupplierId:Id;SupplierName:SupplierName;MoneyType:MoneyType;Symbo:Symbo"
                        popstyle="width=800,height=550" />
                    <input id="SupplierId" name="SupplierId" type="hidden" />
                    <input id="Symbo" name="Symbo" type="hidden" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    产品类型
                </td>
                <td class="aim-ui-td-data">
                    <select id="ProductType" name="ProductType" aimctrl='select' style="width: 152px;"
                        class="validate[required]" enumdata="AimState['ProductType']">
                    </select>
                </td>
                <td class="aim-ui-td-caption">
                    价格类型
                </td>
                <td class="aim-ui-td-data">
                    <select id="PriceType" aimctrl='select' style="width: 155px;" enumdata="AimState['PriceType']"
                        name="PriceType" class="validate[required]">
                    </select>
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    金额合计
                </td>
                <td class="aim-ui-td-data">
                    <input id="PurchaseOrderAmount" name="PurchaseOrderAmount" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    订单号
                </td>
                <td class="aim-ui-td-data">
                    <input id="SupplierBillNo" name="SupplierBillNo" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    运货方式
                </td>
                <td class="aim-ui-td-data">
                    <input id="TransportationMode" name="TransportationMode" />
                </td>
                <td class="aim-ui-td-data">
                    订货日期
                </td>
                <td>
                    <input id="OrderDate" name="OrderDate" aimctrl="date" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    要求发货日期
                </td>
                <td class="aim-ui-td-data">
                    <input id="RequestDeliveryDate" name="RequestDeliveryDate" aimctrl="date" />
                </td>
                <td class="aim-ui-td-data">
                    交易币种
                </td>
                <td>
                    <input id="MoneyType" name="MoneyType" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    采购人
                </td>
                <td class="aim-ui-td-data">
                    <input id="CreateName" name="CreateName" readonly="readonly" style="background-color: ActiveBorder" />
                </td>
                <td class="aim-ui-td-data">
                </td>
                <td>
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
        <div id="StandardSub" align="left" style="width: 100%;">
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
