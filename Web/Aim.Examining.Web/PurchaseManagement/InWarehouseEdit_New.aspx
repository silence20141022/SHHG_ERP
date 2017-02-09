<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InWarehouseEdit_New.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.InWarehouseEdit_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var PurchaseOrderIds = getQueryString("PurchaseOrderIds");
        var id = getQueryString("id");
        var op = getQueryString("op");
        Ext.onReady(function () {
            var store_warehouse = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Name'],
                proxy: {
                    url: 'InWarehouseEdit_New.aspx?action=loadwarehouse',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            })
            var combo_warehouse = Ext.create('Ext.form.field.ComboBox', {
                store: store_warehouse,
                fieldLabel: '入库仓库',
                labelAlign: 'right',
                allowBlank: false,
                msgTarget: 'under',
                blankText: '入库仓库不能为空!',
                name: 'WarehouseName',
                displayField: 'Name',
                valueField: 'Name',
                listeners: {
                    select: function (combo, records, eOpts) {
                        Ext.getCmp("WarehouseId").setValue(records[0].get("Id"));
                    }
                }
            })
            var formpanel = Ext.create('Ext.form.Panel', {
                title: '入库单',
                region: 'center',
                layout: { type: 'table', columns: 2 },
                defaults: { labelAlign: 'right', msgTarget: 'under', margin: '5', xtype: 'textfield', width: 300 },
                items: [
                 { fieldLabel: '入库单编号', name: 'InWarehouseNo', readOnly: true },
                 { fieldLabel: '入库类型', name: 'InWarehouseType', readOnly: true },
                 { fieldLabel: '供应商名称', name: 'SupplierName', readOnly: true, colspan: 2, width: 610 },
                 { fieldLabel: '预计到货时间', xtype: 'datefield', name: 'EstimatedArrivalDate', format: 'Y-m-d', id: 'EstimatedArrivalDate' },
                 combo_warehouse,
                 { xtype: 'textarea', fieldLabel: '备注', colspan: 2, name: 'Remark', width: 610, height: 60 },
                 { name: 'WarehouseId', hidden: true, id: 'WarehouseId' },
                 { name: 'SupplierId', hidden: true }
                ],
                buttons: [{
                    text: '保 存', hidden: op == 'view', handler: function () {
                        if (formpanel.getForm().isValid()) {
                            if (store_detail.data.length == 0) {
                                Ext.MessageBox.alert('提示', '入库单明细不能为空!');
                                return;
                            }
                            var isrepeate = false;
                            //2017-2-9增加验证,入库单明细不能有相同的型号存在 by panhuaguo
                            store_detail.each(function (record) {
                                var recs = store_detail.query('ProductCode', record.get('ProductCode'))
                                if (recs.length>1) {
                                    isrepeate = true;
                                }
                            })
                            if (isrepeate)
                            {
                                Ext.MessageBox.alert('提示', '入库单明细存在相同的产品型号!');
                                return;
                            }
                            var detaildata = Ext.encode(Ext.pluck(grid_detail.store.data.items, 'data'));
                            var action = PurchaseOrderIds ? "create" : "update";
                            Ext.Ajax.request({
                                url: 'InWarehouseEdit_New.aspx',
                                params: { action: action, id: id, formdata: Ext.encode(formpanel.getForm().getValues()), detaildata: detaildata },
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
            var store_detail = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'InWarehouseId', 'PurchaseOrderNo', 'ProductId', 'ProductCode', 'PurchaseOrderDetailId', 'IQuantity', 'InWarehouseState',
                 'SkinArray', 'SeriesArray', 'Remark', 'YiRuQuan', 'MaxQuan'],
                proxy: {
                    url: 'InWarehouseEdit_New.aspx?action=loaddetail&PurchaseOrderIds=' + PurchaseOrderIds + "&id=" + id,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            })
            var toolbar_detail = Ext.create('Ext.toolbar.Toolbar', {
                items: [
                {
                    text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                        var recs = grid_detail.getSelectionModel().getSelection();
                        store_detail.remove(recs);
                    }
                }]
            })
            var editor_detail = Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                listeners: {
                    beforeedit: function (editor, e, eOpts) {
                        if (e.field == "IQuantity") {
                            Ext.getCmp("nf1").setMaxValue(parseFloat(e.record.get("MaxQuan")));
                        }
                    }
                }
            })
            var grid_detail = Ext.create('Ext.grid.Panel', {
                title: '入库单明细',
                tbar: toolbar_detail,
                store: store_detail,
                region: 'south',
                height: 300,
                plugins: [editor_detail],
                columns: [
                    { xtype: 'rownumberer', width: 25 },
                    { header: '采购编号', dataIndex: 'PurchaseOrderNo', width: 110 },
                    { header: '型号', dataIndex: 'ProductCode', width: 140 },
                    {
                        dataIndex: 'IQuantity', header: '数量', width: 60, editor: {
                            xtype: 'numberfield', id: 'nf1', minValue: 1, allowBlank: false,
                            maxText: '入库数量的最大值是{0}', msgTarget: 'under'
                        }
                    },
		            { dataIndex: 'Remark', header: '备注', flex: 1, editor: { xtype: 'textarea' } }
                ]
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [formpanel, grid_detail]
            })
            Ext.Ajax.request({
                url: 'InWarehouseEdit_New.aspx?action=loadform',
                params: { PurchaseOrderIds: PurchaseOrderIds, id: id },
                success: function (response, opts) {
                    var json = Ext.decode(response.responseText);
                    formpanel.getForm().setValues(json.data);
                    if (json.data.EstimatedArrivalDate) {
                        var str = new Date(json.data.EstimatedArrivalDate);
                        Ext.getCmp("EstimatedArrivalDate").setValue(str);
                    }
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
