<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="InWarehouseProductList.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.InWarehouseProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var inWarehouseId = $.getQueryString({ "ID": "InWarehouseId" });
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["InWarehouseDetail"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'InWarehouseDetail',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' }, { name: 'InWarehouseNo' }, { name: 'InWarehouseState' },
			{ name: 'Name' }, { name: 'Code' }, { name: 'Isbn' }, { name: 'Quantity' }
			],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data = options.data || {};
                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            // 表格面板
            var columnarray = [{ id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    { id: 'Name', dataIndex: 'Name', header: '产品名称', width: 100 },
                    { id: 'Code', dataIndex: 'Code', header: '产品型号', renderer: RowRender },
                    { id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 60 },
                    { id: 'Quantity', dataIndex: 'Quantity', header: '数量', width: 35}];
            grid = new Ext.ux.grid.AimGridPanel({
                title: '【' + AimState["InWarehouseNo"] + "】入库单明细",
                store: store,
                region: 'center',
                autoExpandColumn: 'Code',
                columns: columnarray
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "行业名称":
                    if (value) {
                        rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"ContractByMonth.aspx?Industry=" +
                                 escape(record.get('行业名称')) + "\",\"wind\",\"" + WinStyle + "\")'>" + value + "</label>";
                    }
                    break;
                case "Code":
                    if (value) {
                        value = value || "";
                        cellmeta.attr = 'ext:qtitle =""' + ' ext:qtip ="' + value + '"';
                        rtn = value;
                    }
                    break;
            }
            return rtn;
        }    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
