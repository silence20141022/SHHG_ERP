<%@ Page Title="采购订单通知书" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="PrintOrder.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.PrintOrder" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script language="javascript" src="CheckActivX.js" type="text/javascript"></script>

    <object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
        height="0">
    </object>

    <script language="javascript" type="text/javascript">
        var LODOP = document.getElementById("LODOP"); //这行语句是为了符合DTD规范
        CheckLodop();
    </script>

    <script type="text/javascript">
        function onPgLoad() {
            var tab = document.getElementById("tb");
            var myData = AimState["DetailList"] || [];
            var total = 0.00; var quan = 0;
            if (AimState["DetailList"] && AimState["DetailList"].length > 0) {
                var tr = tab.insertRow(); tr.height = 28; tr.align = "center";
                var td = tr.insertCell(); td.innerHTML = "运货方式"; td.width = "18%";
                var td = tr.insertCell();
                if (AimState["PurchaseOrder"].TransportationMode) {
                    td.innerHTML = AimState["PurchaseOrder"].TransportationMode;
                }
                else {
                    td.innerHTML = "&nbsp";
                }
                td.width = "22%";
                var td = tr.insertCell(); td.innerHTML = "订货日期"; td.width = "15%"; var td = tr.insertCell();
                if (AimState["PurchaseOrder"].OrderDate) {
                    td.innerHTML = AimState["PurchaseOrder"].OrderDate;
                }
                else {
                    td.innerHTML = "&nbsp";
                }
                td.width = "15%";
                var td = tr.insertCell(); td.innerHTML = "要求发货日期"; td.width = "15%";
                var td = tr.insertCell();
                if (AimState["PurchaseOrder"].RequestDeliveryDate) {
                    td.innerHTML = AimState["PurchaseOrder"].RequestDeliveryDate;
                }
                else {
                    td.innerHTML = "&nbsp";
                }
                var tr = tab.insertRow(); tr.height = 28; tr.align = "center";
                var td = tr.insertCell(); td.innerHTML = "产品类别"; var td = tr.insertCell(); td.innerHTML = "型号";
                var td = tr.insertCell(); td.innerHTML = "数量"; var td = tr.insertCell(); td.innerHTML = "单价";
                var td = tr.insertCell(); td.innerHTML = "合计"; td.colSpan = 2;
                for (var i = 0; i < myData.length; i++) {
                    var tr = tab.insertRow();
                    tr.height = 28; tr.align = "center";
                    var td = tr.insertCell(); td.innerHTML = myData[i].Name;
                    var td = tr.insertCell(); td.innerHTML = myData[i].Code;
                    var td = tr.insertCell(); td.innerHTML = myData[i].Quantity;
                    var td = tr.insertCell(); td.innerHTML = myData[i].BuyPrice;
                    var td = tr.insertCell(); td.innerHTML = accMul(myData[i].Quantity, myData[i].BuyPrice);
                    td.colSpan = 2;
                    quan = FloatAdd(quan, myData[i].Quantity);
                    total = FloatAdd(total, accMul(myData[i].Quantity, myData[i].BuyPrice));
                }
                var tr = tab.insertRow(); tr.height = 28; tr.align = "center";
                var td = tr.insertCell(); td.innerHTML = "总计"; td.colSpan = 2;
                var td = tr.insertCell(); td.innerHTML = quan; var td = tr.insertCell(); td.innerHTML = "&nbsp";
                var td = tr.insertCell(); td.innerHTML = total; td.colSpan = 2;
                var tr = tab.insertRow(); var td = tr.insertCell(); td.innerHTML = '贸易合规性调查（此项为必填项，否则订单无效）'; td.colSpan = 6;
                td.style.fontWeight = "bold"; tr.height = 28;
                var tr = tab.insertRow(); var td = tr.insertCell(); td.innerHTML = 'A 本采购订购单为一般储存存货，并无按背对背销售基准与销售订单配对';
                td.colSpan = 6; tr.height = 28;
                var tr = tab.insertRow(); var td = tr.insertCell(); td.innerHTML = ' B 本采购订购单按背对背销售基准与销售订单配对';
                td.colSpan = 6; tr.height = 28;
                var tr = tab.insertRow(); var td = tr.insertCell(); td.innerHTML = ' 如果选B，请批露最终用户名称和最终目的地及最终用途等信息（最多100个汉字）';
                td.colSpan = 6; tr.height = 28; td.style.fontWeight = "bold";
                var tr = tab.insertRow(); tr.height = 28; var td = tr.insertCell(); td.innerHTML = '最终用户名称：'; td.style.fontWeight = "bold";
                var td = tr.insertCell(); td.innerHTML = '&nbsp';
                var td = tr.insertCell(); td.innerHTML = '最终目的地：'; td.style.fontWeight = "bold";
                var td = tr.insertCell(); td.innerHTML = "&nbsp"; td.colSpan = 3;
                var tr = tab.insertRow(); var td = tr.insertCell(); tr.height = 40;
                td.innerHTML = '最终用途：客户或最终用户的生产、服务、销售或执行行为会用于一下所列类别吗？';
                td.colSpan = 4; td.style.fontWeight = "bold";
                var td = tr.insertCell(); td.innerHTML = '□&nbsp&nbsp&nbsp&nbsp是'; td.style.fontWeight = "bold"; td.align = "center";
                var td = tr.insertCell(); td.innerHTML = '□&nbsp&nbsp&nbsp&nbsp否'; td.style.fontWeight = "bold"; td.align = "center";
                var tr = tab.insertRow(); var td = tr.insertCell();
                td.innerHTML = '&nbsp&nbsp□&nbsp&nbsp原子能&nbsp&nbsp□&nbsp&nbsp&nbsp&nbsp生物&nbsp&nbsp□&nbsp&nbsp&nbsp&nbsp化学&nbsp&nbsp□&nbsp&nbsp&nbsp&nbsp导弹研发&nbsp&nbsp□&nbsp&nbsp&nbsp&nbsp军事';
                tr.height = 28; td.colSpan = 6; td.style.fontWeight = "bold";
                var tr = tab.insertRow(); tr.height = 28; var td = tr.insertCell(); td.innerHTML = '如果"是",请在相应的框内打勾,并详细解释:';
                td.colSpan = 6; td.style.fontWeight = "bold";
                var tr = tab.insertRow(); var td = tr.insertCell(); tr.height = 40;
                td.innerHTML = '注意:以上问题任何一个回答"是",客户和最终用途必须评估后确认销售是否可以进行.根据客户的业务、国家、参与多边协定等性质，该客户是“拒绝”或“适当接受”必须在客户文件里注明。';
                td.colSpan = 6; td.style.fontWeight = "bold";
                var tr = tab.insertRow(); tr.height = 28; var td = tr.insertCell(); td.innerHTML = '客户订货特殊说明（如有）：';
                td.colSpan = 6;
                var tr = tab.insertRow(); tr.height = 28;
                var td = tr.insertCell(); td.style.fontWeight = "bold";
                //                td.style.borderLeft = 0;
                //                td.style.borderRight = 0;
                td.innerHTML = "代表人签字："; td.colSpan = 6;
                LODOP.PRINT_INIT("");
                // LODOP.SET_PRINT_PAGESIZE(2, 0, 0, "A4");
                LODOP.ADD_PRINT_HTM(100, 65, 650, 900, document.documentElement.innerHTML);
                //LODOP.SET_PRINT_STYLEA(0, "Horient", 2);
                //LODOP.SET_PRINT_STYLEA(0, "Horient", 3);
                // LODOP.ADD_PRINT_TEXT(532, 45, 100, 25, "：");
                //                LODOP.SET_PRINT_STYLEA(1, "FontSize", 10);
                //                LODOP.SET_PRINT_STYLEA(1, "Bold", 1);
                LODOP.ADD_PRINT_IMAGE(1, 65, 220, 100, '<img src="logo1.jpg" />');
                LODOP.ADD_PRINT_IMAGE(30, 575, 220, 120, '<img src="logo2.jpg" />');
                LODOP.PREVIEW();
                //LODOP.PRINT_DESIGN();
                //LODOP.PRINT_SETUP();
            }
        }
        function FloatAdd(arg1, arg2) {
            var r1, r2, m;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m
        }
        function accMul(arg1, arg2) {
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <table id="tb" width="100%" border="1" style="background-color: White; font-size: 14px;"
        cellpadding="0" cellspacing="0">
        <tr style="height: 38px">
            <td colspan="6" style="font-weight: bold; font-size: 15px; text-align: center">
                艾默生(苏州)订货通知书(指定经销商)
            </td>
        </tr>
        <tr style="height: 28px" align="center">
            <td style="">
                客户名称
            </td>
            <td colspan="3">
                上海宏谷冷冻机有限公司
            </td>
            <td>
                客户代码
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
