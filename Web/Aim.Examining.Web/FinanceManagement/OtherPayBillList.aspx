<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="OtherPayBillList.aspx.cs" Inherits="Aim.Examining.Web.FinanceManagement.OtherPayBillList" %>

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
                records: AimState["OtherPayBillList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'PayBillList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'PayBillNo' }, { name: 'PayState' }, { name: 'PayType' }, { name: 'LogisticsCompanyName' },
			    { name: 'ShouldPayAmount' }, { name: 'AcctualPayAmount' }, { name: 'InterfaceArray' }, { name: 'ModifyUserId' },
			    { name: 'InvoiceNo' }, { name: 'ModifyTime' },
			    { name: 'InvoiceAmount' }, { name: 'PayUserId' }, { name: 'PayUserName' }, { name: 'PayTime' },
			    { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateName' }, { name: 'CreateTime' }
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
                items: [
                { fieldLabel: '付款编号', id: 'PayBillNo', schopts: { qryopts: "{ mode: 'Like', field: 'PayBillNo' }"} },
                { fieldLabel: '客户名称', id: 'LogisticsCompanyName', schopts: { qryopts: "{ mode: 'Like', field: 'LogisticsCompanyName' }"} },
                { fieldLabel: '申请人', id: 'CreateName', schopts: { qryopts: "{ mode: 'Like', field: 'CreateName' }"} }
               ]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
                //                { text: '结束付款', iconCls: 'aim-icon-search', handler: function() {
                //                    var recs = grid.getSelectionModel().getSelections();
                //                    if (!recs || recs.length <= 0) {
                //                        AimDlg.show("请选择要结束付款的付款单！");
                //                        return;
                //                    }
                //                    var allow = true;
                //                    $.each(recs, function() {
                //                        if (this.get("Symbo") == "￥") {
                //                            allow = false;
                //                            return;
                //                        }
                //                    });
                //                    if (!allow) {
                //                        AimDlg.show("只有外币才能手动结束付款！");
                //                        return;
                //                    }
                //                    var dt = store.getModifiedDataStringArr(recs) || [];
                //                    jQuery.ajaxExec("FinishPay", { "data": dt }, function(rtn) {
                //                        store.reload();
                //                    });
                //                }
                //            },
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
                    autoExpandColumn: 'Remark',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'PayBillNo', dataIndex: 'PayBillNo', header: '付款编号', width: 130, sortable: true },
					{ id: 'LogisticsCompanyName', dataIndex: 'LogisticsCompanyName', header: '物流公司', width: 100, sortable: true },
					{ id: 'PayType', dataIndex: 'PayType', header: '付款类型', width: 70, sortable: true },
				    { id: 'ShouldPayAmount', dataIndex: 'ShouldPayAmount', header: '应付金额', width: 70, sortable: true, renderer: RowRender },
					{ id: 'InvoiceAmount', dataIndex: 'InvoiceAmount', header: '发票金额', width: 70, sortable: true, renderer: RowRender },
					{ id: 'InvoiceNo', dataIndex: 'InvoiceNo', header: '发票号', width: 70 },
					{ id: 'CreateName', dataIndex: 'CreateName', header: '申请人', width: 70, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '申请日期', width: 80, sortable: true, renderer: ExtGridDateOnlyRender },
					{ id: 'AcctualPayAmount', dataIndex: 'AcctualPayAmount', header: '实付金额', width: 70, renderer: RowRender },
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 80 },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 80 },
					{ id: 'Operation', dataIndex: 'Id', header: '操作', width: 80, renderer: RowRender }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
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
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "Operation":
                        if (value) {//这个地方要判断index =0 or 1  对应的链接发生变化 为查看
                            if (index == 0) {
                                rtn = "<img   src='../images/shared/key.png'/><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"OtherPayBillPay.aspx?id=" +
                       value + "\",\"wind\",\"" + EditWinStyle + "\")'>付款操作</label>";
                            }
                            else {
                                rtn = "<img   src='../images/shared/application_view_list.png'/><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"OtherPayBillPay.aspx?id=" +
                       value + "&op=" + "View" + "\",\"wind\",\"" + EditWinStyle + "\")'>查看详细</label>";
                            }
                        }
                        break;
                    case "PayBillNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../PurchaseManagement/PayBillView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "ShouldPayAmount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = "￥" + whole;
                        break;
                    case "InvoiceAmount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = "￥" + whole;
                        break;
                    case "AcctualPayAmount":
                        if (value) {//因为有null的现象
                            val = String(value);
                            var whole = val;
                            var r = /(\d+)(\d{3})/;
                            while (r.test(whole)) {
                                whole = whole.replace(r, '$1' + ',' + '$2');
                            }
                            rtn = "￥" + whole;
                        }
                        break;
                }
                return rtn;
            }      
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
