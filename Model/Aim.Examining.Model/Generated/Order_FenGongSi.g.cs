// Business class Order_FenGongSi generated from Order_FenGongSi
// Creator: Ray
// Created Date: [2015-01-31]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Order_FenGongSi")]
	public partial class Order_FenGongSi : ExamModelBase<Order_FenGongSi>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_CustomerId = "CustomerId";
		public static string Prop_CustomerName = "CustomerName";
		public static string Prop_Number = "Number";
		public static string Prop_FenGongSiId = "FenGongSiId";
		public static string Prop_FenGongSiName = "FenGongSiName";
		public static string Prop_TotalMoney = "TotalMoney";
		public static string Prop_DiscountAmount = "DiscountAmount";
		public static string Prop_ShouKuanAmount = "ShouKuanAmount";
		public static string Prop_ShouKuanDate = "ShouKuanDate";
		public static string Prop_PayType = "PayType";
		public static string Prop_InvoiceType = "InvoiceType";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _customerId;
		private string _customerName;
		private string _number;
		private string _fenGongSiId;
		private string _fenGongSiName;
		private System.Decimal? _totalMoney;
		private System.Decimal? _discountAmount;
		private System.Decimal? _shouKuanAmount;
		private DateTime? _shouKuanDate;
		private string _payType;
		private string _invoiceType;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public Order_FenGongSi()
		{
		}

		public Order_FenGongSi(
			string p_id,
			string p_customerId,
			string p_customerName,
			string p_number,
			string p_fenGongSiId,
			string p_fenGongSiName,
			System.Decimal? p_totalMoney,
			System.Decimal? p_discountAmount,
			System.Decimal? p_shouKuanAmount,
			DateTime? p_shouKuanDate,
			string p_payType,
			string p_invoiceType,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_customerId = p_customerId;
			_customerName = p_customerName;
			_number = p_number;
			_fenGongSiId = p_fenGongSiId;
			_fenGongSiName = p_fenGongSiName;
			_totalMoney = p_totalMoney;
			_discountAmount = p_discountAmount;
			_shouKuanAmount = p_shouKuanAmount;
			_shouKuanDate = p_shouKuanDate;
			_payType = p_payType;
			_invoiceType = p_invoiceType;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("CustomerId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CustomerId
		{
			get { return _customerId; }
			set
			{
				if ((_customerId == null) || (value == null) || (!value.Equals(_customerId)))
				{
                    object oldValue = _customerId;
					_customerId = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_CustomerId, oldValue, value);
				}
			}

		}

		[Property("CustomerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string CustomerName
		{
			get { return _customerName; }
			set
			{
				if ((_customerName == null) || (value == null) || (!value.Equals(_customerName)))
				{
                    object oldValue = _customerName;
					_customerName = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_CustomerName, oldValue, value);
				}
			}

		}

		[Property("Number", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Number
		{
			get { return _number; }
			set
			{
				if ((_number == null) || (value == null) || (!value.Equals(_number)))
				{
                    object oldValue = _number;
					_number = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_Number, oldValue, value);
				}
			}

		}

		[Property("FenGongSiId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string FenGongSiId
		{
			get { return _fenGongSiId; }
			set
			{
				if ((_fenGongSiId == null) || (value == null) || (!value.Equals(_fenGongSiId)))
				{
                    object oldValue = _fenGongSiId;
					_fenGongSiId = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_FenGongSiId, oldValue, value);
				}
			}

		}

		[Property("FenGongSiName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string FenGongSiName
		{
			get { return _fenGongSiName; }
			set
			{
				if ((_fenGongSiName == null) || (value == null) || (!value.Equals(_fenGongSiName)))
				{
                    object oldValue = _fenGongSiName;
					_fenGongSiName = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_FenGongSiName, oldValue, value);
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
					RaisePropertyChanged(Order_FenGongSi.Prop_TotalMoney, oldValue, value);
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
					RaisePropertyChanged(Order_FenGongSi.Prop_DiscountAmount, oldValue, value);
				}
			}

		}

		[Property("ShouKuanAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ShouKuanAmount
		{
			get { return _shouKuanAmount; }
			set
			{
				if (value != _shouKuanAmount)
				{
                    object oldValue = _shouKuanAmount;
					_shouKuanAmount = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_ShouKuanAmount, oldValue, value);
				}
			}

		}

		[Property("ShouKuanDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? ShouKuanDate
		{
			get { return _shouKuanDate; }
			set
			{
				if (value != _shouKuanDate)
				{
                    object oldValue = _shouKuanDate;
					_shouKuanDate = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_ShouKuanDate, oldValue, value);
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
					RaisePropertyChanged(Order_FenGongSi.Prop_PayType, oldValue, value);
				}
			}

		}

		[Property("InvoiceType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InvoiceType
		{
			get { return _invoiceType; }
			set
			{
				if ((_invoiceType == null) || (value == null) || (!value.Equals(_invoiceType)))
				{
                    object oldValue = _invoiceType;
					_invoiceType = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_InvoiceType, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(Order_FenGongSi.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(Order_FenGongSi.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Order_FenGongSi.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Order_FenGongSi.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // Order_FenGongSi
}

