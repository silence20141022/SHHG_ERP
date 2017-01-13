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

namespace Aim.Examining.Web.BaseInfo
{
    public partial class FrmTransactionHis : ExamListPage
    {
        private IList<TransactionHi> ents = null;

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            TransactionHi ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<TransactionHi>();
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
            if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
                SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));
            SearchCriterion.AddSearch("CId", Request.QueryString["id"], SearchModeEnum.Equal);
            ents = TransactionHi.FindAll(SearchCriterion);
            this.PageState.Add("TransactionHisList", ents);
        }

        #endregion
    }
}

