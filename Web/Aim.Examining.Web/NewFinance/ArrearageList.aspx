<%@ Page Title="欠款报表" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="ArrearageList.aspx.cs" Inherits="Aim.Examining.Web.ArrearageList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var pgBar, schBar, tlBar, titPanel, store, myData, grid, viewport;
        var CC = $.getQueryString({ ID: "CC" });
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
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'Number' }, { name: 'CId' }, { name: 'CCode' }, { name: 'CName' },
			    { name: 'Amount' }, { name: 'Invalid' }, { name: 'Child' }, { name: 'Remark' }, { name: 'CreateId' },
			     { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'PayAmount' }, { name: 'PayState' }, { name: 'MagName' }
			    ],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data.CC = CC;
                }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });

            // 搜索栏
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                collapsed: false,
                columns: 5,
                items: [
                { fieldLabel: '发票号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '负责人', id: 'MagName', schopts: { qryopts: "{ mode: 'Like', field: 'MagName' }"} },
                { fieldLabel: '开票时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"}}]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [
            { text: '添加付款', iconCls: 'aim-icon-add', handler: function() {
                var recs = grid.getSelectionModel().getSelections();
                opencenterwin("CustomerPayEdit.aspx?op=c&CId=" + (recs.length > 0 ? recs[0].get("CId") : "") + "&CName=" + (recs.length > 0 ? escape(recs[0].get("CName")) : ""), "", 900, 400);
            }
            }, '-', {
                text: '导出Excel',
                iconCls: 'aim-icon-xls',
                handler: function() {
                    ExtGridExportExcel(grid, { store: null, title: '标题' });
                }
            }, '->']
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
					{ id: 'Number', dataIndex: 'Number', header: '发票号', width: 100, sortable: true },
					{ id: 'CName', dataIndex: 'CName', header: '客户名称', width: 100, sortable: true },
					{ id: 'Amount', dataIndex: 'Amount', header: '发票金额', width: 100, sortable: true, renderer: RowRender,
					    summaryType: 'sum', summaryRenderer: function(v, paras) { return AmountFormat(v); }
					},
                    { id: 'PayAmount', dataIndex: 'PayAmount', header: '已付款金额', width: 100, sortable: true, renderer: RowRender,
                        summaryType: 'sum', summaryRenderer: function(v, paras) { return AmountFormat(v); }
                    },
                    { id: 'MagName', dataIndex: 'MagName', width: 80, header: '客户负责人', sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '开票日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 150 }
                    ],
                bbar: pgBar,
                tbar: titPanel
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function opencenterwin(url, name, iWidth, iHeight) {
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
            window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
        }
        function onExecuted() {
            store.reload();
        }
        function AmountFormat(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn = "";
            switch (this.id) {
                case "Title":
                    rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='showwin(\"" +
                                                     record.get('Id') + "\")'>" + value + "</label>";
                    break;
                case "Amount":
                case "PayAmount":
                    if (value) {
                        rtn = AmountFormat(value);
                    }
                    break;
            }
            return rtn;
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
