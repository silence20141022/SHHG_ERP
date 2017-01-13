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

namespace Aim.Examining.Web
{
    public partial class FrmReimbursementAppView : ExamListPage
    {
        private IList<Reimbursement> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "submit")
            {
                StartFlow();
            }
            else if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else
            {
                DoSelect();
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
                Reimbursement.DoBatchDelete(idList.ToArray());
            }
        }

        public void StartFlow()
        {
            string state = this.RequestData.Get<string>("state");
            string id = this.RequestData.Get<string>("Id");
            Reimbursement pb = Reimbursement.Find(id);
            pb.State = state;
            pb.Save();
            string code = this.RequestData.Get<string>("FlowKey");

            //启动流程
            //表单路径,后面加上参数传入
            string formUrl = "/DailyManager/FrmReimbursementAppEdit.aspx?op=r&&id=" + id;
            Aim.WorkFlow.WorkFlow.StartWorkFlow(id, formUrl, "报销申请(" + pb.LeaveUser + ")", code, this.UserInfo.UserID, this.UserInfo.Name);
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
                SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));

            ents = Reimbursement.FindAll(SearchCriterion);
            this.PageState.Add("ReimbursementList", ents);
        }
    }
}
