// Business class SysModule generated from SysModule
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Castle.ActiveRecord;
using Aim.Data;
using Aim.Portal.Model;

namespace Aim.Portal.Model
{
    public partial class SysAuthTypeRule	
	{
        /// <summary>
        /// ��ȡ����ö��
        /// </summary>
        public static DataEnum GetAuthTypeEnum()
        {
            DataEnum de = new DataEnum();

            SysAuthType[] types = SysAuthType.FindAll();

            foreach (SysAuthType type in types)
            {
                de.Add(type.AuthTypeID.ToString(), type.Name);
            }

            return de;
        }

        /// <summary>
        /// ��ȡӦ��Ȩ�����͵�Ӧ��
        /// </summary>
        /// <returns></returns>
        public static IList<SysApplication> GetApplications()
        {
            // TypeΪ1��IDΪData��Ȩ��
            DetachedCriteria crits = DetachedCriteria.For<SysAuth>();
            crits.Add(Expression.IsNotNull("Data"));
            crits.Add(Expression.IsNull("ModuleID"));
            crits.Add(Expression.Eq("Type", 1));
            crits.SetResultTransformer(new DistinctRootEntityResultTransformer());
            IList<SysAuth> auths = SysAuth.FindAll(crits);

            return SysAuthRule.RetrieveAllApplicationFromAuthList(auths);
        }

    } // SysAuthTypeRule
}

