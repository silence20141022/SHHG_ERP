<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    Title="其他入库产品选择" CodeBehind="OtherInWarehouseSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.OtherInWarehouseSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["ProductList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'ProductList',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'Isbn' }, { name: 'Code' }, { name: 'Name' }, { name: 'Unit' }, { name: 'SupplierName' },
			    { name: 'Price' }, { name: 'Remark' }, { name: 'Pcn' }, { name: 'Isbn' }, { name: 'ProductType' }
			],
                listeners: { 'aimbeforeload': function(proxy, options) {
                    options.data = options.data || {};
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
                schBar = new Ext.ux.AimSchPanel({
                    store: store,
                    collapsed: false,
                    columns: 4,
                    items: [{ fieldLabel: '产品名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                    { fieldLabel: '产品型号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                    { fieldLabel: '条形码', id: 'Isbn', schopts: { qryopts: "{ mode: 'Like', field: 'Isbn' }"} },
                    { fieldLabel: 'PCN', id: 'Pcn', schopts: { qryopts: "{ mode: 'Like', field: 'Pcn' }"} }
                ]
                });
                tlBar = new Ext.ux.AimToolbar({
                    items: ['<img src="../../images/shared/arrow_right1.png" /><font color=red>说明：双击记录也可以完成选择</font>', '->'
               ]
                });
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Code',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    { id: 'Price', dataIndex: 'Price', header: 'Price', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
                    { id: 'Name', dataIndex: 'Name', header: '产品名称', width: 110, sortable: true },
					{ id: 'Code', dataIndex: 'Code', header: '产品型号', width: 120, sortable: true },
                    { id: 'PCN', dataIndex: 'Pcn', header: 'PCN', width: 100, sortable: true },
                    { id: 'ISBN', dataIndex: 'Isbn', header: '条形码', width: 130, sortable: true },
				    { id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 80, sortable: true },
					{ id: 'Unit', dataIndex: 'Unit', header: '单位', width: 70 }
                    ],
                    tbar: titPanel,
                    bbar: pgBar
                });
                viewport = new Ext.ux.AimViewport({
                    items: [AimSelGrid, buttonPanel]
                });
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
