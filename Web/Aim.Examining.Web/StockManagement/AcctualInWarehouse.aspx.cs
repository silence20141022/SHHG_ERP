using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using Aim.Data;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.Examining.Model;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Data;

namespace Aim.Examining.Web.StockManagement
{
    public partial class AcctualInWarehouse : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        string strjosn = string.Empty;
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            switch (RequestActionString)
            {
                case "IsIsbn":
                    string val = RequestData.Get<string>("Value");
                    IList<Product> proEnts = Product.FindAll("from Product where Isbn='" + val + "'");
                    if (proEnts.Count > 0)
                    {
                        PageState.Add("IsIsbn", true);
                    }
                    else
                    {
                        PageState.Add("IsIsbn", false);
                    }
                    break;
                case "GetPackageInfo":
                    string Isbn = RequestData.Get<string>("Isbn");
                    sql = "select * from SHHG_AimExamine..Products where FirstSkinIsbn='" + Isbn + "' and FirstSkinCapacity is not null";
                    IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                    if (dics.Count > 0)
                    {
                        PageState.Add("ProductIsbn", dics[0].Get<string>("Isbn").ToUpper());
                        PageState.Add("ProductQuan", dics[0].Get<int>("FirstSkinCapacity"));
                    }
                    sql = "select * from SHHG_AimExamine..Products where SecondSkinIsbn='" + Isbn + "' and SecondSkinCapacity is not null";
                    dics = DataHelper.QueryDictList(sql);
                    if (dics.Count > 0)
                    {
                        PageState.Add("ProductIsbn", dics[0].Get<string>("Isbn").ToUpper());
                        PageState.Add("ProductQuan", dics[0].Get<int>("SecondSkinCapacity"));
                    }
                    break;
                case "ScanSkin"://判断包装编号是否存在               
                    string skinNo = RequestData.Get<string>("SkinNo");
                    IList<Skin> sEnts = Skin.FindAll("from Skin where SkinNo='" + skinNo + "'");
                    if (sEnts.Count > 0)//如果该包装的记录不存在
                    {
                        PageState.Add("SkinExist", true);
                    }
                    else
                    {
                        PageState.Add("SkinExist", false);
                    }
                    break;
                case "ScanCompressor":
                    string seriesNo = RequestData.Get<string>("SeriesNo");
                    IList<Compressor> cEnts = Compressor.FindAll("from Compressor where SeriesNo='" + seriesNo + "'");
                    if (cEnts.Count == 0)
                    {
                        PageState.Add("CompressorExist", false);
                    }
                    else
                    {
                        PageState.Add("CompressorExist", true);
                    }
                    break;
                case "InWarehouse":
                    InWarehouse iwEnt = InWarehouse.TryFind(id);
                    IList<string> entStrList = RequestData.GetList<string>("data");
                    if (entStrList != null && entStrList.Count > 0)
                    {
                        for (int j = 0; j < entStrList.Count; j++)
                        {
                            Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                            if (Convert.ToInt32(objL.Value<string>("ActuallyQuantity")) > 0)//只有输入了入库数量才会增加实际入库记录
                            {
                                InWarehouseDetailDetail iwddEnt = new InWarehouseDetailDetail(); //新建一个实际入库详细信息                            
                                if (iwEnt.InWarehouseType == "采购入库")
                                {
                                    iwddEnt.InWarehouseDetailId = objL.Value<string>("Id");
                                    iwddEnt.PurchaseOrderDetailId = objL.Value<string>("PurchaseOrderDetailId");
                                }
                                else//其他入库的情形
                                {
                                    iwddEnt.OtherInWarehouseDetailId = objL.Value<string>("Id");
                                }
                                iwddEnt.Quantity = Convert.ToInt32(objL.Value<string>("ActuallyQuantity"));
                                iwddEnt.ProductId = objL.Value<string>("ProductId");
                                iwddEnt.Remark = objL.Value<string>("Remark");
                                iwddEnt.DoCreate();

                                StockLog slEnt = new StockLog();//创建库存变更日志
                                slEnt.InOrOutDetailId = objL.Value<string>("Id");
                                slEnt.InOrOutBillNo = iwEnt.InWarehouseNo;
                                slEnt.OperateType = "产品入库";
                                slEnt.WarehouseId = iwEnt.WarehouseId;
                                slEnt.WarehouseName = iwEnt.WarehouseName;
                                IList<StockInfo> siEnts = StockInfo.FindAllByProperties(StockInfo.Prop_ProductId, objL.Value<string>("ProductId"), StockInfo.Prop_WarehouseId, iwEnt.WarehouseId);
                                if (siEnts.Count > 0)
                                {
                                    slEnt.StockQuantity = siEnts[0].StockQuantity;
                                }
                                slEnt.Quantity = Convert.ToInt32(objL.Value<string>("ActuallyQuantity"));
                                slEnt.ProductId = objL.Value<string>("ProductId");
                                Product pEnt = Product.Find(objL.Value<string>("ProductId"));
                                slEnt.ProductName = pEnt.Name;
                                slEnt.ProductCode = pEnt.Code;
                                slEnt.ProductIsbn = pEnt.Isbn;
                                slEnt.ProductPcn = pEnt.Pcn;
                                slEnt.DoCreate();

                                ProcessSkin(objL.Value<string>("SkinArray"), objL.Value<string>("ISBN"), iwEnt.Id);
                                ProcessCompressor(objL.Value<string>("SeriesArray"), objL.Value<string>("ISBN"), iwEnt.Id);
                                processremark(objL.Value<string>("Remark"), objL.Value<string>("ISBN"), iwEnt.Id, objL.Value<string>("Code"));

                                //如果实际入库数量和未入库的数量相等 则入库状态为已入库
                                if (objL.Value<string>("ActuallyQuantity") == objL.Value<string>("NoIn"))
                                {
                                    if (iwEnt.InWarehouseType == "采购入库")
                                    {
                                        InWarehouseDetail iwdEnt = InWarehouseDetail.Find(objL.Value<string>("Id"));
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SkinArray")))
                                        {
                                            iwdEnt.SkinArray = objL.Value<string>("SkinArray").ToString();
                                        }
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SeriesArray")))
                                        {
                                            iwdEnt.SeriesArray = objL.Value<string>("SeriesArray").ToString();
                                        }
                                        iwdEnt.InWarehouseState = "已入库";
                                        iwdEnt.DoUpdate();
                                    }
                                    else
                                    {
                                        OtherInWarehouseDetail oiwdEnt = OtherInWarehouseDetail.Find(objL.Value<string>("Id"));
                                        oiwdEnt.InWarehouseState = "已入库";
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SkinArray")))
                                        {
                                            oiwdEnt.SkinArray = objL.Value<string>("SkinArray").ToString();
                                        }
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SeriesArray")))
                                        {
                                            oiwdEnt.SeriesArray = objL.Value<string>("SeriesArray").ToString();
                                        }
                                        oiwdEnt.DoUpdate();
                                    }
                                }
                                else//如果未入库的数量不等于现在输入的数量。只更新包装和压缩机序列号集合
                                {
                                 
                                    if (iwEnt.InWarehouseType == "采购入库")
                                    {
                                        InWarehouseDetail iwdEnt = InWarehouseDetail.Find(objL.Value<string>("Id"));
                                        iwdEnt.Remark = objL.Value<string>("Remark");
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SkinArray")))
                                        {
                                            iwdEnt.SkinArray = objL.Value<string>("SkinArray").ToString();
                                        }
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SeriesArray")))
                                        {
                                            iwdEnt.SeriesArray = objL.Value<string>("SeriesArray").ToString();
                                        }
                                        iwdEnt.DoUpdate();
                                    }
                                    else
                                    {
                                        OtherInWarehouseDetail oiwdEnt = OtherInWarehouseDetail.Find(objL.Value<string>("Id"));
                                        oiwdEnt.Remark = objL.Value<string>("Remark");
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SkinArray")))
                                        {
                                            oiwdEnt.SkinArray = objL.Value<string>("SkinArray").ToString();
                                        }
                                        if (!string.IsNullOrEmpty(objL.Value<string>("SeriesArray")))
                                        {
                                            oiwdEnt.SeriesArray = objL.Value<string>("SeriesArray").ToString();
                                        }
                                        oiwdEnt.DoUpdate();
                                    }
                                }
                                //修改库存 
                                InStorage(iwddEnt.ProductId, iwddEnt.Quantity.Value, iwEnt.WarehouseId);
                            }
                        }
                    }
                    ProcessInWarehouse(iwEnt);
                    if (iwEnt.InWarehouseType == "采购入库")//处理采购单和采购单详细
                    {
                        ProcessPurchaseOrderAndDetail(iwEnt);
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void ProcessSkin(string skinArray, string modelNo, string inWarehouseId)
        {
            if (!string.IsNullOrEmpty(skinArray))
            {
                string[] skins = skinArray.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < skins.Length; i++)
                {
                    string[] skinNo = skins[i].Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                    if (skinNo.Length == 2)
                    {
                        IList<Skin> skinEnts = Skin.FindAll("from Skin where SkinNo='" + skinNo[0] + "'");//如果是调拨或者其他特殊的入库形式 箱号已经存在的  不需要再次创建箱号  
                        if (skinEnts.Count == 0)
                        {
                            Skin sEnt = new Skin();
                            sEnt.SkinNo = skinNo[0];
                            sEnt.Quantity = Convert.ToInt32(skinNo[1]);
                            sEnt.ModelNo = modelNo;
                            sEnt.InWarehouseId = inWarehouseId;
                            sEnt.CreateId = UserInfo.UserID;
                            sEnt.CreateName = UserInfo.Name;
                            sEnt.CreateTime = System.DateTime.Now;
                            sEnt.SkinState = "空箱";
                            sEnt.DoCreate();
                        }
                        else //如果是调拨入库 虽然不需要增加包装箱记录 但是需要将包装里面的压缩的状态 变为  未出库
                        {
                            string sql = @"update SHHG_AimExamine..Compressor set State='未出库' where SkinId='" + skinEnts[0].Id + "'";
                            DataHelper.ExecSql(sql);
                        }
                    }
                }
            }
        }
        private void ProcessCompressor(string seriesArray, string modelNo, string inWarehouseId)
        {
            if (!string.IsNullOrEmpty(seriesArray))
            {
                string[] compressors = seriesArray.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < compressors.Length; i++)
                {
                    IList<Compressor> compEnts = Compressor.FindAll("from Compressor where SeriesNo='" + compressors[i] + "'");
                    if (compEnts.Count == 0)
                    {
                        Compressor cEnt = new Compressor();
                        cEnt.SeriesNo = compressors[i];
                        cEnt.ModelNo = modelNo;
                        cEnt.InWarehouseId = inWarehouseId;
                        cEnt.State = "未出库";
                        cEnt.CreateId = UserInfo.UserID;
                        cEnt.CreateName = UserInfo.Name;
                        cEnt.CreateTime = System.DateTime.Now;
                        cEnt.DoCreate();
                    }
                    else// 否则压缩机序列号存在的情形。会是在退换货入库或者调拨入库的情形下
                    {
                        compEnts[0].State = "未出库";
                        compEnts[0].InWarehouseId = inWarehouseId;
                        compEnts[0].DoUpdate();
                    }
                }

            }
        }
        public void processremark(string remark, string modelno, string inWarehouseId,string productcode)
        {
            //格式  箱号@序列号#箱号@序列号
            string sql = string.Empty;
            DataTable dt_c = null;
            DataTable dt_s = null;
            int quan_c = 0;
            if (!string.IsNullOrEmpty(remark))
            {
                string[] skinarr = remark.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string skin in skinarr)
                {
                    //  [)>06PVR54KSTFP54ES17FM3608ZS17FM3609ZS17FM3610ZS17FM3611ZS17FM3612ZS17FM3613ZS17FM3614ZS17FM3615ZS17FM3616ZS17FM3621ZS17FM3624ZS17FM3630ZS17FM3631ZS17FM3646ZS17FM3647ZS17FM3655Z
                    string[] tmparr = skin.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                    sql = "select * from SHHG_AimExamine..skin where skinno='" + tmparr[0] + "'";
                    dt_s = DataHelper.QueryDataTable(sql);
                    if (dt_s.Rows.Count > 0)
                    {
                        string[] seriesnoarr = tmparr[1].Replace(productcode,"").Replace(modelno,"").Split(new string[] { "S" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 1; i < seriesnoarr.Length; i++)//不从第一个循环，因为第一个不是箱号
                        {
                            sql = "select * from SHHG_AimExamine..compressor where SeriesNo='" + seriesnoarr[i] + "'";
                            dt_c = DataHelper.QueryDataTable(sql);
                            if (dt_c.Rows.Count == 0)
                            {
                                sql = @"insert into SHHG_AimExamine..Compressor (Id,SeriesNo,SkinId,modelno,CreateId,CreateName,CreateTime,State,InWarehouseId) values 
                                      (NEWID(),'" + seriesnoarr[i] + "','" + dt_s.Rows[0]["Id"] + "','" + modelno + "','" + UserInfo.UserID + "','" + UserInfo.Name + "',GETDATE(),'未出库','" + inWarehouseId + "')";
                                DataHelper.ExecSql(sql);
                            }
                        }
                        sql = "select count(1) from SHHG_AimExamine..Compressor where skinid='" + dt_s.Rows[0]["Id"] + "'";
                        quan_c = DataHelper.QueryValue<int>(sql);
                        if (quan_c + "" == dt_s.Rows[0]["Quantity"] + "")
                        {
                            sql = "update SHHG_AimExamine..skin set SkinState='满箱' where Id='" + dt_s.Rows[0]["Id"] + "'";
                            DataHelper.ExecSql(sql);
                        }
                    }
                }
            }
        }
        private void InStorage(string proId, int iQuan, string warehouseId)
        {
            Product proEnt = Product.Find(proId);
            Warehouse whEnt = Warehouse.Find(warehouseId);
            IList<StockInfo> siEnts = StockInfo.FindAll("from StockInfo where ProductId='" + proId + "' and WarehouseId='" + warehouseId + "'");
            if (siEnts.Count > 0)
            {
                siEnts[0].StockQuantity = siEnts[0].StockQuantity + iQuan;
                siEnts[0].DoUpdate();
            }
            else
            {
                StockInfo siEnt = new StockInfo();
                siEnt.ProductId = proId;
                siEnt.WarehouseId = warehouseId;
                siEnt.WarehouseName = whEnt.Name;
                siEnt.ProductCode = proEnt.Code;
                siEnt.ProductName = proEnt.Name;
                siEnt.StockQuantity = iQuan;
                siEnt.DoCreate();
            }
        }
        private void ProcessInWarehouse(InWarehouse tempEnt)
        {
            if (tempEnt.InWarehouseType == "采购入库")
            {
                //判断其下所有的入库详细状态都未已入库 则自己的状态改为已入库
                IList<InWarehouseDetail> iwdEnts = InWarehouseDetail.FindAll("from InWarehouseDetail where InWarehouseState='未入库' and InWarehouseId='" + tempEnt.Id + "'");
                if (iwdEnts.Count == 0)
                {
                    tempEnt.State = "已入库";
                    tempEnt.InWarehouseEndTime = System.DateTime.Now;
                    tempEnt.CheckUserId = UserInfo.UserID;
                    tempEnt.CheckUserName = UserInfo.Name;
                    tempEnt.DoUpdate();
                }
            }
            else
            {
                IList<OtherInWarehouseDetail> oiwdEnts = OtherInWarehouseDetail.FindAll("from OtherInWarehouseDetail where InWarehouseState='未入库' and InWarehouseId='" + tempEnt.Id + "'");
                if (oiwdEnts.Count == 0)//如果明细都入库了
                {
                    //add by cc
                    string db = ConfigurationManager.AppSettings["ExamineDB"];
                    if (tempEnt.InWarehouseType == "退货入库")
                    {
                        ReturnOrder rtnOrder = ReturnOrder.TryFind(tempEnt.PublicInterface);
                        if (rtnOrder != null)
                        {
                            rtnOrder.State = "已完成";
                            rtnOrder.DoSave();
                            //判断是否已经全部退货
                            //string DeliveryState = "";
                            //string tempId = DataHelper.QueryValue<string>("select top 1 Id from " + db + "..Saleorders where number='" + rtnOrder.OrderNumber + "'");
                            //if (!string.IsNullOrEmpty(tempId))
                            //{
                            //    DeliveryState = DataHelper.QueryValue<string>("select " + db + ".dbo.fun_getDeliveryState('" + tempId + "')");
                            //}

                            ////更新SaleOrder表出库状态
                            //if (DeliveryState == "已全部退货")
                            //{
                            //    DataHelper.ExecSql("update " + db + "..SaleOrders set DeState='已全部退货',DeliveryState='已全部退货' where Number='" + rtnOrder.OrderNumber + "'");
                            //}
                            //else
                            //{
                            //    DataHelper.ExecSql("update " + db + "..SaleOrders set DeState='" + DeliveryState + "' where Number='" + rtnOrder.OrderNumber + "'");
                            //    //DataHelper.ExecSql("update " + db + "..SaleOrders set DeState=" + db + ".dbo.fun_getDeliveryState(Id) where Number='" + rtnOrder.OrderNumber + "'");
                            //}
                        }
                    }
                    //else if (tempEnt.InWarehouseType == "换货入库")
                    //{
                    //    ChangeProduct dorder = ChangeProduct.TryFind(tempEnt.PublicInterface);
                    //    if (dorder != null)
                    //    {
                    //        if ((dorder.State + "").Contains("已出库"))
                    //        {
                    //            dorder.State = "已完成";
                    //        }
                    //        else
                    //        {
                    //            dorder.State += "，仓库已入库：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    //        }
                    //        dorder.DoSave();

                    //        //更新SaleOrder表出库状态
                    //        DataHelper.ExecSql("update " + db + "..SaleOrders set DeState=" + db + ".dbo.fun_getDeliveryState(Id) where Number='" + dorder.OrderNumber + "'");
                    //    }
                    //}
                    //end add by cc
                    else if (tempEnt.InWarehouseType == "调拨入库")
                    {
                        WarehouseExchange weEnt = WarehouseExchange.TryFind(tempEnt.PublicInterface);
                        weEnt.InWarehouseState = "已入库";
                        weEnt.DoUpdate();
                        if (weEnt.OutWarehouseState == "已出库")
                        {
                            weEnt.ExchangeState = "已结束";
                            weEnt.EndTime = System.DateTime.Now;
                            weEnt.DoUpdate();
                        }
                    }
                    tempEnt.State = "已入库";
                    tempEnt.InWarehouseEndTime = System.DateTime.Now;
                    tempEnt.CheckUserId = UserInfo.UserID;
                    tempEnt.CheckUserName = UserInfo.Name;
                    tempEnt.DoUpdate();
                }
            }
        }
        private void ProcessPurchaseOrderAndDetail(InWarehouse iwEnt)
        {
            string purchaseOrderId = string.Empty;
            IList<InWarehouseDetail> iwdEnts = InWarehouseDetail.FindAll("from InWarehouseDetail where InWarehouseId='" + iwEnt.Id + "'");
            PurchaseOrderDetail podEnt = null;
            ArrayList poIdArray = new ArrayList();
            int actuallyInQuan = 0;
            foreach (InWarehouseDetail iwdEnt in iwdEnts)
            {
                if (iwdEnt.InWarehouseState == "已入库")//只有入库单详细的状态是已入库 才去遍历采购详细
                {
                    podEnt = PurchaseOrderDetail.Find(iwdEnt.PurchaseOrderDetailId);
                    if (podEnt.PurchaseOrderId != purchaseOrderId)
                    {
                        purchaseOrderId = podEnt.PurchaseOrderId;
                        poIdArray.Add(podEnt.PurchaseOrderId);
                    }
                    actuallyInQuan = DataHelper.QueryValue<int>("select  SHHG_AimExamine.dbo.fun_ActuallyHaveInQuantityByPodId('" + podEnt.Id + "')");
                    if (podEnt.Quantity == actuallyInQuan)
                    {
                        podEnt.InWarehouseState = "已入库";
                        podEnt.DoUpdate();
                    }
                }
            }//循环完毕后 再根据记录的采购单ID集合。对采购单遍历
            for (int i = 0; i < poIdArray.Count; i++)
            {
                IList<PurchaseOrderDetail> podEnts = PurchaseOrderDetail.FindAll("from PurchaseOrderDetail where InWarehouseState='未入库' and PurchaseOrderId='" + poIdArray[i].ToString() + "'");
                if (podEnts.Count == 0)
                {
                    PurchaseOrder poEnt = PurchaseOrder.Find(poIdArray[i].ToString());
                    poEnt.InWarehouseState = "已入库";
                    poEnt.DoUpdate();
                    if (poEnt.InvoiceState == "已关联" && poEnt.PayState == "已付款")
                    {
                        poEnt.OrderState = "已结束";
                        poEnt.DoUpdate();
                    }
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                InWarehouse ent = InWarehouse.Find(id);
                string tempsql = @"select A.* ,B.Name from SHHG_AimExamine..InWarehouse as A 
                left join SHHG_AimExamine..Warehouse as B on A.WarehouseId=B.Id where A.Id='" + id + "'";
                IList<EasyDictionary> dicsInWarehouse = DataHelper.QueryDictList(tempsql);
                if (dicsInWarehouse.Count > 0)
                {
                    this.SetFormData(dicsInWarehouse[0]);
                }
                string sql = string.Empty;
                if (ent.InWarehouseType == "采购入库")
                {
                    sql = @"select A.Id,A.ProductId, A.IQuantity as Quantity,A.InWarehouseId,A.InWarehouseState,
                    A.Remark,A.SkinArray,A.SeriesArray,A.PurchaseOrderDetailId,
                    UPPER(rtrim(ltrim(B.Isbn))) as ISBN,B.Name,UPPER(B.Code) as Code,B.ProductType,
                    SHHG_AimExamine.dbo.fun_ActuallyHaveInQuantity(A.Id,'{0}') as HaveIn,                   
                    (A.IQuantity-SHHG_AimExamine.dbo.fun_ActuallyHaveInQuantity(A.Id,'{1}')) as NoIn
                    from SHHG_AimExamine..InWarehouseDetail as A 
                    left join SHHG_AimExamine..Products as B on A.ProductId=B.Id
                    where A.InWarehouseId='{2}'";
                    sql = string.Format(sql, ent.InWarehouseType, ent.InWarehouseType, ent.Id);
                }
                else
                {
                    sql = @"select A.Id,A.ProductId,A.Quantity,A.InWarehouseId,A.InWarehouseState,A.Remark,A.SkinArray,A.SeriesArray,
                    UPPER(B.Code) as Code , B.Name,UPPER(rtrim(ltrim(B.Isbn))) as ISBN,B.ProductType,
                    SHHG_AimExamine.dbo.fun_ActuallyHaveInQuantity(A.Id,'{0}') as HaveIn,
                    (A.Quantity-SHHG_AimExamine.dbo.fun_ActuallyHaveInQuantity(A.Id,'{1}')) as NoIn
                    from SHHG_AimExamine..OtherInWarehouseDetail as A 
                    left join SHHG_AimExamine..Products as B on A.ProductId=B.Id  where A.InWarehouseId='{2}'";
                    sql = string.Format(sql, ent.InWarehouseType, ent.InWarehouseType, ent.Id);
                }
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", dics);

            }
        }
    }
}

