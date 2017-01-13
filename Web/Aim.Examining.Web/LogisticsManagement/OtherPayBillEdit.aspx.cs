using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Telerik.Web.UI;
using System.Data;
using Aim.Examining.Model;
using Newtonsoft.Json.Linq;
using Aim.Portal.Web;

namespace Aim.Examining.Web.LogisticsManagement
{
    public partial class OtherPayBillEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string logisticsIds = Request.QueryString["LogisticsIds"];
                string totalAmount = Request.QueryString["Total"];
                string logisticsCompanyName = Request.QueryString["LogisticsCompanyName"];
                rtbInterfaceArray.Text = logisticsIds;
                rntbShouldPayAmount.Text = totalAmount;
                rtbLogisticsCompanyName.Text = logisticsCompanyName;
                rtbPayBillNo.Text = DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_GetOtherPayBillNo()").ToString();
            }
        }
        protected void lbtSave_Click(object sender, EventArgs e)
        {
            OtherPayBill opbEnt = new OtherPayBill();
            opbEnt.PayBillNo = rtbPayBillNo.Text;
            opbEnt.PayType = rcbPayType.SelectedValue;
            opbEnt.LogisticsCompanyName = rtbLogisticsCompanyName.Text;
            opbEnt.InvoiceNo = rtbInvoiceNo.Text;
            opbEnt.InvoiceAmount = Convert.ToDecimal(rntbInvoiceAmount.Text);
            opbEnt.InterfaceArray = rtbInterfaceArray.Text;
            opbEnt.ShouldPayAmount = Convert.ToDecimal(rntbShouldPayAmount.Text);
            opbEnt.Remark = rtbRemark.Text;
            opbEnt.PayState = "未付款";
            opbEnt.CreateTime = System.DateTime.Now;
            opbEnt.CreateId = WebPortalService.CurrentUserInfo.UserID;
            opbEnt.CreateName = WebPortalService.CurrentUserInfo.Name;
            opbEnt.DoCreate();
            //付款单创建完毕后将关联的物流单的付款状态改为  已提交
            string[] temparray = opbEnt.InterfaceArray.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < temparray.Length; i++)
            {
                Logistic lEnt = Logistic.Find(temparray[i]);
                lEnt.PayState = "已提交";
                lEnt.DoUpdate();
            }
            ClientScript.RegisterStartupScript(GetType(), "submit", "window.opener.location.reload();window.close();", true);
            lbtSave.Visible = false;
        }
    }
}
