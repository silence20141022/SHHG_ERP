<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleOrder_List.aspx.cs"
    Inherits="Aim.Examining.Web.SaleManagement.SaleOrder_List" %>

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
        Ext.onReady(function () {
           var store = Ext.create('Ext.data.JsonStore', {
                pageSize: 25,
                fields: ['Id', 'Number', 'CId', 'CName', 'Remark', 'InvoiceType', 'PayType',
                'TotalMoney', 'SalesmanId', 'Salesman', 'CreateName', 'CreateTime'],
                proxy: {
                    url: 'SaleOrder_List.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: "total"
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store, options) {
                    var new_params = {
                        Number: Ext.getCmp('Number_s').getValue(),
                        ProductCode: Ext.getCmp('ProductCode_s').getValue()
                    }
                    Ext.apply(store.proxy.extraParams, new_params);
                }
                }
            });
            var toolbar = Ext.create('Ext.toolbar.Toolbar', {
                items: [
                 { xtype: "textfield", fieldLabel: "销售单号", labelWidth: 60, id: "Number_s", labelAlign: 'right' },
                 { xtype: "textfield", fieldLabel: "产品型号", id: "ProductCode_s", labelAlign: 'right', labelWidth: 60 },
                 { text: '查询', icon: '/images/shared/search_show.gif', handler: function () {
                     store.load();
                 }
                 }  
            ]
            });
            var pagebar = Ext.create('Ext.toolbar.Paging', {
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                store: store,
                displayInfo: true
            })
            var grid = Ext.create('Ext.grid.Panel', {
                tbar: toolbar,
                store: store,
                title: '销售管理',
                region: 'center',
                bbar: pagebar,
                selModel: { selType: 'checkboxmodel' },
                columns: [
                { xtype: 'rownumberer', width: 35 },
                { dataIndex: "Id", header: "标示", hidden: true },
                { dataIndex: "Number", header: "销售单号", width: 110, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                }
                },
                { dataIndex: "CName", header: "客户名称", width: 240 },
                { dataIndex: "InvoiceType", header: "票据类型", width: 80 },
                { dataIndex: "PayType", header: "支付方式", width: 80 },
                { dataIndex: "TotalMoney", header: "总金额", width: 80 },
                { dataIndex: "DiscountAmount", header: "折扣金额", width: 80 },
                { xtype: 'actioncolumn', width: 100, text: '分公司销售明细', align: 'center',
                    items: [{
                        icon: '/images/shared/details.gif',
                        handler: function (grid, rowIndex, colIndex) {
                            var rec = grid.getStore().getAt(rowIndex);
                            opencenterwin("FenGongSi_Order_Monitor.aspx?saleorderid=" + rec.get("Id"), 1200, 450);
                        }
                    }]
                },
                { dataIndex: 'CreateTime', header: '创建日期', width: 110 },
                { dataIndex: "Remark", header: "备注", flex: 1 }
                ],
                plugins: [{
                    ptype: 'rowexpander',
                    rowBodyTpl: ['<div id="{Id}"></div>']
                }]
            });
            grid.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                displayInnerGrid(record.get('Id'));
            });
            grid.view.on('collapsebody', function (rowNode, record, expandRow, eOpts) {
                destroyInnerGrid(record.get("Id"));
            });
            var viewport = new Ext.container.Viewport({
                layout: 'border',
                items: [grid]
            })
        });
        function displayInnerGrid(div) {
            var store_inner = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'PId', 'OId', 'PName', 'PCode', 'SalePrice',
                'Count', 'Amount', 'Remark', 'ReturnQuan', 'SaleQuan'],
                proxy: {
                    url: 'SaleOrder_List.aspx?action=loaddetail&id=' + div,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'innerrows'
                    }
                },
                autoLoad: true
            });
            var grid_inner = Ext.create('Ext.grid.Panel', {
                store: store_inner,
                margin: '0 0 0 75',
                columns: [
                            { xtype: 'rownumberer', width: 35 },
                            { header: '产品名称', dataIndex: 'PName', width: 110 },
                            { header: '产品型号', dataIndex: 'PCode', width: 180 },
                            { dataIndex: 'SalePrice', width: 80, header: '销售价格' },
                            { dataIndex: 'Count', width: 70, header: '数量' },
                            { dataIndex: 'Amount', width: 70, header: '金额' },
                            { dataIndex: 'SaleQuan', width: 80, header: '已售数量' },
                            { dataIndex: 'ReturnQuan', width: 80, header: '退货数量' },
                            { xtype: 'actioncolumn', width: 100, text: '分公司销售明细', align: 'center',
                                items: [{
                                    icon: '/images/shared/details.gif',
                                    handler: function (grid, rowIndex, colIndex) {
                                        var rec = grid.getStore().getAt(rowIndex);
                                        opencenterwin("FenGongSi_Order_Monitor.aspx?orderpartid=" + rec.get("Id"), 1200, 450);
                                    }
                                }]
                            },
                            { dataIndex: 'Remark', header: '备注', flex: 1 }
                          ],
                renderTo: div,
                listeners: { cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    if (cellIndex == 6) {
                        opencenterwin('FenGongSi_Order_Monitor.aspx?orderpartid=' + record.get("Id"), 1200, 450);
                    }
                }
                }
            })
            grid_inner.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                display_second(record.get('Id'));
            });
            grid_inner.view.on('collapsebody', function (rowNode, record, expandRow, eOpts) {
                destroy_second(record.get("Id"));
            });
        }
        function destroyInnerGrid(div) {
            var parent = document.getElementById(div);
            var child = parent.firstChild;
            while (child) {
                child.parentNode.removeChild(child);
                child = child.nextSibling;
            }
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
