using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Portal.Web.UI;
using Aim.Data;
using Aim.Examining.Model;
using Castle.ActiveRecord;
using Aim.Portal.Model;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Aim.Examining.Web.StockManagement
{
    public partial class InWarehouseTab : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DoSelect();

        }
        #region 私有方法
        private void DoSelect()
        {
            string[] tabs = { "待入库", "已入库" };
            PageState.Add("Tabs", tabs);
        }
        #endregion
    }
}
