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

namespace Aim.Examining.Web.CommonPages.Select
{
    public partial class InAndOutInWarehouseSelect : ExamListPage
    {
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
            IList<InWarehouse> iwEnts = InWarehouse.FindAll("from InWarehouse where State='未入库'");
            PageState.Add("InWarehouseList", iwEnts);
        }

    }
}

