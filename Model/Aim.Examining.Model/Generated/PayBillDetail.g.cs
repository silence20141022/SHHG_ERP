// Business class PayBillDetail generated from PayBillDetail
// Creator: Ray
// Created Date: [2015-01-28]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PayBillDetail")]
	public partial class PayBillDetail : ExamModelBase<PayBillDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PayBillId = "PayBillId";
		public static string Prop_InWarehouseDetailId = "InWarehouseDetailId";
		public static string Prop_PurchaseOrderDetailId = "PurchaseOrderDetailId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_BuyPrice = "BuyPrice";
		public static string Prop_PayQuantity = "PayQuantity";
		public static string Prop_Amount = "Amount";

		#endregion

		#region Private_Variables

		private string _id;
		private string _payBillId;
		private string _inWarehouseDetailId;
		private string _purchaseOrderDetailId;
		private string _productId;
		private string _productCode;
		private string _productName;
		private System.Decimal? _buyPrice;
		private int? _payQuantity;
		private System.Decimal? _amount;


		#endregion

		#region Constructors

		public PayBillDetail()
		{
		}

		public PayBillDetail(
			string p_id,
			string p_payBillId,
			string p_inWarehouseDetailId,
			string p_purchaseOrderDetailId,
			string p_productId,
			string p_productCode,
			string p_productName,
			System.Decimal? p_buyPrice,
			int? p_payQuantity,
			System.Decimal? p_amount)
		{
			_id = p_id;
			_payBillId = p_payBillId;
			_inWarehouseDetailId = p_inWarehouseDetailId;
			_purchaseOrderDetailId = p_purchaseOrderDetailId;
			_productId = p_productId;
			_productCode = p_productCode;
			_productName = p_productName;
			_buyPrice = p_buyPrice;
			_payQuantity = p_payQuantity;
			_amount = p_amount;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("PayBillId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PayBillId
		{
			get { return _payBillId; }
			set
			{
				if ((_payBillId == null) || (value == null) || (!value.Equals(_payBillId)))
				{
                    object oldValue = _payBillId;
					_payBillId = value;
					RaisePropertyChanged(PayBillDetail.Prop_PayBillId, oldValue, value);
				}
			}

		}

		[Property("InWarehouseDetailId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InWarehouseDetailId
		{
			get { return _inWarehouseDetailId; }
			set
			{
				if ((_inWarehouseDetailId == null) || (value == null) || (!value.Equals(_inWarehouseDetailId)))
				{
                    object oldValue = _inWarehouseDetailId;
					_inWarehouseDetailId = value;
					RaisePropertyChanged(PayBillDetail.Prop_InWarehouseDetailId, oldValue, value);
				}
			}

		}

		[Property("PurchaseOrderDetailId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PurchaseOrderDetailId
		{
			get { return _purchaseOrderDetailId; }
			set
			{
				if ((_purchaseOrderDetailId == null) || (value == null) || (!value.Equals(_purchaseOrderDetailId)))
				{
                    object oldValue = _purchaseOrderDetailId;
					_purchaseOrderDetailId = value;
					RaisePropertyChanged(PayBillDetail.Prop_PurchaseOrderDetailId, oldValue, value);
				}
			}

		}

		[Property("ProductId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ProductId
		{
			get { return _productId; }
			set
			{
				if ((_productId == null) || (value == null) || (!value.Equals(_productId)))
				{
                    object oldValue = _productId;
					_productId = value;
					RaisePropertyChanged(PayBillDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("ProductCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string ProductCode
		{
			get { return _productCode; }
			set
			{
				if ((_productCode == null) || (value == null) || (!value.Equals(_productCode)))
				{
                    object oldValue = _productCode;
					_productCode = value;
					RaisePropertyChanged(PayBillDetail.Prop_ProductCode, oldValue, value);
				}
			}

		}

		[Property("ProductName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string ProductName
		{
			get { return _productName; }
			set
			{
				if ((_productName == null) || (value == null) || (!value.Equals(_productName)))
				{
                    object oldValue = _productName;
					_productName = value;
					RaisePropertyChanged(PayBillDetail.Prop_ProductName, oldValue, value);
				}
			}

		}

		[Property("BuyPrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? BuyPrice
		{
			get { return _buyPrice; }
			set
			{
				if (value != _buyPrice)
				{
                    object oldValue = _buyPrice;
					_buyPrice = value;
					RaisePropertyChanged(PayBillDetail.Prop_BuyPrice, oldValue, value);
				}
			}

		}

		[Property("PayQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? PayQuantity
		{
			get { return _payQuantity; }
			set
			{
				if (value != _payQuantity)
				{
                    object oldValue = _payQuantity;
					_payQuantity = value;
					RaisePropertyChanged(PayBillDetail.Prop_PayQuantity, oldValue, value);
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
					RaisePropertyChanged(PayBillDetail.Prop_Amount, oldValue, value);
				}
			}

		}

		#endregion
	} // PayBillDetail
}

