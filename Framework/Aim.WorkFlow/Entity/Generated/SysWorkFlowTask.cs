// Business class SysWorkFlowTask generated from SysWorkFlowTask
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
	[ActiveRecord("SysWorkFlowTask")]
	public partial class SysWorkFlowTask : EntityBase<SysWorkFlowTask>
	
	{

		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Title = "Title";
		public static string Prop_Description = "Description";
		public static string Prop_OwnerId = "OwnerId";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_Action = "Action";
		public static string Prop_WorkFlowInstanceId = "WorkFlowInstanceId";
		public static string Prop_WorkFlowName = "WorkFlowName";
		public static string Prop_EFormName = "EFormName";
		public static string Prop_ApprovalNodeName = "ApprovalNodeName";
		public static string Prop_GroupId = "GroupId";
		public static string Prop_ApprovalNodeMathConditionType = "ApprovalNodeMathConditionType";
		public static string Prop_BookMarkName = "BookMarkName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_FinishTime = "FinishTime";
		public static string Prop_Status = "Status";
		public static string Prop_Context = "Context";
		public static string Prop_Result = "Result";

		#endregion

		#region Private_Variables

		private string _id;
		private string _title;
		private string _description;
		private string _ownerId;
		private string _ownerName;
		private string _action;
		private string _workFlowInstanceId;
		private string _workFlowName;
		private string _eFormName;
		private string _approvalNodeName;
		private string _groupId;
		private int? _approvalNodeMathConditionType;
		private string _bookMarkName;
		private DateTime? _createTime;
		private DateTime? _finishTime;
		private int? _status;
		private byte[] _context;
		private string _result;


		#endregion

		#region Constructors

		public SysWorkFlowTask()
		{
		}

		public SysWorkFlowTask(
			string p_id,
			string p_title,
			string p_description,
			string p_ownerId,
			string p_ownerName,
			string p_action,
			string p_workFlowInstanceId,
			string p_workFlowName,
			string p_eFormName,
			string p_approvalNodeName,
			string p_groupId,
			int? p_approvalNodeMathConditionType,
			string p_bookMarkName,
			DateTime? p_createTime,
			DateTime? p_finishTime,
			int? p_status,
			byte[] p_context,
			string p_result)
		{
			_id = p_id;
			_title = p_title;
			_description = p_description;
			_ownerId = p_ownerId;
			_ownerName = p_ownerName;
			_action = p_action;
			_workFlowInstanceId = p_workFlowInstanceId;
			_workFlowName = p_workFlowName;
			_eFormName = p_eFormName;
			_approvalNodeName = p_approvalNodeName;
			_groupId = p_groupId;
			_approvalNodeMathConditionType = p_approvalNodeMathConditionType;
			_bookMarkName = p_bookMarkName;
			_createTime = p_createTime;
			_finishTime = p_finishTime;
			_status = p_status;
			_context = p_context;
			_result = p_result;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 80)]
		public string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_Title, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowTask.Prop_Description, oldValue, value);
				}
			}
		}

		[Property("OwnerId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OwnerId
		{
			get { return _ownerId; }
			set
			{
				if ((_ownerId == null) || (value == null) || (!value.Equals(_ownerId)))
				{
                    object oldValue = _ownerId;
					_ownerId = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_OwnerId, oldValue, value);
				}
			}
		}

		[Property("OwnerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string OwnerName
		{
			get { return _ownerName; }
			set
			{
				if ((_ownerName == null) || (value == null) || (!value.Equals(_ownerName)))
				{
                    object oldValue = _ownerName;
					_ownerName = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_OwnerName, oldValue, value);
				}
			}
		}

		[Property("Action", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Action
		{
			get { return _action; }
			set
			{
				if ((_action == null) || (value == null) || (!value.Equals(_action)))
				{
                    object oldValue = _action;
					_action = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_Action, oldValue, value);
				}
			}
		}

		[Property("WorkFlowInstanceId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string WorkFlowInstanceId
		{
			get { return _workFlowInstanceId; }
			set
			{
				if ((_workFlowInstanceId == null) || (value == null) || (!value.Equals(_workFlowInstanceId)))
				{
                    object oldValue = _workFlowInstanceId;
					_workFlowInstanceId = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_WorkFlowInstanceId, oldValue, value);
				}
			}
		}

		[Property("WorkFlowName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string WorkFlowName
		{
			get { return _workFlowName; }
			set
			{
				if ((_workFlowName == null) || (value == null) || (!value.Equals(_workFlowName)))
				{
                    object oldValue = _workFlowName;
					_workFlowName = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_WorkFlowName, oldValue, value);
				}
			}
		}

		[Property("EFormName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 300)]
		public string EFormName
		{
			get { return _eFormName; }
			set
			{
				if ((_eFormName == null) || (value == null) || (!value.Equals(_eFormName)))
				{
                    object oldValue = _eFormName;
					_eFormName = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_EFormName, oldValue, value);
				}
			}
		}

		[Property("ApprovalNodeName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ApprovalNodeName
		{
			get { return _approvalNodeName; }
			set
			{
				if ((_approvalNodeName == null) || (value == null) || (!value.Equals(_approvalNodeName)))
				{
                    object oldValue = _approvalNodeName;
					_approvalNodeName = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_ApprovalNodeName, oldValue, value);
				}
			}
		}

		[Property("GroupId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string GroupId
		{
			get { return _groupId; }
			set
			{
				if ((_groupId == null) || (value == null) || (!value.Equals(_groupId)))
				{
                    object oldValue = _groupId;
					_groupId = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_GroupId, oldValue, value);
				}
			}
		}

		[Property("ApprovalNodeMathConditionType", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? ApprovalNodeMathConditionType
		{
			get { return _approvalNodeMathConditionType; }
			set
			{
				if (value != _approvalNodeMathConditionType)
				{
                    object oldValue = _approvalNodeMathConditionType;
					_approvalNodeMathConditionType = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_ApprovalNodeMathConditionType, oldValue, value);
				}
			}
		}

		[Property("BookMarkName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string BookMarkName
		{
			get { return _bookMarkName; }
			set
			{
				if ((_bookMarkName == null) || (value == null) || (!value.Equals(_bookMarkName)))
				{
                    object oldValue = _bookMarkName;
					_bookMarkName = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_BookMarkName, oldValue, value);
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
					RaisePropertyChanged(SysWorkFlowTask.Prop_CreateTime, oldValue, value);
				}
			}
		}

		[Property("FinishTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? FinishTime
		{
			get { return _finishTime; }
			set
			{
				if (value != _finishTime)
				{
                    object oldValue = _finishTime;
					_finishTime = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_FinishTime, oldValue, value);
				}
			}
		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Status
		{
			get { return _status; }
			set
			{
				if (value != _status)
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_Status, oldValue, value);
				}
			}
		}

		[Property("Context", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public byte[] Context
		{
			get { return _context; }
			set
			{
				if (value != _context)
				{
                    object oldValue = _context;
					_context = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_Context, oldValue, value);
				}
			}
		}

		[Property("Result", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Result
		{
			get { return _result; }
			set
			{
				if ((_result == null) || (value == null) || (!value.Equals(_result)))
				{
                    object oldValue = _result;
					_result = value;
					RaisePropertyChanged(SysWorkFlowTask.Prop_Result, oldValue, value);
				}
			}
		}

		#endregion
	} // SysWorkFlowTask
}

