<%@ Page Title="产品信息" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="True" CodeBehind="ProductEdit.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.ProductEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .aim-ui-td-caption
        {
            text-align: right;
        }
        fieldset
        {
            margin: 15px;
            width: 100%;
            padding: 5px;
        }
        fieldset legend
        {
            font-size: 12px;
            font-weight: bold;
        }
        .righttxt
        {
            text-align: right;
        }
        input
        {
            width: 90%;
        }
        select
        {
            width: 90%;
        }
        .x-superboxselect-display-btns
        {
            width: 90% !important;
        }
        .x-form-field-trigger-wrap
        {
            width: 100% !important;
        }
    </style>

    <script type="text/javascript">
        var ProxyEnum = { 是: '是', 否: '否' };
        function onPgLoad() {
            setPgUI();
            InitGrid();
        }
        function setPgUI() {
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#ProMsgId").val(AimState.UserInfo.UserID);
                $("#ProMsg").dataBind(AimState.UserInfo.Name);
            }
        }
        function SuccessSubmit() {
            if (!$("#SupplierId").val()) {
                AimDlg.show("供应商不能为空！");
                return;
            }
            var recs = store.getRange();
            var dt = store.getModifiedDataStringArr(recs) || [];
            //验证Code是否重复
            jQuery.ajaxExec('VerificationCode', { id: $("#Id").val(), code: $("#Code").val() }, function(rtn) {
                if (rtn.data.IsExist == true) {
                    AimDlg.show("该产品型号已经存在！");
                }
                else {
                    AimFrm.submit(pgAction, { data: dt }, null, SubFinish);
                }
            });
        }
        function SubFinish(args) {
            RefreshClose();
        }
        //选择商品
        function MultiAddPros() {
            var style = "dialogWidth:750px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/ProductSelect.aspx?seltype=multi&rtntype=array&PId=" + $("#Id").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ CId: users[i].Id, Isbn: users[i].Isbn, Code: users[i].Code, Name: users[i].Name, CCount: 1 });
                    store.insert(store.data.length, p);
                }
            });
        }
        function InitGrid() {
            myData = {
                records: AimState["DetailList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                fields: [{ name: 'Id' }, { name: 'PId' }, { name: 'CId' }, { name: 'Isbn' }, { name: 'Code' }, { name: 'Name' },
                 { name: 'CCount' }, { name: 'Remark'}]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                { id: 'Id', header: 'Id', dataIndex: 'Id', hidden: true },
                { id: 'PId', header: 'PId', dataIndex: 'PId', hidden: true },
                { id: 'CId', header: 'CId', dataIndex: 'CId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 120, resizable: true },
                { id: 'Code', header: '产品型号', renderer: ExtGridpperCase, dataIndex: 'Code', width: 120, resizable: true },
                { id: 'Isbn', header: '条形码', dataIndex: 'Isbn', width: 140, resizable: true },
                { id: 'CCount', header: '<label style="color:red;">数量</label>', dataIndex: 'CCount', width: 50, resizable: true, allowBlank: false,
                    editor: new Ext.form.NumberField({ allowBlank: false })
                },
                { id: 'Remark', header: '<label style="color:red;">备注</label>', dataIndex: 'Remark', width: 100, resizable: true, editor: new Ext.form.TextArea({}) }
        ]
            });
            var tlbar = new Ext.Toolbar({
                items: [
            { text: '添加',
                iconCls: 'aim-icon-add',
                handler: function() {
                    MultiAddPros();
                    return;
                }
            }, '-', {
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
}]
            });
            grid = new Ext.ux.grid.AimEditorGridPanel({
                title: '子产品信息',
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                height: 150,
                autoExpandColumn: 'Code',
                tbar: tlbar,
                autoExpandColumn: 'Code'
            });
        }
        window.onresize = function() {
            grid.setWidth(0);
            grid.setWidth(Ext.get("StandardSub").getWidth());
        };
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            产品信息</h1>
    </div>
    <div id="editDiv" align="center">
        <fieldset>
            <legend>基本信息</legend>
            <table class="aim-ui-table-edit" width="100%" style="border: 0">
                <tr style="display: none">
                    <td colspan="4">
                        <input id="Id" name="Id" />
                        <input id="BuyPrice" name="BuyPrice" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption" style="width: 20%">
                        产品型号
                    </td>
                    <td class="aim-ui-td-data" style="width: 30%">
                        <input id="Code" name="Code" class="validate[required]" />
                    </td>
                    <td class="aim-ui-td-caption" style="width: 20%">
                        产品名称
                    </td>
                    <td class="aim-ui-td-data" style="width: 30%">
                        <input id="Name" name="Name" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        条形码
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Isbn" name="Isbn" />
                    </td>
                    <td class="aim-ui-td-caption">
                        PCN
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Pcn" name="Pcn" class="validate[required]" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        单位
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="Unit" name="Unit" aimctrl='select' class="validate[required]" enumdata="AimState['Unit']">
                        </select>
                    </td>
                    <td class="aim-ui-td-caption">
                        供应商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="SupplierName" name="SupplierName" class="validate[required]"
                            popurl="/CommonPages/Select/SupplierSelect.aspx" popparam="SupplierId:Id;SupplierName:SupplierName"
                            popstyle="width=800,height=550" style="width: 80%" />
                        <input id="SupplierId" name="SupplierId" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        产品类型
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="ProductType" name="ProductType" aimctrl='select' class="validate[required]"
                            enumdata="AimState['ProductType']">
                        </select>
                    </td>
                    <td class="aim-ui-td-caption">
                        宏谷代理产品
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="IsProxy" name="IsProxy" aimctrl='select' class="validate[required]" enumdata="ProxyEnum">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        包装箱条码1
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="FirstSkinIsbn" name="FirstSkinIsbn" />
                    </td>
                    <td class="aim-ui-td-caption">
                        产品数量1
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="FirstSkinCapacity" name="FirstSkinCapacity" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        包装箱条码2
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SecondSkinIsbn" name="SecondSkinIsbn" />
                    </td>
                    <td class="aim-ui-td-caption">
                        产品数量2
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SecondSkinCapacity" name="SecondSkinCapacity" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        产地
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="MakeArea" name="MakeArea" />
                    </td>
                    <td class="aim-ui-td-caption">
                        返现
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="BackCash" name="BackCash" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        售价(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SalePrice" name="SalePrice" />
                    </td>
                    <td class="aim-ui-td-caption">
                        最低售价(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="MinSalePrice" name="MinSalePrice" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        重量(kg)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Weight" name="Weight" />
                    </td>
                    <td class="aim-ui-td-caption">
                        预警最低值
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="MinCount" name="MinCount" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        库存预警间隔(天)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="WarnInterval" name="WarnInterval" />
                    </td>
                    <td class="aim-ui-td-caption">
                        产品负责人
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='user' required="true" id="ProMsg" name="ProMsg" relateid="ProMsgId" />
                        <input type="hidden" id="ProMsgId" name="ProMsgId" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <fieldset>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </fieldset>
    <table class="aim-ui-table-edit">
        <tr>
            <td class="aim-ui-button-panel" colspan="4">
                <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">
                    取消</a>
            </td>
        </tr>
    </table>
</asp:Content>
