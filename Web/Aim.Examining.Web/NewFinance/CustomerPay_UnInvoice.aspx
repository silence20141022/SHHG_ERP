<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerPay_UnInvoice.aspx.cs"
    Inherits="Aim.Examining.Web.NewFinance.CustomerPay_UnInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all-neptune.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="/js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var orderid = getQueryString("orderid");
        Ext.onReady(function () {
            var customer = Ext.create('Ext.form.field.Text', {
                fieldLabel: '客户名称',
                name: 'CName',
                readOnly: true,
                width: 400
            })
            var order_no = Ext.create('Ext.form.field.Text', {
                fieldLabel: '订单编号',
                name: 'Number',
                readOnly: true
            })
            var order_amount = Ext.create('Ext.form.field.Text', {
                fieldLabel: '订单金额(￥)',
                name: 'TotalMoney',
                readOnly: true
            })
            var store_paytype = Ext.create('Ext.data.JsonStore', {
                fields: ['name'],
                proxy: {
                    type: 'ajax',
                    url: 'CustomerPay_UnInvoice.aspx?action=loadpaytype',
                    reader: {
                        root: 'rows',
                        type: 'json'
                    }
                },
                autoLoad: true
            })
            var combo_paytype = Ext.create('Ext.form.field.ComboBox', {
                name: 'PayType',
                store: store_paytype,
                queryMode: 'local',
                editable: false,
                fieldLabel: '收款方式',
                displayField: 'name',
                valueField: 'name',
                forceSelection: true
            })
            var paydate = Ext.create('Ext.form.field.Date', {
                format: 'Y-m-d',
                fieldLabel: '收款日期',
                name: 'ReceivablesTime'
            })
            var paymoney = Ext.create('Ext.form.field.Number', {
                name: 'Money',
                fieldLabel: '收款金额(￥)',
                vtypeText: '请输入数字'
            })
            var remark = Ext.create('Ext.form.field.TextArea', {
                fieldLabel: '备注',
                name: 'Remark',
                height: 60,
                width: 600
            })
            var formpanel = Ext.create('Ext.form.Panel', {
                region: 'center',
                border: 0,
                fieldDefaults: {
                    margin: '10 10 10 30',
                    msgTarget: 'right'
                },
                items: [customer, order_no, order_amount, combo_paytype, paydate, paymoney, remark,
                { xtype: 'hiddenfield', name: 'CId' }, { xtype: 'hiddenfield', name: 'Id' }
   ],
                buttons: [{ text: '保 存', handler: function () {
                    if (formpanel.getForm().isValid()) {
                        //                        var detaildata = Ext.encode(Ext.pluck(grid_detail.store.data.items, 'data'));
                        //                      var action = id ? "update" : "create";
                        Ext.Ajax.request({
                            url: 'CustomerPay_UnInvoice.aspx',
                            params: { action: "create", orderid: orderid, formdata: Ext.encode(formpanel.getForm().getValues()) },
                            success: function (response, opts) {
                                Ext.MessageBox.alert('提示', '保存成功!', function () {
                                    if (window.opener && window.opener.store) {
                                        window.opener.store.load();
                                    }
                                    window.close();
                                });
                            }
                        })
                    }
                }
                }],
                buttonAlign: 'center'
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [formpanel]
            })
            Ext.Ajax.request({
                url: 'CustomerPay_UnInvoice.aspx?action=loadbyorderid',
                params: { orderid: orderid },
                async: false,
                success: function (response, opts) {
                    var json = Ext.decode(response.responseText);

                    formpanel.getForm().setValues(json.data);
                }
            })
        });
    </script>
</head>
<body>
</body>
</html>
