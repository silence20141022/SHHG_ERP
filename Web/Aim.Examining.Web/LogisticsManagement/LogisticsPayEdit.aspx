﻿<%@ Page Title="物流信息" Language="C#" MasterPageFile="~/Masters/Ext/formpage.master"
    AutoEventWireup="true" CodeBehind="LogisticsPayEdit.aspx.cs" Inherits="Aim.Examining.Web.LogisticsManagement.LogisticsPayEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            $("#PayBillNo").val(AimState["PayBillNo"]);
            $("#LogisticsCompanyName").val($.getQueryString({ "ID": "LogisticsCompanyName" }));
            $("#ShouldPayAmount").val($.getQueryString({ "ID": "Total" }));
            $("#PayType").val('物流付款');
            $("#InterfaceArray").val(window.opener.ids);
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
            jQuery.ajaxExec("LoadDetail", { "IdArray": $("#InterfaceArray").val() }, function(rtn) { InitialGrid(rtn.data.DataList); });
        }
        function InitialGrid(data) {
            var myData = { records: data || [],
                total: AimSearchCrit["RecordCount"]
            };
            var store = new Ext.ux.data.AimJsonStore({
                dsname: data,
                idProperty: 'Id',
                data: myData,
                fields: [
			    { name: 'Id' }, { name: 'DeliveryId' }, { name: 'DeliveryNumber' }, { name: 'Number' }, { name: 'Name' },
                { name: 'Price' }, { name: 'Weight' }, { name: 'Child' }, { name: 'Remark' }, { name: 'CreateId' },
			    { name: 'CreateName' }, { name: 'CreateTime' }, { name: 'CustomerId' }, { name: 'CustomerName' },
                { name: 'Receiver' }, { name: 'Tel' }, { name: 'MobilePhone' }, { name: 'PayState' }, { name: 'PayType' },
                { name: 'Insured' }, { name: 'Delivery' }, { name: 'Total' }, { name: 'Address' }, { name: 'Volume' }, { name: 'SendDate'}]
            });
            var grid = new Ext.ux.grid.AimGridPanel({
                store: store,
                renderTo: "StandardSub",
                height: 340,
                autoExpandColumn: 'CustomerName',
                columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
					{ id: 'Number', dataIndex: 'Number', header: '物流单号', width: 80 },
					{ id: 'Name', dataIndex: 'Name', header: '物流公司', width: 80, summaryRenderer: function(v, params, data) { return "汇总:" } },
					{ id: 'CustomerName', dataIndex: 'CustomerName', header: '客户名称', width: 160 },

					{ id: 'Weight', dataIndex: 'Weight', header: '重量(kg)', width: 60,
					    summaryType: 'sum', summaryRenderer: function(v, params, data) { return v + 'kg'; }
					},
					{ id: 'Volume', dataIndex: 'Volume', header: '体积(立方米)', width: 80,
					    summaryType: 'sum', summaryRenderer: function(v, params, data) { return v; }
					},
					{ id: 'Price', dataIndex: 'Price', header: '运费(￥)', width: 60,
					    summaryType: 'sum', summaryRenderer: function(v, params, data) { return filterValue(v); }
					},
					{ id: 'Insured', dataIndex: 'Insured', header: '保价费(￥)', width: 70,
					    summaryType: 'sum', summaryRenderer: function(v, params, data) { return filterValue(v); }
					},
					{ id: 'Delivery', dataIndex: 'Delivery', header: '送货费(￥)', width: 70,
					    summaryType: 'sum', summaryRenderer: function(v, params, data) { return filterValue(v); }
					},
					{ id: 'Total', dataIndex: 'Total', header: '合计(￥)', width: 60,
					    summaryType: 'sum', summaryRenderer: function(v, params, data) { return filterValue(v); }
					},
					{ id: 'PayState', dataIndex: 'PayState', header: '付款状态', width: 70 },
					{ id: 'PayType', dataIndex: 'PayType', header: '付款类型', width: 70 },
					{ id: 'SendDate', dataIndex: 'SendDate', header: '托运时间', width: 100, renderer: ExtGridDateOnlyRender }
                    ],
                //                bbar: pgBar,
                //                tbar: titPanel,
                plugins: new Ext.ux.grid.GridSummary()
            });
            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + whole;
        }
        //验证成功执行保存方法
        function SuccessSubmit() {
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            物流付款信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        付款编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PayBillNo" name="PayBillNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        物流公司
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="LogisticsCompanyName" name="LogisticsCompanyName" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        付款类型
                    </td>
                    <td class="aim-ui-td-data" style="width: 31%;">
                        <input id="PayType" name="PayType" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        应付金额(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ShouldPayAmount" name="ShouldPayAmount" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        物流发票号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InvoiceNo" name="InvoiceNo" class="validate[required]" />
                    </td>
                    <td class="aim-ui-td-caption">
                        发票金额(￥)
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="InvoiceAmount" name="InvoiceAmount" class="validate[required]" />
                    </td>
                </tr>
                <tr style="display: none">
                    <td class="aim-ui-td-caption">
                        InterfaceArray
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="InterfaceArray" name="InterfaceArray" cols="50" rows="3"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 72%; height: 45px"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-button-panel" colspan="4">
                    <a id="btnSubmit" class="aim-ui-button submit">提交</a> <a id="btnCancel" class="aim-ui-button cancel">
                        取消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
