<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    CodeBehind="StockCheckView.aspx.cs" Inherits="Aim.Examining.Web.SaleManagement.StockCheckView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var store, myData, grid;
        var id = $.getQueryString({ "ID": "id" });
        function onPgLoad() {
            setPgUI();
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
                    { id: 'StockCheckQuantity', header: '盘点库存', dataIndex: 'StockCheckQuantity', width: 70 },
					{ id: 'StockCheckState', dataIndex: 'StockCheckState', header: '盘点状态', width: 70 },
                    { id: 'StockCheckResult', dataIndex: 'StockCheckResult', header: '盘点结果', width: 70 }
                                   ]
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
                    <input id="StockCheckUserId" name="StockCheckUserId" type="hidden" />
                    <input id="StockCheckUserName" name="StockCheckUserName" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    盘点结果
                </td>
                <td class="aim-ui-td-data">
                    <input id="Result" name="Result" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    备注
                </td>
                <td class="aim-ui-td-data" colspan="3">
                    <textarea id="Remark" name="Remark" style="height: 80px; width: 385px" readonly="readonly"></textarea>
                </td>
            </tr>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
    </div>
</asp:Content>
