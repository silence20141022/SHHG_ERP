var actionPanel;
var viewport;
var panel;
var mask;
var usersText;

function onPgLoad() {
    setPgUI();
}

function setPgUI() {
    mask = new Ext.LoadMask(Ext.getBody(), { msg: "处理中..." });
    mask.show();
    InitTraceBar();
    // 工具栏
    panel = new Ext.Panel({
        id: 'opinionPanel',
        collapsed: false,
        height: 100,
        border: false,
        html: '<font size=2 color=black>填写审批意见:</font><textarea id=textOpinion style=width:99%;height:80px>' + AimState["Task"].Description + '</textarea>'
    });
    usersText = new Ext.Toolbar.TextItem(' ');
    usersText.setText("人员:" + AimState["NextUserNames"] + " ");
    var tlBar = new Ext.Panel({
        region: 'north',
        height: 28,
        items: panel,
        border: false,
        tbar: [new Ext.Toolbar.TextItem('流程下一步：'), {
            xtype: 'combo',
            name: 'submitState',
            id: 'id_SubmitState',
            hiddenName: 'id_SubmitStateH',
            triggerAction: 'all',
            forceSelection: true,
            lazyInit: false,
            editable: false,
            allowBlank: false,
            width: 150,
            store: new Ext.data.SimpleStore({
                fields: ["returnValue", "displayText"],
                data: routeData
            }),
            mode: 'local',
            value: routeData[0][0],
            valueField: "returnValue",
            displayField: "displayText",
            listeners: {
                select: function() {
                    try {
                        $.ajaxExec('getUsers', { TemplateId: AimState["TemplateId"], FlowInstanceId: AimState["InstanceId"], Name: $("#id_SubmitState").val(), CurrentName: AimState["Task"].ApprovalNodeName },
                        function(args) {
                            //AimDlg.show(args.data.NextUserNames);
                            AimState["NextUserIds"] = args.data.NextUserIds;
                            AimState["NextUserNames"] = args.data.NextUserNames;
                            AimState["UserType"] = args.data.NextUserType;
                            AimState["NextNodeName"] = args.data.NextNodeName;
                            usersText.setText("人员:" + AimState["NextUserNames"] + " ");
                        });
                    } catch (ex) {

                    }
                }
            }, anchor: '99%'
        }, usersText
        , '->', {
            text: '暂存',
            iconCls: 'aim-icon-save',
            handler: SaveTask
        }, {
            text: '提交',
            iconCls: 'aim-icon-execute',
            handler: SubmitTask
        }, '-', {
            text: '审批意见',
            id: 'auditOpinion',
            iconCls: 'iconop',
            handler: function() {
                ToolBarToggle();
            } }]
        });
        var tlBarTitle = new Ext.Panel({
            region: 'north',
            contentEl: 'divNorth',
            cls: 'app-header',
            title: AimState["Task"].WorkFlowName + "->" + AimState["Task"].ApprovalNodeName
        });
        formUrl += formUrl.indexOf("?") > 0 ? "&InFlow=T" : formUrl + "?InFlow=T";
        viewport = new Ext.Viewport({
            layout: 'border',
            items: [tlBarTitle, actionPanel,
                    {
                        region: 'center',
                        margins: '0 0 0 0',
                        layout: 'border',
                        bodyStyle: 'background:#f1f1f1',
                        border: false,
                        items: [tlBar, {
                            region: 'center',
                            border: false,
                            html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src = "' + formUrl + '"></iframe>'
}]
}]
        });
        ToolBarToggle();
        //window.setTimeout("ToolBarToggle();actionPanel.toggleCollapse(false);", 4000);
        new Ext.ToolTip({
            target: 'auditOpinion',
            html: '点击填写审批意见'
        });
        mask.hide();
    }
    function ToolBarToggle() {
        panel.toggleCollapse(false);
        panel.collapsed ? panel.el.dom.parentNode.style.display = 'none' : panel.el.dom.parentNode.style.display = '';
        setTimeout("viewport.doLayout()", 100);
    }

    function InitTraceBar() {
        var tasks = eval(AimState["Tasks"]);
        var items = [];
        var item;
        var i = 1;
        var tempName = "";
        $.each(tasks, function() {
            var finishT = this.FinishTime == null ? "[未处理]" : this.FinishTime;
            if (tempName == this.ApprovalNodeName) {
                item.html = item.html.replace("<ul><li>执行人:", "<ul><li>执行人:" + this.Owner + ",");
            }
            else {
                item = new Ext.Panel({
                    frame: true,
                    title: '第' + i + '步 ' + this.ApprovalNodeName,
                    bodyStyle: i == tasks.length ? "background-color:yellow;" : "",
                    collapsible: true,
                    titleCollapse: true,
                    iconCls: 'task',
                    html: '<ul><li>执行人: ' + this.Owner + '</li><li>收到时间: ' + this.CreatedTime + '</li><li>处理时间: ' + finishT + '</li><li>备注: ' + this.Description + '</li></ul>'
                });
            }
            items.push(item);
            tempName = this.ApprovalNodeName;
            i++;
        });
        actionPanel = new Ext.Panel({
            id: 'panelTrace',
            region: 'west',
            iconCls: 'flow',
            title: '任务跟踪',
            contentEl: 'west',
            split: true,
            collapsible: true,
            collapsed: false,
            width: 200,
            autoScroll: true,
            minWidth: 150,
            maxWidth: 300,
            border: true,
            items: items
        });
        return actionPanel;
    }
    function ShowMask() {
        mask.show();
    }
    function HideMask() {
        mask.hide();
    }