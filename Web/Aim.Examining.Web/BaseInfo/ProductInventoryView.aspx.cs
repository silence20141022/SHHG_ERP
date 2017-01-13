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
    public partial class ProductInventoryView : ExamListPage
    {
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

            string where = " where StockQuantity is not null and StockQuantity>0 ";
            if (RequestData.Get<string>("type") == "ysj")
            {
                where += " and Name='压缩机' ";
            }
            else if (RequestData.Get<string>("type") == "pj")
            {
                where += " and Name<>'压缩机' ";
            }
            else if (RequestData.Get<string>("type") == "all")
            {
                where = " where 1=1 ";
            }
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "WarehouseName")
                {
                    where += " and s.WarehouseName like '%" + item.Value + "%' ";
                }
                else
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }

            where += " and p.StorageRule='单件入库' ";

            //string sql = @"select *," + db + ".dbo.fun_getProQuantity(Id) as StockQuantity from " + db + "..Products" + where;
            string sql = "select p.Id,p.Isbn,p.Pcn,p.Unit,p.Code,p.Name,isnull(s.StockQuantity,0) as StockQuantity," + db + ".dbo.getNotDelivery(p.Id) as NotDelivery," + db + ".dbo.fun_getPurchaseQuanByProductId(p.Id) as NotInHouse,s.WarehouseName from " + db + "..Products p left join " + db + "..StockInfo s on p.Id=s.ProductId " + where;
            this.PageState.Add("ProductList", GetPageData(sql, SearchCriterion));
        }

        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Name, Code";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " asc" : " desc";
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

