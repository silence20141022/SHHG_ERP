<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="ShouldPayBillTab.aspx.cs" Inherits="Aim.Examining.Web.FinanceManagement.ShouldPayBillTab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        var tableData, columnData, store, tabs, tlBar, tlBar2, tabpanel;
        var titPanel, grid, viewport;
        //        var EditWinStyle = CenterWin("width=800,height=600,scrollbars=yes");
        //        var EditPageUrl = "ChiefViewHistory.aspx";
        function onPgLoad() {
            setPgUI();
        }
        function setPgUI() {
            // 初始化tooltip
            Ext.apply(Ext.QuickTips.getQuickTip(), { dismissDelay: 0 });
            tabs = AimState["Tabs"] || [];
            tabArray = [];
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
                    frameContent.location.href = "ShouldPayBill.aspx?Index=0";
                }
            }
            function handleActivate(tab) {
                if (document.getElementById("frameContent")) {
                    frameContent.location.href = "ShouldPayBill.aspx?Index=" + tabpanel.activeTab.tooltip;
                }
            }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
