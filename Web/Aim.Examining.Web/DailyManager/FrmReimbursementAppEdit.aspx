<%@ Page Title="出差报销申请" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmReimbursementAppEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmReimbursementAppEdit" %>

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
                    { id: 'Pro', header: '<label style="color:red;">项目</label>', dataIndex: 'Pro', width: 120, resizable: true, editor: new Ext.form.TextField({}), summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'begindate', header: '<label style="color:red;">开始时间</label>', dataIndex: 'begindate', width: 80, resizable: true, renderer: ExtGridDateOnlyRender, editor: { xtype: 'datefield'} },
                    { id: 'enddate', header: '<label style="color:red;">结束时间</label>', dataIndex: 'enddate', width: 80, resizable: true, renderer: ExtGridDateOnlyRender, editor: { xtype: 'datefield'} },

                    { id: 'chailv', header: '<label style="color:red;">差旅费</label>', dataIndex: 'chailv', summaryType: 'sum', width: 80, resizable: true, editor: new Ext.form.NumberField({}) },
                    { id: 'zhusu', header: '<label style="color:red;">住宿费</label>', dataIndex: 'zhusu', summaryType: 'sum', width: 80, resizable: true, editor: new Ext.form.NumberField({}) },
                    { id: 'canfei', header: '<label style="color:red;">餐费</label>', dataIndex: 'canfei', summaryType: 'sum', width: 80, resizable: true, editor: new Ext.form.NumberField({}) },
                    { id: 'other', header: '<label style="color:red;">其他</label>', dataIndex: 'other', summaryType: 'sum', width: 80, resizable: true, editor: new Ext.form.NumberField({}) },

                    { id: 'Amount', header: '合计', dataIndex: 'Amount', width: 80, summaryType: 'sum', resizable: true },
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

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            出差报销申请</h1>
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
                    报销人
                </td>
                <td class="aim-ui-td-data">
                    <input aimctrl='user' required="true" id="LeaveUser" name="LeaveUser" />
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
                    <input id="CreateTime" name="CreateTime" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    报销金额
                </td>
                <td class="aim-ui-td-data">
                    <input id="Amount" name="Amount" disabled="disabled" />
                </td>
                <td class="aim-ui-td-caption">
                    填单人
                </td>
                <td class="aim-ui-td-data">
                    <input disabled="disabled" id="WriteUser" name="WriteUser" />
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
