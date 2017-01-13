// Business class SysUser generated from SysUser
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Portal.Model
{
    [Serializable]
    public partial class SysAuth
    {
        #region ˽�б���

        [NonSerialized]
        private SysAuth _Parent;

        [NonSerialized]
        private IList<SysAuth> _Children;

        private bool? _IsLeaf = null;

        [NonSerialized]
        private IList<SysGroup> _Group = new List<SysGroup>();

        [NonSerialized]
        private IList<SysRole> _Role = new List<SysRole>();

        [NonSerialized]
        private IList<SysUser> _User = new List<SysUser>();

        #endregion

        #region ��Ա����

        /// <summary>
        /// ����������飨���飩
        /// </summary>
        [JsonIgnore]
        public SysAuth Parent
        {
            get
            {
                if (!IsTop)
                {
                    _Parent = SysAuth.Find(this.ParentID);
                }

                return _Parent;
            }
        }

        /// <summary>
        /// �Ƿ񶥲��飨û�и��ڵ㣩
        /// </summary>
        public bool IsTop
        {
            get
            {
                return String.IsNullOrEmpty(this.ParentID);
            }
        }

        /// <summary>
        /// �Ƿ��ģ�飨û���ӽڵ㣩
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                if (_IsLeaf == null)
                {
                    bool isLeaf = !SysAuth.Exists("ParentID = ?", this.AuthID);
                    _IsLeaf = isLeaf;
                }

                return _IsLeaf.GetValueOrDefault();
            }
        }

        /// <summary>
        /// �������ӵ�е���ģ��
        /// </summary>
        [JsonIgnore]
        public IList<SysAuth> Children
        {
            get
            {
                if (_Children == null)
                {
                    _Children = SysAuth.FindAllByProperties("ParentID", this.ModuleID);
                }

                return _Children;
            }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysGroup), Table = "SysGroupPermission", ColumnRef = "GroupID", ColumnKey = "AuthID", Lazy = true)]
        public IList<SysGroup> Group
        {
            get { return _Group; }
            set { _Group = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysRole), Table = "SysRolePermission", ColumnRef = "RoleID", ColumnKey = "AuthID", Lazy = true)]
        public IList<SysRole> Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysUser), Table = "SysUserPermission", ColumnRef = "UserID", ColumnKey = "AuthID", Lazy = true)]
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
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            // ����ʼ
            this.CreateAndFlush();
        }

        /// <summary>
        /// �޸�ģ��(ͬʱ�޸�����Ϊ2����ӦȨ�޵�����)
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.UpdateAndFlush();
        }

        /// <summary>
        /// ɾ��ģ��
        /// </summary>
        public void DoDelete()
        {
            base.Delete();
        }

        /// <summary>
        /// ��ϵͳӦ�ô���Ȩ��
        /// </summary>
        public void CreateByApplication(string appid)
        {
            SysApplication app = SysApplication.Find(appid);

            this.CreateByApplication(app);
        }

        /// <summary>
        /// ��ϵͳӦ�ô���Ȩ��
        /// </summary>
        public void CreateByApplication(SysApplication app)
        {
            this.SetDataByApplication(app);  // ���ݶ�Ӧ��SysApplication��������

            this.DoCreate();
        }

        /// <summary>
        /// ��ϵͳӦ�ø���Ȩ��
        /// </summary>
        public void UpdateByApplication(string appid)
        {
            SysApplication app = SysApplication.Find(appid);

            this.UpdateByApplication(app);
        }

        /// <summary>
        /// ��ϵͳӦ�ø���Ȩ��
        /// </summary>
        public void UpdateByApplication(SysApplication app)
        {
            this.SetDataByApplication(app);  // ���ݶ�Ӧ��SysApplication��������

            this.DoUpdate();
        }

        /// <summary>
        /// ��ϵͳģ�鴴��Ȩ��
        /// </summary>
        public void CreateByModule(string mdlid)
        {
            SysModule mdl = SysModule.Find(mdlid);

            this.CreateByModule(mdl);
        }

        /// <summary>
        /// ��ϵͳģ�鴴��Ȩ��
        /// </summary>
        public void CreateByModule(SysModule mdl)
        {
            this.SetDataByModule(mdl);  // ���ݶ�Ӧ��SysModule��������

            this.DoCreate();
        }

        /// <summary>
        /// ��ϵͳģ�����Ȩ��
        /// </summary>
        public void UpdateByModule(string mdlid)
        {
            SysModule mdl = SysModule.Find(mdlid);
            this.UpdateByModule(mdl);
        }

        /// <summary>
        /// ��ϵͳģ�����Ȩ��
        /// </summary>
        public void UpdateByModule(SysModule mdl)
        {
            this.SetDataByModule(mdl);  // ���ݶ�Ӧ��SysModule��������

            this.DoUpdate();
        }

        /// <summary>
        /// ��ȡ��Ӧ�㼶�Ӽ�
        /// </summary>
        /// <returns></returns>
        public SysAuth[] GetSubs()
        {
            int currLevel = this.PathLevel.GetValueOrDefault();

            return SysAuth.FindAll("PathLevel > ?", currLevel);
        }

        /// <summary>
        /// ��ȡ��Ӧ�㼶�Ӽ�
        /// </summary>
        /// <param name="level">��ǰ�㼶���²㼶</param>
        /// <returns></returns>
        public SysAuth[] GetSubs(int level)
        {
            int currLevel = this.PathLevel.GetValueOrDefault();
            int maxLevel = this.PathLevel.GetValueOrDefault() + level;

            return SysAuth.FindAll("PathLevel > ? AND PathLevel <= ?", currLevel, maxLevel);
        }

        /// <summary>
        /// ��Ӷ���ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsTops()
        {
            this.Path = null;
            this.PathLevel = 0;

            this.DoCreate();
        }

        /// <summary>
        /// ����ֵ�ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSibs(string sibID)
        {
            SysAuth sib = SysAuth.Find(sibID);

            CreateAsSib(sib);
        }

        /// <summary>
        /// ����ֵ�ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSib(SysAuth sib)
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
            SysAuth parent = SysAuth.Find(parentID);

            CreateAsSub(parent);
        }

        /// <summary>
        /// �����ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSub(SysAuth parent)
        {
            this.ParentID = parent.AuthID;
            this.Path = String.Format("{0}.{1}", (parent.Path == null ? String.Empty : parent.Path), parent.AuthID);
            this.PathLevel = parent.PathLevel + 1;

            this.DoCreate();
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

        /// <summary>
        /// ��ȡ�ɷ���Ӧ��
        /// </summary>
        /// <returns></returns>
        public static IList<SysApplication> GetApplicationsByAuths(IEnumerable<SysAuth> auths)
        {
            IEnumerable<string> appids = auths.Where(v => v.Type == 1 && String.IsNullOrEmpty(v.ModuleID) && !String.IsNullOrEmpty(v.Data)).Select(v => v.Data);

            return SysApplication.FindAllByPrimaryKeys(appids.ToArray());
        }

        /// <summary>
        /// ��ȡ�ɷ���ģ��
        /// </summary>
        /// <returns></returns>
        public static IList<SysModule> GetModulesByAuths(IList<SysAuth> auths)
        {
            IEnumerable<string> mdlids = auths.Where(v => v.Type == 1 && !String.IsNullOrEmpty(v.ModuleID)).Select(v => v.ModuleID);

            return SysModule.FindAllByPrimaryKeys(mdlids.ToArray());
        }

        #endregion

        #region ˽�к���

        private void SetDataByApplication(SysApplication app)
        {
            this.Name = app.Name;
            this.Code = String.Format("AUTH_APP_{0}", app.Code);
            this.Data = app.ApplicationID;
            this.Path = ".";
            this.PathLevel = 0;
            this.ParentID = null;
            this.Type = 1;  // 1Ϊϵͳ���Ȩ��
            this.Description = String.Format("Ӧ�� {0} ����Ȩ��", app.Name);
        }

        private void SetDataByModule(SysModule mdl)
        {
            SysAuth[] pAuths;
            SysAuth pAuth = null;

            // ��ȡ��Ȩ�ޣ���ϵͳģ���Ӧ��
            if (!String.IsNullOrEmpty(mdl.ParentID))
            {
                pAuths = SysAuth.FindAllByProperties("ModuleID", mdl.ParentID);
            }
            else
            {
                pAuths = SysAuth.FindAllByProperties("Data", mdl.ApplicationID);
            }

            if (pAuths.Length > 0)
            {
                pAuth = pAuths[0];
                this.ParentID = pAuth.AuthID;
                this.Path = String.Format("{0}.{1}", pAuth.Path, pAuth.AuthID);
                this.PathLevel = (pAuth.PathLevel == null ? 0 : (pAuth.PathLevel + 1));
            }

            this.Name = mdl.Name;
            this.Code = String.Format("AUTH_MDL_{0}", mdl.Code);
            this.Type = 1;  // 1Ϊϵͳ���Ȩ��
            this.SortIndex = mdl.SortIndex;
            this.ModuleID = mdl.ModuleID;
            this.Description = String.Format("ģ�� {0} ����Ȩ��", mdl.Name);
        }

        #endregion
    } // SysAuth
}

