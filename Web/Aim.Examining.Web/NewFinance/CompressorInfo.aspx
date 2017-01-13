<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="CompressorInfo.aspx.cs" Inherits="Aim.Examining.Web.CompressorInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        String.prototype.trim = function () { return this.replace(/(^\s*)|(\s*$)/g, ""); } //移除字符串前后空格
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
                records: AimState["DataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'SkinNo' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'State' },
			    { name: 'CName' }, { name: 'Remark' }, { name: 'ModelNo' }, { name: 'SeriesNo' }, { name: 'InWarehouseId' },
                { name: 'SkinId' }, { name: 'InWarehouseNo' }, { name: 'Number' }, { name: 'InWarehouseTime' }, { name: 'OutWarehouseTime' },
			    { name: 'ReturnOrderNo' }, { name: 'ProductCode' }, { name: 'StockQuan' }, { name: 'Guids'}],
                listeners: { "aimbeforeload": function (proxy, options) {
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
                columns: 5,
                items: [
                    { fieldLabel: '序列号', id: 'SeriesNo', schopts: { qryopts: "{ mode: 'Like', field: 'SeriesNo' }"} },
                    { fieldLabel: '条形码', id: 'ModelNo', schopts: { qryopts: "{ mode: 'Like', field: 'ModelNo' }"} },
                    { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                    { fieldLabel: '状态', id: 'State', schopts: { qryopts: "{ mode: 'Like', field: 'State' }"} },
                    { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} }
                ]
            });
            // 工具栏 new Ext.ux.form.AimComboBox({ enumdata: HTEnum })ux.Aim
            tlBar = new Ext.Toolbar({
                items: [
                {
                    text: '标记已出库',
                    iconCls: 'aim-icon-edit',
                    handler: function () {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要标记的记录！");
                            return;
                        }
                        var ids = "";
                        $.each(recs, function () {
                            ids += (ids ? "," : "") + this.get("Id");
                        })
                        $.ajaxExec("update1", { ids: ids }, function () {
                            store.reload();
                        })
                    }
                },
                 {
                     text: '删除',
                     iconCls: 'aim-icon-delete',
                     hidden: AimState["UserInfo"].Name != '周俊青',
                     handler: function () {
                         var recs = grid.getSelectionModel().getSelections();
                         if (!recs || recs.length <= 0) {
                             AimDlg.show("请先选择要删除的记录！");
                             return;
                         }
                         if (confirm("确定删除所选记录？")) {
                             ExtBatchOperate('batchdelete', recs, null, null, function (rtn) {
                                 $.each(recs, function () { store.remove(this); })
                                 AimDlg.show("删除成功！");
                             });
                         }
                     }
                 },
                 {
                     text: '导出<label style=" font-family:@宋体">Excel</label>',
                     iconCls: 'aim-icon-xls',
                     handler: function () {
                         ExtGridExportExcel(grid, { store: null, title: '标题' });
                     }
                 }
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
                    blur: function (obj) {
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
                    { id: 'SkinNo', dataIndex: 'SkinNo', header: '包装编号', width: 80 },
					{ id: 'SeriesNo', dataIndex: 'SeriesNo', header: '序列号', width: 80, sortable: true },
					{ id: 'ModelNo', dataIndex: 'ModelNo', header: '条形码', width: 120, sortable: true },
                    { id: 'ProductCode', dataIndex: 'ProductCode', header: '产品型号', width: 120, sortable: true },
                    { id: 'StockQuan', dataIndex: 'StockQuan', header: '当前库存', width: 70, sortable: true },
                    { id: 'State', dataIndex: 'State', header: '状态', width: 70, sortable: true },
					{ id: 'InWarehouseNo', dataIndex: 'InWarehouseNo', header: '入库单号', width: 130, sortable: true },
                    { id: 'InWarehouseTime', dataIndex: 'InWarehouseTime', header: '入库时间', width: 130, sortable: true },
					{ id: 'Number', dataIndex: 'Number', header: '出库单号', width: 130, sortable: true },
                    { id: 'OutWarehouseTime', dataIndex: 'OutWarehouseTime', header: '出库时间', width: 130, sortable: true },
                    { id: 'ReturnOrderNo', dataIndex: 'ReturnOrderNo', header: '退货单号', width: 130, sortable: true },
                    { id: 'Guids', dataIndex: 'Guids', header: '出库记录', width: 130, renderer: RowRender },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 180, sortable: true }
					],
                bbar: pgBar,
                tbar: titPanel,
                listeners: { 'afteredit': function (e) {
                    if (e.record.get("SeriesNo")) {//e.field == "SeriesNo"
                        jQuery.ajaxExec("JudgeSeries", { SeriesNo: e.value.trim().toUpperCase() }, function (rtn) {
                            if (rtn.data.SeriesResult) {
                                AimDlg.show(rtn.data.SeriesResult);
                                e.record.set("SeriesNo", e.originalValue);
                                e.record.commit();
                            }
                            else {
                                var dt = store.getModifiedDataStringArr([e.record]) || [];
                                jQuery.ajaxExec("batchsave", { data: dt }, function (rtn) {
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
            grid.getColumnModel().isCellEditable = function (colIndex, rowIndex) {
                var record = store.getAt(rowIndex);
                if (colIndex == 8 && AimState["UserInfo"].Name != '周俊青') {
                    return false;
                }
                return Ext.grid.ColumnModel.prototype.isCellEditable.call(this, colIndex, rowIndex);
            }
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn = "";
            switch (this.id) {
                case "Number":
                    rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='showwin(\"" +
                                                     record.get('Id') + "\")'>" + value + "</label>";
                    break;
                case "Guids":
                    if (value) {
                        cellmeta.attr = 'ext:qtitle="" ext:qtip="' + value + '"';
                        rtn = value;
                    }
                    break;
            }
            return rtn;
        }     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
