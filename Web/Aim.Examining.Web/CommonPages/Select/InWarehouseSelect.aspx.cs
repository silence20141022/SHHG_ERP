using System;
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
namespace Aim.Examining.Web.CommonPages.Select
{
    public partial class InWarehouseSelect : BaseListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }

        private void DoSelect()
        {
            string supplierId = RequestData.Get<string>("SupplierId");
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
            }
            string sql = @"select A.*,SHHG_AimExamine.dbo.fun_HaveInQuantity(A.Id) as HaveIn,
            SHHG_AimExamine.dbo.fun_NoInQuantity(A.Id) as NoIn
            from SHHG_AimExamine..PurchaseOrderDetail as A         
            left join SHHG_AimExamine..PurchaseOrder as B on B.Id=A.PurchaseOrderId     
            where A.InWarehouseState='未入库' and  SHHG_AimExamine.dbo.fun_NoInQuantity(A.Id)>0 
            and B.SupplierId='{0}'" + where;
            sql = string.Format(sql, supplierId);
            this.PageState.Add("PurchaseOrderList", FormatData(GetPageData(sql, SearchCriterion)));
        }
        //sql 分页
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "PurchaseOrderId";
            string asc = search.Orders.Count <= 0 || search.Orders[0].Ascending ? " asc" : " desc";
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
        private IList<EasyDictionary> FormatData(IList<EasyDictionary> dics)
        {
            string same = string.Empty;
            for (int i = 0; i < dics.Count; i++)
            {
                if (dics[i].Get<string>("PurchaseOrderId") == same)
                {
                    dics[i].Set("PurchaseOrderNo", "");
                }
                else
                {
                    same = dics[i].Get<string>("PurchaseOrderId");
                }
            }
            return dics;
        }

    }
}


