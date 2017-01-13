<%@ Page Title="库存盘点" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="StockCheckCheck.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.StockCheckCheck" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });
        var mdls, tbar, grid, store;
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
            var style = "dialogWidth:800px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "StockCheckProductSelect.aspx?seltype=multi&rtntype=array&WarehouseId=" + $("#WarehouseId").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = store.recordType;
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ ProductId: users[i].ProductId, ProductName: users[i].ProductName, ProductCode: users[i].ProductCode,
                        ProductPcn: users[i].ProductPcn, StockQuantity: users[i].StockQuantity
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
                    { id: 'StockCheckQuantity', header: '<label style="color:red">盘点库存</label>', dataIndex: 'StockCheckQuantity', width: 100,
                        editor: { xtype: 'textfield', allowBlank: false, minValue: 0 }, renderer: RowRender
                    },
					{ id: 'StockCheckState', dataIndex: 'StockCheckState', header: '盘点状态', width: 70 },
                    { id: 'StockCheckResult', dataIndex: 'StockCheckResult', header: '盘点结果', width: 70 }
                ]
            });
            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                title: '盘点明细',
                store: store,
                cm: cm,
                renderTo: "div1",
                width: Ext.get("div1").getWidth(),
                height: 200,
                forceLayout: true,
                columnLines: true,
                autoExpandColumn: 'ProductCode'
            });
            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("div1").getWidth());
            };
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "StockCheckQuantity":
                    rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + (value != null ? value : "");
                    break;
                case "Remark":
                    if (value) {
                        cellmeta.attr = "ext:qtitle=''" + " ext:qtip='" + value + "'";
                        rtn = value;
                    }
                    break;
            }
            return rtn;
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
                    <input id="WarehouseName" name="WarehouseName" readonly="readonly" />
                    <input type="hidden" id="WarehouseId" name="WarehouseId" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    盘点人
                </td>
                <td class="aim-ui-td-data">
                    <input id="StockCheckUserName" name="StockCheckUserName" readonly="readonly" />
                    <input id="StockCheckUserId" name="StockCheckUserId" type="hidden" />
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
