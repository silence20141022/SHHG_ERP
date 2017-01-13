<%@ Page Title="员工工资详细" Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/Ext/Site.Master"
    CodeBehind="FrmUserWageView.aspx.cs" Inherits="Aim.Examining.Web.FrmUserWageView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .x-grid3-cell-inner, .x-grid3-hd-inner
        {
            white-space: normal !important;
        }
        /* */.grid-row-span .x-grid3-row
        {
            border-bottom: 0;
        }
        .grid-row-span .x-grid3-col
        {
            border-bottom: 1px solid gray;
        }
        .grid-row-span .row-span
        {
            border-bottom: 1px solid #fff;
        }
        .grid-row-span .row-span-first
        {
            position: relative;
        }
        .grid-row-span .row-span-first .x-grid3-cell-inner
        {
            position: absolute;
            border-right: 1px solid gray;
            border-bottom: 1px solid gray;
        }
    </style>

    <script type="text/javascript">
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
                records: AimState["UserList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'UserList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'UserID' },
			    { name: 'UserName' },
			    { name: 'WorkNo' },
			    { name: 'LoginName' },
			    { name: 'Wage' },
			    { name: 'Stage' },
			    { name: 'DeptName' },
			    { name: 'Bonus' },
			    { name: 'Total' },
			    { name: 'Remark' },
			    { name: 'CreateId' },
			    { name: 'CreateName' },
			    { name: 'CreateTime' }
			], listeners: { "aimbeforeload": function(proxy, options) {
			    options.data.Stage = getQueryString("Stage");
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
                items: [
                { fieldLabel: '部门', id: 'DeptName', schopts: { qryopts: "{ mode: 'Like', field: 'DeptName' }"} },
                { fieldLabel: '姓名', id: 'UserName', schopts: { qryopts: "{ mode: 'Like', field: 'UserName' }"}}]
                //,{ fieldLabel: '历史记录', id: 'Stage', schopts: { qryopts: "{ mode: 'Like', field: 'Stage' }"}}
            });

            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: ['->',
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
                grid = new Ext.ux.grid.AimEditorGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'UserName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'UserID', dataIndex: 'UserID', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'DeptName', dataIndex: 'DeptName', header: '部门', width: 150, sortable: true, renderer: renderRow },
					{ id: 'UserName', dataIndex: 'UserName', header: '姓名', width: 150, sortable: true },
					{ id: 'WorkNo', dataIndex: 'WorkNo', header: '工号', width: 120, sortable: true },
					{ id: 'Wage', dataIndex: 'Wage', header: '基本工资', width: 100, sortable: true },
					{ id: 'Bonus', dataIndex: 'Bonus', header: '<label style="color:red;">其他</label>', width: 100, sortable: true, editor: { xtype: 'numberfield'} },

					{ id: 'Total', dataIndex: 'Total', header: '合计', width: 100, sortable: true },

					{ id: 'Remark', dataIndex: 'Remark', header: '<label style="color:red;">备注</label>', width: 120, sortable: true, editor: { xtype: 'textarea'} },
					{ id: 'Stage', dataIndex: 'Stage', header: '批次', width: 100, sortable: true },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '发放日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true }
                    ],
                    bbar: pgBar,
                    cls: 'grid-row-span',
                    tbar: titPanel,
                    listeners: { "afteredit": function(val) {
                        if (val.field == "Bonus") {
                            if (val.record.get("Bonus")) {
                                val.record.set("Total", val.record.get("Wage") + val.record.get(val.field));
                            }
                        }
                    }
                    }
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid]
                });
            }

            function renderRow(value, meta, record, rowIndex, colIndex, store) {
                var first = !rowIndex || value !== store.getAt(rowIndex - 1).get("DeptName"),
                last = rowIndex >= store.getCount() - 1 || value !== store.getAt(rowIndex + 1).get("DeptName");
                meta.css += 'row-span' + (first ? ' row-span-first' : '') + (last ? ' row-span-last' : '');
                if (first) {
                    var i = rowIndex + 1;
                    while (i < store.getCount() && value === store.getAt(i).get("DeptName")) {
                        i++;
                    }
                    var rowHeight = 23.5, padding = 0, height = (rowHeight * (i - rowIndex) - padding) + 'px';
                    meta.attr = 'style="height:' + height + ';line-height:' + height + ';"';
                }
                return first ? '<b>' + value + '</b>' : '';
            }

            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
