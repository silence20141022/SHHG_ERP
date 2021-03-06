﻿
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
    /// 自定义实体类
    /// </summary>
    [Serializable]
    public partial class SysParameterCatalog : EditSensitiveTreeNodeEntityBase<SysParameterCatalog>
    {
        #region 成员变量

        #endregion

        #region 成员属性
        
        #endregion

        #region 重载

        protected override void DoCreate()
        {
            this.DoValidate();

            this.CreatedDate = DateTime.Now;

            base.DoCreate();
        }

        public override void Update()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            base.Update();
        }

        [ActiveRecordTransaction]
        public override void Delete()
        {
            // 删除本节点及子节点以及相关子节点类型
            string sql = String.Format("DELETE FROM SysParameter WHERE CatalogID IN (SELECT ParameterCatalogID FROM SysParameterCatalog WHERE Path LIKE '%{0}%' OR ParameterCatalogID = '{0}')"
                + "DELETE FROM SysParameterCatalog WHERE Path LIKE '%{0}%' OR ParameterCatalogID = '{0}'; ", this.ParameterCatalogID);

            if (!String.IsNullOrEmpty(this.ParentID))
            {
                sql += String.Format("UPDATE SysParameterCatalog SET IsLeaf = 1 WHERE ParameterCatalogID = '{0}' AND NOT EXISTS (SELECT ParameterCatalogID FROM SysParameterCatalog WHERE ParentID = '{0}')", this.ParentID);
            }

            DataHelper.QueryValue(sql);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			SysParameterCatalog[] tents = SysParameterCatalog.FindAllByPrimaryKeys(args);

			foreach (SysParameterCatalog tent in tents)
			{
				tent.Delete();
			}
        }
        
        #endregion

    } // SysParameterCatalog
}


