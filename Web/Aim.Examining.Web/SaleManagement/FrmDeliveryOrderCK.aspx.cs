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
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmDeliveryOrderCK : ExamListPage
    {
        private IList<DeliveryOrder> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else if (RequestActionString == "markDelivery")
            {
                DeliveryOrder pc = DeliveryOrder.Find(this.RequestData.Get<string>("Id"));
                pc.State = "已出库";
                pc.Save();
                ////添加交易记录
                //TransactionHi his = new TransactionHi
                //{
                //    Child = pc.Child,
                //    CId = pc.CId,
                //    CName = pc.CName,
                //    CreateId = UserInfo.UserID,
                //    CreateName = UserInfo.Name,
                //    CreateTime = DateTime.Now,
                //    RtnOrOut = "购买",
                //    Number = pc.Number,
                //    TransactionTime = DateTime.Now
                //};
                //his.DoCreate();

                ////自动计算客户余额
                //Customer custom = Customer.Find(pc.CId);
                //custom.PreDeposit = custom.PreDeposit - pc.TotalMoneyHis <= 0 ? 0 : custom.PreDeposit - pc.TotalMoneyHis;
                //custom.Save();

                //计算仓库数量
            }
            else
            {
                DoSelect();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string where = " where 1=1 ";
            string wherec = " where 1=1 ";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "Code")
                {
                    if (item.Value + "" != "")
                    {
                        where += " and Id in (select DId from " + db + "..DelieryOrderPart where PCode like '%" + item.Value + "%')";
                    }
                }
                else
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            if (RequestData.Get<string>("type") == "yi")
            {
                where += " and [State]='已出库' ";
                wherec += " and p.[State]='已出库' ";
            }
            else
            {
                where += " and ([State] is null or [State]<>'已出库') ";
                wherec += " and (p.[State] is null or p.[State]<>'已出库') ";
            }
            string sql = @"select * from " + db + "..DeliveryOrder " + where;
            this.PageState.Add("OrderList", GetPageData(sql, SearchCriterion));

            //查询详细产品的数量
            CommonSearchCriterionItem itemtp = SearchCriterion.Searches.Searches.Where(obj => obj.PropertyName == "Code").FirstOrDefault<CommonSearchCriterionItem>();
            SearchCriterion.Searches.RemoveSearch("Code");
            foreach (CommonSearchCriterionItem search in SearchCriterion.Searches.Searches)
            {
                wherec += " and p." + search.PropertyName + " like '%" + search.Value + "%'";
            }
            if (itemtp != null && itemtp.Value + "" != "")
            {
                wherec += " and c.PCode like '%" + itemtp.Value + "%'";
            }
            PageState.Add("quantity", DataHelper.QueryValue("select sum([Count]) from " + db + "..DeliveryOrder p inner join " + db + "..DelieryOrderPart c on c.DId=p.Id" + wherec));

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
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");

            if (idList != null && idList.Count > 0)
            {
                DeliveryOrder.DoBatchDelete(idList.ToArray());
            }
        }
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
        }

    }
}
