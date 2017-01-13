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
    public partial class SysRoleType
    {
        #region ˽�б���

        private bool? _HasRole = null;

        #endregion

        #region ��Ա����

        /// <summary>
        /// �������Ƿ�ӵ��Ȩ��
        /// </summary>
        public bool HasRole
        {
            get
            {
                if (_HasRole == null)
                {
                    _HasRole = SysRole.Exists("Type = ?", this.RoleTypeID);
                }

                return _HasRole.GetValueOrDefault();
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ����
        /// </summary>
        public override void Create()
        {
            base.Create();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CreateAndFlush()
        {
            base.CreateAndFlush();
        }

        /// <summary>
        /// ����
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAndFlush()
        {
            base.UpdateAndFlush();
        }

        /// <summary>
        /// ����
        /// </summary>
        public override void Save()
        {
            base.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SaveAndFlush()
        {
            base.SaveAndFlush();
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique("Name"))
            {
                throw new RepeatedKeyException("�Ѵ��� ��ɫ�� ��" + this.Name + "��");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

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
            ICriterion crit = Expression.Eq("Type", this.RoleTypeID);
            if (SysRole.Exists(crit))
            {
                throw new Exception("���ڴ����͵Ľ�ɫ������ִ��ɾ��������");
            }

            base.Delete();
        }

        #endregion

    } // SysUser
}

