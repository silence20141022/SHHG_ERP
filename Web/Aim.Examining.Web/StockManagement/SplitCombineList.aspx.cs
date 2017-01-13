using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Aim.Portal.Model;
using Aim.Data;
using System.Data.SqlClient;
using System.Data;
using NHibernate.Criterion;

namespace Aim.Examining.Web.StockManagement
{
    public partial class SplitCombineList : ExamListPage
    {
        string skinNo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string sql = string.Empty;
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    if (item.PropertyName == "PCode" || item.PropertyName == "PName")
                    {
                        where += " and B." + item.PropertyName + " like '%" + item.Value + "%' ";
                    }
                    else
                    {
                        where += " and C." + item.PropertyName + " like '%" + item.Value + "%' ";
                    }
                }
            }
            sql = @"select A.*,B.PName,B.PCode,C.CName,C.CCode from SHHG_AimExamine..ProductsPart as A  
 left join (select Name as PName ,Code as PCode,Id from SHHG_AimExamine..Products) as B on A.PId=B.Id 
 left join (select Name as CName,Code as CCode,Id  from SHHG_AimExamine..Products) as C on A.CId=C.Id
where 1=1 
" + where;

            //
            // left join (select Name as PName ,Code as PCode,Id from SHHG_AimExamine..Products) as B on A.PId=B.Id
            //left join (select Name as CName,Code as CCode,Id  from SHHG_AimExamine..Products) as C on A.CId=C.Id
            PageState.Add("DataList", FormatData(GetPageData(sql, SearchCriterion)));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "PCode";
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
        private IList<EasyDictionary> FormatData(IList<EasyDictionary> dics)
        {
            string same = string.Empty;
            for (int i = 0; i < dics.Count; i++)
            {
                if (dics[i].Get<string>("PId") == same)
                {
                    dics[i].Set("PName", "");
                    dics[i].Set("PCode", "");
                }
                else
                {
                    same = dics[i].Get<string>("PId");
                }
            }
            return dics;
        }
    }
}
