// Business class ChangeProduct generated from ChangeProduct
// Creator: Ray
// Created Date: [2012-04-12]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ChangeProduct")]
	public partial class ChangeProduct : ExamModelBase<ChangeProduct>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PId = "PId";
		public static string Prop_Number = "Number";
		public static string Prop_OrderNumber = "OrderNumber";
		public static string Prop_CId = "CId";
		public static string Prop_CCode = "CCode";
		public static string Prop_CName = "CName";
		public static string Prop_ReturnMoney = "ReturnMoney";
		public static string Prop_Child = "Child";
		public static string Prop_Child2 = "Child2";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_InWarehouseId = "InWarehouseId";
		public static string Prop_InWarehouseName = "InWarehouseName";
		public static string Prop_OutWarehouseId = "OutWarehouseId";
		public static string Prop_OutWarehouseName = "OutWarehouseName";

		#endregion

		#region Private_Variables

		private string _id;
		private string _pId;
		private string _number;
		private string _orderNumber;
		private string _cId;
		private string _cCode;
		private string _cName;
		private System.Decimal? _returnMoney;
		private string _child;
		private string _child2;
		private string _state;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _inWarehouseId;
		private string _inWarehouseName;
		private string _outWarehouseId;
		private string _outWarehouseName;


		#endregion

		#region Constructors

		public ChangeProduct()
		{
		}

		public ChangeProduct(
			string p_id,
			string p_pId,
			string p_number,
			string p_orderNumber,
			string p_cId,
			string p_cCode,
			string p_cName,
			System.Decimal? p_returnMoney,
			string p_child,
			string p_child2,
			string p_state,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_inWarehouseId,
			string p_inWarehouseName,
			string p_outWarehouseId,
			string p_outWarehouseName)
		{
			_id = p_id;
			_pId = p_pId;
			_number = p_number;
			_orderNumber = p_orderNumber;
			_cId = p_cId;
			_cCode = p_cCode;
			_cName = p_cName;
			_returnMoney = p_returnMoney;
			_child = p_child;
			_child2 = p_child2;
			_state = p_state;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_inWarehouseId = p_inWarehouseId;
			_inWarehouseName = p_inWarehouseName;
			_outWarehouseId = p_outWarehouseId;
			_outWarehouseName = p_outWarehouseName;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("PId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PId
		{
			get { return _pId; }
			set
			{
				if ((_pId == null) || (value == null) || (!value.Equals(_pId)))
				{
                    object oldValue = _pId;
					_pId = value;
					RaisePropertyChanged(ChangeProduct.Prop_PId, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_Number, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_OrderNumber, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_CId, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_CCode, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_CName, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_ReturnMoney, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_Child, oldValue, value);
				}
			}

		}

		[Property("Child2", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Child2
		{
			get { return _child2; }
			set
			{
				if ((_child2 == null) || (value == null) || (!value.Equals(_child2)))
				{
                    object oldValue = _child2;
					_child2 = value;
					RaisePropertyChanged(ChangeProduct.Prop_Child2, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_State, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(ChangeProduct.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("InWarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InWarehouseId
		{
			get { return _inWarehouseId; }
			set
			{
				if ((_inWarehouseId == null) || (value == null) || (!value.Equals(_inWarehouseId)))
				{
                    object oldValue = _inWarehouseId;
					_inWarehouseId = value;
					RaisePropertyChanged(ChangeProduct.Prop_InWarehouseId, oldValue, value);
				}
			}

		}

		[Property("InWarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string InWarehouseName
		{
			get { return _inWarehouseName; }
			set
			{
				if ((_inWarehouseName == null) || (value == null) || (!value.Equals(_inWarehouseName)))
				{
                    object oldValue = _inWarehouseName;
					_inWarehouseName = value;
					RaisePropertyChanged(ChangeProduct.Prop_InWarehouseName, oldValue, value);
				}
			}

		}

		[Property("OutWarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OutWarehouseId
		{
			get { return _outWarehouseId; }
			set
			{
				if ((_outWarehouseId == null) || (value == null) || (!value.Equals(_outWarehouseId)))
				{
                    object oldValue = _outWarehouseId;
					_outWarehouseId = value;
					RaisePropertyChanged(ChangeProduct.Prop_OutWarehouseId, oldValue, value);
				}
			}

		}

		[Property("OutWarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string OutWarehouseName
		{
			get { return _outWarehouseName; }
			set
			{
				if ((_outWarehouseName == null) || (value == null) || (!value.Equals(_outWarehouseName)))
				{
                    object oldValue = _outWarehouseName;
					_outWarehouseName = value;
					RaisePropertyChanged(ChangeProduct.Prop_OutWarehouseName, oldValue, value);
				}
			}

		}

		#endregion
	} // ChangeProduct
}

