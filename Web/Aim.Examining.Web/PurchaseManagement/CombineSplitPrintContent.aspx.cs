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

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class CombineSplitPrintContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CombineSplit csEnt = CombineSplit.Find(Request.QueryString["Id"]);
            lbCombineSplitNo.InnerText = csEnt.CombineSplitNo;
            lbOperateType.InnerText = csEnt.OperateType;
            lbProductName.InnerText = csEnt.ProductName;
            lbProductPcn.InnerText = csEnt.ProductPcn;
            lbWarehouseName.InnerText = csEnt.WarehouseName;
            lbStockQuantity.InnerText = csEnt.StockQuantity.ToString();
            lbProductQuantity.InnerText = csEnt.ProductQuantity.ToString();
            lbProductCode.InnerText = csEnt.ProductCode;
            lbRemark.InnerText = csEnt.Remark;
            lbCreateName.InnerText = csEnt.CreateName;
            lbCreateTime.InnerText = csEnt.CreateTime.ToString();
            CombineSplitDetail[] csdEnts = CombineSplitDetail.FindAllByProperty("CombineSplitId", csEnt.Id);
            foreach (CombineSplitDetail csdEnt in csdEnts)
            {
                lit.Text += "<tr align='center'><td>" + csdEnt.ProductName + "</td>";
                lit.Text += "<td>" + csdEnt.ProductCode + "</td>";
                lit.Text += "<td>" + csdEnt.ProductPcn + "</td>";
                lit.Text += "<td>" + csdEnt.StockQuantity + "</td>";
                lit.Text += "<td>" + csdEnt.ProductQuantity + "</td>";
            }
        }
    }
}