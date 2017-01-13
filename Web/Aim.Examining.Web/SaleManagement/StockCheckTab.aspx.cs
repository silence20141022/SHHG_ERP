﻿using System;
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

namespace Aim.Examining.Web.SaleManagement
{
    public partial class StockCheckTab : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DoSelect();

        }        
        private void DoSelect()
        {
            string[] tabs = { "未结束的盘点单", "已结束的盘点单" };
            PageState.Add("Tabs", tabs);
        }      
    }
}
