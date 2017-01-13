<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ExchangeRateSelect.aspx.cs" Inherits="Aim.Examining.Web.CommonPages.Select.ExchangeRateSelect" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var store, myData;
        var pgBar, schBar, tlBar, titPanel, AimSelGrid, viewport;

        function onSelPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["List"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'List',
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'MoneyType' }, { name: 'Symbo' }, { name: 'Rate' }, { name: 'Remark' }
			]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: ['<img src="../../images/shared/arrow_right1.png" /><font color=red>说明：双击行可以直接完成选择</font>']
            });
            var buttonPanel = new Ext.form.FormPanel({
                region: 'south',
                frame: true,
                buttonAlign: 'center',
                buttons: [{ text: '确定', handler: function() { AimGridSelect(); } }, { text: '取消', handler: function() {
                    window.close();
                } }]
                });
                // 表格面板
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    title: '『交易币种列表』',
                    store: store,
                    forceFit: true,
                    //autoHeight: true,
                    region: 'center',
                    // checkOnly: true,
                    autoExpandColumn: 'Remark',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                     new Ext.ux.grid.AimRowNumberer(), AimSelCheckModel,
                    { id: 'MoneyType', dataIndex: 'MoneyType', header: '币种名称', width: 120, sortable: true },
					{ id: 'Symbo', dataIndex: 'Symbo', header: '符号', width: 100 },
					{ id: 'Rate', dataIndex: 'Rate', header: '当前汇率', width: 100 },

					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 120 }
                    ],
                    tbar: tlBar
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [AimSelGrid, buttonPanel]

                });
            }           
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            币种选择</h1>
    </div>
</asp:Content>
