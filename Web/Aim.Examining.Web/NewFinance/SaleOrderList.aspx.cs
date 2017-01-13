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
using System.Data;

namespace Aim.Examining.Web
{
    public partial class SaleOrderList : ExamListPage
    {
        string CC = "";
        string sql = "";
        SaleOrder soEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            CC = RequestData.Get<string>("CC");
            string ids = RequestData.Get<string>("ids");
            switch (RequestActionString)
            {
                case "ResetSaleOrderDetail":
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string oid in idArray)
                        {
                            IList<OrdersPart> opEnts = OrdersPart.FindAllByProperty(OrdersPart.Prop_OId, oid);
                            foreach (OrdersPart opEnt in opEnts)
                            {
                                opEnt.BillingCount = null;
                                opEnt.DoUpdate();
                            }
                            soEnt = SaleOrder.Find(oid);
                            soEnt.InvoiceState = null;
                            soEnt.DoUpdate();
                        }
                    }
                    break;
                case "SetReceiptModel":
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            soEnt = SaleOrder.Find(idArray[i]);
                            soEnt.InvoiceType = "收据";
                            soEnt.DoUpdate();
                        }
                    }
                    break;
                case "DelayInvoice":
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            soEnt = SaleOrder.Find(idArray[i]);
                            soEnt.State = "暂缓开票";
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
            }
            if (CC != "T")  //将分子公司的销售单分开 张勇UserId 56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1 (isnull(TotalMoney,0)-isnull(ReturnAmount,0)-isnull(DiscountAmount,0))>0
            {
                sql = @"select * from  SHHG_AimExamine..SaleOrders t where InvoiceType='发票' and  State is null and 
                                 DeState='已全部出库' and (InvoiceState is null or InvoiceState<>'已全部开发票') and 
                                 SalesmanId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' and (ISNULL(TotalMoney, 0) > ISNULL(ReturnAmount, 0)) and 
                                Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')" + where;
            }
            if (CC == "T")
            {
                sql = @"select * from  SHHG_AimExamine..SaleOrders t where InvoiceType='发票' and State is null and 
                DeState='已全部出库' and (InvoiceState is null or InvoiceState<>'已全部开发票') and (ISNULL(TotalMoney, 0) > ISNULL(ReturnAmount, 0)) and
                (SalesmanId='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' or 
                Cid  in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8'))" + where;
            }
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(1) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";//当没有在前台点击排序列的时候 默认的排序字段
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " asc" : " desc";//默认降序  
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
