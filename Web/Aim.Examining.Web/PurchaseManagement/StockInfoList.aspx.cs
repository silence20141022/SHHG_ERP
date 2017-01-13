using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;
using Aim.Portal.Model;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class StockInfoList : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchCriterion.DefaultPageSize = 25;
            string id = RequestData.Get<string>("id");
            switch (RequestActionString)
            {
                case "CreateLog":
                    CreateLog();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = "";
            string productType = RequestData.Get<string>("ProductType");
            if (!string.IsNullOrEmpty(productType))
            {
                SearchCriterion.AddSearch("ProductType", productType);
            }
            string sql = string.Empty;
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.Value.ToString() != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
            //  ((select isnull(sum(isnull(Count,0)-isnull(OutCount,0)),0) from SHHG_AimExamine..DelieryOrderPart t where t.ProductId=A.ProductId and isnull(State,'')<>'已出库')+
            //(select isnull(sum(isnull(Count,0)-isnull(OutCount,0)),0) from SHHG_AimExamine..OrdersPart n where n.PId=A.ProductId and n.IsValid=1 and isnull(OutCount,0)<>isnull(Count,0))) as NoOutQuan,
            //SHHG_AimExamine.dbo.fun_CaculateBuyPrice(A.ProductId)*(A.StockQuantity)*(select top(1) rate from SHHG_AimExamine..ExchangeRate where Symbo=(select top(1) Symbo from SHHG_AimExamine..Supplier where Id=B.SupplierId)) as StockAmount,
            sql = @"select A.*,
            (B.BuyPrice)*(A.StockQuantity) as StockAmount,
            SHHG_AimExamine.dbo.fun_getProQuantity(A.ProductId) as StockQuan,    
            ((select isnull(sum(t.Quantity),0) from  SHHG_AimExamine..PurchaseOrderDetail t where t.ProductId=A.ProductId and t.InWarehouseState='未入库')-  
            (select isnull(sum(m.Quantity),0) from  SHHG_AimExamine..InWareHouseDetailDetail m where m.PurchaseOrderDetailId in 
            (select Id from SHHG_AimExamine..PurchaseOrderDetail t where t.ProductId=A.ProductId and t.InWarehouseState='未入库'))) as NoInQuan,
           ((select isnull(sum(isnull(Count,0)-isnull(OutCount,0)),0) from SHHG_AimExamine..DelieryOrderPart t where t.ProductId=A.ProductId and isnull(State,'')<>'已出库')+
           (select isnull(sum(isnull(Count,0)-isnull(OutCount,0)),0) from SHHG_AimExamine..OrdersPart n where n.PId=A.ProductId and n.IsValid=1 and isnull(OutCount,0)<>isnull(Count,0))) as NoOutQuan,
            SHHG_AimExamine.dbo.fun_CaculateBuyPrice(A.ProductId) as BuyPrice,          
            B.ProductType,B.MinCount,B.Pcn,B.Isbn,B.Name
            from SHHG_AimExamine..StockInfo as A 
            left join (select Id,Isbn,Pcn,MinCount,ProductType,Name,SupplierId,BuyPrice from SHHG_AimExamine..Products) as B on B.Id=A.ProductId
            where 1=1 " + where;
            PageState.Add("StockList", GetPageData(sql, SearchCriterion));
            PageState.Add("ProductTypeEnum", SysEnumeration.GetEnumDict("ProductType"));
        }
        private void CreateLog()
        {
            IList<InWarehouseDetailDetail> iwddEnts = InWarehouseDetailDetail.FindAll();//入库
            foreach (InWarehouseDetailDetail iwddEnt in iwddEnts)
            {
                StockLog slEnt = new StockLog();
                InWarehouse iwEnt = null;
                if (!string.IsNullOrEmpty(iwddEnt.InWarehouseDetailId))
                {
                    slEnt.InOrOutDetailId = iwddEnt.InWarehouseDetailId;
                    InWarehouseDetail iwdEnt = InWarehouseDetail.Find(iwddEnt.InWarehouseDetailId);
                    iwEnt = InWarehouse.Find(iwdEnt.InWarehouseId);
                }
                else
                {
                    slEnt.InOrOutDetailId = iwddEnt.OtherInWarehouseDetailId;
                    OtherInWarehouseDetail oiwdEnt = OtherInWarehouseDetail.Find(iwddEnt.OtherInWarehouseDetailId);
                    iwEnt = InWarehouse.Find(oiwdEnt.InWarehouseId);
                }
                slEnt.InOrOutBillNo = iwEnt.InWarehouseNo;
                slEnt.OperateType = "产品入库";
                slEnt.WarehouseId = iwEnt.WarehouseId;
                slEnt.WarehouseName = iwEnt.WarehouseName;
                //IList<StockInfo> siEnts = StockInfo.FindAllByProperties(StockInfo.Prop_ProductId, iwddEnt.ProductId, StockInfo.Prop_WarehouseId, iwEnt.WarehouseId);
                //if (siEnts.Count > 0)
                //{
                //    slEnt.StockQuantity = siEnts[0].StockQuantity;
                //}
                slEnt.Quantity = iwddEnt.Quantity;
                slEnt.ProductId = iwddEnt.ProductId;
                Product pEnt = Product.TryFind(iwddEnt.ProductId);
                if (pEnt != null)
                {
                    slEnt.ProductName = pEnt.Name;
                    slEnt.ProductCode = pEnt.Code;
                    slEnt.ProductIsbn = pEnt.Isbn;
                    slEnt.ProductPcn = pEnt.Pcn;
                    slEnt.CreateId = iwddEnt.CreateId;
                    slEnt.CreateName = iwddEnt.CreateName;
                    slEnt.CreateTime = iwddEnt.CreateTime;
                    slEnt.DoCreate();
                }
            }
            //生成出库日志
            IList<DelieryOrderPart> dopEnts = DelieryOrderPart.FindAllByProperty(DelieryOrderPart.Prop_State, "已出库");
            foreach (DelieryOrderPart dopEnt in dopEnts)
            {
                StockLog slEnt = new StockLog();
                slEnt.InOrOutDetailId = dopEnt.Id;
                DeliveryOrder doEnt = DeliveryOrder.Find(dopEnt.DId);
                slEnt.InOrOutBillNo = doEnt.Number;
                slEnt.OperateType = "产品出库";
                slEnt.WarehouseId = doEnt.WarehouseId;
                slEnt.WarehouseName = doEnt.WarehouseName;
                //IList<StockInfo> siEnts = StockInfo.FindAllByProperties(StockInfo.Prop_ProductId, iwddEnt.ProductId, StockInfo.Prop_WarehouseId, iwEnt.WarehouseId);
                //if (siEnts.Count > 0)
                //{
                //    slEnt.StockQuantity = siEnts[0].StockQuantity;
                //}
                slEnt.Quantity = dopEnt.OutCount;
                if (!string.IsNullOrEmpty(dopEnt.ProductId))
                {
                    slEnt.ProductId = dopEnt.ProductId;
                    Product pEnt = Product.TryFind(dopEnt.ProductId);
                    if (pEnt != null)
                    {
                        slEnt.ProductName = pEnt.Name;
                        slEnt.ProductCode = pEnt.Code;
                        slEnt.ProductIsbn = pEnt.Isbn;
                        slEnt.ProductPcn = pEnt.Pcn;
                        slEnt.CreateId = dopEnt.CreateId;
                        slEnt.CreateName = dopEnt.CreateName;
                        slEnt.CreateTime = dopEnt.CreateTime;
                        slEnt.DoCreate();
                    }
                }
            }
            //盘点操作库存日志
            string sql = "select * from SHHG_AimExamine..StockCheckDetail where StockCheckResult!='正常' and StockCheckState='盘点结束'";
            IList<EasyDictionary> scdDics = DataHelper.QueryDictList(sql);
            foreach (EasyDictionary scdDic in scdDics)
            {
                StockLog slEnt = new StockLog();
                slEnt.InOrOutDetailId = scdDic.Get<string>("Id");
                StockCheck scEnt = StockCheck.Find(scdDic.Get<string>("StockCheckId"));
                slEnt.InOrOutBillNo = scEnt.StockCheckNo;
                slEnt.OperateType = "库存盘点";
                slEnt.WarehouseId = scEnt.WarehouseId;
                slEnt.WarehouseName = scEnt.WarehouseName;
                slEnt.StockQuantity = scdDic.Get<Int32>("StockQuantity");
                slEnt.Quantity = scdDic.Get<Int32>("StockCheckQuantity") - scdDic.Get<Int32>("StockQuantity");
                slEnt.ProductId = scdDic.Get<string>("ProductId");
                Product pEnt = Product.TryFind(slEnt.ProductId);
                if (pEnt != null)
                {
                    slEnt.ProductName = pEnt.Name;
                    slEnt.ProductCode = pEnt.Code;
                    slEnt.ProductIsbn = pEnt.Isbn;
                    slEnt.ProductPcn = pEnt.Pcn;
                    slEnt.CreateId = scEnt.CreateId;
                    slEnt.CreateName = scEnt.CreateName;
                    slEnt.CreateTime = scEnt.CreateTime;
                    slEnt.DoCreate();
                }
            }
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "ProductType";
            string asc = search.Orders.Count <= 0 || search.Orders[0].Ascending ? " desc ,ProductCode" : " asc ,ProductCode";
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * search.PageSize + 1, search.CurrentPageIndex * search.PageSize);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }
    }
}
