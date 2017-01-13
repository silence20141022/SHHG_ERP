// Business class SysUser generated from SysUser
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using NHibernate.Criterion;
using NHibernate.Transform;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Aim.Data;
	
namespace Aim.Portal.Model
{
    [Serializable]
    public partial class SysGroup : TreeNodeEntityBase<SysGroup>
    {
        #region ˽�б���

        [NonSerialized]
        private SysGroup _ParentGroup;

        [NonSerialized]
        private IList<SysGroup> _AllGroups = new List<SysGroup>();

        [NonSerialized]
        private IList<SysGroup> _ChildGroups = new List<SysGroup>();

        private bool _isLeaf = false;
        private string _aim_Filter = "";


        [NonSerialized]
        private IList<SysAuth> _Auth = new List<SysAuth>();

        [NonSerialized]
        private IList<SysRole> _Role = new List<SysRole>();

        [NonSerialized]
        private IList<SysUser> _User = new List<SysUser>();

        #endregion

        #region ��Ա����

        //�������������չ
        public string Aim_Filter
        {
            get { return _aim_Filter; }
            set { _aim_Filter = value; }
        }

        //�Ƿ�Ҷ�ӽڵ��ж�
        public override bool? IsLeaf
        {
            get { return !SysGroup.Exists("ParentID = ?", this.GroupID); }
            set { _isLeaf = value.GetValueOrDefault(); }
        }

        /// <summary>
        /// ����������飨���飩
        /// </summary>
        public SysGroup ParentGroup
        {
            get 
            {
                if (!IsTopGroup)
                {
                    _ParentGroup = SysGroup.Find(this.ParentID);
                }

                return _ParentGroup;
            }
        }

        /// <summary>
        /// �Լ������и�����
        /// </summary>
        [JsonIgnore]
        public IList<SysGroup> AllGroup
        {
            get
            {
                _AllGroups = this.RetrieveAllGroup();
                return _AllGroups;
            }
        }

        /// <summary>
        /// �Ƿ񶥲��飨û�и��ڵ㣩
        /// </summary>
        public bool IsTopGroup
        {
            get
            {
                return String.IsNullOrEmpty(this.ParentID);
            }
        }

        /// <summary>
        /// �������ӵ�е�����(������)
        /// </summary>
        [JsonIgnore]
        [HasMany(typeof(SysGroup), Table = "SysGroup", ColumnKey = "ParentID", Lazy = true)]
        public IList<SysGroup> ChildGroups
        {
            get { return _ChildGroups; }
            set { _ChildGroups = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysAuth), Table = "SysGroupPermission", ColumnRef = "AuthID", ColumnKey = "GroupID", Lazy = true)]
        public IList<SysAuth> Auth
        {
            get { return _Auth; }
            set { _Auth = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysRole), Table = "SysGroupRole", ColumnRef = "RoleID", ColumnKey = "GroupID", Lazy = true)]
        public IList<SysRole> Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysUser), Table = "SysUserGroup", ColumnRef = "UserID", ColumnKey = "GroupID", Lazy = true)]
        public IList<SysUser> User
        {
            get { return _User; }
            set { _User = value; }
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("�����ظ��� ��� ��" + this.Code + "��");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();
            this.CreateDate = DateTime.Now;

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

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        #region ����

        [ActiveRecordTransaction]
        public override void Delete()
        {
            // ɾ�����ڵ㼰�ӽڵ��Լ�����ӽڵ�����
            string sql = String.Format("DELETE FROM SysUserGroup WHERE GroupID IN (SELECT GroupID FROM SysGroup WHERE Path LIKE '%{0}%' OR GroupID = '{0}')"
                + "DELETE FROM SysGroup WHERE Path LIKE '%{0}%' OR GroupID = '{0}'; ", this.GroupID);

            if (!String.IsNullOrEmpty(this.ParentID))
            {
                //sql += String.Format("UPDATE SysParameterCatalog SET IsLeaf = 1 WHERE ParameterCatalogID = '{0}' AND NOT EXISTS (SELECT ParameterCatalogID FROM SysParameterCatalog WHERE ParentID = '{0}')", this.ParentID);
            }

            DataHelper.QueryValue(sql);
        }

        #endregion

        /// <summary>
        /// ɾ������
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        /// <summary>
        /// ����ɾ������
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            SysGroup[] tents = SysGroup.FindAllByPrimaryKeys(args);

            foreach (SysGroup tent in tents)
            {
                tent.DoDelete();
            }
        }

        /// <summary>
        /// �Ƿ�ӵ��ĳȨ��
        /// </summary>
        /// <param name="role"></param>
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
            return AllRole.Contains(role);
        }

        private ReadOnlyCollection<SysAuth> m_allAuth = null;

        /// <summary>
        /// ������Ȩ��(�Դ�Ȩ��+���и���Ȩ��+���н�ɫȨ��)
        /// </summary>
        [JsonIgnore]
        public ReadOnlyCollection<SysAuth> AllAuth
        {
            get
            {
                if (m_allAuth == null)
                {
                    m_allAuth = new ReadOnlyCollection<SysAuth>(RetrieveAllAuth());
                }

                return m_allAuth;
            }
        }

        private ReadOnlyCollection<SysRole> m_allRole = null;

        /// <summary>
        /// �����н�ɫ(���н�ɫ+���и����ɫ)
        /// </summary>
        [JsonIgnore]
        public ReadOnlyCollection<SysRole> AllRole
        {
            get
            {
                if (m_allRole == null)
                {
                    m_allRole = new ReadOnlyCollection<SysRole>(RetrieveAllRole());
                }

                return m_allRole;
            }
        }

        /// <summary>
        /// ��ȡ��������飨�Լ��������游����)
        /// </summary>
        /// <returns></returns>
        private IList<SysGroup> RetrieveAllGroup()
        {
            IList<SysGroup> groups = new List<SysGroup>();

            if (!this.IsTopGroup)
            {
                string[] groupIDs = this.Path.Split('.');
                ICriterion hqlCriterion = Expression.In("GroupID", groupIDs);
                groups = SysGroup.FindAll(hqlCriterion);
            }

            return groups;
        }

        /// <summary>
        /// ��ȡ��ǰ�����н�ɫ
        /// </summary>
        /// <returns></returns>
        private IList<SysRole> RetrieveAllRole()
        {
            IList<SysRole> roles = new List<SysRole>();

            foreach (SysRole tRole in this.Role)
            {
                roles.Add(tRole);
            }

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
        /// ��ȡ�û���ӵ������Ȩ��(�Դ���ɫ+���ɫ)
        /// </summary>
        /// <returns></returns>
        private IList<SysAuth> RetrieveAllAuth()
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
        /// ��ȡ��Ӧ�㼶��ģ��
        /// </summary>
        /// <returns></returns>
        public SysModule[] GetSubs()
        {
            int currLevel = this.PathLevel.GetValueOrDefault();
            ICriterion crit = Expression.Gt("PathLevel", currLevel);

            return SysModule.FindAll(crit);
        }

        /// <summary>
        /// ��ȡ��Ӧ�㼶��ģ��
        /// </summary>
        /// <param name="level">��ǰ�㼶���²㼶</param>
        /// <returns></returns>
        public SysModule[] GetSubs(int level)
        {
            int currLevel = this.PathLevel.GetValueOrDefault();
            int maxLevel = this.PathLevel.GetValueOrDefault() + level;

            DetachedCriteria crits = DetachedCriteria.For<SysModule>();

            crits.Add(Expression.Gt("PathLevel", currLevel));
            crits.Add(Expression.Le("PathLevel", maxLevel));

            return SysModule.FindAll(crits);
        }

        /// <summary>
        /// ��Ӷ���ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsTop()
        {
            this.Path = null;
            this.PathLevel = 0;

            this.DoCreate();
        }

        /// <summary>
        /// ����ֵ�ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSib(string sibID)
        {
            SysGroup sib = SysGroup.Find(sibID);

            CreateAsSib(sib);
        }

        /// <summary>
        /// ����ֵ�ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSib(SysGroup sib)
        {
            this.ParentID = sib.ParentID;
            this.Path = sib.Path;
            this.PathLevel = sib.PathLevel;

            this.DoCreate();
        }

        /// <summary>
        /// �����ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSub(string parentID)
        {
            SysGroup parent = SysGroup.Find(parentID);

            CreateAsSub(parent);
        }

        /// <summary>
        /// �����ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSub(SysGroup parent)
        {
            this.ParentID = parent.GroupID;
            this.Path = String.Format("{0}.{1}", (parent.Path == null ? String.Empty : parent.Path), parent.GroupID);
            this.PathLevel = parent.PathLevel + 1;

            this.DoCreate();
        }

        /// <summary>
        /// �Ƴ��û�
        /// </summary>
        /// <param name="usrs"></param>
        public void RemoveUsers(IEnumerable<SysUser> usrs)
        {
            IEnumerable<string> idsToRemove = usrs.Select(ent => ent.UserID);

            this.RemoveUsers(idsToRemove);
        }

        /// <summary>
        /// �����û�ID�Ƴ�����Ա
        /// </summary>
        /// <param name="usrIDs"></param>
        public void RemoveUsers(IEnumerable<string> usrIDs)
        {
            IEnumerable<SysUser> usrsToRemove = this.User.Where(ent => usrIDs.Contains(ent.UserID));

            while (usrsToRemove.Count() > 0)
            {
                this.User.Remove(usrsToRemove.First());
            }
        }

        /// <summary>
        /// �����û�Id������Ա
        /// </summary>
        /// <param name="usrIDs"></param>
        public void AddUsers(IEnumerable<string> usrIDs)
        {
            IEnumerable<string> currIDs = this.User.Select(ent => ent.UserID);
            IEnumerable<string> idsToAdd = usrIDs.Where(ent => !currIDs.Contains(ent));

            if (idsToAdd != null && idsToAdd.Count() > 0)
            {
                string[] idListToAdd = idsToAdd.ToArray<string>();

                SysUser[] usrsToAdd = SysUser.FindAll(Expression.In("UserID", idListToAdd));

                foreach (SysUser usr in usrsToAdd)
                {
                    this.User.Add(usr);
                }
            }
        }

        /// <summary>
        /// �������Ա
        /// </summary>
        /// <param name="usrs"></param>
        public void AddUsers(IEnumerable<SysUser> usrs)
        {
            IEnumerable<string> currIDs = this.User.Select(ent => ent.UserID);

            IEnumerable<SysUser> usrsToAdd = usrs.Where(ent => !currIDs.Contains(ent.UserID));

            foreach (SysUser usr in usrsToAdd)
            {
                this.User.Add(usr);
            }
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

        #region ��̬��Ա

        /// <summary>
        /// �ɱ����ȡEnumeration
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static SysGroup Get(string code)
        {
            SysGroup[] tents = SysGroup.FindAllByProperty(SysGroup.Prop_Code, code);
            if (tents != null && tents.Length > 0)
            {
                return tents[0];
            }

            return null;
        }



        #endregion

    } // SysUser
}

