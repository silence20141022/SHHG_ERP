// Business class TreeTest generated from TreeTest
// Creator: Ray
// Created Date: [2011-11-01]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("TreeTest")]
	public partial class TreeTest : ExamModelBase<TreeTest>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_ParentId = "ParentId";
		public static string Prop_Name = "Name";
		public static string Prop_Value = "Value";
		public static string Prop_Sort = "Sort";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _parentId;
		private string _name;
		private string _value;
		private int? _sort;
		private string _remark;


		#endregion

		#region Constructors

		public TreeTest()
		{
		}

		public TreeTest(
			string p_id,
			string p_parentId,
			string p_name,
			string p_value,
			int? p_sort,
			string p_remark)
		{
			_id = p_id;
			_parentId = p_parentId;
			_name = p_name;
			_value = p_value;
			_sort = p_sort;
			_remark = p_remark;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("ParentId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ParentId
		{
			get { return _parentId; }
			set
			{
				if ((_parentId == null) || (value == null) || (!value.Equals(_parentId)))
				{
                    object oldValue = _parentId;
					_parentId = value;
					RaisePropertyChanged(TreeTest.Prop_ParentId, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(TreeTest.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Value", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Value
		{
			get { return _value; }
			set
			{
				if ((_value == null) || (value == null) || (!value.Equals(_value)))
				{
                    object oldValue = _value;
					_value = value;
					RaisePropertyChanged(TreeTest.Prop_Value, oldValue, value);
				}
			}

		}

		[Property("Sort", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Sort
		{
			get { return _sort; }
			set
			{
				if (value != _sort)
				{
                    object oldValue = _sort;
					_sort = value;
					RaisePropertyChanged(TreeTest.Prop_Sort, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(TreeTest.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // TreeTest
}

