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
using System.Data;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class CombineSplitList : ExamListPage
    {
        CombineSplit csEnt = null;
        IList<CombineSplitDetail> csdEnts = null;
        StockInfo siEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    if (RequestData.Get<string>("optype") == "getChildData")
                    {
                        string combineSplitId = RequestData.Get<string>("CombineSplitId");
                        IList<CombineSplitDetail> pidEnts = CombineSplitDetail.FindAll(new Order("ProductCode", true), Expression.Eq("CombineSplitId", combineSplitId));
                        PageState.Add("DetailDataList", pidEnts);
                    }
                    else
                    {
                        DoSelect();
                    }
                    break;
            }
        }
        private void DoSelect()
        {
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
            string sql = @"select A.*,B.Pcn  from SHHG_AimExamine..CombineSplit as A 
            left join SHHG_AimExamine..Products as B on A.ProductId=B.Id  where 1=1" + where;
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");
            if (idList != null && idList.Count > 0)
            {
                foreach (object obj in idList)
                {
                    csEnt = CombineSplit.Find(obj);
                    siEnt = StockInfo.FindFirstByProperties("ProductId", csEnt.ProductId, "WarehouseId", csEnt.WarehouseId);
                    csdEnts = CombineSplitDetail.FindAllByProperties("CombineSplitId", csEnt.Id);
                    if (csEnt.OperateType == "组装")
                    {
                        siEnt.StockQuantity -= csEnt.ProductQuantity;
                        siEnt.DoUpdate();
                        foreach (CombineSplitDetail csdEnt in csdEnts)
                        {
                            siEnt = StockInfo.FindFirstByProperties("ProductId", csdEnt.ProductId, "WarehouseId", csEnt.WarehouseId);
                            siEnt.StockQuantity += csdEnt.ProductQuantity;
                            siEnt.DoUpdate();
                            csdEnt.DoDelete();
                        }
                    }
                    else
                    {
                        siEnt.StockQuantity += csEnt.ProductQuantity;
                        siEnt.DoUpdate();
                        foreach (CombineSplitDetail csdEnt in csdEnts)
                        {
                            siEnt = StockInfo.FindFirstByProperties("ProductId", csdEnt.ProductId, "WarehouseId", csEnt.WarehouseId);
                            siEnt.StockQuantity -= csdEnt.ProductQuantity;
                            siEnt.DoUpdate();
                            csdEnt.DoDelete();
                        }
                    }
                    csEnt.DoDelete();
                }
            }
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || search.Orders[0].Ascending ? " desc" : " asc";
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

