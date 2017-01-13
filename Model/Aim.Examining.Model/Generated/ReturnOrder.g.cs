// Business class ReturnOrder generated from ReturnOrder
// Creator: Ray
// Created Date: [2013-07-12]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ReturnOrder")]
	public partial class ReturnOrder : ExamModelBase<ReturnOrder>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Number = "Number";
		public static string Prop_OrderNumber = "OrderNumber";
		public static string Prop_CId = "CId";
		public static string Prop_CName = "CName";
		public static string Prop_ReturnMoney = "ReturnMoney";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_IsDiscount = "IsDiscount";
		public static string Prop_DiscountState = "DiscountState";

		#endregion

		#region Private_Variables

		private string _id;
		private string _number;
		private string _orderNumber;
		private string _cId;
		private string _cName;
		private System.Decimal? _returnMoney;
		private string _state;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _warehouseId;
		private string _warehouseName;
		private string _isDiscount;
		private string _discountState;


		#endregion

		#region Constructors

		public ReturnOrder()
		{
		}

		public ReturnOrder(
			string p_id,
			string p_number,
			string p_orderNumber,
			string p_cId,
			string p_cName,
			System.Decimal? p_returnMoney,
			string p_state,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_warehouseId,
			string p_warehouseName,
			string p_isDiscount,
			string p_discountState)
		{
			_id = p_id;
			_number = p_number;
			_orderNumber = p_orderNumber;
			_cId = p_cId;
			_cName = p_cName;
			_returnMoney = p_returnMoney;
			_state = p_state;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_isDiscount = p_isDiscount;
			_discountState = p_discountState;
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
					RaisePropertyChanged(ReturnOrder.Prop_Number, oldValue, value);
				}
			}

		}

		[Property("OrderNumber", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string OrderNumber
		{
			get { return _orderNumber; }
			set
			{
				if ((_orderNumber == null) || (value == null) || (!value.Equals(_orderNumber)))
				{
                    object oldValue = _orderNumber;
					_orderNumber = value;
					RaisePropertyChanged(ReturnOrder.Prop_OrderNumber, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_CId, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_CName, oldValue, value);
				}
			}

		}

		[Property("ReturnMoney", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ReturnMoney
		{
			get { return _returnMoney; }
			set
			{
				if (value != _returnMoney)
				{
                    object oldValue = _returnMoney;
					_returnMoney = value;
					RaisePropertyChanged(ReturnOrder.Prop_ReturnMoney, oldValue, value);
				}
			}

		}

		[Property("State", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string State
		{
			get { return _state; }
			set
			{
				if ((_state == null) || (value == null) || (!value.Equals(_state)))
				{
                    object oldValue = _state;
					_state = value;
					RaisePropertyChanged(ReturnOrder.Prop_State, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_WarehouseId, oldValue, value);
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
					RaisePropertyChanged(ReturnOrder.Prop_WarehouseName, oldValue, value);
				}
			}

		}

		[Property("IsDiscount", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string IsDiscount
		{
			get { return _isDiscount; }
			set
			{
				if ((_isDiscount == null) || (value == null) || (!value.Equals(_isDiscount)))
				{
                    object oldValue = _isDiscount;
					_isDiscount = value;
					RaisePropertyChanged(ReturnOrder.Prop_IsDiscount, oldValue, value);
				}
			}

		}

		[Property("DiscountState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string DiscountState
		{
			get { return _discountState; }
			set
			{
				if ((_discountState == null) || (value == null) || (!value.Equals(_discountState)))
				{
                    object oldValue = _discountState;
					_discountState = value;
					RaisePropertyChanged(ReturnOrder.Prop_DiscountState, oldValue, value);
				}
			}

		}

		#endregion
	} // ReturnOrder
}

