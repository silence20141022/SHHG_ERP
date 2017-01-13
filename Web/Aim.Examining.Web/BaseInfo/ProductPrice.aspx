<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPrice.aspx.cs" Inherits="Aim.Examining.Web.ProductPrice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Extjs42/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            var store_producttype = Ext.create('Ext.data.JsonStore', {
                fields: ['name'],
                proxy: {
                    url: 'ProductPrice.aspx?action=loadproducttype',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                }
            })
            var combo_producttype = Ext.create('Ext.form.field.ComboBox', {
                store: store_producttype,
                displayField: 'name',
                valueField: 'name',
                fieldLabel: '产品类型',
                labelAlign: 'right'
            })
            var toolbar = Ext.create('Ext.toolbar.Toolbar', {
                items: [{ xtype: 'textfield', fieldLabel: '产品名称|型号|PCN', labelWidth: 140, labelAlign: 'right', id: 'Product_s', width: 300 },
                combo_producttype,
                { text: '查 询', icon: '/images/shared/preview.png', handler: function () {
                    store.load({ params: { Product: Ext.getCmp("Product_s").getValue(),
                        ProductType: combo_producttype.getValue(), start: 0
                    }
                    });
                }
                }
                ]
            })
            var store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Code', 'Name', 'Pcn', 'CreateTime', 'SalePrice', 'ProductType', 'CreateName', 'SupplierName', 'Symbo','BuyPrice'],
                proxy: {
                    url: 'ProductPrice.aspx?action=select',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            })
            var pagebar = Ext.create('Ext.toolbar.Paging', {
                store: store,
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                emptyMsg: "没有数据",
                beforePageText: "当前页",
                afterPageText: "共{0}页",
                displayInfo: true
            })
            var editor_price = Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 2,
                listeners: {
                    edit: function (editor, e, eOpts) {
                        Ext.Ajax.request({
                            url: 'ProductPrice.aspx?action=updateprice',
                            params: { productid: e.record.get("Id"), BuyPrice: e.value },
                            success: function () {
                            }
                        });
                    }
                }
            })
            var grid = Ext.create('Ext.grid.Panel', {
                title: '价格管理',
                store: store,
                columnLines: true,
                selModel: { selType: 'checkboxmodel' },
                plugins: [editor_price],
                region: 'center',
                tbar: toolbar,
                columns: [
                { xtype: 'rownumberer', width: 40 },
                { header: '产品名称', dataIndex: 'Name', width: 150 },
                { header: '产品型号', dataIndex: 'Code', flex: 1 },
                { header: 'PCN', dataIndex: 'Pcn', width: 140 },
                { header: '产品类型', dataIndex: 'ProductType', width: 80 },
                { header: '币种', dataIndex: 'Symbo', width: 50, align: 'right' },
                { header: '<font color="red">采购价格</font>', dataIndex: 'BuyPrice', width: 100,
                    editor: { xtype: 'numberfield', minValue: 0, allowBlank: false, decimalPrecision: 2 }
                },
                { header: '供应商', dataIndex: 'SupplierName', width: 160 },
                { header: '录入日期', dataIndex: 'CreateTime', width: 130 },
                { header: '录入人', dataIndex: 'CreateName', width: 80 }
                ],
                bbar: pagebar
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [grid]
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
