<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master" AutoEventWireup="true"
    Title="产品入库" CodeBehind="AcctualInWarehouse.aspx.cs" Inherits="Aim.Examining.Web.StockManagement.AcctualInWarehouse" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var id = $.getQueryString({ ID: "id" });
        var array = new Array();
        var producttype = "";
        String.prototype.trim = function () {
            return this.replace(/(^\s*)|(\s*$)/g, "");
        }
        var notifyurl = "../Common/notify.wav";
        var chimesurl = "../Common/chimes.wav";
        var store, logstore, grid, loggrid;
        var normalcompressor = true;
        var qrcode = false;
        var skinno = "";
        var cur_rec;
        function onPgLoad() {
            setPgUI();
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function () {
                window.close();
            });
        }
        //验证成功执行保存方法
        function SuccessSubmit() {
            $("#btnSubmit").hide();
            var recs = store.getModifiedRecords();
            if (recs && recs.length > 0) {
                for (var k = 0; k < recs.length; k++) {
                    if (recs[k].get("ActuallyQuantity") > recs[k].get("NoIn")) {
                        AimDlg.show("入库数量不能大于未入库的数量！"); return;
                    }
                }
                var dt = store.getModifiedDataStringArr(recs) || [];
                jQuery.ajaxExec('InWarehouse', { "data": dt, "id": id }, function () {
                    AimDlg.show("提交成功！");
                    try {
                        RefreshClose();
                    }
                    catch (e) {
                        window.close();
                    }
                });
            }
            else {
                AimDlg.show("请输入入库数量！"); return;
            }
        }
        function SubFinish(args) {
            RefreshClose();
        }
        function setPgUI() {
            // 表格数据
            myData = {
                records: AimState["DetailList"] || []
            };
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'DetailList',
                data: myData,
                fields: [
                { name: 'Id' }, { name: 'ProductId' }, { name: 'Code' }, { name: 'InWarehouseState' }, { name: 'ISBN' },
                { name: 'Remark' }, { name: 'PurchaseOrderDetailId' }, { name: 'ProductType' }, { name: 'SkinArray' }, { name: 'SeriesArray' },
			    { name: 'Name' }, { name: 'Quantity' }, { name: 'HaveIn' }, { name: 'NoIn' }, { name: 'ActuallyQuantity' }],
                listeners: {
                    'aimbeforeload': function (proxy, options) {
                        options.data = options.data || {};
                        options.data.id = id;
                    }
                }
            });
            logstore = new Ext.ux.data.AimJsonStore({
                //                dsname: 'DetailList',
                //                data: myData,
                fields: [
                 { name: 'Content' }]
            });
            loggrid = new Ext.ux.grid.AimGridPanel({
                store: logstore,
                columns: [{ id: 'Content', dataIndex: 'Content', header: '扫描纪录', renderer: logRender }],
                renderTo: "divLog",
                columnLines: true,
                width: Ext.get("divLog").getWidth(),
                height: 120,
                plugins: new Ext.ux.grid.GridSummary(),
                forceLayout: true,
                autoExpandColumn: 'Content'
            });
            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [{ id: 'Id', header: 'Id', dataIndex: 'Id', hidden: true },
                 { id: 'ProductId', dataIndex: 'ProductId', header: 'ProductId', hidden: true },
                 { id: 'ProductType', dataIndex: 'ProductType', header: 'ProductType', hidden: true },
                 { id: 'SkinArray', dataIndex: 'SkinArray', header: 'SkinArray', hidden: true },
                 { id: 'SeriesArray', dataIndex: 'SeriesArray', header: 'SeriesArray', hidden: true },
                 { id: 'PurchaseOrderDetailId', dataIndex: 'PurchaseOrderDetailId', header: 'PurchaseOrderDetailId', hidden: true },
                 new Ext.ux.grid.AimRowNumberer(),
                 new Ext.ux.grid.AimCheckboxSelectionModel(),
                { id: 'Name', header: '产品名称', dataIndex: 'Name', width: 100, resizable: true },
                { id: 'Code', header: '产品型号', dataIndex: 'Code', resizable: true, summaryRenderer: function (v, params, data) { return "汇总:" } },
                { id: 'ISBN', header: '条形码', dataIndex: 'ISBN', width: 150 },
                { id: 'Quantity', header: '应入库数量', dataIndex: 'Quantity', width: 70, summaryType: 'sum' },
                { id: 'HaveIn', header: '已入库数量', dataIndex: 'HaveIn', width: 70, summaryType: 'sum' },
                { id: 'NoIn', header: '未入库数量', dataIndex: 'NoIn', width: 70, summaryType: 'sum' },
                { id: 'InWarehouseState', header: '入库状态', dataIndex: 'InWarehouseState', width: 70 },
                {
                    id: 'ActuallyQuantity', header: '<label style="color:red;">本次入库数量</label>', dataIndex: 'ActuallyQuantity',
                    width: 80, resizable: true, summaryType: 'sum', renderer: RowRender,
                    editor: new Ext.form.NumberField({ id: 'acctual', minValue: 1 })
                },
                {
                    id: 'Remark', dataIndex: 'Remark', header: '<label style="color:red;">备注</label>', width: 90
                    //editor: { xtype: 'textarea' },renderer: RowRender
                }]
            });
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                columnLines: true,
                width: Ext.get("StandardSub").getWidth(),
                autoHeight: true,
                plugins: new Ext.ux.grid.GridSummary(),
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    items: ['<img src="../../images/shared/arrow_right1.png" /><label style="color:red;">入库单详细信息：</label>', "->", '<label>条形码接收区域</label>&nbsp;<input id="receive" name="receive" style="width: 180px" onkeydown ="ProcessISBN(this.value)"/>']
                }),
                autoExpandColumn: 'Code',
                listeners: {
                    "beforeedit": function (e) { (Ext.getCmp("acctual")).setMaxValue(e.record.get("NoIn")); }
                }
            });
            //很重要的方法。让部分编辑项有效。无效
            grid.getColumnModel().isCellEditable = function (colIndex, rowIndex) {
                var record = store.getAt(rowIndex);
                if (record.get("InWarehouseState") == "已入库") {
                    return false;
                }
                else {
                    return true;
                }
                return Ext.grid.ColumnModel.prototype.isCellEditable.call(this, colIndex, rowIndex);
            }
            window.onresize = function () {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }
        function RowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "ActuallyQuantity":
                    rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
                    break;
                case "Remark":
                    if (value == null) {
                        rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;';
                    }
                    else {
                        rtn = '<img src="../images/shared/user_edit.png" />&nbsp; &nbsp;' + value;
                    }
                    break;
                default: //因为有汇总插件存在 所以存在第三种情形
                    rtn = value;
                    break;
            }
            return rtn;
        }
        function logRender(value, cellmeta, record, rowIndex, columnIndex, store) {
            var rtn;
            switch (this.id) {
                case "Content":
                    rtn = '<font color="red">' + value + '</font>';
                    break;
                default: //因为有汇总插件存在 所以存在第三种情形
                    rtn = value;
                    break;
            }
            return rtn;
        }
        function ProcessISBN(val) {
            if (event.keyCode == 13) {
                switch (array.length) {
                    case 0:
                        firststep(val);
                        break;
                    case 1:
                        if (val != "" && val != null) {
                            if (val != array[0]) {//判断一下避免重复扫了ModelNo
                                if (producttype == "配件") {//如果是配件
                                    if (val > 0 && val < 10000) {
                                        playSound(chimesurl);
                                        AddScanLog("输入了配件的数量：" + val);
                                        var index = store.findExact('ISBN', array[0].toUpperCase());
                                        if (index >= 0) {
                                            var rec = store.getAt(index); //散机入库的时候判断数量上限
                                            if (rec.get("NoIn") < FloatAdd(rec.get("ActuallyQuantity"), val - 1)) {
                                                playSound(notifyurl);
                                                AddScanLog("入库失败！已经超过该配件的入库数量"); array = [];
                                                $("#receive").val("");
                                                return;
                                            }
                                            rec.set("ActuallyQuantity", FloatAdd(rec.get("ActuallyQuantity"), val - 1));
                                            array = [];
                                        }
                                    }
                                    else {//第二次输入的不是数字。
                                        array = [];
                                        firststep(val);
                                    }
                                }
                                else {//如果是压缩机
                                    if (val > 0 && val < 100) {
                                        array.push(val);
                                        playSound(chimesurl);
                                        AddScanLog("扫描的是包装的数量：" + val);
                                    }
                                    else {//如果输入的压缩机的唯一编号SeriesNo
                                        jQuery.ajaxExec("IsIsbn", { Value: val }, function (rtn) {
                                            if (!rtn.data.IsIsbn) { //判断第二次扫描的如果在商品表找不到，就当做压缩机的序列号
                                                array.push(val.toUpperCase());
                                                playSound(chimesurl);
                                                AddScanLog("扫描压缩机序列号：" + val.toUpperCase());
                                                var index = store.findExact('ISBN', array[0].toUpperCase());
                                                if (index >= 0) {
                                                    var rec = store.getAt(index); //散机入库的时候判断数量上限
                                                    if (rec.get("NoIn") < FloatAdd(rec.get("ActuallyQuantity"), 1)) {
                                                        playSound(notifyurl);
                                                        AddScanLog("入库失败！已经超过该型号压缩机的入库数量"); array = [];
                                                        $("#receive").val("");
                                                        return;
                                                    }
                                                    if (rec.get("SeriesArray") && rec.get("SeriesArray").indexOf(val.toUpperCase()) >= 0) {
                                                        playSound(notifyurl);
                                                        AddScanLog("序列号为【" + val.toUpperCase() + "】的压缩机重复扫描！此次扫描无效，请重新开始！");
                                                        array = [];
                                                        $("#receive").val("");
                                                        return;
                                                    }
                                                    if (rec.get("SeriesArray")) {
                                                        rec.set("SeriesArray", rec.get("SeriesArray") + "#" + val.toUpperCase());
                                                    }
                                                    else {
                                                        rec.set("SeriesArray", val.toUpperCase());
                                                    }
                                                    rec.set("ActuallyQuantity", FloatAdd(rec.get("ActuallyQuantity"), 1)); array = [];
                                                }
                                            }
                                            else { //如果扫描的是另一种产品的条码
                                                array = [];
                                                firststep(val);
                                            }
                                        });
                                    }
                                }
                            }
                            else {
                                if (producttype == "压缩机") {
                                    playSound(notifyurl);
                                    AddScanLog("重复扫描条码：" + val);
                                }
                                else {
                                    var index = store.findExact('ISBN', array[0].toUpperCase());
                                    if (index >= 0) {
                                        var rec = store.getAt(index); //散机入库的时候判断数量上限
                                        if (rec.get("NoIn") < FloatAdd(rec.get("ActuallyQuantity"), 1)) {
                                            playSound(notifyurl);
                                            AddScanLog("入库失败！已经超过该配件的入库数量"); array = [];
                                            $("#receive").val("");
                                            return;
                                        }
                                        playSound(chimesurl);
                                        AddScanLog("扫描配件条形码：" + val.toUpperCase());
                                        rec.set("ActuallyQuantity", FloatAdd(rec.get("ActuallyQuantity"), 1));
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        if (val != "" && val != null) {
                            if (val != array[0] && val != array[1]) {//判断是否重复扫描了前面的参数
                                if (qrcode)//如果第一步扫描的是二维码
                                {
                                    val = val.trim().toUpperCase();
                                    if (val.indexOf('[)>06P') >= 0) {//判定一下第二次确实是扫描的序列号
                                        //var index = store.findExact('Code', array[0]);
                                        //var rec = store.getAt(index);
                                        AddScanLog("扫描压缩机序列号：" + val)
                                        cur_rec.set("Remark", cur_rec.get("Remark") + "#" + skinno + "@" + val);
                                        playSound(chimesurl);
                                        //客户2017-08-07提出给入库单明细本次入库数量更新要放在扫描完第二个二维码后执行，不是第一个执行
                                        cur_rec.set("ActuallyQuantity", FloatAdd(cur_rec.get("ActuallyQuantity"), quantity));
                                        array = [];//压缩机序列号扫完后将参数重置
                                        qrcode = false;
                                    }
                                    else {
                                        AddScanLog("二维码扫描模式第二步未扫描到有效的压缩机序列号");
                                        playSound(notifyurl);
                                    }
                                    $("#receive").val(""); 
                                    return;
                                }
                                //alert(array.length);
                                jQuery.ajaxExec("IsIsbn", { Value: val }, function (rtn) {
                                    if (!rtn.data.IsIsbn) { //判断第三次扫描的如果在商品表找不到，且不是100以内的数字    就当做包装号
                                        if (val > 0 && val < 100) {
                                            playSound(notifyurl);
                                            AddScanLog("扫描无效！你扫描的是另外一个包装的数量：" + val);
                                            $("#receive").val("");
                                            return;
                                        }
                                        array.push(val);
                                        AddScanLog("扫描的是包装编号：" + val)
                                        var index = store.findExact('ISBN', array[0].toUpperCase());
                                        if (index >= 0) {
                                            var rec = store.getAt(index);
                                            //这个地方先前台验证 箱号有没有重复
                                            if (rec.get("SkinArray") && rec.get("SkinArray").indexOf(val.toUpperCase()) >= 0) {
                                                AddScanLog("编号为【" + val + "】的包装箱重复扫描！此次扫描无效，请重新开始！"); array = [];
                                                playSound(notifyurl); $("#receive").val("");
                                                return;
                                            }
                                            if ($("#InWarehouseType").val() == "调拨入库" || $("#InWarehouseType").val() == "退货入库") {
                                                if (rec.get("SkinArray")) {
                                                    rec.set("SkinArray", rec.get("SkinArray") + "#" + val.toUpperCase() + "@" + array[1]);
                                                }
                                                else {
                                                    rec.set("SkinArray", val.toUpperCase() + "@" + array[1]);
                                                }
                                                rec.set("ActuallyQuantity", FloatAdd(rec.get("ActuallyQuantity"), array[1]));
                                                array = [];
                                                AddScanLog("包装编号：【" + val.toUpperCase() + "】的压缩机入库成功！");
                                                playSound(chimesurl);
                                            }
                                                //再后台判断 该箱号是否重复 前提是入库类型不是调拨入库     
                                            else {
                                                jQuery.ajaxExec("ScanSkin", { SkinNo: array[2].toUpperCase() }, function (rtn) {
                                                    if (!rtn.data.SkinExist) {
                                                        if (rec.get("SkinArray")) {
                                                            rec.set("SkinArray", rec.get("SkinArray") + "#" + val.toUpperCase() + "@" + array[1]);
                                                        }
                                                        else {
                                                            rec.set("SkinArray", val.toUpperCase() + "@" + array[1]);
                                                        }
                                                        rec.set("ActuallyQuantity", FloatAdd(rec.get("ActuallyQuantity"), array[1]));
                                                        array = [];
                                                        AddScanLog("包装编号：【" + val.toUpperCase() + "】的压缩机入库成功！");
                                                        playSound(chimesurl);
                                                    }
                                                    else {
                                                        AddScanLog("编号为【" + val.toUpperCase() + "】的包装箱已经入库！此次扫描无效，请重新开始！");
                                                        array = [];
                                                        playSound(notifyurl);
                                                    }
                                                });
                                            }
                                        }
                                    }
                                    else { //如果扫描的是另一种产品的条码
                                        array = [];
                                        firststep(val);
                                    }
                                });
                            }
                            if (val == array[0]) {
                                playSound(notifyurl);
                                AddScanLog("重复扫描压缩机条码：" + val);
                            }
                            if (val == array[1]) {
                                playSound(notifyurl);
                                AddScanLog("重复扫描包装中压缩机数量：" + val);
                            }
                        }
                        break;
                }
                $("#receive").val("");
            }
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
        function firststep(val) {
            if (val != "" && val != null) {
                val = val.trim().toUpperCase();
                //2017-07-31 新增代码start 
                //处理二维码的扫描所1数量和箱号之间的分隔是1J 2有序列号都是以S作为分隔
                //demo1 [)>061PVR54KS-TFP-54EQ161JF56A867ZWU94612 
                if (val.indexOf('[)>061P') >= 0)//如果是扫描二维码进来的
                {
                    qrcode = true;
                    // var start=val.indexOf(')>061P');
                    var end = val.indexOf('Q', 18);//防止型号中包含字母Q
                    var end2 = val.indexOf('1J', end);
                    var quantity = val.substring(end + 1, end2);
                    skinno = val.substring(end2 + 2, end2 + 10);
                    val = val.substring(7, end);
                    //alert(val); alert(quantity);alert(skinno)
                    var index = store.findExact('Code', val);
                    cur_rec = store.getAt(index);
                   
                    if (cur_rec.get("SkinArray")) {
                        cur_rec.set("SkinArray", cur_rec.get("SkinArray") + "#" + skinno + "@" + quantity);
                    }
                    else {
                        cur_rec.set("SkinArray", skinno + "@" + quantity);
                    }
                    playSound(chimesurl);
                    $("#receive").val("");
                    array.push(val);
                    array.push(quantity);
                    AddScanLog("扫描型号【" + val + "】箱号【" + skinno + "】数量【" + quantity + "】");
                    return;
                }
                //2017-07-31 新增代码end 
                var index = store.findExact('ISBN', val);
                if (index >= 0) {
                    var rec = store.getAt(index);
                    if (rec.get("InWarehouseState") == "已入库") {
                        playSound(notifyurl);
                        AddScanLog("条码为【" + val + "】产品已入库！");
                        $("#receive").val("");
                        return;
                    }
                    if (rec.get("ProductType") == "配件" || rec.get("ProductType") == "其他") {
                        producttype = "配件";
                        if (rec.get("NoIn") < FloatAdd(rec.get("ActuallyQuantity"), 1)) {
                            playSound(notifyurl);
                            AddScanLog("实际入库数量不能大于未入库的数量"); array = [];
                            $("#receive").val("");
                            return;
                        }
                        else {
                            rec.set("ActuallyQuantity", FloatAdd(rec.get("ActuallyQuantity"), 1));
                            playSound(chimesurl);
                            AddScanLog("扫描配件条形码：" + val.toUpperCase());
                            //需要支持配件入库的时候从扫描枪输入数字 特做如下改动 20120426 数组不清空
                            array.push(val);
                        }
                    }
                    if (rec.get("ProductType") == "压缩机") {
                        producttype = "压缩机";
                        array.push(val);
                        playSound(chimesurl);
                        AddScanLog("扫描压缩机条形码：" + val.toUpperCase());
                    }
                }
                else {//如果入库单中找不到该产品还有一种情形就是扫描的配件的包装  因为包装的条码和包装里面配件的条码是不一样的  然后返回包装中配件的产品条码和数量
                    jQuery.ajaxExec("GetPackageInfo", { Isbn: val }, function (rtn) {
                        if (rtn.data.ProductIsbn && rtn.data.ProductQuan) {
                            var index = store.findExact('ISBN', rtn.data.ProductIsbn);
                            if (index >= 0) {
                                var rec = store.getAt(index);
                                if (rec.get("NoIn") < FloatAdd(rec.get("ActuallyQuantity"), rtn.data.ProductQuan)) {
                                    playSound(notifyurl);
                                    AddScanLog("实际入库数量不能大于未入库的数量");
                                }
                                else {
                                    rec.set("ActuallyQuantity", FloatAdd(rec.get("ActuallyQuantity"), rtn.data.ProductQuan));
                                    AddScanLog("条码为【" + val + "】的配件整箱入库：" + rtn.data.ProductQuan + "！");
                                    playSound(chimesurl);
                                }
                            }
                        }
                        else {
                            playSound(notifyurl);
                            AddScanLog("无效条码：【" + val + "】入库单中未找到该型号的产品！");
                        }
                    });
                }
            }
        }
        function AddScanLog(val) {
            var recType = logstore.recordType;
            var p = new recType({ Content: val });
            logstore.insert(logstore.data.length, p);
            var gridview = loggrid.getView();
            var firstRow = Ext.get(gridview.getRow(0));
            var row = Ext.get(gridview.getRow(logstore.data.length - 1));
            var distance = row.getOffsetsTo(firstRow)[1];
            gridview.scroller.dom.scrollTop = distance;
            //loggrid.getView().scroller.down();
            // loggrid.getView().focusRow(logstore.getCount() - 1);
            //loggrid.getSelectionModel().selectLastRow();
        }
        function playSound(src) {
            var _s = document.getElementById('snd');
            if (src != '' && typeof src != undefined) {
                _s.src = src;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <bgsound id="snd" loop="0" src="">
    <div id="header">
        <h1>
            入库单基本信息</h1>
    </div>
    <div id="editDiv">
        <table class="aim-ui-table-edit">
            <tr style="display: none">
                <td colspan="4">
                    <input id="Id" name="Id" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    入库编号
                </td>
                <td class="aim-ui-td-data">
                    <input id="InWarehouseNo" name="InWarehouseNo" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    供应商名称
                </td>
                <td class="aim-ui-td-data">
                    <input id="SupplierName" name="SupplierName" readonly="readonly" style=" width:300px" />
                    <input id="SupplierId" name="SupplierId" style="display: none" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    入库类型
                </td>
                <td class="aim-ui-td-data">
                    <input id="InWarehouseType" name="InWarehouseType" readonly="readonly" />
                </td>
                <td class="aim-ui-td-caption">
                    预计到货日期
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
                <td class="aim-ui-td-caption">
                    创建时间
                </td>
                <td class="aim-ui-td-data">
                    <input id="CreateTime" name="CreateTime" readonly="readonly" />
                </td>
            </tr>
            <tr>
                <td class="aim-ui-td-caption">
                    创建人
                </td>
                <td class="aim-ui-td-data">
                    <input id="CreateName" name="CreateName" readonly="readonly" />
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
                    <textarea id="Remark" name="Remark" style="width: 84%"></textarea>
                </td>
            </tr>
        </table>
        <div id="divLog" align="left" style="width: 100%;">
        </div>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-button-panel">
                        <a id="btnSubmit" class="aim-ui-button submit">提交</a> <a id="btnCancel" class="aim-ui-button cancel">
                            取消</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
