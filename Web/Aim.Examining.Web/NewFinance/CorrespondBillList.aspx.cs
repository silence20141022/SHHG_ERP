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

namespace Aim.Examining.Web
{
    public partial class CorrespondBillList : ExamListPage
    { 
        string sql = ""; 
        string CId = ""; 
        protected void Page_Load(object sender, EventArgs e)
        { 
            CId = RequestData.Get<string>("CId"); 
            switch (RequestActionString)
            {
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
                            where += " and CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                            break;
                        default:
                            where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                            break;
                    }
                }
            }
            sql = @"select * from (
                select Id,Number, CId,(TotalMoney-isnull(ReturnAmount,0)-isnull(DiscountAmount,0)) as BorrowMoney,
                null as PayMoney,CreateTime,'借款' as MoneyType,Remark,CreateName from SHHG_AimExamine..SaleOrders t where
                t.InvoiceType='发票' and  t.State is null and  t.DeState='已全部出库' and (t.InvoiceState is null or t.InvoiceState<>'已全部开发票') 
                and CId='{0}'                
                union 
                select Id,Number,CId,Amount as BorrowMoney,null as PayMoney,CreateTime,'借款' as MoneyType,
                Remark,CreateName from SHHG_AimExamine..OrderInvoice t where t.Invalid is null and t.CId='{0}'    
                union
                select Id,null as Number, CId,null as BorrowMoney,Money as PayMoney,ReceivablesTime as CreateTime,'还款' as MoneyType,Remark,CreateName
                from SHHG_AimExamine..PaymentInvoice t where t.BillType='发票' and t.CId='{0}'                
                ) as t where 1=1 " + where;
            sql = string.Format(sql, CId);
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
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

