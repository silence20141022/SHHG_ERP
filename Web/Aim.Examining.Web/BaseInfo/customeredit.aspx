<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customeredit.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.customeredit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link href="/Extjs42/resources/css/ext-all-gray.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            Ext.Ajax.request({
                url: 'customerlist.aspx?action=loaduser',
               // params: { codes: "ROLE,DEPARTMENT,JOBTITLE,POSITION,DEGREE,BELONGUNIT" },
                success: function (response, option) {
                    var data_user = Ext.decode(response.responseText); 
                    var field_Name = Ext.create('Ext.form.field.Text', {
                        name: 'Name',
                        fieldLabel: '客户全称',
                        allowBlank: false,
                        blankText: '客户全称不能为空!'
                    });            
                    var field_SimpleName = Ext.create('Ext.form.field.Text', {
                        name: 'SimpleName',
                        fieldLabel: '客户简称'
                    }); 
                    var field_Bank = Ext.create('Ext.form.field.Text', {
                        name: 'Bank',
                        fieldLabel: '开户银行'
                    });            
                    var field_AccountNum = Ext.create('Ext.form.field.Text', {
                        fieldLabel: '帐号',
                        name: 'AccountNum'
                    }) 
                    var field_AccountName = Ext.create('Ext.form.field.Text', {
                        fieldLabel: '开户姓名',
                        name: 'AccountName'
                    })
                    var field_TariffNum = Ext.create('Ext.form.field.Text', {
                        fieldLabel: '税号',
                        name: 'TariffNum'
                    })
                    var field_CreditAmount = Ext.create('Ext.form.field.Number', {
                        fieldLabel: '信用额度',
                        name: 'CreditAmount'
                    })
                    var field_AccountValidity = Ext.create('Ext.form.field.Number', {
                        fieldLabel: '账期',
                        name: 'AccountValidity'
                    })                    
                    var field_Address = Ext.create('Ext.form.field.Text', {
                        fieldLabel: '地址',
                        name: 'Address'
                    })
                    var field_Tel = Ext.create('Ext.form.field.Text', {
                        fieldLabel: '联系电话',
                        name: 'Tel'
                    })
                    var field_OpenTime = Ext.create('Ext.form.field.Date', {
                        fieldLabel: '业务关系建立时间',
                        name: 'OpenTime',
                        format: 'Y-m-d'
                    })
                    var store_MagUser = Ext.create('Ext.data.JsonStore', {
                        fields: ['UserID', 'Name'],
                        data: data_user
                    })
                    var combo_MagUser = Ext.create('Ext.form.field.ComboBox', {
                        name: 'MagUser',
                        store: store_MagUser,
                        fieldLabel: '销售负责人',
                        displayField: 'Name',
                        valueField: 'Name',
                        queryMode: 'local',
                        editable: false,
                        listeners: {
                            select: function (cb, records, eOpts) {
                                field_MagId.setValue(records[0].get("MagId")); 
                            }
                        }
                    })
                    var field_Remark = Ext.create('Ext.form.field.TextArea', {
                        fieldLabel: '备注',
                        name: 'Remark'
                    })
                    //隐藏的字段
                    var field_id = Ext.create('Ext.form.field.Hidden', {
                        name: 'Id'
                    });
                    var field_MagId = Ext.create('Ext.form.field.Hidden', {
                        name: 'MagId'
                    });
                    var formpanel = Ext.create('Ext.form.Panel', {
                        region: 'center',
                        title:'客户信息',
                        border: 0,
                        fieldDefaults: {
                            margin: '0 10 10 10',
                            columnWidth: .5,
                            labelAlign: 'right',
                            labelSeparator: '',
                            msgTarget: 'under',
                            labelWidth: 100
                        },
                        items: [
                        { layout: 'column', height: 42, margin: '15 0 0 0', border: 0, items: [field_Name, field_SimpleName] },
                        { layout: 'column', height: 42, border: 0, items: [field_Bank, field_AccountNum] },
                        { layout: 'column', height: 42, border: 0, items: [field_AccountName,field_TariffNum ] },
                        { layout: 'column', height: 42, border: 0, items: [field_CreditAmount,field_AccountValidity ] },
                        { layout: 'column', height: 42, border: 0, items: [field_Address, field_Tel] },
                        { layout: 'column', height: 42, border: 0, items: [field_OpenTime, combo_MagUser] },
                        { layout: 'column', border: 0, items: [field_Remark] },
                            field_id, field_MagId
                        ],
                        buttons: [{
                            text: '保存', handler: function () {
                                if (!formpanel.getForm().isValid()) {
                                    return;
                                }
                                var formdata = Ext.encode(formpanel.getForm().getValues());
                                var mask = new Ext.LoadMask(Ext.get(Ext.getBody()), { msg: "数据保存中，请稍等..." });
                                mask.show();
                                Ext.Ajax.request({
                                    url: 'customerlist.aspx?action=save',
                                    params: { formdata: formdata },
                                    success: function (response, option) {
                                        mask.hide();
                                        if (response.responseText > 0) {
                                            Ext.MessageBox.alert('提示', '保存成功！');
                                            window.location.href = "customerlist.aspx";
                                        }
                                        else {
                                            Ext.MessageBox.alert('提示', '保存失败！');
                                        }
                                    }
                                });
                            }
                        }, { text: '返回' }],
                        buttonAlign:'center'
                    });
                    var viewport = Ext.create('Ext.container.Viewport', {
                        layout: 'border',
                        items: [formpanel]
                    })
                    //if (formdata) {
                    //    formpanel.getForm().setValues(formdata);
                    //    if (Ext.getCmp('icon').getValue()) {
                    //        Ext.getCmp('img_icon').setSrc('/Upload/' + Ext.getCmp('icon').getValue());
                    //    }
                    //    if (Ext.getCmp('qrcode').getValue()) {
                    //        Ext.getCmp('img_qrcode').setSrc('/Upload/' + Ext.getCmp('qrcode').getValue());
                    //    }
                    //}
                }
            })
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
