// Business class SysUser generated from SysUser
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Aim.Data;
	
namespace Aim.Portal.Model
{
    [Serializable]
    public partial class SysModule
    {
        #region ˽�б���

        [NonSerialized]
        private SysModule _Parent;

        [NonSerialized]
        private IList<SysModule> _Children;

        private bool? _IsLeaf = null;

        #endregion

        #region ��Ա����
        
        /// <summary>
        /// ����������飨���飩
        /// </summary>
        [JsonIgnore]
        public SysModule Parent
        {
            get
            {
                if (!IsTop)
                {
                    _Parent = SysModule.Find(this.ParentID);
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
                    ICriterion cirt = Expression.Eq("ParentID", this.ModuleID);
                    bool isLeaf = !SysModule.Exists(cirt);
                    _IsLeaf = isLeaf;
                }

                return _IsLeaf.GetValueOrDefault();
            }
        }

        /// <summary>
        /// �������ӵ�е���ģ��
        /// </summary>
        [JsonIgnore]
        public IList<SysModule> Children
        {
            get
            {
                if (_Children == null)
                {
                    _Children = SysModule.FindAllByProperties("ParentID", this.ModuleID);
                }

                return _Children;
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ����ģ��
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;

            base.Create();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CreateAndFlush()
        {
            this.CreateDate = DateTime.Now;

            base.CreateAndFlush();
        }

        /// <summary>
        /// ����ģ��(��ͬʱ����һ��TypeΪ2��Ȩ����Ϣ)(Ӧ����������)
        /// </summary>
        public override void Update()
        {
            this.LastModifiedDate = DateTime.Now;

            base.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAndFlush()
        {
            this.LastModifiedDate = DateTime.Now;

            base.UpdateAndFlush();
        }

        /// <summary>
        /// ����ģ��(��ͬʱ����һ��TypeΪ2��Ȩ����Ϣ)(Ӧ����������)
        /// </summary>
        public override void Save()
        {
            if (this.ModuleID != null)
            {
                this.LastModifiedDate = DateTime.Now;
            }
            else
            {
                this.CreateDate = DateTime.Now;
            }

            base.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SaveAndFlush()
        {
            if (this.ModuleID != null)
            {
                this.LastModifiedDate = DateTime.Now;
            }
            else
            {
                this.CreateDate = DateTime.Now;
            }

            base.SaveAndFlush();
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡģ��������Ӧ��
        /// </summary>
        /// <returns></returns>
        public SysApplication OwnerApplication()
        {
            if (!String.IsNullOrEmpty(ApplicationID))
            {
                return SysApplication.Find(this.ApplicationID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��ȡ���Ȩ��
        /// </summary>
        /// <returns></returns>
        public SysAuth[] GetRelatedAuth()
        {
            // TypeΪ1 ����Ӧ��Ȩ��
            SysAuth[] auths = SysAuth.FindAllByProperties("Type", 1, "ModuleID", this.ModuleID);
            return auths;
        }

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("�����ظ��� ģ���� ��" + this.Code + "��");
            }
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    // ����ʼ
                    this.CreateAndFlush();

                    SysAuth auth = new SysAuth();
                    auth.CreateByModule(this);

                    trans.VoteCommit();
                }
                catch (Exception ex)
                {
                    trans.VoteRollBack();

                    throw ex;
                }
            }
        }

        /// <summary>
        /// �޸�ģ��(ͬʱ�޸�����Ϊ1����ӦȨ�޵�����)
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            SysAuth[] auths = this.GetRelatedAuth();

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    this.UpdateAndFlush();

                    if (auths.Length > 0)
                    {
                        foreach (SysAuth auth in auths)
                        {
                            auth.UpdateByModule(this);
                        }
                    }
                    else
                    {
                        SysAuth auth = new SysAuth();
                        auth.CreateByModule(this);
                    }

                    trans.VoteCommit();
                }
                catch (Exception ex)
                {
                    trans.VoteRollBack();

                    throw ex;
                }
            }
        }

        /// <summary>
        /// ɾ��ģ��
        /// </summary>
        public void DoDelete()
        {
            SysAuth[] auths = this.GetRelatedAuth();

            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    foreach (SysAuth auth in auths)
                    {
                        auth.DoDelete();
                    }

                    this.Delete();

                    trans.VoteCommit();
                }
                catch (Exception ex)
                {
                    trans.VoteRollBack();

                    throw ex;
                }
            }
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
            SysModule sib = SysModule.Find(sibID);

            CreateAsSib(sib);
        }

        /// <summary>
        /// ����ֵ�ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSib(SysModule sib)
        {
            this.ParentID = sib.ParentID;
            this.ApplicationID = sib.ApplicationID;
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
            SysModule parent = SysModule.Find(parentID);

            CreateAsSub(parent);
        }

        /// <summary>
        /// �����ģ��
        /// </summary>
        /// <param name="module"></param>
        public void CreateAsSub(SysModule parent)
        {
            this.ParentID = parent.ModuleID;
            this.ApplicationID = parent.ApplicationID;
            this.Path = String.Format("{0}.{1}", (parent.Path == null ? String.Empty : parent.Path), parent.ModuleID);
            this.PathLevel = parent.PathLevel + 1;

            this.DoCreate();
        }

        #endregion

    } // SysUser
}

