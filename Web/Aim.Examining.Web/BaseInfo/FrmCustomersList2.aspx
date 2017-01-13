<%@ Page Title="标题" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmCustomersList2.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.FrmCustomersList2" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=800,height=600,scrollbars=no");
        var EditPageUrl = "FrmCustomersEdit2.aspx";

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["CustomerList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'CustomerList',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'SimpleName' },
			{ name: 'Name' },
			{ name: 'Bank' },
			{ name: 'AccountNum' },
			{ name: 'AccountName' },
			{ name: 'TariffNum' },
			{ name: 'Province' },
			{ name: 'UnitType' },
			{ name: 'Contact' },
			{ name: 'Remark' },
			{ name: 'PreDeposit' },
			{ name: 'MagId' },
			{ name: 'MagUser' },
			{ name: 'PreInvoice' },

			{ name: 'Tel' },
			{ name: 'Attachment' },
			{ name: 'City' },
			{ name: 'Address' },
			{ name: 'ZipCode' },
			{ name: 'Level' },
			{ name: 'Importance' },
			{ name: 'OpenTime' },
			{ name: 'Website' },

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
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"}}]
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
                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
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
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Name',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Code', dataIndex: 'Code', header: '编号', width: 100, sortable: true },
					{ id: 'SimpleName', dataIndex: 'SimpleName', header: '简称', width: 100, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '名称', width: 100, sortable: true },
					{ id: 'Province', dataIndex: 'Province', header: '省份', width: 100, sortable: true },
					{ id: 'UnitType', dataIndex: 'UnitType', header: '客户类型', width: 100, sortable: true },
					{ id: 'Bank', dataIndex: 'Bank', header: '开户银行', width: 100, sortable: true },
					{ id: 'AccountNum', dataIndex: 'AccountNum', header: '帐号', width: 100, sortable: true },
					{ id: 'AccountName', dataIndex: 'AccountName', header: '开户姓名', width: 100, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 100, sortable: true, renderer: ExtGridDateOnlyRender }

                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    listeners: { "rowdblclick": function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            return;
                        }
                        //查看
                        ExtOpenGridEditWin(grid, EditPageUrl, "r", EditWinStyle);
                    }
                    }
                });

                grid.on('render', function(grid) {
                    var store = grid.getStore();  // Capture the Store.   

                    var view = grid.getView();    // Capture the GridView.

                    grid.tip = new Ext.ToolTip({
                        target: view.mainBody,    // The overall target element.   

                        delegate: '.x-grid3-row', // Each grid row causes its own seperate show and hide.   
                        dismissDelay: 15000,
                        trackMouse: true,         // Moving within the row should not hide the tip.   

                        renderTo: document.body,  // Render immediately so that tip.body can be referenced prior to the first show.   

                        listeners: {              // Change content dynamically depending on which element triggered the show.   

                            beforeshow: function updateTipBody(tip) {
                                var rowIndex = view.findRowIndex(tip.triggerElement);
                                var temp = "<table style='font-size:12px;'>";
                                var recs = $.getJsonObj(store.getAt(rowIndex).get("Contact"));
                                for (var i = 0; i < recs.length; i++) {
                                    temp += "<tr><td style='vertical-align:top; width:60px;'>姓名:</td><td>" + recs[i].UserName + "</td></tr><tr><td>电话:</td><td>" + recs[i].Tel + "</td></tr>";
                                    if (i < recs.length - 1) {
                                        temp += "<tr><td></td></tr>";
                                    }
                                }
                                temp += "</table>";
                                tip.body.dom.innerHTML = temp;
                            }
                        }
                    });
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

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
