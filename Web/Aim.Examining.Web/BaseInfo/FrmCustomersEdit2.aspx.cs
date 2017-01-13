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
using System.Configuration;

namespace Aim.Examining.Web.BaseInfo
{
    public partial class FrmCustomersEdit2 : ExamBasePage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            int count = 0;
            Customer ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Customer>();
                    ent.DoUpdate();
                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<Customer>();
                    ent.DoCreate();
                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Customer>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
                default:
                    if (RequestActionString == "VerificationCode")
                    {
                        string Code = RequestData.Get<string>("code");
                        count = DataHelper.QueryValue<int>("select count(1) from " + db + "..Customers where Code='" + Code + "' and Id<>'" + id + "'");
                        if (count > 0)
                        {
                            PageState.Add("error", "该客户重复");
                        }
                    }
                    break;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = Customer.Find(id);
                }
                this.SetFormData(ent);
            }

            PageState.Add("CustomImportant", SysEnumeration.GetEnumDict("CustomImportant"));
            PageState.Add("CustomType", SysEnumeration.GetEnumDict("CustomType"));
        }

        #endregion
    }
}

