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
using Castle.ActiveRecord;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Data.OleDb;
using System.Data;
namespace Aim.Examining.Web.StockManagement
{
    public partial class WarehouseExchangeView : ExamBasePage
    {

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型        
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            WarehouseExchange ent = null;
            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = WarehouseExchange.Find(id);
                    SetFormData(ent);
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                WarehouseExchangeDetail[] wedEnts = WarehouseExchangeDetail.FindAllByProperties("WarehouseExchangeId", id);
                PageState.Add("DataList", wedEnts);
            }
        }
    }
}

