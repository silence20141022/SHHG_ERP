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
using System.Configuration;
using Aim.WorkFlow;

namespace Aim.Examining.Web
{
    public partial class NoInvoiceSaleOrderList : ExamListPage
    {
        string sql = "";
        string Index = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Index = RequestData.Get<string>("Index");
            switch (RequestActionString)
            {
                case "CancelSaleOrder":
                    string ids = RequestData.Get<string>("ids");
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        IList<SaleOrder> soEnts = SaleOrder.FindAll(SearchCriterion, Expression.In(SaleOrder.Prop_Id, idArray));
                        foreach (SaleOrder soEnt in soEnts)
                        {
                            soEnt.State = "已作废";
                            soEnt.DoUpdate();
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
            }//显示所有只需要开具收据且已全部出库的销售单 且状态state is null 表示未作废
            if (Index == "0")
            {
                sql = @"select * from  SHHG_AimExamine..SaleOrders where InvoiceType='收据' and  State is null and 
                DeState='已全部出库' and (PayState is null or PayState='部分付款') and (ISNULL(TotalMoney, 0) > ISNULL(ReturnAmount, 0))" + where;
            }
            if (Index == "1")
            {
                sql = @"select * from  SHHG_AimExamine..SaleOrders where InvoiceType='收据' and  State is null and 
                DeState='已全部出库' and  PayState='已全部付款' and (ISNULL(TotalMoney, 0) > ISNULL(ReturnAmount, 0))" + where;
            }
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
