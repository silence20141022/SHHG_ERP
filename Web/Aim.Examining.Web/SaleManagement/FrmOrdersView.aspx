<%@ Page Title="销售单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmOrdersView.aspx.cs" Inherits="Aim.Examining.Web.FrmOrdersView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var index = 0;
        var mdls, tabPanel, grid, store;

        function onPgLoad() {
            setPgUI();
            getPrice();
            $("#btninsertexcel").val("导入Excel");
        }

        function setPgUI() {
            InitEditTable();
            if (pgOperation == "r") {
                $("#tdexcel").css("display", "none");
            }

            //绑定导入excel数据
            $('#btninsertexcel').click(function() {
                if ($("#excel").val().length == 0) {
                    alert("请先上传Excel文件");
                    return;
                }

                var recs = grid.store.getRange();
                var subdata = [];
                $.each(recs, function() {
                    subdata.push(this.data);
                });

                jQuery.ajaxExec('inputexcel', { "path": $("#excel").val().substring(0, $("#excel").val().length - 1), "json": $.getJsonString(subdata) }, function(rtn) {
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
        }

        //验证成功执行保存方法
        function SuccessSubmit() {

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
            ProcGridData();
            AimFrm.submit(pgAction, { "PAState": msg != "" && url != "" ? "Yes" : "No" }, null, function(rtn) {
                //跳转
                //if (msg != "" && url != "") {
                //    alert(msg);
                //    //window.location = url + "&oid=" + rtn.data.OId;
                //}
                RefreshClose();
            });

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
                    var p = new EntRecord({ "Id": users[i].Id, "Isbn": users[i].Isbn, "Code": users[i].Code, "Name": users[i].Name, "MinSalePrice": users[i].MinSalePrice, "Price": users[i].SalePrice, "Count": 1, "Amount": users[i].SalePrice, "Unit": users[i].Unit });
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
			    { name: 'MinSalePrice' },
			    { name: 'Price' },
			    { name: 'Unit' },
			    { name: 'Amount' },
			    { name: 'Count' },
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
                    { id: 'Name', header: '商品名称', dataIndex: 'Name', width: 150, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'Code', header: '规格型号', renderer: ExtGridpperCase, dataIndex: 'Code', width: 150, resizable: true },

                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 100, resizable: true },
                    { id: 'MinSalePrice', header: '最低售价', dataIndex: 'MinSalePrice', width: 100, resizable: true, hidden: true },

                    { id: 'Price', header: '售价', dataIndex: 'Price', width: 100, renderer: filterValue, resizable: true },
                    { id: 'Count', header: '购买量', dataIndex: 'Count', width: 100, resizable: true, summaryType: 'sum' },

                    { id: 'Amount', dataIndex: 'Amount', header: '总金额', summaryType: 'sum', width: 100, renderer: filterValue },
                    { id: 'Remark', header: '备注', dataIndex: 'Remark', width: 100, resizable: true }
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
                    items: ['<label style="color:red;">子商品信息：</label>']
                    }),
                    autoExpandColumn: 'Remark',
                    listeners: { "afteredit": function(val) {
                        if (val.field == "Price" || val.field == "Count") {
                            val.record.set("Amount", (val.record.get("Price")) * (val.record.get("Count")));
                            getPrice();
                        }
                        else if (val.field == "Guids" || val.field == "Remark") {
                            //val.record.set(val.field, val.record.get(val.field).replace("\r\n", ","));
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
                    result += temp.get("Price") * temp.get("Count");
                }
                if (result - $("#PreDeposit").val() > 0) {
                    $("#TotalMoney").val(result - $("#PreDeposit").val());
                }
                else {
                    $("#TotalMoney").val(0);
                }
                $("#TotalMoneyHis").val(result);
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
                    jQuery.ajaxExec("getSalesman", { "CId": val }, function(rtn) {
                        $("#CCode").val(rtn.data.Code);
                    });
                }
            }
    </script>

    <script language="javascript">
        /**********************************************WorkFlow Function Start**************************/
        var permission = {};
        //这里依次统一添加各环节的控件权限
        permission.初审 = { ReadOnly: "", Hidden: "" };
        permission.复审 = { ReadOnly: "", Hidden: "" };

        var StartUserId = "";
        var StartUserName = "";
        function InitUIForFlow() {

            StartUserId = $("#RequestUserId").val();
            StartUserName = $("#RequestUserName").val();
            if (window.parent.AimState["Task"])
                var taskName = window.parent.AimState["Task"].ApprovalNodeName;

            $("#btnSubmit").hide();
            $("#btnCancel").hide();

            ///控制下一步路由
            if (taskName == "确认发布内容") {
                //SetRoute("公司领导",true);//第一个参数为下一步路由,第二个参数为是否禁止重新选择路由
            }

            if (eval("permission." + taskName)) {
                //只读
                var read = eval("permission." + taskName).ReadOnly;
                for (var i = 0; i < read.split(',').length; i++) {
                    var id = read.split(',')[i];
                    if (document.getElementById(id))
                        document.getElementById(id).readOnly = true;
                }
                //隐藏
                var vis = eval("permission." + taskName).Hidden;
                for (var i = 0; i < vis.split(',').length; i++) {
                    var id = vis.split(',')[i];
                    if (document.getElementById(id))
                        document.getElementById(id).style.display = "none";
                }
            }
        }
        //保存流程和提交流程时触发
        function onSave(task) {

        }
        //提交流程时触发
        function onSubmit(task) {

        }
        //获取下一环节用户
        function onGiveUsers(nextName) {
            var users = { UserIds: "", UserNames: "" };
            switch (nextName) {
                case "提交审批":
                    //users.UserIds = $("#PostUserId").val();
                    //users.UserNames = $("#PostUserName").val();
                    break;
            }
            return users;
        }
        //流程结束时触发
        function onFinish(task) {
            //更新任务状态
            jQuery.ajaxExec('submitfinish', { "state": "End", "id": id, "ApprovalState": window.parent.document.getElementById("id_SubmitState").value }, function() {
                RefreshClose();
            });
            //AimFrm.submit(pgAction, { param: "finish" }, null, function() { });
        }
        //第一个参数为下一步路由,第二个参数为是否禁止重新选择路由
        function SetRoute(name, flag) {
            window.parent.SetRoute("公司领导", flag);
        }
        /*****************************************************WorkFlow Function End****************************/
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            销售单</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
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
                        <input id="Number" name="Number" disabled="disabled" />
                    </td>
                    <td class="aim-ui-td-caption">
                        客户名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CName" name="CName" disabled="disabled"  />
                    </td>
                    <td class="aim-ui-td-caption">
                        开票类型
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InvoiceType" name="InvoiceType" disabled="disabled"  />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        结算方式
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CalculateManner" name="CalculateManner" disabled="disabled"  />
                    </td>
                    <td class="aim-ui-td-caption">
                        支付方式
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PayType" name="PayType" disabled="disabled"  />
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
                        要求到货时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ExpectedTime" disabled="disabled" name="ExpectedTime" />
                    </td>
                    <td class="aim-ui-td-caption">
                        提货方式
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <input id="DeliveryMode" disabled="disabled" name="DeliveryMode" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        审批原因
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Reason" name="Reason" style="width: 100%" rows="3" disabled="disabled"></textarea>
                    </td>
                    <td>
                        预计收款日期
                    </td>
                    <td>
                        <input id="ReceivablesDate" disabled="disabled" name="ReceivablesDate" />
                    </td>
                    <%--<td class="aim-ui-td-caption">
                        发货仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="WarehouseName" name="WarehouseName" disabled="disabled"
                            style="width: 120px;" relateid="txtuserid" popurl="/CommonPages/Select/WarehouseSelect.aspx"
                            popparam="WarehouseId:Id;WarehouseName:Name" popstyle="width=700,height=500"
                            class="validate[required]" />
                        <input type="hidden" id="WarehouseId" name="WarehouseId" />
                    </td>--%>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 100%" rows="5" disabled="disabled"></textarea>
                    </td>
                    <td colspan="2" id="tdexcel" style="vertical-align: bottom; display: none;">
                        <input id="excel" aimctrl='file' mode="single" filter="Excel文件 (*.xls;*.xlsx)|*.xls;*.xlsx"
                            name="excel" style="width: 220px;" />
                        <a id="btninsertexcel" class="aim-ui-button submit" style="margin-bottom: 5px; height: 20px;">
                            导入Excel</a>
                    </td>
                </tr>
        </table>
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
