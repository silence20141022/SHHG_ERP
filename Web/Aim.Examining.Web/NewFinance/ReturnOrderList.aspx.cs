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
using Newtonsoft.Json.Linq;

namespace Aim.Examining.Web
{
    public partial class ReturnOrderList : ExamListPage
    {
        string id = "";
        ReturnOrder roEnt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");
            if (!string.IsNullOrEmpty(id))
            {
                roEnt = ReturnOrder.Find(id);
            }
            switch (RequestActionString)
            {
                case "delete":
                    //先删除入库单及入库明细
                    InWarehouse iwEnt = InWarehouse.FindAllByProperty(InWarehouse.Prop_PublicInterface, roEnt.Id).FirstOrDefault<InWarehouse>();
                    IList<OtherInWarehouseDetail> oiwdEnts = OtherInWarehouseDetail.FindAllByProperty(OtherInWarehouseDetail.Prop_InWarehouseId, iwEnt.Id);
                    foreach (OtherInWarehouseDetail oiwdEnt in oiwdEnts)
                    {
                        oiwdEnt.DoDelete();
                    }
                    iwEnt.DoDelete();
                    //更新销售单及销售明细
                    SaleOrder soEnt = SaleOrder.FindAllByProperty(SaleOrder.Prop_Number, roEnt.OrderNumber).FirstOrDefault<SaleOrder>();
                    soEnt.ReturnAmount = (soEnt.ReturnAmount.HasValue ? soEnt.ReturnAmount.Value : 0) - (roEnt.ReturnMoney.HasValue ? roEnt.ReturnMoney.Value : 0);
                    soEnt.DoUpdate();
                    IList<ReturnOrderPart> ropEnts = ReturnOrderPart.FindAllByProperty(ReturnOrderPart.Prop_ReturnOrderId, roEnt.Id);
                    foreach (ReturnOrderPart ropEnt in ropEnts)
                    {
                        OrdersPart opEnt = OrdersPart.Find(ropEnt.OrderPartId);
                        opEnt.ReturnCount = (opEnt.ReturnCount.HasValue ? opEnt.ReturnCount.Value : 0) - ropEnt.Count;
                        opEnt.DoUpdate();
                        ropEnt.DoDelete();
                    }
                    roEnt.DoDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            string where = string.Empty;
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.Value.ToString() != "")
                {
                    if (item.PropertyName == "BeginDate")
                    {
                        where += " and CreateTime>'" + item.Value + "' ";
                    }
                    else
                    {
                        if (item.PropertyName == "EndDate")
                        {
                            where += " and CreateTime<'" + (item.Value.ToString()).Replace(" 0:00:00", " 23:59:59") + "' ";
                        }
                        else
                        {
                            where += " and " + item.PropertyName + " like '%" + item.Value + "%'";
                        }
                    }
                }
            }
            string sql = "select * from SHHG_AimExamine..ReturnOrder where 1=1" + where;
            PageState.Add("OrderList", GetPageData(sql, SearchCriterion));
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
