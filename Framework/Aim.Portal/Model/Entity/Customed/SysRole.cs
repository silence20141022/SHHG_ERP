// Business class SysUser generated from SysUser
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Portal.Model
{
    [Serializable]
	public partial class SysRole
    {
        #region ˽�г�Ա

        [NonSerialized]
        private IList<SysGroup> _Group = new List<SysGroup>();

        [NonSerialized]
        private IList<SysAuth> _Auth = new List<SysAuth>();

        [NonSerialized]
        private IList<SysUser> _User = new List<SysUser>();

        #endregion

        #region ��������

        /// <summary>
        /// �Ƿ�ӵ��ĳȨ��
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasPermission(SysAuth auth)
        {
            return this.Auth.Contains(auth);
        }

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

        /// <summary>
        /// ɾ������
        /// </summary>
        public void DoDelete()
        {
            base.Delete();
        }

        #endregion

        #region ��Ա����

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysAuth), Table = "SysRolePermission", ColumnRef = "AuthID", ColumnKey = "RoleID", Lazy = true)]
        public IList<SysAuth> Auth
        {
            get { return _Auth; }
            set { _Auth = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysGroup), Table = "SysGroupRole", ColumnRef = "GroupID", ColumnKey = "RoleID", Lazy = true)]
        public IList<SysGroup> Group
        {
            get { return _Group; }
            set { _Group = value; }
        }

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(SysUser), Table = "SysUserRole", ColumnRef = "UserID", ColumnKey = "RoleID", Lazy = true)]
        public IList<SysUser> User
        {
            get { return _User; }
            set { _User = value; }
        }

        #endregion

        #region ����

        /// <summary>
        /// ����ģ��(��ͬʱ����һ��TypeΪ2��Ȩ����Ϣ)(Ӧ����������)
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
            if (this.RoleID != null)
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
            if (this.RoleID != null)
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

    } // SysUser
}

