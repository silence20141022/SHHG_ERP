<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Default.aspx.cs" Inherits="Aim.Examining.Web.Default"
    Title="管理系统" %>

<%@ Import Namespace="Aim" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <link href="/App_Themes/Ext/ux/TreeGrid/TreeGrid.css" rel="stylesheet" type="text/css" />

    <script src="/js/ext/ux/TreeGrid.js" type="text/javascript"></script>

    <script src="/js/ext/ux/FieldLabeler.js" type="text/javascript"></script>

    <script src="/js/pgfunc-ext-adv.js" type="text/javascript"></script>

    <script src="/js/ext/ux/TabScrollerMenu.js" type="text/javascript"></script>

    <style type="text/css">
        body
        {
            margin: 2px;
            filter: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#FAFBFF,endColorStr=#C7D7FF);
            color: #003399;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        #main
        {
        }
        .table_banner
        {
            filter: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#E0F0F6,endColorStr=#A4C7E3);
        }
        .tab_item
        {
            font-size: 15;
            border: 0px;
            border-right: 1px dotted;
            padding-left: 10px;
            padding-right: 10px; /*background-color:Gray;*/
        }
    </style>

    <script type="text/javascript">

        var mdls, userwindow, messageWin, messagegrid, receiverName, messagestore, attachment;
        var receiverId = null;
        function onPgLoad() {
            mdls = AimState["Modules"] || [];
            setPgUI();
            RefreshSession();
        }
        function RefreshSession() {
            $.ajax({
                type: "GET",
                url: "Default.aspx",
                data: "tag=Refresh",
                success: function(msg) {
                }
            });
            window.setTimeout("RefreshSession()", 900000);
        }
        function setPgUI() {
            var tabArr = new Array();
            var i = 0;
            var FrameHtml = "";
            // 构建tab标签
            $.each(mdls, function() {
                var tab = {
                    title: this["Name"],
                    href: this["Url"],
                    code: this["Code"],
                    listeners: { activate: handleActivate },
                    margins: '0 0 0 0',
                    border: false,
                    layout: 'border',
                    html: "<div style='display:none;'></div>"
                }
                tabArr.push(tab);
            });
            // 用于tab过多时滚动
            var scrollerMenu = new Ext.ux.TabScrollerMenu({
                menuPrefixText: '项目',
                maxText: 15,
                pageSize: 5
            });

            var tabPanel = new Ext.ux.AimTabPanel({
                enableTabScroll: true,
                border: true,
                defaults: { autoScroll: true },
                plugins: [scrollerMenu],
                region: 'north',
                margins: '50 5 0 5', //顶部空70为了放置公司图片<td style='text-align:left'><img src='MessageSystem/contactuser.png' id='imguserwindow' onclick='userwindowshow()'/>
                activeTab: 0,
                width: document.body.offsetWidth - 5,
                height: 10,
                items: tabArr,
                listeners: { 'click': function() { handleActivate(); } },
                itemTpl: new Ext.XTemplate(
                '<li id="{id}" style="overflow:hidden">',
                    '<span class="tab_item" style="margin-top:3px;">',
                        '<span class="x-tab-strip-text" align="center">{text}</span>',
                    '</span>',
                '</li>'
                )
            });
            var html = "<div><font><table width=99%><tr><td style='width:30%; font-size:12px;'>&nbsp;您好&nbsp;<%=UserInfo.Name %>&nbsp;&nbsp;欢迎您使用系统&nbsp;!&nbsp;&nbsp;<span style='font-size:12px;' onclick=\"window.open('/Modules/Office/calendar.htm','_blank')\" style='text-decoration: underline; cursor: hand;'>  今天是 <%=String.Format("{0}月{1}日", DateTime.Now.Month, DateTime.Now.Day) %></span></td><td style='text-align:center; font-size:12px;'>上海融为信息科技有限公司</td></tr></table></div>";
            var bottomBar = new Ext.Toolbar({
                region: 'south',
                frame: true,
                margins: '0 5 0 5',
                height: 28,
                width: document.body.offsetWidth - 5,
                html: html
            });
            var viewport = new Ext.ux.AimViewport({
                layout: 'border',
                items: [tabPanel, {
                    region: 'center',
                    margins: '0 5 0 5',
                    cls: 'empty',
                    bodyStyle: 'background:#f1f1f1',
                    html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0"></iframe>'
                }, bottomBar]
            });

            function handleActivate(tab) {
                tab = tab || tabPanel.getActiveTab();

                if (!tab) {
                    return;
                }

                var url = tab.href;
                // 首页
                if (tab.code.toUpperCase() != "PORTAL") {
                    url = $.combineQueryUrl("/SubPortal3.aspx", "mcode=" + tab.code);
                }
                if (document.getElementById("frameContent"))
                    frameContent.location.href = url;
                else {
                    window.setTimeout("LoadFirstTab('" + url + "');", 100);
                }
                return;
            }  
            }
            function loadstore(rtn) {
                if (rtn.data.unreadmessage && rtn.data.unreadmessage.length > 0) {
                    var idarray = new Array(); //存储消息ID  方便后台改变消息状态                      
                    for (var j = 0; j < rtn.data.unreadmessage.length; j++) {
                        var recType = messagestore.recordType;
                        var rec = new recType(rtn.data.unreadmessage[j]);
                        if (j == 0) { receiverId = rec.get("SenderId"); receiverName = rec.get("SenderName"); }
                        var rowindex = messagestore.data.length;
                        messagestore.insert(rowindex, rec);
                        idarray.push(rec.get("Id"));
                    }
                    jQuery.ajaxExec("chanagestate", { "IdArray": idarray }, function() { });
                }
            }
            function LoadFirstTab(url) {
                if (document.getElementById("frameContent"))
                    frameContent.location.href = url;
                else
                    window.setTimeout("LoadFirstTab('" + url + "');", 100);
            }

            function DoRelogin() {
                window.setTimeout("location.href = '../Login.aspx'", 200);
            } 
            function rowRender(value, cellmeta, record, rowIndex, columnIndex, store) {
                if (value == "" || value == null) {
                    return "";
                }
                else {
                    value = value.substring(0, 36);
                    return "<img style='height:20px;width:20px' src='../MessageSystem/attachment.png'  onclick='window.open(\"../CommonPages/File/DownLoad.aspx?id=" +
                           value + "\",\"wind\",\"width=1200,height=400,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no\")'/>";
                }
            }   
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="main" align="center">
        <div align="center" align="center">
            <table id="__01" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="5" valign="top">
                    </td>
                    <td style="background-color: #e3eaf5; border: 1px solid #9BAFBC;" class="table_banner">
                        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td valign="middle" style="padding: 0px; display: none;">
                                    <img src="images/portal/login/Login_Sliceup_01_new.gif" /><!--Logo-01.gif-->
                                </td>
                                <td valign="middle" style="width: 60%; padding: 10px; vertical-align: middle;" align="left">
                                    <b>上海宏谷冷冻机公司综合业务系统</b>
                                </td>
                                <td align="right" style="padding-right: 10px; width: 40%; height: 50px;">
                                    <div style="font-size: 12px; margin: 5px;">
                                        <asp:LinkButton ID="lnkRelogin" runat="server" OnClick="lnkRelogin_Click">注销</asp:LinkButton>
                                        &nbsp;|&nbsp;
                                        <asp:LinkButton ID="lnkExit" runat="server" OnClick="lnkExit_Click">退出</asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="5" valign="top">
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
