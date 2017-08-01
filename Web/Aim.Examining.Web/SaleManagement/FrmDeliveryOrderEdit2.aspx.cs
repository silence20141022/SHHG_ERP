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
    public partial class FrmDeliveryOrderEdit2 : ExamBasePage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string paid = String.Empty;   // 销售单Id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            paid = RequestData.Get<string>("paid");
            type = RequestData.Get<string>("type");

            DeliveryOrder ent = null;
            IList<string> strList = RequestData.GetList<string>("data");

            switch (this.RequestAction)
            {
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<DeliveryOrder>();
                    ent.PId = paid;

                    //自动生成流水号
                    ent.Number = DataHelper.QueryValue("select " + db + ".dbo.fun_getDeliveryNumber()") + "";

                    ent.DoCreate();

                    //添加出库商品信息
                    InsertPart(strList, ent.Id);

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
                    //PageState.Add("Tel", customer.Tel);
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
            else if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = DeliveryOrder.Find(id);
                }

                this.SetFormData(ent);
                this.PageState.Add("State", ent.State);

                if (!String.IsNullOrEmpty(id))
                {
                    //查询子商品
                    string sql = "select Id, PCode as Code, PName as Name, ProductId,Isbn, Guids, Count as OutCount, Unit, Remark from " + db + "..DelieryOrderPart where DId='" + id + "'";
                    PageState.Add("DetailList", DataHelper.QueryDictList(sql));
                }
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

        //把json转换为可识别的
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
        }

        #endregion

    }
}

