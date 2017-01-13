<%@ Page Title="客户信息" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmCustomersEdit.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.FrmCustomersEdit" %>

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
                $("#MagId").val(AimState.UserInfo.UserID);
                $("#MagUser").dataBind(AimState.UserInfo.Name);
            }

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.parent.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {

            jQuery.ajaxExec('VerificationName', { id: $("#Id").val(), name: $("#Name").val() }, function(rtn) {
                if (rtn.data.error) {
                    AimDlg.show(rtn.data.error);
                }
                else {
                    ProcGridData();
                    AimFrm.submit(pgAction, {}, null, SubFinish);
                }
            });

        }

        function SubFinish(args) {
            //RefreshClose();
            window.parent.opener.location = window.parent.opener.location;
            window.parent.close();
        }

        // 处理网格数据
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function() {
                subdata.push(this.data);
            });

            var jsonstr = $.getJsonString(subdata);

            $("#Contact").val(jsonstr);
        }

        function InitEditTable() {
            // 表格数据
            myData = {
                records: $.getJsonObj($("#Contact").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
			    { name: 'UserName' },
			    { name: 'Tel' },
			    { name: 'Fax' },
			    { name: 'MobilePhone' },
			    { name: 'Address' },
			    { name: 'Department' },
			    { name: 'Position' },
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
                id: 'UserName',
                header: '姓名',
                dataIndex: 'UserName',
                width: 80,
                resizable: true,
                editor: new Ext.form.TextField({})
            },
            {
                id: 'Tel',
                header: '电话',
                dataIndex: 'Tel',
                width: 80,
                resizable: true,
                editor: new Ext.form.TextField({})
            },
            {
                id: 'Fax',
                header: '传真',
                dataIndex: 'Fax',
                width: 80,
                resizable: true,
                editor: new Ext.form.TextField({})
            },
            {
                id: 'MobilePhone',
                header: '移动电话',
                dataIndex: 'MobilePhone',
                width: 80,
                resizable: true,
                editor: new Ext.form.TextField({})
            }, {
                id: 'Address',
                header: '邮寄地址',
                dataIndex: 'Address',
                width: 100,
                resizable: true,
                editor: new Ext.form.TextField({})
            }, {
                id: 'Department',
                header: '部门',
                dataIndex: 'Department',
                width: 80,
                resizable: true,
                editor: new Ext.form.TextField({})
            },
            {
                id: 'Position',
                header: '职务',
                dataIndex: 'Position',
                width: 80,
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
                width: Ext.get("StandardSub").getWidth() - 17,
                height: 200,
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">联系人：</label>', '-', {
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
    <div id="header" style="width: 101.5%;">
        <h1>
            客户信息</h1>
    </div>
    <div id="editDiv" align="center" style="width: 101.5%;">
        <div style="height: 475px; width: 100%; overflow: auto;">
            <table class="aim-ui-table-edit">
                <tbody>
                    <tr style="display: none">
                        <td colspan="4">
                            <input id="Id" name="Id" />
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            客户编号
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="Code" name="Code" class="validate[required]" />
                        </td>
                        <td class="aim-ui-td-caption">
                            名称
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="Name" name="Name" class="validate[required]" />
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            简称
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="SimpleName" name="SimpleName" />
                        </td>
                        <td class="aim-ui-td-caption">
                            开户银行
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="Bank" name="Bank" />
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            帐号
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="AccountNum" name="AccountNum" />
                        </td>
                        <td class="aim-ui-td-caption">
                            开户姓名
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="AccountName" name="AccountName" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="aim-ui-td-caption">
                            信用额度
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="CreditAmount" name="CreditAmount" />元
                        </td>
                        <td class="aim-ui-td-caption">
                            账期
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="AccountValidity" name="AccountValidity" />天
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="aim-ui-td-caption">
                            警告天数
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="WarnDays" name="WarnDays" />天
                        </td>
                        <td class="aim-ui-td-caption">
                            预警间隔
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="WarnInterval" name="WarnInterval" />天
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            税号
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="TariffNum" name="TariffNum" />
                        </td>
                        <td class="aim-ui-td-caption">
                            省份
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="Province" name="Province" />
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            城市
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="City" name="City" />
                        </td>
                        <td class="aim-ui-td-caption">
                            邮编
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="ZipCode" name="ZipCode" />
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            地址
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="Address" name="Address" />
                        </td>
                        <td class="aim-ui-td-caption">
                            重要性
                        </td>
                        <td class="aim-ui-td-data">
                            <select id="Importance" name="Importance" aimctrl='select' style="width: 152px;"
                                enumdata="AimState['CustomImportant']">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            网址
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="Website" name="Website" />
                        </td>
                        <td class="aim-ui-td-caption">
                            联系电话
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="Tel" name="Tel" />
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            业务关系建立时间
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="OpenTime" name="OpenTime" aimctrl="date" />
                        </td>
                        <td class="aim-ui-td-caption">
                            负责人
                        </td>
                        <td class="aim-ui-td-data">
                            <%--<input aimctrl='popup' id="MagUser" name="MagUser" disabled="disabled"
                            popurl="/CommonPages/Select/UsrSelect/MUsrSelect.aspx?seltype=single" popparam="MagId:UserID;MagUser:Name"
                            popstyle="width=700,height=500" />
                        <input type="hidden" id="MagId" name="MagId" />--%>
                            <input aimctrl='user' required="true" id="MagUser" name="MagUser" relateid="MagId" />
                            <input type="hidden" id="MagId" name="MagId" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="aim-ui-td-caption">
                            预存款金额
                        </td>
                        <td class="aim-ui-td-data">
                            <input id="PreDeposit" name="PreDeposit" />元
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            备注
                        </td>
                        <td class="aim-ui-td-data" colspan="3">
                            <textarea id="Remark" name="Remark" cols="69" rows="5"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="aim-ui-td-caption">
                            附件
                        </td>
                        <td class="aim-ui-td-data" colspan="3">
                            <textarea id="Attachment" name="Attachment" aimctrl="file"></textarea>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="aim-ui-td-caption">
                            客户类型
                        </td>
                        <td class="aim-ui-td-data" colspan="3">
                            <select id="UnitType" name="UnitType" aimctrl='select' style="width: 152px;" enumdata="AimState['CustomType']">
                            </select>
                        </td>
                    </tr>
                </tbody>
            </table>
            <textarea id="Contact" rows="5" style="width: 98%" style="display: none;"></textarea>
            <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
            </div>
        </div>
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
