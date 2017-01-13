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
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Data.OleDb;
using System.Data;
namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class PurchaseOrderView : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型      
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            DoSelect();
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    string sql = @"select A.*,B.SupplierName,B.MoneyType,B.Symbo from SHHG_AimExamine..PurchaseOrder as A 
                    left join SHHG_AimExamine..Supplier as B on A.SupplierId=B.Id where A.Id='" + id + "'";
                    IList<EasyDictionary> ents = DataHelper.QueryDictList(sql);
                    this.SetFormData(ents[0]);
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.* from SHHG_AimExamine..PurchaseOrderDetail as A
                where A.PurchaseOrderId='{0}' order by A.Code asc";
                sql = string.Format(sql, id);
                IList<EasyDictionary> podDics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", podDics);
            }
        }
    }
}

