﻿<%@ Page Title="组编辑" Language="C#" MasterPageFile="~/Masters/Ext/formpage.master" AutoEventWireup="true" CodeBehind="GrpEdit.aspx.cs" Inherits="Aim.Portal.Web.Modules.SysApp.OrgMag.GrpEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var PrimaryKey = "GroupID";
    var StatusEnum = { 1: "启用", 0: "停用" };
    var GrpTypeEnum;
    
    var viewport;
    var form;
    var op = "";
    var action = "";    // 执行提交操作类型
    var tittext = "组编辑";   // 标题字段
    var basicForm;
    var grpTypeField;
    var sbButton;   // 提交按钮

    function onPgLoad() {
        GrpTypeEnum = AimState["GrpTypeEnum"] || {};

        setPgUI();
    }

    function setPgUI() {

        sbButton = new Ext.Button({ text: '提交',
        iconCls: 'aim-icon-accept',
            handler: function() {
                submitForm();
            }
        });

        grpTypeField = new Ext.ux.form.AimComboBox({
            fieldLabel: '组类型',
            name: 'Type',
            allowBlank: false,
            anchor: '75%',
            hiddenName: 'Type',
            enumdata: GrpTypeEnum
        });

        form = new Ext.ux.form.AimFormPanel({
            url: '#',
            region: 'center',
            frame: true,
            bodyStyle: 'padding:5px 5px 0',
            defaultType: 'textfield',

            items: [{
                name: PrimaryKey,
                maxLength: 36,
                hidden: true,
                hideLabel: true
            }, {
                fieldLabel: '组名',
                name: 'Name',
                maxLength: 50,
                allowBlank: false,
                anchor: '75%'
            }, {
            fieldLabel: '编号',
                name: 'Code',
                maxLength: 50,
                allowBlank: false,
                anchor: '75%'
            }, grpTypeField, new Ext.form.NumberField({
                fieldLabel: '排序号',
                name: 'SortIndex',
                anchor: '75%'
            }), new Ext.ux.form.AimComboBox({
                fieldLabel: '状态',
                name: 'Status',
                allowBlank: false,
                anchor: '75%',
                hiddenName: 'Status',
                enumdata: StatusEnum
            }), {
                fieldLabel: '描述',
                name: 'Description',
                maxLength: 500,
                xtype: 'textarea',
                anchor: '100% -153'  // anchor width by percentage and height by raw adjustment
}],

                buttons: [sbButton, {
                    text: '取消',
                    iconCls: 'aim-icon-cancel',
                    handler: function() {
                        returnClose({ result: "cancel" });
                    }
}]
                });

            viewport = new Ext.Viewport({
                layout: 'border',
                items: [form]
            });

            basicForm = form.getForm();

            op = $.getQueryString({ ID: "op" });
            bindData();

            if (op === "c" || op === "cs") {
            }

            switch (op) {
                case "c":
                case "cs":
                    var grpType = $.getQueryString({ ID: "type" });
                    if (grpType || grpType === 0) {
                        grpTypeField.setValue(grpType);
                        grpTypeField.setReadOnly(true);
                    }
                    action = (op == 'c' ? "create" : "createsub");
                    break;
                case "u":
                    action = "update"; 
                    break;
                case "d":
                    action = "delete";
                    sbButton.setIconClass("aim-icon-delete");
                    sbButton.setText("删除");
                    form.setReadOnly(true);
                    break;
                default:
                    sbButton.hide();
                    form.setReadOnly(true);
                    break;
            }
        }

        function bindData() {
            var frmdata = AimState["frmdata"] || {};
            basicForm.setValues(frmdata);
        }

        function submitForm(act, dt, url, onProcFinished) {
            // enable 隐藏主键以便于提交
            $(":input[name=" + PrimaryKey + "]").attr("disabled", false);
            onProcFinished = onProcFinished || onSubmitFinished;
            act = act || action;    // 对应服务器端的RequestAction
            if (basicForm.isValid() && basicForm.el) {
                Aim.Form.submitData(basicForm.getValues(), act, dt, url, onProcFinished);
            }
        }

        function returnClose(rtn) {
            window.returnValue = rtn;
            window.close();
        }

        // 处理完成
        function onSubmitFinished(args) {
            var data, frmdata;
            if (args && args.data) { frmdata = args.data.frmdata }
            var data = frmdata || basicForm.getValues();
            returnClose({ result: "success", data: data });
        }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">

</asp:Content>
