<%@ Page Title="退货单" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="ReturnOrderList.aspx.cs" Inherits="Aim.Examining.Web.ReturnOrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var EditPageUrl = "ReturnOrderEdit.aspx";
        var ViewWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var ViewPageUrl = "ReturnOrderEdit.aspx";
        var store, myData, pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OrderList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OrderList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'OrderNumber' }, { name: 'Number' },
			    { name: 'CId' }, { name: 'CName' }, { name: 'ReturnMoney' },
			    { name: 'Remark' }, { name: 'IsDiscount' }, { name: 'DiscountState' },
			    { name: 'State' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }
			    ]
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 5,
                collapsed: false,
                items: [
                { fieldLabel: '退货单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '退货时间从', labelWidth: 100, id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
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
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function() {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        if (recs[0].get("State") == "已完成") {
                            AimDlg.show("已完成的退货单不能删除！");
                            return;
                        }
                        if (confirm("确定删除所选记录？")) {
                            $.ajaxExec("delete", { id: recs[0].get("Id") }, function() {
                                store.reload();
                            })
                        }
                    }
                }, '-', {
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, '->'
                ]
            });
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'center',
                autoExpandColumn: 'CName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '退货单号', linkparams: { url: ViewPageUrl, style: ViewWinStyle }, width: 130, sortable: true },
					{ id: 'OrderNumber', dataIndex: 'OrderNumber', header: '销售单号', width: 130, sortable: true },
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
                    ExtOpenGridEditWin(grid, EditPageUrl, "r", EditWinStyle);
                }
                }
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function onExecuted() {
            store.reload();
        }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
