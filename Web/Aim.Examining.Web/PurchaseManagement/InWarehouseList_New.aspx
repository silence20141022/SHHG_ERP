<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InWarehouseList_New.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.InWarehouseList_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <!--<script src="/Extjs42/RowExpander.js" type="text/javascript"></script>-->
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var store;
        Ext.onReady(function () {
            var store_State = Ext.create('Ext.data.JsonStore', {
                fields: ['name'],
                data: [{ name: '未入库' }, { name: '已入库' }]
            })
            var combo_State = Ext.create('Ext.form.field.ComboBox', {
                store: store_State,
                queryMode: 'local',
                width: 130,
                displayField: 'name',
                valueField: 'name',
                id: 'State_s',
                fieldLabel: '状态',
                labelAlign: 'right',
                labelWidth: 50,
                editable: true
            })
            store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'WarehouseId', 'WarehouseName', 'InWarehouseNo', 'InWarehouseType', 'State', 'CreateId',
                 'CreateName', 'CreateTime', 'CheckUserId', 'CheckUserName', 'Remark', 'SupplierId', 'SupplierName',
                 'EstimatedArrivalDate', 'PublicInterface'],
                proxy: {
                    url: 'InWarehouseList_New.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: 'total'
                    }
                },
                autoLoad: true,
                listeners: {
                    beforeload: function (store, options) {
                        var new_params = {
                            InWarehouseNo: Ext.getCmp("InWarehouseNo_s").getValue(),
                            SupplierName: Ext.getCmp("SupplierName_s").getValue(),
                            State: combo_State.getValue(), ProductCode: Ext.getCmp('ProductCode_s').getValue()
                        }
                        Ext.apply(store.proxy.extraParams, new_params);
                    }
                }
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "入库编号", id: "InWarehouseNo_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "产品型号", id: "ProductCode_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "供应商", id: "SupplierName_s", labelAlign: 'right', labelWidth: 60 }, combo_State,
                   {
                       text: "查询", icon: '/images/shared/search_show.gif', handler: function () {
                           store.load({ params: { start: 0 } });
                       }
                   },
                   {
                       text: "修改", icon: '/images/shared/edit.gif', handler: function () {
                           var recs = grid.getSelectionModel().getSelection();
                           if (!recs || recs.length <= 0) {
                               Ext.Msg.alert("提示", "请选择要修改的记录！");
                               return;
                           }
                           id = recs[0].get("Id");
                           opencenterwin("InWarehouseEdit_New.aspx?id=" + id, 630, 550);
                       }
                   },
                   {
                       text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                           var recs = grid.getSelectionModel().getSelection();
                           if (!recs || recs.length <= 0) {
                               Ext.Msg.alert("提示", "请选择要删除的记录！");
                               return;
                           }
                           Ext.Ajax.request({
                               url: 'InWarehouseList_New.aspx?action=delete&id=' + recs[0].get("Id"),
                               success: function (response, opts) {
                                   Ext.Msg.alert("提示", "删除成功！");
                                   store.remove(recs[0]);
                               },
                               failure: function () {
                                   // Ext.Msg.alert("提示", "删除失败,已生成入库单的记录不允许删除！");
                               }
                           })
                       }
                   }, {
                       text: '生成付款单', icon: '/images/shared/money_add.png', handler: function () {
                           var recs = grid.getSelectionModel().getSelection();
                           if (!recs || recs.length <= 0) {
                               Ext.Msg.alert("提示", "请选择要生成付款单的记录！");
                               return;
                           }
                           var allow = true;
                           for (var i = 1; i < recs.length; i++) {
                               if (recs[i].get("SupplierId") != recs[0].get("SupplierId")) {
                                   allow = false;
                               }
                           }
                           if (!allow) {
                               Ext.Msg.alert("提示", "生成付款单的记录必须针对同一个供应商！");
                               return;
                           }
                           var inwarehouseids = "";
                           Ext.each(recs, function (rec) {
                               inwarehouseids += rec.get("Id") + ",";
                           })
                           opencenterwin('PayBillEdit_New.aspx?inwarehouseids=' + inwarehouseids, 800, 550);
                       }
                   },
                   {
                       text: '生成采购发票', icon: '/images/shared/checking.gif', handler: function () {
                           var recs = grid.getSelectionModel().getSelection();
                           if (!recs || recs.length <= 0) {
                               Ext.Msg.alert("提示", "请选择要生成采购发票的记录！");
                               return;
                           }
                           var allow = true;
                           for (var i = 1; i < recs.length; i++) {
                               if (recs[i].get("SupplierId") != recs[0].get("SupplierId")) {
                                   allow = false;
                               }
                           }
                           if (!allow) {
                               Ext.Msg.alert("提示", "生成采购发票的记录必须针对同一个供应商！");
                               return;
                           }
                           var inwarehouseids = "";
                           Ext.each(recs, function (rec) {
                               inwarehouseids += rec.get("Id") + ",";
                           });
                           opencenterwin('PurchaseInvoice_Edit.aspx?inwarehouseids=' + inwarehouseids, 800, 550);
                       }
                   }
                ]
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
                title: '入库单管理',
                tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                {
                    dataIndex: 'InWarehouseNo', header: '入库编号', width: 120, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                    }
                },
                { dataIndex: 'SupplierName', header: '供应商', width: 260 },
				{ dataIndex: 'InWarehouseType', header: '入库类型', width: 80 },
                { dataIndex: 'EstimatedArrivalDate', header: '预计到货时间', format: 'Y-m-d', width: 90, xtype: 'datecolumn' },
				{ dataIndex: 'State', header: '入库状态', width: 70 },
			    { dataIndex: 'CreateName', header: '创建人 ', width: 70 },
				{ dataIndex: 'CreateTime', header: '创建时间 ', width: 110 },
                { dataIndex: 'Remark', header: '备注 ', flex: 1 }],
                bbar: pagebar,
                listeners: {
                    cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) { 
                        if (cellIndex == 2) {
                            opencenterwin("InWarehouseEdit_New.aspx?op=view&id=" + record.get("Id"), 700, 550);
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
                    store.load({ params: { start: 0 } });
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
