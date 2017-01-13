<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FenGongSi_Order_Monitor.aspx.cs"
    Inherits="Aim.Examining.Web.SaleManagement.FenGongSi_Order_Monitor" %>

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
        var orderpartid = getQueryString("orderpartid");
        var saleorderid = getQueryString("saleorderid");
        Ext.onReady(function () {
            Ext.regModel('OrderPart', { fields: [{ name: 'Id' }, { name: 'ProductId' }, { name: 'OrderPart_Id' }, { name: 'Name' },
                { name: 'Code' }, { name: 'SecondPrice', type: 'float' }, { name: 'PurchasePrice', type: 'float' },
                { name: 'Quantity', type: 'int' }, { name: 'CustomerName' }, { name: 'CreateTime' }, { name: 'Count', type: 'int' },
                { name: 'Amount', type: 'float' }, { name: 'Remark' }, { name: 'Number' } 
                ]
            })
            var store = Ext.create('Ext.data.JsonStore', {
                model: 'OrderPart',
                proxy: {
                    type: 'ajax',
                    url: "FenGongSi_Order_Monitor.aspx?action=loaddetail&orderpartid=" + orderpartid + "&saleorderid=" + saleorderid,
                    reader: {
                        type: 'json',
                        root: 'rows'
                    }
                },
                autoLoad: true
            });
            var grid = Ext.create('Ext.grid.Panel', {
                region: 'center',
                title: '分公司订单详细',
                store: store,
                selModel: { selType: 'checkboxmodel' },
                columns: [
                            { text: 'Id', dataIndex: 'Id', hidden: true },
                            { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                            { id: 'OrderPart_Id', dataIndex: 'OrderPart_Id', hidden: true },
                            { header: '分公司订单号', dataIndex: 'Number', width: 110 },
                            { header: '客户名称', dataIndex: 'CustomerName', width: 180 },
                            { text: 'Name', header: '产品名称', dataIndex: 'Name', width: 100 },
                            { text: 'Code', header: '产品型号', dataIndex: 'Code', width: 150 },
                            { dataIndex: 'PurchasePrice', width: 70, header: '采购价格' },
                            { dataIndex: 'Count', width: 70, header: '采购数量' },
                            { dataIndex: 'SecondPrice', width: 70, header: '销售价格' },
                            { dataIndex: 'Quantity', width: 70, header: '销售数量' },
                            { dataIndex: 'Amount', width: 70, header: '销售金额' },
                            { dataIndex: 'CreateTime', header: '下单时间', width: 110 },
                            { dataIndex: 'Remark', header: '备注', flex: 1}]
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
