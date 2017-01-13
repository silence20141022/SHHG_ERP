using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Aim.Portal;
using Aim.Portal.Model;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Aim.Examining.Web;

namespace Aim.Examining.Web
{
    public partial class WarehouseSelect : ExamBasePage
    {
        #region 变量

        private Warehouse[] ents = null;

        #endregion

        #region 构造函数

        public WarehouseSelect()
        {
            IsCheckLogon = false;
        }

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            ents = Warehouse.FindAll(SearchCriterion);

            this.PageState.Add("WhList", ents);
        }

        #endregion
    }
}
