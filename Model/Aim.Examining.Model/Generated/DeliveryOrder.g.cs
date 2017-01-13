// Business class DeliveryOrder generated from DeliveryOrder
// Creator: Ray
// Created Date: [2013-06-19]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("DeliveryOrder")]
	public partial class DeliveryOrder : ExamModelBase<DeliveryOrder>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PId = "PId";
		public static string Prop_Number = "Number";
		public static string Prop_CId = "CId";
		public static string Prop_CName = "CName";
		public static string Prop_ExpectedTime = "ExpectedTime";
		public static string Prop_Child = "Child";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_TotalMoneyHis = "TotalMoneyHis";
		public static string Prop_TotalMoney = "TotalMoney";
		public static string Prop_CorrespondState = "CorrespondState";
		public static string Prop_CorrespondInvoice = "CorrespondInvoice";
		public static string Prop_DeliveryType = "DeliveryType";
		public static string Prop_Salesman = "Salesman";
		public static string Prop_Address = "Address";
		public static string Prop_DeliveryMode = "DeliveryMode";
		public static string Prop_Tel = "Tel";
		public static string Prop_SalesmanId = "SalesmanId";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_LogisticState = "LogisticState";
		public static string Prop_DeliveryUser = "DeliveryUser";
		public static string Prop_DeliveryUserId = "DeliveryUserId";

		#endregion

		#region Private_Variables

		private string _id;
		private string _pId;
		private string _number;
		private string _cId;
		private string _cName;
		private DateTime? _expectedTime;
		private string _child;
		private string _state;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private System.Decimal? _totalMoneyHis;
		private System.Decimal? _totalMoney;
		private string _correspondState;
		private string _correspondInvoice;
		private string _deliveryType;
		private string _salesman;
		private string _address;
		private string _deliveryMode;
		private string _tel;
		private string _salesmanId;
		private string _warehouseId;
		private string _warehouseName;
		private string _logisticState;
		private string _deliveryUser;
		private string _deliveryUserId;


		#endregion

		#region Constructors

		public DeliveryOrder()
		{
		}

		public DeliveryOrder(
			string p_id,
			string p_pId,
			string p_number,
			string p_cId,
			string p_cName,
			DateTime? p_expectedTime,
			string p_child,
			string p_state,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			System.Decimal? p_totalMoneyHis,
			System.Decimal? p_totalMoney,
			string p_correspondState,
			string p_correspondInvoice,
			string p_deliveryType,
			string p_salesman,
			string p_address,
			string p_deliveryMode,
			string p_tel,
			string p_salesmanId,
			string p_warehouseId,
			string p_warehouseName,
			string p_logisticState,
			string p_deliveryUser,
			string p_deliveryUserId)
		{
			_id = p_id;
			_pId = p_pId;
			_number = p_number;
			_cId = p_cId;
			_cName = p_cName;
			_expectedTime = p_expectedTime;
			_child = p_child;
			_state = p_state;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_totalMoneyHis = p_totalMoneyHis;
			_totalMoney = p_totalMoney;
			_correspondState = p_correspondState;
			_correspondInvoice = p_correspondInvoice;
			_deliveryType = p_deliveryType;
			_salesman = p_salesman;
			_address = p_address;
			_deliveryMode = p_deliveryMode;
			_tel = p_tel;
			_salesmanId = p_salesmanId;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_logisticState = p_logisticState;
			_deliveryUser = p_deliveryUser;
			_deliveryUserId = p_deliveryUserId;
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
					RaisePropertyChanged(DeliveryOrder.Prop_PId, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_Number, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_CId, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_CName, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_ExpectedTime, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_Child, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_State, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(DeliveryOrder.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("TotalMoneyHis", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? TotalMoneyHis
		{
			get { return _totalMoneyHis; }
			set
			{
				if (value != _totalMoneyHis)
				{
                    object oldValue = _totalMoneyHis;
					_totalMoneyHis = value;
					RaisePropertyChanged(DeliveryOrder.Prop_TotalMoneyHis, oldValue, value);
				}
			}

		}

		[Property("TotalMoney", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? TotalMoney
		{
			get { return _totalMoney; }
			set
			{
				if (value != _totalMoney)
				{
                    object oldValue = _totalMoney;
					_totalMoney = value;
					RaisePropertyChanged(DeliveryOrder.Prop_TotalMoney, oldValue, value);
				}
			}

		}

		[Property("CorrespondState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CorrespondState
		{
			get { return _correspondState; }
			set
			{
				if ((_correspondState == null) || (value == null) || (!value.Equals(_correspondState)))
				{
                    object oldValue = _correspondState;
					_correspondState = value;
					RaisePropertyChanged(DeliveryOrder.Prop_CorrespondState, oldValue, value);
				}
			}

		}

		[Property("CorrespondInvoice", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string CorrespondInvoice
		{
			get { return _correspondInvoice; }
			set
			{
				if ((_correspondInvoice == null) || (value == null) || (!value.Equals(_correspondInvoice)))
				{
                    object oldValue = _correspondInvoice;
					_correspondInvoice = value;
					RaisePropertyChanged(DeliveryOrder.Prop_CorrespondInvoice, oldValue, value);
				}
			}

		}

		[Property("DeliveryType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string DeliveryType
		{
			get { return _deliveryType; }
			set
			{
				if ((_deliveryType == null) || (value == null) || (!value.Equals(_deliveryType)))
				{
                    object oldValue = _deliveryType;
					_deliveryType = value;
					RaisePropertyChanged(DeliveryOrder.Prop_DeliveryType, oldValue, value);
				}
			}

		}

		[Property("Salesman", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Salesman
		{
			get { return _salesman; }
			set
			{
				if ((_salesman == null) || (value == null) || (!value.Equals(_salesman)))
				{
                    object oldValue = _salesman;
					_salesman = value;
					RaisePropertyChanged(DeliveryOrder.Prop_Salesman, oldValue, value);
				}
			}

		}

		[Property("Address", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Address
		{
			get { return _address; }
			set
			{
				if ((_address == null) || (value == null) || (!value.Equals(_address)))
				{
                    object oldValue = _address;
					_address = value;
					RaisePropertyChanged(DeliveryOrder.Prop_Address, oldValue, value);
				}
			}

		}

		[Property("DeliveryMode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string DeliveryMode
		{
			get { return _deliveryMode; }
			set
			{
				if ((_deliveryMode == null) || (value == null) || (!value.Equals(_deliveryMode)))
				{
                    object oldValue = _deliveryMode;
					_deliveryMode = value;
					RaisePropertyChanged(DeliveryOrder.Prop_DeliveryMode, oldValue, value);
				}
			}

		}

		[Property("Tel", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Tel
		{
			get { return _tel; }
			set
			{
				if ((_tel == null) || (value == null) || (!value.Equals(_tel)))
				{
                    object oldValue = _tel;
					_tel = value;
					RaisePropertyChanged(DeliveryOrder.Prop_Tel, oldValue, value);
				}
			}

		}

		[Property("SalesmanId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string SalesmanId
		{
			get { return _salesmanId; }
			set
			{
				if ((_salesmanId == null) || (value == null) || (!value.Equals(_salesmanId)))
				{
                    object oldValue = _salesmanId;
					_salesmanId = value;
					RaisePropertyChanged(DeliveryOrder.Prop_SalesmanId, oldValue, value);
				}
			}

		}

		[Property("WarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string WarehouseId
		{
			get { return _warehouseId; }
			set
			{
				if ((_warehouseId == null) || (value == null) || (!value.Equals(_warehouseId)))
				{
                    object oldValue = _warehouseId;
					_warehouseId = value;
					RaisePropertyChanged(DeliveryOrder.Prop_WarehouseId, oldValue, value);
				}
			}

		}

		[Property("WarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string WarehouseName
		{
			get { return _warehouseName; }
			set
			{
				if ((_warehouseName == null) || (value == null) || (!value.Equals(_warehouseName)))
				{
                    object oldValue = _warehouseName;
					_warehouseName = value;
					RaisePropertyChanged(DeliveryOrder.Prop_WarehouseName, oldValue, value);
				}
			}

		}

		[Property("LogisticState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string LogisticState
		{
			get { return _logisticState; }
			set
			{
				if ((_logisticState == null) || (value == null) || (!value.Equals(_logisticState)))
				{
                    object oldValue = _logisticState;
					_logisticState = value;
					RaisePropertyChanged(DeliveryOrder.Prop_LogisticState, oldValue, value);
				}
			}

		}

		[Property("DeliveryUser", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string DeliveryUser
		{
			get { return _deliveryUser; }
			set
			{
				if ((_deliveryUser == null) || (value == null) || (!value.Equals(_deliveryUser)))
				{
                    object oldValue = _deliveryUser;
					_deliveryUser = value;
					RaisePropertyChanged(DeliveryOrder.Prop_DeliveryUser, oldValue, value);
				}
			}

		}

		[Property("DeliveryUserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string DeliveryUserId
		{
			get { return _deliveryUserId; }
			set
			{
				if ((_deliveryUserId == null) || (value == null) || (!value.Equals(_deliveryUserId)))
				{
                    object oldValue = _deliveryUserId;
					_deliveryUserId = value;
					RaisePropertyChanged(DeliveryOrder.Prop_DeliveryUserId, oldValue, value);
				}
			}

		}

		#endregion
	} // DeliveryOrder
}

