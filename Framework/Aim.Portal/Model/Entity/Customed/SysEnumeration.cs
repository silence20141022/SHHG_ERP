﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
	public partial class SysEnumeration : EditSensitiveTreeNodeEntityBase<SysEnumeration>
    {
        #region 成员变量

        #endregion

        #region 成员属性



        #endregion

        #region 重载

        /// <summary>
        /// 创建操作
        /// </summary>
        protected override void DoCreate()
        {
            // 编码为空而值不为空时(默认将值作为编码后缀)
            if (String.IsNullOrEmpty(this.Code) && !String.IsNullOrEmpty(this.Value))
            {
                if (this.Parent == null)
                {
                    this.Code = this.Value;
                }
                else
                {
                    this.Code = String.Format("{0}.{1}", this.Parent.Code, this.Value);
                }
            }

            this.DoValidate();

            this.CreatedDate = DateTime.Now;

            // 事务开始
            base.DoCreate();
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        public override void Update()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            base.Update();
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

        /// <summary>
        /// 转换为字典(用于前台生成枚举)
        /// </summary>
        public EasyDictionary GetDict()
        {
            EasyDictionary dict = SysEnumeration.GetEnumDict(this.ChildNodes);

            return dict;
        }

        #endregion
        
        #region 静态成员

        /// <summary>
        /// 获取Code为pcode枚举下值为为value的枚举项
        /// </summary>
        /// <param name="pcode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SysEnumeration GetByValue(string pcode, string value)
        {
            if (!String.IsNullOrEmpty(pcode))
            {
                SysEnumeration tenum = SysEnumeration.Get(pcode);

                if (tenum != null)
                {
                    if (tenum.Value == value)
                    {
                        return tenum;
                    }
                    else
                    {
                        return SysEnumeration.FindFirst(
                            Expression.Like(Prop_Path, tenum.ID, MatchMode.Anywhere),
                            Expression.Eq(Prop_Value, value));
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 由枚举编码获取Enum字典
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static IList<SysEnumeration> GetEnumDictList(string code)
        {
            SysEnumeration tent = SysEnumeration.Get(code);
            return tent.ChildNodes;
        }

        /// <summary>
        /// 由编码获取Enumeration
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static SysEnumeration Get(string code)
        {
            SysEnumeration[] tents = SysEnumeration.FindAllByProperty(SysEnumeration.Prop_Code, code);
            if (tents != null && tents.Length > 0)
            {
                return tents[0];
            }

            return null;
        }

        /// <summary>
        /// 由编码获取Enumeration字典
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static EasyDictionary<string, SysEnumeration> Get(params string[] codes)
        {
            EasyDictionary<string, SysEnumeration> enums = new EasyDictionary<string, SysEnumeration>();

            IEnumerable<SysEnumeration> tents = SysEnumeration.FindAll(Expression.In(SysEnumeration.Prop_Code, codes));
            tents = tents.OrderBy(tent => tent.SortIndex).ThenBy(tent => tent.CreatedDate);

            foreach (SysEnumeration tent in tents)
            {
                enums.Set(tent.Code, tent);
            }

            return enums;
        }

        /// <summary>
        /// 由给出的编码列表获取枚举字典列表
        /// </summary>
        /// <returns></returns>
        public static EasyDictionary<string, EasyDictionary> GetEnumDicts(params string[] codes)
        {
            EasyDictionary<string, EasyDictionary> rtndict = new EasyDictionary<string, EasyDictionary>();

            SysEnumeration[] tents = SysEnumeration.FindAll(Expression.In(SysEnumeration.Prop_Code, codes));
            string[] pids = tents.Select(tent => tent.EnumerationID).ToArray();

            IEnumerable<SysEnumeration> subents = SysEnumeration.FindAll(Expression.In(SysEnumeration.Prop_ParentID, pids));
            subents = subents.OrderBy(tsubtent => tsubtent.SortIndex).ThenBy(tsubtent => tsubtent.CreatedDate);

            foreach (SysEnumeration tent in tents)
            {
                EasyDictionary dict = new EasyDictionary();

                IEnumerable<SysEnumeration> tsubents = subents.Where(ttent => ttent.ParentID == tent.EnumerationID);
                foreach (SysEnumeration tsubent in tsubents)
                {
                    dict.Set(tsubent.Value, tsubent.Name);
                }

                rtndict.Set(tent.Code, dict);
            }

            return rtndict;
        }

        /// <summary>
        /// 获取所有子孙节点组合的枚举
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static EasyDictionary GetDescendantEnumDict(string code)
        {
            return GetDescendantEnumDict(code, false);
        }

        /// <summary>
        /// 获取所有子孙节点组合的枚举
        /// </summary>
        /// <param name="code"></param>
        /// <param name="includeSelf">是否包括当前节点</param>
        /// <returns></returns>
        public static EasyDictionary GetDescendantEnumDict(string code, bool includeSelf)
        {
            SysEnumeration tenum = SysEnumeration.Get(code);
            IList<SysEnumeration> enums = tenum.GetDescendantNodes().ToList();

            if (includeSelf)
            {
                enums.Add(tenum);
            }

            if (enums != null && enums.Count > 0)
            {
                enums = enums.OrderBy(tent => tent.PathLevel).ThenBy(tent => tent.SortIndex).ToList();
            }

            return GetEnumDict(enums); ;
        }

        /// <summary>
        /// 由枚举编码获取Enum字典
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static EasyDictionary GetEnumDict(string code)
        {
            SysEnumeration tent = SysEnumeration.Get(code);
            EasyDictionary dict = tent.GetDict();

            return dict;
        }

        /// <summary>
        /// 由enums列表获取Enum字典
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static EasyDictionary GetEnumDict(IEnumerable<SysEnumeration> enums)
        {
            EasyDictionary dict = new EasyDictionary();

            IEnumerable<SysEnumeration> tenums = null;

            if (enums is IOrderedEnumerable<SysEnumeration>)
            {
                tenums = enums;
            }
            else
            {
                tenums = enums.OrderBy(tent => tent.SortIndex).ThenBy(tent => tent.CreatedDate);
            }

            foreach (SysEnumeration item in tenums)
            {
                dict.Set(item.Value, item.Name);
            }

            return dict;
        }
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			SysEnumeration[] tents = SysEnumeration.FindAll(Expression.In("EnumerationID", args));

			foreach (SysEnumeration tent in tents)
			{
				tent.Delete();
			}
        }

        /// <summary>
        /// 批量粘贴操作
        /// </summary>
        /// <param name="patype">sib, sub</param>
        /// <param name="targetId"></param>
        /// <param name="args"></param>
        [ActiveRecordTransaction]
        public static void DoPaste(PasteDataSourceEnum pdstype, PasteAsEnum patype, string targetId, params object[] pasteIds)
        {
            if (!String.IsNullOrEmpty(targetId) && pasteIds.Length > 0)
            {
                IList<SysEnumeration> allnodes = SysEnumeration.FindAllByPrimaryKeys(pasteIds);
                IList<SysEnumeration> nodes = FilterChildNodes(allnodes);

                // 只提取最高节点或无父子关联的节点进行粘贴
                foreach (SysEnumeration tnode in nodes)
                {
                    switch (pdstype)
                    {
                        case PasteDataSourceEnum.Copy:
                            if (patype == PasteAsEnum.Sibling)
                            {
                                tnode.CopyAsSibling(targetId);
                            }
                            else if (patype == PasteAsEnum.Child)
                            {
                                tnode.CopyAsChild(targetId);
                            }
                            break;
                        case PasteDataSourceEnum.Cut:
                            if (patype == PasteAsEnum.Sibling)
                            {
                                tnode.MoveAsSibling(targetId);
                            }
                            else if (patype == PasteAsEnum.Child)
                            {
                                if (tnode.ParentID == targetId)
                                {
                                    tnode.ChangePosition(0);
                                }
                                else
                                {
                                    tnode.MoveAsChild(targetId);
                                }
                            }
                            break;
                    }
                }
            }
        }
        
        #endregion

    } // SysEnumeration
}


