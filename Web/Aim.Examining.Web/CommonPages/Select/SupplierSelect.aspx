<%@ Page Title="供应商选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SupplierSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.SupplierSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, AimSelGrid, viewport;

        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["SupplierList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SupplierList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'Symbo' },
			    { name: 'SupplierName' }, { name: 'SupplierAddress' }, { name: 'MoneyType' },
			    { name: 'ContactPerson' }, { name: 'FixedTelephone' }, { name: 'Mobile' },
			    { name: 'Fax' }, { name: 'Email' }
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
                columns: 3,
                items: [
                { fieldLabel: '名称', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '联系人', id: 'ContactPerson', schopts: { qryopts: "{ mode: 'Like', field: 'ContactPerson' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: ['<font color=red>说明：双击行可以直接完成选择</font>', '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("SupplierName"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            setTimeout("viewport.doLayout()", 50);
                        }
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                var buttonPanel = new Ext.form.FormPanel({
                    region: 'south',
                    frame: true,
                    buttonAlign: 'center',
                    buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                        window.close();
                    } }]
                    });
                    // 表格面板
                    AimSelGrid = new Ext.ux.grid.AimGridPanel({
                        title: '<img src="../../images/shared/arrow_right1.png" />供应商列表',
                        store: store,
                        forceFit: true,
                        //autoHeight: true,
                        bbar: pgBar,
                        region: 'center',
                        // checkOnly: true,
                        autoExpandColumn: 'SupplierName',
                        columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'Symbo', dataIndex: 'Symbo', header: '货币符号', width: 80, hidden: true },
                     new Ext.ux.grid.AimRowNumberer(), AimSelCheckModel,
                    { id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商名称', width: 120, sortable: true },
                        //{ id: 'SupplierAddress', dataIndex: 'SupplierAddress', header: '地址', width: 140, sortable: true },
					{id: 'ContactPerson', dataIndex: 'ContactPerson', header: '联系人', width: 70 },
                    { id: 'FixedTelephone', dataIndex: 'FixedTelephone', header: '固定电话', width: 100 },
                        //					{ id: 'Mobile', dataIndex: 'Mobile', header: '移动电话', width: 100 },
					{id: 'MoneyType', dataIndex: 'MoneyType', header: '交易币种', width: 60 },
					{ id: 'Email', dataIndex: 'Email', header: 'Email', width: 120 }
                    ],
                        tbar: titPanel
                    });

                    // 页面视图
                    viewport = new Ext.ux.AimViewport({
                        items: [AimSelGrid, buttonPanel]

                    });
                }           
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            供应商选择</h1>
    </div>
</asp:Content>
