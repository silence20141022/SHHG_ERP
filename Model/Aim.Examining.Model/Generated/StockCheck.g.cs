// Business class StockCheck generated from StockCheck
// Creator: Ray
// Created Date: [2012-08-01]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("StockCheck")]
	public partial class StockCheck : ExamModelBase<StockCheck>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_StockCheckNo = "StockCheckNo";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_StockCheckUserId = "StockCheckUserId";
		public static string Prop_StockCheckUserName = "StockCheckUserName";
		public static string Prop_StockCheckEndTime = "StockCheckEndTime";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_State = "State";
		public static string Prop_Result = "Result";
		public static string Prop_ExamineResult = "ExamineResult";
		public static string Prop_WorkFlowState = "WorkFlowState";

		#endregion

		#region Private_Variables

		private string _id;
		private string _stockCheckNo;
		private string _warehouseId;
		private string _warehouseName;
		private string _stockCheckUserId;
		private string _stockCheckUserName;
		private DateTime? _stockCheckEndTime;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _state;
		private string _result;
		private string _examineResult;
		private string _workFlowState;


		#endregion

		#region Constructors

		public StockCheck()
		{
		}

		public StockCheck(
			string p_id,
			string p_stockCheckNo,
			string p_warehouseId,
			string p_warehouseName,
			string p_stockCheckUserId,
			string p_stockCheckUserName,
			DateTime? p_stockCheckEndTime,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_state,
			string p_result,
			string p_examineResult,
			string p_workFlowState)
		{
			_id = p_id;
			_stockCheckNo = p_stockCheckNo;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_stockCheckUserId = p_stockCheckUserId;
			_stockCheckUserName = p_stockCheckUserName;
			_stockCheckEndTime = p_stockCheckEndTime;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_state = p_state;
			_result = p_result;
			_examineResult = p_examineResult;
			_workFlowState = p_workFlowState;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("StockCheckNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string StockCheckNo
		{
			get { return _stockCheckNo; }
			set
			{
				if ((_stockCheckNo == null) || (value == null) || (!value.Equals(_stockCheckNo)))
				{
                    object oldValue = _stockCheckNo;
					_stockCheckNo = value;
					RaisePropertyChanged(StockCheck.Prop_StockCheckNo, oldValue, value);
				}
			}

		}

		[Property("WarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string WarehouseId
		{
			get { return _warehouseId; }
			set
			{
				if ((_warehouseId == null) || (value == null) || (!value.Equals(_warehouseId)))
				{
                    object oldValue = _warehouseId;
					_warehouseId = value;
					RaisePropertyChanged(StockCheck.Prop_WarehouseId, oldValue, value);
				}
			}

		}

		[Property("WarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string WarehouseName
		{
			get { return _warehouseName; }
			set
			{
				if ((_warehouseName == null) || (value == null) || (!value.Equals(_warehouseName)))
				{
                    object oldValue = _warehouseName;
					_warehouseName = value;
					RaisePropertyChanged(StockCheck.Prop_WarehouseName, oldValue, value);
				}
			}

		}

		[Property("StockCheckUserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string StockCheckUserId
		{
			get { return _stockCheckUserId; }
			set
			{
				if ((_stockCheckUserId == null) || (value == null) || (!value.Equals(_stockCheckUserId)))
				{
                    object oldValue = _stockCheckUserId;
					_stockCheckUserId = value;
					RaisePropertyChanged(StockCheck.Prop_StockCheckUserId, oldValue, value);
				}
			}

		}

		[Property("StockCheckUserName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string StockCheckUserName
		{
			get { return _stockCheckUserName; }
			set
			{
				if ((_stockCheckUserName == null) || (value == null) || (!value.Equals(_stockCheckUserName)))
				{
                    object oldValue = _stockCheckUserName;
					_stockCheckUserName = value;
					RaisePropertyChanged(StockCheck.Prop_StockCheckUserName, oldValue, value);
				}
			}

		}

		[Property("StockCheckEndTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? StockCheckEndTime
		{
			get { return _stockCheckEndTime; }
			set
			{
				if (value != _stockCheckEndTime)
				{
                    object oldValue = _stockCheckEndTime;
					_stockCheckEndTime = value;
					RaisePropertyChanged(StockCheck.Prop_StockCheckEndTime, oldValue, value);
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
					RaisePropertyChanged(StockCheck.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(StockCheck.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(StockCheck.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(StockCheck.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(StockCheck.Prop_State, oldValue, value);
				}
			}

		}

		[Property("Result", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Result
		{
			get { return _result; }
			set
			{
				if ((_result == null) || (value == null) || (!value.Equals(_result)))
				{
                    object oldValue = _result;
					_result = value;
					RaisePropertyChanged(StockCheck.Prop_Result, oldValue, value);
				}
			}

		}

		[Property("ExamineResult", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ExamineResult
		{
			get { return _examineResult; }
			set
			{
				if ((_examineResult == null) || (value == null) || (!value.Equals(_examineResult)))
				{
                    object oldValue = _examineResult;
					_examineResult = value;
					RaisePropertyChanged(StockCheck.Prop_ExamineResult, oldValue, value);
				}
			}

		}

		[Property("WorkFlowState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string WorkFlowState
		{
			get { return _workFlowState; }
			set
			{
				if ((_workFlowState == null) || (value == null) || (!value.Equals(_workFlowState)))
				{
                    object oldValue = _workFlowState;
					_workFlowState = value;
					RaisePropertyChanged(StockCheck.Prop_WorkFlowState, oldValue, value);
				}
			}

		}

		#endregion
	} // StockCheck
}

