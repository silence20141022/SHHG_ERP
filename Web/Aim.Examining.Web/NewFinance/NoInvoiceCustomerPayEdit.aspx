<%@ Page Title="销售收款" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="NoInvoiceCustomerPayEdit.aspx.cs" Inherits="Aim.Examining.Web.NoInvoiceCustomerPayEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <style type="text/css">
        .aim-ui-td-caption
        {
            text-align: right;
        }
        fieldset
        {
            margin: 15px;
            width: 100%;
            padding: 5px;
        }
        fieldset legend
        {
            font-size: 12px;
            font-weight: bold;
        }
        .righttxt
        {
            text-align: right;
        }
        input
        {
            width: 90%;
        }
        select
        {
            width: 90%;
        }
    </style>

    <script type="text/javascript">
        var CId = $.getQueryString({ ID: 'CustomerId' });
        var CName = unescape($.getQueryString({ ID: 'CustomerName' }));
        var SaleAmount = $.getQueryString({ ID: 'SaleAmount' }); //无需开票的类型的收款单  这个字段就代表销售单金额
        var SaleOrderNo = $.getQueryString({ ID: 'SaleOrderNo' }); //销售单号
        var CorrespondType = { 自动销账: '自动销账', 手动销账: '手动销账', 暂不销账: '暂不销账' };
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            if (pgOperation == "c" || pgOperation == "cs") {
                $("#ReceivablesTime").val(jQuery.dateOnly(AimState.SystemInfo.Date));
            }
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
            if (CId) {
                $("#CId").val(CId); $("#CName").val(CName);
            }
            $("#InvoiceAmount").val(SaleAmount);
            $("#CorrespondInvoice").val(SaleOrderNo);
        }
        function SuccessSubmit() {
            if ($("#Name").val() == "手动销账") {
                if (parseFloat($("#InvoiceAmount").val()) != parseFloat($("#Money").val())) {
                    AimDlg.show("手动销账时发票金额和收款金额必须相等！");
                    return;
                }
            }
            if (parseFloat($("#Money").val()) <= 0) {
                AimDlg.show("收款金额必须大于0！");
                return;
            }
            if ($("#Name").val() == "自动销账") {
                $.ajaxExec("AutoCorrespond", { CId: $("#CId").val(), PayAmount: $("#Money").val() }, function(rtn) {
                    if (rtn.data.Result == "T") {
                        AimFrm.submit(pgAction, { CId: $("#CId").val() }, null, SubFinish);
                    }
                    else {
                        AimDlg.show("当前客户的欠款金额小于本次付款金额，不能自动对应,请选择手动销账！");
                        return;
                    }
                })
            }
            else {
                AimFrm.submit(pgAction, { CId: $("#CId").val() }, null, SubFinish);
            }
        }
        function SubFinish(args) {
            RefreshClose();
        }
        function NewPopUp() {
            $("#CorrespondInvoice").next(".aim-ui-button").hide();
            $("#CorrespondInvoice").attr("popurl", "InvoiceSelect.aspx?seltype=multi&CustomerId=" + $("#CId").val());
            new Aim.PopUp("CorrespondInvoice");
        }
        function TypeChange() {
            if ($("#Name").val() == "手动销账") {
                $("#trInvoice").css("display", "block");
            }
            else {
                $("#trInvoice").css("display", "none");
            }
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            销售收款</h1>
    </div>
    <fieldset>
        <legend>基本信息</legend>
        <table class="aim-ui-table-edit" style="border: none" width="100%">
            <tr style="display: none">
                <td>
                    <input id="Id" name="Id" />
                    <input id="CorrespondState" name="CorrespondState" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption" style="width: 15%">
                    收款时间
                </td>
                <td class="aim-ui-td-data" style="width: 35%">
                    <input id="ReceivablesTime" aimctrl="date" name="ReceivablesTime" class="validate[required]" />
                </td>
                <td class="aim-ui-td-caption" style="width: 15%">
                    客户名称
                </td>
                <td class="aim-ui-td-data" style="width: 35%">
                    <input id="CName" name="CName" class="validate[required]" aimctrl='popup' popurl="/CommonPages/Select/CustomerSelect.aspx"
                        popparam="CId:Id;CName:Name" popstyle="width=800,height=550" readonly="readonly" />
                    <input id="CId" name="CId" type="hidden" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    收款金额(￥)
                </td>
                <td class="aim-ui-td-data">
                    <input id="Money" name="Money" value="0" class="validate[required custom[onlyNumber]]" />
                </td>
                <td class="aim-ui-td-caption">
                    收款方式
                </td>
                <td class="aim-ui-td-data">
                    <select id="PayType" name="PayType" aimctrl='select' class="validate[required]" enumdata="AimState['PayType']">
                    </select>
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    销账类型
                </td>
                <td>
                    <select id="Name" name="Name" aimctrl='select' enum="CorrespondType" class="validate[required]"
                        onchange="TypeChange()">
                    </select>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr id="trInvoice" style="display: none">
                <td class="aim-ui-td-caption">
                    销售单号
                </td>
                <td class="aim-ui-td-data">
                    <input id="CorrespondInvoice" name="CorrespondInvoice" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    销售单金额(￥)
                </td>
                <td class="aim-ui-td-data">
                    <input id="InvoiceAmount" name="InvoiceAmount" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    备注
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <textarea id="Remark" name="Remark" style="width: 96%;" rows="3" cols=""></textarea>
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    说明
                </td>
                <td colspan="3">
                    1 财务人员可选择自动销账或者手动销账；2 自动销账会根据该客户的应收款按时间依次销账；3 手动销账必须选择发票号，且发票的累积金额必须和付款金额相同。
                </td>
            </tr>
        </table>
    </fieldset>
    <table class="aim-ui-table-edit">
        <tr>
            <td class="aim-ui-button-panel">
                <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">
                    取消</a>
            </td>
        </tr>
    </table>
</asp:Content>
