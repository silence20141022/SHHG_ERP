<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QiTaChanPin_Purchase_Edit.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.QiTaChanPin_Purchase_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>其他产品采购</title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var store_detail;
        var id = getQueryString("id");
        Ext.onReady(function () {
            var store_supplier = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'SupplierName', 'Symbo'],
                proxy: {
                    url: 'QiTaChanPin_Purchase.aspx?action=loadsupplier',
                    type: 'ajax',
                    reader: {
                        root: 'rows',
                        type: 'json'
                    }
                }
            })
            var combo_supplier = Ext.create('Ext.form.field.ComboBox', {
                name: 'SupplierName',
                store: store_supplier,
                queryParam: 'SupplierName',
                minChars: 1,
                displayField: 'SupplierName',
                valueField: 'SupplierName',
                fieldLabel: '供应商名称',
                msgTarget: 'under',
                allowBlank: false,
                blankText: '供应商不能为空',
                width: 350,
                hideTrigger: true,
                labelAlign: 'right',
                listeners: { select: function (combo, records, eOpts) {
                    Ext.getCmp("SupplierId").setValue(records[0].get("Id"));
                    Ext.getCmp("Symbo").setValue(records[0].get("Symbo"));
                }
                }
            })
            var store_suppliertype = Ext.create('Ext.data.JsonStore', {
                fields: ['name'],
                data: [{ name: '生产商' }, { name: '中间商'}]
            })
            var combo_PurchaseType = Ext.create('Ext.form.field.ComboBox', {
                store: store_suppliertype,
                colspan: 2,
                queryMode: 'local',
                displayField: 'name',
                valueField: 'name',
                name: 'PurchaseType',
                fieldLabel: '供应商类型',
                allowBlank: false,
                blankText: '供应商类型不能为空!',
                msgTarget: 'under', labelAlign: 'right',
                margin: '5'
            })
            var formPanel = Ext.create("Ext.form.Panel", {
                region: 'center',
                layout: { type: 'table', columns: 2
                },
                defaults: { labelAlign: 'right', xtype: 'textfield', msgTarget: 'under', margin: '5', width: 350 },
                items: [
                    { fieldLabel: '采购编号', name: 'PurchaseOrderNo', allowBlank: false, blankText: '采购编号不能为空' },
                     combo_supplier,
                    { fieldLabel: '交易币种', name: 'Symbo', id: 'Symbo', labelAlign: 'right', readOnly: true },
                    { xtype: 'numberfield', fieldLabel: '金额合计', id: 'PurchaseOrderAmount', name: 'PurchaseOrderAmount', readOnly: true },
                    { fieldLabel: '运货方式', name: 'TransportationMode' },
                    { fieldLabel: '订单号', name: 'SupplierBillNo' }, combo_PurchaseType,
                    { xtype: 'textarea', fieldLabel: '备注', name: 'Remark', colspan: 2, height: 40, width: 710 },
                    { name: 'Id', hidden: true },
                    { id: 'SupplierId', name: 'SupplierId', hidden: true }
                ],
                buttons: [
                { text: "保 存", hidden: getQueryString('op') == 'v', handler: function () {
                    if (formPanel.getForm().isValid()) {
                        var detaildata = Ext.encode(Ext.pluck(grid_detail.store.data.items, 'data'));
                        var action = id ? "update" : "create";
                        Ext.Ajax.request({
                            url: 'QiTaChanPin_Purchase.aspx',
                            params: { action: action, id: id, formdata: Ext.encode(formPanel.getForm().getValues()), detaildata: detaildata },
                            success: function (response, opts) {
                                Ext.MessageBox.alert('提示', '保存成功!');
                                if (window.opener && window.opener.store) {
                                    window.opener.store.load();
                                }
                                window.close();
                            }
                        })
                    }
                }
                }],
                buttonAlign: "center"
            });

            Ext.regModel('PurchaseDetail', { fields: [{ name: 'Id' }, { name: 'ProductId' }, { name: 'Code' }, { name: 'PCN' },
            { name: 'Amount', type: 'float' }, { name: 'Quantity', type: 'int' }, { name: 'BuyPrice', type: 'float' }, { name: 'Name'}]
            })
            store_detail = Ext.create('Ext.data.JsonStore', {
                model: 'PurchaseDetail',
                proxy: {
                    url: 'QiTaChanPin_Purchase.aspx',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'innerrows'
                    }
                }
            })
            var toolbar_detail = Ext.create('Ext.toolbar.Toolbar', {
                items: [{ text: '添加', icon: '/images/shared/add.gif', handler: function () {
                    if (!combo_supplier.getValue() || !combo_PurchaseType.getValue()) {
                        Ext.MessageBox.alert('提示', '请先选择供应商和供应商类型！');
                        return;
                    }
                    ini_win_purchaseproductselect(combo_supplier.getValue());
                }
                }, { text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                    var recs = grid_detail.getSelectionModel().getSelection();
                    store_detail.remove(recs);
                    Ext.getCmp('PurchaseOrderAmount').setValue(store_detail.sum("Amount"));
                }
                }]
            })
            var editor_detail = Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                listeners: {
                    beforeedit: function (editor, e, eOpts) {
                        if (combo_PurchaseType.getValue() == "生产商" && e.field == "BuyPrice") {
                            return false;
                        }
                    },
                    edit: function (editor, e) {
                        if (e.record.get("BuyPrice") && e.record.get("Quantity")) {
                            var amount = Math.round(parseFloat(e.record.get("BuyPrice")) * parseInt(e.record.get("Quantity")) * 100) / 100;
                            e.record.set("Amount", amount);
                            var total = store_detail.sum("Amount");
                            Ext.getCmp('PurchaseOrderAmount').setValue(total);
                        }
                    }
                }
            })
            var grid_detail = Ext.create('Ext.grid.Panel', {
                tbar: toolbar_detail,
                store: store_detail,
                region: 'south',
                height: 300,
                plugins: [editor_detail],
                columns: [
                    { xtype: 'rownumberer', width: 25 },
                    { header: '产品名称', dataIndex: 'Name', width: 100 },
                    { header: '型号', dataIndex: 'Code', width: 140 },
                    { header: 'PCN', dataIndex: 'PCN', width: 140 },
                    { header: '单价', dataIndex: 'BuyPrice', width: 80, editor: { xtype: 'numberfield', minValue: 1, allowBlank: false} },
                    { dataIndex: 'Quantity', header: '数量', width: 60, editor: { xtype: 'numberfield', minValue: 1, allowBlank: false} },
		            { dataIndex: 'Amount', header: '金额', width: 110 }
                    ]
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [formPanel, grid_detail]
            })
            if (id) {
                Ext.Ajax.request({
                    url: 'QiTaChanPin_Purchase.aspx?action=loadform&id=' + id,
                    success: function (response, opts) {
                        var json = Ext.decode(response.responseText);
                        formPanel.getForm().setValues(json.data);
                        store_detail.load({ params: { id: id, action: 'loaddetail'} });
                    }
                })
            }
        })
        function ini_win_purchaseproductselect(supplierid) {
            var store_product = Ext.create('Ext.data.JsonStore', {
                fields: ['ProductId', 'Name', 'Code', 'PCN', 'StockQuantity', 'BuyPrice'],
                model: 'Product',
                proxy: {
                    url: 'QiTaChanPin_Purchase.aspx?action=loadproduct',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            })
            var toolbar_product = Ext.create('Ext.toolbar.Toolbar', {
                items: [
                { xtype: 'textfield', fieldLabel: '产品型号', labelAlign: 'right', labelWidth: 60, width: 170, id: 'ProductCode_s' },
                { xtype: 'textfield', fieldLabel: 'PCN', labelAlign: 'right', labelWidth: 60, width: 170, id: 'ProductPcn_s' },
                { text: '查询', icon: '/images/shared/search_show.gif', handler: function () {
                    store_product.load({ params: { ProductCode: Ext.getCmp('ProductCode_s').getValue(), ProductPcn: Ext.getCmp('ProductPcn_s').getValue()} })
                }
                }]
            })
            var grid_product = Ext.create('Ext.grid.Panel', {
                store: store_product,
                tbar: toolbar_product,
                region: 'center',
                selModel: { selType: 'checkboxmodel' },
                columns: [
                    { xtype: 'rownumberer', width: 40 },
                    { header: '产品名称', dataIndex: 'Name', width: 100 },
                    { header: '型号', dataIndex: 'Code', flex: 1 },
                    { header: 'PCN', dataIndex: 'PCN', width: 140 },
                    { header: '采购价', dataIndex: 'BuyPrice', width: 100 },
                    { dataIndex: 'StockQuantity', header: '库存', width: 60 }
                    ]
            })
            var win_product = Ext.create('Ext.window.Window', {
                title: '产品选择',
                width: 550,
                height: 400,
                layout: 'border',
                items: [grid_product],
                buttons: [{ text: '确定', handler: function () {
                    var recs = grid_product.getSelectionModel().getSelection();
                    if (!recs || recs.length <= 0) {
                        Ext.Msg.alert("提示", "请选择要采购的产品!");
                        return;
                    }
                    Ext.each(recs, function (rec) {
                        if (store_detail.find('ProductId', rec.get("ProductId")) == -1) {
                            var pd = new PurchaseDetail({ ProductId: rec.get("ProductId"), Name: rec.get("Name"),
                                Code: rec.get("Code"), BuyPrice: rec.get("BuyPrice"), PCN: rec.get("PCN")
                            });
                            store_detail.insert(store_detail.data.length, pd);
                        }
                    })
                    win_product.close();
                }
                }],
                buttonAlign: 'center'
            })
            win_product.show();
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
