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
    public partial class FrmUserWageDetail : ExamListPage
    {
        string db = ConfigurationManager.AppSettings["ExamineDB"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "batchsave")
            {
                DoBatchSave();
            }
            else if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else if (RequestActionString == "openwage")
            {
                DataHelper.ExecSp(db + "..sp_InsertUserWage");
            }
            else
            {
                DoSelect();
            }
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
                UserWage.DoBatchDelete(idList.ToArray());
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string where = " where 1=1 ";
            bool hasStage = false;
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                if (item.PropertyName == "Stage" && item.Value + "" != "")
                {
                    hasStage = true;
                }
            }
            if (!hasStage)
            {
                string temp = DataHelper.ExecSql("select max(stage) from " + db + "..UserWage") + "";
                where += " and stage='" + temp + "' ";
            }
            string sql = @"select Id,UserID,UserName,WorkNo,LoginName,DeptName,Wage,Bonus,Total,CreateTime,Remark,Stage from " + db + "..UserWage" + where;
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
                UserWage user = null;
                Dictionary<string, object> dic = null;
                foreach (string str in entStrList)
                {
                    dic = FromJson(str) as Dictionary<string, object>;
                    if (dic != null)
                    {
                        user = UserWage.FindAllByPrimaryKeys(dic["Id"]).FirstOrDefault<UserWage>();

                        if (user != null && user.Wage + "" != dic["Wage"] + "")
                        {
                            user.Bonus = dic.ContainsKey("Bonus") && (dic["Bonus"] + "") != "" ? (decimal?)Convert.ToDecimal(dic["Bonus"]) : null;
                            user.Total = dic.ContainsKey("Total") && (dic["Total"] + "") != "" ? (decimal?)Convert.ToDecimal(dic["Total"]) : null;
                            user.Remark = dic.ContainsKey("Remark") ? dic["Remark"] + "" : "";
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
