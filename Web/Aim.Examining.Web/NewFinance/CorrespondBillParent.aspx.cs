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
    public partial class CorrespondBillParent : ExamListPage
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
            if (CC != "T")
            {
                sql = @"select A.Id,A.Name,A.MagId,A.MagUser,
               (select isnull(sum(TotalMoney),0)-isnull(sum(ReturnAmount),0)  from  SHHG_AimExamine..SaleOrders t where t.InvoiceType='发票' and  t.State is null and 
                t.DeState='已全部出库' and (t.InvoiceState is null or t.InvoiceState<>'已全部开发票') and t.Cid=A.Id ) as SaleAmount,
                (select isnull(sum(Amount-isnull(PayAmount,0)),0) from SHHG_AimExamine..OrderInvoice t where 
                (t.PayState is null or t.PayState<>'已全部付款') and  t.CId=A.Id) as InvoiceAmount,
                (select isnull(sum(Money-isnull(CorrespondAmount,0)),0) from SHHG_AimExamine..PaymentInvoice t where isnull(t.CorrespondState,'')!='已对应' and t.Cid=A.Id) PrepayAmount
                from SHHG_AimExamine..Customers A where A.MagId!='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' and 
                A.id not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')" + where;
            }
            else
            {
                sql = @"select A.Id,A.Name,A.MagId,A.MagUser,
               (select isnull(sum(TotalMoney),0)-isnull(sum(ReturnAmount),0) from  SHHG_AimExamine..SaleOrders t where t.InvoiceType='发票' and  t.State is null and 
                t.DeState='已全部出库' and (t.InvoiceState is null or t.InvoiceState<>'已全部开发票') and t.Cid=A.Id ) as SaleAmount,
                (select isnull(sum(Amount-isnull(PayAmount,0)),0) from SHHG_AimExamine..OrderInvoice t where 
                (t.PayState is null or t.PayState<>'已全部付款') and  t.CId=A.Id) as InvoiceAmount,
                (select isnull(sum(Money),0) from SHHG_AimExamine..PaymentInvoice t where isnull(t.CorrespondState,'')!='已对应' and t.Cid=A.Id) PrepayAmount
                from SHHG_AimExamine..Customers A where (A.MagId='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' or 
                A.id in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8'))" + where;
            }
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "InvoiceAmount";
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

