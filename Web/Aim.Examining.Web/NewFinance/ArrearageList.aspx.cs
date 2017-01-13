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
using System.Data;

namespace Aim.Examining.Web
{
    public partial class ArrearageList : ExamListPage
    { 
        string Index = "";
        string sql = "";
        string CC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Index = RequestData.Get<string>("Index");
            CC = RequestData.Get<string>("CC");
            switch (RequestActionString)
            {
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value + ""))
                {
                    switch (item.PropertyName)
                    {
                        case "BeginDate":
                            where += " and CreateTime>'" + item.Value + "' ";
                            break;
                        case "EndDate":
                            where += " and CreateTime<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                            break;
                        default:
                            where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                    }
                }
            }
            if (CC != "T")
            {
                sql = @"select *,(select top 1 MagUser from SHHG_AimExamine..Customers where Id=CId ) as MagName
                from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and  
                Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')" + where;
            }
            else
            {
                sql = @"select *,(select top 1 MagUser from SHHG_AimExamine..Customers where Id=CId ) as MagName
                from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and  
                Cid in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')" + where;
            }
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");
            if (idList != null && idList.Count > 0)
            {
                OrderInvoice.DoBatchDelete(idList.ToArray());
            }
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CName";
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
