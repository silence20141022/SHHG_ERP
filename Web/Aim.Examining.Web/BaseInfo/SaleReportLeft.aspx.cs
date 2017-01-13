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
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class SaleReportLeft : ExamListPage
    {
        private int index = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.RequestAction)
            {
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = ""; 
            string otherwhere = "";
            string sql = string.Empty;
            index = Convert.ToInt32(RequestData.Get<string>("Index"));  
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate")
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        otherwhere = " and CreateTime >= '" + item.Value.ToString() + "'";
                    }
                }
                else if (item.PropertyName == "EndDate")
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        otherwhere = " and CreateTime <= '" + item.Value.ToString() + "'";
                    }
                }
                else if (item.Value.ToString() != "")
                {
                    where += " and A." + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            if (index == 0)//以产品维度统计 
            {
                //                sql = @"select PId,PName as Name,PCode as Code,sum(Count) as SaleCount,sum(OutCount) as OutCount, 
                //                SHHG_AimExamine.dbo.fun_getProQuantity(PId) as StockQuantity
                //                from (select Id, PId,OId, PCode,PName,Count,OutCount from SHHG_AimExamine..OrdersPart
                //                where OId in (select Id from SHHG_AimExamine..SaleOrders where DeliveryState<>'已作废' 
                //                " + otherwhere + ")" + where + ") as A group by PId,PCode,PName";
                sql = @"select A.Id,A.Name,A.Code,(select isnull(sum(isnull(OutCount,0)-isnull(ReturnCount,0)),0) from SHHG_AimExamine..OrdersPart where PId=A.Id {0}) 
                as SaleCount,(select isnull(sum(isnull(StockQuantity,0)),0) from SHHG_AimExamine..StockInfo where ProductId=A.Id) as StockQuantity
                from SHHG_AimExamine..Products A where 1=1  {1} ";
                sql = string.Format(sql, otherwhere, where);
            }
            else//以客户的维度统计
            {
                //               sql = @"select A.*,SHHG_AimExamine.dbo.fun_getSaleAmountByCustomer(A.Id,'{0}','{1}') as SaleAmount,
                //                                SHHG_AimExamine.dbo.fun_GetLastExchangeDateByCustomer(A.Id) as LastExchangeDate        
                //                                from SHHG_AimExamine..Customers as A";
                sql = @"select A.Id,A.Name,A.MagUser,(select isnull(sum(isnull(TotalMoney,0)-isnull(DiscountAmount,0)-isnull(ReturnAmount,0)),0)
                from SHHG_AimExamine..SaleOrders where CId=A.Id {0}) as SaleAmount,
                (select Convert(varchar(10),Max(CreateTime),120) from SHHG_AimExamine..SaleOrders where CId=A.Id {0}) as LastExchangeDate
                from SHHG_AimExamine..Customers A where 1=1 {1}";
                sql = string.Format(sql, otherwhere, where);
            }
            PageState.Add("DetailList", GetPageData(sql, SearchCriterion));
            //PageState.Add("BeginDate", beginDate);
            //PageState.Add("EndDate", endDate);
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = string.Empty;
            if (index == 0)
            {
                order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Code";
            }
            else
            {
                order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "SaleAmount";
            }
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

