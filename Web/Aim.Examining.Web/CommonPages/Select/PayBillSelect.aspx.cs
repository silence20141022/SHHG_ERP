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
    public partial class PayBillSelect : BaseListPage
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
                if (item.PropertyName == "PurchaseOrderNo")
                {
                    where += " and B." + item.PropertyName + " like '%" + item.Value + "%'";
                }
                else
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
//            string sql = @"select A.*, B.CreateTime,(A.Quantity-SHHG_AimExamine.dbo.fun_NoInPayBillQuan(A.Id)) as HavePay,
//            SHHG_AimExamine.dbo.fun_NoInPayBillQuan(A.Id) as NoPay,
//            SHHG_AimExamine.dbo.fun_getProQuantity(A.ProductId) as StockQuantity 
//            from SHHG_AimExamine..PurchaseOrderDetail as A            
//            left join SHHG_AimExamine..PurchaseOrder as B on A.PurchaseOrderId=B.Id             
//            where A.PayState='未付款' and B.SupplierId='{0}' and SHHG_AimExamine.dbo.fun_NoInPayBillQuan(A.Id) >0" + where;
            string sql = @"select A.Id,A.ProductId,A.Name,A.Code,A.BuyPrice,A.Quantity,A.Amount,B.PurchaseOrderNo, B.CreateTime,
            (A.Quantity-(select isnull(sum(PayQuantity),0) from SHHG_AimExamine..PayBillDetail where PurchaseOrderDetailId=A.Id)) as NoPay          
            from SHHG_AimExamine..PurchaseOrderDetail as A            
            left join SHHG_AimExamine..PurchaseOrder as B on A.PurchaseOrderId=B.Id             
            where B.SupplierId='{0}' and 
            (A.Quantity-(select isnull(sum(PayQuantity),0) from SHHG_AimExamine..PayBillDetail where PurchaseOrderDetailId=A.Id))>0" + where;
            sql = string.Format(sql, supplierId);//显示所有未关联到付款单的采购详细
            PageState.Add("PurchaseOrderDetailList", GetPageData(sql, SearchCriterion));
        }
        //sql 分页
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


