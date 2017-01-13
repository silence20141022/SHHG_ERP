using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using System.Configuration;
using Aim.Data;

namespace Aim.Examining.Web.FinanceManagement
{
    public partial class OtherPayBillView : ExamBasePage
    {

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            OtherPayBill ent = null;
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OtherPayBill.Find(id);
                    PageState.Add("PayType", ent.PayType);
                    SetFormData(ent);
                    if (ent.PayType == "物流付款")
                    {
                        string[] array = ent.InterfaceArray.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        PageState.Add("DataList", Logistic.FindAllByPrimaryKeys(array));
                    }
                }
            }
        }
    }
}
