// Business class WorkflowInstance generated from WorkflowInstance
// Creator: Ray
// Created Date: [2010-11-19]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;

namespace Aim.WorkFlow
{
	[ActiveRecord("WorkflowInstance")]
	public partial class WorkflowInstance : EntityBase<WorkflowInstance>
	{
		#region Property_Names

		public static string Prop_ID = "ID";
		public static string Prop_WorkflowTemplateID = "WorkflowTemplateID";
		public static string Prop_EFormMetaDataID = "EFormMetaDataID";
		public static string Prop_EFormInstanceID = "EFormInstanceID";
		public static string Prop_Status = "Status";
		public static string Prop_SysApplication = "SysApplication";
		public static string Prop_WorkFlowName = "WorkFlowName";
		public static string Prop_RelateId = "RelateId";
		public static string Prop_RelateUrl = "RelateUrl";
		public static string Prop_RelateData = "RelateData";
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
		private string _workflowTemplateID;
		private string _eFormMetaDataID;
		private string _eFormInstanceID;
		private string _status;
		private string _sysApplication;
		private string _workFlowName;
		private string _relateId;
		private string _relateUrl;
		private string _relateData;
		private string _type;
		private string _remark;
		private DateTime? _startTime;
		private DateTime? _endTime;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public WorkflowInstance()
		{
		}

		public WorkflowInstance(
			string p_id,
			string p_workflowTemplateID,
			string p_eFormMetaDataID,
			string p_eFormInstanceID,
			string p_status,
			string p_sysApplication,
			string p_workFlowName,
			string p_relateId,
			string p_relateUrl,
			string p_relateData,
			string p_type,
			string p_remark,
			DateTime? p_startTime,
			DateTime? p_endTime,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_workflowTemplateID = p_workflowTemplateID;
			_eFormMetaDataID = p_eFormMetaDataID;
			_eFormInstanceID = p_eFormInstanceID;
			_status = p_status;
			_sysApplication = p_sysApplication;
			_workFlowName = p_workFlowName;
			_relateId = p_relateId;
			_relateUrl = p_relateUrl;
			_relateData = p_relateData;
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

		[PrimaryKey("ID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string ID
		{
			get { return _id; }
		}

		[Property("WorkflowTemplateID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public string WorkflowTemplateID
		{
			get { return _workflowTemplateID; }
			set
			{
				if (value != _workflowTemplateID)
				{
                    object oldValue = _workflowTemplateID;
					_workflowTemplateID = value;
					RaisePropertyChanged(WorkflowInstance.Prop_WorkflowTemplateID, oldValue, value);
				}
			}
		}

		[Property("EFormMetaDataID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public string EFormMetaDataID
		{
			get { return _eFormMetaDataID; }
			set
			{
				if (value != _eFormMetaDataID)
				{
                    object oldValue = _eFormMetaDataID;
					_eFormMetaDataID = value;
					RaisePropertyChanged(WorkflowInstance.Prop_EFormMetaDataID, oldValue, value);
				}
			}
		}

		[Property("EFormInstanceID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public string EFormInstanceID
		{
			get { return _eFormInstanceID; }
			set
			{
				if (value != _eFormInstanceID)
				{
                    object oldValue = _eFormInstanceID;
					_eFormInstanceID = value;
					RaisePropertyChanged(WorkflowInstance.Prop_EFormInstanceID, oldValue, value);
				}
			}
		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
		public string Status
		{
			get { return _status; }
			set
			{
				if ((_status == null) || (value == null) || (!value.Equals(_status)))
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(WorkflowInstance.Prop_Status, oldValue, value);
				}
			}
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
					RaisePropertyChanged(WorkflowInstance.Prop_SysApplication, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_WorkFlowName, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_RelateId, oldValue, value);
				}
			}
		}

		[Property("RelateUrl", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string RelateUrl
		{
			get { return _relateUrl; }
			set
			{
				if ((_relateUrl == null) || (value == null) || (!value.Equals(_relateUrl)))
				{
                    object oldValue = _relateUrl;
					_relateUrl = value;
					RaisePropertyChanged(WorkflowInstance.Prop_RelateUrl, oldValue, value);
				}
			}
		}

		[Property("RelateData", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string RelateData
		{
			get { return _relateData; }
			set
			{
				if ((_relateData == null) || (value == null) || (!value.Equals(_relateData)))
				{
                    object oldValue = _relateData;
					_relateData = value;
					RaisePropertyChanged(WorkflowInstance.Prop_RelateData, oldValue, value);
				}
			}
		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public string Type
		{
			get { return _type; }
			set
			{
				if (value != _type)
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(WorkflowInstance.Prop_Type, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_StartTime, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_EndTime, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(WorkflowInstance.Prop_CreateTime, oldValue, value);
				}
			}
		}

		#endregion
	} // WorkflowInstance
}

