<%@ Page Title="物流信息" Language="C#" MasterPageFile="~/Masters/Ext/formpage.master"
    AutoEventWireup="true" CodeBehind="LogisticsView.aspx.cs" Inherits="Aim.Examining.Web.LogisticsManagement.LogisticsView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        function onPgLoad() {
            InitEditTable();
        }
        function InitEditTable() {

            myData = {
                records: AimState["DataList"],
               total: AimSearchCrit["RecordCount"]
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DataList',
                data: myData,
                idProperty: 'Id',
                fields: [
                { name: 'Id' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Unit' },
			    { name: 'OutCount' },
			    { name: 'Remark' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Code', header: '产品型号', dataIndex: 'Code', width: 200, resizable: true, renderer: ExtGridpperCase,
                        summaryRenderer: function(v, params, data) { return "汇总:" }
                    },
                    { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 150, resizable: true },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60, resizable: true },
                    { id: 'OutCount', header: '出库数量', dataIndex: 'OutCount', summaryType: 'sum', width: 80, resizable: true,
                        summaryRenderer: function(v, params, data) { return v }
                    },
                    { id: 'Remark', header: '备注', dataIndex: 'Remark', width: 120, resizable: true }
                ]
            });

            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                width: Ext.get("StandardSub").getWidth(),
                height: 260,
                forceLayout: true,
                columnLines: true,
                viewConfig: {
                    forceFit: true
                },
                plugins: new Ext.ux.grid.GridSummary(),
                autoExpandColumn: 'Remark'
            });
            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            物流详细</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-td-caption">
                        物流公司
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Name" name="Name" />
                    </td>
                    <td class="aim-ui-td-caption">
                        运单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Number" name="Number" />
                    </td>
                    <td class="aim-ui-td-caption">
                        客户
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CustomerName" name="CustomerName" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        重量(千克)
                    </td>
                    <td class="aim-ui-td-data" style="width: 31%;">
                        <input id="Weight" name="Weight" />
                    </td>
                    <td class="aim-ui-td-caption">
                        运费(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Price" name="Price" />
                    </td>
                    <td class="aim-ui-td-caption">
                        保价费(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Insured" name="Insured" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        送货费(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Delivery" name="Delivery" />
                    </td>
                    <td class="aim-ui-td-caption">
                        合计(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Total" name="Total" />
                    </td>
                    <td class="aim-ui-td-caption">
                        收货人
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Receiver" name="Receiver" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        收货人电话
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Tel" name="Tel" />
                    </td>
                    <td class="aim-ui-td-caption">
                        收货人手机
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="MobilePhone" name="MobilePhone" />
                    </td>
                    <td class="aim-ui-td-caption">
                        付款方式
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PayType" name="PayType" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        体积(立方米)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Volume" name="Volume" />
                    </td>
                    <td class="aim-ui-td-caption">
                        托运日期
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SendDate" name="SendDate" />
                    </td>
                    <td class="aim-ui-td-caption">
                    </td>
                    <td class="aim-ui-td-data">
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        地址
                    </td>
                    <td class="aim-ui-td-data" colspan="5">
                        <input id="Address" name="Address" style="width: 350px;" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="5">
                        <textarea id="Remark" name="Remark" cols="50" rows="5"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
