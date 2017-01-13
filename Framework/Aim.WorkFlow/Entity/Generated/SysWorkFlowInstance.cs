// Business class SysWorkFlowInstance generated from SysWorkFlowInstance
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
	[ActiveRecord("SysWorkFlowInstance")]
	public partial class SysWorkFlowInstance : EntityBase<SysWorkFlowInstance>
	
	{

		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_SysApplication = "SysApplication";
		public static string Prop_EFormMetaDataId = "EFormMetaDataId";
		public static string Prop_EFormInstanceId = "EFormInstanceId";
		public static string Prop_WorkFlowDefineId = "WorkFlowDefineId";
		public static string Prop_WorkFlowName = "WorkFlowName";
		public static string Prop_Status = "Status";
		public static string Prop_RelateId = "RelateId";
		public static string Prop_RelateName = "RelateName";
		public static string Prop_Type = "Type";
		public static string Prop_Remark = "Remark";
		public static string Prop_StartTime = "StartTime";
		public static string Prop_EndTime = "EndTime";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _sysApplication;
		private string _eFormMetaDataId;
		private string _eFormInstanceId;
		private string _workFlowDefineId;
		private string _workFlowName;
		private string _status;
		private string _relateId;
		private string _relateName;
		private byte[] _type;
		private string _remark;
		private DateTime? _startTime;
		private DateTime? _endTime;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public SysWorkFlowInstance()
		{
		}

		public SysWorkFlowInstance(
			string p_id,
			string p_sysApplication,
			string p_eFormMetaDataId,
			string p_eFormInstanceId,
			string p_workFlowDefineId,
			string p_workFlowName,
			string p_status,
			string p_relateId,
			string p_relateName,
			byte[] p_type,
			string p_remark,
			DateTime? p_startTime,
			DateTime? p_endTime,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_sysApplication = p_sysApplication;
			_eFormMetaDataId = p_eFormMetaDataId;
			_eFormInstanceId = p_eFormInstanceId;
			_workFlowDefineId = p_workFlowDefineId;
			_workFlowName = p_workFlowName;
			_status = p_status;
			_relateId = p_relateId;
			_relateName = p_relateName;
			_type = p_type;
			_remark = p_remark;
			_startTime = p_startTime;
			_endTime = p_endTime;
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
					RaisePropertyChanged(SysWorkFlowInstance.Prop_SysApplication, oldValue, value);
				}
			}
		}

		[Property("EFormMetaDataId", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
		public string EFormMetaDataId
		{
			get { return _eFormMetaDataId; }
			set
			{
				if ((_eFormMetaDataId == null) || (value == null) || (!value.Equals(_eFormMetaDataId)))
				{
                    object oldValue = _eFormMetaDataId;
					_eFormMetaDataId = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_EFormMetaDataId, oldValue, value);
				}
			}
		}

		[Property("EFormInstanceId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string EFormInstanceId
		{
			get { return _eFormInstanceId; }
			set
			{
				if ((_eFormInstanceId == null) || (value == null) || (!value.Equals(_eFormInstanceId)))
				{
                    object oldValue = _eFormInstanceId;
					_eFormInstanceId = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_EFormInstanceId, oldValue, value);
				}
			}
		}

		[Property("WorkFlowDefineId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string WorkFlowDefineId
		{
			get { return _workFlowDefineId; }
			set
			{
				if ((_workFlowDefineId == null) || (value == null) || (!value.Equals(_workFlowDefineId)))
				{
                    object oldValue = _workFlowDefineId;
					_workFlowDefineId = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_WorkFlowDefineId, oldValue, value);
				}
			}
		}

		[Property("WorkFlowName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string WorkFlowName
		{
			get { return _workFlowName; }
			set
			{
				if ((_workFlowName == null) || (value == null) || (!value.Equals(_workFlowName)))
				{
                    object oldValue = _workFlowName;
					_workFlowName = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_WorkFlowName, oldValue, value);
				}
			}
		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Status
		{
			get { return _status; }
			set
			{
				if ((_status == null) || (value == null) || (!value.Equals(_status)))
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_Status, oldValue, value);
				}
			}
		}

		[Property("RelateId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string RelateId
		{
			get { return _relateId; }
			set
			{
				if ((_relateId == null) || (value == null) || (!value.Equals(_relateId)))
				{
                    object oldValue = _relateId;
					_relateId = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_RelateId, oldValue, value);
				}
			}
		}

		[Property("RelateName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string RelateName
		{
			get { return _relateName; }
			set
			{
				if ((_relateName == null) || (value == null) || (!value.Equals(_relateName)))
				{
                    object oldValue = _relateName;
					_relateName = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_RelateName, oldValue, value);
				}
			}
		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public byte[] Type
		{
			get { return _type; }
			set
			{
				if (value != _type)
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_Type, oldValue, value);
				}
			}
		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_Remark, oldValue, value);
				}
			}
		}

		[Property("StartTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? StartTime
		{
			get { return _startTime; }
			set
			{
				if (value != _startTime)
				{
                    object oldValue = _startTime;
					_startTime = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_StartTime, oldValue, value);
				}
			}
		}

		[Property("EndTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? EndTime
		{
			get { return _endTime; }
			set
			{
				if (value != _endTime)
				{
                    object oldValue = _endTime;
					_endTime = value;
					RaisePropertyChanged(SysWorkFlowInstance.Prop_EndTime, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowInstance.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowInstance.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowInstance.Prop_CreateTime, oldValue, value);
				}
			}
		}

		#endregion
	} // SysWorkFlowInstance
}

