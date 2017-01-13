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

namespace Aim.Examining.Web.PurchaseInvoiceList
{
    public partial class ProductInventoryEdit : ExamBasePage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.RequestAction)
            {
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:

                    //添加库存数量
                    string WarehouseId = RequestData.Get<string>("WarehouseId");
                    string WarehouseName = RequestData.Get<string>("WarehouseName");
                    string json = RequestData.Get<string>("json");
                    Dictionary<string, object> dic = null;

                    json = json.Substring(1, json.Length - 2);
                    string[] objarr = json.Replace("},{", "#").Split('#');

                    for (int i = 0; i < objarr.Length; i++)
                    {
                        if (objarr.Length == 1)
                        {
                            dic = FromJson(objarr[i]) as Dictionary<string, object>;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                dic = FromJson(objarr[i] + "}") as Dictionary<string, object>;
                            }
                            else if (i == objarr.Length - 1)
                            {
                                dic = FromJson("{" + objarr[i]) as Dictionary<string, object>;
                            }
                            else
                            {
                                dic = FromJson("{" + objarr[i] + "}") as Dictionary<string, object>;
                            }
                        }
                        if (dic != null)
                        {
                            StockInfo stoinfo = StockInfo.FindAllByProperties("ProductId", dic["Id"], "WarehouseId", WarehouseId).FirstOrDefault<StockInfo>();
                            if (stoinfo != null)
                            {
                                stoinfo.StockQuantity = (stoinfo.StockQuantity == null ? 0 : stoinfo.StockQuantity) + (dic.ContainsKey("Count") ? Convert.ToInt32(dic["Count"]) : 0);
                                stoinfo.DoUpdate();
                            }
                            else
                            {
                                stoinfo = new StockInfo
                                {
                                    ProductCode = dic["Code"] + "",
                                    ProductName = dic["Name"] + "",
                                    ProductId = dic["Id"] + "",
                                    WarehouseId = WarehouseId,
                                    WarehouseName = WarehouseName,
                                    StockQuantity = dic.ContainsKey("Count") ? Convert.ToInt32(dic["Count"]) : 0
                                };
                                stoinfo.DoCreate();
                            }
                        }
                    }

                    break;
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

