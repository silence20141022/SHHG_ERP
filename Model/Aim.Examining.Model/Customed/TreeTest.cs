﻿using System;
using System.Linq;
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
	
namespace Aim.Examining.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class TreeTest
    {
        #region 成员变量
        private bool? _IsHasChild = null;
        #endregion

        #region 成员属性
        /// <summary>
        /// 是否包含子节点
        /// </summary>
        public bool IsHasChild
        {
            get
            {
                if (_IsHasChild == null)
                {
                    ICriterion cirt = Expression.Eq("ParentId", this.Id);
                    bool isMin = TreeTest.Exists(cirt);
                    _IsHasChild = isMin;
                }

                return _IsHasChild.GetValueOrDefault();
            }
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            /*if (!this.IsPropertyUnique("UniqueKey"))
            {
                throw new RepeatedKeyException("存在重复的 UniqueKey “" + this.UniqueKey + "”");
            }*/
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(Id))
            {
                this.DoCreate();
            }
            else
            {
                this.DoUpdate();
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();


            // 事务开始
            this.CreateAndFlush();
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();
                        

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			TreeTest[] tents = TreeTest.FindAll(Expression.In("Id", args));

			foreach (TreeTest tent in tents)
			{
				tent.DoDelete();
			}
        }
        
        #endregion

    } // TreeTest
}


