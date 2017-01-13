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

namespace Aim.Examining.Web
{
    public partial class NoNeedInvoiceTab : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoSelect();//"收款记录", "销售对账", "欠款报表"
        }
        private void DoSelect()
        {
            string[] tabs = { "待收款", "已收款", "收款记录" ,"销售对账单"};
            PageState.Add("Tabs", tabs);
        }
    }
}
