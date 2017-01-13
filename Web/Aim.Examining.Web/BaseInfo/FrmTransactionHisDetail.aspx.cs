using System;
using System.Collections;
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

namespace Aim.Examining.Web
{
    public partial class FrmTransactionHisDetail : ExamBasePage
    {
        #region 变量

        string id = String.Empty;   // 对象id

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");
            TransactionHi ent = null;

            if (!String.IsNullOrEmpty(id))
            {
                ent = TransactionHi.Find(id);
            }

            this.SetFormData(ent);
        }

        #endregion
    }
}

