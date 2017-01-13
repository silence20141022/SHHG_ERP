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

namespace Aim.Examining.Web.SaleManagement
{
    public partial class SaleOrderView : ExamListPage
    {
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string saleorderId = RequestData.Get<string>("id");
                //基本信息
                SaleOrder soEntity = SaleOrder.Find(saleorderId);
                lbSaleOrderNo.Text = soEntity.Number;
                lbCustomerName.Text = soEntity.CName;
                lbInvoiceType.Text = soEntity.InvoiceType;
                lbTotalMoney.Text = soEntity.TotalMoney.ToString();
                lbSalesman.Text = soEntity.Salesman;
                lbCreateName.Text = soEntity.CreateName;
                lbPayType.Text = soEntity.PayType;
                lbDeliveryMode.Text = soEntity.DeliveryMode;
                lbCreateTime.Text = soEntity.CreateTime.ToString();
                lbRemark.Text = soEntity.Remark;

                lbReturnAmount.Text = soEntity.ReturnAmount + "";
                lbDeState.Text = soEntity.DeState;
                lbInvoiceState.Text = soEntity.InvoiceState + "";
                //销售详细信息
                sql = @"select *,B.Number from SHHG_AimExamine..OrdersPart as A
                left join SHHG_AimExamine..SaleOrders as B on A.OId=B.Id
                where A.OId='" + saleorderId + "'";
                rgSaleOrderDetail.DataSource = DataHelper.QueryDataTable(sql);
                rgSaleOrderDetail.DataBind();
                //出库详细
                sql = @"select A.*,D.Number from SHHG_AimExamine..DelieryOrderPart as A 
                left join SHHG_AimExamine..DeliveryOrder as D on D.Id=A.DId
                left join SHHG_AimExamine..OrdersPart as B on A.PId=B.Id
                left join SHHG_AimExamine..SaleOrders as C on B.OId=C.Id where C.Id='" + saleorderId + "'";
                rgDeliveryOrderDetail.DataSource = DataHelper.QueryDataTable(sql);
                rgDeliveryOrderDetail.DataBind();
                sql = @"select A.* from SHHG_AimExamine..PaymentInvoice as A 
                where PATINDEX('%" + soEntity.Number + "%', A.CorrespondInvoice) >0";
                rgReceiveMoney.DataSource = DataHelper.QueryDataTable(sql);
                rgReceiveMoney.DataBind();
                //发票明细
                sql = @"select * from SHHG_AimExamine..OrderInvoice where OId like '%" + saleorderId + "%'";
                rgSaleInvoice.DataSource = DataHelper.QueryDataTable(sql);
                rgSaleInvoice.DataBind();
                //退货明细
                sql = @"select A.*,B.Number from SHHG_AimExamine..ReturnOrderPart A 
                      left join SHHG_AimExamine..ReturnOrder as B on B.Id=A.ReturnOrderId
                      where B.OrderNumber='" + soEntity.Number + "'";
                rgReturnOrder.DataSource = DataHelper.QueryDataTable(sql);
                rgReturnOrder.DataBind();
                //物流信息
                string sql5 = @"select * from SHHG_AimExamine..Logistics where DeliveryId in (select Id from SHHG_AimExamine..DeliveryOrder where Pid='" + saleorderId + "')";
                rgLogistics.DataSource = DataHelper.QueryDataTable(sql5);
                rgLogistics.DataBind();
            }
        }
    }
}
