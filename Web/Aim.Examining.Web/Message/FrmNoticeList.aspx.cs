using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Portal.Web.UI;
using System.Data.SqlClient;
using Aim.Data;
using System.Configuration;

namespace Aim.Examining.Web.Message
{
    public partial class FrmNoticeList : ExamListPage
    {
        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string op = RequestData.Get<string>("op");
            Notice ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Notice>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                default:
                    if (RequestActionString == "batchdelete")
                    {
                        DoBatchDelete();
                    }
                    else
                    {
                        DoSelect();
                    }
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
            //处理查询条件
            string queryStr = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                queryStr += " and " + item.PropertyName + " like '%" + item.Value + "%'";
            }

            string sql = "select * from " + ConfigurationManager.AppSettings["ExamineDB"] + "..Notices where userid='" + UserInfo.UserID + "'" + queryStr + " order by createtime desc";
            //SqlConnection con = new SqlConnection(@"server=WIN-Q9JF4JM4J69\SQLSERVER2005;database=NCRL_AimExamine;uid=sa;pwd=sasa;");
            //con.Open();
            //IList<EasyDictionary> entList = DataHelper.DataTableToDictList(DataHelper.QueryDataTable(sql, con));
            IList<EasyDictionary> entList = DataHelper.DataTableToDictList(DataHelper.QueryDataTable(sql));
            this.PageState.Add("NoticesList", entList);
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
                Notice.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}
