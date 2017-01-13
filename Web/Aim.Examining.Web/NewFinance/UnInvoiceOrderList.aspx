<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnInvoiceOrderList.aspx.cs"
    Inherits="Aim.Examining.Web.NewFinance.UnInvoiceOrderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="/Extjs42/RowExpander.js" type="text/javascript"></script>
    <script src="/js/pan.js" type="text/javascript"></script>
    <link href="../font-awesome41/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            var store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Number', 'CId', 'CName', 'InvoiceType', 'TotalMoney', 'Salesman', 'Remark', 'CreateTime'],
                proxy: {
                    url: 'UnInvoiceOrderList.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: 'total'
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function () {
                    store.getProxy().extraParams = {
                        Number: Ext.getCmp('Number_s').getValue(),
                        CName: Ext.getCmp('CName_s').getValue()
                    }
                }
                }
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "订单编号", id: "Number_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "客户名称", id: "CName_s", labelAlign: 'right', labelWidth: 60 },
                //{ xtype: "textfield", fieldLabel: "产品型号", id: "PCode_s", labelAlign: 'right', labelWidth: 60 },
                   {text: '<i class="fa fa-search"></i>&nbsp;查询', handler: function () {
                       pagebar.moveFirst();
                   }
               },
                   { text: '<i class="fa fa-reply"></i>&nbsp;转至待开票', handler: function () {
                       var recs = grid.getSelectionModel().getSelection();
                       if (recs.length == 0) {
                           Ext.MessageBox.alert('提示', '请选择需要操作的记录！');
                           return;
                       }
                       Ext.MessageBox.confirm("提示", "转至待开票会将先前的收款记录清除,确定需要重新开票吗？", function (btn) {
                           if (btn == 'yes') {
                               Ext.Ajax.request({
                                   url: 'UnInvoiceOrderList.aspx?action=rtninvoice',
                                   params: { id: recs[0].get("Id") },
                                   success: function (response, option) {
                                       store.load();
                                   }
                               })
                           }
                       });
                   }
                   }]
            })
            var pagebar = Ext.create('Ext.toolbar.Paging', {
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                store: store,
                displayInfo: true
            })
            var grid = Ext.create('Ext.grid.Panel', {
                store: store,
                region: 'center',
                selModel: { selType: 'checkboxmodel' },
                title: '暂缓开票订单',
                tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                { dataIndex: 'Number', header: '订单编号', width: 120 },
                { dataIndex: 'CName', header: '客户名称', width: 250 },
				{ dataIndex: 'TotalMoney', header: '总金额', width: 90, summaryType: 'sum', summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); } },
				{ dataIndex: 'DiscountAmount', header: '折扣金额', width: 90, summaryType: 'sum', summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); } },
				{ dataIndex: 'ReturnAmount', header: '退货金额', width: 90, summaryType: 'sum', summaryRenderer: function (v, params, data) { return AmountFormat(Math.round(v * 100) / 100); } },
			    { dataIndex: 'InvoiceState', header: '已收款金额', width: 90 },
                { dataIndex: 'CreateTime', header: '订单日期', width: 110 },
                { dataIndex: 'Salesman', header: '销售负责人', width: 80 },
                { dataIndex: 'Remark', header: '备注', flex: 1}],
                bbar: pagebar,
                plugins: [{
                    ptype: 'rowexpander',
                    rowBodyTpl: ['<div id="{Id}"></div>']
                }],
                viewConfig: {
                    enableTextSelection: true
                }
            })
            grid.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                displayInnerGrid(record.get('Id'));
            });
            grid.view.on('collapsebody', function (rowNode, record, expandRow, eOpts) {
                destroyInnerGrid(record.get("Id"));
            });
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [grid]
            })
        })
        function displayInnerGrid(div) {
            var store_inner = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'PName', 'PCode', 'Count', 'SalePrice', 'Amount', 'ReturnCount'],
                proxy: {
                    url: 'UnInvoiceOrderList.aspx?action=loaddetail&id=' + div,
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
                margin: '0 0 0 70',
                columns: [
                    { xtype: 'rownumberer', width: 25 },
                    { header: '产品名称', dataIndex: 'PName', width: 140 },
                    { header: '型号', dataIndex: 'PCode', width: 150, flex: 1 },
                    { header: '单价', dataIndex: 'SalePrice', width: 80 },
                    { header: '销售数量', dataIndex: 'Count', width: 80 },
                    { header: '退货数量', dataIndex: 'ReturnCount', width: 80 },
		            { header: '金额', dataIndex: 'Amount', width: 110 }
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
    </script>
</head>
<body>
</body>
</html>
