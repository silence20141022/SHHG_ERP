<%@ Page Title="库存盘点" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="StockCheckCheckList.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.StockCheckCheckList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var enumState = { '': '新建', 'null': '新建', 'Flowing': '流程中', 'End': '流程结束' };
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var state = "未结束";
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
			    { name: 'Id' }, { name: 'StockCheckNo' }, { name: 'WarehouseName' }, { name: 'WarehouseId' }, { name: 'WorkFlowState' }, { name: 'Result' },
			    { name: 'State' }, { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'StockCheckUserName' }
			    ],
                listeners: { aimbeforeload: function(proxy, opitions) {
                    opitions.data = opitions.data || [];
                    opitions.data.State = state;
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
                collapsed: false,
                columns: 5,
                items: [
                { fieldLabel: '盘点编号', id: 'StockCheckNo', schopts: { qryopts: "{ mode: 'Like', field: 'StockCheckNo' }"} },
                { fieldLabel: '盘点仓库', id: 'WarehouseName', schopts: { qryopts: "{ mode: 'Like', field: 'WarehouseName' }"} },
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: '按钮', xtype: 'button', iconCls: 'aim-icon-search', width: 60, margins: '2 0 0 0', text: '查 询', handler: function() {
                    Ext.ux.AimDoSearch(Ext.getCmp("StockCheckNo"));
                }
                }
                ]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '未结束',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        state = '未结束';
                        store.reload();
                    }
                }, '-', {
                    text: '已结束',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        state = '已结束';
                        store.reload();
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
                    autoExpandColumn: 'Remark',
                    columns: [
                    //{ id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'StockCheckNo', dataIndex: 'StockCheckNo', header: '盘点编号', width: 130, sortable: true },
					{ id: 'WarehouseName', dataIndex: 'WarehouseName', header: '盘点仓库', width: 150, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '盘点状态', width: 80, sortable: true },
					{ id: 'WorkFlowState', dataIndex: 'WorkFlowState', header: '流程状态', width: 80, enumdata: enumState, sortable: true },
				    { id: 'StockCheckUserName', dataIndex: 'StockCheckUserName', header: '盘点人', width: 80, sortable: true },
				    { id: 'Result', dataIndex: 'Result', header: '盘点结果', width: 80, sortable: true },
					{ id: 'CreateName', dataIndex: 'CreateName', header: '创建人', width: 100, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 200, renderer: RowRender },
					{ id: 'Id', dataIndex: 'Id', header: '操作', width: 100, renderer: RowRender }
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
                    items: [grid]
                });
            }

            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
            function AutoExecuteFlow(flowid) {
                jQuery.ajaxExec('AutoExecuteFlow', { "FlowId": flowid }, function(rtn) {
                    AimDlg.show("提交成功！");
                    onExecuted();
                    Ext.getBody().unmask();
                });
            }
            function opencenterwin(url, name, iWidth, iHeight) {
                var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
                var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
                window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=auto,resizable=yes');
            }
            function showwin(val) {
                opencenterwin("StockCheckCheck.aspx?id=" + val + "&op=u", "newwin", 950, 600);
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Id":
                        if (value) {
                            rtn = " <img src='../images/shared/cog_edit.png' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='showwin(\"" + value + "\")'>盘点</label>";
                        }
                        break;
                    case "Remark":
                        if (value) {
                            cellmeta.attr = "ext:qtitle=''" + " ext:qtip='" + value + "'";
                            rtn = value;
                        }
                        break;
                }
                return rtn;
            }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
