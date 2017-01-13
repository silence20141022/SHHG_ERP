using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using System.Data;
using Aim.Examining.Model;
using Aim.WorkFlow;
using Aim.Portal.Web;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PayBill_List : System.Web.UI.Page
    {
        int totalProperty = 0;
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Aim.Portal.Web.WebPortalService.CheckLogon();
            }
            catch
            {
                Response.Write("<script> window.location.href = '/Login.aspx';</script>");
                Response.End();
            }
            string action = Request["action"];
            string where = "";
            DataTable dt = null;
            string id = Request["id"];
            PayBill pbEnt = null;
            if (!string.IsNullOrEmpty(id))
            {
                pbEnt = PayBill.Find(id);
            }
            IList<PayBillDetail> pbdEnts = null;
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["PayBillNo"]))
                    {
                        where += " and PayBillNo like '%" + Request["PayBillNo"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["SupplierName"]))
                    {
                        where += " and SupplierName like '%" + Request["SupplierName"].Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(Request["State"]))
                    {
                        where += " and State = '" + Request["State"] + "'";
                    }
                    if (!string.IsNullOrEmpty(Request["ProductCode"]))
                    {
                        where += " and Id in (select distinct PayBillId from SHHG_AimExamine..PayBillDetail where  ProductCode like '%" + Request["ProductCode"] + "%')";
                    }
                    sql = @"select * from SHHG_AimExamine..PayBill where 1=1 " + where;
                    dt = DataHelper.QueryDataTable(GetPageSql(sql));
                    Response.Write("{total:" + totalProperty + ",rows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "loaddetail":
                    sql = @"select a.*,c.PurchaseOrderNo from SHHG_AimExamine..PayBillDetail a 
                            left join SHHG_AimExamine..PurchaseOrderDetail b on a.PurchaseOrderDetailId=b.Id 
                            left join SHHG_AimExamine..PurchaseOrder c on b.PurchaseOrderId=c.Id 
                            where a.PayBillId='" + id + "' order by a.ProductCode asc";
                    dt = DataHelper.QueryDataTable(sql);
                    Response.Write("{innerrows:" + JsonHelper.GetJsonStringFromDataTable(dt) + "}");
                    Response.End();
                    break;
                case "delete":
                    pbEnt = PayBill.Find(id);
                    pbdEnts = PayBillDetail.FindAllByProperty("PayBillId", id);
                    foreach (PayBillDetail tempEnt in pbdEnts)
                    {
                        //删除完付款单明细后需要更新入库单明细的生成付款单的数量
                        if (!string.IsNullOrEmpty(tempEnt.InWarehouseDetailId))
                        {
                            InWarehouseDetail iwdEnt = InWarehouseDetail.Find(tempEnt.InWarehouseDetailId);
                            iwdEnt.FuKuanDanQuan = iwdEnt.FuKuanDanQuan - tempEnt.PayQuantity;
                            iwdEnt.DoUpdate();
                            tempEnt.DoDelete();
                        }
                    }
                    //如果已经审批过了，还需要删除workflowinstance    task记录
                    IList<WorkflowInstance> wiEnts = WorkflowInstance.FindAllByProperty(WorkflowInstance.Prop_EFormInstanceID, pbEnt.Id);
                    foreach (WorkflowInstance wiEnt in wiEnts)
                    {
                        IList<Task> tEnts = Task.FindAllByProperty(Task.Prop_WorkflowInstanceID, wiEnt.ID);
                        foreach (Task tEnt in tEnts)
                        {
                            tEnt.DoDelete();
                        }
                    }
                    pbEnt.DoDelete();
                    Response.Write("{success:true}");
                    Response.End();
                    break;
                case "examine":
                    WorkflowTemplate ne = WorkflowTemplate.FindAllByProperties(Aim.WorkFlow.WorkflowTemplate.Prop_Code, Request["flowkey"])[0];
                    string formUrl = "/PurchaseManagement/PayBillView.aspx?id=" + id; //启动流程表单路径,后面加上参数传入 
                    Guid instanceid = Aim.WorkFlow.WorkFlow.StartWorkFlow(id, formUrl, "付款单【" + pbEnt.PayBillNo + "】申请人【" + pbEnt.CreateName + "】", Request["flowkey"], WebPortalService.CurrentUserInfo.UserID, WebPortalService.CurrentUserInfo.Name);
                    pbEnt.WorkFlowState = "flowing";
                    pbEnt.ExamineResult = "已提交";
                    pbEnt.DoUpdate();
                    Response.Write("{success:true,instanceid:'" + instanceid + "'}");
                    Response.End();
                    break;
                case "autoexec":
                    Task task = Task.FindAllByProperties(Task.Prop_WorkflowInstanceID, Request["instanceid"])[0];
                    Aim.WorkFlow.WorkFlow.AutoExecute(task);
                    Response.Write("{success:true}");
                    Response.End();
                    break;
            }
        }
        private string GetPageSql(string tempsql)
        {
            int start = Convert.ToInt32(Request["start"]);
            int limit = Convert.ToInt32(Request["limit"]);
            totalProperty = DataHelper.QueryValue<int>("select count(1) from (" + tempsql + ") t");
            string order = "CREATETIME";
            string asc = " desc";
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, asc, tempsql, start + 1, limit + start);
            return pageSql;
        }
    }
}