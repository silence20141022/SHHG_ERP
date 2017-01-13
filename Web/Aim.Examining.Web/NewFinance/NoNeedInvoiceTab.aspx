<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="NoNeedInvoiceTab.aspx.cs" Inherits="Aim.Examining.Web.NoNeedInvoiceTab" %>

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
                    frameContent.location.href = "NoInvoiceSaleOrderList.aspx?Index=0";
                }
            }
            function handleActivate(tab) {
                if (document.getElementById("frameContent")) {
                    switch (tab.tooltip) {
                        case 0:
                            frameContent.location.href = "NoInvoiceSaleOrderList.aspx?Index=" + tabpanel.activeTab.tooltip;
                            break;
                        case 1:
                            frameContent.location.href = "NoInvoiceSaleOrderList.aspx?Index=" + tabpanel.activeTab.tooltip;
                            break;
                        case 2:
                            frameContent.location.href = "NoInvoicePaymentInvoiceList.aspx?Index=" + tabpanel.activeTab.tooltip;
                            break;
                        case 3:
                            frameContent.location.href = "NoInvoiceCorrespondBillList.aspx";
                            break;
                    }
                }
            }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
