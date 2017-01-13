<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="FrmWageParent.aspx.cs" Inherits="Aim.Examining.Web.FrmWageParent" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        //js获取地址栏参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        var tabArr = [{ title: "工资发放", href: "FrmUserWageDetail.aspx" },
        { title: "历史记录", href: "FrmWageStage.aspx"}];

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
                        html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src="FrmUserWageDetail.aspx"></iframe>'
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
            员工工资</h1>
    </div>
</asp:Content>
