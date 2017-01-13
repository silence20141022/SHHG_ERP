<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="PayBillView.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PayBillView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, grid;
        var id = $.getQueryString({ "ID": "id" });
        function onPgLoad() {
            setPgUI();
            $("#label").text("(" + $("#Symbo").val() + ")");
        }
        function setPgUI() {
            InitEditTable();
        }
        function SuccessSubmit() {
            AimFrm.submit(pgAction, {}, null, SubFinish);
            RefreshClose();
        }
        function SubFinish(args) {
            RefreshClose();
        }
        function InitEditTable() {
            // 表格数据
            myData = {
                records: AimState["DetailList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                fields: [
                { name: 'Id' },
			    { name: 'PayBillId' },
			    { name: 'PurchaseOrderDetailId' },
			    { name: 'PurchaseOrderNo' },
			    { name: 'ProductCode' },
			    { name: 'ProductName' },
			    { name: 'BuyPrice' },
			    { name: 'PayQuantity' },
			    { name: 'Amount' }
			    ]
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                { id: 'Id', dataIndex: 'Id', hidden: true },
                { id: 'PurchaseOrderDetailId', dataIndex: 'PurchaseOrderDetailId', hidden: true },
                { id: 'PayBillId', dataIndex: 'PayBillId', hidden: true },
                new Ext.ux.grid.AimRowNumberer(),
                new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'PurchaseOrderNo', header: '采购单号', dataIndex: 'PurchaseOrderNo', width: 130 },
                { id: 'ProductName', header: '产品名称', dataIndex: 'ProductName', width: 120 },
                { id: 'ProductCode', header: '产品型号', dataIndex: 'ProductCode', width: 200 },
                { id: 'BuyPrice', header: '采购价格', dataIndex: 'BuyPrice', width: 80, renderer: RowRender, summaryRenderer: function(v, params, data) { return "汇总:" } },
                { id: 'PayQuantity', header: '数量', dataIndex: 'PayQuantity', width: 80, summaryType: 'sum', summaryRenderer: function(v, params, data) { return v } },
                { id: 'Amount', header: '金额', dataIndex: 'Amount', width: 100, summaryType: 'sum', renderer: RowRender,
                    summaryRenderer: function(v, params, data) { return $("#Symbo").val() + v } }]
                });
                // 表格面板
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
                        items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">付款单详细信息：</label>']
                    }),
                    autoExpandColumn: 'ProductCode'
                });
                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                };
            }
            function getPrice() {
                var result = 0.0;
                for (var i = 0; i < store.getCount(); i++) {
                    var rec = store.getAt(i);
                    result = FloatAdd(result, rec.get("Amount"));
                }
                $("#PAmount").val(result);
            }
            function funname(val, t) {//返回指定小数位数       
                return val.toString().substring(0, val.toString().indexOf(".") + t + 1);
            }
            function FloatAdd(arg1, arg2) {
                var r1, r2, m;
                try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
                try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
                m = Math.pow(10, Math.max(r1, r2))
                return (arg1 * m + arg2 * m) / m
            }
            function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                var rtn;
                switch (this.id) {
                    case "PurchaseOrderNo":
                        if (value) {
                            rtn = "<label style='color:Blue; cursor:pointer; text-decoration:underline;' onclick='window.open(\"PurchaseOrderView.aspx?id=" +
                                      record.get('Id') + "\",\"wind\",\"" + EditWinStyle + "\")'>" + value + "</label>";
                        }
                        break;
                    case "Amount":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = $("#Symbo").val() + whole;
                        break;
                    case "BuyPrice":
                        val = String(value);
                        var whole = val;
                        var r = /(\d+)(\d{3})/;
                        while (r.test(whole)) {
                            whole = whole.replace(r, '$1' + ',' + '$2');
                        }
                        rtn = $("#Symbo").val() + whole;
                        break;
                }
                return rtn;
            }         
    </script>

    <script language="javascript" type="text/javascript">
        /**********************************************WorkFlow Function Start**************************/
        var permission = {};
        //这里依次统一添加各环节的控件权限
        permission.初审 = { ReadOnly: "", Hidden: "" };
        permission.复审 = { ReadOnly: "", Hidden: "" };

        var StartUserId = "";
        var StartUserName = "";
        function InitUIForFlow() {

            StartUserId = $("#RequestUserId").val();
            StartUserName = $("#RequestUserName").val();
            if (window.parent.AimState["Task"])
                var taskName = window.parent.AimState["Task"].ApprovalNodeName;

            $("#btnSubmit").hide();
            $("#btnCancel").hide();

            ///控制下一步路由
            if (taskName == "确认发布内容") {
                //SetRoute("公司领导",true);//第一个参数为下一步路由,第二个参数为是否禁止重新选择路由
            }

            if (eval("permission." + taskName)) {
                //只读
                var read = eval("permission." + taskName).ReadOnly;
                for (var i = 0; i < read.split(',').length; i++) {
                    var id = read.split(',')[i];
                    if (document.getElementById(id))
                        document.getElementById(id).readOnly = true;
                }
                //隐藏
                var vis = eval("permission." + taskName).Hidden;
                for (var i = 0; i < vis.split(',').length; i++) {
                    var id = vis.split(',')[i];
                    if (document.getElementById(id))
                        document.getElementById(id).style.display = "none";
                }
            }
        }
        //保存流程和提交流程时触发
        function onSave(task) {
            SuccessSubmit();
            //AimFrm.submit(pgAction, { param: "test" }, null, function() { });
        }
        //提交流程时触发
        function onSubmit(task) {

        }
        //获取下一环节用户
        function onGiveUsers(nextName) {
            var users = { UserIds: "", UserNames: "" };
            switch (nextName) {
                case "提交审批":
                    //users.UserIds = $("#PostUserId").val();
                    //users.UserNames = $("#PostUserName").val();
                    break;
            }
            return users;
        }
        //流程结束时触发
        function onFinish(task) {
            //更新任务状态            
            jQuery.ajaxExec('WorkFlowEnd', { "state": "End", "id": id, "ApprovalState": window.parent.document.getElementById("id_SubmitState").value }, function() {
                RefreshClose();
            });
            //AimFrm.submit(pgAction, { param: "finish" }, null, function() { });
        }
        //第一个参数为下一步路由,第二个参数为是否禁止重新选择路由
        function SetRoute(name, flag) {
            window.parent.SetRoute("公司领导", flag);
        }
        /*****************************************************WorkFlow Function End****************************/
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            采购管理=》 付款单信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td colspan="4">
                        <input id="Id" name="Id" />
                        <input id="Symbo" name="Symbo" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        付款单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PayBillNo" name="PayBillNo" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        供应商名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="SupplierName" name="SupplierName" readonly="readonly" style="width: 240px" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        交易币种
                    </td>
                    <td>
                        <input id="MoneyType" name="MoneyType" readonly="readonly" />
                    </td>
                    <td class="aim-ui-td-caption">
                        应付金额
                        <label id="label">
                        </label>
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PAmount" name="PAmount" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td>
                        折扣金额(￥)
                    </td>
                    <td>
                        <input id="DiscountAmount" name="DiscountAmount" readonly="readonly" />
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
                        <textarea id="Remark" name="Remark" style="width: 83.5%" readonly="readonly"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
