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
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web.LogisticsManagement
{
    public partial class LogisticsSettlement : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "batchdelete":
                    IList<object> idList = RequestData.GetList<object>("IdList");
                    if (idList != null && idList.Count > 0)
                    {
                        foreach (object obj in idList)
                        {
                            //删除付款单的时候。要把物流单的付款状态由已提交还原为未付款
                            OtherPayBill opbEnt = OtherPayBill.Find(obj.ToString());
                            string[] temparray = opbEnt.InterfaceArray.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < temparray.Length; i++)
                            {
                                Logistic lEnt = Logistic.Find(temparray[i]);
                                lEnt.PayState = "未付款";
                                lEnt.DoUpdate();
                            }
                            opbEnt.DoDelete();
                        }
                        PageState.Add("Message", "删除成功！");
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
            string sql = string.Empty;
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.Value.ToString() != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }
            sql = @"select A.* from SHHG_AimExamine..OtherPayBill as A where PayType='物流付款'" + where;
            IList<EasyDictionary> dics = GetPageData(sql, SearchCriterion);
            PageState.Add("DataList", dics);
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
