<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="CostPriceList.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.CostPriceList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                pruneModifiedRecords: true,
                data: myData,
                fields: [{ name: 'Id' }, { name: 'Name' }, { name: 'Code' }, { name: 'Isbn' }, { name: 'Pcn' }, { name: 'CostPrice' },
                 { name: 'ProductType' }, { name: 'SupplierName'}],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || {};
                }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 5,
                items: [
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} },
                { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"} },
                { fieldLabel: '产品类型', id: 'ProductType', xtype: 'aimcombo', enumdata: AimState["ProductTypeEnum"], schopts: { qryopts: "{ mode: 'Like', field: 'ProductType' }"} }
                ]
            });
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '导出Excel',
                    iconCls: 'aim-icon-xls',
                    handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '销售单' });
                    }
                }, '->']
            });
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                region: 'center',
                forceFit: true,
                autoExpandColumn: 'Code',
                columns: [
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Id', header: '标识', dataIndex: 'Id', hidden: true },
                    { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 160, sortable: true },
                    { id: 'ProductType', header: '产品类型', dataIndex: 'ProductType', width: 70, sortable: true },
                    { id: 'Code', header: '产品型号', dataIndex: 'Code', width: 180, sortable: true },
                    { id: 'Isbn', header: '条形码', dataIndex: 'Isbn', width: 150, sortable: true },
                    { id: 'Pcn', header: 'PCN', dataIndex: 'Pcn', width: 120, sortable: true },
                    { id: 'CostPrice', header: '<label style="color:red">成本价</label>', dataIndex: 'CostPrice', width: 80, sortable: true,
                        editor: { xtype: 'numberfield', allowBlank: false, minValue: 0, decimalPrecision: 2 }, renderer: RowRender
                    },
                    { id: 'SupplierName', header: '供应商', dataIndex: 'SupplierName', width: 180, sortable: true}],
                bbar: pgBar,
                tbar: titPanel,
                listeners: { 'afteredit': function(e) {
                    jQuery.ajaxExec("Save", { id: e.record.get("Id"), CostPrice: e.record.get("CostPrice") }, function(rtn) {
                        e.record.commit();
                    });
                }
                }
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });

        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "Operation":
                    if (value) {
                        rtn = " <img src='../images/shared/application_view_list.png' /><label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PurchaseOrderView.aspx?id=" +
                                       value + "\",\"wind\",\"" + EditWinStyle + "\")'>查看详细</label>";

                    }
                    break;
                case "CostPrice":
                    if (value) {
                        rtn = '￥' + filterValue(value);
                    }
                    break;
            }
            return rtn;
        }
        function filterValue(val) {
            if (val) {
                var whole = '';
                val = String(val);
                whole = val;
                var r = /(\d+)(\d{3})/;
                while (r.test(whole)) {
                    whole = whole.replace(r, '$1' + ',' + '$2');
                }
                return whole;
            }
        }
        function onExecuted() {
            store.reload();
        }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
