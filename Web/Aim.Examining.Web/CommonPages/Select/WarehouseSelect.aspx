<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="WarehouseSelect.aspx.cs" Inherits="Aim.Examining.Web.WarehouseSelect" %>

<%@ OutputCache Duration="1" VaryByParam="None" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var QualificationEnum, OpPropertyEnum, QualificationGradeEnum;

        var store, viewport;

        function onSelPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            // 表格数据
            var myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["WhList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'WhList',
                idProperty: 'ld',
                data: myData,
                fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'Remark' }
			]
            });

            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            var buttonPanel = new Ext.form.FormPanel({
                region: 'south',
                frame: true,
                buttonAlign: 'center',
                buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                    window.close();
                } }]
                });

                // 搜索栏
                schBar = new Ext.ux.AimSchPanel({
                    store: store,
                    collapsed: false,
                    items: [{ fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                        { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"}}]
                });

                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: [
                    //            { text: '选择', iconCls: 'aim-icon-accept', handler: function() {
                    //                AimGridSelect();
                    //            }
                    //            },
             '<font color=red>请点击复选框选择/取消选择记录</font>', '->',
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
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Remark',
                    columns: [
                    { id: 'Id', header: '标识', dataIndex: 'Id', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
					{ id: 'Code', header: '仓库编号', width: 100, sortable: true, dataIndex: 'Code' },
					{ id: 'Name', header: '仓库名称', width: 100, sortable: true, dataIndex: 'Name' },
					{ id: 'Remark', header: '描述', width: 200, sortable: true, dataIndex: 'Remark' }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    layout: 'border',
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, AimSelGrid, buttonPanel]
                });
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            仓库选择</h1>
    </div>
</asp:Content>
