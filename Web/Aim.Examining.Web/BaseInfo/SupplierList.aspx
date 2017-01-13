<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SupplierList.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.SupplierList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=650,height=450,scrollbars=yes");
        var EditPageUrl = "SupplierEdit.aspx";

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
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
			{ name: 'Id' },
			{ name: 'SupplierName' },
			{ name: 'SupplierAddress' },
			{ name: 'FixedTelephone' },
			{ name: 'Mobile' },
			{ name: 'Fax' },
			{ name: 'ContactPerson' },
			{ name: 'Email' }, { name: 'MailAddress' }, { name: 'MoneyType' },
			{ name: 'IsDefault' },
			{ name: 'Bank' },
			{ name: 'Account' },
			{ name: 'CreateId' },
			{ name: 'CreateName' },
			{ name: 'CreateTime' }
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
                items: [
                { fieldLabel: '名称', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '联系人', id: 'ContactPerson', schopts: { qryopts: "{ mode: 'Like', field: 'ContactPerson' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '添加',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                    }
                }, {
                    text: '修改',
                    iconCls: 'aim-icon-edit',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                    }
                }, {
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, function(rtn) {
                                AimDlg.show(rtn.data.Message); onExecuted();
                            });
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
                    sm: new Ext.grid.RowSelectionModel({ singleSelect: true }),
                    autoExpandColumn: 'SupplierName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商名称', renderer: RowRender },
					{ id: 'SupplierAddress', dataIndex: 'SupplierAddress', header: '地址', width: 150 },
					{ id: 'MoneyType', dataIndex: 'MoneyType', header: '交易币种', width: 70 },
					{ id: 'ContactPerson', dataIndex: 'ContactPerson', header: '联系人', width: 70 },
				    { id: 'Email', dataIndex: 'Email', header: 'Email', width: 120 },
				    { id: 'Bank', dataIndex: 'Bank', header: '开户行', width: 100 },
				    { id: 'Account', dataIndex: 'Account', header: '账号', width: 120 },
			        { id: 'IsDefault', dataIndex: 'IsDefault', header: '默认供应商', width: 70, renderer: RowRender },
			        { id: 'CreateName', dataIndex: 'CreateName', header: '创建人', width: 80 },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建日期', width: 60, renderer: ExtGridDateOnlyRender, sortable: true }
                    ],
                    bbar: pgBar,
                    listeners: { rowdblclick: function(grid, rowindex, e) {
                        var url = "";
                        url = "SupplierView.aspx?id=";
                        window.open(url + store.getAt(rowindex).get("Id"), "viewinfo", EditWinStyle);
                    }
                    },
                    tbar: titPanel
                });

                // 页面视图{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 },
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }

            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "SupplierName":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"SupplierView.aspx?id=" +
                       record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "IsDefault":
                        return value == "on" ? "是" : "否";
                        break;
                }
                return rtn;
            }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            供应商管理</h1>
    </div>
</asp:Content>
