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
    public partial class SupplierIframe : ExamListPage
    {
        #region 变量

        private IList<Supplier> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
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
            ents = Supplier.FindAll(SearchCriterion);
            this.PageState.Add("SupplierList", ents);
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
                Supplier.DoBatchDelete(idList.ToArray());
            }
        }
        #endregion
    }
}

