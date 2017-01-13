// Business class InWarehouse generated from InWarehouse
// Creator: Ray
// Created Date: [2014-08-14]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("InWarehouse")]
	public partial class InWarehouse : ExamModelBase<InWarehouse>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_InWarehouseNo = "InWarehouseNo";
		public static string Prop_InWarehouseType = "InWarehouseType";
		public static string Prop_State = "State";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_CheckUserId = "CheckUserId";
		public static string Prop_CheckUserName = "CheckUserName";
		public static string Prop_Remark = "Remark";
		public static string Prop_InWarehouseEndTime = "InWarehouseEndTime";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_EstimatedArrivalDate = "EstimatedArrivalDate";
		public static string Prop_PublicInterface = "PublicInterface";
		public static string Prop_Symbo = "Symbo";

		#endregion

		#region Private_Variables

		private string _id;
		private string _warehouseId;
		private string _warehouseName;
		private string _inWarehouseNo;
		private string _inWarehouseType;
		private string _state;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _checkUserId;
		private string _checkUserName;
		private string _remark;
		private DateTime? _inWarehouseEndTime;
		private string _supplierId;
		private string _supplierName;
		private DateTime? _estimatedArrivalDate;
		private string _publicInterface;
		private string _symbo;


		#endregion

		#region Constructors

		public InWarehouse()
		{
		}

		public InWarehouse(
			string p_id,
			string p_warehouseId,
			string p_warehouseName,
			string p_inWarehouseNo,
			string p_inWarehouseType,
			string p_state,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_checkUserId,
			string p_checkUserName,
			string p_remark,
			DateTime? p_inWarehouseEndTime,
			string p_supplierId,
			string p_supplierName,
			DateTime? p_estimatedArrivalDate,
			string p_publicInterface,
			string p_symbo)
		{
			_id = p_id;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_inWarehouseNo = p_inWarehouseNo;
			_inWarehouseType = p_inWarehouseType;
			_state = p_state;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_checkUserId = p_checkUserId;
			_checkUserName = p_checkUserName;
			_remark = p_remark;
			_inWarehouseEndTime = p_inWarehouseEndTime;
			_supplierId = p_supplierId;
			_supplierName = p_supplierName;
			_estimatedArrivalDate = p_estimatedArrivalDate;
			_publicInterface = p_publicInterface;
			_symbo = p_symbo;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(InWarehouse.Prop_WarehouseId, oldValue, value);
				}
			}

		}

		[Property("WarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string WarehouseName
		{
			get { return _warehouseName; }
			set
			{
				if ((_warehouseName == null) || (value == null) || (!value.Equals(_warehouseName)))
				{
                    object oldValue = _warehouseName;
					_warehouseName = value;
					RaisePropertyChanged(InWarehouse.Prop_WarehouseName, oldValue, value);
				}
			}

		}

		[Property("InWarehouseNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InWarehouseNo
		{
			get { return _inWarehouseNo; }
			set
			{
				if ((_inWarehouseNo == null) || (value == null) || (!value.Equals(_inWarehouseNo)))
				{
                    object oldValue = _inWarehouseNo;
					_inWarehouseNo = value;
					RaisePropertyChanged(InWarehouse.Prop_InWarehouseNo, oldValue, value);
				}
			}

		}

		[Property("InWarehouseType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InWarehouseType
		{
			get { return _inWarehouseType; }
			set
			{
				if ((_inWarehouseType == null) || (value == null) || (!value.Equals(_inWarehouseType)))
				{
                    object oldValue = _inWarehouseType;
					_inWarehouseType = value;
					RaisePropertyChanged(InWarehouse.Prop_InWarehouseType, oldValue, value);
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
					RaisePropertyChanged(InWarehouse.Prop_State, oldValue, value);
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
					RaisePropertyChanged(InWarehouse.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(InWarehouse.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(InWarehouse.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("CheckUserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CheckUserId
		{
			get { return _checkUserId; }
			set
			{
				if ((_checkUserId == null) || (value == null) || (!value.Equals(_checkUserId)))
				{
                    object oldValue = _checkUserId;
					_checkUserId = value;
					RaisePropertyChanged(InWarehouse.Prop_CheckUserId, oldValue, value);
				}
			}

		}

		[Property("CheckUserName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CheckUserName
		{
			get { return _checkUserName; }
			set
			{
				if ((_checkUserName == null) || (value == null) || (!value.Equals(_checkUserName)))
				{
                    object oldValue = _checkUserName;
					_checkUserName = value;
					RaisePropertyChanged(InWarehouse.Prop_CheckUserName, oldValue, value);
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
					RaisePropertyChanged(InWarehouse.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("InWarehouseEndTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? InWarehouseEndTime
		{
			get { return _inWarehouseEndTime; }
			set
			{
				if (value != _inWarehouseEndTime)
				{
                    object oldValue = _inWarehouseEndTime;
					_inWarehouseEndTime = value;
					RaisePropertyChanged(InWarehouse.Prop_InWarehouseEndTime, oldValue, value);
				}
			}

		}

		[Property("SupplierId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string SupplierId
		{
			get { return _supplierId; }
			set
			{
				if ((_supplierId == null) || (value == null) || (!value.Equals(_supplierId)))
				{
                    object oldValue = _supplierId;
					_supplierId = value;
					RaisePropertyChanged(InWarehouse.Prop_SupplierId, oldValue, value);
				}
			}

		}

		[Property("SupplierName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SupplierName
		{
			get { return _supplierName; }
			set
			{
				if ((_supplierName == null) || (value == null) || (!value.Equals(_supplierName)))
				{
                    object oldValue = _supplierName;
					_supplierName = value;
					RaisePropertyChanged(InWarehouse.Prop_SupplierName, oldValue, value);
				}
			}

		}

		[Property("EstimatedArrivalDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? EstimatedArrivalDate
		{
			get { return _estimatedArrivalDate; }
			set
			{
				if (value != _estimatedArrivalDate)
				{
                    object oldValue = _estimatedArrivalDate;
					_estimatedArrivalDate = value;
					RaisePropertyChanged(InWarehouse.Prop_EstimatedArrivalDate, oldValue, value);
				}
			}

		}

		[Property("PublicInterface", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PublicInterface
		{
			get { return _publicInterface; }
			set
			{
				if ((_publicInterface == null) || (value == null) || (!value.Equals(_publicInterface)))
				{
                    object oldValue = _publicInterface;
					_publicInterface = value;
					RaisePropertyChanged(InWarehouse.Prop_PublicInterface, oldValue, value);
				}
			}

		}

		[Property("Symbo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Symbo
		{
			get { return _symbo; }
			set
			{
				if ((_symbo == null) || (value == null) || (!value.Equals(_symbo)))
				{
                    object oldValue = _symbo;
					_symbo = value;
					RaisePropertyChanged(InWarehouse.Prop_Symbo, oldValue, value);
				}
			}

		}

		#endregion
	} // InWarehouse
}

