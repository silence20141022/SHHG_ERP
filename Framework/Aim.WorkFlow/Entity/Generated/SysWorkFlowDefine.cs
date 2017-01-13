// Business class SysWorkFlowDefine generated from SysWorkFlowDefine
// Creator: Ray
// Created Date: [2010-08-09]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
namespace Aim.WorkFlow
{
	[ActiveRecord("SysWorkFlowDefine")]
	public partial class SysWorkFlowDefine : EntityBase<SysWorkFlowDefine>
	
	{

		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_SysApplication = "SysApplication";
		public static string Prop_Type = "Type";
        public static string Prop_UseKey = "UseKey";
		public static string Prop_Name = "Name";
		public static string Prop_DllSource = "DllSource";
		public static string Prop_AssemblyName = "AssemblyName";
        public static string Prop_XamlString = "XamlString";
		public static string Prop_State = "State";
		public static string Prop_Version = "Version";
		public static string Prop_Description = "Description";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _sysApplication;
		private string _type;
        private string _useKey;
		private string _name;
		private string _dllSource;
        private string _xamlString;
		private string _assemblyName;
		private string _state;
		private string _version;
		private string _description;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public SysWorkFlowDefine()
		{
		}

		public SysWorkFlowDefine(
			string p_id,
			string p_sysApplication,
			string p_type,
			string p_name,
			string p_dllSource,
			string p_assemblyName,
            string p_state,
            string p_xamlString,
			string p_version,
			string p_description,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_sysApplication = p_sysApplication;
			_type = p_type;
			_name = p_name;
			_dllSource = p_dllSource;
			_assemblyName = p_assemblyName;
            _xamlString = p_xamlString;
			_state = p_state;
			_version = p_version;
			_description = p_description;
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
		}

		[Property("SysApplication", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SysApplication
		{
			get { return _sysApplication; }
			set
			{
				if ((_sysApplication == null) || (value == null) || (!value.Equals(_sysApplication)))
				{
                    object oldValue = _sysApplication;
					_sysApplication = value;
					RaisePropertyChanged(SysWorkFlowDefine.Prop_SysApplication, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowDefine.Prop_Type, oldValue, value);
				}
			}
		}

        [Property("UseKey", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string UseKey
        {
            get { return _useKey; }
            set
            {
                if ((_useKey == null) || (value == null) || (!value.Equals(_useKey)))
                {
                    object oldValue = _useKey;
                    _useKey = value;
                    RaisePropertyChanged(SysWorkFlowDefine.Prop_UseKey, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowDefine.Prop_Name, oldValue, value);
				}
			}
		}

		[Property("DllSource", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string DllSource
		{
			get { return _dllSource; }
			set
			{
				if ((_dllSource == null) || (value == null) || (!value.Equals(_dllSource)))
				{
                    object oldValue = _dllSource;
					_dllSource = value;
					RaisePropertyChanged(SysWorkFlowDefine.Prop_DllSource, oldValue, value);
				}
			}
		}

		[Property("AssemblyName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string AssemblyName
		{
			get { return _assemblyName; }
			set
			{
				if ((_assemblyName == null) || (value == null) || (!value.Equals(_assemblyName)))
				{
                    object oldValue = _assemblyName;
					_assemblyName = value;
					RaisePropertyChanged(SysWorkFlowDefine.Prop_AssemblyName, oldValue, value);
				}
			}
		}

        [Property("XamlString", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
        public string XamlString
        {
            get { return _xamlString; }
            set { _xamlString = value; }
        }

		[Property("State", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string State
		{
			get { return _state; }
			set
			{
				if ((_state == null) || (value == null) || (!value.Equals(_state)))
				{
                    object oldValue = _state;
					_state = value;
					RaisePropertyChanged(SysWorkFlowDefine.Prop_State, oldValue, value);
				}
			}
		}

		[Property("Version", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Version
		{
			get { return _version; }
			set
			{
				if ((_version == null) || (value == null) || (!value.Equals(_version)))
				{
                    object oldValue = _version;
					_version = value;
					RaisePropertyChanged(SysWorkFlowDefine.Prop_Version, oldValue, value);
				}
			}
		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(SysWorkFlowDefine.Prop_Description, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowDefine.Prop_CreateId, oldValue, value);
				}
			}
		}

		[Property("CreateName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CreateName
		{
			get { return _createName; }
			set
			{
				if ((_createName == null) || (value == null) || (!value.Equals(_createName)))
				{
                    object oldValue = _createName;
					_createName = value;
					RaisePropertyChanged(SysWorkFlowDefine.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowDefine.Prop_CreateTime, oldValue, value);
				}
			}
		}

		#endregion
	} // SysWorkFlowDefine
}

