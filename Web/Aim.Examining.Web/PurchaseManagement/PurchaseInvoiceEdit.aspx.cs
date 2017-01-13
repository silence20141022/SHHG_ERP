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
using System.Web.Script.Serialization;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseInvoiceEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            PurchaseInvoice ent = null;
            IList<string> entStrList = RequestData.GetList<string>("data");
            switch (RequestActionString)
            {
                case "create":
                    ent = this.GetPostedData<PurchaseInvoice>();
                    ent.State = "未关联";
                    ent.CreateId = UserInfo.UserID;
                    ent.CreateName = UserInfo.Name;
                    ent.CreateTime = System.DateTime.Now;
                    ent.DoCreate();
                    //添加采购发票详情
                    ProcessDetail(entStrList, ent);
                    break;
                case "update":
                    ent = this.GetMergedData<PurchaseInvoice>();
                    ent.DoUpdate();
                    //删除的时候要做其他相关的表的回滚处理   PurchaseInvoiceDetail.DeleteAll("PurchaseInvoiceId='" + ent.Id + "'");
                    //不能做简单如上处理
                    DeleteInvoiceDetail(ent);
                    ProcessDetail(entStrList, ent);
                    break;
                case "batchdelete":
                    if (entStrList.Count > 0)
                    {
                        ArrayList pididarray = new ArrayList();
                        for (int i = 0; i < entStrList.Count; i++)
                        {
                            Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[i]);
                            //DataHelper.ExecSql("delete SHHG_AimExamine..PurchaseInvoiceDetail where Id='" + objL.Value<string>("Id") + "'");
                            //同样删除发票详细的时候也不能做如上简单处理 也要对订单和订单详细做状态回滚
                            pididarray.Add(objL.Value<string>("Id"));
                        }
                        DeleteInvoiceDetail(pididarray);
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    string tempsql = @"select A.* ,B.SupplierName,B.MoneyType,B.Symbo from SHHG_AimExamine..PurchaseInvoice as A 
                    left join SHHG_AimExamine..Supplier as B on A.SupplierId=B.Id where A.Id='{0}'";
                    tempsql = string.Format(tempsql, id);
                    IList<EasyDictionary> dics = DataHelper.QueryDictList(tempsql);
                    if (dics.Count > 0)
                    {
                        this.SetFormData(dics[0]);
                    }
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.*,B.PurchaseOrderNo,B.PurchaseOrderId,A.InvoiceQuantity as Raw,
                       SHHG_AimExamine.dbo.fun_getNoInvoiceQuantityByPurchaseOrderDetailId(A.PurchaseOrderDetailId) as NoInvoice
                       from SHHG_AimExamine..PurchaseInvoiceDetail as A                        
                       left join SHHG_AimExamine..PurchaseOrderDetail as B on A.PurchaseOrderDetailId=B.Id                      
                       where A.PurchaseInvoiceId='{0}' order by B.PurchaseOrderId asc";
                sql = string.Format(sql, id);
                IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                this.PageState.Add("DetailList", dics);
            }
            else
            {
                PageState.Add("PurchaseInvoiceNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getPurchaseInvoiceNo()").ToString());
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
        private void DeleteInvoiceDetail(ArrayList idarray)
        {
            //原理基本同上。只不过一个传过来的是采购发票 而这个传递过来的时发票详细的id集合
            IList<PurchaseInvoiceDetail> pidEnts = new List<PurchaseInvoiceDetail>();
            for (int t = 0; t < idarray.Count; t++)
            {
                PurchaseInvoiceDetail tempEnt = PurchaseInvoiceDetail.TryFind(idarray[t].ToString());
                pidEnts.Add(tempEnt);
            }
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
            //至此采购订单和采购订单详细的状态回滚完毕
        }
        private void ProcessDetail(IList<string> entStrList, PurchaseInvoice piEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                ArrayList idarray = new ArrayList();
                string purorderId = string.Empty;
                decimal totalInvoiceAmount = 0;
                for (int j = 0; j < entStrList.Count; j++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    PurchaseInvoiceDetail pbdEnt = new PurchaseInvoiceDetail();
                    pbdEnt.PurchaseInvoiceId = piEnt.Id;
                    pbdEnt.PurchaseOrderDetailId = objL.Value<string>("PurchaseOrderDetailId");
                    pbdEnt.ProductId = objL.Value<string>("ProductId");
                    pbdEnt.ProductCode = objL.Value<string>("ProductCode");
                    pbdEnt.ProductName = objL.Value<string>("ProductName");
                    pbdEnt.BuyPrice = Convert.ToDecimal(objL.Value<string>("BuyPrice"));
                    pbdEnt.InvoiceQuantity = Convert.ToInt32(objL.Value<string>("InvoiceQuantity"));
                    pbdEnt.InvoiceAmount = Convert.ToDecimal(objL.Value<string>("InvoiceAmount"));
                    pbdEnt.DoCreate();
                    totalInvoiceAmount += pbdEnt.InvoiceAmount.Value;
                    //循环每个采购详细的发票数量 如果==采购数  则状态改为已关联
                    int novoice = Convert.ToInt32(DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getNoInvoiceQuantityByPurchaseOrderDetailId('" + pbdEnt.PurchaseOrderDetailId + "')"));
                    if (novoice == 0)
                    {
                        PurchaseOrderDetail podEnt = PurchaseOrderDetail.TryFind(objL.Value<string>("PurchaseOrderDetailId"));
                        if (podEnt.PurchaseOrderId != purorderId)
                        {
                            idarray.Add(podEnt.PurchaseOrderId);
                            purorderId = podEnt.PurchaseOrderId;
                        }
                        podEnt.InvoiceState = "已关联";
                        podEnt.DoUpdate();
                    }
                }
                //如果所有发票详细的金额==发票的总金额  则把发票的状态改为已关联
                if (totalInvoiceAmount == piEnt.InvoiceAmount)
                {
                    piEnt.State = "已关联";
                    piEnt.DoUpdate();
                }
                //遍历采购单 如果其下所有的采购详细的发票状态都是已关联 则自身变为已关联
                for (int k = 0; k < idarray.Count; k++)
                {
                    IList<EasyDictionary> edics = DataHelper.QueryDictList("select Id from SHHG_AimExamine..PurchaseOrderDetail where PurchaseOrderId='" + idarray[k].ToString() + "' and  InvoiceState='未关联'");
                    if (edics.Count == 0)
                    {
                        PurchaseOrder poEnt = PurchaseOrder.TryFind(idarray[k].ToString());
                        poEnt.InvoiceState = "已关联";
                        if (poEnt.InWarehouseState == "已入库" && poEnt.PayState == "已付款")
                        {
                            poEnt.OrderState = "已结束";
                        }
                        poEnt.DoUpdate();
                    }
                }
            }
        }
    }
}

