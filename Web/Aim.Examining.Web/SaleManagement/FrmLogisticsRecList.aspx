<%@ Page Title="物流信息" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmLogisticsRecList.aspx.cs" Inherits="Aim.Examining.Web.FrmLogisticsRecList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmLogisticsEdit.aspx";

        var EditWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var EditPageUrl = "FrmLogisticsEdit.aspx";

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        var paystate = 0;

        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["LogisticList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'LogisticList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'DeliveryId' },
			    { name: 'DeliveryNumber' },
			    { name: 'Number' },
			    { name: 'Name' },
			    { name: 'Price' },
			    { name: 'Weight' },
			    { name: 'Child' },

			    { name: 'CustomerId' },
			    { name: 'CustomerName' },
			    { name: 'Receiver' },
			    { name: 'Tel' },
			    { name: 'MobilePhone' },
			    { name: 'PayType' },
			    { name: 'Insured' },
			    { name: 'Delivery' },
			    { name: 'Total' },
			    { name: 'Address' },
			    { name: 'PayState' },

			    { name: 'Remark' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
			    { name: 'CreateTime' }
			    ], listeners: { "aimbeforeload": function(proxy, options) {
			        options.data.paystate = paystate;
			    }
			    }
            });

            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                padding: '2 0 0 0',
                items: [
                { fieldLabel: '快递名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '日期', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'CreateTime' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'CreateTime' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                    { text: '标记为已付款',
                        iconCls: 'aim-icon-execute',
                        handler: function() {

                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要操作的数据！");
                                return;
                            }
                            var ids = "";
                            for (var i = 0; i < recs.length; i++) {
                                ids += recs[i].get("Id") + ",";
                            }
                            if (ids.length > 0) {
                                ids = ids.substring(0, ids.length - 1);
                            }

                            jQuery.ajaxExec('mark', { "ids": ids }, function() {
                                onExecuted();
                                AimDlg.show("标记成功！");
                            });
                        }
                    },
             '-', {
                 text: '已付款',
                 iconCls: 'aim-icon-search',
                 handler: function() {
                     paystate = 1;
                     store.reload();
                 }
             }, {
                 text: '未付款',
                 iconCls: 'aim-icon-search',
                 handler: function() {
                     paystate = 0;
                     store.reload();
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
                    plugins: new Ext.ux.grid.GridSummary(),
                    region: 'center',
                    autoExpandColumn: 'Name',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Name', dataIndex: 'Name', header: '快递名称', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 120, summaryRenderer: function(v, params, data) { return "汇总:" } },
					{ id: 'Number', dataIndex: 'Number', header: '运单号', width: 150 },
					{ id: 'Address', dataIndex: 'Address', header: '地址', width: 200 },
					{ id: 'Total', dataIndex: 'Total', header: '总运费', width: 100, summaryType: 'sum', renderer: filterValue },

					{ id: 'Weight', dataIndex: 'Weight', header: '重量', width: 100 },
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 100, renderer: function(val) {
					    if (val == "1") {
					        return '<label style="color:green">已付款</label>'
					    }
					    else {
					        return '<label style="color:red">未付款</label>'
					    }
					}
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '日期', width: 120, renderer: ExtGridDateOnlyRender },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 300 }
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
            物流单付款</h1>
    </div>
</asp:Content>
