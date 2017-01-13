<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="CompressorList.aspx.cs" Inherits="Aim.Examining.Web.CompressorList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        String.prototype.trim = function() { return this.replace(/(^\s*)|(\s*$)/g, ""); } //移除字符串前后空格
        var store, myData, op, win, winform;
        var pgBar, schBar, tlBar, titPanel, grid, viewport, tempRec;
        var inWarehouseId = $.getQueryString({ "ID": "InWarehouseId" });
        var skinNo = $.getQueryString({ ID: "SkinNo" });
        var SkinId = $.getQueryString({ ID: "SkinId" });
        var condition = "All";
        var array = [];
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["CompressorList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'CompressorList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'SkinNo' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'State' },
			    { name: 'CName' }, { name: 'Remark' }, { name: 'ModelNo' }, { name: 'SeriesNo' },
			    { name: 'InWarehouseId' }, { name: 'SkinId' }, { name: 'InWarehouseNo' }, { name: 'Number' }
			    ],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data = options.data || {};
                    options.data.Condition = condition;
                    condition = "All";
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
                columns: 3,
                items: [
                    { fieldLabel: '序列号', id: 'SeriesNo', schopts: { qryopts: "{ mode: 'Like', field: 'SeriesNo' }"} },
                    { fieldLabel: '条形码', id: 'ModelNo', schopts: { qryopts: "{ mode: 'Like', field: 'ModelNo' }"} },
                    { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} }

                ]
            });
            // 工具栏 new Ext.ux.form.AimComboBox({ enumdata: HTEnum })ux.Aim
            tlBar = new Ext.Toolbar({
                items: ['『<b>压缩机列表</b>』'
            , { text: '整板添加', iconCls: 'aim-icon-add',
                handler: function() {
                    if (!SkinId) {
                        AimDlg.show("请先选择箱号！");
                        return;
                    }
                    else {
                        store.removeAll();
                        jQuery.ajaxExec("Whole", { SkinId: SkinId }, function(rtn) {
                            var EntRecord = grid.getStore().recordType;
                            var compData = eval(rtn.data.CompressorList);
                            for (var k = 0; k < compData.length; k++) {
                                var p = new EntRecord(compData[k]);
                                var insRowIdx = store.data.length;
                                store.insert(insRowIdx, p);
                            }
                            var existRecQuan = store.getCount();
                            for (var i = 0; i < rtn.data.SkinEntity.Quantity - existRecQuan; i++) {
                                var p = new EntRecord({ SkinNo: rtn.data.SkinEntity.SkinNo,
                                    ModelNo: rtn.data.SkinEntity.ModelNo, SkinId: rtn.data.SkinEntity.Id, State: '未出库', InWarehouseId: inWarehouseId
                                });
                                grid.stopEditing();
                                var insRowIdx = store.data.length;
                                store.insert(insRowIdx, p);
                            }
                        });
                    }
                }
            },
                  {
                      text: '散机',
                      iconCls: 'aim-icon-search',
                      handler: function() {
                          $("#SkinNo").text("");
                          condition = "NoSkin";
                          store.reload();
                      }
                  },
                  { text: '单个添加', iconCls: 'aim-icon-add',
                      handler: function() {
                          var recType = store.recordType;
                          var rec = new recType({ State: '未出库' });
                          store.insert(store.data.length, rec);
                          var gridview = grid.getView();
                          var firstRow = Ext.get(gridview.getRow(0));
                          var row = Ext.get(gridview.getRow(store.data.length - 1));
                          var distance = row.getOffsetsTo(firstRow)[1];
                          gridview.scroller.dom.scrollTop = distance;
                      }
                  },
                 {
                     text: '删除',
                     iconCls: 'aim-icon-delete',
                     hidden: AimState["UserInfo"].Name != '周俊青',
                     handler: function() {
                         var recs = grid.getSelectionModel().getSelections();
                         if (!recs || recs.length <= 0) {
                             AimDlg.show("请先选择要删除的记录！");
                             return;
                         }
                         if (confirm("确定删除所选记录？")) {
                             ExtBatchOperate('batchdelete', recs, null, null, function(rtn) {
                                 $.each(recs, function() { store.remove(this); })
                                 AimDlg.show("删除成功！");
                             });
                         }
                     }
                 },
                 {
                     text: '导出<label style=" font-family:@宋体">Excel</label>',
                     iconCls: 'aim-icon-xls',
                     handler: function() {
                         ExtGridExportExcel(grid, { store: null, title: '标题' });
                     }
                 }, '<b>当前箱号：<label id="SkinNo"></label></b>'
                ]
            });
            // 工具标题栏
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            var statuscb = new Ext.ux.form.AimComboBox({
                enumdata: { '未出库': '未出库', '已出库': '已出库' },
                lazyRender: true,
                triggerAction: 'all',
                mode: 'local',
                listeners: {
                    blur: function(obj) {
                        if (grid.activeEditor) {
                            var rec = store.getAt(grid.activeEditor.row);
                        }
                    }
                }
            });
            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                region: 'center',
                border: false,
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'InWarehouseId', dataIndex: 'InWarehouseId', header: 'InWarehouseId', hidden: true },
                    { id: 'SkinId', dataIndex: 'SkinId', header: 'SkinId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'SkinNo', dataIndex: 'SkinNo', header: '包装编号', width: 120 },
					{ id: 'SeriesNo', dataIndex: 'SeriesNo', header: '<label style="color:red;">序列号</label>',
					    editor: { xtype: 'textfield', allowBlank: false }, width: 100
					},
					{ id: 'ModelNo', dataIndex: 'ModelNo', header: '<label style="color:red;">条形码</label>', width: 150, editor: { xtype: 'textfield', allowBlank: false} },
                    { id: 'State', dataIndex: 'State', header: '<label style="color:red;">状态</label>', width: 70, sortable: true, editor: statuscb },
					{ id: 'InWarehouseNo', dataIndex: 'InWarehouseNo', header: '入库单号', width: 130 },
					{ id: 'Number', dataIndex: 'Number', header: '出库单号', width: 130 },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 250 }
					],
                bbar: pgBar,
                tbar: titPanel,
                listeners: { 'afteredit': function(e) {
                    if (e.record.get("SeriesNo")) {//e.field == "SeriesNo"
                        jQuery.ajaxExec("JudgeSeries", { SeriesNo: e.value.trim().toUpperCase() }, function(rtn) {
                            if (rtn.data.SeriesResult) {
                                AimDlg.show(rtn.data.SeriesResult);
                                e.record.set("SeriesNo", e.originalValue);
                                e.record.commit();
                            }
                            else {
                                var dt = store.getModifiedDataStringArr([e.record]) || [];
                                jQuery.ajaxExec("batchsave", { data: dt }, function(rtn) {
                                    if (rtn.data.Compressor) {
                                        e.record.set("Id", rtn.data.Compressor.Id);
                                        // e.record.set("CreateTime", rtn.data.CreateTime);
                                        e.record.set("SeriesNo", rtn.data.Compressor.SeriesNo);
                                        e.record.commit();
                                    }
                                });
                            }
                        });
                    }
                }
                }
            });
            viewport = new Ext.Viewport({
                layout: 'border',
                items: [grid]
            });
            $("#SkinNo").text(skinNo);
            grid.getColumnModel().isCellEditable = function(colIndex, rowIndex) {
                var record = store.getAt(rowIndex);
                if (colIndex == 8 && AimState["UserInfo"].Name != '陈燕萍') { 
                    return false;
                }
                return Ext.grid.ColumnModel.prototype.isCellEditable.call(this, colIndex, rowIndex);
            }
        }          
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
