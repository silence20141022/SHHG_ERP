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
    public partial class InvalidOrderList : ExamListPage
    {
        private IList<SaleOrder> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else if (RequestData.Get<string>("optype") == "getChildData")
            {
                string oid = RequestData.Get<string>("OId");
                OrdersPart[] ops = OrdersPart.FindAllByProperties("OId", oid);
                PageState.Add("DetailList", ops);
            }
            else
            {
                DoSelect();
            }
        }
        private void DoSelect()
        {
            //ents =SaleOrder.FindAll(SearchCriterion);
            string sql = @"SELECT * FROM SHHG_AimExamine..SaleOrders AS s WHERE s.DeliveryState='已作废'";
            this.PageState.Add("OrderList", GetPageData(sql, SearchCriterion));
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

        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");

            if (idList != null && idList.Count > 0)
            {
                SaleOrder.DoBatchDelete(idList.ToArray());

                //删除对应的商品信息防止产生垃圾数据
                foreach (string oid in idList)
                {
                    OrdersPart.DeleteAll("OId='" + oid + "'");
                }
            }
        }
    }
}
