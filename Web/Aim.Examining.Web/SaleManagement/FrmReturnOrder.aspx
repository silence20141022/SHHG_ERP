<%@ Page Title="退货单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmReturnOrder.aspx.cs" Inherits="Aim.Examining.Web.FrmReturnOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var EditPageUrl = "FrmReturnOrderEdit.aspx";

        var ViewWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmReturnOrderEdit.aspx";

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OrderList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OrderList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'OrderNumber' },
			    { name: 'Number' },
			    { name: 'CId' },
			    { name: 'CCode' },
			    { name: 'CName' },
			    { name: 'ReturnMoney' },
			    { name: 'Child' },
			    { name: 'Remark' },
			    { name: 'State' },
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
                columns: 5,
                collapsed: false,
                items: [
                { fieldLabel: '退货单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '退货时间从', labelWidth: 100, id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} },
                { fieldLabel: '按钮', xtype: 'button', iconCls: 'aim-icon-search', width: 60, margins: '2 30 0 0', text: '查 询', handler: function() {
                    Ext.ux.AimDoSearch(Ext.getCmp("Number"));
                }
                }
                ]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                {
                    text: '添加',
                    iconCls: 'aim-icon-add',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                    }
                },
                {
                    text: '修改',
                    iconCls: 'aim-icon-edit',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要修改的记录！");
                            return;
                        }
                        if (recs[0].get("State")) {
                            alert("已审批的记录不能修改!"); return;
                        }

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

                        for (var i = 0; i < recs.length; i++) {
                            if (recs[i].get("State")) {
                                alert("已审批的记录不能删除!"); return;
                            }
                        }
                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
                        }
                    }
                }, '-', {
                    text: '同意',
                    iconCls: 'aim-icon-execute',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }

                        if (recs[0].get("State")) {
                            alert("所选记录已审核完成!"); return;
                        }

                        ExtOpenGridEditWin(grid, "FrmReturnOrderEdit.aspx?id=" + recs[0].get("Id") + "&operate=yes", "u", EditWinStyle);

                        //window.open("FrmReturnOrderEdit.aspx?id=" + recs[0].get("Id") + "&operate=yes", "win", EditWinStyle);
                        //if (confirm("确定同意退货？")) {
                        //    jQuery.ajaxExec('markReturn', { "Id": recs[0].get("Id") }, onExecuted);
                        //}
                    }
                }, {
                    text: '不同意',
                    iconCls: 'aim-icon-execute',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要操作的记录！");
                            return;
                        }

                        if (recs[0].get("State")) {
                            alert("所选记录已审核完成!"); return;
                        }

                        ExtOpenGridEditWin(grid, "FrmReturnOrderEdit.aspx?id=" + recs[0].get("Id") + "&operate=no", "u", EditWinStyle);
                        //window.open("FrmReturnOrderEdit.aspx?id=" + recs[0].get("Id") + "&operate=no", "win", EditWinStyle);
                    }
                }, '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, {
                    text: '初始化',
                    hidden: true,
                    iconCls: 'aim-icon-execute',
                    handler: function() {
                        $.ajaxExec('initdata', {}, onExecuted);
                    }
                }, '->'
                ]
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
                autoExpandColumn: 'CName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '退货单号', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 130, sortable: true },
					{ id: 'CCode', dataIndex: 'CCode', header: '客户编号', width: 100, sortable: true },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 100, sortable: true },

					{ id: 'ReturnMoney', dataIndex: 'ReturnMoney', header: '退款金额', width: 100, sortable: true },

					{ id: 'State', dataIndex: 'State', header: '状态', width: 100, sortable: true, renderer: function(val) {
					    if (val == "已退货")
					        return '<label style="color:green;">已退货</label>';
					    else
					        return val;
					}
					},
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '订单日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true }
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
            标题</h1>
    </div>
</asp:Content>
