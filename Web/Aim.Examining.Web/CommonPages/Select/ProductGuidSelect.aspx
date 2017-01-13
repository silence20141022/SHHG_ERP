<%@ Page Title="子商品选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductGuidSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.ProductGuidSelect" %>

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
			    { name: 'PCode' },
			    { name: 'PName' },
			    { name: 'GuId' },
			    { name: 'State' }
			], listeners: { "aimbeforeload": function(proxy, options) {
			    options.data.WarehouseId = getQueryString("ProductId");
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
                    columns: 2,
                    items: [
                { fieldLabel: '规格型号', id: 'PCode', schopts: { qryopts: "{ mode: 'Like', field: 'PCode' }"} },
                { fieldLabel: '商品名称', id: 'PName', schopts: { qryopts: "{ mode: 'Like', field: 'PName' }"}}]
                });

                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: [
                    //                { text: '选择', iconCls: 'aim-icon-accept', handler: function() {
                    //                    AimGridSelect();
                    //                }
                    //            }, 
                '<font color=red>请点击复选框选择/取消选择记录</font>'
                , '->',
                {
                    text: '复杂查询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        schBar.toggleCollapse(false);

                        setTimeout("viewport.doLayout()", 50);
                    } }]
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
                    autoExpandColumn: 'PName',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
					{ id: 'PCode', dataIndex: 'PCode', header: '规格型号', width: 120, renderer: ExtGridpperCase, sortable: true },
					{ id: 'PName', dataIndex: 'PName', header: '商品名称', width: 120, sortable: true },
					{ id: 'GuId', dataIndex: 'GuId', header: '唯一编号', width: 120, sortable: true },
					{ id: 'State', dataIndex: 'State', header: '状态', width: 100, sortable: true, renderer: function(val) {
					    if (val) {
					        return "<label style='color:red;'>未售出</label>";
					    }
					    else {
					        return "<label style='color:green;'>已售出</label>";
					    }
					}
					}
                    ],
                    bbar: pgBar,
                    tbar: titPanel
                });
                AimSelGrid = grid;
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
