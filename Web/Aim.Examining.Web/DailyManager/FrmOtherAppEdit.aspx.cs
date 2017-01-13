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

namespace Aim.Examining.Web
{
    public partial class FrmOtherAppEdit : ExamBasePage
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

            OtherCost ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<OtherCost>();
                    ent.DoUpdate();
                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<OtherCost>();

                    //自动生成流水号
                    ent.Number = DataHelper.QueryValue("select " + db + ".dbo.fun_getOtherCostsNumber()") + "";

                    ent.DoCreate();
                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<OtherCost>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
                default:
                    if (RequestActionString == "save")
                    {
                        DoSave();
                    }
                    else if (RequestActionString == "submitfinish")
                    {
                        OtherCost pc = OtherCost.Find(this.RequestData.Get<string>("id"));
                        pc.State = "End";
                        pc.AppState = this.RequestData.Get<string>("ApprovalState");
                        pc.Save();
                    }
                    break;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OtherCost.Find(id);
                }

                this.SetFormData(ent);
                this.PageState.Add("State", ent.State);
            }

            if (op == "c")
            {
                PageState.Add("ReleDepartment", DataHelper.QueryValue("select " + db + ".dbo.get_DeptName('" + UserInfo.UserID + "')"));
            }
            this.PageState.Add("FlowEnum", SysEnumeration.GetEnumDictList("WorkFlow.Simple"));
            PageState.Add("OtherCost", SysEnumeration.GetEnumDict("OtherCost"));
        }

        #endregion


        /// <summary>
        /// 保存操作
        /// </summary>
        private void DoSave()
        {
            OtherCost ent = null;

            id = FormData.Get<String>("Id");

            if (String.IsNullOrEmpty(id))
            {
                ent = this.GetPostedData<OtherCost>();
                ent.DoCreate();
            }
            else
            {
                ent = this.GetMergedData<OtherCost>();

                ent.DoUpdate();
            }

            this.SetFormData(ent);
        }

    }
}

