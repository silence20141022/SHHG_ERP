<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayBillEdit_New.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.PayBillEdit_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var id = getQueryString("id");
        var inwarehouseids = getQueryString("inwarehouseids");
        var op = getQueryString("op");
        Ext.onReady(function () {
            var formpanel = Ext.create('Ext.form.Panel', {
                region: 'center',
                items: [
                       { layout: 'column', border: 0, items: [
                       { xtype: 'textfield', fieldLabel: '付款单编号', margin: '5', name: 'PayBillNo', readOnly: true, columnWidth: .5, labelAlign: 'right' },
                       { xtype: 'textfield', fieldLabel: '供应商', margin: '5', name: 'SupplierName', readOnly: true, columnWidth: .5, labelAlign: 'right' }
                       ]
                       },
                       { layout: 'column', border: 0, items: [
                       { xtype: 'textfield', fieldLabel: '币种', margin: '5', name: 'Symbo', readOnly: true, columnWidth: .5, labelAlign: 'right' },
                       { xtype: 'textfield', fieldLabel: '应付金额', margin: '5', id: 'PAmount', name: 'PAmount', readOnly: true, columnWidth: .5, labelAlign: 'right' }
                       ]
                       },
                       { layout: 'column', border: 0, items: [
                       { xtype: 'numberfield', fieldLabel: '折扣金额', margin: '5', name: 'DiscountAmount', columnWidth: .5, labelAlign: 'right' }
                       ]
                       },
                       { xtype: 'textfield', fieldLabel: '备注', margin: '5', name: 'Remark', anchor: '100%', height: 50, labelAlign: 'right' },
                       { xtype: 'textfield', name: 'SupplierId', hidden: true }, { xtype: 'textfield', name: 'Id', hidden: true }
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
                            url: 'PayBillEdit_New.aspx',
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
            Ext.regModel('PayBillDetail', { fields: [{ name: 'Id' }, { name: 'PayBillId' }, { name: 'InWarehouseDetailId' }, { name: 'PurchaseOrderDetailId' },
                      { name: 'ProductId' }, { name: 'ProductCode' }, { name: 'ProductName' },
                      { name: 'BuyPrice', type: 'float' }, { name: 'PayQuantity', type: 'int' },
                      { name: 'Amount', type: 'float' }, { name: 'PurchaseOrderNo' }, { name: 'MaxQuan', type: 'int'}]
            })
            var store_detail = Ext.create('Ext.data.JsonStore', {
                model: 'PayBillDetail',
                proxy: {
                    url: 'PayBillEdit_New.aspx?action=loaddetail&id=' + id + '&inwarehouseids=' + inwarehouseids,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true,
                listeners: { load: function () {
                    Ext.getCmp('PAmount').setValue(store_detail.sum('Amount'));
                }
                }
            })
            var toolbar_detail = Ext.create('Ext.toolbar.Toolbar', {
                items: [
                { text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                    var recs = grid_detail.getSelectionModel().getSelection();
                    store_detail.remove(recs);
                    Ext.getCmp('PAmount').setValue(store_detail.sum('Amount'));
                }
                }]
            })
            var editor_detail = Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                listeners: {
                    beforeedit: function (editor, e, eOpts) {
                        if (e.field == "PayQuantity") {
                            Ext.getCmp("nf1").setMaxValue(parseFloat(e.record.get("MaxQuan")));
                        }
                    }, edit: function (editor, e) {
                        if (e.record.get("BuyPrice") && e.record.get("PayQuantity")) {
                            var amount = Math.round(parseFloat(e.record.get("BuyPrice")) * parseInt(e.record.get("PayQuantity")) * 100) / 100;
                            e.record.set("Amount", amount);
                            Ext.getCmp('PAmount').setValue(store_detail.sum("Amount"));
                        }
                    }
                }
            })
            var grid_detail = Ext.create('Ext.grid.Panel', {
                title: '付款单明细',
                tbar: toolbar_detail,
                store: store_detail,
                region: 'south',
                height: 350,
                plugins: [editor_detail],
                columns: [
                    { xtype: 'rownumberer', width: 25 },
                    { header: '采购编号', dataIndex: 'PurchaseOrderNo', width: 110 },
                    { header: '产品名称', dataIndex: 'ProductName', width: 110 },
                    { header: '型号', dataIndex: 'ProductCode', flex: 1 },
                    { header: '价格', dataIndex: 'BuyPrice', width: 80 },
                    { dataIndex: 'PayQuantity', header: '数量', width: 60, editor: { xtype: 'numberfield', id: 'nf1', minValue: 1, allowBlank: false,
                        maxText: '生成付款单数量的最大值是{0}', msgTarget: 'under'
                    }
                    },
                    { dataIndex: 'Amount', header: '金额', width: 100 }
                    ]
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [formpanel, grid_detail]
            })
            Ext.Ajax.request({
                url: 'PayBillEdit_New.aspx?action=loadform',
                params: { id: id, inwarehouseids: inwarehouseids },
                success: function (response, opts) {
                    var json = Ext.decode(response.responseText);
                    formpanel.getForm().setValues(json.data);
                }
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
