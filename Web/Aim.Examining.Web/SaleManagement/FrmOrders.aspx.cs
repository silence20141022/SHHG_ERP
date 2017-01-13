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
    public partial class FrmOrders : ExamListPage
    {
        private IList<SaleOrder> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "submit")
            {
                StartFlow();
            }
            else if (RequestActionString == "AutoExecuteFlow")
            {
                Task task = Task.FindAllByProperties(Task.Prop_WorkflowInstanceID, this.RequestData.Get<string>("FlowId"))[0];
                //自动执行,关键代码
                Aim.WorkFlow.WorkFlow.AutoExecute(task);
                this.PageState.Add("TaskId", task.ID);
            }
            else if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            } 
            else if (RequestData.Get<string>("optype") == "getChildData")
            {
                string oid = RequestData.Get<string>("OId");
                OrdersPart[] ops = OrdersPart.FindAllByProperties("OId", oid);
                PageState.Add("DetailList", ops);
            }
            else
            {
                DoSelect();
            }
        } 
        public void StartFlow()
        {
            string state = this.RequestData.Get<string>("state");
            string id = this.RequestData.Get<string>("Id");
            SaleOrder pb = SaleOrder.Find(id);
            pb.State = state;
            pb.Save();
            string code = this.RequestData.Get<string>("FlowKey");

            //启动流程
            //表单路径,后面加上参数传入
            string formUrl = "/SaleManagement/FrmOrdersView.aspx?op=u&id=" + id;
            Guid guid = Aim.WorkFlow.WorkFlow.StartWorkFlow(id, formUrl, "销售订单审批(" + pb.Number + ")[" + pb.CreateName + "]", code, this.UserInfo.UserID, this.UserInfo.Name);

            this.PageState.Add("FlowId", guid.ToString());
        } 
        private void DoSelect()
        {
            string ftype = RequestData.Get<string>("ftype"); 
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            string where = " where (DeliveryState is null or DeliveryState<>'已作废') ";
            foreach (CommonSearchCriterionItem item in SearchCriterion.Searches.Searches)
            {
                if (item.PropertyName == "BeginDate")
                {
                    if (item.Value + "" != "")
                    {
                        where += " and CreateTime>'" + item.Value + "' ";
                    }
                }
                else if (item.PropertyName == "EndDate")
                {
                    if (item.Value + "" != "")
                    {
                        where += " and CreateTime<'" + (item.Value + "").Replace(" 0:00:00", " 23:59:59") + "' ";
                    }
                }
                else if (item.PropertyName == "Code")
                {
                    if (item.Value + "" != "")
                    { 
                        where += " and Id in (select OId from " + db + "..OrdersPart where PCode like '%" + item.Value + "%')";
                    }
                }
                else
                {
                    where += " and " + item.PropertyName + " like '%" + item.Value + "%' ";
                }
            }
            //已完成
            if (ftype == "1")
            {
                //where += " and ((InvoiceType='发票' and InvoiceState='已全部开发票' or InvoiceType='收据') and DeState='已全部出库' and CorrespondState='已对应' or DeliveryState='已全部退货')";
                where += " and  isnull(DeState,'')='已全部出库'";
            }
            //未完成
            else if (ftype == "0")
            {
                //where += " and ((InvoiceType='发票' and (InvoiceState is null or InvoiceState<>'已全部开发票')) or isnull(DeState,'')<>'已全部出库' or CorrespondState is null or CorrespondState<>'已对应') and isnull(DeliveryState,'')<>'已全部退货'";//" + db + ".dbo.fun_getDeliveryState(Id)
                where += " and  isnull(DeState,'')<>'已全部出库'"; 
            }
            string sql = @"select * from " + db + "..SaleOrders" + where;
            this.PageState.Add("OrderList", GetPageData(sql, "select count(1) from " + db + "..SaleOrders" + where, SearchCriterion));
        }

        private IList<EasyDictionary> GetPageData(String cols, string tablename, string where, string countsql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>(countsql);
            string order = search.Orders.Count > 0 ? search.Orders[0].PropertyName : "CreateTime";
            string asc = search.Orders.Count <= 0 || !search.Orders[0].Ascending ? " desc" : " asc";
            string pageSql = @"
		    select * from (SELECT {5},
		    ROW_NUMBER() OVER (order by {0} {1}) as RowNumber
		    FROM {2} " + where + ") t where RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, tablename, (search.CurrentPageIndex - 1) * search.PageSize + 1, search.CurrentPageIndex * search.PageSize, cols);
            IList<EasyDictionary> dicts = DataHelper.QueryDictList(pageSql);
            return dicts;
        }

        private IList<EasyDictionary> GetPageData(String sql, string countsql, SearchCriterion search)
        {
            SearchCriterion.RecordCount = DataHelper.QueryValue<int>(countsql);
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


        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");

            if (idList != null && idList.Count > 0)
            {
                SaleOrder.DoBatchDelete(idList.ToArray());

                //删除对应的商品信息防止产生垃圾数据
                foreach (string oid in idList)
                {
                    OrdersPart.DeleteAll("OId='" + oid + "'");
                }
            }
        } 
    }
}
