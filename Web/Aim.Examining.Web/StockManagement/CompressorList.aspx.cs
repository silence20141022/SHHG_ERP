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
    public partial class CompressorList : ExamListPage
    {
        private string sql = "";
        private string SkinId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SkinId = RequestData.Get<string>("SkinId");
            switch (RequestActionString)
            {
                case "IsIsbn":
                    string val = RequestData.Get<string>("Value");
                    IList<Product> proEnts = Product.FindAll("from Product where Isbn='" + val + "'");
                    if (proEnts.Count > 0)
                    {
                        PageState.Add("Code", proEnts[0].Code);
                        PageState.Add("IsIsbn", true);
                    }
                    else
                    {
                        PageState.Add("IsIsbn", false);
                    }
                    break;
                case "batchsave":
                    BatchSave();
                    break;
                case "Whole":
                    if (!string.IsNullOrEmpty(SkinId))
                    {
                        sql = @"select A.Id,UPPER(A.SeriesNo) as SeriesNo,A.SkinId,UPPER(A.ModelNo) as ModelNo,A.ProductId,A.State,A.InWarehouseId,A.CreateTime,A.Remark,
                        B.SkinNo ,C.InWarehouseNo,D.Number,D.CName from SHHG_AimExamine..Compressor as  A                 
                        left join (Select Id,UPPER(SkinNo) as SkinNo from SHHG_AimExamine..Skin) as B on A.SkinId=B.Id
                        left join (select Id,InWarehouseNo from SHHG_AimExamine..InWarehouse )as C on A.InWarehouseId=C.Id
                        left join (select Id,Number,CName from SHHG_AimExamine..DeliveryOrder) as D on D.Id=A.DeliveryOrderId
                        where A.SkinId ='" + SkinId + "'";
                        PageState.Add("CompressorList", DataHelper.QueryDictList(sql));
                        PageState.Add("SkinEntity", Skin.Find(SkinId));
                    }
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
            SearchCriterion.DefaultPageSize = 50;
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.Value.ToString() != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            if (!String.IsNullOrEmpty(SkinId))
            {
                PageState.Add("SkinEntity", Skin.Find(SkinId));
                sql = @"select A.Id,UPPER(A.SeriesNo) as SeriesNo,A.SkinId,UPPER(A.ModelNo) as ModelNo,A.ProductId,A.State,A.InWarehouseId,A.CreateTime,A.Remark,
                        B.SkinNo ,C.InWarehouseNo,D.Number,D.CName from SHHG_AimExamine..Compressor as  A                 
                        left join (Select Id,UPPER(SkinNo) as SkinNo from SHHG_AimExamine..Skin) as B on A.SkinId=B.Id
                        left join (select Id,InWarehouseNo from SHHG_AimExamine..InWarehouse )as C on A.InWarehouseId=C.Id
                        left join (select Id,Number,CName from SHHG_AimExamine..DeliveryOrder) as D on D.Id=A.DeliveryOrderId
                        where   A.SkinId ='" + SkinId + "'";
            }
            else
            {
                string condition = RequestData.Get<string>("Condition");
                if (condition == "All")
                {
                    sql = @"select A.Id,UPPER(A.SeriesNo) as SeriesNo,A.SkinId,UPPER(A.ModelNo) as ModelNo,A.ProductId,A.State,A.InWarehouseId,A.CreateTime,
                    B.SkinNo,C.InWarehouseNo,D.Number,D.CName from SHHG_AimExamine..Compressor as  A  
                    left join (Select Id,UPPER(SkinNo) as SkinNo from SHHG_AimExamine..Skin) as B on A.SkinId=B.Id
                    left join (select Id,InWarehouseNo from SHHG_AimExamine..InWarehouse )as C on A.InWarehouseId=C.Id
                    left join (select Id,Number,CName from SHHG_AimExamine..DeliveryOrder) as D on D.Id=A.DeliveryOrderId
                    where  1=1" + where;
                }
                else //(condition == "NoSkin")如果无空箱 默认显示所有的散机
                {
                    sql = @"select A.Id,UPPER(A.SeriesNo) as SeriesNo,A.SkinId,UPPER(A.ModelNo) as ModelNo,A.ProductId,A.State,A.InWarehouseId,
                    A.CreateTime,C.InWarehouseNo,D.Number,D.CName from SHHG_AimExamine..Compressor as A      
                    left join (select Id,InWarehouseNo from SHHG_AimExamine..InWarehouse )as C on A.InWarehouseId=C.Id
                    left join (select Id,Number,CName from SHHG_AimExamine..DeliveryOrder) as D on D.Id=A.DeliveryOrderId
                    where A.SkinId is null" + where;
                }
            }
            PageState.Add("CompressorList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "SeriesNo";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " asc" : " desc";
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
