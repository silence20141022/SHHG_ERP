<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewHome.aspx.cs" Inherits="Aim.Examining.Web.NewHome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="/Extjs42/RowExpander.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            var store_customer = Ext.create('Ext.data.JsonStore', {
                fields: ['CustomerId', 'CustomerName'],
                proxy: {
                    url: 'NewHome.aspx?action=loadcustomer',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                }
            })
            var combo_customer = Ext.create('Ext.form.field.ComboBox', {
                store: store_customer,
                fieldLabel: '客户名称',
                labelAlign: 'right',
                labelWidth: 60,
                valueField: 'CustomerId',
                displayField: 'CustomerName',
                minChars: 1,
                width: 280,
                queryParam: 'CustomerName',
                hideTrigger: true
            })
            var store_fuzeren = Ext.create('Ext.data.JsonStore', {
                fields: ['UserId', 'UserName'],
                proxy: {
                    url: 'NewHome.aspx?action=loadfuzeren',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                }
            })
            var combo_fuzeren = Ext.create('Ext.form.field.ComboBox', {
                store: store_fuzeren,
                fieldLabel: '负责人',
                labelAlign: 'right',
                labelWidth: 60,
                width: 140,
                valueField: 'UserId',
                displayField: 'UserName',
                minChars: 1,
                queryParam: 'UserName',
                hideTrigger: true
            })
            var toolbar1 = Ext.create('Ext.toolbar.Toolbar', {
                items: [combo_customer, combo_fuzeren,
                { text: '查 询', icon: '/images/shared/search_show.gif', handler: function () {
                    store1.load();
                }
                }]
            })
            var store1 = Ext.create('Ext.data.JsonStore', {
                fields: [{ name: 'year' }, { name: 'daikaipiao', type: 'float' }, { name: 'fapiaoyingshou', type: 'float' },
                { name: 'shoujuyingshou', type: 'float' }, { name: 'total', type: 'float'}],
                proxy: {
                    url: 'NewHome.aspx?action=loadyingshou',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store1, options) {
                    var new_params = {
                        CustomerId: combo_customer.getValue(), UserId: combo_fuzeren.getValue()
                    }
                    Ext.apply(store1.proxy.extraParams, new_params);
                }
                }
            })
            var panel1 = Ext.create("Ext.grid.Panel", {
                title: '应收款',
                store: store1,
                tbar: toolbar1,
                height: 270,
                features: [{ ftype: 'summary'}],
                plugins: [{
                    ptype: 'rowexpander',
                    rowBodyTpl: ['<div id="{year}"></div>']
                }],
                columnWidth: .4,
                columns: [
                { header: '所属年份', dataIndex: 'year', width: 70 },
                { header: '待开票', dataIndex: 'daikaipiao', width: 110, summaryType: 'sum', xtype: 'numbercolumn', format: '0,000'
                    //summaryRenderer: function (v, params, data) { retrunv},
                    //                                        renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    //                                            if (value)
                    //                                                return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                    //                                        }
                },
                { header: '发票应收', dataIndex: 'fapiaoyingshou', width: 110, summaryType: 'sum', xtype: 'numbercolumn', format: '0,000'
                    //                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    //                    if (value);
                    //                    return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                    //                }
                },
                { header: '收据应收', dataIndex: 'shoujuyingshou', width: 110, summaryType: 'sum', xtype: 'numbercolumn', format: '0,000' },
                { header: '合计', dataIndex: 'total', flex: 1, summaryType: 'sum', xtype: 'numbercolumn', format: '0,000' }
                ],
                listeners: { 'cellclick': function (panel1, td, cellIndex, record, tr, rowIndex, e, eOpts) {                   
                    if (cellIndex == 2) {
                        opencenterwin("DaiKaiPiao.aspx?year=" + record.get("year"), 900, 500);
                    }
                    if (cellIndex == 3) {
                        opencenterwin("FaPiaoYingShou.aspx?year=" + record.get("year"), 900, 500);
                    }
                    if (cellIndex ==4) {
                        opencenterwin("ShouJuYingShou.aspx?year=" + record.get("year"), 900, 500);
                    }
                }
                }
            });
            var store4 = Ext.create('Ext.data.JsonStore', {
                fields: ['year', 'daikaipiao', 'fapiaoyingshou'],
                proxy: {
                    url: 'NewHome.aspx?action=loadyingshou_fengongsi',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store4, options) {
                    //                var new_params = {
                    //                    CustomerId: combo_customer.getValue(), UserId: combo_fuzeren.getValue()
                    //                }
                    //                Ext.apply(store1.proxy.extraParams, new_params);
                }
                }
            })
            var panel4 = Ext.create("Ext.grid.Panel", {
                title: '应收款_分公司',
                store: store4,
                //  tbar: toolbar1,
                height: 200,
                plugins: [{
                    ptype: 'rowexpander',
                    rowBodyTpl: ['<div id="f{year}"></div>']
                }],
                columnWidth: .4,
                columns: [
                { header: '所属年份', dataIndex: 'year', width: 80 },
                { header: '待开票', dataIndex: 'daikaipiao', width: 120, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    if (value)
                        return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                }
                },
                { header: '发票应收', dataIndex: 'fapiaoyingshou', width: 120, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    if (value);
                    return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                }
                }
                ]
            });
            var store_supplier = Ext.create('Ext.data.JsonStore', {
                fields: ['SupplierId', 'SupplierName'],
                proxy: {
                    url: 'NewHome.aspx?action=loadsupplier',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                }
            })
            var combo_supplier = Ext.create('Ext.form.field.ComboBox', {
                store: store_supplier,
                fieldLabel: '供应商',
                labelAlign: 'right',
                labelWidth: 60,
                valueField: 'SupplierId',
                displayField: 'SupplierName',
                minChars: 1,
                width: 280,
                queryParam: 'SupplierName',
                hideTrigger: true
            })
            var toolbar2 = Ext.create('Ext.toolbar.Toolbar', {
                items: [combo_supplier, { text: '查 询', icon: '/images/shared/search_show.gif', handler: function () {
                    store2.load();
                }
                }]
            })
            var store2 = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'InWarehouseNo', 'SupplierName', 'State', 'InWarehouseType', 'CreateName', 'CreateTime'],
                proxy: {
                    url: 'NewHome.aspx?action=loadweiruku',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'data'
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store2, options) {
                    var new_params = {
                        SupplierId: combo_supplier.getValue()
                    }
                    Ext.apply(store2.proxy.extraParams, new_params);
                }
                }
            })
            var panel2 = Ext.create("Ext.grid.Panel", {
                title: '待入库',
                tbar: toolbar2,
                height: 270,
                columnWidth: .6,
                store: store2,
                selModel: { selType: 'checkboxmodel' },
                columns: [
                    { xtype: 'rownumberer', width: 25 },
                    { dataIndex: 'InWarehouseNo', header: '入库编号', width: 120 },
                   	{ dataIndex: 'SupplierName', header: '供应商', flex: 1 },
                   	{ dataIndex: 'InWarehouseType', header: '入库类型', width: 70 },
                    { dataIndex: 'CreateName', header: '创建人 ', width: 60 },
                    { dataIndex: 'CreateTime', header: '创建时间 ', width: 130}]
            });
            var store3 = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Title', 'CreatedTime'],
                proxy: {
                    url: 'NewHome.aspx?action=loadtask',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store3, options) {
                    //                    var new_params = {
                    //                        SupplierId: combo_supplier.getValue()
                    //                    }
                    //                    Ext.apply(store3.proxy.extraParams, new_params);
                }
                }
            })
            var panel3 = Ext.create("Ext.grid.Panel", {
                title: '待审批',
                height: 200,
                columnWidth: .4,
                store: store3,
                selModel: { selType: 'checkboxmodel' },
                columns: [
                    { xtype: 'rownumberer', width: 25 },
                    { dataIndex: 'Title', header: '任务名称', flex: 1, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                    }
                    },
                    { dataIndex: 'CreatedTime', header: '创建时间 ', width: 130}],
                listeners: { cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    if (cellIndex == 2) {
                        opencenterwin("/WorkFlow/TaskExecute.aspx?TaskId=" + record.get("Id"), 1000, 550);
                    }
                }
                }
            });
            panel1.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                displayInnerGrid(record.get('year'));
            });
            panel1.view.on('collapsebody', function (rowNode, record, expandRow, eOpts) {
                destroyInnerGrid(record.get("year"));
            });
            panel4.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                displayInnerGrid_f(record.get('year'));
            });
            panel4.view.on('collapsebody', function (rowNode, record, expandRow, eOpts) {
                destroyInnerGrid_f(record.get("year"));
            });

            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'column',
                items: [
                { layout: 'column', columnWidth: 1, border: 0, items: [panel1, panel2] },
                { layout: 'column', columnWidth: 1, border: 0, items: [panel4, panel3] }
            ]
            })
        })
        function displayInnerGrid(div) {
            var store_inner = Ext.create('Ext.data.JsonStore', {
                fields: ['month', 'daikaipiao', 'fapiaoyingshou', 'shoujuyingshou'],
                proxy: {
                    url: 'NewHome.aspx?action=loaddetail&year=' + div,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'innerrows'
                    }
                },
                autoLoad: true
            })
            var grid_inner = Ext.create('Ext.grid.Panel', {
                store: store_inner,
                columnLines: true,
                margin: '0 0 0 30',
                columns: [
                    { header: '月份', dataIndex: 'month', width: 90 },
                    { header: '待开票', dataIndex: 'daikaipiao', width: 110 },
                    { header: '发票应收', dataIndex: 'fapiaoyingshou', width: 110 },
                    { header: '收据应收', dataIndex: 'shoujuyingshou', width: 110 }
                    ],
                renderTo: div
            })
        }
        function destroyInnerGrid(div) {
            var parent = document.getElementById(div);
            var child = parent.firstChild;
            while (child) {
                child.parentNode.removeChild(child);
                child = child.nextSibling;
            }
        }
        function displayInnerGrid_f(div) {
            var store_inner = Ext.create('Ext.data.JsonStore', {
                fields: ['month', 'daikaipiao', 'fapiaoyingshou'],
                proxy: {
                    url: 'NewHome.aspx?action=loaddetail_f&year=' + div,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'innerrows'
                    }
                },
                autoLoad: true
            })
            var grid_inner = Ext.create('Ext.grid.Panel', {
                store: store_inner,
                columnLines: true,
                margin: '0 0 0 30',
                columns: [
                    { header: '月份', dataIndex: 'month', width: 90 },
                    { header: '待开票', dataIndex: 'daikaipiao', width: 110 },
                    { header: '发票应收', dataIndex: 'fapiaoyingshou', width: 110}],
                renderTo: 'f' + div
            })
        }
        function destroyInnerGrid_f(div) {
            var parent = document.getElementById('f' + div);
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
