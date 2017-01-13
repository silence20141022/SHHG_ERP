using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Aim.Portal;
using Aim.Portal.Model;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Castle.ActiveRecord;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web.BaseInfo
{
    public partial class ProductEdit : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id 
        Dictionary<string, object> dic = null;
        ProductsPart entPart = null;
        Product ent = null;
        string Code = "";
        string sql = "";
        string db = ConfigurationManager.AppSettings["ExamineDB"];
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            Code = RequestData.Get<string>("code");
            IList<string> entStrList = RequestData.GetList<string>("data");
            if (!string.IsNullOrEmpty(id))
            {
                ent = Product.Find(id);
            }
            switch (RequestActionString)
            {
                case "update":
                    // string tempisbn = Product.Find(id).Isbn;
                    ent = GetMergedData<Product>();
                    ent.DoUpdate();
                    //if (tempisbn != ent.Isbn)
                    //{
                    //    //一旦更新产品信息后 要注意反写ProductPart表 不仅仅删除或者添加自己本身的子产品 BY PH 20120402
                    //    DataHelper.ExecSql("update SHHG_AimExamine..ProductsPart set CIsbn='" + ent.Isbn + "' where CId='" + ent.Id + "'");
                    //    //一旦条形码更新后 顺带更新未出库明细的产品条形码
                    //    string tempsql = @"UPDATE " + db + "..DelieryOrderPart set Isbn='" + ent.Isbn + "' where PCode='" + ent.Code + "' and State='未出库' " +
                    //                      " update " + db + "..OrdersPart set Isbn='" + ent.Isbn + "' where IsValid='1' and (OutCount is null or [Count]<> OutCount) and PId='" + ent.Id + "'";
                    //    DataHelper.ExecSql(tempsql);
                    //}
                    //if (tempisbn != ent.Isbn)
                    //{
                    //    //一旦更新产品信息后 要注意反写ProductPart表 不仅仅删除或者添加自己本身的子产品 BY PH 20120402
                    //    DataHelper.ExecSql("update SHHG_AimExamine..ProductsPart set CIsbn='" + ent.Isbn + "' where CId='" + ent.Id + "'");
                    //    //一旦条形码更新后 顺带更新未出库明细的产品条形码
                    //    string tempsql = @"UPDATE " + db + "..DelieryOrderPart set Isbn='" + ent.Isbn + "' where PCode='" + ent.Code + "' and State='未出库' " +
                    //                      " update " + db + "..OrdersPart set Isbn='" + ent.Isbn + "' where IsValid='1' and PId='" + ent.Id + "'";
                    //    DataHelper.ExecSql(tempsql);
                    //} 
                    ProcessDetail(entStrList);
                    break;
                case "create":
                    ent = GetPostedData<Product>();
                    ent.DoCreate();
                    ProcessDetail(entStrList);
                    break;
                case "VerificationCode":
                    bool IsExist = false;
                    if (!string.IsNullOrEmpty(id))
                    {
                        sql = "select Id from SHHG_AimExamine..Products where code='" + Code + "' and id!='" + id + "'";
                    }
                    else
                    {
                        sql = "select Id from SHHG_AimExamine..Products where code='" + Code + "'";
                    }
                    IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
                    if (dics.Count > 0)
                    {
                        IsExist = true;
                    }
                    PageState.Add("IsExist", IsExist);
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            SetFormData(ent);
            PageState.Add("ProductType", SysEnumeration.GetEnumDict("ProductType"));
            PageState.Add("Unit", SysEnumeration.GetEnumDict("Unit"));
            sql = @"select A.*,B.Name,B.Code,B.Isbn from SHHG_AimExamine..ProductsPart as A
                left join SHHG_AimExamine..Products as B on A.CId=B.Id  where PId='" + id + "'";
            IList<EasyDictionary> dics = DataHelper.QueryDictList(sql);
            PageState.Add("DetailList", dics);
        } 
        private void ProcessDetail(IList<string> entStrList)
        {
            ProductsPart.DeleteAll("PId='" + ent.Id + "'");
            if (entStrList != null && entStrList.Count > 0)
            {
                for (int j = 0; j < entStrList.Count; j++)
                {
                    Newtonsoft.Json.Linq.JObject objL = JsonHelper.GetObject<Newtonsoft.Json.Linq.JObject>(entStrList[j]);
                    ProductsPart propartEnt = new ProductsPart();
                    propartEnt.PId = ent.Id;
                    propartEnt.PIsbn = ent.Isbn;
                    propartEnt.CId = objL.Value<string>("CId");
                    propartEnt.CIsbn = objL.Value<string>("Isbn");
                    propartEnt.CCount = objL.Value<int>("CCount");
                    propartEnt.Remark = objL.Value<string>("Remark");
                    propartEnt.DoCreate();
                }
            }
        }
    }
}

