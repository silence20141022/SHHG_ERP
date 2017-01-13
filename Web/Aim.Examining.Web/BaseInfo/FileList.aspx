<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileList.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.FileList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Extjs42/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="/Extjs42/bootstrap.js" type="text/javascript"></script>
    <script src="../Extjs42/locale/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="../js/pan.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            var store = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'Name', 'CreateTime', 'CreatorName'],
                proxy: {
                    url: 'FileList.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: 'total'
                    }
                },
                autoLoad: { start: 0, limit: 25 }
            })
            var pagebar = Ext.create('Ext.toolbar.Paging', {
                //items: [{ xtype: 'numberfield', value: 20, fieldLabel: '单页记录', labelWidth: 60, id: 'pagesize'}],
                //displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                // pageSize: 30,
                store: store,
                displayInfo: true
            })
            var toolbar = Ext.create("Ext.toolbar.Toolbar", {
                items: [
                 { text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                     var recs = grid.getSelectionModel().getSelection();
                     if (!recs || recs.length <= 0) {
                         Ext.Msg.alert("提示", "请选择要删除的记录！");
                         return;
                     }
                     Ext.Ajax.request({
                         url: 'FileList.aspx?action=delete&id=' + recs[0].get("Id"),
                         success: function (response, opts) {
                             Ext.Msg.alert("提示", "删除成功！");
                             store.remove(recs[0]);
                         }
                     })
                 }
                 }
                ]
            });
            var grid = Ext.create('Ext.grid.Panel', {
                store: store,
                region: 'center',
                selModel: { selType: 'checkboxmodel' },
                title: '文档列表',
                tbar: toolbar,
                columns: [
                { xtype: "rownumberer", width: 35 },
                { header: '文件名', dataIndex: 'Name', flex: 1 },
                { header: '创建人', dataIndex: 'CreatorName', width: 90 },
                { header: '创建时间', dataIndex: 'CreateTime', width: 110 },
                { xtype: 'actioncolumn', width: 60, text: '下载', align: 'center',
                    items: [{
                        icon: '/images/download.png',
                        handler: function (grid, rowIndex, colIndex) {
                            var rec = grid.getStore().getAt(rowIndex);
                            opencenterwin("/File/" + rec.get("Name"), 200, 200);
                           // window.location.href = "/File/" + rec.get("Name");
                        }
                    }]
                }],
                bbar: pagebar
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
