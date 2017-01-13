using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using NHibernate.Criterion;
using Aim.Portal.Model;
using Aim.Data;
using Aim.Examining.Web;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web.StockManagement
{
    public partial class WarehouseExchangeList : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "batchdelete":
                    IList<object> idList = RequestData.GetList<object>("IdList");
                    if (idList != null && idList.Count > 0)
                    {
                        foreach (object obj in idList)
                        {
                            InWarehouse[] iwEnts = InWarehouse.FindAllByProperties("PublicInterface", obj.ToString());
                            if (iwEnts.Length > 0)
                            {
                                OtherInWarehouseDetail.DeleteAll("InWarehouseId='" + iwEnts[0].Id + "'");
                                InWarehouse.DeleteAll("PublicInterface='" + obj.ToString() + "'");
                            }
                            DeliveryOrder[] doEnts = DeliveryOrder.FindAllByProperties("PId", obj.ToString());
                            if (doEnts.Length > 0)
                            {
                                DelieryOrderPart.DeleteAll("DId='" + doEnts[0].Id + "'");
                                DeliveryOrder.DeleteAll("PId='" + obj.ToString() + "'");
                            }
                            DataHelper.ExecSql("delete SHHG_AimExamine..WarehouseExchange where Id='" + obj.ToString() + "'");
                            DataHelper.ExecSql("delete SHHG_AimExamine..WarehouseExchangeDetail where WarehouseExchangeId='" + obj.ToString() + "'");
                            PageState.Add("Message", "删除成功！");

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
            string sql = string.Empty;
            string payType = RequestData.Get<string>("PayType");
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate" && item.Value.ToString() != "")
                {
                    where += " and SendDate>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate" && item.Value.ToString() != "")
                {
                    where += " and SendDate<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.Value.ToString() != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
            sql = @"select A.*,B.ProductCode from SHHG_AimExamine..WarehouseExchange as A 
            left join SHHG_AimExamine..WarehouseExchangeDetail as B on A.Id=B.WarehouseExchangeId
            where 1=1" + where;
            IList<EasyDictionary> dics = GetPageData(sql, SearchCriterion);
            PageState.Add("DataList", dics);
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
