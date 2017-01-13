using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using System.Configuration;
using Aim.Data;

namespace Aim.Examining.Web.LogisticsManagement
{
    public partial class FrmLogisticsEdit : ExamBasePage
    {

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            string Dids = Request.QueryString["Dids"];
            Logistic ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Logistic>();
                    ent.DoUpdate();
                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<Logistic>();
                    ent.DeliveryId = Dids;
                    ent.PayState = "未付款";
                    ent.DeliveryNumber = Request.QueryString["dnumbers"];
                    ent.DoCreate();
                    //修改物流状态
                    Aim.Data.DataHelper.ExecSql("update " + db + "..DeliveryOrder set LogisticState='已填写' where Id in ('" + Dids.Replace(",", "','") + "')");
                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Logistic>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
                default:
                    if (RequestActionString == "getchild")
                    {
                        string number = RequestData.Get<string>("number");
                        string strjson = RequestData.Get<string>("child").Replace("[]", "");
                        if (strjson.Length > 0)
                        {
                            strjson = strjson.Substring(1, strjson.Length - 2);
                        }
                        if (number != "")
                        {
                            DeliveryOrder order = DeliveryOrder.FindAllByProperty("Number", number).FirstOrDefault<DeliveryOrder>();
                            if (order != null)
                            {
                                if (strjson.Length != 0 && order.Child.Substring(1, order.Child.Length - 2) != "")
                                {
                                    strjson += ",";
                                }
                                strjson += order.Child.Substring(1, order.Child.Length - 2);
                            }
                        }
                        if (strjson != "")
                        {
                            strjson = "[" + strjson + "]";
                            PageState.Add("result", strjson);
                        }
                    }
                    break;
            }

            if (op == "c" && !string.IsNullOrEmpty(Dids))
            {
                string[] didarry = Dids.Split(',');
                DeliveryOrder[] dorders = DeliveryOrder.FindAllByPrimaryKeys(didarry);
                if (dorders.Length > 0)
                {
                    dorders[0].Id = "";
                    dorders[0].Number = "";
                    dorders[0].Remark = "";
                    dorders[0].State = "";

                    //string json = "";
                    //foreach (DeliveryOrder order in dorders)
                    //{
                    //    if (order != null && !string.IsNullOrEmpty(order.Child))
                    //    {
                    //        json += order.Child.Substring(1, order.Child.Length - 2) + ",";
                    //    }
                    //}
                    //dorders[0].Child = "[" + json.Substring(0, json.Length - 1) + "]";

                    this.SetFormData(dorders[0]);

                    IList<EasyDictionary> dicts = DataHelper.QueryDictList("select Id,PCode as Code,PName as [Name],Unit,[Count] as OutCount,Remark from " + db + "..DelieryOrderPart where DId in ('" + Dids.Replace(",", "','") + "')");
                    PageState.Add("DetailList", dicts);
                }
            }
            else if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = Logistic.Find(id);
                }
                this.SetFormData(ent);
            }
            PageState.Add("EDNames", SysEnumeration.GetEnumDict("EDNames"));
            PageState.Add("EDPayType", SysEnumeration.GetEnumDict("EDPayType"));
        }
    }
}
