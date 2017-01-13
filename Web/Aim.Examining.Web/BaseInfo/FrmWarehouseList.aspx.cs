﻿using System;
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
    public partial class FrmWarehouseList : ExamListPage
    {
        private IList<Warehouse> ents = null;

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            Warehouse ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Warehouse>();
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
            SearchCriterion.AutoOrder = false;
            ents = Warehouse.FindAll(SearchCriterion);
            this.PageState.Add("WarehouseList", ents);
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
                Warehouse.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}

