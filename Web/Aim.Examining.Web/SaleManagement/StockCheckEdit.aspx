<%@ Page Title="库存盘点" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="StockCheckEdit.aspx.cs" Inherits="Aim.Examining.Web.SaleManagement.StockCheckEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });
        var mdls, tabPanel, grid, store;
        function onPgLoad() {
            setPgUI();
            InitGrid();
        }
        //移除字符串前后空格
        String.prototype.trim = function() {
            return this.replace(/(^\s*)|(\s*$)/g, "");
        }
        function setPgUI() {
            if (pgOperation == "c") {
                $("#StockCheckNo").val(AimState["StockCheckNo"]);
            }
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }
        //验证成功执行保存方法
        function SuccessSubmit() {
            if (store.data.length > 0) {
                var recs = store.getRange();
                var dt = store.getModifiedDataStringArr(recs) || [];
                AimFrm.submit(pgAction, { data: dt }, null, SubFinish);
            }
        }
        function SubFinish(args) {
            RefreshClose();
        }
        //选择商品
        function MultiAddProduct() {
            var style = "dialogWidth:700px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "StockCheckProductSelect.aspx?seltype=multi&rtntype=array&WarehouseId=" + $("#WarehouseId").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = store.recordType;
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ ProductId: users[i].ProductId, ProductName: users[i].ProductName, ProductCode: users[i].ProductCode,
                        ProductPcn: users[i].ProductPcn, StockQuantity: users[i].StockQuantity, StockCheckState: '盘点中'
                    });
                    insRowIdx = store.data.length;
                    store.insert(insRowIdx, p);
                }
            });
        }
        function InitGrid() {
            // 表格数据
            myData = {
                total: AimSearchCrit["RecordCount"],
                records: AimState["DataList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                data: myData,
                fields: [
                { name: 'Id' }, { name: 'ProductName' }, { name: 'ProductCode' }, { name: 'ProductPcn' }, { name: 'StockCheckId' }, { name: 'Remark' },
                { name: 'StockCheckQuantity' }, { name: 'StockQuantity' }, { name: 'StockCheckResult' }, { name: 'StockCheckState' }, { name: 'ProductId' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, hidden: true },
                    { id: 'StockCheckId', header: 'StockCheckId', dataIndex: 'StockCheckId', width: 80, hidden: true },
                    { id: 'ProductId', header: 'ProductId', dataIndex: 'ProductId', width: 80, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 120 },
                    { id: 'ProductCode', header: '产品型号', renderer: ExtGridpperCase, dataIndex: 'ProductCode', width: 100 },
                    { id: 'ProductPcn', header: 'PCN', dataIndex: 'ProductPcn', width: 130 },
                    { id: 'StockQuantity', header: '本仓库存', dataIndex: 'StockQuantity', width: 70 },
                // { id: 'StockCheckQuantity', header: '盘点库存', dataIndex: 'StockCheckQuantity', width: 70, editor: { xtype: 'textfield'} },
					{id: 'StockCheckState', dataIndex: 'StockCheckState', header: '盘点状态', width: 70 }
                ]
            });
            // 表格面板
            grid = new Ext.ux.grid.AimGridPanel({
                title: '盘点明细',
                store: store,
                cm: cm,
                renderTo: "div1",
                width: Ext.get("div1").getWidth(),
                height: 200,
                forceLayout: true,
                columnLines: true,
                tbar: new Ext.Toolbar({
                    items: [{
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function() {
                            if ($("#WarehouseId").val()) {
                                MultiAddProduct();
                            }
                            else {
                                AimDlg.show("请先选择盘点仓库！");
                                return;
                            }
                        }
                    }, {
                        text: '删除',
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要删除的记录！");
                                return;
                            }
                            if (confirm("确定删除所选记录？")) {
                                $.each(recs, function() {
                                    store.remove(this);
                                })
                            }
                        }
}]
                    }),
                    autoExpandColumn: 'ProductCode'
                });
                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("div1").getWidth());
                };
            }            
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            库存管理-》库存盘点</h1>
    </div>
    <div id="editDiv">
        <table class="aim-ui-table-edit">
            <tr style="display: none">
                <td>
                    <input id="Id" name="Id" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    盘点编号
                </td>
                <td class="aim-ui-td-data">
                    <input id="StockCheckNo" name="StockCheckNo" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    仓库名称
                </td>
                <td class="aim-ui-td-data">
                    <input aimctrl='popup' id="WarehouseName" name="WarehouseName" disabled="disabled"
                        class="validate[required]" relateid="txtuserid" popurl="/CommonPages/Select/WarehouseSelect.aspx"
                        popparam="WarehouseId:Id;WarehouseName:Name" popstyle="width=700,height=500" />
                    <input type="hidden" id="WarehouseId" name="WarehouseId" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    盘点人
                </td>
                <td class="aim-ui-td-data">
                    <input id="StockCheckUserId" name="StockCheckUserId" type="hidden" />
                    <input aimctrl="user" id="StockCheckUserName" name="StockCheckUserName" relateid="StockCheckUserId"
                        class="validate[required]" style="width: 152px" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    备注
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <textarea id="Remark" name="Remark" style="height: 80px; width: 385px"></textarea>
                </td>
            </tr>
        </table>
        <div id="div1">
        </div>
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-button-panel">
                    <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">
                        取消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
