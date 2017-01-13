// Business class PriceApply generated from PriceApply
// Creator: Ray
// Created Date: [2012-02-17]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PriceApply")]
	public partial class PriceApply : ExamModelBase<PriceApply>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Number = "Number";
		public static string Prop_CId = "CId";
		public static string Prop_CCode = "CCode";
		public static string Prop_CName = "CName";
		public static string Prop_ExpectedTime = "ExpectedTime";
		public static string Prop_Child = "Child";
		public static string Prop_Reason = "Reason";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_ApprovalState = "ApprovalState";
		public static string Prop_PreDeposit = "PreDeposit";
		public static string Prop_OId = "OId";

		#endregion

		#region Private_Variables

		private string _id;
		private string _number;
		private string _cId;
		private string _cCode;
		private string _cName;
		private DateTime? _expectedTime;
		private string _child;
		private string _reason;
		private string _state;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _approvalState;
		private string _preDeposit;
		private string _oId;


		#endregion

		#region Constructors

		public PriceApply()
		{
		}

		public PriceApply(
			string p_id,
			string p_number,
			string p_cId,
			string p_cCode,
			string p_cName,
			DateTime? p_expectedTime,
			string p_child,
			string p_reason,
			string p_state,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_approvalState,
			string p_preDeposit,
			string p_oId)
		{
			_id = p_id;
			_number = p_number;
			_cId = p_cId;
			_cCode = p_cCode;
			_cName = p_cName;
			_expectedTime = p_expectedTime;
			_child = p_child;
			_reason = p_reason;
			_state = p_state;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_approvalState = p_approvalState;
			_preDeposit = p_preDeposit;
			_oId = p_oId;
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
					RaisePropertyChanged(PriceApply.Prop_Number, oldValue, value);
				}
			}

		}

		[Property("CId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CId
		{
			get { return _cId; }
			set
			{
				if ((_cId == null) || (value == null) || (!value.Equals(_cId)))
				{
                    object oldValue = _cId;
					_cId = value;
					RaisePropertyChanged(PriceApply.Prop_CId, oldValue, value);
				}
			}

		}

		[Property("CCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string CCode
		{
			get { return _cCode; }
			set
			{
				if ((_cCode == null) || (value == null) || (!value.Equals(_cCode)))
				{
                    object oldValue = _cCode;
					_cCode = value;
					RaisePropertyChanged(PriceApply.Prop_CCode, oldValue, value);
				}
			}

		}

		[Property("CName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string CName
		{
			get { return _cName; }
			set
			{
				if ((_cName == null) || (value == null) || (!value.Equals(_cName)))
				{
                    object oldValue = _cName;
					_cName = value;
					RaisePropertyChanged(PriceApply.Prop_CName, oldValue, value);
				}
			}

		}

		[Property("ExpectedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? ExpectedTime
		{
			get { return _expectedTime; }
			set
			{
				if (value != _expectedTime)
				{
                    object oldValue = _expectedTime;
					_expectedTime = value;
					RaisePropertyChanged(PriceApply.Prop_ExpectedTime, oldValue, value);
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
					RaisePropertyChanged(PriceApply.Prop_Child, oldValue, value);
				}
			}

		}

		[Property("Reason", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 2000)]
		public string Reason
		{
			get { return _reason; }
			set
			{
				if ((_reason == null) || (value == null) || (!value.Equals(_reason)))
				{
                    object oldValue = _reason;
					_reason = value;
					RaisePropertyChanged(PriceApply.Prop_Reason, oldValue, value);
				}
			}

		}

		[Property("State", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string State
		{
			get { return _state; }
			set
			{
				if ((_state == null) || (value == null) || (!value.Equals(_state)))
				{
                    object oldValue = _state;
					_state = value;
					RaisePropertyChanged(PriceApply.Prop_State, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 2000)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(PriceApply.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(PriceApply.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(PriceApply.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(PriceApply.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("ApprovalState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ApprovalState
		{
			get { return _approvalState; }
			set
			{
				if ((_approvalState == null) || (value == null) || (!value.Equals(_approvalState)))
				{
                    object oldValue = _approvalState;
					_approvalState = value;
					RaisePropertyChanged(PriceApply.Prop_ApprovalState, oldValue, value);
				}
			}

		}

		[Property("PreDeposit", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PreDeposit
		{
			get { return _preDeposit; }
			set
			{
				if ((_preDeposit == null) || (value == null) || (!value.Equals(_preDeposit)))
				{
                    object oldValue = _preDeposit;
					_preDeposit = value;
					RaisePropertyChanged(PriceApply.Prop_PreDeposit, oldValue, value);
				}
			}

		}

		[Property("OId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OId
		{
			get { return _oId; }
			set
			{
				if ((_oId == null) || (value == null) || (!value.Equals(_oId)))
				{
                    object oldValue = _oId;
					_oId = value;
					RaisePropertyChanged(PriceApply.Prop_OId, oldValue, value);
				}
			}

		}

		#endregion
	} // PriceApply
}

