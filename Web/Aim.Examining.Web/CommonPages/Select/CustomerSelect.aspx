<%@ Page Title="客户选择" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="CustomerSelect.aspx.cs" Inherits="Aim.Examining.Web.CustomerSelect" %>

<%@ OutputCache Duration="1" VaryByParam="None" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/pgfunc-ext-sel.js" type="text/javascript"></script>

    <script type="text/javascript">

        var QualificationEnum, OpPropertyEnum, QualificationGradeEnum;

        var store, viewport;

        function onSelPgLoad() {
            setPgUI();
        }

        function setPgUI() {
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
            // 表格数据
            var myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["WhList"] || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'WhList',
                idProperty: 'ld',
                data: myData,
                fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'PreDeposit' },
			{ name: 'Importance' },
			{ name: 'Remark' }
			]
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
                    items: [{ fieldLabel: '客户编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                        { fieldLabel: '客户名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"}}]
                });

                // 工具栏
                tlBar = new Ext.ux.AimToolbar({
                    items: [
           ]
                });

                // 工具标题栏
                titPanel = new Ext.ux.AimPanel({
                    // tbar: tlBar,
                    items: [schBar]
                });

                // 表格面板
                AimSelGrid = new Ext.ux.grid.AimGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Name',
                    columns: [
                    { id: 'Id', header: '标识', dataIndex: 'Id', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    AimSelCheckModel,
					{ id: 'Code', header: '客户编号', width: 100, sortable: true, dataIndex: 'Code' },
					{ id: 'Name', header: '客户名称', width: 100, sortable: true, dataIndex: 'Name' },
					{ id: 'PreDeposit', header: '预存款', width: 100, sortable: true, dataIndex: 'PreDeposit' },
					{ id: 'Importance', header: '重要程度', width: 100, sortable: true, dataIndex: 'Importance' },
					{ id: 'Remark', header: '描述', width: 200, sortable: true, dataIndex: 'Remark' }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
                });
                viewport = new Ext.ux.AimViewport({
                    layout: 'border',
                    items: [AimSelGrid, buttonPanel]
                });
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
