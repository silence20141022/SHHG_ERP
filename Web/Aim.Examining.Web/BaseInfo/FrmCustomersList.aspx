<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmCustomersList.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.FrmCustomersList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=800,height=600,scrollbars=no");
        var EditPageUrl = "FrmCustomer.aspx";

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
			{ name: 'Id' }, { name: 'Code' }, { name: 'SimpleName' }, { name: 'Name' }, { name: 'Address' }, { name: 'ZipCode' },
			{ name: 'Bank' }, { name: 'AccountNum' }, { name: 'AccountName' }, { name: 'Remark' }, { name: 'City' },
			{ name: 'TariffNum' }, { name: 'Province' }, { name: 'UnitType' }, { name: 'Contact' }, { name: 'Attachment' },
	        { name: 'PreDeposit' }, { name: 'MagId' }, { name: 'MagUser' }, { name: 'PreInvoice' }, { name: 'Tel' }, { name: 'CreateName' },
			{ name: 'Level' }, { name: 'Importance' }, { name: 'OpenTime' }, { name: 'Website' }, { name: 'CreateId' }, { name: 'CreateTime' }
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
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '负责人', id: 'MagUser', schopts: { qryopts: "{ mode: 'Like', field: 'MagUser' }"} }
                ]
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
                },
              {
                  text: '导出Excel',
                  iconCls: 'aim-icon-xls',
                  handler: function() {
                      ExtGridExportExcel(grid, { store: null, title: '标题' });
                  }
              },
                 '->',
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
					{ id: 'SimpleName', dataIndex: 'SimpleName', header: '简称', width: 120, sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '编号', width: 100, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '全称', width: 100, sortable: true },
					{ id: 'MagUser', dataIndex: 'MagUser', header: '负责人', width: 80, sortable: true },
					{ id: 'Address', dataIndex: 'Address', header: '地址', width: 150, sortable: true },
                    //{ id: 'UnitType', dataIndex: 'UnitType', header: '客户类型', width: 100, sortable: true },
					{id: 'Bank', dataIndex: 'Bank', header: '开户银行', width: 120, sortable: true },
					{ id: 'AccountNum', dataIndex: 'AccountNum', header: '帐号', width: 120, sortable: true },
					{ id: 'AccountName', dataIndex: 'AccountName', header: '开户姓名', width: 100, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 80, sortable: true, renderer: ExtGridDateOnlyRender }

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

                //                grid.on('render', function(grid) {
                //                    var store = grid.getStore();  // Capture the Store.   

                //                    var view = grid.getView();    // Capture the GridView.

                //                    grid.tip = new Ext.ToolTip({
                //                        target: view.mainBody,    // The overall target element.   

                //                        delegate: '.x-grid3-row', // Each grid row causes its own seperate show and hide.   
                //                        dismissDelay: 15000,
                //                        trackMouse: true,         // Moving within the row should not hide the tip.   

                //                        renderTo: document.body,  // Render immediately so that tip.body can be referenced prior to the first show.   

                //                        listeners: {              // Change content dynamically depending on which element triggered the show.   

                //                            beforeshow: function updateTipBody(tip) {
                //                                var rowIndex = view.findRowIndex(tip.triggerElement);
                //                                var temp = "<table style='font-size:12px;'>";
                //                                var recs = $.getJsonObj(store.getAt(rowIndex).get("Contact"));
                //                                for (var i = 0; i < recs.length; i++) {
                //                                    temp += "<tr><td style='vertical-align:top; width:60px;'>姓名:</td><td>" + recs[i].UserName + "</td></tr><tr><td>电话:</td><td>" + recs[i].Tel + "</td></tr>";
                //                                    if (i < recs.length - 1) {
                //                                        temp += "<tr><td></td></tr>";
                //                                    }
                //                                }
                //                                temp += "</table>";
                //                                tip.body.dom.innerHTML = temp;
                //                            }
                //                        }
                //                    });
                //                });
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
