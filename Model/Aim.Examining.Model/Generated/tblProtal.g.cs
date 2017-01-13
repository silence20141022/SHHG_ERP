// Business class tblProtal generated from tblProtal
// Creator: Ray
// Created Date: [2011-11-21]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("tblProtal")]
	public partial class tblProtal : ExamModelBase<tblProtal>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_Description = "Description";
		public static string Prop_Sort = "Sort";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _name;
		private string _type;
		private string _description;
		private int? _sort;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public tblProtal()
		{
		}

		public tblProtal(
			string p_id,
			string p_name,
			string p_type,
			string p_description,
			int? p_sort,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_name = p_name;
			_type = p_type;
			_description = p_description;
			_sort = p_sort;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(tblProtal.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(tblProtal.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(tblProtal.Prop_Description, oldValue, value);
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
					RaisePropertyChanged(tblProtal.Prop_Sort, oldValue, value);
				}
			}

		}

		[Property("CreateId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CreateId
		{
			get { return _createId; }
			set
			{
				if ((_createId == null) || (value == null) || (!value.Equals(_createId)))
				{
                    object oldValue = _createId;
					_createId = value;
					RaisePropertyChanged(tblProtal.Prop_CreateId, oldValue, value);
				}
			}

		}

		[Property("CreateName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string CreateName
		{
			get { return _createName; }
			set
			{
				if ((_createName == null) || (value == null) || (!value.Equals(_createName)))
				{
                    object oldValue = _createName;
					_createName = value;
					RaisePropertyChanged(tblProtal.Prop_CreateName, oldValue, value);
				}
			}

		}

		[Property("CreateTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? CreateTime
		{
			get { return _createTime; }
			set
			{
				if (value != _createTime)
				{
                    object oldValue = _createTime;
					_createTime = value;
					RaisePropertyChanged(tblProtal.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // tblProtal
}

