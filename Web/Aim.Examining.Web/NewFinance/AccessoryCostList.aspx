<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    Title="配件成本" CodeBehind="AccessoryCostList.aspx.cs" Inherits="Aim.Examining.Web.AccessoryCostList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        //        var CId = $.getQueryString({ ID: "CId" });
        //        var CName = unescape($.getQueryString({ ID: "CName" }));
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
			    { name: 'Id' }, { name: 'DId' }, { name: 'PCode' }, { name: 'PName' }, { name: 'PCN' }, { name: 'Count' }, { name: 'Number' }, { name: 'CName' },
			    { name: 'Unit' }, { name: 'NewCostPrice' }, { name: 'ProductId' }, { name: 'OutCount' }, { name: 'InvoiceNo' }, { name: 'CostPrice' }, { name: 'CostAmount' }
			    ],
                listeners: { aimbeforeload: function(proxy, options) {
                    options.data = options.data || [];
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
                { fieldLabel: '出库单号', id: 'Number', schopts: { qryopts: "{ mode: 'Like', field: 'Number' }"} },
                { fieldLabel: '客户名称', id: 'CName', schopts: { qryopts: "{ mode: 'Like', field: 'CName' }"} },
                { fieldLabel: '产品型号', id: 'PCode', schopts: { qryopts: "{ mode: 'Like', field: 'PCode' }"} },
                { fieldLabel: '开始时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '结束时间', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
               ]
            });
            tlBar = new Ext.ux.AimToolbar({
                items: [
               {
                   text: '导出Excel',
                   iconCls: 'aim-icon-xls',
                   handler: function() {
                       ExtGridExportExcel(grid, { store: null, title: '标题' });
                   }

               }, '->', '<b>说明：请输入出库单对应的发票号和成本价，系统自动计算成本金额</b>']
            });
            titPanel = new Ext.ux.AimPanel({
                tbar: tlBar,
                items: [schBar]
            });
            var columnarray = [
                    { id: 'DId', dataIndex: 'DId', header: 'DId', hidden: true },
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Number', dataIndex: 'Number', header: '出库单号', width: 130, sortable: true },
                    { id: 'CName', dataIndex: 'CName', header: '客户名称', width: 120, sortable: true, renderer: RowRender },
					{ id: 'PCode', dataIndex: 'PCode', header: '产品型号', width: 150, sortable: true },
				    { id: 'PName', dataIndex: 'PName', header: '产品名称', width: 100, sortable: true },
				    { id: 'PCN', dataIndex: 'PCN', header: 'PCN', width: 80, sortable: true },
				    { id: 'Count', dataIndex: 'Count', header: '数量', width: 50 },
					{ id: 'Unit', dataIndex: 'Unit', header: '单位', width: 50 },
					{ id: 'CostPrice', dataIndex: 'CostPrice', header: '成本价', width: 80, renderer: RowRender },
					{ id: 'CostAmount', dataIndex: 'CostAmount', header: '成本金额', width: 100, renderer: RowRender,
					    summaryType: 'sum', summaryRenderer: function(v) { return AmountFormat(Math.round(parseFloat(v) * 100) / 100); }
					},
					{ id: 'InvoiceNo', dataIndex: 'InvoiceNo', header: '<label style="color: Red">发票号码 </label>', width: 80, editor: { xtype: 'textfield', allowBland: false} },
					{ id: 'NewCostPrice', dataIndex: 'NewCostPrice', header: '<label style="color: Red">当前成本价 </label>', width: 80,
					    editor: { xtype: 'numberfield', allowBland: false }, renderer: RowRender
					}
					 ];
            grid = new Ext.ux.grid.AimEditorGridPanel({
                title: '配件销售成本表',
                store: store,
                region: 'center',
                autoExpandColumn: 'PCode',
                columns: columnarray,
                bbar: pgBar,
                tbar: titPanel,
                plugins: new Ext.ux.grid.GridSummary(),
                listeners: { afteredit: function(e) {
                    if (e.record.get("NewCostPrice") && e.record.get("InvoiceNo")) {
                        $.ajaxExec("Update", { id: e.record.get("Id"), InvoiceNo: e.record.get("InvoiceNo"), NewCostPrice: e.record.get("NewCostPrice") },
                       function(rtn) {
                           e.record.set("CostPrice", e.record.get("NewCostPrice"));
                           var costamount = Math.round(parseFloat(e.record.get("Count")) * parseFloat(e.record.get("NewCostPrice")) / parseFloat(1.17) * 100) / 100;
                           e.record.set("CostAmount", costamount);
                           e.record.commit();
                       });
                    }
                }
                }
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        function onExecuted() {
            store.reload();
        }
        function opencenterwin(url, name, iWidth, iHeight) {
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
            window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn = "";
            switch (this.id) {
                case "CorrespondBillNo":
                    if (value) {
                        rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"../FinanceManagement/OtherPayBillView.aspx?id=" +
                                  record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                    }
                    break;
                case "CostPrice":
                case "CostAmount":
                case "NewCostPrice":
                    if (value) {
                        rtn = AmountFormat(value);
                    }
                    break;
                case "CName":
                    cellmeta.attr = 'ext:qtitle="" ext:qtip="' + value + '"';
                    rtn = value;
                    break;
            }
            return rtn;
        }
        function AmountFormat(value) {
            val = String(value);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return "￥" + whole;
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <label style="color: Red">
    </label>
</asp:Content>
