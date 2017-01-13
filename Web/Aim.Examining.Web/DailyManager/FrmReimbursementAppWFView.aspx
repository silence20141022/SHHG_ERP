<%@ Page Title="出差报销申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmReimbursementAppWFView.aspx.cs" Inherits="Aim.Examining.Web.FrmReimbursementAppWFView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, subContentPanel, sngrid;

        function onPgLoad() {

            setPgUI();

            if (!$("#LeaveTime").val()) {
                $("#LeaveTime").val(new Date().format("Y-m-d"));
                $("#LeaveUser").val(AimState.UserInfo.Name);
                $("#DeptName").val(AimState["ReleDepartment"]);
            }
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
			    { name: 'Pro' },

			    { name: 'chailv' },
			    { name: 'zhusu' },
			    { name: 'canfei' },
			    { name: 'other' },

			    { name: 'begindate' },
			    { name: 'enddate' },
			    { name: 'Amount' },
			    { name: 'Remark' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Pro', header: '项目', dataIndex: 'Pro', width: 120, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'begindate', header: '开始时间', dataIndex: 'begindate', width: 80, resizable: true, renderer: ExtGridDateOnlyRender },
                    { id: 'enddate', header: '结束时间', dataIndex: 'enddate', width: 80, resizable: true, renderer: ExtGridDateOnlyRender },

                    { id: 'chailv', header: '差旅费', dataIndex: 'chailv', summaryType: 'sum', width: 80, resizable: true },
                    { id: 'zhusu', header: '住宿费', dataIndex: 'zhusu', summaryType: 'sum', width: 80, resizable: true },
                    { id: 'canfei', header: '餐费', dataIndex: 'canfei', summaryType: 'sum', width: 80, resizable: true },
                    { id: 'other', header: '其他', dataIndex: 'other', summaryType: 'sum', width: 80, resizable: true },

                    { id: 'Amount', header: '合计', dataIndex: 'Amount', width: 80, summaryType: 'sum', resizable: true },
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
                    items: ['<label style="color:red;">出差项：</label>']
                    }),
                    autoExpandColumn: 'Remark',
                    listeners: { "afteredit": function(val) {
                        if (val.field == "chailv" || val.field == "zhusu" || val.field == "canfei" || val.field == "other") {

                            val.record.set("Amount", tryParseFloat(val.record.get("chailv")) + tryParseFloat(val.record.get("zhusu")) + tryParseFloat(val.record.get("canfei")) + tryParseFloat(val.record.get("other")));
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
                    if (temp.get("Amount")) {
                        result += temp.get("Amount");
                    }
                }
                $("#Amount").val(result);
            }

            function tryParseFloat(val) {
                if (val) {
                    return parseFloat(val);
                }
                else {
                    return 0;
                }
            }
    </script>

    <script language="javascript">
        /**********************************************WorkFlow Function Start**************************/
        var permission = {};
        //这里依次统一添加各环节的控件权限
        permission.初审 = { ReadOnly: "", Hidden: "BeginTime" };
        permission.复审 = { ReadOnly: "", Hidden: "EndTime" };

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
            SuccessSubmit();
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
            出差报销申请</h1>
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
                    <td class="aim-ui-td-caption">
                        编号
                    </td>
                    <td class="aim-ui-td-data">
                    <input id="Number" name="Number" disabled="disabled" />
                    </td>
                    <td class="aim-ui-td-caption">
                        申请人
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled="disabled" id="LeaveUser" name="LeaveUser" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        部门
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled="disabled" id="DeptName" name="DeptName" />
                    </td>
                    <td class="aim-ui-td-caption">
                        申请日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="LeaveTime" name="LeaveTime" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        报销金额
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <input id="Amount" name="Amount" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Reason" name="Reason" cols="65" rows="5" disabled="disabled" ></textarea>
                    </td>
                </tr>
        </table>
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
