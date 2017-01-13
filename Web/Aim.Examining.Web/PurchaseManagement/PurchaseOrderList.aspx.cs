using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Data;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseOrderList : ExamListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = RequestData.Get<string>("id");
            switch (RequestActionString)
            {
                case "Modify":
                    PageState.Add("result", AllowOperate(id) == true ? "true" : "false");
                    break;
                case "batchdelete":
                    IList<object> idList = RequestData.GetList<object>("IdList");
                    if (idList != null && idList.Count > 0)
                    {
                        foreach (object obj in idList)
                        {
                            bool allowDelete = AllowOperate(obj.ToString());
                            if (allowDelete)
                            {
                                DataHelper.ExecSql("delete SHHG_AimExamine..PurchaseOrder where Id='" + obj.ToString() + "'");
                                PurchaseOrderDetail.DeleteAll("PurchaseOrderId='" + obj + "'");//删除采购订单详细
                                PageState.Add("Message", "删除成功！");
                            }
                            else
                            {
                                PageState.Add("Message", "已创建入库单或者付款单的订单不允许删除！");
                                return;
                            }
                        }
                    }
                    break;
                default:
                    if (RequestData.Get<string>("optype") == "getChildData")
                    {
                        string purchaseOrderId = RequestData.Get<string>("PurchaseOrderId");
                        string tempsql = @"select A.*,C.Symbo from SHHG_AimExamine..PurchaseOrderDetail as A left join SHHG_AimExamine..PurchaseOrder as B
                        on A.PurchaseOrderId=B.Id left join SHHG_AimExamine..Supplier as C on B.SupplierId=C.Id where A.PurchaseOrderId='" + purchaseOrderId + "'";
                        IList<EasyDictionary> dics = DataHelper.QueryDictList(tempsql);
                        PageState.Add("DetailList", dics);
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
            string index = RequestData.Get<string>("Index");
            string where = "";
            string detailwhere = "";
            string payState = RequestData.Get<string>("PayState");
            string inWarehouseState = RequestData.Get<string>("InWarehouseState");
            string orderState = RequestData.Get<string>("OrderState");
            if (!string.IsNullOrEmpty(payState))
            {
                SearchCriterion.AddSearch("PayState", payState);
            }
            if (!string.IsNullOrEmpty(inWarehouseState))
            {
                SearchCriterion.AddSearch("InWarehouseState", inWarehouseState);
            }
            if (!string.IsNullOrEmpty(orderState))
            {
                SearchCriterion.AddSearch("OrderState", orderState);
            }
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
                    if (item.PropertyName != "Code")
                    {
                        where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                    else
                    {
                        detailwhere = "where " + item.PropertyName + " like '%" + item.Value + "%'";
                    }
                }
            }
            if (index == "0")
            {
                sql = @"select A.* from SHHG_AimExamine..PurchaseOrder as A                       
                where A.InWarehouseState='未入库' and A.Id in 
                (select distinct PurchaseOrderId from SHHG_AimExamine..PurchaseOrderDetail " + detailwhere + ")" + where;
            }//采购人员也能看到别人做的采购订单所以这里不加人员过滤
            else
            {
                sql = @"select A.* from SHHG_AimExamine..PurchaseOrder as A           
                where A.InWarehouseState='已入库' and  A.Id in
                (select distinct PurchaseOrderId from SHHG_AimExamine..PurchaseOrderDetail " + detailwhere + ")" + where;
            }
            PageState.Add("PurchaseOrderList", GetPageData(sql, SearchCriterion));
        }
        //sql 分页
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
        private bool AllowOperate(string id)
        {
            //通过采购单Id去查询入库单详细和付款申请单  如果已经生成入库单 或者申请付款单据 或者发票单  拒绝删除
            bool allow = true;
            string inwareSql = @"select A.Id from SHHG_AimExamine..InWarehouseDetail as A 
                            left join SHHG_AimExamine..PurchaseOrderDetail as B on A.PurchaseOrderDetailId=B.Id
                            where B.PurchaseOrderId='{0}'";
            inwareSql = string.Format(inwareSql, id);
            IList<EasyDictionary> iwdDics = DataHelper.QueryDictList(inwareSql);//判断入库单详细中有没有该采购单的产品
            if (iwdDics.Count > 0)
            {
                allow = false;
            }
            string pbSql = @"select A.Id from SHHG_AimExamine..PayBillDetail as A 
            left join SHHG_AimExamine..PurchaseOrderDetail as B on A.PurchaseOrderDetailId=B.Id where B.PurchaseOrderId='{0}'";
            pbSql = string.Format(pbSql, id);
            IList<EasyDictionary> pbDics = DataHelper.QueryDictList(pbSql);//判断付款单详细中有没有该采购单的产品
            if (pbDics.Count > 0)
            {
                allow = false;
            }
            string piSql = @"select A.Id from SHHG_AimExamine..PurchaseInvoiceDetail as A
            left join SHHG_AimExamine..PurchaseOrderDetail as B on A.PurchaseOrderDetailId=B.Id where B.PurchaseOrderId='{0}'";
            piSql = string.Format(piSql, id);
            IList<EasyDictionary> piDics = DataHelper.QueryDictList(piSql);
            if (piDics.Count > 0)//判断采购发票详细中有没有该采购单的产品
            {
                allow = false;
            }
            return allow;
        }
    }
}
