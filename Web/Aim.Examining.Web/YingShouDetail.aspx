<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YingShouDetail.aspx.cs"
    Inherits="Aim.Examining.Web.YingShouDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="/js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var year = getQueryString('year');
        var month = getQueryString('month');
        Ext.onReady(function () {
            var store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'PriceType', 'PurchaseOrderNo', 'Symbo', 'PurchaseType', 'SupplierId', 'SupplierName', 'TransportationMode',
                'OrderDate', 'Remark', 'OrderState', 'PurchaseOrderAmount', 'ProductType', 'InvoiceState', 'MoneyType',
                'PayState', 'InWarehouseState', 'CreateId', 'CreateTime', 'CreateName', 'RuKuDanQuan', 'DetailQuan'],
                proxy: {
                    url: 'GuoChanPeiJian_Purchase.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: 'total'
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store, options) {
                    var new_params = {
                        PurchaseOrderNo: Ext.getCmp("PurchaseOrderNo_s").getValue(),
                        SupplierName: Ext.getCmp("SupplierName_s").getValue(), 
                        ProductCode: Ext.getCmp('ProductCode_s').getValue()
                    }
                    Ext.apply(store.proxy.extraParams, new_params);
                }
                }
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "采购编号", id: "PurchaseOrderNo_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "产品型号", id: "ProductCode_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "供应商", id: "SupplierName_s", labelAlign: 'right', labelWidth: 60 }, combo_InWarehouseState,
                   { text: "查询", icon: '/images/shared/search_show.gif', handler: function () {
                       store.load({ params: { start: 0} });
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
                title: '应收明细',
                tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                { dataIndex: 'PurchaseOrderNo', header: '采购编号', width: 100, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                }
                },
                { dataIndex: 'SupplierName', header: '供应商', width: 240 },
				{ dataIndex: 'PurchaseType', header: '供应商类型', width: 80 },
			    { dataIndex: 'PurchaseOrderAmount', header: '采购单金额', width: 80, renderer:
                 function (value, cellmeta, record, rowIndex, columnIndex, store) {
                     return record.get("Symbo") + value;
                 }
			    },
				{ dataIndex: 'PayState', header: '付款状态', width: 70 },
				{ dataIndex: 'InWarehouseState', header: '入库状态', width: 70 },
                { dataIndex: 'InvoiceState', header: '发票状态', width: 70 },
                { dataIndex: 'RuKuDanQuan', header: '生成入库单状态', width: 110, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    if (value > 0 && value < record.get("DetailQuan")) {
                        return '部分生成入库单';
                    }
                    if (value == record.get("DetailQuan")) {
                        return '已全部生成入库单';
                    }
                    if (!value) {
                        return '未生成入库单';
                    }
                }
                },
			    { dataIndex: 'CreateName', header: '采购人 ', width: 60 },
				{ dataIndex: 'CreateTime', header: '下单时间 ', width: 110 },
                { dataIndex: 'Remark', header: '备注 ', flex: 1}],
                bbar: pagebar,
                listeners: { cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    if (cellIndex == 3) {
                        opencenterwin('GuoChanPeiJian_Purchase_Edit.aspx?op=v&id=' + record.get("Id"), 800, 550);
                    }
                }
                }
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [grid]
            })
            document.onkeydown = function (e) {
                var theEvent = e || window.event; // 兼容FF和IE和Opera     
                var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
                if (code == 13) {
                    store.load({ params: { start: 0} });
                }
            }
        }); 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
