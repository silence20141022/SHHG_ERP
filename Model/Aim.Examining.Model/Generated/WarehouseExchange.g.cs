// Business class WarehouseExchange generated from WarehouseExchange
// Creator: Ray
// Created Date: [2012-05-18]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("WarehouseExchange")]
	public partial class WarehouseExchange : ExamModelBase<WarehouseExchange>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_ExchangeNo = "ExchangeNo";
		public static string Prop_FromWarehouseId = "FromWarehouseId";
		public static string Prop_FromWarehouseName = "FromWarehouseName";
		public static string Prop_ToWarehouseId = "ToWarehouseId";
		public static string Prop_ToWarehouseName = "ToWarehouseName";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";
		public static string Prop_OutWarehouseState = "OutWarehouseState";
		public static string Prop_InWarehouseState = "InWarehouseState";
		public static string Prop_ExchangeState = "ExchangeState";
		public static string Prop_EndTime = "EndTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _exchangeNo;
		private string _fromWarehouseId;
		private string _fromWarehouseName;
		private string _toWarehouseId;
		private string _toWarehouseName;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;
		private string _outWarehouseState;
		private string _inWarehouseState;
		private string _exchangeState;
		private DateTime? _endTime;


		#endregion

		#region Constructors

		public WarehouseExchange()
		{
		}

		public WarehouseExchange(
			string p_id,
			string p_exchangeNo,
			string p_fromWarehouseId,
			string p_fromWarehouseName,
			string p_toWarehouseId,
			string p_toWarehouseName,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark,
			string p_outWarehouseState,
			string p_inWarehouseState,
			string p_exchangeState,
			DateTime? p_endTime)
		{
			_id = p_id;
			_exchangeNo = p_exchangeNo;
			_fromWarehouseId = p_fromWarehouseId;
			_fromWarehouseName = p_fromWarehouseName;
			_toWarehouseId = p_toWarehouseId;
			_toWarehouseName = p_toWarehouseName;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_remark = p_remark;
			_outWarehouseState = p_outWarehouseState;
			_inWarehouseState = p_inWarehouseState;
			_exchangeState = p_exchangeState;
			_endTime = p_endTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("ExchangeNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ExchangeNo
		{
			get { return _exchangeNo; }
			set
			{
				if ((_exchangeNo == null) || (value == null) || (!value.Equals(_exchangeNo)))
				{
                    object oldValue = _exchangeNo;
					_exchangeNo = value;
					RaisePropertyChanged(WarehouseExchange.Prop_ExchangeNo, oldValue, value);
				}
			}

		}

		[Property("FromWarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string FromWarehouseId
		{
			get { return _fromWarehouseId; }
			set
			{
				if ((_fromWarehouseId == null) || (value == null) || (!value.Equals(_fromWarehouseId)))
				{
                    object oldValue = _fromWarehouseId;
					_fromWarehouseId = value;
					RaisePropertyChanged(WarehouseExchange.Prop_FromWarehouseId, oldValue, value);
				}
			}

		}

		[Property("FromWarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string FromWarehouseName
		{
			get { return _fromWarehouseName; }
			set
			{
				if ((_fromWarehouseName == null) || (value == null) || (!value.Equals(_fromWarehouseName)))
				{
                    object oldValue = _fromWarehouseName;
					_fromWarehouseName = value;
					RaisePropertyChanged(WarehouseExchange.Prop_FromWarehouseName, oldValue, value);
				}
			}

		}

		[Property("ToWarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ToWarehouseId
		{
			get { return _toWarehouseId; }
			set
			{
				if ((_toWarehouseId == null) || (value == null) || (!value.Equals(_toWarehouseId)))
				{
                    object oldValue = _toWarehouseId;
					_toWarehouseId = value;
					RaisePropertyChanged(WarehouseExchange.Prop_ToWarehouseId, oldValue, value);
				}
			}

		}

		[Property("ToWarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ToWarehouseName
		{
			get { return _toWarehouseName; }
			set
			{
				if ((_toWarehouseName == null) || (value == null) || (!value.Equals(_toWarehouseName)))
				{
                    object oldValue = _toWarehouseName;
					_toWarehouseName = value;
					RaisePropertyChanged(WarehouseExchange.Prop_ToWarehouseName, oldValue, value);
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
					RaisePropertyChanged(WarehouseExchange.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(WarehouseExchange.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(WarehouseExchange.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(WarehouseExchange.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("OutWarehouseState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OutWarehouseState
		{
			get { return _outWarehouseState; }
			set
			{
				if ((_outWarehouseState == null) || (value == null) || (!value.Equals(_outWarehouseState)))
				{
                    object oldValue = _outWarehouseState;
					_outWarehouseState = value;
					RaisePropertyChanged(WarehouseExchange.Prop_OutWarehouseState, oldValue, value);
				}
			}

		}

		[Property("InWarehouseState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InWarehouseState
		{
			get { return _inWarehouseState; }
			set
			{
				if ((_inWarehouseState == null) || (value == null) || (!value.Equals(_inWarehouseState)))
				{
                    object oldValue = _inWarehouseState;
					_inWarehouseState = value;
					RaisePropertyChanged(WarehouseExchange.Prop_InWarehouseState, oldValue, value);
				}
			}

		}

		[Property("ExchangeState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ExchangeState
		{
			get { return _exchangeState; }
			set
			{
				if ((_exchangeState == null) || (value == null) || (!value.Equals(_exchangeState)))
				{
                    object oldValue = _exchangeState;
					_exchangeState = value;
					RaisePropertyChanged(WarehouseExchange.Prop_ExchangeState, oldValue, value);
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
					RaisePropertyChanged(WarehouseExchange.Prop_EndTime, oldValue, value);
				}
			}

		}

		#endregion
	} // WarehouseExchange
}

