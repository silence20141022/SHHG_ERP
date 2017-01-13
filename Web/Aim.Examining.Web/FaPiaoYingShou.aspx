<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaPiaoYingShou.aspx.cs"
    Inherits="Aim.Examining.Web.FaPiaoYingShou" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../Extjs42/locale/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="/Extjs42/RowExpander.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        var store, store_detail, id;
        var year = getQueryString("year");
        Ext.onReady(function () {
            store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Number', 'CName', 'Amount', 'PayAmount', 'MagUser', 'PayState', 'InvoiceDate', 'CreateTime'],
                proxy: {
                    url: 'FaPiaoYingShou.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: 'total'
                    }
                },
                autoLoad: { start: 0, limit: 20 },
                listeners: { beforeload: function (store, options) {
                    var new_params = {
                        year: year
                    }
                    Ext.apply(store.proxy.extraParams, new_params);
                }
                }
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "采购编号", id: "PurchaseOrderNo_s", labelAlign: 'right', labelWidth: 60 },
                   { xtype: "textfield", fieldLabel: "产品型号", id: "ProductCode_s", labelAlign: 'right', labelWidth: 60 },
                // { xtype: "textfield", fieldLabel: "供应商", id: "SupplierName_s", labelAlign: 'right', labelWidth: 60 }, combo_InWarehouseState,
                   {text: "查询", icon: '/images/shared/search_show.gif', handler: function () {
                       store.load({ params: { start: 0} });
                   }
               },
                   { text: "添加", icon: '/images/shared/add.png', handler: function () {
                       opencenterwin('GuoChanYaSuoJi_Purchase_Edit.aspx', 800, 550);
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
                       opencenterwin('GuoChanYaSuoJi_Purchase_Edit.aspx?id=' + recs[0].get("Id"), 800, 550);
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
                           url: 'GuoChanYaSuoJi_Purchase.aspx?action=delete&id=' + recs[0].get("Id"),
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
                //   items: [{ xtype: 'numberfield', value: 20, fieldLabel: '单页记录', labelWidth: 60, id: 'pagesize'}],
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                store: store,
                displayInfo: true
            })
            var grid = Ext.create('Ext.grid.Panel', {
                store: store,
                region: 'center',
                selModel: { selType: 'checkboxmodel' },
                title: year + '发票应收',
                // tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                { dataIndex: 'Number', header: '发票号', width: 110 },
                { dataIndex: 'CName', header: '客户名称', width: 240 },
				{ dataIndex: 'Amount', header: '发票金额', width: 80, xtype: 'numbercolumn', format: '0,000' },
                { dataIndex: 'PayAmount', header: '已付款金额 ', width: 80, xtype: 'numbercolumn', format: '0,000' },
                { dataIndex: 'PayState', header: '付款状态', width: 100 },
				{ dataIndex: 'InvoiceDate', header: '开票日期 ', width: 120 }
               ],
                bbar: pagebar
                //                plugins: [{
                //                    ptype: 'rowexpander',
                //                    rowBodyTpl: ['<div id="{Id}"></div>']
                //                }],
                //                listeners: { cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                //                    if (cellIndex == 3) {
                //                        opencenterwin('GuoChanYaSuoJi_Purchase_Edit.aspx?op=v&id=' + record.get("Id"), 800, 550);
                //                    }
                //                }
                //                }
            })
            var viewport = Ext.create("Ext.container.Viewport", {
                layout: 'border',
                items: [grid]
            })
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
