using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Aim.Portal;
using Aim.Portal.Model;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using System.Data.OleDb;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class SaleOrderInvoiceEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string orderids = String.Empty;
        string id = String.Empty;   // 对象id  
        string sql = "";
        OrderInvoice ent = null;
        string CustomerId = "";
        IList<string> entStrList = null;
        string CustomerName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            orderids = RequestData.Get<string>("orderids");
            id = RequestData.Get<string>("id");
            CustomerId = RequestData.Get<string>("CustomerId");
            CustomerName = Server.HtmlDecode(RequestData.Get<string>("CustomerName"));
            entStrList = RequestData.GetList<string>("data");
            if (!string.IsNullOrEmpty(id))
            {
                ent = OrderInvoice.Find(id);
            }
            switch (RequestActionString)
            {
                case "update":
                    ent = GetMergedData<OrderInvoice>();
                    ent.DoUpdate();
                    ProcessDetail();
                    break;
                case "JudgeRepeat":
                    sql = "select count(1) from SHHG_AimExamine..OrderInvoice where Number like '%" + RequestData.Get<string>("InvoiceNo") + "%' ";
                    if (DataHelper.QueryValue<int>(sql) > 0)
                    {
                        PageState.Add("IsRepeat", "T");
                    }
                    else
                    {
                        PageState.Add("IsRepeat", "F");
                    }
                    break;
                case "create":
                    ent = GetPostedData<OrderInvoice>();
                    ent.OId = orderids;
                    ent.DoCreate();
                    ProcessDetail();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            if (op == "c")
            {
                sql = @"select A.Id as OrderDetailId,A.OId as SaleOrderId,A.PId as ProductId,A.Unit,
                A.PName as ProductName,A.PCode as ProductCode,A.Count,A.SalePrice,B.Number,
                isnull(BillingCount,0) as HaveInvoiceCount,(Count-isnull(BillingCount,0)-isnull(ReturnCount,0)) as InvoiceCount,
                isnull(ReturnCount,0) ReturnCount,(Count-isnull(BillingCount,0)-isnull(ReturnCount,0))*SalePrice as NowAmount
                from SHHG_AimExamine..OrdersPart A
                left join SHHG_AimExamine..SaleOrders B on A.OId=B.Id
                where PatIndex('%'+OId+'%','{0}')> 0 and Count-isnull(ReturnCount,0) > isnull(BillingCount,0) ";
                //NowAmount 表示本次开票金额
                sql = string.Format(sql, orderids);
                ent = new OrderInvoice();
                ent.CId = CustomerId;
                ent.CName = CustomerName;
                //有可能对多个销售单一起开发票 多个销售单可能各自有折扣金额 有可能一个销售单会分多次开票。但折扣金额只在第一次开发票的时候带到开票界面
                //string tempsql = "select sum(isnull(DiscountAmount,0)) from SHHG_AimExamine..SaleOrders where PatIndex('%'+Id+'%','" + orderids + "')>0 and  InvoiceState = ''";
                //ent.DiscountAmount = DataHelper.QueryValue<decimal>(tempsql);
                //tempsql = "select sum(isnull(ReturnAmount,0)) from SHHG_AimExamine..SaleOrders where PatIndex('%'+Id+'%','" + orderids + "')>0 and  InvoiceState = ''";
                //ent.ReturnAmount = DataHelper.QueryValue<decimal>(tempsql);不需要减退货金额 因为明细显示的时候数量上已经减掉了退货数量
            }
            else
            {
                sql = @"select A.*,B.Number from SHHG_AimExamine..OrderInvoiceDetail A
                left join SHHG_AimExamine..SaleOrders B on A.SaleOrderId=B.Id
                where OrderInvoiceId='{0}' order by ProductCode asc";
                sql = string.Format(sql, id);
            }
            PageState.Add("DataList", DataHelper.QueryDictList(sql));
            SetFormData(ent);
        }
        private void ProcessDetail()
        {
            IList<OrderInvoiceDetail> oidEnts = OrderInvoiceDetail.FindAllByProperty("OrderInvoiceId", ent.Id);
            foreach (OrderInvoiceDetail oidEnt in oidEnts)
            {
                oidEnt.DoDelete();
            }
            oidEnts = entStrList.Select(tent => JsonHelper.GetObject<OrderInvoiceDetail>(tent) as OrderInvoiceDetail).ToList();
            foreach (OrderInvoiceDetail oidEnt in oidEnts)
            {
                oidEnt.OrderInvoiceId = ent.Id;
                oidEnt.DoCreate();
                OrdersPart opEnt = OrdersPart.Find(oidEnt.OrderDetailId);
                opEnt.BillingCount = (opEnt.BillingCount.HasValue ? opEnt.BillingCount : 0) + oidEnt.InvoiceCount;//顺便更新订单明细的开票数量
                opEnt.DoUpdate();
            }
            string[] orderIdArray = orderids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in orderIdArray)//更新涉及订单的开票状态
            {
                sql = @"select count(Id) from SHHG_AimExamine..OrdersPart where OId='" + str + "' and Count-isnull(ReturnCount,0)<>isnull(BillingCount,0)";
                SaleOrder soEnt = SaleOrder.Find(str);
                if (DataHelper.QueryValue<int>(sql) == 0)
                {
                    soEnt.InvoiceState = "已全部开发票";
                }
                else
                {
                    soEnt.InvoiceState = "已部分开发票";
                }
                soEnt.DoUpdate();
            }
        }
    }
}

