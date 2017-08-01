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
using System.Configuration;
using System.Web.Script.Serialization;

namespace Aim.Examining.Web
{
    public partial class FrmDeliveryOrderEdit3 : ExamBasePage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string paids = String.Empty;   // 销售单Id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            paids = RequestData.Get<string>("paids");
            type = RequestData.Get<string>("type");

            DeliveryOrder ent = null;
            IList<string> strList = RequestData.GetList<string>("data");
            PageState.Add("Id", "");

            switch (this.RequestAction)
            {
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<DeliveryOrder>();
                    ent.PId = paids;

                    //自动生成流水号
                    ent.Number = DataHelper.QueryValue("select " + db + ".dbo.fun_getDeliveryNumber()") + "";

                    ent.DoCreate();

                    //更新销售单状态(放到保存的地方)
                    if (!string.IsNullOrEmpty(paids))
                    {
                        GetDeliveryState(strList, paids);
                    }

                    //添加出库商品信息
                    InsertPart(strList, ent.Id);

                    PageState["Id"] = ent.Id;
                    this.SetMessage("新建成功！");
                    break;
                default:
                    break;
            }

            if (RequestActionString == "getSalesman")
            {
                string cid = RequestData.Get<string>("CId");
                Customer customer = Customer.Find(cid);
                if (customer != null)
                {
                    PageState.Add("result", customer.MagUser);
                    PageState.Add("MagId", customer.MagId);
                    PageState.Add("Address", customer.Address);
                   // PageState.Add("Tel", customer.Tel);
                    PageState.Add("Code", customer.Code);
                }
            }
            else if (RequestActionString == "checkdata")
            {
                string productIds = RequestData.Get<string>("productIds");
                string WarehouseId = RequestData.Get<string>("WarehouseId");
                if (!string.IsNullOrEmpty(productIds))
                {
                    int count = DataHelper.QueryValue<int>("select count(1) from " + db + "..StockInfo where WarehouseId='" + WarehouseId + "' and ProductId in ('" + productIds.Replace(",", "','") + "')");
                    if (count != productIds.Split(',').Length)
                    {
                        PageState.Add("error", "所选商品在指定仓库里没有找到，请重新选择");
                    }
                }
            }
            //销售订单生成销售出库单
            else if (op == "c" && !string.IsNullOrEmpty(paids))
            {
                string[] oids = paids.Split(',');
                SaleOrder[] ents = SaleOrder.FindAllByPrimaryKeys(oids);

                if (ents.Length > 0)
                {
                    ents[0].Id = "";
                    ents[0].Number = "自动生成";
                    //ents[0].Remark = "";
                    ents[0].State = "";
                    ents[0].Child = "[]";
                    this.SetFormData(ents[0]);
                }
            }
            else if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = DeliveryOrder.Find(id);
                }

                this.SetFormData(ent);
                this.PageState.Add("State", ent.State);
            }
            PageState.Add("DeliveryMode", SysEnumeration.GetEnumDict("DeliveryMode"));
        }

        /// <summary>
        /// 添加出库商品信息
        /// </summary>
        private void InsertPart(IList<string> strList, string Did)
        {
            Dictionary<string, object> dic = null;
            DelieryOrderPart entPart = null;

            for (int i = 0; i < strList.Count; i++)
            {
                dic = FromJson(strList[i]) as Dictionary<string, object>;

                if (dic != null)
                {
                    //一个一个的添加
                    entPart = new DelieryOrderPart
                    {
                        DId = Did,
                        PId = dic["PId"] + "",
                        ProductId = dic["ProductId"] + "",
                        PName = dic["Name"] + "",
                        PCode = dic["Code"] + "",
                        Isbn = dic.ContainsKey("Isbn") ? dic["Isbn"] + "" : "",
                        Unit = dic.ContainsKey("Unit") ? dic["Unit"] + "" : null,
                        Count = dic.ContainsKey("OutCount") ? (int?)Convert.ToInt32(dic["OutCount"]) : null,
                        Remark = dic.ContainsKey("Remark") ? dic["Remark"] + "" : "",
                        State = "未出库",
                        Guids = dic.ContainsKey("Guids") ? dic["Guids"] + "" : "",
                        CreateId = UserInfo.UserID,
                        CreateName = UserInfo.Name,
                        CreateTime = DateTime.Now
                    };
                    entPart.DoCreate();
                }
            }
        }

        /// <summary>
        /// 更新出库状态
        /// </summary>
        /// <param name="childjson">出库单出库商品json</param>
        private void GetDeliveryState(IList<string> strList, string paids)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            Dictionary<string, object> dic = null;
            OrdersPart op = null;

            for (int i = 0; i < strList.Count; i++)
            {
                dic = FromJson(strList[i]) as Dictionary<string, object>;

                if (dic != null)
                {
                    op = OrdersPart.TryFind(dic["PId"]);
                    if (op != null)
                    {
                        op.OutCount = (op.OutCount == null ? 0 : Convert.ToInt32(op.OutCount)) + Convert.ToInt32(dic["OutCount"]);
                        op.DoSave();
                    }
                    else
                    {
                        //throw new Exception("PId未在OrdersPart的Id中");
                    }
                }
            }

            //更新销售单状态
            string[] oids = paids.Split(',');
            SaleOrder order = null;
            string count = "0";//待出库数量
            string state = "";
            OrdersPart[] ops = null;
            string jsons = "";
            foreach (string str in oids)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                count = DataHelper.QueryValue("select count(1) from " + db + "..OrdersPart where OId='" + str + "' and (OutCount is null or OutCount<>[Count])") + "";
                if (count == "0")
                {
                    //已出库state
                    //state = "已出库";
                    state = "已全部生成出库单";
                }
                else
                {
                    //state = "部分出库";
                    state = "部分生成出库单";
                }
                order = SaleOrder.TryFind(str);
                if (order != null)
                {
                    jsons = "";
                    order.DeliveryState = state;

                    //更新order的json
                    ops = OrdersPart.FindAllByProperty("OId", str);

                    //拼json
                    foreach (OrdersPart opt in ops)
                    {
                        jsons += "{";
                        jsons += "Id:'" + opt.PId + "',";
                        jsons += "Isbn:'" + opt.Isbn + "',";
                        jsons += "Code:'" + opt.PCode + "',";
                        jsons += "Name:'" + opt.PName + "',";
                        jsons += "Unit:'" + opt.Unit + "',";
                        jsons += "MinSalePrice:'" + opt.MinSalePrice + "',";
                        jsons += "Price:'" + opt.SalePrice + "',";
                        jsons += "Amount:'" + opt.Amount + "',";
                        jsons += "Count:'" + opt.Count + "',";
                        jsons += "OutCount:'" + opt.OutCount + "',";
                        jsons += "Remark:'" + opt.Remark + "'";
                        jsons += "},";
                    }

                    order.Child = "[" + jsons.Substring(0, jsons.Length - 1) + "]";

                    order.DoSave();
                }
            }
        }

        //把json转换为可识别的
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
        }
        #endregion

    }
}

