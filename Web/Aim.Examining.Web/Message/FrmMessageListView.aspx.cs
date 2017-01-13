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
using System.Configuration;

namespace Aim.Examining.Web.Message
{
    public partial class FrmMessageListView : ExamListPage
    {
        #region 变量

        private IList<MessageInfo> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string op = RequestData.Get<string>("op");
            switch (this.RequestAction)
            {
                default:
                    IList<EasyDictionary> entList = null;
                    if (op == "SelCollection")//(RequestActionString == "SelCollection")
                    {
                        //现在收藏之后，没权限点我的收藏也可以看到

                        string sql = "select msg.* from " + ConfigurationManager.AppSettings["ExamineDB"] + "..MessageInfo as msg inner join " + ConfigurationManager.AppSettings["ExamineDB"] + "..CollectionToUser on msgId=msg.id "
                            + "where userid='" + UserInfo.UserID + "' and ReleaseState='1'";
                        entList = DataHelper.DataTableToDictList(DataHelper.QueryDataTable(sql));
                    }
                    else if (op == "read")
                    {
                        DoSelect("read");
                    }
                    else if (op == "notread")
                    {
                        DoSelect("notread");
                    }
                    else
                    {
                        DoSelect("");
                    }

                    if (entList != null)
                    {
                        this.PageState.Add("MessageList", entList);
                    }

                    WebPart[] secs = WebPart.FindAll();
                    PageState.Add("TypeEnum", GetEnumDict(secs));
                    break;
            }

        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect(string ReadState)
        {
            #region 权限
            //ents = MessageInfo.FindAll(SearchCriterion);
            //ents.OrderBy(ent => ent.CreateTime);
            //this.PageState.Add("MessageList", ents);

            string temp = ""; //暂时用不到(根据角色查询权限)
            //DataTable dt = DataHelper.QueryDataTable("select RoleID from SysUserRole where UserID='" + UserInfo.UserID + "'");
            //if (dt.Rows.Count > 0)
            //{
            //    temp = "or RoleId='" + dt.Rows[0]["RoleID"] + "'";
            //}
            string type = ""; //暂时用不到（栏目类型）
            //if (RequestData["lx"] + "" != "")
            //{
            //    type = "and a.BlockTitle='" + RequestData["lx"] + "'";
            //}
            #endregion

            string queryStr = "";

            //处理查询条件
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                queryStr += " and m." + item.PropertyName + " like '%" + item.Value + "%'";
            }

            if (ReadState == "read")
            {
                queryStr += " and charindex('" + UserInfo.UserID + "',convert(varchar(8000),readstate))>0";
            }
            else if (ReadState == "notread")
            {
                queryStr += " and charindex('" + UserInfo.UserID + "',convert(varchar(8000),readstate))=0";
            }


            //过滤权限
            //string sql = @"select m.* from " + ConfigurationManager.AppSettings["ExamineDB"] + "..MessageInfo as m inner join NCRL_AimPortal..WebPart as a on m.TypeId=a.Id " +
            //        "left join " + ConfigurationManager.AppSettings["ExamineDB"] + "..SectionReadRole as b on a.Id=b.SecId " +
            //        "where (RoleId='" + UserInfo.UserID + "' " + temp + ") and b.[type]='read' " + type + queryStr + " order by CreateTime desc";

            string sql = @"select * from " + ConfigurationManager.AppSettings["ExamineDB"] + "..MessageInfo";
            DataTable dtMsg = DataHelper.QueryDataTable(sql);

            this.PageState.Add("MessageList", GetPageData(sql, SearchCriterion));

            //查询系统信息
            if (dtMsg.Rows.Count > 0)
            {
                DataTable dttemp = dtMsg.Clone();
                //dttemp.Rows.Clear();
                foreach (DataRow row in dtMsg.Rows)
                {
                    if (!row["ReadState"].ToString().Contains(UserInfo.UserID) && row["ReleaseState"].ToString() == "1" && row["IsEnforcementUp"].ToString() == "on")
                    {
                        dttemp.Rows.Add(row.ItemArray);
                    }
                }
                this.PageState.Add("msgs", dttemp);
            }
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


        public static EasyDictionary GetEnumDict(WebPart[] enums)
        {
            EasyDictionary dict = new EasyDictionary();

            foreach (WebPart item in enums)
            {
                dict.Set(item.Id, item.BlockTitle);
            }

            return dict;
        }

        #endregion
    }
}
