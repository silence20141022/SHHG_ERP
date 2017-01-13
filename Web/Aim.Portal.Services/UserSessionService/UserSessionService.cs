using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Castle.ActiveRecord;
using Aim.Security;
using Aim.Common;
using Aim.Common.Authentication;
using Aim.Common.Service;
using Aim.Data;
using Aim.Portal.Model;

namespace Aim.Portal.Services
{
    /// <summary>
    /// 用户Session服务实现
    /// </summary>
    public class UserSessionService : IUserSessionService
    {
        /// <summary>
        /// 用户Session服务提供对象
        /// </summary>
        public static UserSessionServer Server = UserSessionServer.Instance;

        static UserSessionService()
        {
        }

        #region IUserSessionService Members

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        public string AuthenticateUser(string authMsg)
        {
            try
            {
                return Server.AuthenticateUser(authMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 判断用户Session状态(判断用户是否已注销)
        /// </summary>
        /// <param name="sessionID">此用户的UserSession标识</param>
        /// <returns>true-有效  false-无效</returns>
        public bool CheckUserSession(string sessionID)
        {
            try
            {
                return Server.CheckUserSession(sessionID);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 用户注销或者页面超时时释放用户Session
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns>true-释放成功,false--释放失败</returns>
        public bool ReleaseSession(string sessionID)
        {
            try
            {
                return Server.ReleaseSession(sessionID, UserSessionEventEnum.Logout);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置预释放
        /// </summary>
        /// <param name="sessionID"></param>
        public bool SetPrepRelease(string sessionID)
        {
            try
            {
                Server.SetPrepRelease(sessionID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置预释放(包含登录模式)
        /// </summary>
        /// <param name="sessionID"></param>
        public bool SetPrepRelease(string sessionID, LoginTypeEnum logMode)
        {
            try
            {
                Server.SetPrepRelease(sessionID, logMode);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 刷新指定用户的状态信息,改变此用户最近的活动时间为当前时间
        /// </summary>
        /// <param name="sessionID"></param>
        public bool RefreshSession(string sessionID)
        {
            try
            {
                Server.RefreshSession(sessionID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetUserData(string msg)
        {
            try
            {
                OpMessage opMsg = new OpMessage(msg);
                opMsg.Lable = "GetUserData";

                return ExecuteServiceByMsgObj(opMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取系统数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetSystemData(string msg)
        {
            try
            {
                OpMessage opMsg = new OpMessage(msg);
                opMsg.Lable = "GetSystemData";

                return ExecuteServiceByMsgObj(opMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public byte[] ExecuteService(string msg)
        {
            try
            {
                OpMessage opMsg = new OpMessage(msg);

                return ExecuteServiceByMsgObj(new OpMessage(msg));
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 私有方法

        private byte[] ExecuteServiceByMsgObj(OpMessage opMsg)
        {
            try
            {
                byte[] data = null;
                Object dataObj = null;

                if (String.IsNullOrEmpty(opMsg.Operation))
                {
                    return null;
                }

                string label = (opMsg.Lable == null ? String.Empty : opMsg.Lable).ToLower();
                string op = (opMsg.Operation == null ? String.Empty : opMsg.Operation).ToLower();

                if (label == "getuserdata")
                {
                    using (new SessionScope())
                    {
                        UserLogonInfo logonInfo = Server.GetLogonInfo(opMsg.SessionID);
                        IList<string> ids = new List<string>();

                        if (logonInfo != null)
                        {
                            if (op == "getlogoninfo")
                            {
                                dataObj = logonInfo;
                            }
                            else if (op == "getalluserauth")
                            {
                                dataObj = logonInfo.User.RetrieveAllAuth();
                            }
                            else if (op == "getallusergroup")
                            {
                                dataObj = logonInfo.User.RetrieveAllGroup();
                            }
                            else if (op == "getalluserrole")
                            {
                                dataObj = logonInfo.User.RetrieveAllRole();
                            }
                            else if (op == "getalluserauthids")
                            {
                                IList<SysAuth> auths = logonInfo.User.RetrieveAllAuth();
                                IEnumerable<string> authIDs = auths.Select(ent => { return ent.AuthID; });

                                dataObj = new List<string>(authIDs);
                            }
                            else if (op == "getallusergroupids")
                            {
                                IList<SysGroup> grps = logonInfo.User.RetrieveAllGroup();
                                IEnumerable<string> grpIDs = grps.Select(ent => { return ent.GroupID; });

                                dataObj = new List<string>(grpIDs);
                            }
                            else if (op == "getalluserroleids")
                            {
                                IList<SysRole> roles = logonInfo.User.RetrieveAllRole();
                                IEnumerable<string> roleIDs = roles.Select(ent => { return ent.RoleID; });

                                dataObj = new List<string>(roleIDs);
                            }
                            else if (op == "getuserinfo")
                            {
                                dataObj = new SimpleUserInfo(logonInfo);
                            }
                            else if (op == "getsysuser")
                            {
                                dataObj = logonInfo.User;
                            }
                        }
                    }
                }
                else if (label == "getsystemdata")
                {
                    using (new SessionScope())
                    {
                        if (opMsg.Operation == "getallapplications")
                        {
                            dataObj = new List<SysApplication>(SysApplicationRule.FindAll());
                        }
                        else if (opMsg.Operation == "getallmodules")
                        {
                            dataObj = new List<SysModule>(SysModuleRule.FindAll());
                        }
                        else if (opMsg.Operation == "getallgroups")
                        {
                            dataObj = new List<SysGroup>(SysGroupRule.FindAll());
                        }
                        else if (opMsg.Operation == "getallusers")
                        {
                            dataObj = new List<SysUser>(SysUserRule.FindAll());
                        }
                        else if (opMsg.Operation == "getallroles")
                        {
                            dataObj = new List<SysRole>(SysRoleRule.FindAll());
                        }
                        else if (opMsg.Operation == "getallauths")
                        {
                            dataObj = new List<SysAuth>(SysAuthRule.FindAll());
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (op == "checkusersession")
                        {
                            dataObj = Server.CheckUserSession(opMsg.SessionID);
                        }
                        else if (op == "releasesession")
                        {
                            dataObj = Server.ReleaseSession(opMsg.SessionID);
                        }
                        else if (op == "setpreprelease")
                        {
                            if (opMsg["logmode"].Type != TypeCode.Empty)
                            {
                                Server.SetPrepRelease(opMsg.SessionID, (LoginTypeEnum)opMsg["logmode"].Value);
                            }
                            else
                            {
                                Server.SetPrepRelease(opMsg.SessionID);
                            }

                            Server.SetPrepRelease(opMsg.SessionID);
                        }
                        else if (op == "refreshsession")
                        {
                            Server.RefreshSession(opMsg.SessionID);
                        }
                    }
                    catch (Exception ex)
                    {
                        dataObj = false;
                    }
                }

                if (dataObj != null)
                {
                    data = ServiceHelper.SerializeToBytes(dataObj);
                }

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
