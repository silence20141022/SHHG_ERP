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

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseInvoiceList : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    if (RequestData.Get<string>("optype") == "getChildData")
                    {
                        string purchaseInvoiceId = RequestData.Get<string>("PurchaseInvoiceId");
                        IList<PurchaseInvoiceDetail> pidEnts = PurchaseInvoiceDetail.FindAllByProperties("PurchaseInvoiceId", purchaseInvoiceId);
                        PageState.Add("DetailList", pidEnts);
                    }
                    else
                    {
                        DoSelect();
                    }
                    break;
            }
        }
        private void DoSelect()
        {
            string where = "";
            string detailwhere = "";
            string state = RequestData.Get<string>("State");
            if (!string.IsNullOrEmpty(state))
            {
                SearchCriterion.AddSearch("State", state);
            }
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
                    if (item.PropertyName != "ProductCode")
                    {
                        where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                    else
                    {
                        detailwhere = "where " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                }
            }
            string sql = @"select A.*,B.MoneyType,B.Symbo  from SHHG_AimExamine..PurchaseInvoice as A 
            left join (select Id ,MoneyType,Symbo from SHHG_AimExamine..Supplier) as B on A.SupplierId=B.Id        
            where  A.Id in (select PurchaseInvoiceId from SHHG_AimExamine..PurchaseInvoiceDetail " + detailwhere + ") " + where;
            this.PageState.Add("PurchaseInvoiceList", GetPageData(sql, SearchCriterion));
        }
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");
            if (idList != null && idList.Count > 0)
            {
                foreach (object obj in idList)
                {
                    IList<PurchaseInvoiceDetail> pidEnts = PurchaseInvoiceDetail.FindAll("from PurchaseInvoiceDetail where PurchaseInvoiceId='" + obj.ToString() + "'");
                    PurchaseInvoice piEnt = PurchaseInvoice.Find(obj);
                    DeleteInvoiceDetail(piEnt);
                    DataHelper.ExecSql("delete SHHG_AimExamine..PurchaseInvoice where Id='" + obj.ToString() + "'");
                }
            }
        }
        private void DeleteInvoiceDetail(PurchaseInvoice piEnt)
        {
            //1 找到该发票下 所有的采购发票详细
            IList<PurchaseInvoiceDetail> pidEnts = PurchaseInvoiceDetail.FindAll("from PurchaseInvoiceDetail where PurchaseInvoiceId='" + piEnt.Id + "'");
            ArrayList poidarray = new ArrayList();
            string poid = string.Empty;
            PurchaseOrderDetail podEnt = null;
            for (int i = 0; i < pidEnts.Count; i++)
            {
                podEnt = PurchaseOrderDetail.TryFind(pidEnts[i].PurchaseOrderDetailId);//找发票详细对应的采购详细
                if (podEnt.PurchaseOrderId != poid)
                {
                    poid = podEnt.PurchaseOrderId;
                    poidarray.Add(poid);
                }
                if (podEnt.InvoiceState == "已关联")//如果采购详细的发票的状态已经改变了 则还原
                {
                    podEnt.InvoiceState = "未关联";
                    podEnt.DoUpdate();
                }
                pidEnts[i].Delete();//删除该发票详细
            }
            //再对采购详细涉及的采购单遍历一次  如果发票状态已改变 则还原
            for (int k = 0; k < poidarray.Count; k++)
            {
                PurchaseOrder poEnt = PurchaseOrder.TryFind(poidarray[k].ToString());
                IList<PurchaseOrderDetail> podEnts = PurchaseOrderDetail.FindAll("from PurchaseOrderDetail where PurchaseOrderId='" + poidarray[k].ToString() + "' and InvoiceState='未关联'");
                if (podEnts.Count > 0 && poEnt.InvoiceState == "已关联")
                {
                    poEnt.InvoiceState = "未关联";
                    poEnt.OrderState = "未结束";
                    poEnt.DoUpdate();
                }
            }
            //至此订单和订单详细的状态回滚完毕
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

