using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Aim.Examining.Model;
using NHibernate.Criterion;
using Aim.Portal.Model;
using Aim.Data;
using Aim.Examining.Web;
using System.Configuration;
using Aim.WorkFlow;

namespace Aim.Examining.Web
{
    public partial class NoInvoicePaymentInvoiceList : ExamListPage
    {
        string sql = "";
        string id = "";
        PaymentInvoice piEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string ids = RequestData.Get<string>("ids");
            string[] idArray = null;
            if (!string.IsNullOrEmpty(ids))
            {
                idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            id = RequestData.Get<string>("id");
            if (!string.IsNullOrEmpty(id))
            {
                piEnt = PaymentInvoice.Find(id);
            }
            switch (RequestActionString)
            {
                case "CancelCorrespond":
                    foreach (string str in idArray)//可以同时对多个付款进行撤销对应
                    {
                        piEnt = PaymentInvoice.Find(str);
                        string[] invoiceArray = piEnt.CorrespondInvoice.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < invoiceArray.Length; i++)
                        {
                            int index = invoiceArray[i].IndexOf("_");
                            string number = invoiceArray[i].Substring(0, index); //找到对应的发票号和销账金额 对发票进行回滚
                            decimal amount = Convert.ToDecimal(invoiceArray[i].Substring(index + 1));
                            SaleOrder oiEnt = SaleOrder.FindAllByProperty(SaleOrder.Prop_Number, number).FirstOrDefault<SaleOrder>();
                            oiEnt.ReceiptAmount = (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) - amount;
                            oiEnt.PayState = oiEnt.ReceiptAmount > 0 ? "部分付款" : null;
                            oiEnt.DoUpdate();
                        }
                        piEnt.CorrespondInvoice = "";
                        piEnt.CorrespondState = "";
                        piEnt.Name = "暂不销账";
                        piEnt.DoUpdate();
                    }
                    break;
                case "AutoCorrespond":
                    sql = @"select sum(TotalMoney-isnull(DiscountAmount,0)-isnull(ReturnAmount,0)-isnull(ReceiptAmount,0)) 
                    from SHHG_AimExamine..SaleOrders where  CId='" + piEnt.CId + "' and State is null and InvoiceType='收据' and (PayState is null or PayState='部分付款')";
                    decimal TotalArrearage = DataHelper.QueryValue<decimal>(sql);
                    if (TotalArrearage >= piEnt.Money)
                    {
                        sql = @"select * from SHHG_AimExamine..SaleOrders  where InvoiceType='收据' and 
                          CId='" + piEnt.CId + "' and State is null and (PayState is null or PayState='部分付款') order by CreateTime asc";
                        IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                        decimal payamount = piEnt.Money.Value;
                        piEnt.CorrespondInvoice = "";
                        foreach (EasyDictionary dic in dics)
                        {
                            if (payamount > 0)
                            {
                                SaleOrder oiEnt = SaleOrder.Find(dic.Get<string>("Id"));
                                decimal unpayAmount = oiEnt.TotalMoney.Value - (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) -
                                    (oiEnt.DiscountAmount.HasValue ? oiEnt.DiscountAmount.Value : 0) - (oiEnt.ReturnAmount.HasValue ? oiEnt.ReturnAmount.Value : 0);
                                if (payamount >= unpayAmount)
                                {
                                    payamount = payamount - unpayAmount;
                                    oiEnt.ReceiptAmount = (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) + unpayAmount;
                                    oiEnt.PayState = "已全部付款";
                                    piEnt.CorrespondInvoice += oiEnt.Number + "_" + unpayAmount + ",";
                                }
                                else
                                {
                                    oiEnt.ReceiptAmount = (oiEnt.ReceiptAmount.HasValue ? oiEnt.ReceiptAmount.Value : 0) + payamount;
                                    piEnt.CorrespondInvoice += oiEnt.Number + "_" + payamount + ",";
                                    payamount = 0;
                                }
                                oiEnt.DoUpdate();
                            }
                        }
                        piEnt.CorrespondState = "已对应";
                        piEnt.Name = "自动销账";
                        piEnt.DoUpdate();
                        PageState.Add("Result", "T");
                    }
                    else
                    {
                        PageState.Add("Result", "F");
                    }
                    break;
                case "delete":
                    foreach (string str in idArray)
                    {
                        piEnt = PaymentInvoice.Find(str);
                        piEnt.DoDelete();
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = "";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (!string.IsNullOrEmpty(item.Value + ""))
                {
                    switch (item.PropertyName)
                    {
                        case "BeginDate":
                            where += " and ReceivablesTime>'" + item.Value + "' ";
                            break;
                        case "EndDate":
                            where += " and ReceivablesTime<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                            break;
                        default:
                            where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                    }
                }
            }
            sql = @"select * from  SHHG_AimExamine..PaymentInvoice where BillType='收据'" + where;
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " desc" : " asc";
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, sql, (search.CurrentPageIndex - 1) * search.PageSize + 1, search.CurrentPageIndex * search.PageSize);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }
    }
}
