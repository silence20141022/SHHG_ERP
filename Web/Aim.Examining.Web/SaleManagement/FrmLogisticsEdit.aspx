<%@ Page Title="物流信息详细" Language="C#" MasterPageFile="~/Masters/Ext/formpage.master"
    AutoEventWireup="true" CodeBehind="FrmLogisticsEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmLogisticsEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        function onPgLoad() {
            setPgUI();

            if (getQueryString("op") == "c" && !$("#CustomerId").val()) {
                $("#CustomerId").val($("#CId").val());
                $("#CustomerName").dataBind($("#CName").val());
            }
        }

        function setPgUI() {

            InitEditTable();
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            ProcGridData();
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }

        function getTotal() {
            var temp = 0.0;

            if ($("#Price").val())
                temp += parseFloat($("#Price").val());
            if ($("#Insured").val())
                temp += parseFloat($("#Insured").val());
            if ($("#Delivery").val())
                temp += parseFloat($("#Delivery").val());

            $("#Total").val(temp);
        }


        // 处理网格数据
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function() {
                subdata.push(this.data);
            });
            var jsonstr = $.getJsonString(subdata);

            $("#Child").val(jsonstr);
        }

        function InitEditTable() {

            var temp = (AimState["DetailList"] || []).length == 0 ? ($.getJsonObj($("#Child").val()) || []) : (AimState["DetailList"] || []);

            // 表格数据
            myData = {
                records: temp
                //records: $.getJsonObj($("#Child").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
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
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, resizable: true, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Code', header: '规格型号', dataIndex: 'Code', width: 120, resizable: true, renderer: ExtGridpperCase, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'Name', header: '商品名称', dataIndex: 'Name', width: 150, resizable: true },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60, resizable: true },
                    { id: 'OutCount', header: '出库数量', dataIndex: 'OutCount', summaryType: 'sum', width: 80, resizable: true },
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
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">出库商品信息：</label>']
                }),
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
            物流信息详细</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="CName" name="CName" />
                        <input id="CId" name="CId" />
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        物流公司
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="Name" aimctrl='select' style="width: 152px;" enumdata="AimState['EDNames']"
                            class="validate[required]" name="Name">
                        </select>
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
                        <input aimctrl='customer' required="true" id="CustomerName" name="CustomerName" relateid="CustomerId" />
                        <input type="hidden" name="CustomerId" id="CustomerId" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        重量
                    </td>
                    <td class="aim-ui-td-data" style="width: 31%;">
                        <input id="Weight" name="Weight" class="validate[required custom[onlyNumber]]" />千克
                    </td>
                    <td class="aim-ui-td-caption">
                        运费
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Price" name="Price" onchange="getTotal()" class="validate[required custom[onlyNumber]]" />元
                    </td>
                    <td class="aim-ui-td-caption">
                        保价费
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Insured" name="Insured" onchange="getTotal()" class="validate[required custom[onlyNumber]]" />元
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        送货费
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Delivery" name="Delivery" onchange="getTotal()" class="validate[required custom[onlyNumber]]" />元
                    </td>
                    <td class="aim-ui-td-caption">
                        合计
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Total" disabled="disabled" name="Total" />元
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
                        <select id="PayType" aimctrl='select' style="width: 152px;" enumdata="AimState['EDPayType']"
                            class="validate[required]" name="PayType">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        地址
                    </td>
                    <td class="aim-ui-td-data" colspan="5">
                        <input id="Address" name="Address" style="width: 350px;" class="validate[required]" />
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
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
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
