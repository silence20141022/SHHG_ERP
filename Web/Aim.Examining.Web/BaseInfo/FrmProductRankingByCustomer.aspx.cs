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
    public partial class FrmProductRankingByCustomer : ExamListPage
    {
        private IList<Product> ents = null;

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            Product ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Product>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                default:
                    DoSelect();
                    break;
            }

        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string where = " where 1=1 ";
            string beginDate = "";
            string endDate = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate")
                {
                    beginDate = (item.Value + "").Replace(" 0:00:00", "");
                }
                else if (item.PropertyName == "EndDate")
                {
                    endDate = (item.Value + "").Replace(" 0:00:00", "");
                }
                else
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }

            string sql = @"select Id,Code,[Name]," + db + ".dbo.[fun_getCustomerByCount](Id,'" + beginDate + "','" + endDate + "') as BuyCount from " + db + "..Customers " + where;
            this.PageState.Add("ProductList", GetPageData(sql, SearchCriterion));
        }

        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "BuyCount";
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

