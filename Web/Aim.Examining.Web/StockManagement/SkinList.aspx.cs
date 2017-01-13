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

namespace Aim.Examining.Web
{
    public partial class SkinList : ExamListPage
    {
        string skinNo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "JudgeSkinIsExist":
                    skinNo = RequestData.Get<string>("SkinNo");
                    if (!string.IsNullOrEmpty(skinNo))
                    {
                        IList<Skin> skinEnts = Skin.FindAll("from Skin where SkinNo='" + skinNo + "'");
                        if (skinEnts.Count > 0)
                        {
                            PageState.Add("SkinIsExist", true);
                        }
                    }
                    break;
                case "batchsave":
                    IList<string> entStrList = RequestData.GetList<string>("data");
                    if (entStrList != null && entStrList.Count > 0)
                    {
                        IList<Skin> skinEnts = entStrList.Select(tent => JsonHelper.GetObject<Skin>(tent) as Skin).ToList();
                        foreach (Skin ent in skinEnts)
                        {
                            if (ent != null)
                            {
                                Skin sent = ent;
                                sent.SkinNo = sent.SkinNo.ToUpper();//将包装编号中的字母转换成大写
                                if (String.IsNullOrEmpty(sent.Id))
                                {
                                    sent.CreateId = UserInfo.UserID;
                                    sent.CreateName = UserInfo.Name;
                                    sent.CreateTime = System.DateTime.Now;
                                }
                                else
                                {
                                    sent = DataHelper.MergeData(Skin.Find(sent.Id), sent);
                                }
                                sent.DoSave();
                                // PageState.Add("SkinId", sent.Id);
                            }
                        }
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string state = RequestData.Get<string>("State");
            string sql = string.Empty;
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
            }
            if (string.IsNullOrEmpty(state))
            {
                sql = @"select Id ,UPPER(SkinNo) as SkinNo,UPPER(ModelNo) as ModelNo,Quantity,CreateTime,CreateId,CreateName,InWarehouseId,SkinState
                from SHHG_AimExamine..Skin where SkinState !='满箱' " + where;
            }
            else
            {
                switch (state)
                {
                    case "已满":
                        sql = @"select Id ,UPPER(SkinNo) as SkinNo ,UPPER(ModelNo) as ModelNo,Quantity,CreateTime,CreateId,CreateName,InWarehouseId,SkinState
                        from SHHG_AimExamine..Skin where SkinState='满箱'" + where;
                        break;
                    case "未满":
                        sql = @"select  Id ,UPPER(SkinNo) as SkinNo,UPPER(ModelNo) as ModelNo,Quantity,CreateTime,CreateId,CreateName,InWarehouseId,SkinState
                        from SHHG_AimExamine..Skin where SkinState !='满箱'" + where;
                        break;
                    default:
                        sql = @"select  Id ,UPPER(SkinNo) as SkinNo,UPPER(ModelNo) as ModelNo,Quantity,CreateTime,CreateId,CreateName,InWarehouseId,SkinState 
                        from SHHG_AimExamine..Skin where 1=1" + where;
                        break;
                }
            }
            this.PageState.Add("SkinList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "SkinState";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? "asc" : "desc";
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
