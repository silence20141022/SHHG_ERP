<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayBill_List.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PayBill_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="/Extjs42/RowExpander.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var store;
        Ext.onReady(function () {
            var store_State = Ext.create('Ext.data.JsonStore', {
                fields: ['name'],
                data: [{ name: '未付款' }, { name: '已付款'}]
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
                editable: true,
                labelWidth: 50,
                listeners: { select: function (combo, records, eOpts) {
                    //                    if (records[0].get("name") == "已付款") {
                    //                        Ext.getCmp("btn_modify").setVisible(false);
                    //                        Ext.getCmp("btn_delete").setVisible(false);
                    //                    }
                    //                    else {
                    //                        Ext.getCmp("btn_modify").setVisible(true);
                    //                        Ext.getCmp("btn_delete").setVisible(true);
                    //                    }
                }
                }
            })
            store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'PayBillNo', 'State', 'CreateId', 'CreateName', 'CreateTime', 'ActuallyPayAmount',
                 'Remark', 'SupplierId', 'SupplierName', 'PAmount', 'Symbo', 'ExamineResult', 'WorkFlowState'],

                proxy: {
                    url: 'PayBill_List.aspx?action=load',
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
                        PayBillNo: Ext.getCmp("PayBillNo_s").getValue(),
                        ProductCode: Ext.getCmp('ProductCode_s').getValue(),
                        SupplierName: Ext.getCmp("SupplierName_s").getValue(),
                        State: combo_State.getValue()
                    }
                    Ext.apply(store.proxy.extraParams, new_params);
                }
                }
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "付款编号", id: "PayBillNo_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "产品型号", id: "ProductCode_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "供应商", id: "SupplierName_s", labelAlign: 'right', labelWidth: 60 }, combo_State,
                   { text: "查询", icon: '/images/shared/search_show.gif', handler: function () {
                       store.load({ params: { start: 0} });
                   }
                   },
                   { text: "修改", id: 'btn_modify', icon: '/images/shared/edit.gif', handler: function () {
                       var recs = grid.getSelectionModel().getSelection();
                       if (!recs || recs.length <= 0) {
                           Ext.Msg.alert("提示", "请选择要修改的记录！");
                           return;
                       }
                       if (recs[0].get("WorkFlowState")) {
                           Ext.MessageBox.alert('提示', "审批中或者审批结束的记录不允许修改！");
                           return;
                       }
                       opencenterwin("PayBillEdit_New.aspx?id=" + recs[0].get("Id"), 900, 550);
                   }
                   },
                   { text: '删除', id: 'btn_delete', icon: '/images/shared/delete.gif', handler: function () {
                       var recs = grid.getSelectionModel().getSelection();
                       if (!recs || recs.length <= 0) {
                           Ext.Msg.alert("提示", "请选择要删除的记录！");
                           return;
                       }
                       if (recs[0].get("State")=="已付款") {
                           Ext.MessageBox.alert('提示', "已付款的记录不允许删除！");
                           return;
                       }
                       Ext.Ajax.request({
                           url: 'PayBill_List.aspx?action=delete&id=' + recs[0].get("Id"),
                           success: function (response, opts) {
                               Ext.Msg.alert("提示", "删除成功！");
                               store.remove(recs[0]);
                           } 
                       })
                   }
                   }, { text: '提交审批', icon: '/images/shared/user_suit.png', handler: function () {
                       var recs = grid.getSelectionModel().getSelection();
                       if (!recs || recs.length <= 0) {
                           Ext.MessageBox.alert('提示', "请先选择要审核的记录！");
                           return;
                       }
                       if (recs[0].get("WorkFlowState")) {
                           Ext.MessageBox.alert('提示', "审批中或者审批结束的记录不允许重复提交！");
                           return;
                       }
                       Ext.Ajax.request({
                           url: 'PayBill_List.aspx',
                           params: { action: 'examine', flowkey: 'PayBillApply', id: recs[0].get("Id") },
                           async: false,
                           success: function (response, opts) {                              
                               var json = Ext.decode(response.responseText);
                               Ext.Ajax.request({
                                   url: 'PayBill_List.aspx',
                                   params: { instanceid: json.instanceid, action: 'autoexec' },
                                   success: function () {
                                       Ext.Msg.alert("提示", "提交成功！");
                                       store.load();
                                   }
                               });
                           }
                       })
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
                title: '付款单管理',
                tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                { dataIndex: 'PayBillNo', header: '付款编号', width: 120, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                }
                },
                { dataIndex: 'SupplierName', header: '供应商', width: 260 },
				{ dataIndex: 'PAmount', header: '付款金额', width: 80, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
				    return record.get("Symbo") + value;
				}
				},
                { dataIndex: 'ActuallyPayAmount', header: '已付金额', width: 90, renderer:
                 function (value, cellmeta, record, rowIndex, columnIndex, store) {
                     return '￥' + value ? value : 0;
                 }
                },
				{ dataIndex: 'State', header: '付款状态', width: 70 },
                { dataIndex: 'WorkFlowState', header: '审批状态', width: 70 },
                { dataIndex: 'ExamineResult', header: '审批结果', width: 70 },
			    { dataIndex: 'CreateName', header: '创建人 ', width: 70 },
				{ dataIndex: 'CreateTime', header: '创建时间 ', width: 110 },
                { dataIndex: 'Remark', header: '备注 ', flex: 1}],
                bbar: pagebar,
                plugins: [{
                    ptype: 'rowexpander',
                    rowBodyTpl: ['<div id="{Id}"></div>']
                }],
                listeners: { cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    if (cellIndex == 3) {
                        opencenterwin("PayBillEdit_New.aspx?op=view&id=" + record.get("Id"), 900, 550);
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
        })
        function displayInnerGrid(div) {
            var store_inner = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'PayBillId', 'ProductId', 'ProductCode', 'PayQuantity', 'PurchaseOrderDetailId',
                     'BuyPrice', 'ProductName', 'Amount', 'PurchaseOrderNo'],
                proxy: {
                    url: 'PayBill_List.aspx?action=loaddetail&id=' + div,
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
                    { header: '采购编号', dataIndex: 'PurchaseOrderNo', width: 110 },
                    { header: '产品名称', dataIndex: 'ProductName', width: 80 },
                    { header: '产品型号', dataIndex: 'ProductCode', width: 160 },
                    { header: '价格', dataIndex: 'BuyPrice', width: 80 },
                    { dataIndex: 'PayQuantity', header: '数量', width: 70 },
                    { dataIndex: 'Amount', header: '金额', width: 90 }
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
