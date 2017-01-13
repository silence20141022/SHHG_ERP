// Business class SysWorkFlowEFormMetaData generated from SysWorkFlowEFormMetaData
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
	[ActiveRecord("SysWorkFlowEFormMetaData")]
	public partial class SysWorkFlowEFormMetaData : EntityBase<SysWorkFlowEFormMetaData>
	
	{

		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Name = "Name";
		public static string Prop_Version = "Version";
		public static string Prop_QualfiedEntitySetName = "QualfiedEntitySetName";
		public static string Prop_Url = "Url";
		public static string Prop_KeyName = "KeyName";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_UpdateTime = "UpdateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _name;
		private string _version;
		private string _qualfiedEntitySetName;
		private string _url;
		private string _keyName;
		private string _remark;
		private DateTime? _createTime;
		private DateTime? _updateTime;


		#endregion

		#region Constructors

		public SysWorkFlowEFormMetaData()
		{
		}

		public SysWorkFlowEFormMetaData(
			string p_id,
			string p_name,
			string p_version,
			string p_qualfiedEntitySetName,
			string p_url,
			string p_keyName,
			string p_remark,
			DateTime? p_createTime,
			DateTime? p_updateTime)
		{
			_id = p_id;
			_name = p_name;
			_version = p_version;
			_qualfiedEntitySetName = p_qualfiedEntitySetName;
			_url = p_url;
			_keyName = p_keyName;
			_remark = p_remark;
			_createTime = p_createTime;
			_updateTime = p_updateTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
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
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_Version, oldValue, value);
				}
			}
		}

		[Property("QualfiedEntitySetName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string QualfiedEntitySetName
		{
			get { return _qualfiedEntitySetName; }
			set
			{
				if ((_qualfiedEntitySetName == null) || (value == null) || (!value.Equals(_qualfiedEntitySetName)))
				{
                    object oldValue = _qualfiedEntitySetName;
					_qualfiedEntitySetName = value;
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_QualfiedEntitySetName, oldValue, value);
				}
			}
		}

		[Property("Url", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Url
		{
			get { return _url; }
			set
			{
				if ((_url == null) || (value == null) || (!value.Equals(_url)))
				{
                    object oldValue = _url;
					_url = value;
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_Url, oldValue, value);
				}
			}
		}

		[Property("KeyName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string KeyName
		{
			get { return _keyName; }
			set
			{
				if ((_keyName == null) || (value == null) || (!value.Equals(_keyName)))
				{
                    object oldValue = _keyName;
					_keyName = value;
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_KeyName, oldValue, value);
				}
			}
		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_CreateTime, oldValue, value);
				}
			}
		}

		[Property("UpdateTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? UpdateTime
		{
			get { return _updateTime; }
			set
			{
				if (value != _updateTime)
				{
                    object oldValue = _updateTime;
					_updateTime = value;
					RaisePropertyChanged(SysWorkFlowEFormMetaData.Prop_UpdateTime, oldValue, value);
				}
			}
		}

		#endregion
	} // SysWorkFlowEFormMetaData
}

