<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="OutWarehouseProductList.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.OutWarehouseProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=850,height=600,scrollbars=yes");
        var inWarehouseId = $.getQueryString({ "ID": "InWarehouseId" });
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["OutWarehouseDetail"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'OutWarehouseDetail',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' }, { name: 'Number' }, { name: 'DId' }, { name: 'Count' }, { name: 'OutCount' }, { name: 'Guids' }, { name: 'State' }, { name: 'CreateTime' }, { name: 'Isbn' },
			{ name: 'PId' }, { name: 'PCode' }, { name: 'PName' }, { name: 'Unit' }, { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateName' }
			],
                listeners: { "aimbeforeload": function(proxy, options) {
                    options.data.Id = getQueryString("Id") || "";
                }
                }
            });
            // 分页栏
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: 30,
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 2,
                items: [
                // { fieldLabel: '名称', id: 'InWarehouseNo', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                {fieldLabel: '产品型号', id: 'PCode', schopts: { qryopts: "{ mode: 'Like', field: 'PCode' }"} },
                { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"}}]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: ['<img src="../../images/shared/arrow_right1.png" /><font color=red>出库明细</font>', '->',
                               {
                                   text: '复杂查询',
                                   iconCls: 'aim-icon-search',
                                   handler: function() {
                                       schBar.toggleCollapse(false);
                                       setTimeout("viewport.doLayout()", 50);
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
            var columnarray = [{ id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    //{ id: 'Number', dataIndex: 'Number', header: '出库单号', width: 120 },
                    { id: 'PCode', dataIndex: 'PCode', header: '产品型号' },
                 	{ id: 'Isbn', dataIndex: 'Isbn', header: '条形码', width: 130 },
                    { id: 'Count', dataIndex: 'Count', header: '出库数量', width: 80 },
                    { id: 'OutCount', header: '已出库数量', dataIndex: 'OutCount', width: 80 },
                    { id: 'State', dataIndex: 'State', header: '出库状态', width: 100}];
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                region: 'center',
                autoExpandColumn: 'PCode',
                columns: columnarray,
                bbar: pgBar,
                tbar: titPanel
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
            schBar.toggleCollapse(false);
            viewport.doLayout();
        }
            
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
