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
using System.Web.Script.Serialization;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Aim.Examining.Web
{
    public partial class SaleOrderInvoiceList : ExamListPage
    {
        string Index = "";
        string sql = "";
        string CC = "";
        OrderInvoice oiEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Index = RequestData.Get<string>("Index");
            CC = RequestData.Get<string>("CC");
            string ids = RequestData.Get<string>("ids");
            switch (RequestActionString)
            {
                case "delete":
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            oiEnt = OrderInvoice.Find(idArray[i]);
                            IList<OrderInvoiceDetail> oidEnts = OrderInvoiceDetail.FindAllByProperty("OrderInvoiceId", oiEnt.Id);
                            foreach (OrderInvoiceDetail oidEnt in oidEnts)
                            {
                                OrdersPart opEnt = OrdersPart.Find(oidEnt.OrderDetailId);
                                opEnt.BillingCount = opEnt.BillingCount - oidEnt.InvoiceCount;
                                opEnt.DoUpdate();
                                oidEnt.DoDelete();
                            }
                            //删除发票的时候 回滚销售订单的发票状态   soEnt.InvoiceState = "";
                            string[] oidArray = oiEnt.OId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string oid in oidArray)
                            {
                                SaleOrder soEnt = SaleOrder.Find(oid);
                                sql = "select count(Id) from SHHG_AimExamine..OrdersPart where BillingCount>0 and OId='" + oid + "'";
                                if (DataHelper.QueryValue<int>(sql) == 0)
                                {
                                    soEnt.InvoiceState = "";
                                }
                                else
                                {
                                    soEnt.InvoiceState = "已部分开发票";
                                }
                                soEnt.DoUpdate();
                            }
                            oiEnt.DoDelete();
                        }
                    }
                    break;
                case "RollBack":
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string[] idArray = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < idArray.Length; i++)
                        {
                            oiEnt = OrderInvoice.Find(idArray[i]);
                            oiEnt.PayState = null;
                            oiEnt.PayAmount = null;
                            oiEnt.DoUpdate();
                        }
                    }
                    break;
                case "updateorderinvoicedetail":
                    IList<OrderInvoice> oiEnts = OrderInvoice.FindAll();
                    foreach (OrderInvoice oiEnt in oiEnts)
                    {
                        IList<OrderInvoiceDetail> oidEnts = OrderInvoiceDetail.FindAllByProperty(OrderInvoiceDetail.Prop_OrderInvoiceId, oiEnt.Id);
                        if (oidEnts.Count == 0)
                        {
                            JArray ja = JsonHelper.GetObject<JArray>(oiEnt.Child);
                            foreach (JObject jo in ja)
                            {
                                OrderInvoiceDetail oidEnt = new OrderInvoiceDetail();
                                oidEnt.OrderDetailId = jo.Value<string>("Id");
                                oidEnt.OrderInvoiceId = oiEnt.Id;
                                oidEnt.SaleOrderId = jo.Value<string>("OId");
                                oidEnt.ProductId = jo.Value<string>("ProductId");
                                oidEnt.ProductCode = jo.Value<string>("ProductCode");
                                oidEnt.ProductName = jo.Value<string>("ProductName");
                                oidEnt.SalePrice = jo.Value<decimal>("SalePrice");
                                oidEnt.Unit = jo.Value<string>("Unit");
                                oidEnt.Amount = jo.Value<decimal>("Amount");
                                oidEnt.Count = jo.Value<Int32>("Count");
                                oidEnt.InvoiceCount = jo.Value<Int32>("Count");
                                oidEnt.DoCreate();
                            }
                        }
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
                            where += " and A.CreateTime>'" + item.Value + "' ";
                            break;
                        case "EndDate":
                            where += " and A.CreateTime<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                            break;
                        case "MagUser":
                            where += " and B." + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                        default:
                            where += " and A." + item.PropertyName + " like '%" + item.Value + "%' ";
                            break;
                    }
                }
            }
            switch (Index)//为了让此页面可以为财务和销售所共用所以用swith  销售进来index为空
            {
                case "1"://应收款
                    if (CC != "T")
                    {
                        sql = @"select A.*,B.MagId,B.MagUser from SHHG_AimExamine..OrderInvoice A                    
                    left join SHHG_AimExamine..Customers B on B.Id=A.CId
                    where (A.PayState is null or A.PayState<>'已全部付款') and B.MagId !='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                    and A.Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8') " + where;
                    }
                    else
                    {
                        sql = @"select A.*,B.MagId,B.MagUser from SHHG_AimExamine..OrderInvoice A
                    left join SHHG_AimExamine..Customers B on B.Id=A.CId
                    where (A.PayState is null or A.PayState<>'已全部付款') and 
                    (B.MagId='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'
                    or A.Cid in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8')) " + where;
                    }
                    break;
                case "2"://已付款
                    if (CC != "T")
                    {
                        sql = @"select A.*,(A.Amount-A.PayAmount) BadDetb,B.MagId,B.MagUser from SHHG_AimExamine..OrderInvoice A
                        left join SHHG_AimExamine..Customers B on B.Id=A.CId
                        where  A.PayState='已全部付款' and B.MagId!='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                        and A.Cid not in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8') " + where;
                    }
                    else
                    {
                        sql = @"select A.*,B.MagId,B.MagUser from SHHG_AimExamine..OrderInvoice A
                        left join SHHG_AimExamine..Customers B on B.Id=A.CId
                        where A.PayState='已全部付款'  and (B.MagId='56bb4d2f-8a6e-47e2-9d2c-dbbb942704a1'                   
                        or A.Cid in ('b1b1e57e-1e6e-4d75-a631-089370041d5b','c241fa9e-813d-47cc-9267-2f26330fa957','db2b0081-53dc-41f3-81e7-893c0c6333d8'))" + where;
                    }
                    break;
                default:
                    sql = @"select A.*,B.MagId,B.MagUser from SHHG_AimExamine..OrderInvoice A                    
                    left join SHHG_AimExamine..Customers B on B.Id=A.CId
                    where 1=1 " + where;
                    break;
            }
            PageState.Add("DataList", GetPageData(sql, SearchCriterion));
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " asc" : " desc";
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
