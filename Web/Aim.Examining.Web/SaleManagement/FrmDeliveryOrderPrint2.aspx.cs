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
    public partial class FrmDeliveryOrderPrint2 : ExamListPage //System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DeliveryOrder order = DeliveryOrder.TryFind(Request.QueryString["Id"]);
            int pageindex = Request.QueryString["pageindex"] + "" == "" ? 1 : Convert.ToInt32(Request.QueryString["pageindex"]);
            if (order == null)
            {
                return;
            }
            lblbianhao.InnerText = order.Number;
            lblCustomerOrderNo.InnerText = "";
            lbldizhi.InnerText = order.Address;
            lblfphm.InnerText = "";
            lblghdw.InnerText = order.CName;
            lbljhfs.InnerText = order.DeliveryMode;
            SaleOrder saleOrder = SaleOrder.TryFind(order.PId + "");
            lblkdr.InnerText = saleOrder != null ? saleOrder.CreateName : "";
            lbllxdh.InnerText = order.Tel;
            lblriqi.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            lblxsjl.InnerText = "";
            lblywy.InnerText = order.Salesman;
            lblzhaiyao.InnerText = order.Remark;
            //循环子商品
            DelieryOrderPart[] deOrderParts = DelieryOrderPart.FindAllByProperty("DId", order.Id);
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            int pagecount = DataHelper.QueryValue<int>("select top 1 Count from " + db + "..PrintCount");
            for (int i = 0; i < deOrderParts.Length; i++)
            {
                if (i < (pageindex - 1) * pagecount)
                    continue;
                if (i >= pageindex * pagecount)
                    break;

                lit.Text += "<tr align='center'><td>" + deOrderParts[i].PName + "</td>";
                lit.Text += "<td>" + deOrderParts[i].PCode + "</td>";
                lit.Text += "<td>" + deOrderParts[i].Unit + "</td>";
                lit.Text += "<td>" + deOrderParts[i].Count + "</td>";
                lit.Text += "<td></td>";
                lit.Text += "<td></td>";
                lit.Text += "<td>" + deOrderParts[i].Remark + "</td>";
                lit.Text += "<td>" + DateTime.Now.ToString("yyyy-MM-dd") + "</td></tr>";

                //count += deOrderParts[i].Count;
                //amount += deOrderParts[i].Count
            }
        }

        //把json转换为可识别的
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
        }
    }
}