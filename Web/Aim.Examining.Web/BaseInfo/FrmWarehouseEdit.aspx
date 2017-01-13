<%@ Page Title="仓库信息" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmWarehouseEdit.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.FrmWarehouseEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            InitEditTable();
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#CreateName").val(AimState.UserInfo.Name);
                $("#CreateTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
            }

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

            $("#WareSeat").val(jsonstr);
        }

        function InitEditTable() {
            // 表格数据
            myData = {
                records: $.getJsonObj($("#WareSeat").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Type' },
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

            {
                id: 'Code',
                header: '编号',
                dataIndex: 'Code',
                width: 80,
                resizable: true,
                editor: new Ext.form.TextField({})
            },
            {
                id: 'Name',
                header: '名称',
                dataIndex: 'Name',
                width: 100,
                resizable: true,
                editor: new Ext.form.TextField({})
            },
            {
                id: 'Type',
                header: '类型',
                dataIndex: 'Type',
                width: 120,
                resizable: true,
                editor: new Ext.form.TextField({})
            },
            {
                id: 'Remark',
                header: '备注',
                dataIndex: 'Remark',
                width: 100,
                resizable: true,
                editor: new Ext.form.TextArea({})
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
                height: 280,
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">库位信息：</label>', '-', {
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function() {
                            var EntRecord = grid.getStore().recordType;
                            var p = new EntRecord({
                        });
                        grid.stopEditing();
                        var insRowIdx = store.data.length;
                        store.insert(insRowIdx, p);
                        grid.startEditing(insRowIdx, 1);
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
            仓库信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td colspan="4">
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption" style="width: 15%">
                        仓库编号
                    </td>
                    <td class="aim-ui-td-data" style="width: 35%">
                        <input id="Code" name="Code" />
                    </td>
                    <td class="aim-ui-td-caption">
                        仓库名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Name" name="Name" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        管理员
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <%--<input aimctrl='popup' id="ManagerName" name="ManagerName" disabled="disabled" relateid="txtuserid" 
                            popurl="/CommonPages/Select/UsrSelect/MUsrSelect.aspx?seltype=single" popparam="ManagerId:UserID;ManagerName:Name"
                            popstyle="width=700,height=500" />
                        <input type="hidden" id="ManagerId" name="ManagerId" />--%>
                        
                        <input aimctrl='user' required="true" id="ManagerName" name="ManagerName" relateid="ManagerId" />
                        <input type="hidden" id="ManagerId" name="ManagerId"/>
                    </td>
                </tr>
                <tr>
                    <td>仓库图片</td>
                    <td class="aim-ui-td-data" colspan="3">
                        <input id="Images" name="Images" aimctrl='file' value="" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" cols="54" rows="5" ></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        
        <textarea id="WareSeat" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;"></div>
        
        <table class="aim-ui-table-edit">
            <tbody>
                <tr width="100%" style="display: none">
                    <td class="aim-ui-td-caption">
                        录入人
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateName" name="CreateName" />
                    </td>
                    <td class="aim-ui-td-caption">
                        录入日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input disabled id="CreateTime" name="CreateTime" dateonly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnSubmit" class="aim-ui-button submit">提交</a> <a id="btnCancel" class="aim-ui-button cancel">
                            取消</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
