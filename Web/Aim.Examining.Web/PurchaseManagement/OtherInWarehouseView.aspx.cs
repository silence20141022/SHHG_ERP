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
    public partial class OtherInWarehouseView : ExamBasePage
    {
        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型    
        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            IList<string> entStrList = RequestData.GetList<string>("data");
            InWarehouse ent = null;
            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    string sql = @"select A.*,C.Name from SHHG_AimExamine..InWarehouse as A 
                    left join SHHG_AimExamine..Warehouse as C on A.WarehouseId=C.Id where A.Id='{0}'";
                    sql = string.Format(sql, id);
                    IList<EasyDictionary> Dics = DataHelper.QueryDictList(sql);
                    if (Dics.Count > 0)
                    {
                        this.SetFormData(Dics[0]);
                    }
                }
            }
        }
        private void DoSelect()
        {
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"select A.*,(A.Quantity-SHHG_AimExamine.dbo.fun_ActuallyHaveInQuantity(A.Id,B.InWarehouseType)) as NoInQuan
                from SHHG_AimExamine..OtherInWarehouseDetail as A
                left join SHHG_AimExamine..InWarehouse as B on B.Id=A.InWarehouseId
                where A.InWarehouseId='{0}' order by ProductCode";
                sql = string.Format(sql, id);
                IList<EasyDictionary> podDics = DataHelper.QueryDictList(sql);
                PageState.Add("DetailList", podDics);
            }
            PageState.Add("InWarehouseType", SysEnumeration.GetEnumDict("InWarehouseType"));
        }
    }
}

