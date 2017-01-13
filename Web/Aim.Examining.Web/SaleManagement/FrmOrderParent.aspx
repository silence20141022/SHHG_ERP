<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmOrderParent.aspx.cs" Inherits="Aim.Examining.Web.FrmOrderParent" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var tabArr = [{ title: "未完成", href: "FrmOrders.aspx?ftype=0" },
        { title: "已完成", href: "FrmOrders.aspx?ftype=1"}];

        var viewport = null;
        var tab = null;

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            $.each(tabArr, function(i) {
                this.listeners = { activate: handleActivate };
                this.html = "<div style='display:none;'></div>";
            });

            tab = new Ext.TabPanel({
                region: 'north',
                margins: '0 0 0 0',
                activeTab: 0,
                width: document.body.offsetWidth - 5,
                height: 29,
                items: tabArr
            });

            viewport = new Ext.Viewport({
                layout: 'border',
                items: [
                { xtype: 'box', region: 'north', applyTo: 'header', height: 30 },
                    tab, {
                        region: 'center',
                        margins: '0 0 0 0',
                        cls: 'empty',
                        bodyStyle: 'background:#f1f1f1',
                        html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src="FrmOrders.aspx?ftype=0"></iframe>'
}]
            });
        }

        function handleActivate(tab) {
            if (document.getElementById("frameContent")) {
                document.getElementById("frameContent").src = tab.href;
            }
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none;">
        <h1>
            客户信息</h1>
    </div>
</asp:Content>
