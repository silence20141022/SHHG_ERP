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
    public partial class CombineSplitEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        CombineSplit csEnt = null;
        IList<StockInfo> siEnts = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            IList<string> entStrList = RequestData.GetList<string>("data");
            switch (RequestActionString)
            {
                case "create":
                    csEnt = this.GetPostedData<CombineSplit>();
                    csEnt.DoCreate();
                    ProcessDetail(entStrList, csEnt);
                    break;
                case "LoadProductDetail":
                    string productId = RequestData.Get<string>("ProductId");
                    string warehouseId = RequestData.Get<string>("WarehouseId");
                    string tempsql = @"select A.Id,A.PId, A.CId as ProductId,A.CCount as ProductQuantity,A.CCount as RawValue,
                    B.Name as ProductName,
                    B.Code as ProductCode,B.Pcn as ProductPcn,
                    SHHG_AimExamine.dbo.fun_getProQuantityByWarehouseId(A.CId,'{0}') as StockQuantity
                    from SHHG_AimExamine..ProductsPart as A 
                    left join SHHG_AimExamine..Products as B
                    on A.CId=B.Id where A.PId='{1}' order by B.Code asc";
                    tempsql = string.Format(tempsql, warehouseId, productId);
                    IList<EasyDictionary> tempdics = DataHelper.QueryDictList(tempsql);
                    PageState.Add("DetailDataList", tempdics);
                    break;
                //case "update":
                //    csEnt = this.GetMergedData<CombineSplit>();
                //    csEnt.DoUpdate();
                //    //删除的时候要做其他相关的表的回滚处理   PurchaseInvoiceDetail.DeleteAll("PurchaseInvoiceId='" + ent.Id + "'");
                //    //不能做简单如上处理
                //    DeleteInvoiceDetail(ent);
                //    ProcessDetail(entStrList, ent);
                //    break;
                //case "batchdelete":
                //    if (entStrList.Count > 0)
                //    {
                //        ArrayList pididarray = new ArrayList();
                //        for (int i = 0; i < entStrList.Count; i++)
                //        {
                //            Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[i]);
                //            //DataHelper.ExecSql("delete SHHG_AimExamine..PurchaseInvoiceDetail where Id='" + objL.Value<string>("Id") + "'");
                //            //同样删除发票详细的时候也不能做如上简单处理 也要对订单和订单详细做状态回滚
                //            pididarray.Add(objL.Value<string>("Id"));
                //        }
                //        DeleteInvoiceDetail(pididarray);
                //    }
                //    break;
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
                IList<CombineSplitDetail> csdEnts = CombineSplitDetail.FindAllByProperties("CombineSplitId", id);
                this.PageState.Add("DataList", csdEnts);
            }
            else
            {
                PageState.Add("CombineSplitNo", DataHelper.QueryValue("select SHHG_AimExamine.dbo.fun_getCombineSplitNo()").ToString());
            }
        }
        private void ProcessDetail(IList<string> entStrList, CombineSplit csEnt)
        {
            if (entStrList != null && entStrList.Count > 0)
            {
                StockInfo siEnt = new StockInfo();
                for (int j = 0; j < entStrList.Count; j++)
                {
                    //组装拆分详细创建
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    CombineSplitDetail csdEnt = new CombineSplitDetail();
                    csdEnt.CombineSplitId = csEnt.Id;
                    csdEnt.ProductId = objL.Value<string>("ProductId");
                    csdEnt.ProductCode = objL.Value<string>("ProductCode");
                    csdEnt.ProductName = objL.Value<string>("ProductName");
                    csdEnt.ProductPcn = objL.Value<string>("ProductPcn");
                    csdEnt.StockQuantity = objL.Value<int>("StockQuantity");
                    csdEnt.ProductQuantity = objL.Value<int>("ProductQuantity");
                    csdEnt.DoCreate();
                    //更新库存    
                    siEnts = StockInfo.FindAllByProperties("ProductId", csdEnt.ProductId, "WarehouseId", csEnt.WarehouseId);
                    if (siEnts.Count > 0)//如果库存信息存在
                    {
                        siEnt = siEnts[0];
                        if (csEnt.OperateType == "组装")
                        {
                            siEnt.StockQuantity -= csdEnt.ProductQuantity;
                        }
                        else
                        {
                            siEnt.StockQuantity += csdEnt.ProductQuantity;
                        }
                        siEnt.DoUpdate();
                    }
                }
                siEnts = StockInfo.FindAllByProperties("ProductId", csEnt.ProductId, "WarehouseId", csEnt.WarehouseId);
                if (siEnts.Count > 0)
                {
                    siEnt = siEnts[0];
                    if (csEnt.OperateType == "组装")
                    {
                        siEnt.StockQuantity += csEnt.ProductQuantity;
                    }
                    else
                    {
                        siEnt.StockQuantity -= csEnt.ProductQuantity;
                    }
                    siEnt.DoUpdate();
                }
            }
        }
    }
}

