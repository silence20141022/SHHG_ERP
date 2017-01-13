<%@ Page Title="员工工资" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmUserWage.aspx.cs" Inherits="Aim.Examining.Web.FrmUserWage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
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
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["UserList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'UserList',
                idProperty: 'UserID',
                data: myData,
                fields: [
			    { name: 'UserID' },
			    { name: 'Name' },
			    { name: 'WorkNo' },
			    { name: 'LoginName' },
			    { name: 'Wage' },
			    { name: 'DeptName' }
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
                items: [
                { fieldLabel: '姓名', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '部门', id: 'DeptName', schopts: { qryopts: "{ mode: 'Like', field: 'DeptName' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '保存',
                    iconCls: 'aim-icon-save',
                    handler: function() {

                        //保存修改的数据begin
                        var recs = store.getModifiedRecords();
                        if (recs && recs.length > 0) {
                            var dt = store.getModifiedDataStringArr(recs) || [];

                            jQuery.ajaxExec('batchsave', { "data": dt }, function() {
                                store.commitChanges();

                                AimDlg.show("保存成功！");
                            });
                            //保存修改的数据end
                        }
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
                grid = new Ext.ux.grid.AimEditorGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Name',
                    columns: [
                    { id: 'UserID', dataIndex: 'UserID', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'DeptName', dataIndex: 'DeptName', header: '部门', width: 150, sortable: true, renderer: renderRow },
					{ id: 'Name', dataIndex: 'Name', header: '姓名', width: 150, sortable: true },
					{ id: 'WorkNo', dataIndex: 'WorkNo', header: '工号', width: 150, sortable: true },
					{ id: 'LoginName', dataIndex: 'LoginName', header: '登陆名', width: 150, sortable: true },
					{ id: 'Wage', dataIndex: 'Wage', header: '<label style="color:red;">工资</label>', width: 150, sortable: true, editor: { xtype: 'numberfield'} }
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

            function renderRow(value, meta, record, rowIndex, colIndex, store) {
                var first = !rowIndex || value !== store.getAt(rowIndex - 1).get("DeptName"),
                last = rowIndex >= store.getCount() - 1 || value !== store.getAt(rowIndex + 1).get("DeptName");
                meta.css += 'row-span' + (first ? ' row-span-first' : '') + (last ? ' row-span-last' : '');
                if (first) {
                    var i = rowIndex + 1;
                    while (i < store.getCount() && value === store.getAt(i).get("DeptName")) {
                        i++;
                    }
                    var rowHeight = 23.5, padding = 0, height = (rowHeight * (i - rowIndex) - padding) + 'px';
                    meta.attr = 'style="height:' + height + ';line-height:' + height + ';"';
                }
                return first ? '<b>' + value + '</b>' : '';
            }

            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
