// Business class SectionReadRole generated from SectionReadRole
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
	[ActiveRecord("SectionReadRole")]
	public partial class SectionReadRole : ExamModelBase<SectionReadRole>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_RoleId = "RoleId";
		public static string Prop_SecId = "SecId";
		public static string Prop_RoleType = "RoleType";
		public static string Prop_Type = "Type";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _roleId;
		private string _secId;
		private string _roleType;
		private string _type;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public SectionReadRole()
		{
		}

		public SectionReadRole(
			string p_id,
			string p_roleId,
			string p_secId,
			string p_roleType,
			string p_type,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_roleId = p_roleId;
			_secId = p_secId;
			_roleType = p_roleType;
			_type = p_type;
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

		[Property("RoleId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string RoleId
		{
			get { return _roleId; }
			set
			{
				if ((_roleId == null) || (value == null) || (!value.Equals(_roleId)))
				{
                    object oldValue = _roleId;
					_roleId = value;
					RaisePropertyChanged(SectionReadRole.Prop_RoleId, oldValue, value);
				}
			}

		}

		[Property("SecId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string SecId
		{
			get { return _secId; }
			set
			{
				if ((_secId == null) || (value == null) || (!value.Equals(_secId)))
				{
                    object oldValue = _secId;
					_secId = value;
					RaisePropertyChanged(SectionReadRole.Prop_SecId, oldValue, value);
				}
			}

		}

		[Property("RoleType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string RoleType
		{
			get { return _roleType; }
			set
			{
				if ((_roleType == null) || (value == null) || (!value.Equals(_roleType)))
				{
                    object oldValue = _roleType;
					_roleType = value;
					RaisePropertyChanged(SectionReadRole.Prop_RoleType, oldValue, value);
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
					RaisePropertyChanged(SectionReadRole.Prop_Type, oldValue, value);
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
					RaisePropertyChanged(SectionReadRole.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(SectionReadRole.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(SectionReadRole.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // SectionReadRole
}

