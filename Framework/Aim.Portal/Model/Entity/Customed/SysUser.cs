// Business class SysUser generated from SysUser
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Aim.Data;
	
namespace Aim.Portal.Model
{
    /// <summary>
    /// ϵͳ�û���
    /// </summary>
    [Serializable]
	public partial class SysUser
    {
        #region ˽�г�Ա

        [NonSerialized]
        private IList<SysAuth> _AllAuth = null;

        [NonSerialized]
        private IList<SysGroup> _AllGroup = null;

        [NonSerialized]
        private IList<SysRole> _AllRole = null;

        [NonSerialized]
        private IList<SysGroup> _Group = new List<SysGroup>();

        [NonSerialized]
        private IList<SysAuth> _Auth = new List<SysAuth>();

        [NonSerialized]
        private IList<SysRole> _Role = new List<SysRole>();

        #endregion

        #region ��Ա����

        /// <summary>
        /// ��������
        /// </summary>
        [JsonIgnore]
        public IList<SysAuth> AllAuth
        {
            get
            {
                if (_AllAuth == null)
                {
                    _AllAuth = this.RetrieveAllAuth();
                }

                return _AllAuth;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        [JsonIgnore]
        public IList<SysGroup> AllGroup
        {
            get
            {
                if (_AllGroup == null)
                {
                    _AllGroup = this.RetrieveAllGroup();
                }

                return _AllGroup;
            }
        }

        /// <summary>
        /// ���н�ɫ
        /// </summary>
        [JsonIgnore]
        public IList<SysRole> AllRole
        {
            get
            {
                if (_AllRole == null)
                {
                    _AllRole = this.RetrieveAllRole();
                }

                return _AllRole;
            }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysGroup), Table = "SysUserGroup", ColumnRef = "GroupID", ColumnKey = "UserID", Lazy = true)]
        public IList<SysGroup> Group
        {
            get
            {
                int i = _Group.Count;
                return _Group;
            }
            set { _Group = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysAuth), Table = "SysUserPermission", ColumnRef = "AuthID", ColumnKey = "UserID", Lazy = true)]
        public IList<SysAuth> Auth
        {
            get
            {
                int i = _Auth.Count;
                return _Auth;
            }
            set { _Auth = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysRole), Table = "SysUserRole", ColumnRef = "RoleID", ColumnKey = "UserID", Lazy = true)]
        public IList<SysRole> Role
        {
            get
            {
                int i = _Role.Count; 
                return _Role;
            }
            set { _Role = value; }
        }

        #endregion

        #region ����

        public override void Create()
        {
            this.CreateDate = DateTime.Now;

            base.Create();
        }

        public override void CreateAndFlush()
        {
            this.CreateDate = DateTime.Now;

            base.CreateAndFlush();
        }

        public override void Update()
        {
            this.LastModifiedDate = DateTime.Now;

            base.Update();
        }

        public override void UpdateAndFlush()
        {
            this.LastModifiedDate = DateTime.Now;

            base.UpdateAndFlush();
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique("LoginName"))
            {
                throw new RepeatedKeyException("�����ظ��� ��½�� ��" + this.LoginName + "��");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            // ����ʼ
            this.CreateAndFlush();
        }

        /// <summary>
        /// �޸Ĳ���
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.UpdateAndFlush();
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        public void DoDelete()
        {
            base.Delete();
        }

        /// <summary>
        /// �Ƿ�ӵ��ĳȨ��
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public bool HasPermission(SysAuth auth)
        {
            return AllAuth.Contains(auth);
        }

        /// <summary>
        /// �Ƿ���ĳ��ɫ
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsRole(SysRole role)
        {
            return this.Role.Contains(role);
        }

        /// <summary>
        /// �Ƿ��ǹ���Ա
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            foreach (SysRole role in this.Role)
            {
                if (role.RoleID == "0")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// �Ƿ�����ĳ����
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool IsGroup(SysGroup group)
        {
            return this.Group.Contains(group);
        }

        /// <summary>
        /// ��ȡ�û�������(/*��������ĳ�������ǣ��㽫�������и��飨�ݲ����ǣ�*/)
        /// </summary>
        /// <returns></returns>
        public IList<SysGroup> RetrieveAllGroup()
        {
            IList<SysGroup> groups = this.Group;

            List<string> groupIDs = new List<string>();
            foreach (SysGroup sg in groups)
            {
                if (sg.Path != null)
                {
                    string[] paths = sg.Path.Split('.');
                    foreach (string p in paths)
                    {
                        if (!groupIDs.Contains(p))
                        {
                            groupIDs.Add(p);
                        }
                    }
                }
            }

            ICriterion hqlCriterion = Expression.In("GroupID", groupIDs);

            groups = (SysGroup[])SysGroup.FindAll(hqlCriterion);

            return groups;
        }

        /// <summary>
        /// ��ȡ�û����������н�ɫ(�Դ���ɫ+���ɫ)
        /// </summary>
        /// <returns></returns>
        public IList<SysRole> RetrieveAllRole()
        {
            IList<SysRole> roles = new List<SysRole>();
            
            foreach (SysRole tRole in this.Role)
            {
                roles.Add(tRole);
            }
            //by huo 
            foreach (SysGroup tGroup in this.AllGroup)
            {
                foreach (SysRole tRole in tGroup.Role)
                {
                    if (!roles.Contains(tRole))
                    {
                        roles.Add(tRole);
                    }
                } 
            }
            
            return roles;
        }

        /// <summary>
        /// ��ȡ�û���ӵ������Ȩ��(�Դ�Ȩ��+�Դ���ɫȨ��+��Ȩ��)
        /// </summary>
        /// <returns></returns>
        public IList<SysAuth> RetrieveAllAuth()
        {
            IList<SysAuth> auths = new List<SysAuth>();

            foreach (SysAuth tAuth in this.Auth)
            {
                auths.Add(tAuth);
            }

            foreach (SysRole tRole in this.AllRole)
            {
                foreach (SysAuth tAuth in tRole.Auth)
                {
                    if (!auths.Contains(tAuth))
                    {
                        auths.Add(tAuth);
                    }
                }
            }

            foreach (SysGroup tGroup in this.AllGroup)
            {
                foreach (SysAuth tAuth in tGroup.Auth)
                {
                    if (!auths.Contains(tAuth))
                    {
                        auths.Add(tAuth);
                    }
                }
            }

            return auths;
        }

        /// <summary>
        /// ��ȡ�ɷ��ʵ�Ӧ��
        /// </summary>
        /// <returns></returns>
        public IList<SysApplication> GetAccessibleApplications()
        {
            return SysAuth.GetApplicationsByAuths(this.AllAuth);
        }

        /// <summary>
        /// ��ȡ�ɷ�����ģ��
        /// </summary>
        /// <returns></returns>
        public IList<SysModule> GetAccessibleModules()
        {
            return SysAuth.GetModulesByAuths(this.AllAuth);
        }

        #endregion

        #region ��̬����

        /// <summary>
        /// ���ݵ�¼����ȡSysUser
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static SysUser Get(string loginName)
        {
            if (String.IsNullOrEmpty(loginName) || !SysUser.Exists(Expression.Eq(SysUser.Prop_LoginName, loginName)))
            {
                return null;
            }
            else
            {
                return SysUser.FindFirstByProperties(SysUser.Prop_LoginName, loginName);
            }
        }

        /// <summary>
        /// �����������ɵ�¼��
        /// </summary>
        /// <returns></returns>
        public static string NewLoginNameFromChineseName(string chineseName)
        {
            string pyName = StringHelper.ConvertChineseToPY(chineseName);
            string loginName = pyName;

            // �Ѿ����ڴ��û��� (��Ϊ�������ʽ�С���������ѭ����ʽ����)
            if (SysUser.Exists("LoginName=?", loginName))
            {
                int i = 1;

                loginName = String.Format("{0}_{1}", loginName, i);
                while (SysUser.Exists("LoginName=?", loginName))
                {
                    loginName = String.Format("{0}_{1}", loginName, i);
                    i++;
                }
            }

            return loginName;
        }

        #endregion

    } // SysUser
}

