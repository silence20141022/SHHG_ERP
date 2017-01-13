<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="LogisticsList.aspx.cs" Inherits="Aim.Examining.Web.LogisticsManagement.LogisticsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        var ViewWinStyle = CenterWin("width=900,height=600,scrollbars=yes");
        var ViewPageUrl = "FrmLogisticsEdit.aspx";
        var EditWinStyle = CenterWin("width=1000,height=600,scrollbars=yes,resizable=yes");
        var EditPageUrl = "FrmLogisticsEdit.aspx";
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var payType = '';
        var ids = '';
        function onPgLoad() {
            setPgUI();
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'DeliveryId' }, { name: 'DeliveryNumber' }, { name: 'Number' }, { name: 'Name' },
                { name: 'Price' }, { name: 'Weight' }, { name: 'Child' }, { name: 'Remark' }, { name: 'CreateId' },
			    { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'CustomerId' }, { name: 'CustomerName' },
                { name: 'Receiver' }, { name: 'Tel' }, { name: 'MobilePhone' }, { name: 'PayState' }, { name: 'PayType' },
                { name: 'Insured' }, { name: 'Delivery' }, { name: 'Total' }, { name: 'Address' }, { name: 'Volume' }, { name: 'SendDate'}],
                listeners: { aimbeforeload: function (proxy, options) {
                    options.data = options.data || [];
                    options.data.PayType = payType;
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
                collapsed: false,
                columns: 7,
                items: [
                { fieldLabel: '物流单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '物流公司', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CustomerName' }"} },
                { fieldLabel: '托运时间', id: 'BeginDate', xtype: 'datefield', vtype: 'daterange', endDateField: 'EndDate', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'EndDate', xtype: 'datefield', vtype: 'daterange', startDateField: 'BeginDate', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} },
                { fieldLabel: '备注', id: 'Remark1', schopts: { qryopts: "{ mode: 'Like', field: 'Remark' }"} },
                { fieldLabel: '按钮', xtype: 'button', iconCls: 'aim-icon-search', width: 60, margins: '2 30 0 0', text: '查 询', handler: function () {
                    Ext.ux.AimDoSearch(Ext.getCmp("Number"));
                }
                }
                ]
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '现付未付款',
                    iconCls: 'aim-icon-search',
                    handler: function () {
                        payType = "现付未付款"; store.reload();
                    }
                }, '-', {
                    text: '现付已付款',
                    iconCls: 'aim-icon-search',
                    handler: function () {
                        payType = "现付已付款"; store.reload();
                    }
                }, '-',
                 {
                     text: '到付',
                     iconCls: 'aim-icon-search',
                     handler: function () {
                         payType = "到付"; store.reload();
                     }
                 }, '-', {
                     text: '全部',
                     iconCls: 'aim-icon-search',
                     handler: function () {
                         payType = "全部"; store.reload();
                     }
                 }, '-', {
                     text: '导出<label style=" font-family:@宋体">Excel</label>',
                     iconCls: 'aim-icon-xls',
                     handler: function () {
                         ExtGridExportExcel(grid, { store: null, title: '标题' });
                     }
                 }, '-',
                {
                    text: '创建物流付款单',
                    iconCls: 'aim-icon-add',
                    handler: function () {
                        ids = '';
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要付款的物流单！");
                            return;
                        }
                        var allow = true;
                        $.each(recs, function () {
                            if (this.get("PayState") != "未付款") {
                                allow = false;
                                return false;
                            }
                        })
                        if (!allow) {
                            AimDlg.show("已提交或者已付款的物流单不能再次创建付款单！");
                            return;
                        }
                        var total = 0.00;
                        for (var i = 0; i < recs.length; i++) {
                            total = FloatAdd(total, recs[i].get("Total"));
                            ids += recs[i].get("Id") + ",";
                        }
                        window.open("LogisticsPayEdit.aspx?Total=" + total + "&LogisticsCompanyName=" + recs[0].get("Name") + "&op=c", "LogisticsPay", EditWinStyle);
                    }
                }, '->'
                ]
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
                autoExpandColumn: 'CustomerName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '物流单号', width: 80, renderer: RowRender, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '物流公司', width: 80, sortable: true },
					{ id: 'CustomerName', dataIndex: 'CustomerName', header: '客户名称', width: 160, renderer: RowRender },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 120, renderer: RowRender },
					{ id: 'DeliveryNumber', dataIndex: 'DeliveryNumber', header: '出库单号', width: 130,
					    summaryRenderer: function (v, params, data) { return "汇总:" }
					},
					{ id: 'Weight', dataIndex: 'Weight', header: '重量(kg)', width: 60,
					    summaryType: 'sum', summaryRenderer: function (v, params, data) { return v + 'kg'; }
					},
					{ id: 'Volume', dataIndex: 'Volume', header: '体积(立方米)', width: 80,
					    summaryType: 'sum', summaryRenderer: function (v, params, data) { return v; }
					},
					{ id: 'Price', dataIndex: 'Price', header: '运费(￥)', width: 60,
					    summaryType: 'sum', summaryRenderer: function (v, params, data) { return filterValue(v); }
					},
					{ id: 'Insured', dataIndex: 'Insured', header: '保价费(￥)', width: 70,
					    summaryType: 'sum', summaryRenderer: function (v, params, data) { return filterValue(v); }
					},
					{ id: 'Delivery', dataIndex: 'Delivery', header: '送货费(￥)', width: 70,
					    summaryType: 'sum', summaryRenderer: function (v, params, data) { return filterValue(v); }
					},
					{ id: 'Total', dataIndex: 'Total', header: '合计(￥)', width: 60,
					    summaryType: 'sum', summaryRenderer: function (v, params, data) { return filterValue(v); }
					},
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 70 },
					{ id: 'PayType', dataIndex: 'PayType', header: '付款类型', width: 70 },
					{ id: 'SendDate', dataIndex: 'SendDate', header: '托运时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true }
                    ],
                bbar: pgBar,
                tbar: titPanel,
                plugins: new Ext.ux.grid.GridSummary()
            });

            // 页面视图
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "Number":
                    if (value) {
                        rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"LogisticsView.aspx?id=" +
                                      record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                    }
                    break;
                case "Remark":
                    if (value) {
                        value = value || "";
                        cellmeta.attr = 'ext:qtitle =""' + ' ext:qtip ="' + value + '"';
                        rtn = value;
                    }
                    break;
                case "CustomerName":
                    if (value) {
                        value = value || "";
                        cellmeta.attr = 'ext:qtitle =""' + ' ext:qtip ="' + value + '"';
                        rtn = value;
                    }
                    break;
            }
            return rtn;
        }
        // 提交数据成功后
        function onExecuted() {
            store.reload();
        }
    
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
    </div>
</asp:Content>
