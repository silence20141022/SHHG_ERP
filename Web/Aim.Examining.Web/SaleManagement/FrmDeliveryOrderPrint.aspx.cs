using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Aim.Examining.Model;
using Aim.Data;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmDeliveryOrderPrint : ExamListPage //System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DeliveryOrder order = DeliveryOrder.TryFind(Request.QueryString["Id"]);
            int pageindex = Request.QueryString["pageindex"] + "" == "" ? 1 : Convert.ToInt32(Request.QueryString["pageindex"]);
            if (order == null)
                return;

            //循环子商品
            string strjosn = order.Child.Substring(1, order.Child.Length - 2);
            string[] objarr = strjosn.Replace("},{", "#").Split('#');

            //计算pagecount
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            int pgcount = DataHelper.QueryValue<int>("select top 1 Count from " + db + "..PrintCount");
            int pagecount = 0;
            if (pgcount != 0)
            {
                pagecount = (objarr.Length % pgcount) == 0 ? objarr.Length / pgcount : (objarr.Length / pgcount) + 1;
            }
            lblpagecount.InnerText = pagecount == 0 ? 1 + "" : pagecount + "";

        }
    }
}