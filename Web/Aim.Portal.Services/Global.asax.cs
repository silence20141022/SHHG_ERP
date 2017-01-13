using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Aim.Aop;
using Aim.Data;
using Aim.Portal.Model;

namespace Aim.Portal.Services
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            InitApplication();
        }

        #region 初始化应用

        private void InitApplication()
        {
            EntityManager.InitializeEntity("Aim.Portal");

            SysSystem system = SysSystemRule.GetCurrentSystemInfo();

            if (system != null)
            {
                Application["SysSystem"] = system;
            }

            //日志
            log4net.Config.XmlConfigurator.Configure();

            //日志、异常
            LogAttribute.del += WriteLog;
            ExceptionAttribute.del += WriteLog;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        protected void WriteLog(string message)
        {
            //System.Web.HttpContext.Current.Response.Write(message);
        }

        #endregion

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            // 系统释放
            UserSessionServer.Instance.Dispose();
        }
    }
}