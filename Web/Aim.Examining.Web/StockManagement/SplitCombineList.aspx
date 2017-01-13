<%@ Page Title="组装拆分规则" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master"
    AutoEventWireup="true" CodeBehind="SplitCombineList.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.SplitCombineList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .x-grid3-cell-inner, .x-grid3-hd-inner
        {
            white-space: normal !important;
        }
        .x-grid3-cell-inner
        {
        }
        .x-grid3-summary-row
        {
            background-color: #FFFFC0;
        }
        .x-grid3-summary-row .x-grid3-td-Ext2
        {
            background-color: #FFFFC0;
        }
        .x-grid3-td-Ext2
        {
            background-color: #FAFAD1;
        }
        .x-grid3-row-expanded
        {
            border-width: 1px;
            border-color: Red;
        }
    </style>

    <script type="text/javascript">
        var store, myData, op, win, winform;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var EditWinStyle = CenterWin("width=950,height=600,scrollbars=yes,resizable=yes");
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据            
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
			    { name: 'Id' }, { name: 'PId' }, { name: 'PIsbn' }, { name: 'CId' }, { name: 'CIsbn' }, { name: 'CCount' },
			    { name: 'Remark' }, { name: 'PName' }, { name: 'PCode' }, { name: 'CName' }, { name: 'CCode' }, { name: 'CreateName' }
			    ]
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
                columns: 4,
                items: [
                     { fieldLabel: '主产品名称', labelWidth: 100, id: 'PName', schopts: { qryopts: "{ mode: 'Like', field: 'PName' }"} },
                     { fieldLabel: '主产品型号', labelWidth: 100, id: 'PCode', schopts: { qryopts: "{ mode: 'Like', field: 'PCode' }"} },
                     { fieldLabel: '子产品名称', labelWidth: 100, id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                     { fieldLabel: '子产品型号', labelWidth: 100, id: 'CCode', schopts: { qryopts: "{ mode: 'Like', field: 'CCode' }"} }
                ]
            });
            // 工具栏 
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                },
                '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        State = "全部";
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("PName"));
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
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    border: false,
                    autoExpandColumn: 'PCode',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: 'Id', hidden: true },
                    { id: 'PId', dataIndex: 'PId', header: 'PId', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'PName', dataIndex: 'PName', header: '主产品名称', width: 150, sortable: true },
					{ id: 'PCode', dataIndex: 'PCode', header: '主产品型号', width: 325, sortable: true, renderer: RowRender },
					{ id: 'CName', dataIndex: 'CName', header: '子产品名称', width: 150 },
					{ id: 'CCode', dataIndex: 'CCode', header: '子产品型号', width: 325 },
					{ id: 'CCount', dataIndex: 'CCount', header: '数量', width: 50 },
				    { id: '创建人', dataIndex: 'CreateName', header: '创建人', width: 80}],
                    bbar: pgBar,
                    tbar: titPanel
                });
                // 页面视图{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, 
                viewport = new Ext.Viewport({
                    layout: 'border',
                    items: [grid]
                });
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "PCode":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../BaseInfo/ProductEdit.aspx?id=" +
                                      record.get('PId') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                }
                return rtn;
            }   
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
