using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Portal.Model;
using System.Data;
using Aim.Data;
using Aim.Examining.Model;
using System.Configuration;

namespace Aim.Examining.Web.SaleManagement
{
    public partial class FenGongSi_Order_Monitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string sql = "";
            switch (action)
            {
                case "loaddetail":
                    if (!string.IsNullOrEmpty(Request["orderpartid"]))
                    {
                        sql = @"select a.Id,a.ProductId,a.OrderPart_Id,a.Name,a.Code,a.PurchasePrice,a.SecondPrice,
                        a.Quantity,a.Amount,a.Remark,b.Number,b.CustomerName,b.ShouKuanAmount,b.CreateTime,c.Count
                        from SHHG_AimExamine..Orderdetail_FenGongSi a 
                        left join SHHG_AimExamine..Order_FenGongSi b on a.Order_FenGongSi_Id=b.Id 
                        left join SHHG_AimExamine..OrdersPart c on c.Id=a.OrderPart_Id
                        where a.OrderPart_Id='" + Request["orderpartid"] + "' order by b.CreateTime asc";

                    }
                    if (!string.IsNullOrEmpty(Request["saleorderid"]))
                    {
                        sql = @"select a.Id,a.ProductId,a.OrderPart_Id,a.Name,a.Code,a.PurchasePrice,a.SecondPrice,
                        a.Quantity,a.Amount,a.Remark,b.Number,b.CustomerName,b.ShouKuanAmount,b.CreateTime,c.Count
                        from SHHG_AimExamine..Orderdetail_FenGongSi a 
                        left join SHHG_AimExamine..Order_FenGongSi b on a.Order_FenGongSi_Id=b.Id 
                        left join SHHG_AimExamine..OrdersPart c on c.Id=a.OrderPart_Id where c.OId='" + Request["saleorderid"] + "' order by b.CreateTime asc";
                    }
                    DataTable dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
            }
        }
    }
}