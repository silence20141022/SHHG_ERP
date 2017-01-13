<%@ Page Title="标题" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmTransactionHis.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.FrmTransactionHis" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
        var EditPageUrl = "FrmCustomersEdit.aspx";

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["TransactionHisList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'TransactionHisList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'CId' },
			    { name: 'CCode' },
			    { name: 'CName' },
			    { name: 'Number' },
			    { name: 'TransactionTime' },
			    { name: 'Child' },
			    { name: 'Remark' },
			    { name: 'RtnOrOut' },

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
                { fieldLabel: '客户编号', id: 'CCode', schopts: { qryopts: "{ mode: 'Like', field: 'CCode' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
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
                    autoExpandColumn: 'CName',
                    region: 'north',
                    split: true,
                    height: 280,
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'CCode', dataIndex: 'CCode', header: '客户编号', width: 100, sortable: true },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 100, sortable: true },
					{ id: 'RtnOrOut', dataIndex: 'RtnOrOut', header: '类型', width: 100, sortable: true, renderer: function(val) {
					    if (val == "购买") {
					        return '<label style="color:green;">购买<label>';
					    }
					    else if (val == "退货") {
					        return '<label style="color:red;">退货<label>';
					    }
					    else {
					        return val;
					    }
					}
					},
					{ id: 'Number', dataIndex: 'Number', header: '(发/退)货单号', width: 100, sortable: true },
					{ id: 'TransactionTime', dataIndex: 'TransactionTime', header: '交易完成时间', renderer: ExtGridDateOnlyRender, width: 100, sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    listeners: { "rowclick": function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            return;
                        }
                        frameContent.window.location = "FrmTransactionHisDetail.aspx?id=" + recs[0].get("Id");
                    }
                    }
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid, {
                        region: 'center',
                        margins: '0 0 0 0',
                        cls: 'empty',
                        bodyStyle: 'background:#f1f1f1',
                        html: '<iframe width="100%" height="100%" id="frameContent" src="FrmTransactionHisDetail.aspx" name="frameContent" frameborder="0" src=""></iframe>'}]
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
