using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class OtherInWarehouseList : ExamListPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = RequestData.Get<string>("id");
            switch (RequestActionString)
            {
                case "Modify":
                    PageState.Add("result", AllowOperate(id) == true ? "true" : "false");
                    break;
                case "batchdelete":
                    IList<object> idList = RequestData.GetList<object>("IdList");
                    if (idList != null && idList.Count > 0)
                    {
                        foreach (object obj in idList)
                        {
                            bool allowDelete = AllowOperate(obj.ToString());
                            if (allowDelete)
                            {
                                DataHelper.ExecSql("delete SHHG_AimExamine..InWarehouse where Id='" + obj.ToString() + "'");
                                OtherInWarehouseDetail.DeleteAll("InWarehouseId='" + obj + "'");
                                PageState.Add("Message", "删除成功！");
                            }
                            else
                            {
                                PageState.Add("Message", "有实际入库记录的入库单不允许删除！");
                                return;
                            }
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
            string state = RequestData.Get<string>("State");
            if (!string.IsNullOrEmpty(state))
            {
                SearchCriterion.AddSearch("State", state);
            }
            string sql = string.Empty;
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
                    if (item.PropertyName != "ProductCode")
                    {
                        where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                    else
                    {
                        detailwhere = "where " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                }
            }
            sql = @"select A.* from SHHG_AimExamine..InWarehouse as A                
                where A.InWarehouseType !='采购入库' and A.Id in (select distinct InWarehouseId from SHHG_AimExamine..OtherInWarehouseDetail
                " + detailwhere + ")" + where;

            this.PageState.Add("InWarehouseList", GetPageData(sql, SearchCriterion));
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
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * search.PageSize + 1, search.CurrentPageIndex * search.PageSize);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }
        private bool AllowOperate(string id)
        {
            //通过入库单Id去查询实际入库详细，如果有入库记录  拒绝修改和删除
            bool allow = true;
            string inwareSql = @"select A.Id from SHHG_AimExamine..InWarehouseDetailDetail as A 
                            left join SHHG_AimExamine..OtherInWarehouseDetail as B on A.OtherInWarehouseDetailId=B.Id 
                            where B.InWarehouseId='{0}'";
            inwareSql = string.Format(inwareSql, id);
            IList<EasyDictionary> iwdDics = DataHelper.QueryDictList(inwareSql);//判断入库单详细中有没有该入库单的产品
            if (iwdDics.Count > 0)
            {
                allow = false;
            }
            return allow;
        }
    }
}
