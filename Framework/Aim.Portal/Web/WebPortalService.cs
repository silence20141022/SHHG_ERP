﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;
using Aim.Common;
using Aim.Common.Service;
using Aim.Common.Authentication;
using Aim.Portal.Model;

namespace Aim.Portal.Web
{
    /// <summary>
    /// 处理Portal服务相关信息
    /// </summary>
    public class WebPortalService: PortalService
    {
        #region 成员

        public const string LOGOUT_PAGE = "~/Unlogin.aspx";

        #endregion

        #region 属性

        #endregion

        #region 构造函数

        protected WebPortalService()
            : base()
        {
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 登出并转向登录页面
        /// </summary>
        public static void LogoutAndRedirect()
        {
            string loginPage = String.Format("{0}?ReturnUrl={1}", System.Web.Security.FormsAuthentication.LoginUrl, HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));

            LogoutAndRedirect(loginPage);
        }

        /// <summary>
        /// 登出并转向指定页面
        /// </summary>
        /// <param name="page"></param>
        public static void LogoutAndRedirect(string page)
        {
            WebPortalService.Logout();

            HttpContext.Current.Response.Redirect(page);
        }

        /// <summary>
        /// 退出
        /// </summary>
        public static void Exit()
        {
            HttpContext.Current.Response.Redirect(LOGOUT_PAGE);
        }

        #endregion
    }
}
