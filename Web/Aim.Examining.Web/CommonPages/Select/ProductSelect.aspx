<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.ProductSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var QualificationEnum, OpPropertyEnum, QualificationGradeEnum;

        var store, viewport;

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
			    { name: 'Isbn' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Pcn' },
			    { name: 'Unit' },
			    { name: 'StockQuantity' },
			    { name: 'DestineCount' },
			    { name: 'BuyPrice' },
			    { name: 'Remark' }
			], listeners: { "aimbeforeload": function(proxy, options) {
			    options.data.WarehouseId = getQueryString("WarehouseId");
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
                    collapsed: false,
                    columns: 3,
                    items: [
                { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"}}]
                });

                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: [
                    //                    { text: '选择', iconCls: 'aim-icon-accept', handler: function() {
                    //                        AimGridSelect();
                    //                    }
                    //                    },
                    '<font color=red>请点击复选框选择/取消选择记录</font>', '->',
                {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("Code"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            setTimeout("viewport.doLayout()", 50);
                        }
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });

                // 表格面板
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Name',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'Isbn', dataIndex: 'Isbn', header: 'Isbn', hidden: true },
                     { id: 'BuyPrice', dataIndex: 'BuyPrice', header: 'BuyPrice', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
					{ id: 'Code', dataIndex: 'Code', header: '产品型号', width: 170, sortable: true, renderer: ExtGridpperCase },
					{ id: 'Name', dataIndex: 'Name', header: '产品名称', width: 110, sortable: true },
					{ id: 'Pcn', dataIndex: 'Pcn', header: 'PCN', width: 110, sortable: true },
					{ id: 'StockQuantity', dataIndex: 'StockQuantity', header: '库存量', width: 60, sortable: true },
					{ id: 'DestineCount', dataIndex: 'DestineCount', header: '可销售数量', width: 70, sortable: true, renderer: function (val, p, r) {
					    var tmp = parseFloat(r.get("StockQuantity")) - parseFloat(val);
					    return tmp > 0 ? tmp : 0;
					}
					},
					{ id: 'Unit', dataIndex: 'Unit', header: '单位', width: 60 }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
                });
                
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
                    items: [{ xtype: 'box', region: 'north', applyTo: 'header', height: 30 }, AimSelGrid, buttonPanel]
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
