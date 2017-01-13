<%@ Page Title="商品信息" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="True"
    CodeBehind="FrmProductDetailList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.FrmProductDetailList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .x-grid3-cell-inner, .x-grid3-hd-inner
        {
            white-space: normal !important;
        }
        /* */.grid-row-span .x-grid3-row
        {
            border-bottom: 0;
        }
        .grid-row-span .x-grid3-col
        {
            border-bottom: 1px solid gray;
        }
        .grid-row-span .row-span
        {
            border-bottom: 1px solid #fff;
        }
        .grid-row-span .row-span-first
        {
            position: relative;
        }
        .grid-row-span .row-span-first .x-grid3-cell-inner
        {
            position: absolute;
            border-right: 1px solid gray;
            border-bottom: 1px solid gray;
        }
    </style>

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
        var EditPageUrl = "ProductEdit.aspx";

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["ProductList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'ProductList',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' },
			{ name: 'PId' },
			{ name: 'PIsbn' },
			{ name: 'PPcn' },
			{ name: 'PCode' },
			{ name: 'PName' },
			{ name: 'GuId' },
			{ name: 'State' },
			{ name: 'Remark' },
			{ name: 'CreateId' },
			{ name: 'CreateName' },
			{ name: 'CreateTime' }
			]
            });
            //store.sort('BeRoleName', 'ASC');

            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                items: [
                { fieldLabel: '唯一编号', id: 'GuId', schopts: { qryopts: "{ mode: 'Like', field: 'GuId' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }

                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
                        }
                    }
                }, '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, '->',
                {
                    text: '复杂查询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        schBar.toggleCollapse(false);

                        setTimeout("viewport.doLayout()", 50);
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });

                // 表格面板
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'PName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'PName', dataIndex: 'PName', header: '商品名称', width: 150, sortable: true, renderer: renderRow2 },
                    { id: 'PCode', dataIndex: 'PCode', header: '规格型号', width: 150, sortable: true, renderer: renderRow },
					{ id: 'GuId', dataIndex: 'GuId', header: '唯一编号', width: 200, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '状态', width: 200, sortable: true, renderer: function(val) {
					    if (val) {
					        return "<label style='color:red;'>未售出</label>";
					    }
					    else {
					        return "<label style='color:green;'>已售出</label>";
					    }
					}
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', renderer: ExtGridDateOnlyRender, width: 100, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 150, sortable: true }
                    ],
                    bbar: pgBar,
                    cls: 'grid-row-span',
                    tbar: titPanel
                });
                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid]
                });
            }

            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
            function renderRow(value, meta, record, rowIndex, colIndex, store) {
                var first = !rowIndex || value !== store.getAt(rowIndex - 1).get("PCode"), last = rowIndex >= store.getCount() - 1 || value !== store.getAt(rowIndex + 1).get("PCode");
                meta.css += 'row-span' + (first ? ' row-span-first' : '') + (last ? ' row-span-last' : '');
                if (first) {
                    var i = rowIndex + 1;
                    while (i < store.getCount() && value === store.getAt(i).get("PCode")) {
                        i++;
                    }
                    var rowHeight = 23.5, padding = 0, height = (rowHeight * (i - rowIndex) - padding) + 'px';
                    meta.attr = 'style="height:' + height + ';line-height:' + height + ';"';
                }
                return first ? '<b>' + value + '</b>' : '';
            }

            function renderRow2(value, meta, record, rowIndex, colIndex, store) {
                var first = !rowIndex || value !== store.getAt(rowIndex - 1).get("PName"), last = rowIndex >= store.getCount() - 1 || value !== store.getAt(rowIndex + 1).get("PName");
                meta.css += 'row-span' + (first ? ' row-span-first' : '') + (last ? ' row-span-last' : '');
                if (first) {
                    var i = rowIndex + 1;
                    while (i < store.getCount() && value === store.getAt(i).get("PName")) {
                        i++;
                    }
                    var rowHeight = 23.5, padding = 0, height = (rowHeight * (i - rowIndex) - padding) + 'px';
                    meta.attr = 'style="height:' + height + ';line-height:' + height + ';"';
                }
                return first ? '<b>' + value + '</b>' : '';
            }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
