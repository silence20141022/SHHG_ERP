using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Aim.Portal;
using Aim.Portal.Model;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using System.Data.OleDb;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmOrdersEdit : ExamBasePage
    {


        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string paid = String.Empty;   // 报价单Id
        string PAState = String.Empty;   // 是否需要价格审核
        string type = String.Empty; // 对象类型
        Dictionary<string, object> dic = null;
        OrdersPart entPart = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            paid = RequestData.Get<string>("paid");
            PAState = RequestData.Get<string>("PAState");
            type = RequestData.Get<string>("type");

            SaleOrder ent = null;
            IList<string> strList = RequestData.GetList<string>("data");

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<SaleOrder>();
                    if (PAState == "Yes")
                    {
                        ent.PAState = "待审核";
                        ent.PANumber = DataHelper.QueryValue("select " + db + ".dbo.fun_getPriceAppNumber()") + "";
                    }
                    else
                    {
                        ent.PAState = "";
                        ent.PANumber = "";
                    }

                    ent.DoUpdate();

                    //处理OrdersPart表
                    //删除
                    OrdersPart.DeleteAll("OId='" + ent.Id + "'");
                    //添加
                    InsertProPart(ent, strList);

                    //创建价格申请单
                    if (PAState == "Yes")
                    {
                        CreatePriceApply(ent);
                    }

                    #region 删除出库单表并更新销售单状态

                    if (RequestData.Get<string>("del") == "yes")
                    {
                        DeliveryOrder[] dorders = DeliveryOrder.FindAllByProperty("PId", ent.Id);
                        foreach (DeliveryOrder dorder in dorders)
                        {
                            DelieryOrderPart.DeleteAll("DId='" + dorder.Id + "'");
                            DeliveryOrder.DeleteAll("Id='" + dorder.Id + "'");
                        }

                        //更新出库单状态
                        OrdersPart[] ops = OrdersPart.FindAllByProperty("OId", ent.Id);
                        foreach (OrdersPart opt in ops)
                        {
                            if (opt != null)
                            {
                                opt.OutCount = 0;
                                opt.DoSave();
                            }
                        }

                        //更新order的json
                        string jsons = "";
                        ops = OrdersPart.FindAllByProperty("OId", ent.Id);
                        foreach (OrdersPart opt in ops)
                        {
                            jsons += "{";
                            jsons += "Id:'" + opt.PId + "',";
                            jsons += "Isbn:'" + opt.Isbn + "',";
                            jsons += "Code:'" + opt.PCode + "',";
                            jsons += "Name:'" + opt.PName + "',";
                            jsons += "Unit:'" + opt.Unit + "',";
                            jsons += "MinSalePrice:'" + opt.MinSalePrice + "',";
                            jsons += "Price:'" + opt.SalePrice + "',";
                            jsons += "Amount:'" + opt.Amount + "',";
                            jsons += "Count:'" + opt.Count + "',";
                            jsons += "OutCount:'" + opt.OutCount + "',";
                            jsons += "Remark:'" + opt.Remark + "'";
                            jsons += "},";
                        }

                        string count = DataHelper.QueryValue("select count(1) from " + db + "..OrdersPart where OId='" + ent.Id + "' and OutCount=[Count]") + "";
                        if (count == "0")
                        {
                            ent.DeliveryState = "";
                        }
                        else
                        {
                            ent.DeliveryState = "部分生成出库单";
                        }

                        ent.Child = "[" + jsons.Substring(0, jsons.Length - 1) + "]";
                        ent.DoUpdate();
                    }
                    #endregion

                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<SaleOrder>();
                    if (PAState == "Yes")
                    {
                        ent.PAState = "待审核";
                        ent.PANumber = DataHelper.QueryValue("select " + db + ".dbo.fun_getPriceAppNumber()") + "";
                    }
                    ent.PId = paid;

                    //自动生成流水号
                    ent.Number = DataHelper.QueryValue("select " + db + ".dbo.fun_getOrderNumber()") + "";

                    ent.DoCreate();

                    //处理OrdersPart表
                    //添加

                    InsertProPart(ent, strList);

                    //创建价格申请单
                    if (PAState == "Yes")
                    {
                        CreatePriceApply(ent);
                    }

                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<SaleOrder>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
                default:
                    if (RequestActionString == "submitfinish")
                    {
                        SaleOrder pc = SaleOrder.Find(this.RequestData.Get<string>("id"));
                        pc.State = "End";
                        pc.DeliveryState = "已生成出库单";
                        pc.ApprovalState = this.RequestData.Get<string>("ApprovalState");
                        pc.Save();

                        //自动生成发货单
                        if (pc.ApprovalState == "同意")
                        {
                            DeliveryOrder dor = new DeliveryOrder
                            {
                                Child = pc.Child,
                                CId = pc.Id,
                                WarehouseId = pc.WarehouseId,
                                WarehouseName = pc.WarehouseName,
                                CName = pc.CName,
                                CreateId = UserInfo.UserID,
                                CreateName = UserInfo.Name,
                                CreateTime = DateTime.Now,
                                ExpectedTime = pc.ExpectedTime,
                                Number = DateTime.Now.ToString("yyyyMMddHHmmss")
                            };
                            dor.DoCreate();
                        }
                    }
                    else if (RequestActionString == "inputexcel")
                    {
                        string path = @"D:\RW\Files\AppFiles\Portal\Default\" + RequestData.Get<string>("path");
                        string strcn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';";//连接字符串

                        #region 遍历整个excel
                        //DataSet ds = GetDataFromExcel(path);

                        ////遍历DataSet取数据

                        ////清除无效行
                        //ds.Tables[0].Rows.RemoveAt(0);
                        //ds.Tables[0].Rows.RemoveAt(0);
                        //ds.Tables[0].Rows.RemoveAt(0);
                        //PageState.Add("error", "Tables：" + ds.Tables.Count + " Table[0]：" + ds.Tables[0].Select("F4 <> ''").Length + "数量" + ds.Tables[0].Rows[0][3]);

                        //return;
                        #endregion

                        #region old
                        try
                        {
                            string sql = "select * from [Sheet1$]";
                            OleDbDataAdapter oldr = new OleDbDataAdapter(sql, strcn);//读取数据，并填充到DATASET里
                            DataTable dt = new DataTable();
                            oldr.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                string strjson = RequestData.Get<string>("json").Replace("[]", "");
                                if (strjson.Length > 0)
                                {
                                    strjson = strjson.Substring(1, strjson.Length - 2) + ",";
                                }
                                DataRow row = null;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    row = dt.Rows[i];

                                    strjson += "{";
                                    strjson += "Id:'" + row["Id"] + "',";
                                    strjson += "Isbn:'" + row["Isbn"] + "',";
                                    strjson += "Code:'" + row["Code"] + "',";
                                    strjson += "Name:'" + row["Name"] + "',";
                                    strjson += "Unit:'" + row["Unit"] + "',";
                                    strjson += "MinSalePrice:'" + row["MinSalePrice"] + "',";
                                    strjson += "Price:'" + row["MinSalePrice"] + "',";
                                    strjson += "Amount:'" + row["Amount"] + "',";
                                    strjson += "Count:'" + row["Count"] + "',";
                                    strjson += "Remark:'" + row["Remark"] + "'";
                                    strjson += "},";
                                }
                                if (strjson != "")
                                {
                                    strjson = "[" + strjson.Substring(0, strjson.Length - 1) + "]";
                                    PageState.Add("result", strjson);
                                }
                            }
                            return;
                        }
                        catch (Exception ex)
                        {
                            PageState.Add("error", ex.Message);
                            return;
                        }
                        #endregion
                    }
                    else if (RequestActionString == "updateAmount")
                    {
                        SaleOrder o = SaleOrder.TryFind(id);
                        string TotalMoney = RequestData.Get<string>("TotalMoney");
                        o.DiscountAmount = RequestData.Get<decimal>("DiscountAmount");
                        o.TotalMoney = Convert.ToDecimal(TotalMoney);
                        o.DoUpdate();
                    }
                    break;
            }

            if (RequestActionString == "getSalesman")
            {
                string cid = RequestData.Get<string>("CId");
                Customer customer = Customer.Find(cid);
                if (customer != null)
                {
                    PageState.Add("Code", customer.Code);
                    PageState.Add("SaleUser", customer.MagUser);
                    PageState.Add("SaleUserId", customer.MagId);
                }
            }
            else if (op == "c" && !string.IsNullOrEmpty(paid))
            {
                if (!string.IsNullOrEmpty(paid))
                {
                    PriceApply paent = PriceApply.TryFind(paid);
                    if (paent != null)
                    {
                        paent.Reason = "";
                        paent.ApprovalState = "";
                        paent.ExpectedTime = null;
                        paent.Id = "";
                        paent.Number = "";
                        paent.Remark = "";
                        paent.State = "";
                        this.SetFormData(paent);
                    }
                }
            }
            else if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = SaleOrder.Find(id);
                }

                this.SetFormData(ent);
                this.PageState.Add("State", ent.State);

                //查询子商品
                string sql = "select Id, PId, PCode as Code, PName as Name, Isbn, Count, Unit, MinSalePrice, SalePrice as Price, Amount,CustomerOrderNo, Remark, OutCount,BillingCount from " + db + "..OrdersPart where OId='" + id + "'";
                PageState.Add("DetailList", DataHelper.QueryDictList(sql));
            }

            this.PageState.Add("FlowEnum", SysEnumeration.GetEnumDictList("WorkFlow.Simple"));

            PageState.Add("InvoiceType", SysEnumeration.GetEnumDict("InvoiceType"));
            PageState.Add("DeliveryMode", SysEnumeration.GetEnumDict("DeliveryMode"));
            PageState.Add("PayType", SysEnumeration.GetEnumDict("PayType"));
            PageState.Add("CalculateManner", SysEnumeration.GetEnumDict("CalculateManner"));
        }

        //创建价格申请单
        private void CreatePriceApply(SaleOrder order)
        {
            PriceApply pa = new PriceApply
            {
                CCode = order.CCode,
                Child = order.Child,
                CName = order.CName,
                CId = order.CId,
                CreateId = UserInfo.UserID,
                CreateName = UserInfo.Name,
                CreateTime = DateTime.Now,
                ExpectedTime = order.ExpectedTime,
                Number = order.PANumber,
                OId = order.Id,
                PreDeposit = order.PreDeposit.ToString(),
                Reason = "商品价格低于最低售价"
            };
            pa.DoCreate();
        }


        //添加子商品
        private void InsertProPart(SaleOrder ent, IList<string> strList)
        {
            for (int i = 0; i < strList.Count; i++)
            {
                dic = FromJson(strList[i]) as Dictionary<string, object>;

                if (dic != null)
                {
                    //一个一个的添加
                    entPart = new OrdersPart
                    {
                        OId = ent.Id,
                        PId = dic["PId"] + "",
                        PName = dic["Name"] + "",
                        PCode = dic["Code"] + "",
                        Isbn = dic.ContainsKey("Isbn") ? dic["Isbn"] + "" : "",
                        MinSalePrice = dic.ContainsKey("MinSalePrice") && dic["MinSalePrice"] + "" != "" ? (decimal?)Convert.ToDecimal(dic["MinSalePrice"]) : null,
                        SalePrice = dic.ContainsKey("Price") && dic["Price"] + "" != "" ? (decimal?)Convert.ToDecimal(dic["Price"]) : null,
                        Amount = dic.ContainsKey("Amount") && dic["Amount"] + "" != "NaN" ? (decimal?)Convert.ToDecimal(dic["Amount"]) : null,
                        Unit = dic.ContainsKey("Unit") ? dic["Unit"] + "" : null,
                        Count = dic.ContainsKey("Count") && dic["Count"] + "" != "" ? (int?)Convert.ToInt32(dic["Count"]) : null,
                        OutCount = dic.ContainsKey("OutCount") && dic["OutCount"] + "" != "" ? (int?)Convert.ToInt32(dic["OutCount"]) : null,
                        CustomerOrderNo = dic.ContainsKey("CustomerOrderNo") ? dic["CustomerOrderNo"] + "" : "",
                        Remark = dic.ContainsKey("Remark") ? dic["Remark"] + "" : "",
                        IsValid = true,
                        Guids = dic.ContainsKey("Guids") ? dic["Guids"] + "" : "",
                        CreateId = UserInfo.UserID,
                        CreateName = UserInfo.Name,
                        CreateTime = ent.CreateTime
                    };
                    //entPart.OId = ent.Id;
                    //entPart.PId = dic["PId"] + "";
                    //entPart.MinSalePrice = dic.ContainsKey("MinSalePrice") && dic["MinSalePrice"] + "" != "" ? (decimal?)Convert.ToDecimal(dic["MinSalePrice"]) : null;
                    //entPart.SalePrice = dic.ContainsKey("Price") && dic["Price"] + "" != "" ? (decimal?)Convert.ToDecimal(dic["Price"]) : null;
                    //if (dic.ContainsKey("Amount"))
                    //{
                    //    entPart.Amount = Convert.ToDecimal(dic["Amount"]);
                    //}
                    entPart.DoCreate();
                }
            }
        }

        //把json转换为可识别的
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
            //JsonHelper.GetObject<SaleOrder>("");
        }


        /// <summary>
        /// 获取excel全部数据
        /// </summary>
        /// <param name="Path"></param>
        /// <returns>DataSet</returns>
        private DataSet GetDataFromExcel(string Path)
        {
            //连接串
            string strcn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';";
            OleDbConnection conn = new OleDbConnection(strcn);
            conn.Open();

            //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等  
            DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            conn.Close();

            DataTable dt = null;
            DataSet ds = new DataSet();
            OleDbDataAdapter oldr = null;

            //包含excel中表名的字符串数组
            for (int k = 0; k < dtSheetName.Rows.Count; k++)
            {
                string sql = "select * from [" + dtSheetName.Rows[k]["TABLE_NAME"] + "]";
                oldr = new OleDbDataAdapter(sql, strcn);
                dt = new DataTable();
                oldr.Fill(dt);
                ds.Tables.Add(dt);
            }
            oldr.Dispose();
            return ds;
        }
    }
}

