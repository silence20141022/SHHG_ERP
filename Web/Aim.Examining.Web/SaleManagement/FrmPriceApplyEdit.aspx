<%@ Page Title="销售价格申请单" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmPriceApplyEdit.aspx.cs" Inherits="Aim.Examining.Web.FrmPriceApplyEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <script src="/js/proctrack.js" type="text/javascript"></script>
    <script type="text/javascript">

        var isFlow = $.getQueryString({ "ID": "InFlow", "DefaultValue": "" });
        var id = $.getQueryString({ "ID": "id" });
        var index = 0;
        var mdls, tabPanel, subContentPanel;
        function onPgLoad() {
            InitEditTable();
            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function () {
                window.close();
            });
        }
        //验证成功执行保存方法
        function SuccessSubmit() {
            ProcGridData();
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }
        function SubFinish(args) {
            RefreshClose();
        }


        //选择商品
        function MultiAddPros() {
            var style = "dialogWidth:550px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/ProductSelect.aspx?seltype=multi&rtntype=array&PId=" + $("#Id").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function () {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ "Id": users[i].Id, "Isbn": users[i].Isbn, "Code": users[i].Code, "Name": users[i].Name, "MinSalePrice": users[i].MinSalePrice, "Price": users[i].SalePrice, "Count": 1, "Amount": users[i].SalePrice, "Unit": users[i].Unit });
                    insRowIdx = store.data.length;
                    store.insert(insRowIdx, p);
                    //var rec = store.getAt(insRowIdx);
                    grid.startEditing(insRowIdx, 3);
                }
            });
        }


        // 处理网格数据
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function () {
                subdata.push(this.data);
            });
            var jsonstr = $.getJsonString(subdata);

            $("#Child").val(jsonstr);
        }

        function InitEditTable() { 
            myData = {
                // records: $.getJsonObj($("#Child").val()) || []
                records: AimState["DataList"] || []
            }; 
            store = new Ext.ux.data.AimJsonStore({
                data: myData,
                fields: [
                { name: 'Id' },
                // { name: 'Isbn' },
                {name: 'PName' },
			    { name: 'PCode' },
			    { name: 'MinSalePrice' },
			    { name: 'SalePrice' },
			    { name: 'Amount' },
			    { name: 'Unit' },
			    { name: 'Count' },
			    { name: 'Remark' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                {
                    id: 'Id',
                    header: 'Id',
                    dataIndex: 'Id',
                    width: 80,
                    resizable: true,
                    hidden: true
                },
                {
                    id: 'Isbn',
                    header: 'Isbn',
                    dataIndex: 'Isbn',
                    width: 80,
                    resizable: true,
                    hidden: true
                },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),

            {
                header: '规格型号', renderer: ExtGridpperCase,
                dataIndex: 'PCode',
                width: 150,
                resizable: true,
                summaryRenderer: function (v, params, data) { return "汇总:" }
            },
            { 
                header: '商品名称',
                dataIndex: 'PName',
                width: 150,
                resizable: true
            },
            {
                id: 'Unit',
                header: '单位',
                dataIndex: 'Unit',
                width: 100,
                resizable: true
            },
            {
                               header: '最低售价',
                dataIndex: 'MinSalePrice',
                width: 100,
                resizable: true
            },
            { 
                header: '实际售价',
                dataIndex: 'SalePrice',
                width: 100,
                renderer: filterValue,
                resizable: true,
                editor: new Ext.form.NumberField({ allowBlank: false })
            },{
                id: 'Count',
                header: '数量',
                dataIndex: 'Count',
                summaryType: 'sum',
                width: 100,
                resizable: true,
                allowBlank: false,
                editor: new Ext.form.NumberField({ allowBlank: false })
            },
            { id: 'Amount', dataIndex: 'Amount', header: '总金额', summaryType: 'sum', width: 100, summaryRenderer: function (v) { return filterValue(Math.round(v * 100) / 100); }, renderer: filterValue },

            {
                id: 'Remark',
                header: '<label style="color:red;">备注</label>',
                dataIndex: 'Remark',
                width: 100,
                resizable: true,
                editor: new Ext.form.TextArea({})
            }
        ]
            });

            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                //width: 633,
                width: Ext.get("StandardSub").getWidth(),
                height: 300,
                forceLayout: true,
                viewConfig: {
                    forceFit: true
                },
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">子商品信息：</label>', '-', {
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function () {
                            MultiAddPros();
                            return;
                        }
                    }, {
                        text: '删除',
                        iconCls: 'aim-icon-delete',
                        handler: function () {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要删除的记录！");
                                return;
                            }
                            if (confirm("确定删除所选记录？")) {
                                $.each(recs, function () {
                                    store.remove(this);
                                })
                            }
                        }
                    }, {
                        text: '清空',
                        iconCls: 'aim-icon-delete',
                        handler: function () {
                            if (confirm("确定清空所有记录？")) {
                                store.removeAll();
                            }
                        }
                    }]
                }),
                autoExpandColumn: 'Remark',
                listeners: { "afteredit": function (val) {
                    if (val.field == "Price" || val.field == "Count") {
                        val.record.set("Amount", (val.record.get("Price")) * (val.record.get("Count")))
                    }
                }
                }
            });

            window.onresize = function () {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth());
            };
        }

        function filterValue(val) {
            val = String(val);
            var whole = val;
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            return '￥' + (whole == "null" || whole == null ? "" : whole);
        }

        function getSalesman(val) {
            index++;
            if (val && index > 1) {
                jQuery.ajaxExec("getSalesman", { "CId": val }, function (rtn) {
                    $("#CCode").val(rtn.data.Code);
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            价格申请单</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption" width="60px">
                        单号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="Number" name="Number" disabled="disabled" value="自动生成" />
                    </td>
                    <td class="aim-ui-td-caption" width="60px">
                        客户名称
                    </td>
                    <td class="aim-ui-td-data" width="120px">
                        <input aimctrl='customer' required="true" id="CName" name="CName" relateid="CId" />
                        <input type="hidden" name="CId" id="CId" onpropertychange="getSalesman(this.value);" />
                    </td>
                    <td class="aim-ui-td-caption" width="60px">
                        客户编号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="CCode" disabled="disabled" name="CCode" />
                        <input type="hidden" id="PreDeposit" name="PreDeposit" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        申请原因
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Reason" name="Reason" style="width: 100%"></textarea>
                    </td>
                    <td class="aim-ui-td-caption">
                        预计销售时间
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="ExpectedTime" name="ExpectedTime" aimctrl="date" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" style="width: 100%"></textarea>
                    </td>
                    <td class="aim-ui-td-caption">
                    </td>
                    <td class="aim-ui-td-data">
                    </td>
                </tr>
        </table>
        <textarea id="Child" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tr>
                <td class="aim-ui-button-panel">
                    <a id="btnSubmit" class="aim-ui-button submit">保存</a> <a id="btnCancel" class="aim-ui-button cancel">
                        取消</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
