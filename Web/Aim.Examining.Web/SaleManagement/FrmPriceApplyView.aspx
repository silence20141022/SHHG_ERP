<%@ Page Title="销售价格申请单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmPriceApplyView.aspx.cs" Inherits="Aim.Examining.Web.FrmPriceApplyView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var index = 0;
        var mdls, tabPanel, subContentPanel;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            InitEditTable();
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            ProcGridData();
            AimFrm.submit(pgAction, {}, null, SubFinish);
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
			    { name: 'Amount' },
			    { name: 'Unit' },
			    { name: 'Count' },
			    { name: 'Remark' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                {
                    id: 'Id',
                    header: 'Id',
                    dataIndex: 'Id',
                    width: 80,
                    resizable: true,
                    hidden: true
                },
                {
                    id: 'Isbn',
                    header: 'Isbn',
                    dataIndex: 'Isbn',
                    width: 80,
                    resizable: true,
                    hidden: true
                },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),

            {
                id: 'Code',
                header: '规格型号',
                renderer: ExtGridpperCase,
                dataIndex: 'Code',
                width: 120,
                resizable: true,
                summaryRenderer: function(v, params, data) { return "汇总:" }
            },
            {
                id: 'Name',
                header: '商品名称',
                dataIndex: 'Name',
                width: 150,
                resizable: true
            },
            {
                id: 'Unit',
                header: '单位',
                dataIndex: 'Unit',
                width: 100,
                resizable: true
            },
            {
                id: 'MinSalePrice',
                header: '最低售价',
                dataIndex: 'MinSalePrice',
                width: 100,
                resizable: true
            },
            {
                id: 'Price',
                header: '预计售价',
                dataIndex: 'Price',
                width: 100,
                renderer: filterValue,
                resizable: true
            }, {
                id: 'zhekoulv', header: '折扣率', dataIndex: '', width: 80, renderer: function(v, p, r) {
                    if (r.get("Price") && r.get("MinSalePrice")) {
                        return Math.round(r.get("Price") / r.get("MinSalePrice") * 100 * 100) / 100 + "%";
                    }
                }
            }, {
                id: 'Count',
                header: '预计销售量',
                dataIndex: 'Count',
                summaryType: 'sum',
                width: 100,
                resizable: true,
                allowBlank: false
            },
            { id: 'Amount', dataIndex: 'Amount', header: '总金额', summaryType: 'sum', width: 100, renderer: filterValue },

            {
                id: 'Remark',
                header: '备注',
                dataIndex: 'Remark',
                width: 100,
                resizable: true
            }
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
                viewConfig: {
                    forceFit: true
                },
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    items: ['<label style="color:red;">子商品信息：</label>']
                }),
                autoExpandColumn: 'Remark',
                listeners: { "afteredit": function(val) {
                    if (val.field == "Price" || val.field == "Count") {
                        val.record.set("Amount", (val.record.get("Price")) * (val.record.get("Count")))
                    }
                }
                }
            });

            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
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

        function getSalesman(val) {
            index++;
            if (val && index > 1) {
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
            //SuccessSubmit();
            //AimFrm.submit(pgAction, { param: "test" }, null, function() { });
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
            价格申请单</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption" width="60px">
                        单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Number" name="Number" disabled="disabled" />
                    </td>
                    <td class="aim-ui-td-caption" width="60px">
                        客户名称
                    </td>
                    <td class="aim-ui-td-data" width="120px">
                        <input id="CName" name="CName" disabled="disabled" />
                    </td>
                    <td class="aim-ui-td-caption" width="60px">
                        客户编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CCode" name="CCode" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        申请原因
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Reason" name="Reason" style="width: 100%" disabled="disabled"></textarea>
                    </td>
                    <td class="aim-ui-td-caption">
                        预计销售时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ExpectedTime" name="ExpectedTime" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 100%" disabled="disabled"></textarea>
                    </td>
                    <td class="aim-ui-td-caption">
                    </td>
                    <td class="aim-ui-td-data">
                    </td>
                </tr>
        </table>
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
