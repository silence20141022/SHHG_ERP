<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SkinList.aspx.cs" Inherits="Aim.Examining.Web.SkinList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, op, win, winform;
        var pgBar, schBar, tlBar, titPanel, grid, viewport, tempRec, State;
        function onPgLoad() {
            setPgUI();
            //schBar.toggleCollapse(false);
            viewport.doLayout();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["SkinList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SkinList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'SkinNo' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'InWarehouseId' },
			    { name: 'ModelNo' }, { name: 'Quantity' }, { name: 'SkinState' }
			    ],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || {};
                    options.data.State = State;
                    State = "全部";
                }
                }
            });

            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 2,
                items: [
                    { fieldLabel: '包装编号', id: 'SkinNo', schopts: { qryopts: "{ mode: 'Like', field: 'SkinNo' }"} },
                     { fieldLabel: '产品条码', id: 'ModelNo', schopts: { qryopts: "{ mode: 'Like', field: 'ModelNo' }"} }
                ]
            });

            // 工具栏 new Ext.ux.form.AimComboBox({ enumdata: HTEnum })
            tlBar = new Ext.ux.AimToolbar({
                items: ['『<b>包装列表</b>』',
                { text: '未满',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        State = '未满';
                        store.reload();
                    }
                }, '-',
               { text: '已满',
                   iconCls: 'aim-icon-search',
                   handler: function() {
                       State = '已满';
                       store.reload();
                   }
               }, '-',
               { text: '全部',
                   iconCls: 'aim-icon-search',
                   handler: function() {
                       State = "全部";
                       store.reload();
                   }
               },
                '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        State = "全部";
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("SkinNo"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            viewport.doLayout();
                        }
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });

                // 表格面板, renderer: ExtGridDateOnlyRender
                grid = new Ext.ux.grid.AimEditorGridPanel({
                    store: store,
                    region: 'west',
                    split: true,
                    border: false,
                    width: 500,
                    autoExpandColumn: 'ModelNo',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: 'Id', hidden: true },
                    { id: 'InWarehouseId', dataIndex: 'InWarehouseId', header: 'InWarehouseId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'SkinNo', dataIndex: 'SkinNo', header: '<label style="color:red;">包装编号</label>', width: 100, sortable: true, editor: { xtype: 'textfield', allowBlank: false} },
					{ id: 'Quantity', dataIndex: 'Quantity', header: '<label style="color:red;">数量</label>', width: 50, editor: { xtype: 'numberfield', minValue: 1, allowBlank: false} },
					{ id: 'ModelNo', dataIndex: 'ModelNo', header: '产品条码', sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 80, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'SkinState', dataIndex: 'SkinState', header: '状态', width: 50, sortable: true}],
                    bbar: pgBar,
                    tbar: titPanel,
                    listeners: { rowclick: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (recs || recs.length > 0) {
                            frameContent.location.href = "CompressorList.aspx?SkinNo=" + recs[0].get("SkinNo") + "&InWarehouseId=" + recs[0].get("InWarehouseId") + "&SkinId=" + recs[0].get("Id");
                        }
                    }
                      , 'afteredit': function(e) {
                          if (e.field == "SkinNo") {
                              jQuery.ajaxExec("JudgeSkinIsExist", { SkinNo: e.value.toUpperCase() }, function(rtn) {
                                  if (rtn.data.SkinIsExist) {
                                      AimDlg.show("该包装号已经存在！");
                                      e.record.set("SkinNo", e.originalValue); e.record.commit();
                                  }
                                  else {
                                      var recs = store.getModifiedRecords();
                                      var dt = store.getModifiedDataStringArr(recs) || [];
                                      jQuery.ajaxExec("batchsave", { data: dt }, function(rtn) {//修改完SkinNo后重新加载该包装下的压缩机
                                          e.record.set("SkinNo", e.value.toUpperCase()); e.record.commit();
                                          frameContent.location.href = "CompressorList.aspx?SkinNo=" + e.record.get("SkinNo") + "&InWarehouseId=" + e.record.get("InWarehouseId");
                                      });
                                  }
                              });
                          }
                          if (e.field == "Quantity") {
                              var recs = store.getModifiedRecords();
                              var dt = store.getModifiedDataStringArr(recs) || [];
                              jQuery.ajaxExec("batchsave", { data: dt }, function(rtn) {
                                  e.record.commit();
                              });
                          }
                      }
                    }
                });
                // 页面视图{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, 
                viewport = new Ext.Viewport({
                    layout: 'border',
                    items: [grid, {
                        id: 'frmcon',
                        region: 'center',
                        margins: '-1 0 -2 0',
                        html: '<iframe width="100%" height="100%" id="frameContent" src="CompressorList.aspx" name="frameContent" frameborder="0"></iframe>'}]
                    });
                    if (store.data.length > 0) {
                        frameContent.location.href = "CompressorList.aspx?SkinNo=" + store.getAt(0).get("SkinNo") + "&InWarehouseId=" + store.getAt(0).get("InWarehouseId");
                    }
                }        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
