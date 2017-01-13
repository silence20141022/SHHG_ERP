<%@ Page Title="出库商品选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master"
    AutoEventWireup="true" CodeBehind="OrderProductSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.OrderProductSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;

        function onSelPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["ProductList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'ProductList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' },
			    { name: 'OId' },
			    { name: 'PId' },
			    { name: 'Isbn' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Unit' },
			    { name: 'Count' },
			    { name: 'OutCount' },
			    { name: 'NoOutCount' },
			    { name: 'SalePrice' },
			    { name: 'Remark' }
			], listeners: { "aimbeforeload": function(proxy, options) {
			    options.data.paids = getQueryString("paids");
			}
			}
            });

            // 分页栏
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

                // 搜索栏
                schBar = new Ext.ux.AimSchPanel({
                    store: store,
                    columns: 2,
                    items: [
                { fieldLabel: '规格型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '商品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"}}]
                });

                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: [
                    //                { text: '选择', iconCls: 'aim-icon-accept', handler: function() {
                    //                    AimGridSelect();
                    //                }
                    //            },
                 '<font color=red>请点击复选框选择/取消选择记录</font>', '->',
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
                grid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    //checkOnly: true,
                    autoExpandColumn: 'Name',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'OId', dataIndex: 'OId', header: '订单Id', hidden: true },
                    { id: 'Isbn', dataIndex: 'Isbn', header: 'Isbn', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
					{ id: 'Code', dataIndex: 'Code', header: '规格型号', renderer: ExtGridpperCase, width: 130, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '商品名称', width: 100, sortable: true },
					{ id: 'SalePrice', dataIndex: 'SalePrice', header: '售价', width: 80, sortable: true },
					{ id: 'Count', dataIndex: 'Count', header: '销售数量', width: 80, sortable: true },
					{ id: 'OutCount', dataIndex: 'OutCount', header: '已出库数量', width: 80, sortable: true, hidden: true },
					{ id: 'NoOutCount', dataIndex: 'NoOutCount', header: '待出库数量', width: 80, sortable: true, renderer: function(v, p, r) {
					    if (r.get("OutCount"))
					        return r.get("Count") - r.get("OutCount");
					    else
					        return r.get("Count");
					}
					},
					{ id: 'Unit', dataIndex: 'Unit', header: '单位', width: 80, sortable: true },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 80, sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
                });
                AimSelGrid = grid;

                Ext.override(Ext.grid.CheckboxSelectionModel, {
                    handleMouseDown: function(g, rowIndex, e) {
                        if (e.button !== 0 || this.isLocked()) {
                            return;
                        }
                        var view = this.grid.getView();
                        if (e.shiftKey && !this.singleSelect && this.last !== false) {
                            var last = this.last;
                            this.selectRange(last, rowIndex, e.ctrlKey);
                            this.last = last; // reset the last     
                            view.focusRow(rowIndex);
                        } else {
                            var isSelected = this.isSelected(rowIndex);
                            if (isSelected) {
                                this.deselectRow(rowIndex);
                            } else if (!isSelected || this.getCount() > 1) {
                                this.selectRow(rowIndex, true);
                                view.focusRow(rowIndex);
                            }
                        }
                    }
                });
                
                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, grid, buttonPanel]
                });
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            标题</h1>
    </div>
</asp:Content>
