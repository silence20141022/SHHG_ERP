<%@ Page Title="出差申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmTravelAppEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmTravelAppEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, subContentPanel, sngrid;

        function onPgLoad() {

            setPgUI();

            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateTime").val(new Date().format("Y-m-d"));
                $("#WriteUser").val(AimState.UserInfo.Name);
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
                    { id: 'Custom', header: '<label style="color:red;">客户</label>', dataIndex: 'Custom', width: 150, resizable: true, editor: new Ext.form.TextField({}) },
                    { id: 'Address', header: '<label style="color:red;">地址</label>', editor: new Ext.form.TextField({}), dataIndex: 'Address', width: 200, resizable: true }, //, editor: { xtype: 'aimproject', emptyText: '输入拼音首字母', popAfter: UserSel, allowBlank: false }

                    {id: 'Days', header: '<label style="color:red;">天数</label>', dataIndex: 'Days', width: 100, resizable: true, editor: new Ext.form.NumberField({}) },
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
                    items: ['<label style="color:red;">出差项：</label>', '-', {
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
                    autoExpandColumn: 'Remark'
                });

                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                };
            }
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
                    <input id="Number" name="Number" disabled="disabled" value="自动生成" />
                </td>
                <td class="aim-ui-td-caption">
                    开始时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="BeginTime" name="BeginTime" aimctrl="date" class="validate[required]" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    预计天数
                </td>
                <td class="aim-ui-td-data">
                    <input id="Days" name="Days" class="validate[required custom[onlyInteger]]" />
                </td>
                <td class="aim-ui-td-caption">
                    地点
                </td>
                <td class="aim-ui-td-data">
                    <input id="Address" name="Address" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    出差人
                </td>
                <td class="aim-ui-td-data">
                    <input aimctrl='user' required="true" id="LeaveUser" name="LeaveUser" />
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
                    填单人
                </td>
                <td class="aim-ui-td-data">
                    <input disabled="disabled" id="WriteUser" name="WriteUser" />
                </td>
                <td class="aim-ui-td-caption">
                    申请日期
                </td>
                <td class="aim-ui-td-data">
                    <input id="CreateTime" name="CreateTime" aimctrl="date" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    出差原因
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
