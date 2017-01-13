<%@ Page Title="采购价管理" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SupplierIframe.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.SupplierIframe" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

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
			{ name: 'Fax' }, { name: 'MoneyType' },
			{ name: 'ContactPerson' },
			{ name: 'Email' },
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
                items: ['<label style="color:red;">请选择供应商对其产品价格进行维护：</label>', '->',
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
            var sm = new Ext.grid.RowSelectionModel({
                singleSelect: true,
                listeners: { 'rowselect': function(sm, rowindex, rec) {
                    //                        WaitSignal = false;
                    //                        projectId = rec.get("Id");
                    if (document.getElementById("frameContent")) {
                        //                            if (document.getElementById("frameContent").contentWindow.AutoSave) {//初次加载的时候 防止方法还没有完全加载完报错
                        //                                document.getElementById("frameContent").contentWindow.AutoSave();
                        //                            }
                        //                            window.setTimeout(trans, "10");
                        frameContent.location.href = "ProductPriceList.aspx?SupplierId=" + rec.get("Id") + "&SupplierName=" + rec.get("SupplierName") + "&MoneyType=" + rec.get("MoneyType");
                    }
                }
                }
            });
            // 表格面板
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'north',
                sm: sm,
                height: 200,
                autoExpandColumn: 'SupplierName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商名称', width: 100 },
					{ id: 'SupplierAddress', dataIndex: 'SupplierAddress', header: '地址', width: 150 },
					{ id: 'ContactPerson', dataIndex: 'ContactPerson', header: '联系人', width: 80 },
					 { id: 'MoneyType', dataIndex: 'MoneyType', header: '交易币种', width: 70 },
				    { id: 'Email', dataIndex: 'Email', header: 'Email', width: 120 },
				    { id: 'Bank', dataIndex: 'Bank', header: '开户行', width: 120 },
				    { id: 'Account', dataIndex: 'Account', header: '账号', width: 140 },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true }
                    ],
                bbar: pgBar,
                tbar: titPanel
            });

            // 页面视图
            viewport = new Ext.ux.AimViewport({
                items: [grid, {
                    region: 'center',
                    margins: '0 0 0 0',
                    cls: 'empty',
                    bodyStyle: 'background:#f1f1f1',
                    html: '<iframe width="100%" height="100%" id="frameContent"  name="frameContent" frameborder="0" ></iframe>'}]
                });
                if (document.getElementById("frameContent")) {
                    if (store.getCount() > 0) {
                        var rec = store.getAt(0);
                        frameContent.location.href = "ProductPriceList.aspx?SupplierId=" + rec.get("Id") + "&SupplierName=" + rec.get("SupplierName") + "&MoneyType=" + rec.get("MoneyType");
                    }
                }
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
            供应商管理</h1>
    </div>
</asp:Content>
