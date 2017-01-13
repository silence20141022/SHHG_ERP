using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using Aim.Data;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.Examining.Model;
using System.Data;

namespace Aim.Examining.Web.FinanceManagement
{
    public partial class SaleCorrespondList : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "FinishPay":
                    IList<string> entStrList = RequestData.GetList<string>("data");
                    IList<PayBill> ents = entStrList.Select(tent => JsonHelper.GetObject<PayBill>(tent) as PayBill).ToList();
                    foreach (PayBill pbEnt in ents)
                    {
                        if (!string.IsNullOrEmpty(pbEnt.Id))
                        {
                            PayBill tempEnt = PayBill.Find(pbEnt.Id);//直接更新会丢失字段信息
                            tempEnt.State = "已付款";
                            tempEnt.DoUpdate();
                            UpdatePayState(tempEnt.Id);
                        }
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void UpdatePayState(string payBillId)
        {
            string tempsql = @"select * from SHHG_AimExamine..PayBillDetail where PayBillId='{0}'";
            tempsql = string.Format(tempsql, payBillId);
            IList<EasyDictionary> dicPayDetail = DataHelper.QueryDictList(tempsql);
            PurchaseOrderDetail podEnt = null;
            ArrayList idarray = new ArrayList();//搜集涉及的采购单ID
            string poId = string.Empty;
            string podId = string.Empty;
            int nopay = 0;
            foreach (EasyDictionary ea in dicPayDetail)
            {
                podId = ea.Get<string>("PurchaseOrderDetailId");
                podEnt = PurchaseOrderDetail.TryFind(podId);
                //取得采购详细的实际未付款数目
                nopay = Convert.ToInt32(DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_ActuallNoPayQuan('" + podId + "')"));
                if (nopay == 0)
                {
                    podEnt.PayState = "已付款";
                    podEnt.DoUpdate();
                }
                if (podEnt.PurchaseOrderId != poId)
                {
                    poId = podEnt.PurchaseOrderId;
                    idarray.Add(poId);
                }
            }
            //付款单涉及的采购详细表状态遍历完后。再遍历付款单涉及的采购单
            PurchaseOrder poEnt = null;
            foreach (object ob in idarray)
            {
                IList<PurchaseOrderDetail> podEnts = PurchaseOrderDetail.FindAll("from PurchaseOrderDetail where PayState='未付款' and PurchaseOrderId='" + ob.ToString() + "'");
                if (podEnts.Count == 0)
                {
                    poEnt = PurchaseOrder.TryFind(ob.ToString());
                    poEnt.PayState = "已付款";
                    if (poEnt.InvoiceState == "已关联" && poEnt.InWarehouseState == "已入库")
                    {
                        poEnt.OrderState = "已结束";
                    }
                    poEnt.DoUpdate();
                }
            }
        }
        private void DoSelect()
        {
            string index = RequestData.Get<string>("Index");//0:待付    1：已付
            string where = ""; 
            string sql = string.Empty;
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime>'" + item.Value + "' ";
                }
                else if (item.PropertyName == "EndDate" && item.Value.ToString() != "")
                {
                    where += " and CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                }
                else if (item.Value.ToString() != "")
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                }
            }           
            if (index == "0")
            {
                sql = @"select A.*  from SHHG_AimExamine..OtherPayBill as A
                where  A.PayState='未付款'" + where;
            }
            else
            {
                sql = @"select A.*  from SHHG_AimExamine..OtherPayBill as A               
                where A.PayState='已付款'" + where;
            }
            PageState.Add("OtherPayBillList", GetPageData(sql, SearchCriterion));
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");
            if (idList != null && idList.Count > 0)
            {
                PayBill.DoBatchDelete(idList.ToArray());
                foreach (object obj in idList)
                {
                    PayBillDetail.DeleteAll("PayBillId='" + obj + "'");
                }
            }
        }
        private IList<EasyDictionary> GetPageData(String sql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>("select count(*) from (" + sql + ") t");
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || search.Orders[0].Ascending ? " desc" : " asc";
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

