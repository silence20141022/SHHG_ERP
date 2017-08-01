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

namespace Aim.Examining.Web.SaleManagement
{
    public partial class SaleOrderDetailReport : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }
        private void DoSelect()
        {
            string where = "";
            string productType = RequestData.Get<string>("ProductType");
            if (!string.IsNullOrEmpty(productType))
            {
                SearchCriterion.AddSearch("ProductType", productType);
            }
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate1" && item.Value + "" != "")
                {
                    where += " and B.CreateTime>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate1" && item.Value + "" != "")
                {
                    where += " and B.CreateTime<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.PropertyName == "BeginDate" && item.Value + "" != "")
                {
                    where += " and B.EndDate>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate" && item.Value + "" != "")
                {
                    where += " and B.EndDate<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.Value + "" != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            string userid = Aim.Portal.Web.WebPortalService.CurrentUserInfo.UserID;
            //2017-07-21 新增用户 f54d92c5-6b98-4120-8c5d-9b770125a0e5 沈艳霞
            if (userid == "97968039-a657-467b-b735-04dab4a9b60c" || userid == "c7712e33-2b20-463e-8910-8f234cf260ef" || userid == "46c5f4df-f6d1-4b36-96ac-d39d3dd65a5d" || userid == "7580578a-ce30-47ef-9a7e-95ca8ac97df3" || userid == "f54d92c5-6b98-4120-8c5d-9b770125a0e5")
            { 
                where += "";
            }
            else
            {
                where += " and B.SalesmanId='" + userid + "'";
            }
            string sql = @"select A.*,B.Number,B.CName,B.Salesman,B.EndDate,C.ProductType,C.Pcn,C.CostPrice,C.SalePrice as BuyPrice,
                                (A.SalePrice/
                                ((isnull(C.SalePrice,1))*(select top(1) Rate from SHHG_AimExamine..ExchangeRate where Symbo =(select top(1) Symbo from SHHG_AimExamine..Supplier where Id=C.SupplierId ))))
                                as ProfitRate,
                                SHHG_AimExamine.dbo.fun_CaculateCostAmount(A.PId,A.Count,A.SalePrice) as CostAmount,
                                (select top(1) Rate from SHHG_AimExamine..ExchangeRate where Symbo =(select top(1) Symbo from SHHG_AimExamine..Supplier where Id=C.SupplierId )) as Rate,
                                B.CreateTime
                                from (
                                select OId,PId,PCode,PName,Isbn,sum(count) as Count,max(SalePrice) as SalePrice, sum(Amount) as Amount,max(CustomerOrderNo) as CustomerOrderNo from(
                                select OId,PId,PCode,PName,Isbn,(Count-isnull(ReturnCount,0)) as Count,SalePrice,Amount,CustomerOrderNo from SHHG_AimExamine..OrdersPart 
                                )t
                                group by PId,PCode,PName,Isbn,OId,isnull(CustomerOrderNo,'') having sum([Count])>0
                                ) as A
                                left join SHHG_AimExamine..SaleOrders as B on A.OId=B.Id 
                                left join SHHG_AimExamine..Products as C on C.Id=A.PId 
                                where (B.DeliveryState is null or B.DeliveryState<>'已作废') " + where;

            PageState.Add("OrderList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> FormatData(IList<EasyDictionary> dics)
        {
            string same = string.Empty;
            for (int i = 0; i < dics.Count; i++)
            {
                if (dics[i].Get<string>("Number") == same)
                {
                    dics[i].Set("Number", ""); dics[i].Set("CName", "");
                }
                else
                {
                    same = dics[i].Get<string>("Number");
                }
            }
            return dics;
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Number";
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
