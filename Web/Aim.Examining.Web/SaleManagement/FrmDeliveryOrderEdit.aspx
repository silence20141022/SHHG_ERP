<%@ Page Title="出库单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmDeliveryOrderEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmDeliveryOrderEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/ext/ux/RowExpander.js" type="text/javascript"></script>

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">
        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var index = 0;
        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });

        var mdls, tabPanel, grid, store;

        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            if (getQueryString("type") == "ck") {
                $("#DeliveryUser").val(AimState.UserInfo.Name);
                $("#DeliveryUserId").val(AimState.UserInfo.UserID);
            }

            InitEditTable();

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);

            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            //隐藏提交按钮
            $("#btnSubmit").hide();
            ProcGridData();
            var type = getQueryString("type");
            if (type == "ck") { 
            } 
            var recs = store.getRange();
            var dt = store.getModifiedDataStringArr(recs) || []; 
            AimFrm.submit(pgAction, { "type": type, "data": dt }, null, SubFinish);
        } 
        function SubFinish(args) {
            RefreshClose();
        } 
        function selGuid(rtn) {
            if (!rtn || !rtn.data)
                return;

            var rec = store.getAt(grid.activeEditor.row);
            var Guids = rtn.data || [];
            var temp = "";

            for (var i = 0; i < Guids.length; i++) {
                temp += Guids[i].GuId + ",";
            }
            if (temp.length > 0) {
                temp = temp.substring(0, temp.length - 1);
            }

            rec.set("Guids", temp);
            grid.activeEditor.setValue(temp);
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

            // 表格数据
            myData = {
                records: AimState["DetailList"] || [] //$.getJsonObj($("#Child").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
                { name: 'Id' },
                { name: 'PId' },
                { name: 'ProductId' },
			    { name: 'Isbn' },
			    { name: 'Code' },
			    { name: 'ProductType' },
			    { name: 'Name' },
			    { name: 'StockQuan' },
			    { name: 'BenStockQuan' },
			    { name: 'Guids' },
			    { name: 'Unit' },
			    { name: 'FirstSkinIsbn' },
			    { name: 'dck' },
			    { name: 'SmCount' },
			    { name: 'OutCount' },
			    { name: 'Count' },
			    { name: 'Remark' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },

                columns: [
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, resizable: true, hidden: true },
                    { id: 'PId', header: 'PId', dataIndex: 'PId', width: 80, resizable: true, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(), 
                    {id: 'Code', header: '产品型号', renderer: ExtGridpperCase, dataIndex: 'Code', width: 120, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },

                    { id: 'Isbn', header: '条形码', dataIndex: 'Isbn', width: 100, resizable: true },
                    { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 150, resizable: true },
                    { id: 'FirstSkinIsbn', header: '包装条码', dataIndex: 'FirstSkinIsbn', width: 100, resizable: true },

                    { id: 'ProductType', header: '产品类型', hidden: true, dataIndex: 'ProductType', width: 100, resizable: true },
                    { id: 'Unit', header: '单位', dataIndex: 'Unit', width: 60, resizable: true },
                    { id: 'Count', header: '总出库数量', dataIndex: 'Count', summaryType: 'sum', width: 80, resizable: true },
                    { id: 'OutCount', header: '已出库数量', dataIndex: 'OutCount', summaryType: 'sum', width: 60, resizable: true },
                    { id: 'dck', dataIndex: 'dck', header: '待出库数量', width: 80, summaryType: 'sum' },
                    { id: 'SmCount', header: '本次出库数量', dataIndex: 'SmCount', width: 90, resizable: true },
                    { id: 'BenStockQuan', dataIndex: 'BenStockQuan', header: '本仓存量', width: 80 },
                    { id: 'StockQuan', dataIndex: 'StockQuan', header: '库存量', width: 80 },
                    { id: 'Guids', dataIndex: 'Guids', header: '唯一编号', width: 130 },
                    { id: 'Remark', header: '备注', dataIndex: 'Remark', width: 120, resizable: true }
                ]
            });

            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                //width: 633,
                width: Ext.get("StandardSub").getWidth(),
                height: 300,
                forceLayout: true,
                columnLines: true,
                viewConfig: {
                    forceFit: true
                },
                plugins: [new Ext.ux.grid.GridSummary()], //, expander
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r'
                }),
                autoExpandColumn: 'Remark'
            });

            Ext.onReady(function() {
                var store = grid.getStore();  // Capture the Store.

                var view = grid.getView();    // Capture the GridView.

                grid.tip = new Ext.ToolTip({
                    target: view.mainBody,    // The overall target element.   

                    delegate: '.x-grid3-row', // Each grid row causes its own seperate show and hide.   
                    dismissDelay: 15000,
                    trackMouse: true,         // Moving within the row should not hide the tip.   

                    renderTo: document.body,  // Render immediately so that tip.body can be referenced prior to the first show.   

                    listeners: {              // Change content dynamically depending on which element triggered the show.

                        beforeshow: function updateTipBody(tip) {
                            var rowIndex = view.findRowIndex(tip.triggerElement);
                            var guids = store.getAt(rowIndex).get("Guids") || "";
                            tip.body.dom.innerHTML = "<label style='font-size:12px; line-height:20px;'>唯一编号：<br />" + fomatTooltip(guids.substring(0, guids.length - 1), "") + "</label>";
                        }
                    }
                });
            });

            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }

        //格式化tooltip
        function fomatTooltip(str, tempstr) {
            var temp = tempstr;
            if (str.length > 30) {
                temp += str.substring(0, 30) + "<br />";
                return fomatTooltip(str.substring(30), temp);
            }
            else {
                temp += str;
                return temp;
            }
        }


        //扩展js根据下标删除元素
        Array.prototype.removeAt = function(s) {
            this.splice(s, 1);
        }

        var arry = [];
        var count;

        function addhis(val) {

            if (val.indexOf("成功") > -1) {
                $("#divsmlist").html($("#divsmlist").html() + "<p style='color:green'>" + val + "</p>");
            }
            else {
                $("#divsmlist").html($("#divsmlist").html() + "<p style='color:red'>" + val + "</p>");
            }
            document.getElementById("divsmlist").scrollTop = document.getElementById("divsmlist").scrollHeight;
        }

        //播放声音
        function playSound(src) {
            var _s = document.getElementById('snd');
            if (src != '' && typeof src != undefined) {
                _s.src = src;
            }
        }

        //移除字符串前后空格
        String.prototype.trim = function() {
            return this.replace(/(^\s*)|(\s*$)/g, "");
        }

        //记录配件条码
        var beforeIsbn;
        var beforerow;

        //导入条形码(单改进之后的版本)
        function getProByIsbn(isbn) {
            if (isbn) {
                count = store.getCount();
                var temp;
                var ishas = false; //判断列表是否包含此条码，不包含就是压缩机唯一编号 
                //先判断在不在出库列表里
                for (var i = 0; i < count; i++) {
                    temp = store.getAt(i);
                    if (temp.get("Isbn").toLowerCase().trim() == isbn.toLowerCase().trim()) {
                        ishas = true; 
                        if (temp.get("ProductType") == "压缩机") {
                            addhis("压缩机只需要扫唯一编号即可，条形码：" + isbn);
                            playSound('/wav/msg.wav');
                        }
                        else {//配件(非压缩机)
                            if ((isNaN(parseInt(temp.get("SmCount"))) ? 0 : parseInt(temp.get("SmCount"))) < temp.get("dck")) {
                                temp.set("SmCount", isNaN(parseInt(temp.get("SmCount"))) ? 1 : parseInt(temp.get("SmCount")) + 1);

                                addhis(temp.get("Code") + "出库成功，条码：" + isbn);
                                playSound('/Common/chimes.wav');
                                beforeIsbn = isbn;
                                beforerow = temp;
                            }
                            else {
                                addhis(temp.get("Code") + " 出库数量不能超过购买量");
                                playSound('/wav/Global.wav');
                            }
                        }

                        $("#txtisbn").val("");
                        return;
                    }
                }
                //add by 0425 begin

                //判断是不是配件的数量(数量小于1w)
                if (ishas == false) {
                    var pjcount = isNaN(parseInt(isbn)) ? 0 : parseInt(isbn);
                    if (isbn.length < 5 && pjcount < 10000 && beforerow) {

                        if (parseInt(beforerow.get("SmCount")) + (pjcount - 1) <= beforerow.get("dck")) {
                            beforerow.set("SmCount", parseInt(beforerow.get("SmCount")) + pjcount - 1);

                            addhis(beforerow.get("Code") + "出库成功，条码：" + isbn + " 数量：" + pjcount);
                            playSound('/Common/chimes.wav');

                            beforerow = null;
                            beforeIsbn = "";
                        }
                        else {
                            //数量超出
                            addhis(temp.get("Code") + " 出库数量不能超过购买量");
                            playSound('/wav/Global.wav');
                        }

                        $("#txtisbn").val("");
                        return;
                    }
                }

                //add by 0425 end

                var isfind = false; //判断该条码是否被找到

                //不在出库列表里(压缩机箱号、压缩机唯一编号、配件整箱出库)
                if (ishas == false) {

                    //查询是不是压缩机GUID
                    jQuery.ajaxExec('getprobyguid', { "Guid": isbn }, function(rtn) {
                        if (rtn.data && rtn.data.isbn && rtn.data.isbn != "") {

                            for (var i = 0; i < count; i++) {
                                temp = store.getAt(i);
                                if (temp.get("Isbn").toLowerCase().trim() == rtn.data.isbn.toLowerCase().trim()) {

                                    isfind = true;

                                    if (temp.get("Guids") && temp.get("Guids").indexOf(isbn) > -1) {
                                        //alert("该产品重复扫描");
                                        addhis("该产品重复扫描，条形码：" + isbn);
                                        playSound('/wav/Audio.wav');
                                    }
                                    else if ((isNaN(parseInt(temp.get("SmCount"))) ? 0 : parseInt(temp.get("SmCount"))) < temp.get("dck")) {
                                        temp.set("SmCount", isNaN(parseInt(temp.get("SmCount"))) ? 1 : parseInt(temp.get("SmCount")) + 1);

                                        //添加唯一编号
                                        temp.set("Guids", (temp.get("Guids") && temp.get("Guids") != "null" ? temp.get("Guids") : "") + isbn + ",");

                                        addhis(temp.get("Code") + "出库成功，条码：" + isbn);
                                        playSound('/Common/chimes.wav');
                                    }
                                    else {
                                        addhis(temp.get("Code") + " 出库数量不能超过购买量");
                                        playSound('/wav/Global.wav');
                                    }

                                    $("#txtisbn").val("");
                                    return;
                                }
                            }
                        }

                        //查询是不是压缩机箱号
                        if (isfind == false) {

                            jQuery.ajaxExec('getprobyboxnum', { "boxnum": isbn }, function(rtn) {//返回值 isbn、count、guids
                                if (rtn.data && rtn.data.isbn && rtn.data.isbn != "") {

                                    for (var i = 0; i < count; i++) {
                                        temp = store.getAt(i);
                                        if (temp.get("Isbn").toLowerCase().trim() == rtn.data.isbn.toLowerCase().trim()) {

                                            isfind = true;

                                            if ((isNaN(parseInt(temp.get("SmCount"))) ? 0 : parseInt(temp.get("SmCount"))) + parseInt(rtn.data.count) <= temp.get("dck")) {
                                                temp.set("SmCount", isNaN(parseInt(temp.get("SmCount"))) ? parseInt(rtn.data.count) : parseInt(temp.get("SmCount")) + parseInt(rtn.data.count));

                                                //添加唯一编号
                                                temp.set("Guids", (temp.get("Guids") && temp.get("Guids") != "null" ? temp.get("Guids") : "") + rtn.data.guids); //传过来最后已经有逗号

                                                addhis(temp.get("Code") + "出库成功，条码：" + isbn);
                                                playSound('/Common/chimes.wav');
                                            }
                                            else {
                                                addhis(temp.get("Code") + " 出库数量不能超过购买量，本箱压缩机数量：" + rtn.data.count);
                                                playSound('/wav/Global.wav');
                                            }

                                            $("#txtisbn").val("");
                                            return;
                                        }
                                    }

                                }

                                //查询是不是配件整箱出库
                                if (isfind == false) {

                                    jQuery.ajaxExec('getPJbyboxnum', { "boxnum": isbn }, function(rtn) {//返回值 isbn、count
                                        if (rtn.data && rtn.data.isbn && rtn.data.isbn != "") {
                                            isfind = true;
                                            for (var i = 0; i < count; i++) {
                                                temp = store.getAt(i);
                                                if (temp.get("Isbn").toLowerCase().trim() == rtn.data.isbn.toLowerCase().trim()) {
                                                    if ((isNaN(parseInt(temp.get("SmCount"))) ? 0 : parseInt(temp.get("SmCount"))) + parseInt(rtn.data.count) <= temp.get("dck")) {
                                                        temp.set("SmCount", isNaN(parseInt(temp.get("SmCount"))) ? parseInt(rtn.data.count) : parseInt(temp.get("SmCount")) + parseInt(rtn.data.count));

                                                        addhis(temp.get("Code") + "出库成功，条码：" + isbn);
                                                        playSound('/Common/chimes.wav');
                                                    }
                                                    else {
                                                        addhis(temp.get("Code") + " 出库数量不能超过购买量，本箱配件数量：" + rtn.data.count);
                                                        playSound('/wav/Global.wav');
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (isfind == false) {
                                            addhis("异常条码：" + isbn);
                                            playSound('/wav/system.wav');
                                        }
                                        $("#txtisbn").val("");
                                    }); //end 3
                                }
                            }); //end2
                        }
                    });        //end1
                }
            }
        }

        function getSalesman(val) {
            index++;
            var op = getQueryString("op");
            if ((op == "c" && val && index > 1) || (op == "u" && val && index > 2)) {
                jQuery.ajaxExec("getSalesman", { "CId": val }, function(rtn) {
                    $("#SalesmanId").val(rtn.data.MagId);
                    $("#Salesman").dataBind(rtn.data.MagUser);
                    $("#Address").val(rtn.data.Address);
                    $("#CCode").val(rtn.data.Code);
                    $("#Tel").val(rtn.data.Tel);
                });
            }
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <bgsound id="snd" loop="0" src="">
    <div id="header">
        <h1 style="margin-left: 10px;">
            出库单</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="CorrespondState" type="hidden" name="CorrespondState" />
                        <input id="CorrespondInvoice" type="hidden" name="CorrespondInvoice" />
                        <input id="TotalMoney" type="hidden" name="TotalMoney" />
                        <input id="TotalMoneyHis" type="hidden" name="TotalMoneyHis" />
                        <input id="PId" name="PId" />
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        发货单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Number" name="Number" disabled="disabled" value="自动生成" />
                    </td>
                    <td class="aim-ui-td-caption">
                        客户名称
                    </td>
                    <td class="aim-ui-td-data" width="120px">
                        <input aimctrl='customer' required="true" id="CName" name="CName" relateid="CId" />
                        <input type="hidden" name="CId" id="CId" onpropertychange="getSalesman(this.value);" />
                    </td>
                    <td class="aim-ui-td-caption">
                        客户编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CCode" disabled="disabled" name="CCode" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        联系电话
                    </td>
                    <td class="aim-ui-td-data" width="120px">
                        <input id="Tel" name="Tel" />
                    </td>
                    <td class="aim-ui-td-caption">
                        发货仓库
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="WarehouseName" name="WarehouseName" disabled="disabled"
                            style="width: 158px;" relateid="txtuserid" popurl="/CommonPages/Select/WarehouseSelect.aspx?seltype=multi"
                            popparam="WarehouseId:Id;WarehouseName:Name" popstyle="width=700,height=500"
                            class="validate[required]" />
                        <input type="hidden" id="WarehouseId" name="WarehouseId" />
                    </td>
                    <td class="aim-ui-td-caption">
                        要求到货时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ExpectedTime" aimctrl="date" name="ExpectedTime" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        出库类型
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="DeliveryType" disabled="disabled" name="DeliveryType" value="正常出库" />
                    </td>
                    <td class="aim-ui-td-caption">
                        业务员
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='user' required="true" id="Salesman" name="Salesman" relateid="SalesmanId" />
                        <input type="hidden" id="SalesmanId" name="SalesmanId" />
                    </td>
                    <td class="aim-ui-td-caption">
                        地址
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Address" name="Address" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        提货方式
                    </td>
                    <td class="aim-ui-td-data">
                        <select id="DeliveryMode" aimctrl='select' style="width: 152px;" enumdata="AimState['DeliveryMode']"
                            name="DeliveryMode">
                        </select>
                    </td>
                    <td class="aim-ui-td-caption">
                        出库操作人
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <input id="DeliveryUser"  readonly="readonly" style="width: 182px;" name="DeliveryUser" />
                        <input type="hidden" id="DeliveryUserId" name="DeliveryUserId" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="6">
                        <textarea id="Remark" name="Remark" cols="60" rows="5"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        条码扫描记录
                    </td>
                    <td colspan="3">
                        <div id="divsmlist" style="background-color:White; width:501px; height:80px; overflow:auto; border:solid 1px #8FAACF"></div>
                    </td>
                    <td class="aim-ui-td-caption">
                        条形码
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="txtisbn" type="text" onkeydown="if(event.keyCode==13) getProByIsbn(this.value);">
                    </td>
                </tr>
        </table>
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
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
