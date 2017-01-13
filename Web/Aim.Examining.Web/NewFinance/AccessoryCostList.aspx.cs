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
    public partial class AccessoryCostList : ExamListPage
    {
        string sql = "";
        string id = "";
        string InvoiceNo = "";
        decimal NewCostPrice = 0;
        DelieryOrderPart dopEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            switch (RequestActionString)
            {
                case "Update":
                    id = RequestData.Get<string>("id");
                    InvoiceNo = RequestData.Get<string>("InvoiceNo");
                    NewCostPrice = RequestData.Get<decimal>("NewCostPrice");
                    dopEnt = DelieryOrderPart.Find(id);
                    dopEnt.CostPrice = NewCostPrice;
                    dopEnt.CostAmount = Math.Round(NewCostPrice * Convert.ToDecimal(dopEnt.Count) / Convert.ToDecimal(1.17), 2);
                    dopEnt.InvoiceNo = InvoiceNo;
                    dopEnt.DoUpdate();
                    Product pEnt = Product.Find(dopEnt.ProductId);
                    if (pEnt.CostPrice != NewCostPrice)
                    {
                        pEnt.CostPrice = NewCostPrice;
                        pEnt.DoUpdate();
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
                            where += " and A.CreateTime>'" + item.Value + "' ";
                            break;
                        case "EndDate":
                            where += " and A.CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                            break;
                        case "Number":
                        case "CName":
                            where += " and C." + item.PropertyName + " like '%" + item.Value + "%'";
                            break;
                        default:
                            where += " and A." + item.PropertyName + " like '%" + item.Value + "%'";
                            break;
                    }
                }
            }//A.State='已出库' and
            sql = @"select A.Id,A.DId,A.PCode,A.PName,A.Count,A.CostPrice,A.CostAmount,A.InvoiceNo,A.CreateTime,A.Unit,
                  B.CostPrice as NewCostPrice,B.PCN,C.Number,C.CName 
                  from SHHG_AimExamine..DelieryOrderPart A  
                  left join SHHG_AimExamine..Products as B on A.ProductId=B.Id
                  left join SHHG_AimExamine..DeliveryOrder as C on A.DId=C.Id
                  left join SHHG_AimExamine..SaleOrders as D on C.PId=D.Id
                  where  B.ProductType='配件' and D.InvoiceType='发票' " + where;
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

