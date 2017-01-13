<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="FinanceTab.aspx.cs" Inherits="Aim.Examining.Web.FinanceTab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var tableData, columnData, tabs, tabpanel;
        var CC = $.getQueryString({ ID: 'CC' });
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            Ext.apply(Ext.QuickTips.getQuickTip(), { dismissDelay: 0 });
            tabs = AimState["Tabs"] || [];
            var tabArray = [];
            for (var a = 0; a < tabs.length; a++) {
                var tab = {
                    title: tabs[a],
                    tooltip: a,
                    listeners: { activate: handleActivate },
                    autoScroll: true,
                    border: false,
                    layout: 'border',
                    html: "<div style='display:none;'></div>"
                };
                tabArray.push(tab);
            }
            tabpanel = new Ext.TabPanel({
                enableTabScroll: true,
                border: true,
                region: 'north',
                activeTab: 0,
                items: [tabArray]
            });
            var viewport = new Ext.ux.AimViewport({
                items: [tabpanel, {
                    region: 'center',
                    margins: '-2 0 0 0',
                    cls: 'empty',
                    bodyStyle: 'background:#f1f1f1',
                    html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0"></iframe>'}]
                });
                if (document.getElementById("frameContent")) {
                    frameContent.location.href = "SaleOrderList.aspx?CC=" + CC;
                }
            }
            function handleActivate(tab) {
                if (document.getElementById("frameContent")) {
                    switch (tab.tooltip) {
                        case 0:
                            frameContent.location.href = "SaleOrderList.aspx?Index=" + tabpanel.activeTab.tooltip + "&CC=" + CC;
                            break;
                        case 1:
                            frameContent.location.href = "SaleOrderInvoiceList.aspx?Index=" + tabpanel.activeTab.tooltip + "&CC=" + CC;
                            break;
                        case 2:
                            frameContent.location.href = "SaleOrderInvoiceList.aspx?Index=" + tabpanel.activeTab.tooltip + "&CC=" + CC;
                            break;
                        case 3:
                            frameContent.location.href = "PaymentInvoiceList.aspx?CC=" + CC;
                            break;
                        case 4:
                            frameContent.location.href = "CorrespondBillParent.aspx?Index=" + tabpanel.activeTab.tooltip + "&CC=" + CC;
                            break;
                        case 5:
                            frameContent.location.href = "ArrearageList.aspx?Index=" + tabpanel.activeTab.tooltip + "&CC=" + CC;
                            break;
                    }
                }
            }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
