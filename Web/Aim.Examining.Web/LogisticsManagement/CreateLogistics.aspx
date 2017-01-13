<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="CreateLogistics.aspx.cs" Inherits="Aim.Examining.Web.LogisticsManagement.CreateLogistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmLogisticsEdit.aspx";
        var ViewWinStyle2 = CenterWin("width=900,height=600,scrollbars=yes");
        var ViewPageUrl2 = "../SaleManagement/FrmDeliveryOrderEdit.aspx";

        var EditWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var EditPageUrl = "FrmLogisticsEdit.aspx";

        var store, myData, store2, myData2;
        var pgBar, schBar, tlBar, titPanel, grid, viewport, pgBar2, schBar2, tlBar2, titPanel2, grid2;

        var wtype = "0";
        var did;

        function onPgLoad() {
            setPgUI();
        }

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
			    { name: 'Id' }, { name: 'DeliveryId' }, { name: 'DeliveryNumber' }, { name: 'Number' },
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
			    { name: 'Remark' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
			    { name: 'CreateTime' }
			    ],
                listeners: { "aimbeforeload": function(proxy, options) {
                    Ext.getCmp("Number").setValue("");
                    Ext.getCmp("CName").setValue("");
                    options.did = did;
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
                //collapsed: false,
                padding: '2 0 0 0',
                items: [
                { fieldLabel: '快递名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '日期', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'CreateTime' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'CreateTime' }"}}]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                {
                    text: '修改',
                    iconCls: 'aim-icon-edit',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                    }
                },
                        '-', {
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
                    //autoExpandColumn: 'Name',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Number', dataIndex: 'Number', header: '物流单号', width: 80 },
					{ id: 'Name', dataIndex: 'Name', header: '物流公司', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 120 },
					{ id: 'CustomerName', dataIndex: 'CustomerName', header: '客户名称', width: 200 },
					{ id: 'DeliveryNumber', dataIndex: 'DeliveryNumber', header: '出库单号', width: 130 },
					{ id: 'Weight', dataIndex: 'Weight', header: '重量(千克)', width: 100 },
					{ id: 'Price', dataIndex: 'Price', header: '运费', width: 80, renderer: filterValue },
					{ id: 'Insured', dataIndex: 'Insured', header: '保价费', width: 80, renderer: filterValue },
					{ id: 'Delivery', dataIndex: 'Delivery', header: '送货费', width: 80, renderer: filterValue },
					{ id: 'Total', dataIndex: 'Total', header: '合计', width: 80, renderer: filterValue },
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 80 },
          			{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 120, renderer: ExtGridDateOnlyRender }

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
                // 表格数据
                myData2 = {
                    total: AimSearchCrit["RecordCount"],
                    records: AimState["OrderList"] || []
                };
                // 表格数据源
                store2 = new Ext.ux.data.AimJsonStore({
                    dsname: 'OrderList',
                    idProperty: 'Id',
                    data: myData2,
                    fields: [
			    { name: 'Id' }, { name: 'PId' }, { name: 'Number' }, { name: 'CId' }, { name: 'CCode' }, { name: 'CName' },
			    { name: 'ExpectedTime' }, { name: 'Child' }, { name: 'State' }, { name: 'Remark' },
			    { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'TotalMoneyHis' }, { name: 'TotalMoney' },
			    { name: 'WarehouseName' }, { name: 'TotalMoneyHis' }, { name: 'CorrespondState' }, { name: 'CorrespondInvoice' },
			    { name: 'DeliveryMode' }, { name: 'Tel' }, { name: 'SalesmanId' }, { name: 'WarehouseId' },
			    { name: 'WarehouseName' }, { name: 'LogisticState' }, { name: 'DeliveryUser' }, { name: 'DeliveryUserId' }
			    ],
                    listeners: { "aimbeforeload": function(proxy, options) {
                        Ext.getCmp("Name").setValue("");
                        Ext.getCmp("BeginDate").setValue("");
                        Ext.getCmp("EndDate").setValue("");
                        options.data.wtype = wtype;
                    }
                    }
                });

                // 分页栏
                pgBar2 = new Ext.ux.AimPagingToolbar({
                    pageSize: AimSearchCrit["PageSize"],
                    store: store2
                });

                // 搜索栏
                schBar2 = new Ext.ux.AimSchPanel({
                    store: store2,
                    columns: 4,
                    //collapsed: false,          
                    items: [
                { fieldLabel: '出库单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"}}]
                });

                // 工具栏
                tlBar2 = new Ext.ux.AimToolbar({
                    items: [
                {
                    text: '添加物流信息',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        var recs = grid2.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要添加的记录！");
                            return;
                        }
                        var ids = "";
                        var dnumbers = "";
                        for (var i = 0; i < recs.length; i++) {
                            ids += recs[i].get("Id") + ",";
                            dnumbers += recs[i].get("Number") + ",";
                        }
                        if (ids.length > 0) {
                            ids = ids.substring(0, ids.length - 1);
                            dnumbers = dnumbers.substring(0, dnumbers.length - 1);
                        }
                        window.open("FrmLogisticsEdit.aspx?op=c&Dids=" + ids + "&dnumbers=" + dnumbers, "Logistics", "width=900,height=600,scrollbars=yes");
                    }
                }, '-', { text: '未填写',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        wtype = "0";
                        store2.reload();
                        Refresh();
                    }
                }, { text: '已填写',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        wtype = "1";
                        store2.reload();
                        Refresh();
                    }
                }, '-', { text: '标记为不需要填写',
                    iconCls: 'aim-icon-execute',
                    handler: function() {
                        var recs = grid2.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }

                        var ids = "";
                        for (var i = 0; i < recs.length; i++) {
                            ids += recs[i].get("Id") + ",";
                            if (recs[i].get("LogisticState") && recs[i].get("LogisticState") != '未填写') {
                                alert("请选择未填写的记录");
                                return;
                            }
                        }

                        if (!confirm("确定要标记为无需填写物流?"))
                            return;

                        jQuery.ajaxExec('bjwxtx', { "ids": ids.substring(0, ids.length - 1) }, function() {
                            store2.reload();
                            Refresh();
                        });
                    }
                }, { text: '撤销标记为不需要填写',
                    iconCls: 'aim-icon-execute',
                    handler: function() {
                        var recs = grid2.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }

                        var ids = "";
                        for (var i = 0; i < recs.length; i++) {
                            ids += recs[i].get("Id") + ",";
                            if (recs[i].get("LogisticState") != '无需填写') {
                                alert("请选择无需填写的记录");
                                return;
                            }
                        }

                        if (!confirm("确定要取消标记无需填写?"))
                            return;

                        jQuery.ajaxExec('qxbjwxtx', { "ids": ids.substring(0, ids.length - 1) }, function() {
                            store2.reload();
                            Refresh();
                        });
                    }
                }, '->',
                {
                    text: '复杂查询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        schBar2.toggleCollapse(false);

                        setTimeout("viewport.doLayout()", 50);
                    }
}]
                });

                // 工具标题栏
                titPanel2 = new Ext.ux.AimPanel({
                    tbar: tlBar2,
                    items: [schBar2]
                });

                // 表格面板
                grid2 = new Ext.ux.grid.AimGridPanel({
                    store: store2,
                    region: 'north',
                    split: true,
                    height: 280,
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '出库单号', linkparams: { url: ViewPageUrl2, style: ViewWinStyle2 }, width: 130 },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 220 },
					{ id: 'State', dataIndex: 'State', header: '出库状态', width: 80 },
					{ id: 'LogisticState', dataIndex: 'LogisticState', header: '物流信息', width: 100, renderer: function(val) {
					    if (val && val != "未填写")
					        return '<label style="color:green;">' + val + '</label>';
					    else
					        return '<label style="color:red;">未填写</label>';
					}
					},
					{ id: 'DeliveryMode', dataIndex: 'DeliveryMode', header: '提货方式', width: 80 },
					{ id: 'DeliveryUser', dataIndex: 'DeliveryUser', header: '出库核对人', width: 80 },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间', width: 130 },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 180 }
                    ],
                    bbar: pgBar2,
                    tbar: titPanel2,
                    listeners: { "rowclick": function() {
                        var recs = grid2.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            return;
                        }

                        AimState["SearchCriterion"].Searches.Searches = [];
                        did = recs[0].get("Id");
                        store.reload();

                    }
                    }
                });
                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [grid2, grid]
                });
            }
            function Refresh() {
                //        if (wtype == "1") {
                //            grid2.region = "north";
                //            grid2.split = false;
                //            viewport = new Ext.ux.AimViewport({
                //                items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid2, grid]
                //            });
                //        }
                //        else {
                //            grid2.region = "center";
                //            grid2.split = false;
                //            viewport = new Ext.ux.AimViewport({
                //                items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid2]
                //            });
                //        }
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
