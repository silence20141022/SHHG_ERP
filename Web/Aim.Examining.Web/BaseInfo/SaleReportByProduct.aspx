<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SaleReportByProduct.aspx.cs" Inherits="Aim.Examining.Web.SaleReportByProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport, tempRec;
        var productId = $.getQueryString({ "ID": "ProductId" });
        var beginDate = $.getQueryString({ "ID": "BeginDate" });
        var endDate = $.getQueryString({ "ID": "EndDate" });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OrdersPartList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OrdersPartList',
                idProperty: 'Id',
                data: myData,
                fields: [
		        { name: 'CName' }, { name: 'Total'}],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data = options.data || {};
                    options.data.ProductId = productId;
                    options.data.BeginDate = beginDate;
                    options.data.EndDate = endDate;
                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["RecordCount"],
                store: store
            });
            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 2,
                items: [
                    { fieldLabel: '客户名称', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} }
                ]
            });
            // 工具栏 
            tlBar = new Ext.ux.AimToolbar({
            items: [{
                text: '导出Excel',
                iconCls: 'aim-icon-xls',
                handler: function() {
                    ExtGridExportExcel(grid, { store: null, title: '标题' });
                }
            }
                ]
            });
            // 工具标题栏
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                region: 'center',
                border: false,
                autoExpandColumn: 'CName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', sortable: true,
					    summaryRenderer: function(v, params, data) { return "汇总:" }
					},
					{ id: 'Total', dataIndex: 'Total', header: '合计', width: 60, summaryType: 'sum', sortable: true}],
                bbar: pgBar,
                tbar: titPanel,
                plugins: new Ext.ux.grid.GridSummary()
            });
            viewport = new Ext.Viewport({
                layout: 'border',
                items: [grid]
            });
        }
        //    function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
        //        var rtn;
        //        switch (this.id) {
        //            case "Operate":
        //                if (record.get("SkinId") == null || record.get("SkinId") == "") {
        //                    rtn = " <img src='../images/shared/device-add.gif' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='Box(\"" + value + "\")'>装箱</label>";
        //                }
        //                else {
        //                    rtn = " <img src='../images/shared/device-del.gif' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='UnBox(\"" + value + "\")'>拆箱</label>";
        //                }
        //                break;
        //            case "Remark":
        //                if (value == null) {
        //                    rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;';
        //                }
        //                else {
        //                    rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
        //                }
        //                break;
        //            default: //因为有汇总插件存在 所以存在第三种情形
        //                rtn = value;
        //                break;
        //        }
        //        return rtn;
        //    }           
          
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
