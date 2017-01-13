<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="OtherInWarehouseView.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.OtherInWarehouseView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store;
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            myData = {
                records: AimState["DetailList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                fields: [
			    { name: 'InWarehouseId' }, { name: 'Remark' }, { name: 'ProductISBN' }, { name: 'NoInQuan' },
			    { name: 'ProductPCN' }, { name: 'ProductCode' }, { name: 'Quantity' }, { name: 'ProductType' }, { name: 'InWarehouseState' },
			    { name: 'ProductName' }, { name: 'Id' }, { name: 'ProductId' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                 { id: 'Id', dataIndex: 'Id', hidden: true },
                 { id: 'InWarehouseId', dataIndex: 'InWarehouseId', hidden: true },
                 { id: 'ProductId', dataIndex: 'ProductId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 100 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode' },
                { id: 'ProductPCN', header: 'PCN', dataIndex: 'ProductPCN', width: 150 },
                { id: 'ProductISBN', header: '条形码', dataIndex: 'ProductISBN', width: 150, summaryRenderer: function(v, params, data) { return "汇总:" } },
                { id: 'Quantity', dataIndex: 'Quantity', header: '应入库数量', width: 75, summaryType: 'sum' },
                { id: 'NoInQuan', dataIndex: 'NoInQuan', header: '未入库数量', width: 75, summaryType: 'sum' },
            	{ id: 'ProductType', dataIndex: 'ProductType', header: '产品类型', width: 70 },
                { id: 'InWarehouseState', dataIndex: 'InWarehouseState', header: '入库状态', width: 70 }
		        ]
            });
            grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                columnLines: true,
                width: Ext.get("StandardSub").getWidth(),
                autoHeight: true,
                plugins: new Ext.ux.grid.GridSummary(),
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">其他入库详细信息：</label>']
                }),
                autoExpandColumn: 'ProductCode'
            });
            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
        function accMul(arg1, arg2) {
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            入库单基本信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-td-caption">
                        入库编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InWarehouseNo" name="InWarehouseNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        供货商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierName" name="SupplierName" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        入库类型
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InWarehouseType" name="InWarehouseType" areadonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption" "">
                        预计到货时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="EstimatedArrivalDate" name="EstimatedArrivalDate" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        入库仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Name" name="Name" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption" "">
                        录入日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateTime" name="CreateTime" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        录入人
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CreateName" name="CreateName" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 83.5%" readonly="readonly"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
