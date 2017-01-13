<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.master" AutoEventWireup="true"
    CodeBehind="OtherInWarehouseList.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.OtherInWarehouseList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript">
        var EditWinStyle = CenterWin("width=950,height=600,scrollbars=yes,resizable=no");
        var EditPageUrl = "OtherInWarehouseEdit.aspx";
        var store, myData;
        var pgBar, schBar, tlBar, titPanel, grid, viewport;
        var State = '';
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["InWarehouseList"] || []
            };
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'InWarehouseList',
                idProperty: 'Id',
                data: myData,
                fields: [
			{ name: 'Id' }, { name: 'SupplierId' }, { name: 'SupplierName' }, { name: 'State' }, { name: 'CheckUserName' },
            { name: 'Remark' }, { name: 'CreateId' }, { name: 'CreateTime' }, { name: 'InWarehouseEndTime' },
            { name: 'InWarehouseNo' }, { name: 'InWarehouseType' }, { name: 'CreateName' }, { name: 'EstimatedArrivalDate'}],
                listeners: { "aimbeforeload": function (proxy, options) {
                    options.data = options.data || {};
                    options.data.State = State;
                    State = '';
                }
                }
            });
            pgBar = new Ext.ux.AimPagingToolbar({
                pageSize: AimSearchCrit["PageSize"],
                store: store
            });
            schBar = new Ext.ux.AimSchPanel({
                store: store,
                columns: 4,
                collapsed: false,
                items: [
                { fieldLabel: '产品型号', id: 'ProductCode', schopts: { qryopts: "{ mode: 'Like', field: 'ProductCode' }"} },
                { fieldLabel: '入库编号', id: 'InWarehouseNo', schopts: { qryopts: "{ mode: 'Like', field: 'InWarehouseNo' }"} },
                { fieldLabel: '创建时间', id: 'CreateTimeStart', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreateTimeEnd', schopts: { qryopts: "{ mode: 'GreaterThan', datatype:'Date', field: 'BeginDate' }"} },
                { fieldLabel: '至', id: 'CreateTimeEnd', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreateTimeStart', schopts: { qryopts: "{ mode: 'LessThan', datatype:'Date', field: 'EndDate' }"} }
 ]
            });
            tlBar = new Ext.ux.AimToolbar({
                items: [{
                    text: '添加',
                    iconCls: 'aim-icon-add',
                    handler: function () {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                    }
                }, {
                    text: '修改',
                    iconCls: 'aim-icon-edit',
                    hidden: true,
                    handler: function () {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要修改的记录！");
                            return;
                        }
                        jQuery.ajaxExec("Modify", { id: recs[0].get("Id") }, function (rtn) {
                            if (rtn.data.result && rtn.data.result == "true") {
                                ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                            }
                            else {
                                AimDlg.show("有入库记录的入库单不允许修改！"); return;
                            }
                        });
                    }
                }, {
                    text: '删除',
                    iconCls: 'aim-icon-delete',
                    handler: function () {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            AimDlg.show("请先选择要删除的记录！");
                            return;
                        }
                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, function (rtn) {
                                AimDlg.show(rtn.data.Message); onExecuted();
                            });
                        }
                    }
                }, "-",
                                {
                                    text: '已入库',
                                    iconCls: 'aim-icon-search',
                                    handler: function () {
                                        State = "已入库"; ;
                                        store.reload();
                                    }
                                }, "-", {
                                    text: '未入库',
                                    iconCls: 'aim-icon-search',
                                    handler: function () {
                                        State = "未入库"; ;
                                        store.reload();
                                    }
                                }, "-", {
                                    text: '显示全部',
                                    iconCls: 'aim-icon-search',
                                    handler: function () {
                                        State = "";
                                        store.reload();
                                    }
                                },
                 '->']
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
                autoExpandColumn: 'SupplierName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'InWarehouseNo', dataIndex: 'InWarehouseNo', header: '入库编号', width: 120, sortable: true, renderer: RowRender },
                    { id: 'SupplierName', dataIndex: 'SupplierName', header: '供应商', sortable: true },
					{ id: 'InWarehouseType', dataIndex: 'InWarehouseType', header: '入库类型', width: 90 },
				    { id: 'State', dataIndex: 'State', header: '入库状态', width: 80 },
			    	{ id: 'CreateName', dataIndex: 'CreateName', header: '创建人 ', width: 80 },
					{ id: 'CreateTime', dataIndex: 'CreateTime', header: '创建时间 ', width: 90, sortable: true, renderer: ExtGridDateOnlyRender },
					{ id: 'EstimatedArrivalDate', dataIndex: 'EstimatedArrivalDate', header: '预计到货时间 ', width: 90, renderer: ExtGridDateOnlyRender },
					{ id: 'InWarehouseEndTime', dataIndex: 'InWarehouseEndTime', header: '入库结束时间 ', width: 90, renderer: ExtGridDateOnlyRender },
					{ id: 'CheckUserName', dataIndex: 'CheckUserName', header: '核对人 ', width: 80 },
					{ id: 'Remark', dataIndex: 'Remark', header: '备注', width: 90 }
					],
                bbar: pgBar,
                tbar: titPanel,
                listeners: { rowdblclick: function (grid, rowindex, e) {
                    window.open("OtherInWarehouseView.aspx?id=" + store.getAt(rowindex).get("Id"), "viewinfo", EditWinStyle);
                }
                }
            });
            viewport = new Ext.ux.AimViewport({
                items: [grid]
            });
        }
        // 提交数据成功后
        function onExecuted() {
            store.reload();
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "InWarehouseNo":
                    if (value) {
                        rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"OtherInWarehouseView.aspx?id=" +
                       record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";

                    }
                    break;
                case "chargedetail":
                    if (value) {
                        rtn = "<label style='width:70px'>" + value + "</label>" + "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"ChargeDetail.aspx?ContractCode=" +
                        record.get("相关项目") + "&ContractName=" + record.get("合同名称") + "\",\"wind\",\"width=1200,height=400,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no\")'>查看明细</label>";
                    }
                    else {
                        value = 0;
                        rtn = "<label style='width:70px'>" + value + "</label>" + "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"ChargeDetail.aspx?ContractCode=" +
                        record.get("相关项目") + "&ContractName=" + record.get("合同名称") + "\",\"wind\",\"width=1200,height=400,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no\")'>查看明细</label>";
                    }
                    break;
            }
            return rtn;
        }      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
