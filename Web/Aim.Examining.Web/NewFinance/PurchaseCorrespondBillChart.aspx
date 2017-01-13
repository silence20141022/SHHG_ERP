<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    Title="" CodeBehind="PurchaseCorrespondBillChart.aspx.cs" Inherits="Aim.Examining.Web.PurchaseCorrespondBillChart" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/FusionChart32/FusionCharts.js" type="text/javascript"></script>

    <script type="text/javascript">
        function onPgLoad() {
            CreateChart(AimState["DataList"]);
        }
        function CreateChart(myData) {
            var categoryarray = new Array();
            for (var i = 0; i < myData.length; i++) {
                categoryarray.push({ label: myData[i].Date })
            }
            var datasetarray = new Array();
            var seriesarray = ['借款金额', '付款金额']; //eval('myData[k].' + fieldarray[j])
            var fieldarray = ['BorrowAmount', 'PayAmount'];
            for (var j = 0; j < seriesarray.length; j++) {
                var dataarray = new Array();
                for (var k = 0; k < myData.length; k++) {
                    dataarray.push({ value: eval('myData[k].' + fieldarray[j]),
                        link: 'j-ShowDetail-' + myData[k].Date
                    });
                }
                datasetarray.push({ SeriesName: seriesarray[j], data: dataarray });
            }
            var jsondata = { chart: { decimalPrecision: '0', caption: "",
                formatNumberScale: '0', placeValuesInside: '0', chartTopMargin: '5', chartBottomMargin: '5', showValues: '1', unescapeLinks: '0'
            },
                categories: { category: categoryarray },
                dataset: datasetarray
            };
            var mychart = new FusionCharts("/FusionChart32/MSColumn3D.swf", "myChartId", Ext.getBody().getWidth(), '350');
            mychart.setJSONData(jsondata);
            mychart.render('div1');
        }
        function opencenterwin(url, name, iWidth, iHeight) {
            var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
            var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
            window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
        }                       
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="div1">
    </div>
</asp:Content>
