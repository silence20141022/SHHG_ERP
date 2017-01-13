<%@ Page Title="出差申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmTravelAppWFView.aspx.cs" Inherits="Aim.Examining.Web.FrmTravelAppWFView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, subContentPanel, sngrid;

        function onPgLoad() {

            setPgUI();

            if (!$("#LeaveTime").val()) {
                $("#CreateTime").val(new Date().format("Y-m-d"));
                $("#LeaveUser").val(AimState.UserInfo.Name);
                $("#DeptName").val(AimState["ReleDepartment"]);
            }
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
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }

        function UserSel(rtn) {
            if (rtn && rtn.data && grid.activeEditor) {
                var rec = store.getAt(grid.activeEditor.row);
                if (rec) {
                    rec.set("Address", rtn.data.Name || rtn.data[0].Name);
                }
            }
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
			    { name: 'Custom' },
			    { name: 'Address' },
			    { name: 'Days' },
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
                    { id: 'Custom', header: '客户', dataIndex: 'Custom', width: 150, resizable: true },
                    { id: 'Address', header: '地址', dataIndex: 'Address', width: 200, resizable: true }, //, editor: { xtype: 'aimproject', emptyText: '输入拼音首字母', popAfter: UserSel, allowBlank: false }

                    {id: 'Days', header: '天数', dataIndex: 'Days', width: 100, resizable: true },
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
                    autoExpandColumn: 'Remark'
                });

                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                };
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
            出差申请</h1>
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
                    编号
                </td>
                <td class="aim-ui-td-data">
                    <input id="Number" name="Number" disabled="disabled" />
                </td>
                <td class="aim-ui-td-caption">
                    开始时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="BeginTime" name="BeginTime" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    预计天数
                </td>
                <td class="aim-ui-td-data">
                    <input id="Days" name="Days" disabled="disabled" />
                </td>
                <td class="aim-ui-td-caption">
                    地点
                </td>
                <td class="aim-ui-td-data">
                    <input id="Address" name="Address" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    申请人
                </td>
                <td class="aim-ui-td-data">
                    <input disabled="disabled" id="LeaveUser" name="LeaveUser" />
                </td>
                <td class="aim-ui-td-caption">
                    部门
                </td>
                <td class="aim-ui-td-data">
                    <input disabled="disabled" id="DeptName" name="DeptName" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    申请日期
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <input id="CreateTime" name="CreateTime" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    出差原因
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
