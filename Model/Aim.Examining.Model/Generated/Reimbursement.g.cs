// Business class Reimbursement generated from Reimbursement
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
	[ActiveRecord("Reimbursement")]
	public partial class Reimbursement : ExamModelBase<Reimbursement>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Amount = "Amount";
		public static string Prop_Reason = "Reason";
		public static string Prop_LeaveUser = "LeaveUser";
		public static string Prop_State = "State";
		public static string Prop_AppState = "AppState";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Number = "Number";
		public static string Prop_DeptName = "DeptName";
		public static string Prop_Child = "Child";
		public static string Prop_WriteUser = "WriteUser";

		#endregion

		#region Private_Variables

		private string _id;
		private System.Decimal? _amount;
		private string _reason;
		private string _leaveUser;
		private string _state;
		private string _appState;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _number;
		private string _deptName;
		private string _child;
		private string _writeUser;


		#endregion

		#region Constructors

		public Reimbursement()
		{
		}

		public Reimbursement(
			string p_id,
			System.Decimal? p_amount,
			string p_reason,
			string p_leaveUser,
			string p_state,
			string p_appState,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_number,
			string p_deptName,
			string p_child,
			string p_writeUser)
		{
			_id = p_id;
			_amount = p_amount;
			_reason = p_reason;
			_leaveUser = p_leaveUser;
			_state = p_state;
			_appState = p_appState;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_number = p_number;
			_deptName = p_deptName;
			_child = p_child;
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

		[Property("Amount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Amount
		{
			get { return _amount; }
			set
			{
				if (value != _amount)
				{
                    object oldValue = _amount;
					_amount = value;
					RaisePropertyChanged(Reimbursement.Prop_Amount, oldValue, value);
				}
			}

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
					RaisePropertyChanged(Reimbursement.Prop_Reason, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_LeaveUser, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_State, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_AppState, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_Number, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_DeptName, oldValue, value);
				}
			}

		}

		[Property("Child", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Child
		{
			get { return _child; }
			set
			{
				if ((_child == null) || (value == null) || (!value.Equals(_child)))
				{
                    object oldValue = _child;
					_child = value;
					RaisePropertyChanged(Reimbursement.Prop_Child, oldValue, value);
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
					RaisePropertyChanged(Reimbursement.Prop_WriteUser, oldValue, value);
				}
			}

		}

		#endregion
	} // Reimbursement
}

