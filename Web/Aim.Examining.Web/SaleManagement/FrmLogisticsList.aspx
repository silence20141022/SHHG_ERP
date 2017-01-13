<%@ Page Title="物流信息" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmLogisticsList.aspx.cs" Inherits="Aim.Examining.Web.FrmLogisticsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmLogisticsEdit.aspx";

        var ViewWinStyle2 = CenterWin("width=900,height=600,scrollbars=yes");
        var ViewPageUrl2 = "FrmDeliveryOrderEdit.aspx";

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
			    { name: 'Id' },
			    { name: 'DeliveryId' },
			    { name: 'DeliveryNumber' },
			    { name: 'Number' },
			    { name: 'Name' },
			    { name: 'Price' },
			    { name: 'Weight' },
			    { name: 'Child' },
			    { name: 'PayState' },

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
                //                {
                //                    text: '添加',
                //                    iconCls: 'aim-icon-add',
                //                    handler: function() {
                //                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                //                    }
                //                }, 
                {
                text: '修改',
                iconCls: 'aim-icon-edit',
                handler: function() {
                    ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                }
            },
            //            { text: '对应',
            //                iconCls: 'aim-icon-edit',
            //                handler: function() {

            //                    var recs = grid.getSelectionModel().getSelections();
            //                    if (!recs || recs.length <= 0) {
            //                        AimDlg.show("请先选择物流信息！");
            //                        return;
            //                    }
            //                    var lid = recs[0].get("Id");

            //                    recs = grid2.getSelectionModel().getSelections();
            //                    if (!recs || recs.length <= 0) {
            //                        AimDlg.show("请先选择出库单！");
            //                        return;
            //                    }
            //                    var did = recs[0].get("Id");
            //                    jQuery.ajaxExec('duiying', { "DId": did, "LId": lid }, function() {
            //                        alert("对应成功");
            //                    });
            //                }
            //            },
            //            {
            //                text: '删除',
            //                iconCls: 'aim-icon-delete',
            //                handler: function() {
            //                    var recs = grid.getSelectionModel().getSelections();
            //                    if (!recs || recs.length <= 0) {
            //                        AimDlg.show("请先选择要删除的记录！");
            //                        return;
            //                    }

            //                    if (confirm("确定删除所选记录？")) {
            //                        ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
            //                    }
            //                }
            //            },
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
            //					{ id: 'DeliveryId', dataIndex: 'DeliveryId', header: '对应状态', width: 100, sortable: true, renderer: function(val) {
            //					    if (val) {
            //					        return "已对应";
            //					    }
            //					}
            //					},

					{id: 'CreateTime', dataIndex: 'CreateTime', header: '日期', width: 120, renderer: ExtGridDateOnlyRender },
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


        //22222222222222222222222

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
			    { name: 'Id' },
			    { name: 'PId' },
			    { name: 'Number' },
			    { name: 'CId' },
			    { name: 'CCode' },
			    { name: 'CName' },
			    { name: 'ExpectedTime' },
			    { name: 'CorrespondState' },
			    { name: 'CorrespondInvoice' },
			    { name: 'DeliveryOrder' },
			    { name: 'SalesmanId' },

			    { name: 'WarehouseId' },
			    { name: 'WarehouseName' },
			    { name: 'TotalMoneyHis' },
			    { name: 'TotalMoney' },
			    { name: 'Salesman' },
			    { name: 'Address' },
			    { name: 'Tel' },
			    { name: 'DeliveryMode' },

			    { name: 'Child' },
			    { name: 'Remark' },
			    { name: 'State' },
			    { name: 'LogisticState' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
			    { name: 'CreateTime' }
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
            //collapsed: false,
            padding: '2 0 0 0',
            items: [
                { fieldLabel: '单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
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
                        for (var i = 0; i < recs.length; i++) {
                            if (recs[i].get("LogisticState") == "已填写") {
                                alert("请填写未添加物流信息的记录");
                                return;
                            }
                            ids += recs[i].get("Id") + ",";
                        }
                        if (ids.length > 0) {
                            ids = ids.substring(0, ids.length - 1);
                        }
                        window.open("FrmLogisticsEdit.aspx?op=c&Dids=" + ids, "Logistics", "width=900,height=600,scrollbars=yes");
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

            autoExpandColumn: 'CName',
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '单号', linkparams: { url: ViewPageUrl2, style: ViewWinStyle2 }, width: 200 },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 100 },
					{ id: 'ExpectedTime', dataIndex: 'ExpectedTime', header: '要求到货时间', width: 200, renderer: ExtGridDateOnlyRender },
					{ id: 'LogisticState', dataIndex: 'LogisticState', header: '物流信息', width: 100, renderer: function(val) {
					    if (val == "已填写")
					        return '<label style="color:green;">已填写</label>';
					    else
					        return '<label style="color:red;">未填写</label>';
					}
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 200, renderer: ExtGridDateOnlyRender }
                    ],
            bbar: pgBar2,
            tbar: titPanel2,
            listeners: { "rowclick": function() {
                var recs = grid2.getSelectionModel().getSelections();
                if (!recs || recs.length <= 0) {
                    return;
                }
                if (wtype == "1") {

                    AimState["SearchCriterion"].Searches.Searches = [];

                    did = recs[0].get("Id");
                    store.reload();
                }
            }
            }
        });

        // 页面视图
        viewport = new Ext.ux.AimViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid2, grid]
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
