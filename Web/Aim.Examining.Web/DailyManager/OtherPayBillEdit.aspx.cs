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

namespace Aim.Examining.Web.DailyManager
{
    public partial class OtherPayBillEdit : ExamBasePage
    {

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            OtherPayBill ent = null;
            switch (RequestActionString)
            {
                case "create":
                    ent = GetPostedData<OtherPayBill>();
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = System.DateTime.Now;
                    ent.CreateId = UserInfo.UserID;
                    ent.PayState = "未付款";
                    ent.DoCreate();
                    break;
                case "update":
                    ent = GetMergedData<OtherPayBill>();
                    ent.DoUpdate();
                    break;
                default:
                    break;
            }
            if (op == "c")
            {
                PageState.Add("PayBillNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_GetOtherPayBillNo()"));
            }
            else
            {
                ent = OtherPayBill.Find(id);
                SetFormData(ent);
            }
        }
    }
}
