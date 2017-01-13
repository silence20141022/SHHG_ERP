<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BadDebtList.aspx.cs" Inherits="Aim.Examining.Web.NewFinance.BadDebtList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Extjs42/resources/css/ext-all-neptune.css" rel="stylesheet" type="text/css" /> 
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <link href="../font-awesome41/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "客户名称", id: "CName_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "发票号", id: "Number_s", labelAlign: 'right', labelWidth: 60 },
                   { text: '<i class="fa fa-search" aria-hidden="true"></i>&nbsp;查询', handler: function () { 
                       pagebar.moveFirst();
                   }
                   }]
            });
            var store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Number', 'CName', 'Amount', 'Remark', 'PayAmount', 'InvoiceDate', 'BadDebtAmount'],
                proxy: {
                    url: 'BadDebtList.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: 'total'
                    }
                },
                listeners: { beforeload: function (store, options) {
                    var new_params = {
                        CName: Ext.getCmp("CName_s").getValue(),
                        Number: Ext.getCmp("Number_s").getValue()
                    }
                    Ext.apply(store.proxy.extraParams, new_params);
                }
                },
                autoLoad: true
            })
            var pagebar = Ext.create('Ext.toolbar.Paging', {
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                store: store,
                displayInfo: true
            })
            var grid = Ext.create('Ext.grid.Panel', {
                title: '坏账列表',
                tbar: toolbar,
                store: store,
                bbar: pagebar,
                region: 'center',
                columns: [
                { header: '发票号', dataIndex: 'Number', width: 100 },
                { header: '开票日期', dataIndex: 'InvoiceDate', width: 150 },
                { header: ' 客户名称', dataIndex: 'CName', width: 200 },
                { header: '发票金额(￥)', dataIndex: 'Amount', width: 100 },
                { header: '已付金额(￥)', dataIndex: 'PayAmount', width: 100 },
                { header: '坏账金额(￥)', dataIndex: 'BadDebtAmount', width: 100 },
                { header: '备注', dataIndex: 'Remark', flex: 1 }
                ]
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [grid]
            })
            //            Ext.Ajax.request({
            //                url: 'ManualPayoffInvoice.aspx?action=loadform&invoiceid=' + invoiceid,
            //                //  params: { id: id, inwarehouseids: inwarehouseids },
            //                //   async: false,
            //                success: function (response, opts) {
            //                    var json = Ext.decode(response.responseText);
            //                    formpanel.getForm().setValues(json.data);
            //                    field_baddebt.setValue(field_invoiceamount.getValue() - field_payamount.getValue())
            //                }
            //            })
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    </form>
</body>
</html>
