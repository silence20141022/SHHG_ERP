<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FenGongSi_Order_Edit.aspx.cs"
    Inherits="Aim.Examining.Web.SaleManagement.FenGongSi_Order_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/ext-theme-classic/ext-theme-classic-all.css" rel="stylesheet"
        type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="/Extjs42/locale/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var store, win_orderpart;
        var id = getQueryString("id");
        var op = getQueryString("op");
        Ext.onReady(function () {
            var Number = Ext.create('Ext.form.field.Text', {
                name: 'Number',
                readOnly: true,
                margin: '10',
                columnWidth: .5,
                fieldLabel: '销售单号',
                labelAlign: 'right',
                emptyText: '自动生成'
            });
            var store_CName = Ext.create('Ext.data.JsonStore', {
                fields: ['name', 'id'],
                proxy: {
                    url: 'FenGongSi_Order_Edit.aspx?action=loadcustomer',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                }
            })
            var combo_CustomerName = Ext.create('Ext.form.field.ComboBox', {
                name: 'CustomerName',
                store: store_CName,
                allowBlank: false,
                forceSelection: !id,
              //  blankText: '客户名称不能为空',
                queryParam: 'name',
                emptyText: '请输入客户名称',
                msgTarget: 'under',
                hideTrigger: true,
                minChars: 1,
                displayField: 'name',
                valueField: 'name',
                margin: '10',
                columnWidth: .5,
                fieldLabel: '客户名称',
                labelAlign: 'right',
                listeners: { select: function (picker, record, eOpts) {
                    Ext.getCmp("CustomerId").setValue(record[0].get("id"));
                }
                }
            });
            var store_InvoiceType = Ext.create("Ext.data.JsonStore", {
                fields: ["name"],
                proxy: {
                    url: 'FenGongSi_Order_Edit.aspx?action=loadinvoicetype',
                    type: "ajax",
                    reader: {
                        type: "json",
                        root: "rows"
                    }
                },
                autoLoad: true
            });
            var combo_InvoiceType = Ext.create('Ext.form.field.ComboBox', {
                name: 'InvoiceType',
                store: store_InvoiceType,
                displayField: 'name',
                valueField: 'name',
                margin: '10',
                columnWidth: .5,
                fieldLabel: '开票类型',
                labelAlign: 'right'
            });
            var store_PayType = Ext.create('Ext.data.JsonStore', {
                fields: ['name'],
                proxy: {
                    url: "FenGongSi_Order_Edit.aspx?action=loadpaytype",
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            })
            var combo_PayType = Ext.create('Ext.form.field.ComboBox', {
                name: 'PayType',
                store: store_PayType,
                displayField: 'name',
                valueField: 'name',
                margin: '10',
                columnWidth: .5,
                fieldLabel: '支付方式',
                labelAlign: 'right'
            });
            var form = Ext.create('Ext.form.Panel', {
                region: 'center',
                items: [
                { layout: 'column', border: 0, items: [Number,
                { xtype: 'textfield', fieldLabel: '分公司名称', labelAlign: 'right', name: 'FenGongSiName', columnWidth: .5, readOnly: true, margin: '10', allowBlank: false, msgTarget: 'under'}]
                },
                { layout: 'column', border: 0, items: [combo_CustomerName, combo_InvoiceType] },
                { layout: 'column', border: 0, items: [combo_PayType,
                { xtype: 'numberfield', name: 'TotalMoney', columnWidth: .5, id: 'TotalMoney', fieldLabel: '总金额', margin: '10', labelAlign: 'right', readOnly: true }
              ]
                },
                { layout: 'column', border: 0, items: [
                { xtype: 'numberfield', name: 'DiscountAmount', columnWidth: .5, fieldLabel: '折扣金额(￥)', margin: '10', labelAlign: 'right' },
                { xtype: 'numberfield', name: 'ShouKuanAmount', columnWidth: .5, fieldLabel: '实际收款金额(￥)', margin: '10', labelAlign: 'right' }
                 ]
                },
                 { layout: 'column', border: 0, items: [
                 { xtype: 'datefield', name: 'ShouKuanDate', format: 'Y-m-d', id: 'ShouKuanDate', columnWidth: .5, fieldLabel: '收款日期', margin: '10', labelAlign: 'right'}]
                 },
                { layout: 'column', border: 0, items: [
                { xtype: 'textareafield', name: 'Remark', margin: '10', height: 40, columnWidth: 1, fieldLabel: '备注', labelAlign: 'right'}]
                },
                { xtype: "textfield", name: "Id", id: "Id", hidden: true },
                { xtype: "textfield", name: "CustomerId", id: "CustomerId", hidden: true },
                { xtype: "textfield", name: "FenGongSiId", id: "FenGongSiId", hidden: true }
            ],
                buttons: [
                { text: '保 存', hidden: op == 'v', handler: function () {
                    if (form.getForm().isValid()) {
                        var json = Ext.encode(Ext.pluck(store.data.items, 'data'));
                        var data = Ext.encode(form.getForm().getValues());
                        var action = id ? "update" : "create";
                        Ext.Ajax.request({
                            url: "FenGongSi_Order_Edit.aspx",
                            params: { action: action, data: data, json: json, id: id },
                            callback: function (option, success, response) {
                                if (window.opener && window.opener.store) {
                                    window.opener.store.reload();
                                }
                                window.close();
                            }
                        });
                    }
                }
                }
                ],
                buttonAlign: 'center'
            });
            Ext.regModel('OrderPart', { fields: [{ name: 'Id' }, { name: 'ProductId' }, { name: 'OrderPart_Id' }, { name: 'Name' },
                { name: 'Code' }, { name: 'SecondPrice', type: 'float' }, { name: 'PurchasePrice', type: 'float' }, { name: 'Quantity', type: 'int' },
                { name: 'Amount', type: 'float' }, { name: 'Remark' }, { name: 'MaxQuan', type: 'int' }
                ]
            })
            store = Ext.create('Ext.data.JsonStore', {
                model: 'OrderPart',
                proxy: {
                    type: 'ajax',
                    url: "FenGongSi_Order_Edit.aspx?action=loaddetail&id=" + id,
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            });
            var toolbar = Ext.create('Ext.toolbar.Toolbar', {
                items: [{ text: '添加', icon: '/images/shared/add.png', handler: function () {
                    win_orderpart.show();
                }
                }, { text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                    var recs = grid.getSelectionModel().getSelection();
                    if (!recs || recs.length <= 0) {
                        Ext.Msg.alert('提示', '请先选择要删除的记录！');
                        return;
                    }
                    store.remove(recs);
                }
                }]
            })
            var editor = Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1, //设置单击单元格编辑  
                listeners: { beforeedit: function (editor, e, eOpts) {
                    if (e.field == "Quantity") {
                        Ext.getCmp("nf1").setMaxValue(parseInt(e.record.get("MaxQuan")));
                    }
                }, edit: function (editor, e) {
                    if (e.record.get('SecondPrice') && e.record.get('Quantity')) {
                        var amount = parseFloat(e.record.get("SecondPrice")) * parseFloat(e.record.get("Quantity"));
                        e.record.set("Amount", amount);
                        Ext.getCmp("TotalMoney").setValue(store.sum('Amount'));
                    }
                }
                }
            })
            var grid = Ext.create('Ext.grid.Panel', {
                region: 'south',
                height: 250,
                store: store,
                tbar: toolbar,
                selModel: { selType: 'checkboxmodel' },
                plugins: [editor],
                columns: [
                            { text: 'Id', dataIndex: 'Id', hidden: true },
                            { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                            { id: 'OrderPart_Id', dataIndex: 'OrderPart_Id', hidden: true },
                            { text: 'Name', header: '产品名称', dataIndex: 'Name', width: 120 },
                            { text: 'Code', header: '产品型号', dataIndex: 'Code', width: 150 },
                            { dataIndex: 'PurchasePrice', width: 80, header: '采购价格' },
                            { dataIndex: 'SecondPrice', width: 80, header: '销售价格', field: { xtype: 'numberfield', allowBlank: false} },
                            { dataIndex: 'Quantity', width: 80, header: '数量', field: { id: 'nf1', xtype: 'numberfield', allowBlank: false, minValue: 1} },
                            { dataIndex: 'Amount', width: 80, header: '金额' },
                            { dataIndex: 'Remark', header: '备注', flex: 1, field: { xtype: 'textarea', allowBlank: false} },
                            { dataIndex: 'MaxQuan', hidden: true }
                          ]
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [form, grid]
            })
            Ext.Ajax.request({
                url: 'FenGongSi_Order_Edit.aspx?action=loadform&id=' + id,
                success: function (response, opts) {
                    var json = Ext.decode(response.responseText);
                    form.getForm().setValues(json.data);
                    if (json.data.ShouKuanDate) {
                        var str = new Date(json.data.ShouKuanDate);
                        Ext.getCmp("ShouKuanDate").setValue(str);
                    }

                    IniWin();
                }
            })
        })
        function IniWin() {
            var store_orderpart = new Ext.data.JsonStore({
                fields: ['Id', 'Number', 'PId', 'PCode', 'PName', 'SalePrice', 'SaleQuan', 'Count'],
                proxy: {
                    type: 'ajax',
                    url: 'FenGongSi_Order_Edit.aspx?action=loadorderpart',
                    reader: {
                        reader: 'json',
                        totalProperty: 'total',
                        root: 'rows'
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store, options) {
                    var new_params = {
                        fengongsiid: Ext.getCmp('FenGongSiId').getValue(),
                        ProductCode: Ext.getCmp('ProductCode').getValue()
                    }
                    Ext.apply(store_orderpart.proxy.extraParams, new_params);
                }
                }
            });
            var toolbar_orderpart = Ext.create('Ext.toolbar.Toolbar', {
                items: [
                { xtype: 'textfield', fieldLabel: '产品型号', labelWidth: 60, labelAlign: 'right', id: 'ProductCode' },
                { xtype: 'button', text: '查询', icon: '/images/shared/search_show.gif', handler: function () {
                    store_orderpart.load();
                }
                }
            ]
            });
            var pagebar_orderpart = Ext.create('Ext.toolbar.Paging', {
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                store: store_orderpart,
                displayInfo: true
            })
            var grid_orderpart = Ext.create('Ext.grid.Panel', {
                store: store_orderpart,
                tbar: toolbar_orderpart,
                bbar: pagebar_orderpart,
                region: 'center',
                selModel: { selType: 'checkboxmodel' },
                columns: [
                { header: '标识', dataIndex: 'Id', hidden: true },
                { header: '销售单号', dataIndex: 'Number', width: 110 },
                { header: '产品名称', dataIndex: 'PName', width: 90 },
                { header: '产品型号', dataIndex: 'PCode', flex: 1 },
                { header: '采购价', dataIndex: 'SalePrice', width: 70 },
                { header: '总数量', dataIndex: 'Count', width: 70 },
                { header: '已销售数量', dataIndex: 'SaleQuan', width: 80 }
                ]
            });
            win_orderpart = Ext.create('Ext.window.Window', {
                height: 400,
                width: 600,
                title: '产品选择',
                layout: 'border',
                closeAction: 'hide',
                items: [grid_orderpart],
                buttonAlign: 'center',
                buttons: [{ text: '确 定', handler: function () {
                    var recs = grid_orderpart.getSelectionModel().getSelection();
                    Ext.each(recs, function (rec) {
                        if (store.find("OrderPart_Id", rec.get("Id")) == -1) {
                            var orderpart = new OrderPart({ ProductId: rec.get("PId"), Name: rec.get("PName"),
                                Code: rec.get("PCode"), OrderPart_Id: rec.get("Id"), PurchasePrice: rec.get("SalePrice"),
                                Quantity: parseInt(rec.get("Count")) - parseInt(rec.get("SaleQuan")), MaxQuan: parseInt(rec.get("Count")) - parseInt(rec.get("SaleQuan"))
                            })
                            store.insert(store.data.length, orderpart);
                        }
                    })
                    win_orderpart.close();
                }
                }
                ]
            })
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
