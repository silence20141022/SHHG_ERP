using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using System.Data;
using Aim.Data;

namespace Aim.Examining.Web.NewFinance
{
    public partial class ManualPayoffInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string id = Request["invoiceid"];
            OrderInvoice oiEnt = null;
            switch (action)
            {
                case "loadform":
                    if (!string.IsNullOrEmpty(id))
                    {
                        oiEnt = OrderInvoice.Find(id); 
                        Response.Write("{data:" + JsonHelper.GetJsonString(oiEnt) + "}");
                        Response.End();
                    }
                    break;
                case "loaddetail":
                    string sql = @"select A.*,B.Number from SHHG_AimExamine..OrderInvoiceDetail A
                                 left join SHHG_AimExamine..SaleOrders B on A.SaleOrderId=B.Id
                                 where OrderInvoiceId='{0}' order by ProductCode asc";
                    sql = string.Format(sql, id);
                    DataTable dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "payoffinvoice":
                    oiEnt = OrderInvoice.Find(id);
                    oiEnt.PayState = "已全部付款";
                    oiEnt.Remark = Request["remark"];
                    oiEnt.DoUpdate();
                    Response.Write("{success:'true'}");
                    Response.End();
                    break;
            }
        }
    }
}