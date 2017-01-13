// Business class Leave generated from Leave
// Creator: Ray
// Created Date: [2012-03-15]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Leave")]
	public partial class Leave : ExamModelBase<Leave>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Reason = "Reason";
		public static string Prop_BeginTime = "BeginTime";
		public static string Prop_EndTime = "EndTime";
		public static string Prop_LeaveUser = "LeaveUser";
		public static string Prop_LeaveTime = "LeaveTime";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_State = "State";
		public static string Prop_AppState = "AppState";
		public static string Prop_Number = "Number";
		public static string Prop_DeptName = "DeptName";
		public static string Prop_LeaveType = "LeaveType";
		public static string Prop_LeaveDays = "LeaveDays";
		public static string Prop_WriteUser = "WriteUser";

		#endregion

		#region Private_Variables

		private string _id;
		private string _reason;
		private DateTime? _beginTime;
		private DateTime? _endTime;
		private string _leaveUser;
		private DateTime? _leaveTime;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _state;
		private string _appState;
		private string _number;
		private string _deptName;
		private string _leaveType;
		private float? _leaveDays;
		private string _writeUser;


		#endregion

		#region Constructors

		public Leave()
		{
		}

		public Leave(
			string p_id,
			string p_reason,
			DateTime? p_beginTime,
			DateTime? p_endTime,
			string p_leaveUser,
			DateTime? p_leaveTime,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_state,
			string p_appState,
			string p_number,
			string p_deptName,
			string p_leaveType,
			float? p_leaveDays,
			string p_writeUser)
		{
			_id = p_id;
			_reason = p_reason;
			_beginTime = p_beginTime;
			_endTime = p_endTime;
			_leaveUser = p_leaveUser;
			_leaveTime = p_leaveTime;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_state = p_state;
			_appState = p_appState;
			_number = p_number;
			_deptName = p_deptName;
			_leaveType = p_leaveType;
			_leaveDays = p_leaveDays;
			_writeUser = p_writeUser;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Reason", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1000)]
		public string Reason
		{
			get { return _reason; }
			set
			{
				if ((_reason == null) || (value == null) || (!value.Equals(_reason)))
				{
                    object oldValue = _reason;
					_reason = value;
					RaisePropertyChanged(Leave.Prop_Reason, oldValue, value);
				}
			}

		}

		[Property("BeginTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? BeginTime
		{
			get { return _beginTime; }
			set
			{
				if (value != _beginTime)
				{
                    object oldValue = _beginTime;
					_beginTime = value;
					RaisePropertyChanged(Leave.Prop_BeginTime, oldValue, value);
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
					RaisePropertyChanged(Leave.Prop_EndTime, oldValue, value);
				}
			}

		}

		[Property("LeaveUser", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1000)]
		public string LeaveUser
		{
			get { return _leaveUser; }
			set
			{
				if ((_leaveUser == null) || (value == null) || (!value.Equals(_leaveUser)))
				{
                    object oldValue = _leaveUser;
					_leaveUser = value;
					RaisePropertyChanged(Leave.Prop_LeaveUser, oldValue, value);
				}
			}

		}

		[Property("LeaveTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? LeaveTime
		{
			get { return _leaveTime; }
			set
			{
				if (value != _leaveTime)
				{
                    object oldValue = _leaveTime;
					_leaveTime = value;
					RaisePropertyChanged(Leave.Prop_LeaveTime, oldValue, value);
				}
			}

		}

		[Property("CreateId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CreateId
		{
			get { return _createId; }
			set
			{
				if ((_createId == null) || (value == null) || (!value.Equals(_createId)))
				{
                    object oldValue = _createId;
					_createId = value;
					RaisePropertyChanged(Leave.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Leave.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Leave.Prop_CreateTime, oldValue, value);
				}
			}

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
					RaisePropertyChanged(Leave.Prop_State, oldValue, value);
				}
			}

		}

		[Property("AppState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string AppState
		{
			get { return _appState; }
			set
			{
				if ((_appState == null) || (value == null) || (!value.Equals(_appState)))
				{
                    object oldValue = _appState;
					_appState = value;
					RaisePropertyChanged(Leave.Prop_AppState, oldValue, value);
				}
			}

		}

		[Property("Number", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Number
		{
			get { return _number; }
			set
			{
				if ((_number == null) || (value == null) || (!value.Equals(_number)))
				{
                    object oldValue = _number;
					_number = value;
					RaisePropertyChanged(Leave.Prop_Number, oldValue, value);
				}
			}

		}

		[Property("DeptName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string DeptName
		{
			get { return _deptName; }
			set
			{
				if ((_deptName == null) || (value == null) || (!value.Equals(_deptName)))
				{
                    object oldValue = _deptName;
					_deptName = value;
					RaisePropertyChanged(Leave.Prop_DeptName, oldValue, value);
				}
			}

		}

		[Property("LeaveType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string LeaveType
		{
			get { return _leaveType; }
			set
			{
				if ((_leaveType == null) || (value == null) || (!value.Equals(_leaveType)))
				{
                    object oldValue = _leaveType;
					_leaveType = value;
					RaisePropertyChanged(Leave.Prop_LeaveType, oldValue, value);
				}
			}

		}

		[Property("LeaveDays", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public float? LeaveDays
		{
			get { return _leaveDays; }
			set
			{
				if (value != _leaveDays)
				{
                    object oldValue = _leaveDays;
					_leaveDays = value;
					RaisePropertyChanged(Leave.Prop_LeaveDays, oldValue, value);
				}
			}

		}

		[Property("WriteUser", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string WriteUser
		{
			get { return _writeUser; }
			set
			{
				if ((_writeUser == null) || (value == null) || (!value.Equals(_writeUser)))
				{
                    object oldValue = _writeUser;
					_writeUser = value;
					RaisePropertyChanged(Leave.Prop_WriteUser, oldValue, value);
				}
			}

		}

		#endregion
	} // Leave
}

