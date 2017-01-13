<%@ Page Title="销售单选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="SaleOrderSelect.aspx.cs" Inherits="Aim.Examining.Web.SaleOrderSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        var myData, store, viewport, pgBar, schBar, tlBar, titPanel;
        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            //            Ext.override(Ext.grid.CheckboxSelectionModel, {
            //                handleMouseDown: function(g, rowIndex, e) {
            //                    if (e.button !== 0 || this.isLocked()) {
            //                        return;
            //                    }
            //                    var view = this.grid.getView();
            //                    if (e.shiftKey && !this.singleSelect && this.last !== false) {
            //                        var last = this.last;
            //                        this.selectRange(last, rowIndex, e.ctrlKey);
            //                        this.last = last; // reset the last     
            //                        view.focusRow(rowIndex);
            //                    } else {
            //                        var isSelected = this.isSelected(rowIndex);
            //                        if (isSelected) {
            //                            this.deselectRow(rowIndex);
            //                        } else if (!isSelected || this.getCount() > 1) {
            //                            this.selectRow(rowIndex, true);
            //                            view.focusRow(rowIndex);
            //                        }
            //                    }
            //                }
            //            });
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'ld',
                data: myData,
                fields: [
			{ name: 'Id' }, { name: 'Number' }, { name: 'CId' }, { name: 'CName' }, { name: 'TotalMoney' }, { name: 'DiscountAmount' },
			 { name: 'DeState' }, { name: 'ReturnAmount' }, { name: 'InvoiceState' }, { name: 'Remark' }
			]
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            var buttonPanel = new Ext.form.FormPanel({
                region: 'south',
                frame: true,
                buttonAlign: 'center',
                buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                    window.close();
                } }]
                });
                schBar = new Ext.ux.AimSchPanel({
                    store: store,
                    collapsed: false,
                    items: [{ fieldLabel: '销售单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                        { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"}}]
                });
                tlBar = new Ext.ux.AimToolbar({
                    items: [
           ]
                });
                titPanel = new Ext.ux.AimPanel({
                    items: [schBar]
                });
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    title: '销售单选择',
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'CName',
                    columns: [
                    { id: 'Id', header: '标识', dataIndex: 'Id', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
					{ id: 'Number', header: '销售单号', width: 130, sortable: true, dataIndex: 'Number' },
					{ id: 'CName', header: '客户名称', width: 180, sortable: true, dataIndex: 'CName' },
					{ id: 'TotalMoney', header: '订单金额', width: 100, sortable: true, dataIndex: 'TotalMoney' },
					{ id: 'DeState', header: '出库状态', width: 100, sortable: true, dataIndex: 'DeState' },
					{ id: 'InvoiceState', header: '发票状态', width: 100, sortable: true, dataIndex: 'InvoiceState' },
                    { id: 'DiscountAmount', header: '折扣金额', width: 100, sortable: true, dataIndex: 'DiscountAmount' },
					{ id: 'ReturnAmount', header: '退货金额', width: 100, sortable: true, dataIndex: 'ReturnAmount'}],
                    bbar: pgBar,
                    tbar: titPanel
                });
                viewport = new Ext.ux.AimViewport({
                    items: [AimSelGrid, buttonPanel]
                });
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
