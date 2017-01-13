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
    public partial class SysAuthType
    {
        #region ˽�б���

        private bool? _HasAuth = null;

        #endregion

        #region ��Ա����

        /// <summary>
        /// �������Ƿ�ӵ��Ȩ��
        /// </summary>
        public bool HasAuth
        {
            get
            {
                if (_HasAuth == null)
                {
                    return SysAuth.Exists("Type = ?", this.AuthTypeID);
                }

                return _HasAuth.GetValueOrDefault();
            }
        }

        #endregion

    } // SysAuthType
}

