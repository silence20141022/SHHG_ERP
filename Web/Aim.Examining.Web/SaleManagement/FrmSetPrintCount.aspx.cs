using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.Data;
using System.Configuration;

namespace Aim.Examining.Web
{
    public partial class FrmSetPrintCount : ExamBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.AppSettings["ExamineDB"];
            if (RequestActionString == "setcount")
            {
                string count = RequestData.Get<string>("count");
                if (DataHelper.QueryValue<int>("select count(1) from " + db + "..PrintCount") == 0)
                {
                    DataHelper.ExecSql("insert into " + db + "..PrintCount values (newid()," + count + ")");
                }
                else
                {
                    DataHelper.ExecSql("update " + db + "..PrintCount set Count=" + count);
                }
            }
            else
            {
                PageState.Add("count", DataHelper.QueryValue<int>("select top 1 Count from " + db + "..PrintCount"));
            }
        }
    }
}
