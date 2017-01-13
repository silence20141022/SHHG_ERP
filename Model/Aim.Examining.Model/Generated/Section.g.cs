// Business class Section generated from Section
// Creator: Ray
// Created Date: [2011-11-09]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Section")]
	public partial class Section : ExamModelBase<Section>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Name = "Name";
		public static string Prop_EnName = "EnName";
		public static string Prop_RemindDays = "RemindDays";
		public static string Prop_Isvalid = "Isvalid";
		public static string Prop_Type = "Type";
		public static string Prop_DisplayStyle = "DisplayStyle";
		public static string Prop_SelRole = "SelRole";
		public static string Prop_MgrRole = "MgrRole";
		public static string Prop_SubRole = "SubRole";
		public static string Prop_AuditRole = "AuditRole";
		public static string Prop_IsMainpage = "IsMainpage";
		public static string Prop_MainPageCount = "MainPageCount";
		public static string Prop_Sort = "Sort";
		public static string Prop_UserId = "UserId";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _name;
		private string _enName;
		private int? _remindDays;
		private string _isvalid;
		private string _type;
		private string _displayStyle;
		private string _selRole;
		private string _mgrRole;
		private string _subRole;
		private string _auditRole;
		private string _isMainpage;
		private int? _mainPageCount;
		private int? _sort;
		private string _userId;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public Section()
		{
		}

		public Section(
			string p_id,
			string p_name,
			string p_enName,
			int? p_remindDays,
			string p_isvalid,
			string p_type,
			string p_displayStyle,
			string p_selRole,
			string p_mgrRole,
			string p_subRole,
			string p_auditRole,
			string p_isMainpage,
			int? p_mainPageCount,
			int? p_sort,
			string p_userId,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_name = p_name;
			_enName = p_enName;
			_remindDays = p_remindDays;
			_isvalid = p_isvalid;
			_type = p_type;
			_displayStyle = p_displayStyle;
			_selRole = p_selRole;
			_mgrRole = p_mgrRole;
			_subRole = p_subRole;
			_auditRole = p_auditRole;
			_isMainpage = p_isMainpage;
			_mainPageCount = p_mainPageCount;
			_sort = p_sort;
			_userId = p_userId;
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
					RaisePropertyChanged(Section.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("EnName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string EnName
		{
			get { return _enName; }
			set
			{
				if ((_enName == null) || (value == null) || (!value.Equals(_enName)))
				{
                    object oldValue = _enName;
					_enName = value;
					RaisePropertyChanged(Section.Prop_EnName, oldValue, value);
				}
			}

		}

		[Property("RemindDays", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? RemindDays
		{
			get { return _remindDays; }
			set
			{
				if (value != _remindDays)
				{
                    object oldValue = _remindDays;
					_remindDays = value;
					RaisePropertyChanged(Section.Prop_RemindDays, oldValue, value);
				}
			}

		}

		[Property("Isvalid", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 10)]
		public string Isvalid
		{
			get { return _isvalid; }
			set
			{
				if ((_isvalid == null) || (value == null) || (!value.Equals(_isvalid)))
				{
                    object oldValue = _isvalid;
					_isvalid = value;
					RaisePropertyChanged(Section.Prop_Isvalid, oldValue, value);
				}
			}

		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(Section.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("DisplayStyle", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string DisplayStyle
		{
			get { return _displayStyle; }
			set
			{
				if ((_displayStyle == null) || (value == null) || (!value.Equals(_displayStyle)))
				{
                    object oldValue = _displayStyle;
					_displayStyle = value;
					RaisePropertyChanged(Section.Prop_DisplayStyle, oldValue, value);
				}
			}

		}

		[Property("SelRole", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string SelRole
		{
			get { return _selRole; }
			set
			{
				if ((_selRole == null) || (value == null) || (!value.Equals(_selRole)))
				{
                    object oldValue = _selRole;
					_selRole = value;
					RaisePropertyChanged(Section.Prop_SelRole, oldValue, value);
				}
			}

		}

		[Property("MgrRole", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string MgrRole
		{
			get { return _mgrRole; }
			set
			{
				if ((_mgrRole == null) || (value == null) || (!value.Equals(_mgrRole)))
				{
                    object oldValue = _mgrRole;
					_mgrRole = value;
					RaisePropertyChanged(Section.Prop_MgrRole, oldValue, value);
				}
			}

		}

		[Property("SubRole", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string SubRole
		{
			get { return _subRole; }
			set
			{
				if ((_subRole == null) || (value == null) || (!value.Equals(_subRole)))
				{
                    object oldValue = _subRole;
					_subRole = value;
					RaisePropertyChanged(Section.Prop_SubRole, oldValue, value);
				}
			}

		}

		[Property("AuditRole", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string AuditRole
		{
			get { return _auditRole; }
			set
			{
				if ((_auditRole == null) || (value == null) || (!value.Equals(_auditRole)))
				{
                    object oldValue = _auditRole;
					_auditRole = value;
					RaisePropertyChanged(Section.Prop_AuditRole, oldValue, value);
				}
			}

		}

		[Property("IsMainpage", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 10)]
		public string IsMainpage
		{
			get { return _isMainpage; }
			set
			{
				if ((_isMainpage == null) || (value == null) || (!value.Equals(_isMainpage)))
				{
                    object oldValue = _isMainpage;
					_isMainpage = value;
					RaisePropertyChanged(Section.Prop_IsMainpage, oldValue, value);
				}
			}

		}

		[Property("MainPageCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? MainPageCount
		{
			get { return _mainPageCount; }
			set
			{
				if (value != _mainPageCount)
				{
                    object oldValue = _mainPageCount;
					_mainPageCount = value;
					RaisePropertyChanged(Section.Prop_MainPageCount, oldValue, value);
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
					RaisePropertyChanged(Section.Prop_Sort, oldValue, value);
				}
			}

		}

		[Property("UserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string UserId
		{
			get { return _userId; }
			set
			{
				if ((_userId == null) || (value == null) || (!value.Equals(_userId)))
				{
                    object oldValue = _userId;
					_userId = value;
					RaisePropertyChanged(Section.Prop_UserId, oldValue, value);
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
					RaisePropertyChanged(Section.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Section.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Section.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // Section
}

