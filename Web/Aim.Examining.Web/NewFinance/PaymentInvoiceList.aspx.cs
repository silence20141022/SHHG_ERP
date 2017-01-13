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
    public partial class PaymentInvoiceList : ExamListPage
    {
        string CC = "";
        string sql = "";
        PaymentInvoice piEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            CC = RequestData.Get<string>("CC");
            string ids = RequestData.Get<string>("ids");
            string[] idArray = null;
            if (!string.IsNullOrEmpty(ids))
            {
                idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
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
                            OrderInvoice oiEnt = OrderInvoice.FindAllByProperty(OrderInvoice.Prop_Number, number).FirstOrDefault<OrderInvoice>();
                            oiEnt.PayAmount = oiEnt.PayAmount - amount;
                            oiEnt.PayState = oiEnt.PayAmount > 0 ? "部分付款" : "";
                            oiEnt.DoUpdate();
                        }
                        piEnt.CorrespondAmount = 0;
                        piEnt.CorrespondInvoice = "";
                        piEnt.CorrespondState = "";
                        piEnt.Name = "暂不销账";
                        piEnt.DoUpdate();
                    }
                    break;
                case "AutoCorrespond":
                    foreach (string id in idArray)
                    {
                        piEnt = PaymentInvoice.Find(id);
                        sql = @"select sum(Amount-isnull(PayAmount,0)) from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and CId='" + piEnt.CId + "'";
                        decimal shouldpayAmount = DataHelper.QueryValue<decimal>(sql);//合计应付金额
                        if (shouldpayAmount > 0)
                        {
                            decimal validAmount = piEnt.Money.Value - (piEnt.CorrespondAmount.HasValue ? piEnt.CorrespondAmount.Value : 0);//有效金额
                            if (shouldpayAmount >= validAmount)//如果 付款金额小于等于应付款总额 
                            {
                                sql = @"select * from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and CId='" + piEnt.CId + "' order by InvoiceDate asc";
                                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                                //decimal payamount = piEnt.Money.Value; 
                                foreach (EasyDictionary dic in dics)
                                {
                                    if (validAmount > 0)
                                    {
                                        OrderInvoice oiEnt = OrderInvoice.Find(dic.Get<string>("Id"));
                                        if (validAmount >= (oiEnt.Amount.Value - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0)))//大于等于该发票的未付金额
                                        {
                                            validAmount = validAmount - (oiEnt.Amount.Value - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0));
                                            oiEnt.PayState = "已全部付款";
                                            piEnt.CorrespondInvoice += (string.IsNullOrEmpty(piEnt.CorrespondInvoice) ? "" : ",") + oiEnt.Number + "_" + (oiEnt.Amount - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0));
                                            oiEnt.PayAmount = oiEnt.Amount;
                                        }
                                        else
                                        {
                                            oiEnt.PayAmount = (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0) + validAmount;
                                            oiEnt.PayState = "部分付款";
                                            piEnt.CorrespondInvoice += (string.IsNullOrEmpty(piEnt.CorrespondInvoice) ? "" : ",") + oiEnt.Number + "_" + validAmount;
                                            validAmount = 0;
                                        }
                                        oiEnt.DoUpdate();
                                    }
                                }
                                piEnt.CorrespondAmount = piEnt.Money;
                                piEnt.CorrespondState = "已对应";
                                piEnt.Name = "自动销账";
                                piEnt.DoUpdate();
                            }
                            else //如果付款金额大于应付款总金额
                            {
                                sql = @"select * from SHHG_AimExamine..OrderInvoice where (PayState is null or PayState<>'已全部付款') and CId='" + piEnt.CId + "' order by InvoiceDate asc";
                                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                                foreach (EasyDictionary dic in dics)
                                {
                                    OrderInvoice oiEnt = OrderInvoice.Find(dic.Get<string>("Id"));
                                    piEnt.CorrespondInvoice += (string.IsNullOrEmpty(piEnt.CorrespondInvoice) ? "" : ",") + oiEnt.Number + "_" + (oiEnt.Amount - (oiEnt.PayAmount.HasValue ? oiEnt.PayAmount.Value : 0));
                                    piEnt.CorrespondState = "部分对应";
                                    piEnt.CorrespondAmount = (piEnt.CorrespondAmount.HasValue ? piEnt.CorrespondAmount.Value : 0) + shouldpayAmount;
                                    piEnt.DoUpdate();
                                    oiEnt.PayState = "已全部付款";
                                    oiEnt.PayAmount = oiEnt.Amount;
                                    oiEnt.DoUpdate();
                                }
                            }
                        }
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
            if (CC != "T")  //将分子公司的销售单分开
            {
                sql = @"select a.*,b.MagUser from  SHHG_AimExamine..PaymentInvoice a
                      left join SHHG_AimExamine..Customers b on a.CId=b.Id where  a.BillType='发票' and
                      b.MagId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' and 
                      a.CId not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')" + where;
            }
            else
            {
                sql = @"select * from  SHHG_AimExamine..PaymentInvoice where BillType='发票' and 
               ((select top 1 MagId from SHHG_AimExamine..Customers c  where c.Id=CId)='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1' or
                Cid in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8'))" + where;
            }
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
