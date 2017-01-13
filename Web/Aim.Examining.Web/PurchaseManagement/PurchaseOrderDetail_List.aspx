<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderDetail_List.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.PurchaseOrderDetail_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../Extjs42/locale/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var store, store_detail, id;
        Ext.onReady(function () {
            var store_supplier = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'SupplierName', 'Symbo'],
                proxy: {
                    url: 'PurchaseOrderDetail_List.aspx?action=loadsupplier',
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
                labelWidth: 60,
                fieldLabel: '供应商',
                hideTrigger: true,
                labelAlign: 'right'
            })
            store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'PurchaseOrderNo', 'Symbo', 'PurchaseType', 'SupplierName', 'BuyPrice',
                'Name', 'Code', 'PCN', 'Quantity', 'Amount', 'CreateTime', 'CreateName', 'RuKuDanQuan'],
                proxy: {
                    url: 'PurchaseOrderDetail_List.aspx?action=load',
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
                        PurchaseOrderNo: Ext.getCmp('PurchaseOrderNo_s').getValue(),
                        SupplierName: combo_supplier.getValue(),
                        Code: Ext.getCmp('Code_s').getValue(),
                        PCN: Ext.getCmp('PCN_s').getValue(),
                        StartTime: Ext.getCmp('StartTime_s').getValue(),
                        EndTime: Ext.getCmp('EndTime_s').getValue()
                    }
                    Ext.apply(store.proxy.extraParams, new_params);
                }
                }
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "采购编号", id: "PurchaseOrderNo_s", labelAlign: 'right', labelWidth: 60 },
                   combo_supplier,
                   { xtype: "textfield", fieldLabel: "产品型号", id: "Code_s", labelAlign: 'right', labelWidth: 60, width: 150 },
                   { xtype: "textfield", fieldLabel: "PCN", id: "PCN_s", labelAlign: 'right', labelWidth: 50, width: 150 },
                   { xtype: "datefield", fieldLabel: "下单时间从", id: "StartTime_s", labelAlign: 'right', labelWidth: 70, format: 'Y-m-d' },
                   { xtype: "datefield", fieldLabel: "到", id: "EndTime_s", labelAlign: 'right', labelWidth: 30, format: 'Y-m-d' },
                   { text: "查询", icon: '/images/shared/search_show.gif', handler: function () {
                       store.load({ params: { start: 0} });
                   }
                   }, '-', { text: '导出Excel', icon: '/images/shared/xls.gif', handler: function () {
                       Ext.Ajax.request({
                           url: 'PurchaseOrderDetail_List.aspx?action=exportexcel',
                           params: {
                               PurchaseOrderNo: Ext.getCmp('PurchaseOrderNo_s').getValue(),
                               SupplierName: combo_supplier.getValue(),
                               Code: Ext.getCmp('Code_s').getValue(),
                               PCN: Ext.getCmp('PCN_s').getValue(),
                               StartTime: Ext.getCmp('StartTime_s').getValue(),
                               EndTime: Ext.getCmp('EndTime_s').getValue()
                           },
                           success: function (response, opts) {
                               Ext.Msg.alert('提示', '导出成功!');
                           },
                           failure: function (response, opts) {
                               Ext.Msg.alert('提示', '导出失败!');
                           }
                       })
                   }
                   }]
            })
            var pagebar = Ext.create('Ext.toolbar.Paging', {
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                store: store,
                //                pageSize: pageSize.getValue(),
                //                items: [pageSize],
                displayInfo: true
            })
            var grid = Ext.create('Ext.grid.Panel', {
                store: store,
                region: 'center',
                selModel: { selType: 'checkboxmodel' },
                title: '采购详细报表',
                tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                { dataIndex: 'PurchaseOrderNo', header: '采购编号', width: 110 },
                { dataIndex: 'SupplierName', header: '供应商', width: 110 },
                { dataIndex: 'Name', header: '产品名称', width: 110 },
				{ dataIndex: 'Code', header: '型号', width: 150 },
                { dataIndex: 'PCN', header: 'PCN', width: 130 },
                { dataIndex: 'BuyPrice', header: '采购价格', width: 80 },
                { dataIndex: 'Quantity', header: '数量', width: 60 },
			    { dataIndex: 'Amount', header: '金额', width: 80, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
			        return record.get("Symbo") + value;
			    }
			    },
                { dataIndex: 'RuKuDanQuan', header: '生成入库单数量', width: 110 },
			    { dataIndex: 'CreateName', header: '采购人 ', width: 60 },
				{ dataIndex: 'CreateTime', header: '下单时间 ', width: 110 },
                { dataIndex: 'Remark', header: '备注 ', flex: 1}],
                bbar: pagebar
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
