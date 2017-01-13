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
using System.Web.Script.Serialization;

namespace Aim.Examining.Web
{
    public partial class FrmUserWage : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "batchsave")
            {
                DoBatchSave();
            }
            else
            {
                DoSelect();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string where = " where UserId<>'46c5f4df-f6d1-4b36-96ac-d39d3dd65a5d' ";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "DeptName")
                {
                    where += " and " + db + ".dbo.get_DeptName(UserId) like '%" + item.Value + "%' ";
                }
                else
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            string sql = @"select UserID,[Name],WorkNo,LoginName," + db + ".dbo.get_DeptName(UserId) as DeptName,Wage from SysUser" + where;
            this.PageState.Add("UserList", GetPageData(sql, SearchCriterion));
        }


        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "DeptName";
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
        /// 批量保存
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchSave()
        {
            IList<string> entStrList = RequestData.GetList<string>("data");

            if (entStrList != null && entStrList.Count > 0)
            {
                SysUser user = null;
                Dictionary<string, object> dic = null;
                foreach (string str in entStrList)
                {
                    dic = FromJson(str) as Dictionary<string, object>;
                    if (dic != null)
                    {
                        user = SysUser.FindAllByPrimaryKeys(dic["UserID"]).FirstOrDefault<SysUser>();

                        if (user != null && user.Wage + "" != dic["Wage"] + "")
                        {
                            user.Wage = dic["Wage"] == null ? null : (decimal?)Convert.ToDecimal(dic["Wage"]);
                            user.DoUpdate();
                        }
                    }
                }
            }
        }

        //把json转换为可识别的
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
        }

    }
}
