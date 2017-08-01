<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customerlist.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.customerlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all-gray.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <link href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/pan.js" type="text/javascript"></script>
    <style type="text/css">
        a:hover {
            text-decoration: none;
        }
    </style>
    <script type="text/javascript">
        var store, pagebar;
        Ext.onReady(function () { 
            store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Name', 'SimpleName', 'MagId', 'MagUser', 'CreateTime'],
                proxy: {
                    url: 'customerlist.aspx?action=load',
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
                            name: Ext.getCmp("name_s").getValue()
                        }
                        Ext.apply(store.proxy.extraParams, new_params);
                    }
                }
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                   { xtype: "textfield", fieldLabel: "客户名称", id: "name_s", labelAlign: 'right', labelWidth: 60 },
                   {
                       text: '<span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;查询', handler: function () {
                           pagebar.moveFirst();
                       }
                   }, '-', {
                       text: '<span class="glyphicon glyphicon-refresh"></span>&nbsp;&nbsp;重置', tooltip: '清空查询条件', tooltipType: 'title', handler: function () {
                           Ext.getCmp('name_s').setValue('');
                       }
                   }, '->',
                    {
                        text: '<span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;添加', handler: function () {
                            window.location.href = "customeredit.aspx";
                        }
                    }, '-',
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
                   }
                ]
            })
            pagebar = Ext.create('Ext.toolbar.Paging', {
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                store: store,
                displayInfo: true
            })
            var grid = Ext.create('Ext.grid.Panel', {
                store: store,
                region: 'center',
                selModel: { selType: 'checkboxmodel' },
                title: '客户管理',
                tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                { dataIndex: 'Name', header: '客户名称', width: 260 },
				{ dataIndex: 'SimpleName', header: '客户简称', width: 80 },
                { dataIndex: 'MagUser', header: '销售负责人', width: 90 },
				{ dataIndex: 'CreateTime', header: '创建时间 ', flex: 1 }],
                bbar: pagebar,
                listeners: {
                    //cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    //    if (cellIndex == 2) {
                    //        opencenterwin("InWarehouseEdit_New.aspx?op=view&id=" + record.get("Id"), 700, 550);
                    //    }
                    //}
                }
            })
            var viewport = Ext.create('Ext.container.Viewport', {
                layout: 'border',
                items: [grid]
            })
            //document.onkeydown = function (e) {
            //    var theEvent = e || window.event; // 兼容FF和IE和Opera     
            //    var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
            //    if (code == 13) {
            //        store.load({ params: { start: 0 } });
            //    }
            //}
        })
    </script>
</head>
<body style="height: 48px">
</body>
</html>
