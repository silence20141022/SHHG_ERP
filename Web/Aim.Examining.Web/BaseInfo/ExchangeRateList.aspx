<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ExchangeRateList.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.ExchangeRateList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, columnData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                idProperty: 'Id',
                pruneModifiedRecords: true,
                data: myData,
                fields: [{ name: 'Id' }, { name: 'MoneyType' }, { name: 'Rate' }, { name: 'Symbo' }, { name: 'CreateTime' },
                 { name: 'CreateId' }, { name: 'CreateName' }, { name: 'ModifyDate' }, { name: 'ModifyUserId' }, { name: 'ModifyName' },
                 { name: 'Remark'}]
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
                columns: 4,
                items: [
                { fieldLabel: '货币名称', id: 'MoneyType', schopts: { qryopts: "{ mode: 'Like', field: 'MoneyType' }"} },
                { fieldLabel: '货币符号', id: 'Symbo', schopts: { qryopts: "{ mode: 'Like', field: 'Symbo' }"} }
               ]
            });
            // 工具栏
            tlBar = new Ext.ux.AimToolbar({
                items: [{ text: '添加', iconCls: 'aim-icon-add', handler: function() {
                    grid.stopEditing();
                    var rec = new store.recordType({});
                    store.insert(store.data.length, rec);
                }
                }, '->', {
                    text: '查  询',
                    iconCls: 'aim-icon-search',
                    handler: function() {
                        if (!schBar.collapsed) {
                            Ext.ux.AimDoSearch(Ext.getCmp("Code"));
                        }
                        else {
                            schBar.toggleCollapse(false);
                            viewport.doLayout();
                        }
                    }
}]
                });
                // 工具标题
                titPanel = new Ext.ux.AimPanel({
                    tbar: tlBar,
                    items: [schBar]
                });
                // 表格面板
                grid = new Ext.ux.grid.AimEditorGridPanel({
                    store: store,
                    region: 'center',
                    forceFit: true,
                    autoExpandColumn: 'Remark',
                    columns: [
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Id', header: '标识', dataIndex: 'Id', hidden: true },
                    { id: 'MoneyType', header: '货币名称', dataIndex: 'MoneyType', width: 120, sortable: true,
                        editor: { xtype: 'textfield', allowBlank: false }
                    },
                    { id: 'Symbo', header: '货币符号', dataIndex: 'Symbo', width: 100, sortable: true,
                        editor: { xtype: 'textfield', allowBlank: false }
                    },
                    { id: 'Rate', header: '<label style="color:red">汇率</label>', dataIndex: 'Rate', width: 100, sortable: true,
                        editor: { xtype: 'numberfield', minValue: 0, decimalPrecision: 2, allowBlank: false }
                    },
                    { id: 'ModifyDate', header: '最后一次修改时间', dataIndex: 'ModifyDate', width: 130, sortable: true },
                    { id: 'ModifyName', header: '修改人', dataIndex: 'ModifyName', width: 100, sortable: true },
                    { id: 'Remark', header: '备注', dataIndex: 'Remark', width: 100, editor: { xtype: 'textfield', allowBlank: false} }
                    ],
                    bbar: pgBar,
                    tbar: titPanel,
                    listeners: { afteredit: function(e) {
                        var recs = store.getModifiedRecords();
                        var dt = store.getModifiedDataStringArr(recs) || [];
                        jQuery.ajaxExec("batchsave", { data: dt }, function(rtn) {
                            e.record.commit();
                        });
                    }
                    }
                });

                // 页面视图
                viewport = new Ext.ux.AimViewport({
                    items: [grid]
                });
            }
            function filterValue(val) {
                if (val == null || val == "") {
                    return null;
                }
                else {
                    val = String(val);
                    var whole = val;
                    var r = /(\d+)(\d{3})/;
                    while (r.test(whole)) {
                        whole = whole.replace(r, '$1' + ',' + '$2');
                    }
                    return whole;
                }
            }
            // 提交数据成功后
            function onExecuted() {
                store.reload();
            }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
