<%@ Page Title="交易历史记录详细" Language="C#" MasterPageFile="~/Masters/Ext/formpage.Master"
    AutoEventWireup="true" CodeBehind="FrmTransactionHisDetail.aspx.cs" Inherits="Aim.Examining.Web.FrmTransactionHisDetail" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/js/proctrack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var mdls, tabPanel, grid, store;

        function onPgLoad() {
            InitEditTable();
        }


        // 处理网格数据
        function ProcGridData() {
            var recs = grid.store.getRange();
            var subdata = [];
            $.each(recs, function() {
                subdata.push(this.data);
            });
            var jsonstr = $.getJsonString(subdata);

            $("#Child").val(jsonstr);
        }

        function InitEditTable() {
            // 表格数据
            myData = {
                records: $.getJsonObj($("#Child").val()) || []
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
			    { name: 'MinSalePrice' },
			    { name: 'Price' },
			    { name: 'ReturnPrice' },
			    { name: 'Count' },
			    { name: 'Remark' }
			    ]
            });

            var cm = new Ext.grid.ColumnModel({
                defaults: {
                    resizable: true
                },
                columns: [
                    { id: 'Id', header: 'Id', dataIndex: 'Id', width: 80, resizable: true, hidden: true },
                    { id: 'Isbn', header: 'Isbn', dataIndex: 'Isbn', width: 80, resizable: true, hidden: true },
                    new Ext.ux.grid.AimRowNumberer(),
                    new Ext.ux.grid.AimCheckboxSelectionModel(),
                    { id: 'Code', header: '规格型号', renderer: ExtGridpperCase, dataIndex: 'Code', width: 120, resizable: true, summaryRenderer: function(v, params, data) { return "汇总:" } },
                    { id: 'Name', header: '商品名称', dataIndex: 'Name', width: 120, resizable: true },
                    { id: 'MinSalePrice', header: '最低售价', dataIndex: 'MinSalePrice', width: 100, resizable: true },

                    { id: 'Price', header: '售价', dataIndex: 'Price', width: 100, resizable: true, summaryType: 'sum' },

                //{ header: '总售价', dataIndex: 's', width: 80, resizable: true, hidden: true, summaryType: 'sum' },
                //{ header: '总退货价', dataIndex: 't', width: 80, resizable: true, hidden: true, summaryType: 'sum' },

                    {id: 'ReturnPrice', header: '退货价', dataIndex: 'ReturnPrice', width: 100, resizable: true, allowBlank: false },

                    { id: 'Count', header: '购买量', dataIndex: 'Count', width: 100, resizable: true, allowBlank: false },
                    { id: 'Remark', header: '备注', dataIndex: 'Remark', width: 100, resizable: true }
                ]
            });

            // 表格面板
            grid = new Ext.ux.grid.AimEditorGridPanel({
                store: store,
                cm: cm,
                renderTo: "StandardSub",
                width: Ext.get("StandardSub").getWidth() + 17,
                height: 280,
                forceLayout: true,
                columnLines: true,
                plugins: new Ext.ux.grid.GridSummary(),
                tbar: new Ext.Toolbar({
                    hidden: pgOperation == 'r',
                    items: ['<label style="color:red;">商品信息：</label>']
                }),
                autoExpandColumn: 'Remark'
            });

            window.onresize = function() {
                grid.setWidth(0);
                grid.setWidth(Ext.get("StandardSub").getWidth() + 17);
            };
        }

        function getPrice() {
            var count = grid.store.getCount();
            var resule = 0.0;
            var temp;
            for (var i = 0; i < count; i++) {
                temp = store.getAt(i);
                resule += temp.get("Price") * temp.get("Count");
            }
            if (resule - $("#PreDeposit").val() > 0) {
                $("#TotalMoney").val(resule - $("#PreDeposit").val());
            }
            else {
                $("#TotalMoney").val(0);
            }
        }
            
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <input type="hidden" id="Child" />
    <div id="StandardSub" name="StandardSub" style="width: 100%;">
    </div>
</asp:Content>
