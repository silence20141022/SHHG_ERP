<%@ Page Language="C#" MasterPageFile="~/Masters/Ext/Site.Master" AutoEventWireup="true"
    CodeBehind="Home3.aspx.cs" Inherits="Aim.Portal.Web.EPC.Home3" Title="��Ŀ��ҳ" %>

<%@ Import Namespace="Aim" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">
    <link href="/App_Themes/Ext/ux/css/Portal.css" rel="stylesheet" type="text/css" />

    <script src="/js/ext/ux/Portal.js" type="text/javascript"></script>

    <script src="/js/ext/ux/PortalColumn.js" type="text/javascript"></script>

    <script src="/js/ext/ux/Portlet.js" type="text/javascript"></script>

    <style type="text/css">
        body
        {
            margin: 2px;
            filter: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#FAFBFF,endColorStr=#C7D7FF);
            color: #003399;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        #header
        {
            background-color: #BAD1EF;
            filter: progid:DXImageTransform.Microsoft.Gradient(gradientType=0,startColorStr=#BAD1EF,endColorStr=#8DB2E3);
        }
        .tab_item
        {
            font-size: 15;
            border: 0px;
            border-right: 1px dotted;
            padding-left: 10px;
            padding-right: 10px; /*background-color:Gray;*/
        }
        .info_panel
        {
            padding: 10px;
            font-size: 12px;
        }
        .info_panel hr
        {
            color: gray;
            height: 1px;
        }
        .panelSearch
        {
        	filter:progid:DXImageTransform.Microsoft.Gradient(startColorStr=#ffffff, endColorStr=#000000, gradientType=0)
        }
    </style>

    <script type="text/javascript">

        var InfoPopupStyle;
        var InfoTmpl;

        function onPgLoad() {
            InfoPopupStyle = CenterWin("width=650,height=500,scrollbars=yes");
            InfoTmpl = buildInfoTemplate();

            setPgUI();
        }

        function setPgUI() {
            var infoPanels = [];

            var panelSearch = new Ext.ux.AimPanel({
                border: true,
                title: "��������",
                paddings: '5 0 0 0 ',
                html: "<table width=100%><tr width=100%><td align=center><select style=width:120px id=selSearch></select></td></tr><tr width=100%><td align=center><input id=txtSearch style=width:120px;/></td></tr><tr width=100%><td align=center><input type=button id=btnSearch value=ִ�� class='aim-ui-button' style='cursor:hand;'/></td></tr></table>"
            });
            infoPanels.push(panelSearch);
            panelSearch = new Ext.ux.AimPanel({
            border: true,
            title: "�����½�",
                paddings: '5 0 0 0 ',
                html: "<table width=100%><tr width=100%><td align=center><select style=width:120px id=selAddNew><option value=>�½�...</option></select></td></tr><tr width=100%><td align=center><input id=btnAddNew type=button value=ִ�� class='aim-ui-button' style='cursor:hand;'/></td></tr></table>"
            });
            infoPanels.push(panelSearch);
            
            /*var pal = buildInfoView({ title: '��˾��Ѷ', url: '/Modules/PubNews/NewsList.aspx?TypeId={id}', mappings: { 'id': 'Id' }, records: ($.getJsonObj(AimState["News"]) || []) });
            infoPanels.push(pal);*/

            pal = buildInfoView({ title: '�칫��Ϣ', url: '', records: [{ title: '�ڲ���Ϣ', count: AimState["MsgCount"] || "0", url: '/Modules/Office/SysMessageTab.aspx'}] });
            infoPanels.push(pal);

            //��ҵ���
            var recordsArr = [];
            recordsArr.push({ title: '�ҵĲ߻�', count: null, url: '/EPC/WBS/PrjWbs.aspx', style: CenterWin("width=800,height=600,scrollbars=yes") });
            pal = buildInfoView({ title: '��ҵ', url: '', records: recordsArr });
            infoPanels.push(pal);

            //��Ŀ���
            recordsArr = [];
            recordsArr.push({ title: '��̱�', count: null, url: '/EPC/Schedule/PRJ_MilestoneList.aspx' });
            recordsArr.push({ title: '�ļ�', count: null, url: '/EPC/Document/DocManager.aspx', style: CenterWin("width=800,height=600,scrollbars=yes") });
            recordsArr.push({ title: '��Ŀ���ñ���', count: null, url: '/EPC/Common/Report/PRJ_Report.aspx?rpt=cost', style: CenterWin("width=800,height=600,scrollbars=yes") });
            //recordsArr.push({ title: '����', count: AimState["MsgCount"] || 0, url: '/Modules/Office/SysMessageList.aspx?TypeId=Receive' });
            //recordsArr.push({ title: '����', count: AimState["MsgCount"] || 0, url: '/Modules/Office/SysMessageList.aspx?TypeId=Receive' });
            //recordsArr.push({ title: '����', count: AimState["MsgCount"] || 0, url: '/Modules/Office/SysMessageList.aspx?TypeId=Receive' });
            pal = buildInfoView({ title: '��Ŀ', url: '', records: recordsArr });
            infoPanels.push(pal);
            portalPanel = new Ext.ux.AimPanel({
                layout: 'border',
                border: false,
                region: 'center',
                /*tbar: { xtype: 'aimtoolbar', items: [{ iconCls: 'aim-icon-user-edit' }, { html: '<b>�ҵĸ�����ҳ</b>', xtype: 'tbtext' }, '->', { iconCls: 'aim-icon-help', tooltip: '����'}] },*/
                items: [
                { region: 'center', autoScroll: true, margins: '2 0 0 0', html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src="/home.aspx"></iframe>' },
                { id: 'infoView', autoScroll: true, iconCls: 'aim-icon-info', region: 'west', width: 200, margins: '2 0 0 0',
                    items: infoPanels
                }
                ]
            });

            // ҳ����ͼ
            viewport = new Ext.ux.AimViewport({
                layout: 'border',
                items: [portalPanel]
            });
            BindFucSelect();
        }
        //��������
        var tempInternalAdd = 0;
        var tempInternalSearch = 0;
        function BindFucSelect() {
            var addNews = AimState["ModulesCreate"];
            for (var i = 0, len = addNews.length; i < len; i++) {
                var option = new Option(addNews[i].Name, addNews[i].Url);
                if ($.browser.msie) {
                    document.getElementById("selAddNew").options.add(option);
                }
                else {
                    document.getElementById("selAddNew").options.add(option);
                }
            }
            $("#btnAddNew").click(function() {
                tempInternalAdd = 0;
                if ($("#selAddNew").val() != "") {
                    document.getElementById("frameContent").src = $("#selAddNew").val();
                    var delayedTask = new Ext.util.DelayedTask();
                    delayedTask.delay(400, AddNewFire, this, [this, $("#txtSearch").val(), $("#selAddNew").val()]);
                }
            });
            var addNews = AimState["ModulesSearch"];
            for (var i = 0, len = addNews.length; i < len; i++) {
                var option = new Option(addNews[i].Name, addNews[i].Url);
                if ($.browser.msie) {
                    document.getElementById("selSearch").options.add(option);
                }
                else {
                    document.getElementById("selSearch").options.add(option);
                }
            }
            $("#btnSearch").click(function() {
                tempInternalSearch = 0;
                document.getElementById("frameContent").src = "/SearchBus.aspx";
                var delayedTask = new Ext.util.DelayedTask();
                delayedTask.delay(400, SearchResult, this, [this, $("#txtSearch").val(), $("#selSearch").val()]);
            });
        }
        function AddNewFire() {
            tempInternalAdd++;
            if (tempInternalAdd == 5) return;
            if (document.getElementById("frameContent").contentWindow && document.getElementById("frameContent").contentWindow.location.href.indexOf(arguments[2]) >= 0 && document.getElementById("frameContent").contentWindow.document.body.readyState == "complete") {
                var inputsall = document.getElementById("frameContent").contentWindow.$("button");
                if (inputsall)
                    $.each(inputsall, function() {
                        if (this.innerText == "���") {
                            this.click();
                        }
                    });
            }
            else {
                var delayedTask = new Ext.util.DelayedTask();
                delayedTask.delay(400, AddNewFire, arguments[0], [arguments[0], arguments[1], arguments[2]]);
            }
        }
        function SearchResult() {
            tempInternalSearch++;
            if (tempInternalSearch == 5) return;
            if (document.getElementById("frameContent").contentWindow && document.getElementById("frameContent").contentWindow.location.href.indexOf(arguments[2])>=0 && document.getElementById("frameContent").contentWindow.document.body.readyState == "complete") {
                if (document.getElementById("frameContent").contentWindow.Ext.getCmp("defaultFullSearch")) {
                    document.getElementById("frameContent").contentWindow.Ext.getCmp("defaultFullSearch").el.dom.value = $("#txtSearch").val();
                    document.getElementById("frameContent").contentWindow.Ext.getCmp("defaultFullSearch").onTrigger2Click(); return;
                }  
                /*
                var inputsall = document.getElementById("frameContent").contentWindow.$("input");
                if(inputsall)
                    $.each(inputsall, function() {
                        if (this.qryopts == "{ type: 'fulltext' }") {
                            this.value = $("#txtSearch").val();
                            document.getElementById("frameContent").contentWindow.Ext.getCmp("defaultFullSearch").onTrigger2Click();
                        }
                    });*/
            }
            else {
                var delayedTask = new Ext.util.DelayedTask();
                delayedTask.delay(400, SearchResult, arguments[0], [arguments[0], arguments[1],arguments[2]]);
            }
        }

        function buildInfoView(opts) {
            var fields = ['title', 'count', 'id', 'url', 'style'];
            var mappings = opts["mappings"] || {};

            var opts = opts || {};
            var store = opts.store;
            var data = opts.data || [];

            if (!store) {
                if (!opts.data && opts.records) {
                    data = { records: opts.records };
                } else {
                    data = opts.data;
                }

                $.each(data.records, function() {
                    for (var key in mappings) {
                        if (mappings[key] && !this[key]) {
                            this[key] = this[mappings[key]];
                        }
                    }
                });

                store = new Ext.ux.data.AimJsonStore({ fields: fields, data: data });
            }

            var tpl = opts.tmpl || buildInfoTemplate(opts);

            var view = new Ext.ux.AimDataView({
                store: store,

                itemSelector: 'p',
                tpl: tpl,
                border: false,
                margins: '5 5 5 5'
            });

            return view;
        }

        function buildInfoTemplate(opts) {
            var opts = opts || {};
            var title = opts.title || '';
            // var colcount = opts.colcount || 2;
            url = $.combineQueryUrl(opts.url, "op=r");

            var tpl = new Ext.XTemplate(
                    '<div class="info_panel">',
                    '<b style="color:black;">' + title + '</b>',
                    '<hr />',
                    '<tpl for=".">',
                    '<span style="width:50%;"><a href="#" onclick=\'OpenWin("',
                    '<tpl if="url">',
                    '{url}',
                    '</tpl>',
                    '<tpl if="!url">',
                    url,
                    '</tpl>',
                    '", "_blank", "',
                    '<tpl if="style">',
                    '{style}',
                    '</tpl>',
                    '<tpl if="!style">',
                    InfoPopupStyle,
                    '</tpl>',
                     '")\'>&nbsp;',
                     '<tpl if="count">',
                    '({count})&nbsp;',
                    '</tpl>',
                    '{title}</a></span>',
                    '</tpl>',
                    '<div>'
                );

            return tpl;
        }
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header" style="display: none">
        <h1>
            �ҵĸ�����ҳ</h1>
    </div>
</asp:Content>
