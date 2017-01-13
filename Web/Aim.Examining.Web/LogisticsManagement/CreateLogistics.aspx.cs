﻿using System;
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

namespace Aim.Examining.Web.LogisticsManagement
{
    public partial class CreateLogistics : ExamListPage
    {
        private IList<Logistic> ents = null;
        string strjosn = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else if (RequestActionString == "bjwxtx")
            {
                string db = ConfigurationManager.AppSettings["ExamineDB"];
                string ids = RequestData.Get<string>("ids");
                DataHelper.ExecSql("update " + db + "..DeliveryOrder set LogisticState='无需填写' where Id in ('" + ids.Replace(",", "','") + "')");
            }
            else if (RequestActionString == "qxbjwxtx")
            {
                string db = ConfigurationManager.AppSettings["ExamineDB"];
                string ids = RequestData.Get<string>("ids");
                DataHelper.ExecSql("update " + db + "..DeliveryOrder set LogisticState='' where Id in ('" + ids.Replace(",", "','") + "')");
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
            if (SearchCriterion.Orders.Count == 0)
            {
                if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
                    SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));
            }

            string type = "all";

            foreach (CommonSearchCriterionItem sear in SearchCriterion.Searches.Searches)
            {
                if (sear.PropertyName == "Name" && sear.Value + "" != "")
                {
                    type = "Logistic";
                }
                else if ((sear.PropertyName == "CName" && sear.Value + "" != "") || (sear.PropertyName == "Number" && sear.Value + "" != ""))
                {
                    type = "Order";
                }
            }

            string did = RequestData.Get<string>("did");
            if (type == "all" || type == "Logistic" || !string.IsNullOrEmpty(did))
            {
                ents = Logistic.FindAll(SearchCriterion, Expression.Sql(" charindex('" + did + "',DeliveryId)>0 "));

                //if (ents.Count == 0 && !string.IsNullOrEmpty(did))
                //{
                //    DeliveryOrder doent = DeliveryOrder.TryFind(did);
                //    if (doent != null)
                //    {
                //        ents = Logistic.FindAll(SearchCriterion, Expression.Eq("CustomerId", doent.CId));
                //    }
                //}

                this.PageState.Add("LogisticList", ents);
            }

            if (type == "all" || type == "Order")
            {
                string wtype = RequestData.Get<string>("wtype");
                ICriterion crit = null;
                if (wtype == "1")
                {
                    //SearchCriterion.AddSearch("LogisticState", "已填写");
                    crit = Expression.Sql(" (LogisticState = '已填写' or  LogisticState='无需填写') ");
                }
                else
                {
                    crit = Expression.Or(Expression.Sql(" LogisticState <> '已填写' and  LogisticState<>'无需填写'"), Expression.IsNull("LogisticState"));
                }
                SearchCriterion.AddSearch("State", "已出库");
                DeliveryOrder[] ents2 = DeliveryOrder.FindAll(SearchCriterion, crit);
                this.PageState.Add("OrderList", ents2);

                SearchCriterion.RemoveSearch("State");
            }
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
                Logistic.DoBatchDelete(idList.ToArray());
            }
        }
    }
}
