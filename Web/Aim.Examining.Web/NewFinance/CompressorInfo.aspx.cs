using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Aim.Portal.Model;
using Aim.Data;
using System.Data.SqlClient;
using System.Data;
using NHibernate.Criterion;

namespace Aim.Examining.Web
{
    public partial class CompressorInfo : ExamListPage
    {
        private string sql = "";
        Compressor compEnt = null;
        Product pEnt = null;
        DelieryOrderPart dopEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string ids = RequestData.Get<string>("ids");
            switch (RequestActionString)
            {
                case "update1":
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] strarray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string str in strarray)
                        {
                            compEnt = Compressor.Find(str);
                            pEnt = Product.FindAllByProperty(Product.Prop_Isbn, compEnt.ModelNo).First<Product>();
                            //找到对应的出库单明细
                            IList<DelieryOrderPart> dopEnts = DelieryOrderPart.FindAllByProperties(DelieryOrderPart.Prop_DId, compEnt.DeliveryOrderId, DelieryOrderPart.Prop_ProductId, pEnt.Id);
                            if (dopEnts.Count > 0)
                            {
                                dopEnt = dopEnts[0];
                                if (!dopEnt.Guids.ToUpper().Contains(compEnt.SeriesNo.ToUpper()))
                                {
                                    dopEnt.Guids += (string.IsNullOrEmpty(dopEnt.Guids) ? "" : ",") + compEnt.SeriesNo;
                                    dopEnt.DoUpdate();
                                }
                            }
                            else
                            {
                                compEnt.DeliveryOrderId = DateTime.Now.ToString();
                            }
                            compEnt.State = "已出库";
                            compEnt.DoUpdate();
                        }
                        // sql = "update SHHG_AimExamine..Compressor set State='已出库' where patindex('%'+id+'%','" + ids + "')>0";
                    }
                    break;
                case "batchsave":
                    BatchSave();
                    break;
                case "JudgeSeries":
                    string seriesNo = RequestData.Get<string>("SeriesNo");
                    if (!string.IsNullOrEmpty(seriesNo))
                    {
                        string compsql = @"select A.*,UPPER(B.SkinNo) as SkinNo from SHHG_AimExamine..Compressor as A 
                        left join SHHG_AimExamine..Skin as B on A.SkinId=B.Id where A.SeriesNo='" + seriesNo + "'";
                        IList<EasyDictionary> dics = DataHelper.QueryDictList(compsql);
                        if (dics.Count > 0)
                        {
                            PageState.Add("SeriesResult", "输入的压缩机序列号已经存在于【" + dics[0].Get<string>("SkinNo") + "】包装中！");
                        }
                    }
                    break;
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoBatchDelete()
        {
            int index = 0;
            IList<object> idList = RequestData.GetList<object>("IdList");
            if (idList != null && idList.Count > 0)
            {
                foreach (object obj in idList)
                {
                    Compressor compEnt = Compressor.Find(obj.ToString());
                    string sql1 = @"from InWarehouseDetail  WHERE PATINDEX('%{0}%', SeriesArray) >0";
                    sql1 = string.Format(sql1, compEnt.SeriesNo);
                    IList<InWarehouseDetail> iwdEnts = InWarehouseDetail.FindAll(sql1);
                    if (iwdEnts.Count > 0)
                    {
                        index = iwdEnts[0].SeriesArray.IndexOf(compEnt.SeriesNo);
                        iwdEnts[0].SeriesArray = iwdEnts[0].SeriesArray.Remove(index, compEnt.SeriesNo.Length);
                        iwdEnts[0].DoUpdate();
                    }
                    string sql2 = @"from OtherInWarehouseDetail  WHERE PATINDEX('%{0}%', SeriesArray) > 0";
                    sql2 = string.Format(sql2, compEnt.SeriesNo);
                    IList<OtherInWarehouseDetail> oiwdEnts = OtherInWarehouseDetail.FindAll(sql2);
                    if (oiwdEnts.Count > 0)
                    {
                        index = oiwdEnts[0].SeriesArray.IndexOf(compEnt.SeriesNo);
                        oiwdEnts[0].SeriesArray = oiwdEnts[0].SeriesArray.Remove(index, compEnt.SeriesNo.Length);
                        oiwdEnts[0].DoUpdate();
                    }
                }
            }
            Compressor.DoBatchDelete(idList.ToArray());//不能仅仅删除压缩机记录 还得把InWarehouseDetail 或者OtherInWarehouseDetail 记录的压缩机序列号删除
        }
        private void BatchSave()
        {
            IList<string> entStrList = RequestData.GetList<string>("data");
            if (entStrList != null && entStrList.Count > 0)
            {
                Compressor compEnt = entStrList.Select(tent => JsonHelper.GetObject<Compressor>(tent) as Compressor).First<Compressor>();
                compEnt.SeriesNo = compEnt.SeriesNo.ToUpper();
                if (String.IsNullOrEmpty(compEnt.Id))
                {
                    compEnt.CreateId = UserInfo.UserID;
                    compEnt.CreateName = UserInfo.Name;
                    compEnt.CreateTime = System.DateTime.Now;
                    compEnt.DoCreate();
                }
                else
                {
                    compEnt.DoUpdate();
                }
                if (!String.IsNullOrEmpty(compEnt.SkinId))//如果是散机不需要更新包装
                {
                    string sql = @"update SHHG_AimExamine..Skin set SkinState=(select SHHG_AimExamine.dbo.fun_GetSkinState('{0}')) where Id='{1}'";
                    sql = string.Format(sql, compEnt.SkinId, compEnt.SkinId);
                    DataHelper.ExecSql(sql);
                    Skin skinEnt = Skin.Find(compEnt.SkinId);
                }
                PageState.Add("Compressor", compEnt);
            }
        }
        private void DoSelect()
        {
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                //if (item.PropertyName == "BeginDate" && item.Value.ToString() != "")
                //{
                //    where += " and CreateTime>'" + item.Value + "' ";
                //}
                //else if (item.PropertyName == "EndDate" && item.Value.ToString() != "")
                //{
                //    where += " and CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                //}
                if (!string.IsNullOrEmpty(item.Value + ""))
                {
                    switch (item.PropertyName)
                    {
                        case "CName":
                            where += " and d." + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                        case "ProductCode":
                            where += " and g." + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                        default:
                            where += " and a." + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                    }
                }
            }
            sql = @"select A.Id,UPPER(A.SeriesNo) as SeriesNo,A.SkinId,UPPER(A.ModelNo) as ModelNo,A.ProductId,A.State,A.InWarehouseId,
                A.CreateTime,A.Remark,g.Code ProductCode,UPPER(B.SkinNo) SkinNo ,C.InWarehouseNo,c.CreateTime InWarehouseTime,
                D.Number,d.CreateTime OutWarehouseTime,D.CName,e.Number SaleOrderNo,f.Number ReturnOrderNo,f.CreateTime ReturnOrderTime,
                (select sum(StockQuantity) from SHHG_AimExamine..StockInfo where ProductId=g.Id ) as StockQuan, 
                (select top 1 Guids from SHHG_AimExamine..DelieryOrderPart where Did=d.Id and ProductId=g.Id) as  Guids    
                from SHHG_AimExamine..Compressor as  A                                         
                left join SHHG_AimExamine..Skin B on A.SkinId=B.Id
                left join SHHG_AimExamine..InWarehouse C on A.InWarehouseId=C.Id
                left join SHHG_AimExamine..DeliveryOrder D on D.Id=A.DeliveryOrderId
                left join SHHG_AimExamine..SaleOrders e on e.Id=d.Pid
                left join SHHG_AimExamine..ReturnOrder f on f.OrderNumber=e.Number
                left join SHHG_AimExamine..Products g on g.Isbn=a.ModelNo  
                where (select sum(StockQuantity) from SHHG_AimExamine..StockInfo where ProductId=g.Id ) !=
                (select count(1) from SHHG_AimExamine..Compressor where state='未出库' and UPPER(A.ModelNo)=Upper(g.Isbn)) " + where;
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " desc" : " asc";
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
