<%@ Page Title="商品信息" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmProductDetailEdit.aspx.cs" Inherits="Aim.Examining.Web.PurchaseManagement.FrmProductDetailEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            //绑定按钮验证
            FormValidationBind('btnSubmit', SuccessSubmit);
            $("#btnCancel").click(function() {
                window.close();
            });

            InitEditTable();
        }

        function addProByGuid(isbn) {
            if (!isbn) return;
            SuccessSubmit();
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            AimFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            if (!args.data.error) {
                if (!$("#GuId").val()) return;
                var EntRecord = grid.getStore().recordType;
                var p = new EntRecord({ "PCode": $("#PCode").val(), "PName": $("#PName").val(), "GuId": $("#GuId").val(), "Remark": $("#Remark").val() });
                insRowIdx = store.data.length;
                store.insert(insRowIdx, p);

                $("#GuId").val("");
                $("#Remark").val("");
            }
            else {
                AimDlg.show(args.data.error);
            }
        }


        function InitEditTable() {
            // 表格数据
            myData = {
                records: []
            };
            //Id, PId, PISBN, PPcn, PCode, PName, GuId, Remark, CreateId, CreateName, CreateTime
            // 表格数据源
            store = new Ext.ux.data.AimJsonStore({
                dsname: 'SubList',
                data: myData,
                fields: [
			    { name: 'PCode' },
			    { name: 'PName' },
			    { name: 'GuId' },
			    { name: 'Remark' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
            {
                id: 'PCode',
                header: '商品编号',
                dataIndex: 'PCode',
                width: 100,
                resizable: true
            },
            {
                id: 'PName',
                header: '商品名称',
                dataIndex: 'PName',
                width: 100,
                resizable: true
            },
            {
                id: 'GuId',
                header: '唯一编码',
                dataIndex: 'GuId',
                width: 100,
                resizable: true
            },
            {
                id: 'Remark',
                header: '备注',
                dataIndex: 'Remark',
                width: 150,
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
                height: 300,
                forceLayout: true,
                tbar: new Ext.Toolbar({
                    items: ['<label style="color:red;">联系人：</label>']
                }),
                autoExpandColumn: 'PName'
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
            商品信息</h1>
    </div>
    <div id="editDiv" align="center">
        <table class="aim-ui-table-edit">
            <tbody>
                <tr style="display: none">
                    <td colspan="4">
                        <input id="PId" name="PId" />
                        <input id="PPcn" name="PPcn" />
                        <input id="Id" name="Id" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        规格型号
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PCode" name="PCode" disabled="disabled" />
                    </td>
                    <td class="aim-ui-td-caption">
                        商品名称
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PName" name="PName" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        条形码
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="PISBN" name="PISBN" disabled="disabled" />
                    </td>
                    <td class="aim-ui-td-caption">
                        唯一编码
                    </td>
                    <td class="aim-ui-td-data">
                        <input id="GuId" name="GuId" class="validate[required]" onkeydown="if(event.keyCode==13) addProByGuid(this.value);" />
                    </td>
                </tr>
                <tr>
                    <td class="aim-ui-td-caption">
                        备注
                    </td>
                    <td class="aim-ui-td-data" colspan="3">
                        <textarea id="Remark" name="Remark" cols="54" rows="5"></textarea>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="StandardSub" name="StandardSub" align="left" style="width: 100%;">
        </div>
        <table class="aim-ui-table-edit">
            <tbody>
                <tr>
                    <td class="aim-ui-button-panel" colspan="4">
                        <a id="btnSubmit" class="aim-ui-button submit">添加</a> <a id="btnCancel" class="aim-ui-button cancel">
                            关闭</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
