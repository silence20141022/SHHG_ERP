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
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmDeliveryOrder : ExamListPage
    {
        private IList<DeliveryOrder> ents = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestActionString == "batchdelete")
            {
                DoBatchDelete();
            }
            else if (RequestActionString == "markDelivery")
            {
                DeliveryOrder pc = DeliveryOrder.Find(this.RequestData.Get<string>("Id"));
                pc.State = "已出库";
                pc.Save();

                ////添加交易记录
                //TransactionHi his = new TransactionHi
                //{
                //    Child = pc.Child,
                //    CId = pc.CId,
                //    CName = pc.CName,
                //    CreateId = UserInfo.UserID,
                //    CreateName = UserInfo.Name,
                //    CreateTime = DateTime.Now,
                //    RtnOrOut = "购买",
                //    Number = pc.Number,
                //    TransactionTime = DateTime.Now
                //};
                //his.DoCreate();

                //自动计算客户余额
                //Customer custom = Customer.Find(pc.CId);
                //custom.PreDeposit = custom.PreDeposit - pc.TotalMoneyHis <= 0 ? 0 : custom.PreDeposit - pc.TotalMoneyHis;
                //custom.Save();

                //计算仓库数量
            }
            else if (RequestActionString == "print")
            {
                //Print();
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
            string db = ConfigurationManager.AppSettings["ExamineDB"];

            if (!SearchCriterion.Orders.Exists(en => en.PropertyName == "CreateTime"))
                SearchCriterion.Orders.Add(new OrderCriterionItem("CreateTime", false));

            string where = "";
            string wherec = " where 1=1 ";
            if (RequestData.Get<string>("type") == "yi")
            {
                where += " [State]='已出库' ";
                wherec += " and p.[State]='已出库' ";
            }
            else
            {
                where += " ([State] is null or [State]<>'已出库') ";
                wherec += " and (p.[State] is null or p.[State]<>'已出库') ";
            }

            CommonSearchCriterionItem item = SearchCriterion.Searches.Searches.Where(obj => obj.PropertyName == "Code").FirstOrDefault<CommonSearchCriterionItem>();
            SearchCriterion.Searches.RemoveSearch("Code");
            if (item != null && item.Value + "" != "")
            {
                ents = DeliveryOrder.FindAll(SearchCriterion, Expression.Sql(" Id in (select DId from " + db + "..DelieryOrderPart where PCode like '%" + item.Value + "%') and " + where));
            }
            else
            {
                ents = DeliveryOrder.FindAll(SearchCriterion, Expression.Sql(where));
            }
            this.PageState.Add("OrderList", ents);

            //查询详细产品的数量
            foreach (CommonSearchCriterionItem search in SearchCriterion.Searches.Searches)
            {
                wherec += " and p." + search.PropertyName + " like '%" + search.Value + "%'";
            }
            if (item != null && item.Value + "" != "")
            {
                wherec += " and c.PCode like '%" + item.Value + "%'";
            }
            PageState.Add("quantity", DataHelper.QueryValue("select sum([Count]) from " + db + "..DeliveryOrder p inner join " + db + "..DelieryOrderPart c on c.DId=p.Id" + wherec));
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchDelete()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");
            string db = ConfigurationManager.AppSettings["ExamineDB"];

            if (idList != null && idList.Count > 0)
            {
                OrdersPart op = null;
                OrdersPart[] ops = null;
                DelieryOrderPart[] dops = null;
                DeliveryOrder dorder = null;
                SaleOrder order = null;
                string count = "";

                //更新订单状态，同时删除DeliveryOrderPart表数据
                foreach (object obj in idList)
                {
                    dorder = DeliveryOrder.TryFind(obj);
                    if (dorder == null)
                    {
                        DelieryOrderPart.DeleteAll("DId='" + obj + "'"); //删除DeliveryOrderPart
                        continue;
                    }

                    dops = DelieryOrderPart.FindAllByProperty("DId", obj);
                    foreach (DelieryOrderPart dop in dops)
                    {
                        //更新订单状态
                        op = OrdersPart.TryFind(dop.PId);
                        if (op != null)
                        {
                            op.OutCount = (op.OutCount == null ? 0 : Convert.ToInt32(op.OutCount)) - Convert.ToInt32(dop.Count);
                            op.DoSave();
                        }
                    }

                    //更新order的json
                    string jsons = "";
                    order = SaleOrder.FindAllByProperty("Id", dorder.PId).FirstOrDefault<SaleOrder>();
                    if (order == null)
                    {
                        DelieryOrderPart.DeleteAll("DId='" + obj + "'"); //删除DeliveryOrderPart
                        continue;
                    }
                    ops = OrdersPart.FindAllByProperty("OId", order.Id);
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

                    count = DataHelper.QueryValue("select count(1) from " + db + "..OrdersPart where OId='" + order.Id + "' and OutCount=[Count]") + "";
                    if (count == "0")
                    {
                        order.DeliveryState = "";
                    }
                    else
                    {
                        order.DeliveryState = "部分生成出库单";
                    }

                    order.Child = "[" + jsons.Substring(0, jsons.Length - 1) + "]";
                    order.DoSave();

                    DelieryOrderPart.DeleteAll("DId='" + obj + "'"); //删除DeliveryOrderPart
                }

                DeliveryOrder.DoBatchDelete(idList.ToArray());
            }
        }

        //打印
        //public void Print()
        //{
        //    Microsoft.Office.Interop.Word.Application app = null;
        //    Microsoft.Office.Interop.Word.Document doc = null;

        //    DeliveryOrder order = DeliveryOrder.TryFind(RequestData.Get<string>("Id"));
        //    if (order == null)
        //        return;

        //    object missing = System.Reflection.Missing.Value;
        //    object templateFile = Server.MapPath("/WordTemplates/Deliveryorder.doc");

        //    try
        //    {
        //        app = new Microsoft.Office.Interop.Word.ApplicationClass();
        //        doc = app.Documents.Add(ref templateFile, ref missing, ref missing, ref missing);

        //        int count = 0;
        //        double amount = 0;

        //        try
        //        {
        //            //循环子商品
        //            string strjosn = order.Child.Substring(1, order.Child.Length - 2);
        //            string[] objarr = strjosn.Replace("},{", "#").Split('#');
        //            Dictionary<string, object> dic = null;

        //            if (objarr.Length > 0 && objarr[0] != "")
        //            {
        //                //插入一行 在document对象的集合操作中，起始点是从1开始
        //                Microsoft.Office.Interop.Word.Table table = doc.Tables[1];
        //                object beforeRow = null;
        //                int j = 0;
        //                for (int i = 0; i < objarr.Length; i++)
        //                {
        //                    if (objarr.Length == 1)
        //                    {
        //                        dic = FromJson(objarr[i]) as Dictionary<string, object>;
        //                    }
        //                    else
        //                    {
        //                        if (i == 0)
        //                        {
        //                            dic = FromJson(objarr[i] + "}") as Dictionary<string, object>;
        //                        }
        //                        else if (i == objarr.Length - 1)
        //                        {
        //                            dic = FromJson("{" + objarr[i]) as Dictionary<string, object>;
        //                        }
        //                        else
        //                        {
        //                            dic = FromJson("{" + objarr[i] + "}") as Dictionary<string, object>;
        //                        }
        //                    }

        //                    if (dic != null && dic.ContainsKey("OutCount") && Convert.ToInt32(dic["OutCount"]) > 0)
        //                    {
        //                        beforeRow = table.Rows[2];
        //                        table.Rows.Add(ref beforeRow);
        //                        if (j == 0)
        //                        {
        //                            table.Rows[3].Delete();
        //                            j++;
        //                        }
        //                        table.Cell(2, 1).Range.Text = dic["Name"] + "";
        //                        table.Cell(2, 2).Range.Text = dic["Code"] + "";
        //                        table.Cell(2, 3).Range.Text = dic.ContainsKey("Unit") ? dic["Unit"] + "" : "";
        //                        table.Cell(2, 4).Range.Text = dic["OutCount"] + "";
        //                        table.Cell(2, 5).Range.Text = dic.ContainsKey("Price") ? dic["Price"] + "" : "";
        //                        table.Cell(2, 6).Range.Text = (Convert.ToDouble(dic["Price"]) * Convert.ToInt32(dic["OutCount"])) + "";
        //                        table.Cell(2, 7).Range.Text = dic["Remark"] + "";
        //                        table.Cell(2, 8).Range.Text = DateTime.Now.ToString("yyyy-MM-dd");

        //                        count += Convert.ToInt32(dic["OutCount"]);
        //                        amount += dic.ContainsKey("Price") ? Convert.ToDouble(dic["Price"]) : 0;
        //                    }
        //                }
        //            }

        //            #region 填充word信息
        //            foreach (Microsoft.Office.Interop.Word.Bookmark bm in doc.Bookmarks)
        //            {
        //                bm.Select();

        //                string item = bm.Name;
        //                switch (item)
        //                {
        //                    case "bianhao":
        //                        bm.Range.Text = order.Number;
        //                        break;
        //                    case "ckdh":
        //                        bm.Range.Text = "出库单号";
        //                        break;
        //                    case "dizhi":
        //                        bm.Range.Text = order.Address;
        //                        break;
        //                    case "fphm":
        //                        bm.Range.Text = "发票号码";
        //                        break;
        //                    case "ghdw":
        //                        bm.Range.Text = order.CName;
        //                        break;
        //                    case "jhfs":
        //                        bm.Range.Text = order.DeliveryMode;
        //                        break;
        //                    case "kdy":
        //                        bm.Range.Text = UserInfo.Name;
        //                        break;
        //                    case "lxdh":
        //                        bm.Range.Text = order.Tel;
        //                        break;
        //                    case "riqi":
        //                        bm.Range.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //                        break;
        //                    case "xsjl":
        //                        bm.Range.Text = "销售经理";
        //                        break;
        //                    case "ywy":
        //                        bm.Range.Text = order.Salesman;
        //                        break;
        //                    case "zhaiyao":
        //                        bm.Range.Text = "摘要";
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //        //打印
        //        doc.PrintOut(ref missing, ref missing, ref missing, ref missing,
        //             ref missing, ref missing, ref missing, ref missing, ref missing,
        //             ref missing, ref missing, ref missing, ref missing, ref missing,
        //             ref missing, ref missing, ref missing, ref missing);

        //        //doc.Save();
        //    }
        //    catch (Exception exp)
        //    {
        //        throw new Exception(exp.Message);
        //    }
        //    //销毁word进程
        //    finally
        //    {
        //        object saveChange = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
        //        if (doc != null)
        //            doc.Close(ref saveChange, ref missing, ref missing);

        //        if (app != null)
        //            app.Quit(ref missing, ref missing, ref missing);
        //    }
        //}


        //把json转换为可识别的
        public object FromJson(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<object>(json);
        }

    }
}
