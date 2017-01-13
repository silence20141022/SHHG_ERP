using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;

namespace Aim.Examining.Web.StockManagement
{
    public partial class ProductInWarehouse : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "Judge":
                    string id = RequestData.Get<string>("id");
                    if (!string.IsNullOrEmpty(id))
                    {
                        InWarehouse iwEnt = InWarehouse.Find(id);
                        WarehouseExchange weEnt = WarehouseExchange.Find(iwEnt.PublicInterface);
                        if (weEnt.OutWarehouseState == "已出库")
                        {
                            PageState.Add("Allow", true);
                        }
                        else
                        {
                            PageState.Add("Allow", false);
                        }
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = "";
            string detailwhere = "";
            string sql = string.Empty;
            int index = Convert.ToInt32(RequestData.Get<string>("Index"));
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!String.IsNullOrEmpty(item.Value.ToString()))
                {
                    if (item.PropertyName != "ProductCode")
                    {
                        where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                    else
                    {
                        detailwhere += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                }
            }
            if (index == 0)
            {
                sql = @"select A.* from SHHG_AimExamine..InWarehouse as A where A.State='未入库' and 
                ((A.InWarehouseType='采购入库' and  A.Id in (select distinct InWarehouseId from SHHG_AimExamine..InWarehouseDetail where 1=1 {0}))
                or (A.InWarehouseType!='采购入库' and A.Id in (select distinct InWarehouseId from SHHG_AimExamine..OtherInWarehouseDetail where 1=1 {1}))){2}";
                sql = string.Format(sql, detailwhere, detailwhere, where);
            }
            else
            {
                sql = @"select A.* from SHHG_AimExamine..InWarehouse as A where A.State='已入库' and 
                ((A.InWarehouseType='采购入库' and  A.Id in (select distinct InWarehouseId from SHHG_AimExamine..InWarehouseDetail where 1=1 {0}))
                or (A.InWarehouseType!='采购入库' and A.Id in (select distinct InWarehouseId from SHHG_AimExamine..OtherInWarehouseDetail where 1=1 {1}))){2}";
                sql = string.Format(sql, detailwhere, detailwhere, where);
            }
            PageState.Add("InWarehouse", GetPageData(sql, SearchCriterion));
        }
        //sql 分页
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
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * 120 + 1, search.CurrentPageIndex * 120);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }
    }
}
