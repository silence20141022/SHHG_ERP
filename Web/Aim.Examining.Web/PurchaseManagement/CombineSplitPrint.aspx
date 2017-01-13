<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CombineSplitPrint.aspx.cs"
    Inherits="Aim.Examining.Web.PurchaseManagement.CombineSplitPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>出库单</title>
    <style type="text/css">
        #dtmain td
        {
            border: solid #000 1px;
        }
        td
        {
            font-size: 14px;
            height: 22px;
        }
        b
        {
            font-size: 12px;
        }
    </style> 
    <script src="/js/lib/jquery-1.4.2.min.js" type="text/javascript"></script>

    <object id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
        height="0">
    </object>

    <script language="javascript" type="text/javascript">
        var LODOP = document.getElementById("LODOP"); //这行语句是为了符合DTD规范
        CheckLodop();
        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        window.onload = print;
        function print() {
            LODOP.PRINT_INIT("");
            //            var pagecount = parseInt($("#lblpagecount").text());
            //            for (var i = 1; i <= pagecount; i++) {
            LODOP.NewPage();
            LODOP.ADD_PRINT_URL(20, 18, 750, 600, "CombineSplitPrintContent.aspx?Id=" + getQueryString("Id"));
            //            }
            LODOP.PREVIEW();
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <center>
        加载数据中..请稍后....
        <div id="divpage" style="display: none;">
            <label id="lblpagecount" runat="server">
            </label>
        </div>
    </center>
    </form>
</body>
</html>
