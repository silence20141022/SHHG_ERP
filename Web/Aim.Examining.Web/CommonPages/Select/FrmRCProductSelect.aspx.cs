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
using System.Configuration;
namespace Aim.Examining.Web.CommonPages.Select
{
    public partial class FrmRCProductSelect : BaseListPage
    {

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string onumber = RequestData.Get<string>("onumber");
            string where = "";

            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and P" + item.PropertyName + " like '%" + item.Value + "%' ";
            }
            string PId = Request.QueryString["PId"];//订单ID
            string sql = @"select b.PId as Id,a.PCode as Code,a.PName as [Name],a.Isbn,a.[Count],a.Guids,a.Unit,b.SalePrice from
                        (select * from " + db + "..DelieryOrderPart where DId in (select Id from " + db + "..DeliveryOrder where PId=(select top 1 Id " +
                        "from " + db + "..SaleOrders where Number='" + onumber + "')) and [State]='已出库' " + where + ") a inner join " + db + "..OrdersPart b on a.PId=b.Id ";
            this.PageState.Add("ProductList", GetPageData(sql, SearchCriterion));
        }

        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Name";
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

        #endregion
    }
}


