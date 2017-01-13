// Business class OtherCost generated from OtherCosts
// Creator: Ray
// Created Date: [2012-03-28]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("OtherCosts")]
	public partial class OtherCost : ExamModelBase<OtherCost>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Number = "Number";
		public static string Prop_DeptName = "DeptName";
		public static string Prop_Child = "Child";
		public static string Prop_Amount = "Amount";
		public static string Prop_Reason = "Reason";
		public static string Prop_LeaveUser = "LeaveUser";
		public static string Prop_State = "State";
		public static string Prop_AppState = "AppState";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _number;
		private string _deptName;
		private string _child;
		private System.Decimal? _amount;
		private string _reason;
		private string _leaveUser;
		private string _state;
		private string _appState;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public OtherCost()
		{
		}

		public OtherCost(
			string p_id,
			string p_number,
			string p_deptName,
			string p_child,
			System.Decimal? p_amount,
			string p_reason,
			string p_leaveUser,
			string p_state,
			string p_appState,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_number = p_number;
			_deptName = p_deptName;
			_child = p_child;
			_amount = p_amount;
			_reason = p_reason;
			_leaveUser = p_leaveUser;
			_state = p_state;
			_appState = p_appState;
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
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(OtherCost.Prop_Number, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_DeptName, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_Child, oldValue, value);
				}
			}

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
					RaisePropertyChanged(OtherCost.Prop_Amount, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_Reason, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_LeaveUser, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_State, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_AppState, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(OtherCost.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // OtherCost
}

