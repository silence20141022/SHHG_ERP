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
    public partial class ExchangeRateList : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "batchsave":
                    DoBatchSave();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            ExchangeRate[] erEnts = ExchangeRate.FindAll(SearchCriterion);
            PageState.Add("DataList", erEnts);
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
            string sql = string.Empty;
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int k = 0; k < entStrList.Count; k++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[k]);
                    if (!string.IsNullOrEmpty(objL.Value<string>("Id")))
                    {
                        ExchangeRate erEnt = ExchangeRate.Find(objL.Value<string>("Id"));
                        erEnt.ModifyDate = System.DateTime.Now;
                        erEnt.ModifyUserId = UserInfo.UserID;
                        erEnt.ModifyName = UserInfo.Name;
                        erEnt.MoneyType = objL.Value<string>("MoneyType");
                        erEnt.Rate = objL.Value<decimal>("Rate");
                        erEnt.Symbo = objL.Value<string>("Symbo");
                        erEnt.Remark = objL.Value<string>("Remark");
                        erEnt.DoUpdate();
                    }
                    else
                    {
                        ExchangeRate erEnt = new ExchangeRate();
                        erEnt.MoneyType = objL.Value<string>("MoneyType"); ;
                        erEnt.Rate = objL.Value<decimal>("Rate");
                        erEnt.Symbo = objL.Value<string>("Symbo");
                        erEnt.CreateId = UserInfo.UserID;
                        erEnt.CreateName = UserInfo.Name;
                        erEnt.ModifyDate = System.DateTime.Now;
                        erEnt.ModifyUserId = UserInfo.UserID;
                        erEnt.ModifyName = UserInfo.Name;
                        erEnt.CreateTime = System.DateTime.Now;
                        erEnt.Remark = objL.Value<string>("Remark");
                        erEnt.DoCreate();
                    }
                }
            }
        }
    }
}

