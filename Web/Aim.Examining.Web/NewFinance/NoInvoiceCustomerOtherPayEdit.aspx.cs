using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Aim.Portal.Model;
using Aim.Data;
using System.Data.SqlClient;
using System.Data;


namespace Aim.Examining.Web
{
    public partial class NoInvoiceCustomerOtherPayEdit : ExamListPage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id 
        string sql = "";
        string CId = "";
        PaymentInvoice ent = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            CId = RequestData.Get<string>("CId");
            switch (RequestActionString)
            {
                case "update":
                    ent = this.GetMergedData<PaymentInvoice>();
                    ent.DoUpdate();
                    break;
                case "create":
                    ent = this.GetPostedData<PaymentInvoice>();
                    ent.BillType = "收据";
                    ent.CreateId = UserInfo.UserID;
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = DateTime.Now;
                    ent.Name = "自动销账";
                    ent.CorrespondState = "已对应";
                    ent.DoCreate();
                    ent.CorrespondInvoice = ent.CorrespondInvoice + "_" + ent.Money;
                    ent.DoUpdate();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = PaymentInvoice.Find(id);
                }
            }
            else
            {
                ent = new PaymentInvoice();
            }
            SetFormData(ent);
            PageState.Add("PayType", SysEnumeration.GetEnumDict("PayType"));
        }
    }
}