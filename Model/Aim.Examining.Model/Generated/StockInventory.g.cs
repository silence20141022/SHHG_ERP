// Business class StockInventory generated from StockInventory
// Creator: Ray
// Created Date: [2012-02-28]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("StockInventory")]
	public partial class StockInventory : ExamModelBase<StockInventory>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_InventoryName = "InventoryName";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_Child = "Child";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_InventoryState = "InventoryState";

		#endregion

		#region Private_Variables

		private string _id;
		private string _inventoryName;
		private string _warehouseId;
		private string _warehouseName;
		private string _child;
		private string _state;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _inventoryState;


		#endregion

		#region Constructors

		public StockInventory()
		{
		}

		public StockInventory(
			string p_id,
			string p_inventoryName,
			string p_warehouseId,
			string p_warehouseName,
			string p_child,
			string p_state,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_inventoryState)
		{
			_id = p_id;
			_inventoryName = p_inventoryName;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_child = p_child;
			_state = p_state;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_inventoryState = p_inventoryState;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("InventoryName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string InventoryName
		{
			get { return _inventoryName; }
			set
			{
				if ((_inventoryName == null) || (value == null) || (!value.Equals(_inventoryName)))
				{
                    object oldValue = _inventoryName;
					_inventoryName = value;
					RaisePropertyChanged(StockInventory.Prop_InventoryName, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_WarehouseId, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_WarehouseName, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_Child, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_State, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(StockInventory.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("InventoryState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string InventoryState
		{
			get { return _inventoryState; }
			set
			{
				if ((_inventoryState == null) || (value == null) || (!value.Equals(_inventoryState)))
				{
                    object oldValue = _inventoryState;
					_inventoryState = value;
					RaisePropertyChanged(StockInventory.Prop_InventoryState, oldValue, value);
				}
			}

		}

		#endregion
	} // StockInventory
}

