<%@ Page Title="出库单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmDeliveryProduct.aspx.cs" Inherits="Aim.Examining.Web.FrmDeliveryProduct" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/ext/ux/RowExpander.js" type="text/javascript"></script>

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var mdls, tabPanel, grid, store;

        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }

        function onPgLoad() {

            $("#StandardSub").css({ "height": $(document).height() - 30 });

            setPgUI();
        }

        function setPgUI() {

            InitEditTable();
        }

        // 处理网格数据
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function() {
                subdata.push(this.data);
            });
            var jsonstr = $.getJsonString(subdata);

            $("#Child").val(jsonstr);
        }

        function InitEditTable() {

            // 表格数据
            myData = {
                records: $.getJsonObj($("#Child").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
                { name: 'Id' },
			    { name: 'Isbn' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Price' },
			    { name: 'Guids' },
			    { name: 'Unit' },
			    { name: 'Amount' },
			    { name: 'SmCount' },
			    { name: 'OutCount' },
			    { name: 'Remark' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                columns: [
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, resizable: true, hidden: true },
                    { id: 'Isbn', header: 'Isbn', dataIndex: 'Isbn', width: 80, resizable: true, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Code', header: '规格型号', renderer: ExtGridpperCase, dataIndex: 'Code', width: 120, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'Name', header: '商品名称', dataIndex: 'Name', width: 150, resizable: true },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60, resizable: true },
                    //{ id: 'Price', header: '售价', dataIndex: 'Price', width: 80, resizable: true, renderer: filterValue },
                    { id: 'OutCount', header: '出库数量', dataIndex: 'OutCount', summaryType: 'sum', width: 60, resizable: true, allowBlank: false }
                    //{ id: 'Amount', dataIndex: 'Amount', header: '总金额', summaryType: 'sum', width: 80, renderer: filterValue }
                ]
            });

            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                //width: 633,
                width: Ext.get("StandardSub").getWidth(),
                height: Ext.get("StandardSub").getHeight(),
                //forceLayout: true,
                columnLines: true,
                viewConfig: {
                    forceFit: true
                },
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r'
                }),

                autoExpandColumn: 'Remark'
            });

            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
                grid.setHeight(0);
                grid.setWidth(Ext.get("StandardSub").getHeight());
            };
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1 style="margin-left: 10px;">
            出库单商品信息</h1>
    </div>
    <textarea id="Child" cols="3" rows="1" style="display: none;"></textarea>
    <div id="StandardSub" align="left" style="width: 100%;">
    </div>
</asp:Content>
