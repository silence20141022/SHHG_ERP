<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FenGongSi_Order_List.aspx.cs"
    Inherits="Aim.Examining.Web.SaleManagement.FenGongSi_Order_List" %>

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
        var store;
        Ext.onReady(function () {
            Ext.regModel('Order_FenGongSi', { fields: ['Id', 'CustomerId', 'CustomerName', 'Remark', 'FenGongSiName', 'FenGongSiId',
             'InvoiceType', 'PayType', 'TotalMoney', 'DiscountAmount', 'ShouKuanAmount', 'ShouKuanDate', 'Number', 'CreateName', 'CreateTime']
            });
            store = Ext.create('Ext.data.JsonStore', {
                pageSize: 25,
                model: 'Order_FenGongSi',
                proxy: {
                    url: 'FenGongSi_Order_List.aspx?action=load',
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'rows',
                        totalProperty: "total"
                    }
                },
                autoLoad: true,
                listeners: { beforeload: function (store, options) {
                    var new_params = {
                        Number: Ext.getCmp('Number_s').getValue(),
                        ProductCode: Ext.getCmp('ProductCode_s').getValue()
                    }
                    Ext.apply(store.proxy.extraParams, new_params);
                }
                }
            });
            var toolbar = Ext.create('Ext.toolbar.Toolbar', {
                items: [
                 { xtype: "textfield", fieldLabel: "销售单号", labelWidth: 60, id: "Number_s", labelAlign: 'right' },
                 { xtype: "textfield", fieldLabel: "产品型号", id: "ProductCode_s", labelAlign: 'right', labelWidth: 60 },
                 { text: '查询', icon: '/images/shared/search_show.gif', handler: function () {
                     store.load({ params: {
                         Number: Ext.getCmp('Number_s').getValue(),
                         ProductCode: Ext.getCmp('ProductCode_s').getValue()
                     }
                     });
                 }
                 }, '-',
                 { text: '添加', icon: '/images/shared/add.png', handler: function () {
                     opencenterwin('FenGongSi_Order_Edit.aspx', 800, 550);
                 }
                 }, '-',
                  { text: '修改', icon: '/images/shared/edit.gif', handler: function () {
                      recs = grid.getSelectionModel().getSelection();
                      if (!recs || recs.length <= 0) {
                          Ext.Msg.alert("提示", "请选择要修改的记录！");
                          return;
                      }
                      opencenterwin("FenGongSi_Order_Edit.aspx?id=" + recs[0].get("Id"), 800, 550);
                  }
                  }, '-',
                  { text: '删除', icon: '/images/shared/delete.gif', handler: function () {
                      var recs = grid.getSelectionModel().getSelection();
                      if (!recs || recs.length <= 0) {
                          Ext.Msg.alert("提示", "请选择删除信息!");
                          return;
                      }
                      Ext.Ajax.request({
                          url: "FenGongSi_Order_List.aspx?action=delete",
                          params: { id: recs[0].get("Id") },
                          success: function (response, opts) {
                              Ext.Msg.alert("提示", "删除成功！");
                              store.remove(recs[0]);
                          }
                      });
                  }
                  }
            ]
            });
            var grid = Ext.create('Ext.grid.Panel', {
                tbar: toolbar,
                store: store,
                title: '分公司销售管理',
                region: 'center',
                enableColumnHide: false,
                selModel: { selType: 'checkboxmodel' },
                columns: [
                { xtype: 'rownumberer', width: 35 },
                { dataIndex: "Id", header: "标示", hidden: true },
                { dataIndex: "Number", header: "销售单号", width: 110, renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    return "<label style='color:Blue; cursor:pointer; text-decoration:underline;'>" + value + "</label>";
                }
                },
                { dataIndex: "CustomerName", header: "客户名称", width: 180 },
                { dataIndex: "FenGongSiName", header: "分公司名称", flex: 1 },
                { dataIndex: "InvoiceType", header: "票据类型", width: 80 },
                //{ dataIndex: "PayType", header: "支付方式", width: 80 },
                { dataIndex: "TotalMoney", header: "总金额", width: 80 },
                { dataIndex: "DiscountAmount", header: "折扣金额", width: 80 },
                { dataIndex: "ShouKuanAmount", header: "实际收款金额", width: 90 },
                { dataIndex: "ShouKuanDate", header: "收款日期", width: 80 },
                { dataIndex: 'CreateTime', header: '创建日期', width: 110 },
                { dataIndex: "Remark", header: "备注", width: 120 }
                ],
                plugins: [{
                    ptype: 'rowexpander',
                    rowBodyTpl: ['<div id="{Id}"></div>']
                }],
                listeners: { cellclick: function (tableview, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    if (cellIndex == 3) {
                        opencenterwin('FenGongSi_Order_Edit.aspx?op=v&id=' + record.get("Id"), 800, 550);
                    }
                }
                }
            });
            grid.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                displayInnerGrid(record.get('Id'));
            });
            grid.view.on('collapsebody', function (rowNode, record, expandRow, eOpts) {
                destroyInnerGrid(record.get("Id"));
            });
            var viewport = new Ext.container.Viewport({
                layout: 'border',
                items: [grid]
            })
        });
        function displayInnerGrid(div) {
            var store_inner = Ext.create('Ext.data.JsonStore', {
                fields: ['Id', 'ProductId', 'OrderPart_Id', 'Name', 'Code', 'PurchasePrice', 'SecondPrice',
                'Quantity', 'Amount', 'Remark', 'PurchaseNo'],
                proxy: {
                    url: 'FenGongSi_Order_List.aspx?action=loaddetail&id=' + div,
                    type: 'ajax',
                    reader: {
                        type: 'json',
                        root: 'innerrows'
                    }
                },
                autoLoad: true
            });
            var grid_inner = Ext.create('Ext.grid.Panel', {
                store: store_inner,
                margin: '0 0 0 100',
                columns: [
                            { xtype: 'rownumberer', width: 35 },
                            { dataIndex: 'PurchaseNo', header: '采购编号', width: 110 },
                            { header: '产品名称', dataIndex: 'Name', width: 120 },
                            { header: '产品型号', dataIndex: 'Code', width: 180 },
                            { dataIndex: 'PurchasePrice', width: 90, header: '采购价格' },
                            { dataIndex: 'SecondPrice', width: 90, header: '销售价格' },
                            { dataIndex: 'Quantity', width: 90, header: '数量' },
                            { dataIndex: 'Amount', width: 110, header: '金额' },
                            { dataIndex: 'Remark', header: '备注', flex: 1 }
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
