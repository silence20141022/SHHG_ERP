<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManualPayoffInvoice.aspx.cs"
    Inherits="Aim.Examining.Web.NewFinance.ManualPayoffInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <link href="../font-awesome41/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var invoiceid = getQueryString("invoiceid");
        Ext.onReady(function () {
            var field_invoiceamount = Ext.create('Ext.form.field.Number', {
                fieldLabel: '开票金额(￥)',
                name: 'Amount'
            })
            var field_payamount = Ext.create('Ext.form.field.Number', {
                fieldLabel: '已付金额(￥)',
                name: 'PayAmount',
                value: 0
            })
            var field_baddebt = Ext.create('Ext.form.field.Number', {
                name: 'BadDebt',
                fieldLabel: '坏账金额(￥)'
            });
            var formpanel = Ext.create('Ext.form.Panel', {
                title: '发票基本信息',
                region: 'center',
                fieldDefaults: {
                    readOnly: true,
                    columnWidth: .50,
                    labelAlign: 'right',
                    hideTrigger: true,
                    margin: '10'
                },
                items: [
                { layout: 'column', border: 0, items: [
                                   { xtype: 'textfield', fieldLabel: '发票号', name: 'Number', columnWidth: .5, readOnly: true, labelAlign: 'right' },
                                   { xtype: 'textfield', fieldLabel: '客户名称', name: 'CName', columnWidth: .5, readOnly: true, labelAlign: 'right'}]
                }, { layout: 'column', border: 0, items: [field_invoiceamount, field_payamount]
                },
                { layout: 'column', border: 0, items: [
                                   { xtype: 'textfield', fieldLabel: '开票日期', name: 'InvoiceDate', readOnly: true, columnWidth: .5, labelAlign: 'right' },
                                  field_baddebt]
                },
                                   { xtype: 'textareafield', fieldLabel: '备注', allowBlank: false, msgTarget: 'under',
                                       blankText: '备注不能为空', id: 'remark', name: 'Remark', height: 50, anchor: '100%', labelAlign: 'right',readOnly:false
                                   },
                                   { xtype: 'textfield', name: 'Id', hidden: true }
                                   ],
                buttons: [{ text: '<i class="fa fa-check-square"></i>&nbsp;标记已全部付款', handler: function () {
                    if (formpanel.getForm().isValid()) {
                        Ext.Ajax.request({
                            url: 'ManualPayoffInvoice.aspx?action=payoffinvoice&invoiceid=' + invoiceid,
                            params: { remark: Ext.getCmp('remark').getValue() },
                            success: function (response, opts) {
                                var json = Ext.decode(response.responseText);
                                if (json.success == 'true') {
                                    Ext.MessageBox.alert('提示', '保存成功!', function () {
                                        if (window.opener && window.opener.store) {
                                            window.opener.store.load();
                                        }
                                        window.close();
                                    });
                                }
                            }
                        })
                    }
                }
                }],
                buttonAlign: 'center'
            })
            Ext.regModel('InvoiceDetail', { fields: [
                            { name: 'Id' }, { name: 'OrderInvoiceId' }, { name: 'OrderDetailId' }, { name: 'SaleOrderId' }, { name: 'ProductId' },
                            { name: 'ProductCode' }, { name: 'Unit' }, { name: 'ProductName' }, { name: 'SalePrice' }, { name: 'Count' }, { name: 'Number' }
                            ]
            })
            var store_detail = Ext.create('Ext.data.JsonStore', {
                model: 'InvoiceDetail',
                proxy: {
                    url: 'ManualPayoffInvoice.aspx?action=loaddetail&invoiceid=' + invoiceid,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            })
            var grid_detail = Ext.create('Ext.grid.Panel', {
                title: '发票明细',
                store: store_detail,
                region: 'south',
                height: 250,
                columns: [
                                { header: '销售单号', dataIndex: 'Number', width: 130 },
                                { header: '产品型号', dataIndex: 'ProductCode', flex: 1 },
                                { header: ' 产品名称', dataIndex: 'ProductName', width: 150 },
                                { header: '单位', dataIndex: 'Unit', width: 60 },
                                { header: '售价', dataIndex: 'SalePrice', width: 80 },
                                { header: '数量', dataIndex: 'Count', width: 60 },
                                { header: '金额', dataIndex: 'Amount', width: 80 }
                //                                { header: '退货数量', dataIndex: 'ReturnCount', width: 80 }
                                ]
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [formpanel, grid_detail]
            })
            Ext.Ajax.request({
                url: 'ManualPayoffInvoice.aspx?action=loadform&invoiceid=' + invoiceid,
                //  params: { id: id, inwarehouseids: inwarehouseids },
                //   async: false,
                success: function (response, opts) {
                    var json = Ext.decode(response.responseText);
                    formpanel.getForm().setValues(json.data);
                    field_baddebt.setValue(field_invoiceamount.getValue() - field_payamount.getValue())
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
