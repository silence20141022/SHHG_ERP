<%@ Page Title="销售出库" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmDeliveryOrderEdit3.aspx.cs" Inherits="Aim.Examining.Web.FrmDeliveryOrderEdit3" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var index = 0;
        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, grid, store;

        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }

        function onPgLoad() {
            setPgUI();

            if (getQueryString("type") == "qt") {
                $("#DeliveryType").val("其他出库");
            }
        }

        function setPgUI() {

            InitEditTable();

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });

            FormValidationBind('btnprint', SuccessSubmitPrint);
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            //隐藏提交按钮
            $("#btnSubmit").hide();

            ProcGridData();
            if ($("#Child").val() == "[]") {
                alert("未添加商品信息");
                $("#btnSubmit").show();
                return;
            }

            var recs = store.getRange();

            //验证仓库是否有要出的货
            //            var productIds = "";
            //            for (var i = 0; i < recs.length; i++) {
            //                productIds += recs[i].data.ProductId + ",";
            //            }
            //            if (productIds != "") {
            //                productIds = productIds.substring(0, productIds.length - 1);

            //                jQuery.ajaxExec('checkdata', { "productIds": productIds, "WarehouseId": $("#WarehouseId").val() }, function(rtns) {
            //                    if (rtns.data.error) {
            //                        alert(rtns.data.error);
            //                        $("#btnSubmit").show();
            //                    }
            //                    else {
            //                        var dt = store.getModifiedDataStringArr(recs) || [];
            //                        AimFrm.submit(pgAction, { "data": dt }, null, SubFinish);
            //                    }
            //                });
            //            }

            var dt = store.getModifiedDataStringArr(recs) || [];
            AimFrm.submit(pgAction, { "data": dt }, null, SubFinish);
        }

        //打印用到的保存
        function SuccessSubmitPrint() {
            //隐藏提交按钮
            $("#btnprint").hide();

            ProcGridData();
            if ($("#Child").val() == "[]") {
                alert("未添加商品信息");
                //隐藏提交按钮
                $("#btnprint").show();
                return;
            }

            var recs = store.getRange();
            var dt = store.getModifiedDataStringArr(recs) || [];

            var recs = store.getRange();

            //验证仓库是否有要出的货
            //            var productIds = "";
            //            for (var i = 0; i < recs.length; i++) {
            //                productIds += recs[i].data.ProductId + ",";
            //            }
            //            if (productIds != "") {
            //                productIds = productIds.substring(0, productIds.length - 1);
            //            }
            //            jQuery.ajaxExec('checkdata', { "productIds": productIds, "WarehouseId": $("#WarehouseId").val() }, function(rtns) {
            //                if (rtns.data.error) {
            //                    alert(rtns.data.error);
            //                    $("#btnprint").show();
            //                }
            //                else {
            //                    AimFrm.submit(pgAction, { "data": dt }, null, function(rtn) {
            //                        window.open('FrmDeliveryOrderPrint.aspx?Id=' + rtn.data.Id, 'deliveryprint', 'width=50,height=20,scrollbars=yes');
            //                        RefreshClose();
            //                    });
            //                }
            //            });

            AimFrm.submit(pgAction, { "data": dt }, null, function(rtn) {
                window.open('FrmDeliveryOrderPrint.aspx?Id=' + rtn.data.Id, 'deliveryprint', 'width=50,height=20,scrollbars=yes');
                RefreshClose();
            });

        }

        function SubFinish(args) {
            RefreshClose();
        }


        //选择商品
        function MultiAddPros() {
            var style = "dialogWidth:700px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/OrderProductSelect.aspx?seltype=multi&rtntype=array&paids=" + getQueryString("paids");
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                var count = store.getCount();
                var temp;
                var flag = false;
                for (var i = 0; i < users.length; i++) {
                    flag = false;
                    for (var j = 0; j < count; j++) {
                        temp = store.getAt(j);
                        if (temp.get("Id") == users[i].Id) {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true) {
                        continue;
                    }

                    var p = new EntRecord({ "OId": users[i].OId, "PId": users[i].Id, "ProductId": users[i].PId, "Isbn": users[i].Isbn, "Code": users[i].Code, "Name": users[i].Name, "Price": users[i].SalePrice, "OutCount": users[i].Count - users[i].OutCount, "NoOutCount": users[i].Count - users[i].OutCount, "Amount": users[i].SalePrice * (users[i].Count - users[i].OutCount), "Unit": users[i].Unit, "Remark": users[i].Remark });
                    insRowIdx = store.data.length;
                    store.insert(insRowIdx, p);
                    grid.startEditing(insRowIdx, 3);
                }
            });
        }

        function selGuid(rtn) {
            if (!rtn || !rtn.data)
                return;

            var rec = store.getAt(grid.activeEditor.row);
            var Guids = rtn.data || [];
            var temp = "";

            for (var i = 0; i < Guids.length; i++) {
                temp += Guids[i].GuId + ",";
            }
            if (temp.length > 0) {
                temp = temp.substring(0, temp.length - 1);
            }

            rec.set("Guids", temp);
            grid.activeEditor.setValue(temp);
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
            // 表格数据
            myData = {
                records: $.getJsonObj($("#Child").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                idProperty: 'Id',
                fields: [
                { name: 'Id' },
                { name: 'PId' },
                { name: 'ProductId' },
                { name: 'OId' },
			    { name: 'Isbn' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Price' },
			    { name: 'Guids' },
			    { name: 'Unit' },
			    { name: 'Amount' },
                { name: 'NoOuntCount' },
			    { name: 'OutCount' },
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
                    { id: 'Isbn', header: 'Isbn', dataIndex: 'Isbn', width: 80, resizable: true, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Code', header: '规格型号', renderer: ExtGridpperCase, dataIndex: 'Code', width: 120, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'Name', header: '商品名称', dataIndex: 'Name', width: 150, resizable: true },

                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60, resizable: true },

                //{ id: 'Price', header: '售价', dataIndex: 'Price', width: 80, resizable: true, renderer: filterValue },

                    {id: 'NoOutCount', header: '待出库数量', dataIndex: 'NoOutCount', summaryType: 'sum', width: 80, resizable: true },
                    { id: 'OutCount', header: '<label style="color:red;">本次出库数量</label>', dataIndex: 'OutCount', summaryType: 'sum', width: 80, resizable: true, editor: new Ext.form.NumberField({ allowBlank: false }) },

                //{ id: 'Amount', dataIndex: 'Amount', header: '总金额', summaryType: 'sum', width: 80, renderer: filterValue },

                //{ id: 'Guids', dataIndex: 'Guids', header: '<label style="color:red;">唯一编号</label>', width: 130, editor: { xtype: 'aimproguidselector', popAfter: selGuid} },

                    {id: 'Remark', header: '<label style="color:red;">备注</label>', dataIndex: 'Remark', width: 120, resizable: true, editor: new Ext.form.TextArea({}) }
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
                    //hidden: !getQueryString("type") || getQueryString("type") != "qt",
                    items: ['<label style="color:red;">待出库商品信息：</label>'
                    , {
                        text: '添加',
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
                                })
                            }
                        }
                    }, {
                        text: '清空',
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            if (confirm("确定清空所有记录？")) {
                                store.removeAll();
                            }
                        }
                    }
]
                }),
                autoExpandColumn: 'Remark',
                listeners: { "afteredit": function(val) {
                    if (val.field == "OutCount") {
                        if (val.record.get("OutCount") > val.record.get("NoOutCount")) {
                            alert("出库数量不能超过购买量");
                            val.record.set("OutCount", val.record.get("NoOutCount"));
                        }
                        val.record.set("Amount", val.record.get("OutCount") * val.record.get("Price"));
                    }
                }
                }
            });

            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }

        //导入条形码(单)
        function getProByIsbn(isbn) {
            if (isbn) {

                var count = store.getCount();
                var result = 0.0;
                var temp;
                for (var i = 0; i < count; i++) {
                    temp = store.getAt(i);
                    if (temp.get("Isbn") == isbn) {
                        if (!temp.get("OutCount") || temp.get("OutCount") < temp.get("Count")) {
                            temp.set("OutCount", temp.get("OutCount") + 1);
                        }
                        else {
                            alert("出库数量不能超过购买量");
                        }
                    }
                }
                $("#txtisbn").val("");
            }
        }

        function getSalesman(val) {
            index++;
            if (val && index > 1) {
                jQuery.ajaxExec("getSalesman", { "CId": val }, function(rtn) {
                    $("#SalesmanId").val(rtn.data.MagId);
                    $("#Salesman").dataBind(rtn.data.result);
                    $("#Address").val(rtn.data.Address);
                    $("#CCode").val(rtn.data.Code);
                    $("#Tel").val(rtn.data.Tel);
                });
            }
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            销售出库</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="CorrespondState" type="hidden" name="CorrespondState" />
                        <input id="CorrespondInvoice" type="hidden" name="CorrespondInvoice" />
                        <input id="TotalMoney" type="hidden" name="TotalMoney" />
                        <input id="TotalMoneyHis" type="hidden" name="TotalMoneyHis" />
                        <input id="PId" name="PId" />
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        发货单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Number" name="Number" disabled="disabled" value="自动生成" />
                    </td>
                    <td class="aim-ui-td-caption">
                        客户名称
                    </td>
                    <td class="aim-ui-td-data" width="120px">
                        <%--<input id="CName" disabled="disabled" class="validate[required]" name="CName" />--%>
                        <input aimctrl='customer' required="true" id="CName" name="CName" relateid="CId" />
                        <input type="hidden" name="CId" id="CId" onpropertychange="getSalesman(this.value);" />
                    </td>
                    <td class="aim-ui-td-caption">
                        客户编号
                    </td>
                    <td class="aim-ui-td-data">
                        <%--<input aimctrl='popup' id="CCode" class="validate[required]" name="CCode" disabled="disabled"
                            relateid="CId" style="width: 120px;" popurl="/CommonPages/Select/CustomerSelect.aspx"
                            popparam="CId:Id;CName:Name;CCode:Code;PreDeposit:PreDeposit" popstyle="width=700,height=500" />--%>
                        <input id="CCode" disabled="disabled" name="CCode" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        联系电话
                    </td>
                    <td class="aim-ui-td-data" width="120px">
                        <input id="Tel" name="Tel" />
                    </td>
                    <td class="aim-ui-td-caption">
                        发货仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="WarehouseName" name="WarehouseName" disabled="disabled"
                            style="width: 158px;" relateid="txtuserid" popurl="/CommonPages/Select/WarehouseSelect.aspx?seltype=multi"
                            popparam="WarehouseId:Id;WarehouseName:Name" popstyle="width=700,height=500"
                            class="validate[required]" />
                        <input type="hidden" id="WarehouseId" name="WarehouseId" />
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
                        出库类型
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="DeliveryType" disabled="disabled" name="DeliveryType" value="正常出库" />
                    </td>
                    <td class="aim-ui-td-caption">
                        业务员
                    </td>
                    <td class="aim-ui-td-data">
                        <%--<input aimctrl='popup' id="Salesman" name="Salesman" style="width: 120px;" disabled="disabled"
                            popurl="/CommonPages/Select/UsrSelect/MUsrSelect.aspx?seltype=single" popparam="Salesman:Name"
                            popstyle="width=700,height=500" />--%>
                        <input aimctrl='user' required="true" id="Salesman" name="Salesman" relateid="SalesmanId" />
                        <input type="hidden" id="SalesmanId" name="SalesmanId" />
                    </td>
                    <td class="aim-ui-td-caption">
                        地址
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Address" name="Address" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        提货方式
                    </td>
                    <td class="aim-ui-td-data" colspan="5">
                        <select id="DeliveryMode" aimctrl='select' style="width: 152px;" enumdata="AimState['DeliveryMode']"
                            name="DeliveryMode">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="5">
                        <textarea id="Remark" name="Remark" cols="60" rows="5"></textarea>
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
                    <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnprint" class="aim-ui-button submit">
                        打印</a> <a id="btnCancel" class="aim-ui-button cancel">取消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
