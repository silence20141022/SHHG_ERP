<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JinKouChanPin_Purchase.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.JinKouChanPin_Purchase" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="/Extjs42/RowExpander.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var store, id;
        Ext.onReady(function () {
            var store_InWarehouseState = Ext.create('Ext.data.JsonStore', {
                fields: ['name'],
                data: [{ name: '未入库' }, { name: '已入库'}]
            })
            var combo_InWarehouseState = Ext.create('Ext.form.field.ComboBox', {
                store: store_InWarehouseState,
                queryMode: 'local',
                width: 120,
                displayField: 'name',
                valueField: 'name',
                id: 'InWarehouseState_s',
                fieldLabel: '状态',
                labelAlign: 'right',
                labelWidth: 50,
                editable: true
            })
            store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'PriceType', 'PurchaseOrderNo', 'Symbo', 'PurchaseType', 'SupplierId', 'SupplierName', 'TransportationMode',
                'OrderDate', 'Remark', 'OrderState', 'PurchaseOrderAmount', 'ProductType', 'InvoiceState', 'MoneyType',
                'PayState', 'InWarehouseState', 'CreateId', 'CreateTime', 'CreateName', 'RuKuDanQuan', 'DetailQuan'],
                proxy: {
                    url: 'JinKouChanPin_Purchase.aspx?action=load',
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
                        InWarehouseState: combo_InWarehouseState.getValue(),
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
                   },
                   { text: "添加", icon: '/images/shared/add.png', handler: function () {
                       opencenterwin('JinKouChanPin_Purchase_Edit.aspx', 800, 550);
                   }
                   },
                   { text: "修改", icon: '/images/shared/edit.gif', handler: function () {
                       var recs = grid.getSelectionModel().getSelection();
                       if (!recs || recs.length <= 0) {
                           Ext.Msg.alert("提示", "请选择要修改的记录！");
                           return;
                       }
                       if (recs[0].get("RuKuDanQuan")) {
                           Ext.Msg.alert("提示", "已全部或者部分生成入库单的记录不允许修改！");
                           return;
                       }
                       opencenterwin('JinKouChanPin_Purchase_Edit.aspx?id=' + recs[0].get("Id"), 800, 550);
                   }
                   },
                   { text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                       var recs = grid.getSelectionModel().getSelection();
                       if (!recs || recs.length <= 0) {
                           Ext.Msg.alert("提示", "请选择要删除的记录！");
                           return;
                       }
                       if (recs[0].get("RuKuDanQuan")) {
                           Ext.Msg.alert("提示", "已全部或者部分生成入库单的记录不允许删除！");
                           return;
                       }
                       Ext.Ajax.request({
                           url: 'JinKouChanPin_Purchase.aspx?action=delete&id=' + recs[0].get("Id"),
                           success: function (response, opts) {
                               Ext.Msg.alert("提示", "删除成功！");
                               store.remove(recs[0]);
                           },
                           failure: function () {
                               Ext.Msg.alert("提示", "删除失败,已生成入库单的记录不允许删除！");
                           }
                       })
                   }
                   }, { text: '生成入库单', icon: '/images/shared/db_import.png', handler: function () {
                       var recs = grid.getSelectionModel().getSelection();
                       if (!recs || recs.length <= 0) {
                           Ext.Msg.alert("提示", "请选择要生成入库单的记录！");
                           return;
                       }
                       var allow = true;
                       for (var i = 1; i < recs.length; i++) {
                           if (recs[i].get("SupplierId") != recs[0].get("SupplierId")) {
                               allow = false;
                           }
                       }
                       if (!allow) {
                           Ext.Msg.alert("提示", "生成入库单的记录必须针对同一个供应商！");
                           return;
                       }
                       ids = "";
                       Ext.each(recs, function (rec) {
                           ids += rec.get("Id") + ",";
                       })
                       opencenterwin("InWarehouseEdit_New.aspx?PurchaseOrderIds=" + ids, 630, 550);
                   }
                   }, { text: '采购详细报表', icon: '/images/shared/details.gif', handler: function () {
                       opencenterwin("PurchaseOrderDetail_List.aspx", 1250, 550);
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
                title: '进口产品采购',
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
                plugins: [{
                    ptype: 'rowexpander',
                    rowBodyTpl: ['<div id="{Id}"></div>']
                }],
                listeners: { cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    if (cellIndex == 3) {
                        opencenterwin('JinKouChanPin_Purchase_Edit.aspx?op=v&id=' + record.get("Id"), 800, 550);
                    }
                }
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
            document.onkeydown = function (e) {
                var theEvent = e || window.event; // 兼容FF和IE和Opera     
                var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
                if (code == 13) {
                    store.load({ params: { start: 0} });
                }
            }
        });
        function displayInnerGrid(div) {
            var store_inner = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Name', 'Code', 'Quantity', 'BuyPrice', 'ProductId', 'PCN', 'Amount', 'RuKuDanQuan'],
                proxy: {
                    url: 'JinKouChanPin_Purchase.aspx?action=loaddetail&id=' + div,
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
                    { header: '产品名称', dataIndex: 'Name', width: 100 },
                    { header: '型号', dataIndex: 'Code', width: 150 },
                    { header: 'PCN', dataIndex: 'PCN', width: 140 },
                    { header: '单价', dataIndex: 'BuyPrice', width: 80 },
                    { dataIndex: 'Quantity', header: '采购数量', width: 80 },
		            { dataIndex: 'Amount', header: '金额', width: 110 },
                    { dataIndex: 'RuKuDanQuan', header: '已生成入库单数量', width: 120 }
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
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
