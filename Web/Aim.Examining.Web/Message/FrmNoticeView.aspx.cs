using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

using Aim.Common;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Aim.Examining.Web;
using System.Data.SqlClient;

namespace Aim.Examining.Web.Message
{
    public partial class FrmNoticeView : ExamBasePage
    {
        #region 变量

        protected string JsonData = "";

        #endregion

        #region 构造函数

        public FrmNoticeView()
        {
            IsCheckLogon = false;
        }

        #endregion;

        #region ASP.NET 事件


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsAsyncRequest)
            {
                this.SetMessage("数据异步步加载成功！");
            }
            else
            {
                this.SetMessage("数据同步加载成功！");
            }

            if (this.Request["Id"] != null)
            {
                Notice notice = Notice.Find(this.Request["Id"]);
                notice.ReadState = "1";
                notice.Save();
                this.SetFormData(notice);
            }
        }

        #endregion
    }
}
