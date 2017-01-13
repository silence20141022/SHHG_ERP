using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using System.Data;
using Aim.Examining.Model;
using Newtonsoft.Json.Linq;

namespace Aim.Examining.Web.NewFinance
{
    public partial class CustomerPay_UnInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string sql = "";
            DataTable dt = null;
            PaymentInvoice piEnt = null;
             SaleOrder soEnt=null;
            switch (action)
            {
                case "loadpaytype":
                    sql = @"select name from SysEnumeration where parentid=(select TOP 1 EnumerationID from SysEnumeration WHERE Code = 'PayType') order by sortindex asc ";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loadbyorderid": 
                      soEnt = SaleOrder.Find(Request["orderid"]); 
                    Response.Write("{data:" + JsonHelper.GetJsonString(soEnt) + "}");
                    Response.End();
                    break;
                case "create":
                    soEnt = SaleOrder.Find(Request["orderid"]);
                    piEnt = JsonHelper.GetObject<PaymentInvoice>(Request["formdata"]);
                    piEnt.BillType = "订单";
                    piEnt.CorrespondState = "已对应";
                    piEnt.CollectionType = "销售收款";
                    piEnt.Name = "手动销账";
                    piEnt.CorrespondInvoice =soEnt.Number + "_" + soEnt.TotalMoney;
                    piEnt.DoCreate();
                    //创建完付款单后，更新订单状态
                    soEnt.PayState = "已付款";
                    soEnt.DoUpdate();
                    break;
            }
        }
    }
}