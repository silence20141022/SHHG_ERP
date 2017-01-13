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

namespace Aim.Examining.Web.BaseInfo
{
    public partial class CostPriceList : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "Save":
                    string id = RequestData.Get<string>("id");
                    decimal costprice = RequestData.Get<decimal>("CostPrice");
                    Product proEnt = Product.Find(id);
                    proEnt.CostPrice = costprice;
                    proEnt.DoUpdate();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = string.Empty;
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
            string sql = @"select * from SHHG_AimExamine..Products where 1=1" + where;
            IList<EasyDictionary> dics = GetPageData(sql, SearchCriterion);
            PageState.Add("DataList", dics);
            PageState.Add("ProductTypeEnum", SysEnumeration.GetEnumDict("ProductType"));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "Name";
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
        private void DoBatchSave()
        {
            IList<string> entStrList = RequestData.GetList<string>("data");
            string priceType = RequestData.Get<string>("PriceType");
            string sql = string.Empty;
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int k = 0; k < entStrList.Count; k++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[k]);
                    IList<ProductPriceDetail> ppdEnts = ProductPriceDetail.FindAllByProperties("ProductId", objL.Value<string>("ProductId"), "PriceTypeName", priceType);
                    if (ppdEnts.Count > 0)
                    {
                        ppdEnts[0].Price = objL.Value<decimal>(priceType);
                        ppdEnts[0].DoUpdate();
                    }
                    else
                    {
                        ProductPriceDetail ppdEnt = new ProductPriceDetail();
                        string parentId = DataHelper.QueryValue("select EnumerationID from SHHG_AimPortal.dbo.SysEnumeration where Code='PriceType'").ToString();
                        SysEnumeration[] seEnts = SysEnumeration.FindAllByProperties("Name", priceType, "ParentID", parentId);
                        if (seEnts.Length > 0)
                        {
                            ppdEnt.EnumerationID = seEnts[0].EnumerationID;
                        }
                        ppdEnt.PriceTypeName = priceType;
                        ppdEnt.ProductId = objL.Value<string>("ProductId");
                        ppdEnt.ProductName = objL.Value<string>("产品名称");
                        ppdEnt.ProductCode = objL.Value<string>("产品型号");
                        ppdEnt.ProductPCN = objL.Value<string>("PCN"); 
                        ppdEnt.Price = objL.Value<decimal>(priceType); 
                        ppdEnt.DoCreate();
                    }
                } 
            }
        }
    }
}

