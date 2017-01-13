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
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data;

namespace Aim.Examining.Web
{
    public partial class FrmDeliveryOrderEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型 
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            DeliveryOrder ent = null;
            IList<string> strList = RequestData.GetList<string>("data");
            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<DeliveryOrder>();
                    if (type == "xg")
                    {
                        ent.DoUpdate();
                    }
                    else if (type == "ck")
                    {
                        UpdateStockInfo(strList, ent);  //更新库存及唯一编号
                        if (ent.DeliveryType == "换货出库")
                        {
                            ChangeProduct dorder = ChangeProduct.TryFind(ent.PId);
                            if (dorder != null)
                            {
                                if ((dorder.State + "").Contains("已入库"))
                                {
                                    dorder.State = "已完成";
                                }
                                else
                                {
                                    dorder.State += "，仓库已出库：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                }
                                dorder.DoSave();
                            }
                        }
                        //更新子商品信息(先删除，再添加)
                        DelieryOrderPart.DeleteAll("DId='" + ent.Id + "'"); //删除DeliveryOrderPart
                        //添加出库商品信息并更新出库单状态
                        ent.State = InsertPart(strList, ent.Id);
                        ent.DoUpdate();
                        if (ent.DeliveryType == "调拨出库")//add by phg 20120519 跟出库单的状态保持一致 
                        {
                            WarehouseExchange weEnt = WarehouseExchange.Find(ent.PId);
                            weEnt.OutWarehouseState = ent.State;
                            weEnt.DoUpdate();
                        }
                        //更新SaleOrder表出库状态
                        DataHelper.ExecSql("update " + db + "..SaleOrders set DeState=" + db + ".dbo.fun_getDeliveryState(Id) where id='" + ent.PId + "'");
                    }
                    break;
                default:
                    break;
            }
            if (RequestActionString == "getSalesman")
            {
                string cid = RequestData.Get<string>("CId");
                Customer customer = Customer.Find(cid);
                if (customer != null)
                {
                    PageState.Add("MagUser", customer.MagUser);
                    PageState.Add("MagId", customer.MagId);
                    PageState.Add("Address", customer.Address);
                    PageState.Add("Tel", customer.Tel);
                    PageState.Add("Code", customer.Code);
                }
            }
            //根据压缩机唯一Id获取压缩机条码
            else if (RequestActionString == "getprobyguid")
            {
                //string isbn = DataHelper.QueryValue("select ModelNo from " + db + "..Compressor where SeriesNo='" + RequestData.Get<string>("isbn") + "'") + "";

                string isbn = DataHelper.QueryValue("select ModelNo from " + db + "..v_Compressor where SeriesNo='" + RequestData.Get<string>("Guid") + "' and ([State] is null or [State]<>'已出库')") + "";
                PageState.Add("isbn", isbn);
            }
            //获取压缩机整版的数量及GUIDS
            else if (RequestActionString == "getprobyboxnum")
            {
                string boxnum = RequestData.Get<string>("boxnum");
                string isbn = DataHelper.QueryValue("select ModelNo from " + db + "..v_Compressor where SkinNo='" + boxnum + "' and ([State] is null or [State]<>'已出库')") + "";
                string count = DataHelper.QueryValue("select count(1) from " + db + "..v_Compressor where SkinNo='" + boxnum + "' and ([State] is null or [State]<>'已出库')") + "";
                string guids = DataHelper.QueryValue("select " + db + ".dbo.fun_getGuids('" + boxnum + "')") + "";

                PageState.Add("isbn", isbn);
                PageState.Add("count", count);
                PageState.Add("guids", guids);
            }
            //获取配件箱子所包含的配件数量 
            else if (RequestActionString == "getPJbyboxnum")
            {
                string boxnum = RequestData.Get<string>("boxnum");
                sql = "select * from SHHG_AimExamine..Products where FirstSkinIsbn='" + boxnum + "' and FirstSkinCapacity is not null";
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                if (dics.Count > 0)
                {
                    PageState.Add("isbn", dics[0].Get<string>("Isbn"));
                    PageState.Add("count", dics[0].Get<int>("FirstSkinCapacity"));
                }
                sql = "select * from SHHG_AimExamine..Products where SecondSkinIsbn='" + boxnum + "' and SecondSkinCapacity is not null";
                dics = DataHelper.QueryDictList(sql);
                if (dics.Count > 0)
                {
                    PageState.Add("isbn", dics[0].Get<string>("Isbn"));
                    PageState.Add("count", dics[0].Get<int>("SecondSkinCapacity"));
                }
            }
            else if (RequestActionString == "getguids")
            {
                string guids = DataHelper.QueryValue("select " + db + ".dbo.fun_getGuids('" + RequestData.Get<string>("SkinNo") + "'," + RequestData.Get<string>("Count") + ")") + "";
                PageState.Add("guids", guids);
            }
            else if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = DeliveryOrder.Find(id);
                }
                SetFormData(ent);
                PageState.Add("State", ent.State);
                //查询子商品
                sql = "select p.Id, PId, PCode as Code, ProductId, PName as Name, c.ProductType, p.Isbn, Guids, Count,Count-(isnull(OutCount,0)) as dck, isnull(OutCount,0) as OutCount, p.Unit, p.Remark, FirstSkinIsbn," + db + ".dbo.fun_getProQuantity(ProductId) as StockQuan," + db + ".dbo.fun_getProQuantityByWarehouseId(ProductId,'" + ent.WarehouseId + "') as BenStockQuan from " + db + "..DelieryOrderPart p "
                  + " left join " + db + "..Products c on c.Id=p.ProductId  where DId='" + id + "'";
                PageState.Add("DetailList", DataHelper.QueryDictList(sql));
            }
            PageState.Add("DeliveryMode", SysEnumeration.GetEnumDict("DeliveryMode"));
        }
        private string InsertPart(IList<string> strList, string Did)
        {
            string deliveryState = "已出库";
            List<string> listState = new List<string>();
            Dictionary<string, object> dic = null;
            DelieryOrderPart entPart = null;
            for (int i = 0; i < strList.Count; i++)
            {

                dic = FromJson(strList[i]) as Dictionary<string, object>;

                if (dic != null)
                {
                    //一个一个的添加
                    entPart = new DelieryOrderPart
                    {
                        DId = Did,
                        PId = dic.ContainsKey("PId") ? dic["PId"] + "" : "",
                        ProductId = dic["ProductId"] + "",
                        PName = dic["Name"] + "",
                        PCode = dic["Code"] + "",
                        Isbn = dic.ContainsKey("Isbn") ? dic["Isbn"] + "" : "",
                        Unit = dic.ContainsKey("Unit") ? dic["Unit"] + "" : "",
                        Count = dic.ContainsKey("Count") ? Convert.ToInt32(dic["Count"]) : 0,
                        OutCount = Convert.ToInt32(dic["OutCount"]) + (dic.ContainsKey("SmCount") && dic["SmCount"] + "" != "" ? Convert.ToInt32(dic["SmCount"]) : 0),
                        Remark = dic.ContainsKey("Remark") ? dic["Remark"] + "" : "",
                        State = "部分出库",
                        Guids = dic.ContainsKey("Guids") ? dic["Guids"] + "" : "",
                        CreateId = UserInfo.UserID,
                        CreateName = UserInfo.Name,
                        CreateTime = DateTime.Now
                    };
                    if (entPart.OutCount == entPart.Count)
                    {
                        entPart.State = "已出库";
                    }
                    else if (entPart.OutCount == null || entPart.OutCount == 0)
                    {
                        entPart.State = "未出库";
                    }
                    listState.Add(entPart.State);
                    entPart.DoCreate();
                }
            }
            if (listState.IndexOf("未出库") > -1 || listState.IndexOf("部分出库") > -1)
            {
                deliveryState = "部分出库";
            }
            return deliveryState;
        }

        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="childjson">出库单出库商品json</param>
        private void UpdateStockInfo(IList<string> strList, DeliveryOrder deorder)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string strguids = "";

            Dictionary<string, object> dic = null;

            StockInfo stoinfo = null;

            for (int i = 0; i < strList.Count; i++)
            {
                dic = FromJson(strList[i]) as Dictionary<string, object>;

                if (dic != null)
                {
                    //记录日志begin
                    if (dic.ContainsKey("SmCount") && dic["SmCount"] + "" != "")
                    {
                        StockLog slEnt = new StockLog();//创建库存变更日志
                        slEnt.InOrOutDetailId = dic["Id"] + "";
                        slEnt.InOrOutBillNo = deorder.Number;
                        slEnt.OperateType = "产品出库";
                        slEnt.WarehouseId = deorder.WarehouseId;
                        slEnt.WarehouseName = deorder.WarehouseName;
                        IList<StockInfo> siEnts = StockInfo.FindAllByProperties(StockInfo.Prop_ProductId, dic["ProductId"], StockInfo.Prop_WarehouseId, deorder.WarehouseId);
                        if (siEnts.Count > 0)
                        {
                            slEnt.StockQuantity = siEnts[0].StockQuantity;
                        }
                        slEnt.Quantity = Convert.ToInt32(dic["SmCount"]);
                        slEnt.ProductId = dic["ProductId"] + "";
                        Product pEnt = Product.Find(dic["ProductId"]);
                        slEnt.ProductCode = pEnt.Code;
                        slEnt.ProductName = pEnt.Name;
                        slEnt.ProductIsbn = pEnt.Isbn;
                        slEnt.ProductPcn = pEnt.Pcn;
                        slEnt.DoCreate();
                    }
                    //记录日志end

                    //更新唯一编号的状态
                    if (dic.ContainsKey("Guids") && dic["Guids"] + "" != "")
                    {
                        string guids = dic["Guids"] + "";
                        guids = guids.Substring(0, guids.Length - 1);
                        strguids = "'" + guids.Replace(",", "','") + "'";

                        DataHelper.ExecSql("update " + db + "..Compressor set [state]='已出库',CustomerId='" + deorder.CId + "',DeliveryOrderId='" + deorder.Id + "' where SeriesNo in (" + strguids + ")");
                    }
                    //更新库存 OutCount 
                    stoinfo = StockInfo.FindAllByProperties("ProductId", dic["ProductId"], "WarehouseId", deorder.WarehouseId).FirstOrDefault<StockInfo>();
                    if (stoinfo != null)
                    {
                        //SmCount 本次出库数量
                        stoinfo.StockQuantity -= (dic.ContainsKey("SmCount") && dic["SmCount"] + "" != "" ? Convert.ToInt32(dic["SmCount"]) : 0);
                        stoinfo.DoUpdate();
                    }
                    else
                    {
                        stoinfo = new StockInfo
                        {
                            ProductCode = dic["Code"] + "",
                            ProductId = dic["ProductId"] + "",
                            ProductName = dic["Name"] + "",
                            StockQuantity = -(dic.ContainsKey("SmCount") && dic["SmCount"] + "" != "" ? Convert.ToInt32(dic["SmCount"]) : 0),
                            WarehouseId = deorder.WarehouseId,
                            WarehouseName = deorder.WarehouseName
                        };
                        stoinfo.DoCreate();
                    }
                }
            }
        }
        //把json转换为可识别的
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
        }
    }
}

