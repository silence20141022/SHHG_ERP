<%@ Page Title="其他费用申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmOtherAppEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmOtherAppEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, subContentPanel, sngrid;

        function onPgLoad() {

            setPgUI();

        }

        function setPgUI() {

            if (pgOperation == "c" || pgOperation == "cs") {
                $("#LeaveTime").val(new Date().format("Y-m-d"));
                $("#LeaveUser").val(AimState.UserInfo.Name);
                $("#DeptName").val(AimState["ReleDepartment"]);
            }

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
			    { name: 'date' },
			    { name: 'type' },
			    { name: 'feiyong' },
			    { name: 'InvoNumber' },
			    { name: 'Remark' }
			    ]
            });

            //商品状态下拉框
            var becombo = new Ext.ux.form.AimComboBox({
                enumdata: AimState["OtherCost"],
                lazyRender: false,
                allowBlank: false,
                triggerAction: 'all',
                mode: 'local',
                listeners: {
                    collapse: function(obj) {
                        if (grid.activeEditor) {
                            var rec = store.getAt(grid.activeEditor.row);
                            if (rec) {
                                rec.set("type", obj.getValue());
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
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Pro', header: '<label style="color:red;">项目</label>', dataIndex: 'Pro', width: 120, resizable: true, editor: new Ext.form.TextField({}), summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'date', header: '<label style="color:red;">时间</label>', dataIndex: 'date', width: 80, resizable: true, renderer: ExtGridDateOnlyRender, editor: { xtype: 'datefield'} },

                    { id: 'type', header: '<label style="color:red;">报销项目</label>', dataIndex: 'type', summaryType: 'sum', width: 80, resizable: true, editor: becombo },
                    { id: 'feiyong', header: '<label style="color:red;">报销费用</label>', dataIndex: 'feiyong', summaryType: 'sum', width: 80, resizable: true, editor: new Ext.form.NumberField({}) },
                    { id: 'InvoNumber', header: '<label style="color:red;">发票号码</label>', dataIndex: 'InvoNumber', width: 100, resizable: true, editor: new Ext.form.TextField({}) },

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
                    items: ['<label style="color:red;">项目：</label>', '-', {
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function() {

                            var EntRecord = grid.getStore().recordType;
                            var p = new EntRecord({});
                            grid.stopEditing();
                            insRowIdx = store.data.length;
                            store.insert(insRowIdx, p);
                            grid.startEditing(insRowIdx, 1);

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
}]
                    }),
                    autoExpandColumn: 'Remark',
                    listeners: { "afteredit": function(val) {
                        if (val.field == "feiyong") {
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
                    if (temp.get("feiyong")) {
                        result += temp.get("feiyong");
                    }
                }
                $("#Amount").val(result);
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
            其他费用申请</h1>
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
                    <input id="Number" name="Number" disabled="disabled" value="自动生成" />
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
                <td class="aim-ui-td-data">
                    <input id="LeaveTime" name="LeaveTime" disabled="disabled" />
                </td>
                <td class="aim-ui-td-caption">
                    报销金额
                </td>
                <td class="aim-ui-td-data">
                    <input id="Amount" name="Amount" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    申请人
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <input disabled="disabled" id="LeaveUser" name="LeaveUser" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    备注
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <textarea id="Reason" name="Reason" cols="65" rows="5"></textarea>
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
