<%@ Page Title="退货单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmReturnOrderEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmReturnOrderEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        function init() {
            if (getQueryString("operate")) {
                var op = "同意";
                if (getQueryString("operate") == "no") {
                    op = "不同意";
                }
                $("#btncheck").html(op);
                $("#btncheck").css("display", "inline");

                $("#btnSubmit").css("display", "none");

                $("#btncheck").click(function() {
                    jQuery.ajaxExec('docheck', { "remark": $("#Remark").val(), "id": getQueryString("id"), "operate": getQueryString("operate") }, SubFinish);
                });
            }
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

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, grid, store;

        function onPgLoad() {
            setPgUI();
            init();
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

            //隐藏提交按钮
            $("#btnSubmit").hide();

            ProcGridData();
            if ($("#Child").val() == "[]") {
                alert("请填写需要退货的商品");
                return;
            }
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }


        //选择商品
        function MultiAddPros() {
            var style = "dialogWidth:700px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/ProductSelect.aspx?seltype=multi&rtntype=array&PId=" + $("#Id").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ "Id": users[i].Id, "Isbn": users[i].Isbn, "Code": users[i].Code, "Name": users[i].Name, "ReturnPrice": users[i].SalePrice, "Price": users[i].SalePrice, "Count": 1, "Amount": users[i].SalePrice, "Unit": users[i].Unit });
                    insRowIdx = store.data.length;
                    store.insert(insRowIdx, p);
                    //var rec = store.getAt(insRowIdx);
                    grid.startEditing(insRowIdx, 3);
                }
                getPrice();
            });
        }

        //根据订单选择
        function MultiAddProsByONumber(onumber) {
            var style = "dialogWidth:550px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/FrmRCProductSelect.aspx?seltype=multi&rtntype=array&onumber=" + onumber;
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ "Id": users[i].Id, "Isbn": users[i].Isbn, "Code": users[i].Code, "Name": users[i].Name, "ReturnPrice": users[i].SalePrice, "Price": users[i].SalePrice, "Count": 1, "Amount": users[i].SalePrice, "Unit": users[i].Unit, "Guid": users[i].Guids.indexOf(',') > -1 ? users[i].Guids.substring(0, users[i].Guids.indexOf(',')) : users[i].Guids });
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

        function selGuid(rtn) {
            if (!rtn || !rtn.data)
                return;

            var rec = store.getAt(grid.activeEditor.row);
            rec.set("Guid", rtn.data.GuId || rtn.data[0].GuId);
            grid.activeEditor.setValue(rtn.data.GuId || rtn.data[0].GuId);

        }

        function InitEditTable() {
            // 表格数据
            myData = {
                records: $.getJsonObj($("#Child").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
                { name: 'Id' },
			    { name: 'Isbn' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'ReturnPrice' },
			    { name: 'Price' },
			    { name: 'Unit' },
			    { name: 'Guid' },
			    { name: 'ProState' },
			    { name: 'Amount' },
			    { name: 'Count' },
			    { name: 'Remark' }
			    ]
            });

            //商品状态下拉框
            var becombo = new Ext.ux.form.AimComboBox({
                enumdata: AimState["ProState"],
                lazyRender: false,
                allowBlank: false,
                triggerAction: 'all',
                mode: 'local',
                listeners: {
                    collapse: function(obj) {
                        if (grid.activeEditor) {
                            var rec = store.getAt(grid.activeEditor.row);
                            if (rec) {
                                rec.set("ProState", obj.getValue());
                            }
                        }
                    }
                }
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
                    { id: 'Code', header: '规格型号', renderer: ExtGridpperCase, dataIndex: 'Code', width: 150, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'Name', header: '商品名称', dataIndex: 'Name', width: 120, resizable: true },

                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60, resizable: true },

                    { id: 'Price', header: '售价', dataIndex: 'Price', width: 100, resizable: true, renderer: filterValue },
                    { id: 'ReturnPrice', header: '<label style="color:red;">退货价</label>', dataIndex: 'ReturnPrice', renderer: filterValue, width: 100, resizable: true, editor: new Ext.form.NumberField({ allowBlank: false }) },

                    { id: 'Count', header: '<label style="color:red;">退货数量</label>', dataIndex: 'Count', summaryType: 'sum', width: 100, resizable: true, allowBlank: false, editor: new Ext.form.NumberField({ allowBlank: false }) },

                //{ id: 'Guid', dataIndex: 'Guid', header: '<label style="color:red;">唯一编号</label>', width: 130, editor: new Ext.form.TextField({}) }, //{ xtype: 'aimproguidselector', popAfter: selGuid}
                    {id: 'ProState', dataIndex: 'ProState', header: '<label style="color:red;">状态</label>', width: 100, editor: becombo },

                    { id: 'Amount', dataIndex: 'Amount', header: '退货金额', summaryType: 'sum', width: 100, renderer: function(val) {
                        return filterValue(Math.round(val * 100) / 100);
                    }
                    },
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
                viewConfig: {
                    forceFit: true
                },
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">需要退货的商品：</label>'
                    , '-', {
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function() {
                            if ($("#OrderNumber").val()) {
                                MultiAddProsByONumber($("#OrderNumber").val());
                            }
                            else {
                                MultiAddPros();
                            }
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
                                })
                            }
                        }
                    }, {
                        text: '清空',
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            if (confirm("确定清空所有记录？")) {
                                store.removeAll();
                                getPrice();
                            }
                        }
                    }
]
                }),
                autoExpandColumn: 'Remark',
                listeners: { "afteredit": function(val) {
                    if (val.field == "ReturnPrice" || val.field == "Count") {
                        val.record.set("Amount", (val.record.get("ReturnPrice")) * (val.record.get("Count")));
                        getPrice();
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
                result += temp.get("ReturnPrice") * temp.get("Count");
            }
            if (result) {
                $("#ReturnMoney").val(result);
            }
            else {
                $("#ReturnMoney").val(0);
            }
        }

        //此方法可改进
        function getProByIsbn(isbn) {
            if (isbn) {
                jQuery.ajaxExec('getProByIsbn', { "isbn": isbn }, function(rtn) {
                    if (rtn.data.state == "success") {
                        var insRowIdx = store.data.length;
                        var EntRecord = grid.getStore().recordType;
                        grid.stopEditing();

                        var pro = rtn.data;
                        var p = new EntRecord({ "Id": pro.Id, "Isbn": pro.Isbn, "Code": pro.Code, "Name": pro.Name, "ReturnPrice": pro.SalePrice, "Price": pro.SalePrice, "Count": 1, "Amount": pro.SalePrice, "Unit": pro.Unit });
                        store.insert(insRowIdx, p);
                        grid.startEditing(insRowIdx, 3);

                        getPrice();
                    }
                    $("#txtisbn").val("");
                });
            }
        }

        var index = 0;
        function getSalesman(val) {
            index++;
            var op = getQueryString("op");
            if ((op == "c" && val && index > 1) || (op == "u" && val && index > 2)) {
                jQuery.ajaxExec("getSalesman", { "CId": val }, function(rtn) {
                    $("#CCode").val(rtn.data.Code);
                });
            }
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            退货单</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="PId" name="PId" />
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        退货单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Number" name="Number" disabled="disabled" value="自动生成" />
                    </td>
                    <td class="aim-ui-td-caption">
                        订单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="OrderNumber" class="validate[required]" name="OrderNumber" />
                    </td>
                    <%--<td class="aim-ui-td-caption">
                        客户名称
                    </td>
                    <td class="aim-ui-td-data" width="120px">
                        <input aimctrl='customer' required="true" id="CName" name="CName" relateid="CId" />
                        <input type="hidden" name="CId" id="CId" onpropertychange="getSalesman(this.value);" />
                    </td>--%>
                </tr>
                <tr>
                    <%--<td class="aim-ui-td-caption">
                        客户编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CCode" name="CCode" disabled="disabled" />
                    </td>--%>
                    <td class="aim-ui-td-caption">
                        退款金额
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ReturnMoney" disabled="disabled" name="ReturnMoney" />
                    </td>
                    <td class="aim-ui-td-caption">
                        收货仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="InWarehouseName" name="InWarehouseName" disabled="disabled"
                            style="width: 158px;" relateid="txtuserid" popurl="/CommonPages/Select/WarehouseSelect.aspx?seltype=multi"
                            popparam="InWarehouseId:Id;InWarehouseName:Name" popstyle="width=700,height=500"
                            class="validate[required]" />
                        <input type="hidden" id="InWarehouseId" name="InWarehouseId" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" cols="65"></textarea>
                    </td>
                    <%--<td class="aim-ui-td-caption">
                        条形码
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="txtisbn" type="text" onkeydown="if(event.keyCode==13) getProByIsbn(this.value);">
                    </td>--%>
                </tr>
        </table>
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-button-panel">
                    <a id="btncheck" class="aim-ui-button submit" style="display: none;">审核</a> <a id="btnSubmit"
                        class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">取消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
