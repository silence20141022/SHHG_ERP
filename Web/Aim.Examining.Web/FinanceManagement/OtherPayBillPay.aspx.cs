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
    public partial class OtherPayBillPay : ExamBasePage
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
                case "update":
                    ent = OtherPayBill.Find(id);
                    string nowPayAmount = RequestData.Get<string>("NowPayAmount");
                    if (!string.IsNullOrEmpty(nowPayAmount))
                    {
                        ent.AcctualPayAmount = (ent.AcctualPayAmount.HasValue ? ent.AcctualPayAmount : 0) + Convert.ToDecimal(nowPayAmount);
                        if (ent.AcctualPayAmount == ent.ShouldPayAmount)
                        {
                            ent.PayState = "已付款";
                            ent.PayTime = System.DateTime.Now;
                            ent.PayUserId = UserInfo.UserID;
                            ent.PayUserName = UserInfo.Name;
                        }
                        ent.DoUpdate();
                    }
                    if (ent.PayType == "物流付款" && ent.PayState == "已付款")
                    {
                        string[] temparray = ent.InterfaceArray.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < temparray.Length; i++)
                        {
                            Logistic lEnt = Logistic.Find(temparray[i]);
                            lEnt.PayState = "已付款";
                            lEnt.DoUpdate();
                        }
                    }
                    break;
                default:
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OtherPayBill.Find(id);
                    SetFormData(ent);
                    PageState.Add("PayType", ent.PayType);
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
