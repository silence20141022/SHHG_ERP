<%@ Page Title="销售开票" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmOrdersInvoice.aspx.cs" Inherits="Aim.Examining.Web.FrmOrdersInvoice" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var mdls, tabPanel, grid, store;

        function onPgLoad() {
            setPgUI();
            getPrice();
        }

        function setPgUI() {
            InitEditTable();

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            ProcGridData();

            if ($("#Child").val() == "[]") {
                alert("请选择要开票的商品");
                return;
            }

            var recs = store.getRange();
            var dt = store.getModifiedDataStringArr(recs) || [];
            //alert(dt); return;
            AimFrm.submit(pgAction, { "data": dt }, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }


        //选择商品
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
                getPrice();
            });
        }

        // 处理网格数据
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function() {
                subdata.push(this.data);
            });
            var jsonstr = $.getJsonString(subdata);

            $("#Child").val(jsonstr);
        }

        function InitEditTable() {
            var temp = (AimState["DetailList"] || []).length == 0 ? ($.getJsonObj($("#Child").val()) || []) : (AimState["DetailList"] || []);

            // 表格数据
            myData = {
                records: temp
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
                { name: 'Id' },
                { name: 'OId' },
                { name: 'ProductId' },
			    { name: 'ProductCode' },
			    { name: 'ProductName' },
			    { name: 'SalePrice' },
			    { name: 'Unit' },
			    { name: 'Amount' },
			    { name: 'Count' },
			    { name: 'BcCount' },
			    { name: 'Type' },
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
                    { id: 'OId', header: 'OId', dataIndex: 'OId', width: 80, resizable: true, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'ProductCode', header: '规格型号', renderer: ExtGridpperCase, dataIndex: 'ProductCode', width: 150, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'ProductName', header: '商品名称', dataIndex: 'ProductName', width: 150, resizable: true },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 100, resizable: true },
                    { id: 'SalePrice', header: '售价', dataIndex: 'SalePrice', width: 100, renderer: filterValue, resizable: true },

                    { id: 'Count', header: '总数量', dataIndex: 'Count', width: 100, renderer: filterValue, resizable: true },

                    { id: 'BcCount', header: '<label style="color:red;">本次开票数量</label>', dataIndex: 'BcCount', width: 100, resizable: true, allowBlank: false, editor: new Ext.form.NumberField({ allowBlank: false }) },
                    { id: 'BillingCount', header: '已开票数量', dataIndex: 'BillingCount', width: 100, resizable: true, summaryType: 'sum', allowBlank: false },
                    { id: 'Amount', dataIndex: 'Amount', header: '总金额', summaryType: 'sum', width: 100, renderer: filterValue },
                    { id: 'Remark', header: '<label style="color:red;">备注</label>', dataIndex: 'Remark', width: 100, resizable: true, editor: new Ext.form.TextArea({}) }
                ]
            });

            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                //width: 633,
                width: Ext.get("StandardSub").getWidth(),
                height: 300,
                forceLayout: true,
                columnLines: true,
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">商品信息：</label>', '-', {
                        text: '添加',
                        hidden: true,
                        iconCls: 'aim-icon-add',
                        handler: function() {
                            MultiAddPros();
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
                                $.each(recs, function() {
                                    store.remove(this);
                                    getPrice();
                                });
                            }
                        }
                    }, {
                        text: '清空',
                        hidden: true,
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            if (confirm("确定清空所有记录？")) {
                                store.removeAll();
                                getPrice();
                            }
                        }
}]
                    }),
                    autoExpandColumn: 'Remark',
                    listeners: { "afteredit": function(val) {
                        if (val.field == "BcCount") {
                            if (val.record.get("BcCount") > val.record.get("Count") - val.record.get("BillingCount")) {
                                val.record.set("BcCount", val.record.get("Count") - val.record.get("BillingCount"))
                            }
                            val.record.set("Amount", (val.record.get("SalePrice")) * (val.record.get("BcCount")));
                            getPrice();
                        }
                    }
                    }
                });

                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                };

                var storecount = store.getCount();
                for (var i = 0; i < storecount; i++) {
                    temp = store.getAt(i);
                    if (!temp.get("BcCount")) {
                        temp.data.BcCount = temp.data.Count - temp.data.BillingCount;
                    }
                }
            }

            function getPrice() {
                var count = store.getCount();
                var result = 0.0;
                var temp;
                for (var i = 0; i < count; i++) {
                    temp = store.getAt(i);
                    result += temp.get("SalePrice") * temp.get("BcCount");
                }

                if (parseFloat($("#DiscountAmount").val())) {
                    result = result + parseFloat($("#DiscountAmount").val());
                }

                $("#Amount").val(result);

                //if (pgOperation != "r") {
                //    $("#DiscountAmount").val(result);
                //}
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
            销售开票</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tr style="display: none">
                <td>
                    <input id="Id" name="Id" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    发票号
                </td>
                <td class="aim-ui-td-data">
                    <input id="Number" name="Number" class="validate[required]" />
                </td>
                <td class="aim-ui-td-caption">
                    客户编号
                </td>
                <td class="aim-ui-td-data">
                    <input aimctrl='popup' id="CCode" class="validate[required]" name="CCode" disabled="disabled"
                        relateid="CId" style="width: 120px;" popurl="/CommonPages/Select/CustomerSelect.aspx"
                        popparam="CId:Id;CName:Name;CCode:Code;PreDeposit:PreDeposit" popstyle="width=700,height=500" />
                    <input type="hidden" name="CId" id="CId" />
                </td>
                <td class="aim-ui-td-caption">
                    客户名称
                </td>
                <td class="aim-ui-td-data">
                    <input id="CName" disabled="disabled" class="validate[required]" name="CName" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    发票金额
                </td>
                <td class="aim-ui-td-data">
                    <input id="Amount" disabled="disabled" name="Amount" />元
                </td>
                <td class="aim-ui-td-caption">
                    折扣金额
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <input id="DiscountAmount" name="DiscountAmount" onblur="getPrice()" />元
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    备注
                </td>
                <td class="aim-ui-td-data" colspan="5">
                    <textarea id="Remark" name="Remark" cols="58"></textarea>
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
