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
using System.Data.OleDb;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmOrdersInvoice : ExamBasePage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string ids = String.Empty;
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        string db = ConfigurationManager.AppSettings["ExamineDB"];

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            ids = RequestData.Get<string>("ids");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");

            OrderInvoice ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<OrderInvoice>();
                    ent.DoUpdate();
                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<OrderInvoice>();
                    ent.OId = ids;
                    ent.DoCreate();

                    IList<string> strList = RequestData.GetList<string>("data");

                    string[] oids = ids.Split(',');
                    SaleOrder[] ents = SaleOrder.FindAllByPrimaryKeys(oids);

                    if (ents.Length > 0)
                    {
                        foreach (SaleOrder soent in ents)
                        {
                            //更新订单开票状态及发票号码
                            soent.InvoiceState = getInvoiceState(strList, soent.Id, soent.Number);
                            soent.InvoiceNumber += ent.Number + ",";

                            soent.DoSave();
                        }
                    }

                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<OrderInvoice>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
                default:
                    if (RequestActionString == "")
                    { }
                    break;
            }

            //加载要开发票的数据
            if (op == "c" && !string.IsNullOrEmpty(ids))
            {
                string[] oids = ids.Split(',');

                SaleOrder[] ents = SaleOrder.FindAllByPrimaryKeys(oids);

                if (ents.Length > 0)
                {
                    DataTable dtall = null;
                    DataTable dtMain = null;
                    DataTable dtReturn = null;
                    DataTable dtChange = null;
                    string columns = "Id,OId,PId as ProductId,PCode as ProductCode,PName as ProductName,Isbn,[Count],Unit,SalePrice,Amount,Remark,BillingCount,'销售' as Type";
                    string columns2 = "Id,OId,ProductId,ProductCode,ProductName,Isbn,[Count],Unit,ReturnPrice as SalePrice,Amount,Remark,BillingCount";
                    foreach (SaleOrder soent in ents)
                    {
                        //partList.AddRange(OrdersPart.FindAllByProperty("OId", soent.Id));

                        //查询数据
                        dtMain = DataHelper.QueryDataTable("select " + columns + " from " + db + "..OrdersPart where OId='" + soent.Id + "'");
                        dtReturn = DataHelper.QueryDataTable("select " + columns2 + ",'退货' as Type from " + db + "..ReturnOrderPart where OId='" + soent.Id + "'");
                        dtChange = DataHelper.QueryDataTable("select " + columns2 + ",'换货' as Type from " + db + "..ChangeOrderPart where OId='" + soent.Id + "'");

                        //处理换货商品
                        DataTable dttemp = dtMain;
                        if (dtChange.Rows.Count > 0)
                        {
                            bool isfind = false;
                            foreach (DataRow charow in dtChange.Rows)
                            {
                                isfind = false;
                                foreach (DataRow mainrow in dtMain.Rows)
                                {
                                    if (charow["ProductId"] + "" == mainrow["ProductId"] + "")
                                    {
                                        isfind = true;
                                        //加上换货的商品数量
                                        //dttemp.Rows.Remove(mainrow);
                                        mainrow["Count"] = Convert.ToInt32(mainrow["Count"]) + Convert.ToInt32(charow["Count"]);
                                        mainrow["Amount"] = Convert.ToInt32(mainrow["Amount"]) + Convert.ToInt32(charow["Amount"]);
                                        //dttemp.Rows.Add(mainrow);
                                        break;
                                    }
                                }
                                //不在原商品列表里，添加新的
                                if (!isfind)
                                {
                                    dttemp.Rows.Add(charow.ItemArray);
                                }
                            }
                            dtMain = dttemp;
                        }

                        //处理退货商品
                        if (dtReturn.Rows.Count > 0)
                        {
                            foreach (DataRow rtnrow in dtReturn.Rows)
                            {
                                foreach (DataRow mainrow in dtMain.Rows)
                                {
                                    if (rtnrow["ProductId"] + "" == mainrow["ProductId"] + "")
                                    {
                                        //完全退货移除
                                        if (rtnrow["Count"] + "" == mainrow["Count"] + "")
                                        {
                                            dttemp.Rows.Remove(mainrow);
                                        }
                                        else
                                        {
                                            //dttemp.Rows.Remove(mainrow);
                                            mainrow["Count"] = Convert.ToInt32(mainrow["Count"]) - Convert.ToInt32(rtnrow["Count"]);
                                            mainrow["Amount"] = Convert.ToInt32(mainrow["Amount"]) - Convert.ToInt32(rtnrow["Amount"]);
                                            //dttemp.Rows.Add(mainrow);
                                        }
                                        break;
                                    }
                                }
                            }
                            dtMain = dttemp;
                        }

                        if (dtall == null)
                        {
                            dtall = dtMain;
                        }
                        else
                        {
                            dtall.Merge(dtMain);
                        }
                    }

                    #region old

                    //                    string sql = @"select OId,ProductId,ProductCode,ProductName,Isbn,sum([count]) as [Count],max(SalePrice) as SalePrice,sum(Amount) as Amount,Unit,sum(BillingCount) as BillingCount from(
                    //                                select Id,OId,PId as ProductId,PCode as ProductCode,PName as ProductName,Isbn,[Count],Unit,SalePrice,Amount,BillingCount from {0}..OrdersPart
                    //                                union all 
                    //                                select Id,OId,ProductId,ProductCode,ProductName,Isbn,[Count],Unit,0,Amount,BillingCount from {1}..ChangeOrderPart
                    //                                union all 
                    //                                select Id,OId,ProductId,ProductCode,ProductName,Isbn,([Count]*-1) as [Count],Unit,0,(Amount*-1) as Amount,BillingCount from {2}..ReturnOrderPart
                    //                                )t where OId in ('" + ids.Replace(",", "','") + "') group by ProductId,ProductCode,ProductName,Unit,Isbn,OId having sum([Count])>0";
                    //                    DataTable dt = DataHelper.QueryDataTable(string.Format(sql, db, db, db));

                    //                    PageState.Add("DetailList", DataHelper.DataTableToDictList(dt));

                    #endregion

                    PageState.Add("DetailList", DataHelper.DataTableToDictList(dtall));
                    ents[0].Child = "";
                    ents[0].Number = "";
                    this.SetFormData(ents[0]);
                }
            }
            else if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OrderInvoice.Find(id);
                }

                this.SetFormData(ent);
            }
        }

        /// <summary>
        /// 获取销售单开发票状态,并修改销售详细开票数量
        /// </summary>
        /// <param name="json">开票明细json</param>
        /// <param name="oid">销售单Id</param>
        /// <returns></returns>
        private string getInvoiceState(IList<string> strList, string oid, string onumber)
        {
            OrdersPart[] ops1 = OrdersPart.FindAllByProperty("OId", oid);//查出来的
            Dictionary<string, object> dic = null;

            //dtall 所有需要开发票的 dttemp 当前订单该开票的商品
            //DataTable dttemp = null;
            //DataRow[] rows = dtall.Select("OId='" + oid + "'");
            //dttemp = dtall.Clone();
            //foreach (DataRow row in rows)
            //{
            //    dttemp.Rows.Add(row.ItemArray);
            //}

            for (int i = 0; i < strList.Count; i++)
            {
                dic = FromJson(strList[i]) as Dictionary<string, object>;
                if (dic["Type"] + "" == "销售")
                {
                    foreach (OrdersPart op1 in ops1)
                    {
                        if (op1.Id == dic["Id"] + "")
                        {
                            if (op1.BillingCount == null)
                            {
                                op1.BillingCount = Convert.ToInt32(dic["BcCount"]);
                            }
                            else
                            {
                                op1.BillingCount += Convert.ToInt32(dic["BcCount"]);
                            }
                            op1.DoUpdate();
                            break;
                        }
                    }
                }
                else if (dic["Type"] + "" == "退货")
                {
                    //估计用不到
                }
                else if (dic["Type"] + "" == "换货")
                {
                    ChangeOrderPart[] chanparts = ChangeOrderPart.FindAllByProperty("OId", oid);

                    foreach (ChangeOrderPart chanpart in chanparts)
                    {
                        if (chanpart.Id == dic["Id"] + "")
                        {
                            if (chanpart.BillingCount == null)
                            {
                                chanpart.BillingCount = Convert.ToInt32(dic["BcCount"]);
                            }
                            else
                            {
                                chanpart.BillingCount += Convert.ToInt32(dic["BcCount"]);
                            }
                            chanpart.DoUpdate();
                            break;
                        }
                    }
                }

            }
            //查询 以获得state
            return DataHelper.QueryValue<string>("select " + db + ".dbo.fun_getWKPCount('" + oid + "')");
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

