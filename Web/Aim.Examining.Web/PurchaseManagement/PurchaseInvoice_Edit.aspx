<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseInvoice_Edit.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseInvoice_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var inwarehouseids = getQueryString("inwarehouseids");
        var op = getQueryString("op");
        var id = getQueryString("PurchaseInvoiceId");
        Ext.onReady(function () {
            var formpanel = Ext.create('Ext.form.Panel', {
                region: 'center',
                items: [
                       { layout: 'column', border: 0, items: [
                       { xtype: 'textfield', fieldLabel: '发票编号', margin: '5', name: 'InvoiceNo', columnWidth: .5, labelAlign: 'right' },
                       { xtype: 'textfield', fieldLabel: '供应商', margin: '5', name: 'SupplierName', readOnly: true, columnWidth: .5, labelAlign: 'right' }
                       ]
                       },
                       { layout: 'column', border: 0, items: [
                       { xtype: 'textfield', fieldLabel: '币种', margin: '5', name: 'Symbo', readOnly: true, columnWidth: .5, labelAlign: 'right' },
                       { xtype: 'textfield', fieldLabel: '发票金额', margin: '5', id: 'InvoiceAmount', name: 'InvoiceAmount', readOnly: true, columnWidth: .5, labelAlign: 'right' }
                       ]
                       },
                       { xtype: 'textfield', fieldLabel: '备注', margin: '5', name: 'Remark', anchor: '100%', height: 50, labelAlign: 'right' },
                       { xtype: 'textfield', name: 'SupplierId', hidden: true },
                       { xtype: 'textfield', name: 'Id', hidden: true }
            ],
                buttons: [{ text: '保 存', hidden: op == 'view', handler: function () {
                    if (formpanel.getForm().isValid()) {
                        if (store_detail.data.length == 0) {
                            Ext.MessageBox.alert('提示', '付款单明细不能为空!');
                            return;
                        }
                        var detaildata = Ext.encode(Ext.pluck(grid_detail.store.data.items, 'data'));
                        var action = id ? "update" : "create";
                        Ext.Ajax.request({
                            url: 'PurchaseInvoice_Edit.aspx',
                            params: { action: action, id: id, inwarehouseids: inwarehouseids, formdata: Ext.encode(formpanel.getForm().getValues()), detaildata: detaildata },
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
            Ext.regModel('PurchaseInvoiceDetail', { fields: [{ name: 'Id' }, { name: 'InWarehouseDetailId' }, { name: 'PurchaseInvoiceId' }, { name: 'PurchaseOrderDetailId' }, { name: 'ProductId' },
                      { name: 'ProductCode' }, { name: 'ProductName' }, { name: 'PurchaseOrderNo' },
                      { name: 'BuyPrice', type: 'float' }, { name: 'InvoiceQuantity', type: 'int' },
                      { name: 'InvoiceAmount', type: 'float' }, { name: 'Remark' }, { name: 'MaxQuan'}]
            })
            var store_detail = Ext.create('Ext.data.JsonStore', {
                model: 'PurchaseInvoiceDetail',
                proxy: {
                    url: 'PurchaseInvoice_Edit.aspx?action=loaddetail&id=' + id + '&inwarehouseids=' + inwarehouseids,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true,
                listeners: { load: function () {
                    if (!id) {
                        Ext.getCmp('InvoiceAmount').setValue(store_detail.sum('InvoiceAmount'));
                    }
                }
                }
            })
            var toolbar_detail = Ext.create('Ext.toolbar.Toolbar', {
                items: [
                { text: '删除', icon: '/images/shared/delete.gif', hidden: op == 'view', handler: function () {
                    var recs = grid_detail.getSelectionModel().getSelection();
                    store_detail.remove(recs);
                    Ext.getCmp('InvoiceAmount').setValue(store_detail.sum('InvoiceAmount'));
                }
                }]
            })
            var editor_detail = Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                listeners: {
                    beforeedit: function (editor, e, eOpts) {
                        if (e.field == "InvoiceQuantity") {
                            Ext.getCmp("nf1").setMaxValue(parseFloat(e.record.get("MaxQuan")));
                        }
                    }
                , edit: function (editor, e) {
                    if (e.field == "BuyPrice" || e.field == "InvoiceQuantity") {
                        if (e.record.get("BuyPrice") && e.record.get("InvoiceQuantity")) {
                            var amount = Math.round(parseFloat(e.record.get("BuyPrice")) * parseInt(e.record.get("InvoiceQuantity")) * 100) / 100;
                            e.record.set("InvoiceAmount", amount);
                            Ext.getCmp('InvoiceAmount').setValue(store_detail.sum("InvoiceAmount"));
                        }
                    }
                }
                }
            })
            var grid_detail = Ext.create('Ext.grid.Panel', {
                title: '发票明细',
                tbar: toolbar_detail,
                store: store_detail,
                region: 'south',
                height: 370,
                plugins: [editor_detail],
                columns: [
                    { xtype: 'rownumberer', width: 25 },
                    { header: 'Id', dataIndex: 'Id', hidden: true },
                    { header: 'InWarehouseDetailId', dataIndex: 'InWarehouseDetailId', hidden: true },
                    { dataIndex: 'PurchaseInvoiceId', hidden: true },
                    { header: 'PurchaseOrderDetailId', dataIndex: 'PurchaseOrderDetailId', hidden: true },
                    { header: 'ProductId', dataIndex: 'ProductId', hidden: true },
                    { header: '产品名称', dataIndex: 'ProductName', width: 150 },
                    { header: '型号', dataIndex: 'ProductCode', flex: 1 },
                    { header: '价格', dataIndex: 'BuyPrice', width: 80 },
                    { dataIndex: 'InvoiceQuantity', header: '数量', width: 60, editor: { xtype: 'numberfield', id: 'nf1', minValue: 1, allowBlank: false,
                        maxText: '生成付款单数量的最大值是{0}', msgTarget: 'under'
                    }
                    },
                    { dataIndex: 'InvoiceAmount', header: '金额', width: 100 },
                    { dataIndex: 'Remark', header: '备注', width: 100, field: { xtype: 'textarea', allowBlank: false} }
                    ]
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [formpanel, grid_detail]
            })
            Ext.Ajax.request({
                url: 'PurchaseInvoice_Edit.aspx?action=loadform',
                params: { id: id, inwarehouseids: inwarehouseids },
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
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
