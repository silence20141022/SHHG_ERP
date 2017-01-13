<%@ Page Title="商品库存信息维护" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="ProductInventoryEdit.aspx.cs" Inherits="Aim.Examining.Web.BaseInfo.ProductInventoryEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            InitEditTable();
            if (pgOperation == "c" || pgOperation == "cs") {
            }

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            ProcGridData();
            AimFrm.submit(pgAction, { "json": $("#WareSeat").val(), "WarehouseId": $("#WarehouseId").val(), "WarehouseName": $("#WarehouseName").val() }, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }


        // 处理网格数据
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function() {
                subdata.push(this.data);
            });

            var jsonstr = $.getJsonString(subdata);

            $("#WareSeat").val(jsonstr);
        }


        //选择商品
        function MultiAddPros() {
            var style = "dialogWidth:450px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";
            var url = "/CommonPages/Select/ProductSelect.aspx?seltype=multi&rtntype=array&PId=" + $("#Id").val();
            var insRowIdx = store.data.length;
            OpenModelWin(url, {}, style, function() {
                if (this.data == null || this.data.length == 0) return;
                var EntRecord = grid.getStore().recordType;
                grid.stopEditing();
                var users = this.data;
                for (var i = 0; i < users.length; i++) {
                    var p = new EntRecord({ "Id": users[i].Id, "Isbn": users[i].Isbn, "Code": users[i].Code, "Name": users[i].Name, "Unit": users[i].Unit });
                    insRowIdx = store.data.length;
                    store.insert(insRowIdx, p);
                    //var rec = store.getAt(insRowIdx);
                    grid.startEditing(insRowIdx, 3);
                }
            });
        }

        function InitEditTable() {
            // 表格数据
            myData = {
                records: $.getJsonObj($("#WareSeat").val()) || []
            };

            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
                { name: 'Id' },
			    { name: 'Isbn' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Unit' },
			    { name: 'Count' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [{
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
                id: 'Code',
                header: '编号',
                dataIndex: 'Code',
                width: 120,
                resizable: true
            },
            {
                id: 'Name',
                header: '名称',
                dataIndex: 'Name',
                width: 100,
                resizable: true
            },
            {
                id: 'Count',
                header: '<label style="color:red;">数量</label>',
                dataIndex: 'Count',
                width: 100,
                resizable: true,
                editor: new Ext.form.NumberField({})
            },
            {
                id: 'Unit',
                header: '单位',
                dataIndex: 'Unit',
                width: 100,
                resizable: true
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
                height: 480,
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">商品信息：</label>', '-', {
                        text: '添加',
                        iconCls: 'aim-icon-add',
                        handler: function() {
                            MultiAddPros();
                            return;
                        }
                    }, {
                        text: '删除',
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            var recs = grid.getSelectionModel().getSelections();
                            if (!recs || recs.length <= 0) {
                                AimDlg.show("请先选择要删除的记录！");
                                return;
                            }
                            if (confirm("确定删除所选记录？")) {
                                $.each(recs, function() {
                                    store.remove(this);
                                })
                            }
                        }
                    }, {
                        text: '清空',
                        iconCls: 'aim-icon-delete',
                        handler: function() {
                            if (confirm("确定清空所有记录？")) {
                                store.removeAll();
                            }
                        }
}]
                    }),
                    autoExpandColumn: 'Name'
                });

                window.onresize = function() {
                    grid.setWidth(0);
                    grid.setWidth(Ext.get("StandardSub").getWidth());
                };
            }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="header">
        <h1>
            商品库存信息维护</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td colspan="4">
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        仓库名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input aimctrl='popup' id="WarehouseName" name="WarehouseName" disabled="disabled"
                            class="validate[required]" relateid="txtuserid" popurl="/CommonPages/Select/WarehouseSelect.aspx"
                            popparam="WarehouseId:Id;WarehouseName:Name" popstyle="width=700,height=500" />
                        <input type="hidden" id="WarehouseId" name="WarehouseId" />
                    </td>
                </tr>
            </tbody>
        </table>
        <textarea id="WareSeat" rows="5" style="width: 98%" style="display: none;"></textarea>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnSubmit" class="aim-ui-button submit">提交</a> <a id="btnCancel" class="aim-ui-button cancel">
                            取消</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
