<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ShouldPayBill.aspx.cs" Inherits="Aim.Examining.Web.ShouldPayBill" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes,resizable=yes");
        //        var EditPageUrl = "PayBillPay.aspx";
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var index = $.getQueryString({ "ID": 'Index' });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["PayBillList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'PayBillList',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' }, { name: 'PayBillNo' }, { name: 'PAmount' }, { name: 'State' }, { name: 'Symbo' },
			{ name: 'Code' }, { name: 'ExamineResult' }, { name: 'WorkFlowState' }, { name: 'SupplierName' }, { name: 'MoneyType' },
			{ name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'ActuallyPayAmount' }, { name: 'ActuallyPayTime' }
			],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || [];
                    options.data.Index = index;
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
                columns: 4,
                collapsed: false,
                items: [
                { fieldLabel: '付款编号', id: 'PayBillNo', schopts: { qryopts: "{ mode: 'Like', field: 'PayBillNo' }"} },
                { fieldLabel: '客户名称', id: 'SupplierName', schopts: { qryopts: "{ mode: 'Like', field: 'SupplierName' }"} },
                { fieldLabel: '申请人', id: 'CreateName', schopts: { qryopts: "{ mode: 'Like', field: 'CreateName' }"} }
               ]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{ text: '结束付款', iconCls: 'aim-icon-search', handler: function() {
                    var recs = grid.getSelectionModel().getSelections();
                    if (!recs || recs.length <= 0) {
                        AimDlg.show("请选择要结束付款的付款单！");
                        return;
                    }
                    var allow = true;
                    $.each(recs, function() {
                        if (this.get("Symbo") == "￥") {
                            allow = false;
                            return;
                        }
                    });
                    if (!allow) {
                        AimDlg.show("只有外币才能手动结束付款！");
                        return;
                    }
                    var dt = store.getModifiedDataStringArr(recs) || [];
                    jQuery.ajaxExec("FinishPay", { "data": dt }, function(rtn) {
                        store.reload();
                    });
                }
                }, '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("PayBillNo"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            viewport.doLayout();
                        }
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                var columnarray = [{ id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'Symbo', dataIndex: 'Symbo', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'PayBillNo', dataIndex: 'PayBillNo', header: '付款编号', width: 120, sortable: true, renderer: RowRender },
					{ id: 'SupplierName', dataIndex: 'SupplierName', header: '客户名称', width: 100, sortable: true },
				    { id: 'PAmount', dataIndex: 'PAmount', header: '应付金额', width: 80, sortable: true, renderer: RowRender },
					{ id: 'ExamineResult', dataIndex: 'ExamineResult', header: '审核结果', width: 80, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '付款状态', width: 80, sortable: true },
					{ id: 'MoneyType', dataIndex: 'MoneyType', header: '交易币种', width: 70, sortable: true },
					{ id: 'CreateName', dataIndex: 'CreateName', header: '申请人', width: 70, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '申请日期', width: 80, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'ActuallyPayAmount', dataIndex: 'ActuallyPayAmount', header: '实付金额', width: 120, renderer: RowRender, summaryType: 'sum',
					    summaryRenderer: function(v, params, data) {
					        var temp = Math.round(parseFloat(v) * 100) / 100;
					        return '￥' + filterValue(temp.toString());
					    } }];
                // 表格面板
                if (index == 0) {
                    columnarray.push({ id: 'Operation', dataIndex: 'Id', header: '操作', width: 80, renderer: RowRender });
                }
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'SupplierName',
                    columns: columnarray,
                    bbar: pgBar,
                    tbar: titPanel,
                    plugins: new Ext.ux.grid.GridSummary()
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
            function filterValue(val) {
                val = String(val);
                var whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return (whole == "null" || whole == null ? "" : whole);
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Operation":
                        if (value) {
                            rtn = "<img   src='../images/shared/key.png'/><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PayBillPay.aspx?id=" +
                       value + "&op=" + "u" + "\",\"wind\",\"" + EditWinStyle + "\")'>付款操作</label>";
                        }
                        break;
                    case "PayBillNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../FinanceManagement/PayBillView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "PAmount":
                        if (value) {
                            rtn = record.get("Symbo") + filterValue(value.toString());
                        }
                        break;
                    case "ActuallyPayAmount":
                        if (value) {
                            rtn = "￥" + filterValue(value.toString());
                        }
                        break;
                }
                return rtn;
            }      
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
