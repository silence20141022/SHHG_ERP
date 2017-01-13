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

namespace Aim.Examining.Web.LogisticsManagement
{
    public partial class LogisticsPayEdit : ExamBasePage
    {

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");

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
                    string[] temparray = ent.InterfaceArray.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < temparray.Length; i++)
                    {
                        Logistic lEnt = Logistic.Find(temparray[i]);
                        lEnt.PayState = "已提交";
                        lEnt.DoUpdate();
                    }
                    break;
                default:
                    break;
            }
            PageState.Add("PayBillNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_GetOtherPayBillNo()"));
        }
    }
}
