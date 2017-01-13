// Business class OrderInvoice generated from OrderInvoice
// Creator: Ray
// Created Date: [2013-07-17]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("OrderInvoice")]
	public partial class OrderInvoice : ExamModelBase<OrderInvoice>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Number = "Number";
		public static string Prop_CId = "CId";
		public static string Prop_CName = "CName";
		public static string Prop_Amount = "Amount";
		public static string Prop_Child = "Child";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Invalid = "Invalid";
		public static string Prop_OId = "OId";
		public static string Prop_DiscountAmount = "DiscountAmount";
		public static string Prop_ReturnAmount = "ReturnAmount";
		public static string Prop_PayType = "PayType";
		public static string Prop_PayState = "PayState";
		public static string Prop_PayAmount = "PayAmount";
		public static string Prop_InvoiceDate = "InvoiceDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _number;
		private string _cId;
		private string _cName;
		private System.Decimal? _amount;
		private string _child;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _invalid;
		private string _oId;
		private System.Decimal? _discountAmount;
		private System.Decimal? _returnAmount;
		private string _payType;
		private string _payState;
		private System.Decimal? _payAmount;
		private DateTime? _invoiceDate;


		#endregion

		#region Constructors

		public OrderInvoice()
		{
		}

		public OrderInvoice(
			string p_id,
			string p_number,
			string p_cId,
			string p_cName,
			System.Decimal? p_amount,
			string p_child,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_invalid,
			string p_oId,
			System.Decimal? p_discountAmount,
			System.Decimal? p_returnAmount,
			string p_payType,
			string p_payState,
			System.Decimal? p_payAmount,
			DateTime? p_invoiceDate)
		{
			_id = p_id;
			_number = p_number;
			_cId = p_cId;
			_cName = p_cName;
			_amount = p_amount;
			_child = p_child;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_invalid = p_invalid;
			_oId = p_oId;
			_discountAmount = p_discountAmount;
			_returnAmount = p_returnAmount;
			_payType = p_payType;
			_payState = p_payState;
			_payAmount = p_payAmount;
			_invoiceDate = p_invoiceDate;
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
					RaisePropertyChanged(OrderInvoice.Prop_Number, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_CId, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_CName, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_Amount, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_Child, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("Invalid", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 10)]
		public string Invalid
		{
			get { return _invalid; }
			set
			{
				if ((_invalid == null) || (value == null) || (!value.Equals(_invalid)))
				{
                    object oldValue = _invalid;
					_invalid = value;
					RaisePropertyChanged(OrderInvoice.Prop_Invalid, oldValue, value);
				}
			}

		}

		[Property("OId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1000)]
		public string OId
		{
			get { return _oId; }
			set
			{
				if ((_oId == null) || (value == null) || (!value.Equals(_oId)))
				{
                    object oldValue = _oId;
					_oId = value;
					RaisePropertyChanged(OrderInvoice.Prop_OId, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_DiscountAmount, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_ReturnAmount, oldValue, value);
				}
			}

		}

		[Property("PayType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayType
		{
			get { return _payType; }
			set
			{
				if ((_payType == null) || (value == null) || (!value.Equals(_payType)))
				{
                    object oldValue = _payType;
					_payType = value;
					RaisePropertyChanged(OrderInvoice.Prop_PayType, oldValue, value);
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
					RaisePropertyChanged(OrderInvoice.Prop_PayState, oldValue, value);
				}
			}

		}

		[Property("PayAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PayAmount
		{
			get { return _payAmount; }
			set
			{
				if (value != _payAmount)
				{
                    object oldValue = _payAmount;
					_payAmount = value;
					RaisePropertyChanged(OrderInvoice.Prop_PayAmount, oldValue, value);
				}
			}

		}

		[Property("InvoiceDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? InvoiceDate
		{
			get { return _invoiceDate; }
			set
			{
				if (value != _invoiceDate)
				{
                    object oldValue = _invoiceDate;
					_invoiceDate = value;
					RaisePropertyChanged(OrderInvoice.Prop_InvoiceDate, oldValue, value);
				}
			}

		}

		#endregion
	} // OrderInvoice
}

