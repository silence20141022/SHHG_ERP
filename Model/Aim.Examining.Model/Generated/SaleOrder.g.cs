// Business class SaleOrder generated from SaleOrders
// Creator: Ray
// Created Date: [2013-07-14]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("SaleOrders")]
	public partial class SaleOrder : ExamModelBase<SaleOrder>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Number = "Number";
		public static string Prop_CId = "CId";
		public static string Prop_CCode = "CCode";
		public static string Prop_CName = "CName";
		public static string Prop_ExpectedTime = "ExpectedTime";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_CalculateManner = "CalculateManner";
		public static string Prop_InvoiceType = "InvoiceType";
		public static string Prop_PayType = "PayType";
		public static string Prop_TotalMoney = "TotalMoney";
		public static string Prop_PreDeposit = "PreDeposit";
		public static string Prop_Child = "Child";
		public static string Prop_Reason = "Reason";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_ApprovalState = "ApprovalState";
		public static string Prop_PId = "PId";
		public static string Prop_DeliveryState = "DeliveryState";
		public static string Prop_TotalMoneyHis = "TotalMoneyHis";
		public static string Prop_PAState = "PAState";
		public static string Prop_PANumber = "PANumber";
		public static string Prop_InvoiceState = "InvoiceState";
		public static string Prop_DeliveryMode = "DeliveryMode";
		public static string Prop_AccountValidity = "AccountValidity";
		public static string Prop_ReceivablesDate = "ReceivablesDate";
		public static string Prop_Salesman = "Salesman";
		public static string Prop_SalesmanId = "SalesmanId";
		public static string Prop_InvoiceNumber = "InvoiceNumber";
		public static string Prop_CorrespondState = "CorrespondState";
		public static string Prop_CorrespondInvoice = "CorrespondInvoice";
		public static string Prop_CorrespondAmount = "CorrespondAmount";
		public static string Prop_EndDate = "EndDate";
		public static string Prop_DiscountAmount = "DiscountAmount";
		public static string Prop_DeState = "DeState";
		public static string Prop_ReceiptAmount = "ReceiptAmount";
		public static string Prop_ReturnAmount = "ReturnAmount";
		public static string Prop_PayState = "PayState";

		#endregion

		#region Private_Variables

		private string _id;
		private string _number;
		private string _cId;
		private string _cCode;
		private string _cName;
		private DateTime? _expectedTime;
		private string _warehouseId;
		private string _warehouseName;
		private string _calculateManner;
		private string _invoiceType;
		private string _payType;
		private System.Decimal? _totalMoney;
		private System.Decimal? _preDeposit;
		private string _child;
		private string _reason;
		private string _state;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _approvalState;
		private string _pId;
		private string _deliveryState;
		private System.Decimal? _totalMoneyHis;
		private string _pAState;
		private string _pANumber;
		private string _invoiceState;
		private string _deliveryMode;
		private int? _accountValidity;
		private string _receivablesDate;
		private string _salesman;
		private string _salesmanId;
		private string _invoiceNumber;
		private string _correspondState;
		private string _correspondInvoice;
		private System.Decimal? _correspondAmount;
		private DateTime? _endDate;
		private System.Decimal? _discountAmount;
		private string _deState;
		private System.Decimal? _receiptAmount;
		private System.Decimal? _returnAmount;
		private string _payState;


		#endregion

		#region Constructors

		public SaleOrder()
		{
		}

		public SaleOrder(
			string p_id,
			string p_number,
			string p_cId,
			string p_cCode,
			string p_cName,
			DateTime? p_expectedTime,
			string p_warehouseId,
			string p_warehouseName,
			string p_calculateManner,
			string p_invoiceType,
			string p_payType,
			System.Decimal? p_totalMoney,
			System.Decimal? p_preDeposit,
			string p_child,
			string p_reason,
			string p_state,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_approvalState,
			string p_pId,
			string p_deliveryState,
			System.Decimal? p_totalMoneyHis,
			string p_pAState,
			string p_pANumber,
			string p_invoiceState,
			string p_deliveryMode,
			int? p_accountValidity,
			string p_receivablesDate,
			string p_salesman,
			string p_salesmanId,
			string p_invoiceNumber,
			string p_correspondState,
			string p_correspondInvoice,
			System.Decimal? p_correspondAmount,
			DateTime? p_endDate,
			System.Decimal? p_discountAmount,
			string p_deState,
			System.Decimal? p_receiptAmount,
			System.Decimal? p_returnAmount,
			string p_payState)
		{
			_id = p_id;
			_number = p_number;
			_cId = p_cId;
			_cCode = p_cCode;
			_cName = p_cName;
			_expectedTime = p_expectedTime;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_calculateManner = p_calculateManner;
			_invoiceType = p_invoiceType;
			_payType = p_payType;
			_totalMoney = p_totalMoney;
			_preDeposit = p_preDeposit;
			_child = p_child;
			_reason = p_reason;
			_state = p_state;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_approvalState = p_approvalState;
			_pId = p_pId;
			_deliveryState = p_deliveryState;
			_totalMoneyHis = p_totalMoneyHis;
			_pAState = p_pAState;
			_pANumber = p_pANumber;
			_invoiceState = p_invoiceState;
			_deliveryMode = p_deliveryMode;
			_accountValidity = p_accountValidity;
			_receivablesDate = p_receivablesDate;
			_salesman = p_salesman;
			_salesmanId = p_salesmanId;
			_invoiceNumber = p_invoiceNumber;
			_correspondState = p_correspondState;
			_correspondInvoice = p_correspondInvoice;
			_correspondAmount = p_correspondAmount;
			_endDate = p_endDate;
			_discountAmount = p_discountAmount;
			_deState = p_deState;
			_receiptAmount = p_receiptAmount;
			_returnAmount = p_returnAmount;
			_payState = p_payState;
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
					RaisePropertyChanged(SaleOrder.Prop_Number, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_CId, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_CCode, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_CName, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_ExpectedTime, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_WarehouseId, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_WarehouseName, oldValue, value);
				}
			}

		}

		[Property("CalculateManner", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string CalculateManner
		{
			get { return _calculateManner; }
			set
			{
				if ((_calculateManner == null) || (value == null) || (!value.Equals(_calculateManner)))
				{
                    object oldValue = _calculateManner;
					_calculateManner = value;
					RaisePropertyChanged(SaleOrder.Prop_CalculateManner, oldValue, value);
				}
			}

		}

		[Property("InvoiceType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string InvoiceType
		{
			get { return _invoiceType; }
			set
			{
				if ((_invoiceType == null) || (value == null) || (!value.Equals(_invoiceType)))
				{
                    object oldValue = _invoiceType;
					_invoiceType = value;
					RaisePropertyChanged(SaleOrder.Prop_InvoiceType, oldValue, value);
				}
			}

		}

		[Property("PayType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PayType
		{
			get { return _payType; }
			set
			{
				if ((_payType == null) || (value == null) || (!value.Equals(_payType)))
				{
                    object oldValue = _payType;
					_payType = value;
					RaisePropertyChanged(SaleOrder.Prop_PayType, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_TotalMoney, oldValue, value);
				}
			}

		}

		[Property("PreDeposit", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PreDeposit
		{
			get { return _preDeposit; }
			set
			{
				if (value != _preDeposit)
				{
                    object oldValue = _preDeposit;
					_preDeposit = value;
					RaisePropertyChanged(SaleOrder.Prop_PreDeposit, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_Child, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_Reason, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_State, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_ApprovalState, oldValue, value);
				}
			}

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
					RaisePropertyChanged(SaleOrder.Prop_PId, oldValue, value);
				}
			}

		}

		[Property("DeliveryState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string DeliveryState
		{
			get { return _deliveryState; }
			set
			{
				if ((_deliveryState == null) || (value == null) || (!value.Equals(_deliveryState)))
				{
                    object oldValue = _deliveryState;
					_deliveryState = value;
					RaisePropertyChanged(SaleOrder.Prop_DeliveryState, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_TotalMoneyHis, oldValue, value);
				}
			}

		}

		[Property("PAState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PAState
		{
			get { return _pAState; }
			set
			{
				if ((_pAState == null) || (value == null) || (!value.Equals(_pAState)))
				{
                    object oldValue = _pAState;
					_pAState = value;
					RaisePropertyChanged(SaleOrder.Prop_PAState, oldValue, value);
				}
			}

		}

		[Property("PANumber", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PANumber
		{
			get { return _pANumber; }
			set
			{
				if ((_pANumber == null) || (value == null) || (!value.Equals(_pANumber)))
				{
                    object oldValue = _pANumber;
					_pANumber = value;
					RaisePropertyChanged(SaleOrder.Prop_PANumber, oldValue, value);
				}
			}

		}

		[Property("InvoiceState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string InvoiceState
		{
			get { return _invoiceState; }
			set
			{
				if ((_invoiceState == null) || (value == null) || (!value.Equals(_invoiceState)))
				{
                    object oldValue = _invoiceState;
					_invoiceState = value;
					RaisePropertyChanged(SaleOrder.Prop_InvoiceState, oldValue, value);
				}
			}

		}

		[Property("DeliveryMode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string DeliveryMode
		{
			get { return _deliveryMode; }
			set
			{
				if ((_deliveryMode == null) || (value == null) || (!value.Equals(_deliveryMode)))
				{
                    object oldValue = _deliveryMode;
					_deliveryMode = value;
					RaisePropertyChanged(SaleOrder.Prop_DeliveryMode, oldValue, value);
				}
			}

		}

		[Property("AccountValidity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? AccountValidity
		{
			get { return _accountValidity; }
			set
			{
				if (value != _accountValidity)
				{
                    object oldValue = _accountValidity;
					_accountValidity = value;
					RaisePropertyChanged(SaleOrder.Prop_AccountValidity, oldValue, value);
				}
			}

		}

		[Property("ReceivablesDate", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ReceivablesDate
		{
			get { return _receivablesDate; }
			set
			{
				if ((_receivablesDate == null) || (value == null) || (!value.Equals(_receivablesDate)))
				{
                    object oldValue = _receivablesDate;
					_receivablesDate = value;
					RaisePropertyChanged(SaleOrder.Prop_ReceivablesDate, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_Salesman, oldValue, value);
				}
			}

		}

		[Property("SalesmanId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string SalesmanId
		{
			get { return _salesmanId; }
			set
			{
				if ((_salesmanId == null) || (value == null) || (!value.Equals(_salesmanId)))
				{
                    object oldValue = _salesmanId;
					_salesmanId = value;
					RaisePropertyChanged(SaleOrder.Prop_SalesmanId, oldValue, value);
				}
			}

		}

		[Property("InvoiceNumber", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string InvoiceNumber
		{
			get { return _invoiceNumber; }
			set
			{
				if ((_invoiceNumber == null) || (value == null) || (!value.Equals(_invoiceNumber)))
				{
                    object oldValue = _invoiceNumber;
					_invoiceNumber = value;
					RaisePropertyChanged(SaleOrder.Prop_InvoiceNumber, oldValue, value);
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
					RaisePropertyChanged(SaleOrder.Prop_CorrespondState, oldValue, value);
				}
			}

		}

		[Property("CorrespondInvoice", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string CorrespondInvoice
		{
			get { return _correspondInvoice; }
			set
			{
				if ((_correspondInvoice == null) || (value == null) || (!value.Equals(_correspondInvoice)))
				{
                    object oldValue = _correspondInvoice;
					_correspondInvoice = value;
					RaisePropertyChanged(SaleOrder.Prop_CorrespondInvoice, oldValue, value);
				}
			}

		}

		[Property("CorrespondAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? CorrespondAmount
		{
			get { return _correspondAmount; }
			set
			{
				if (value != _correspondAmount)
				{
                    object oldValue = _correspondAmount;
					_correspondAmount = value;
					RaisePropertyChanged(SaleOrder.Prop_CorrespondAmount, oldValue, value);
				}
			}

		}

		[Property("EndDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? EndDate
		{
			get { return _endDate; }
			set
			{
				if (value != _endDate)
				{
                    object oldValue = _endDate;
					_endDate = value;
					RaisePropertyChanged(SaleOrder.Prop_EndDate, oldValue, value);
				}
			}

		}

		[Property("DiscountAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? DiscountAmount
		{
			get { return _discountAmount; }
			set
			{
				if (value != _discountAmount)
				{
                    object oldValue = _discountAmount;
					_discountAmount = value;
					RaisePropertyChanged(SaleOrder.Prop_DiscountAmount, oldValue, value);
				}
			}

		}

		[Property("DeState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string DeState
		{
			get { return _deState; }
			set
			{
				if ((_deState == null) || (value == null) || (!value.Equals(_deState)))
				{
                    object oldValue = _deState;
					_deState = value;
					RaisePropertyChanged(SaleOrder.Prop_DeState, oldValue, value);
				}
			}

		}

		[Property("ReceiptAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ReceiptAmount
		{
			get { return _receiptAmount; }
			set
			{
				if (value != _receiptAmount)
				{
                    object oldValue = _receiptAmount;
					_receiptAmount = value;
					RaisePropertyChanged(SaleOrder.Prop_ReceiptAmount, oldValue, value);
				}
			}

		}

		[Property("ReturnAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ReturnAmount
		{
			get { return _returnAmount; }
			set
			{
				if (value != _returnAmount)
				{
                    object oldValue = _returnAmount;
					_returnAmount = value;
					RaisePropertyChanged(SaleOrder.Prop_ReturnAmount, oldValue, value);
				}
			}

		}

		[Property("PayState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayState
		{
			get { return _payState; }
			set
			{
				if ((_payState == null) || (value == null) || (!value.Equals(_payState)))
				{
                    object oldValue = _payState;
					_payState = value;
					RaisePropertyChanged(SaleOrder.Prop_PayState, oldValue, value);
				}
			}

		}

		#endregion
	} // SaleOrder
}

