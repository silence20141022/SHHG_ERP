// Business class PurchaseInvoiceDetail generated from PurchaseInvoiceDetail
// Creator: Ray
// Created Date: [2015-02-02]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PurchaseInvoiceDetail")]
	public partial class PurchaseInvoiceDetail : ExamModelBase<PurchaseInvoiceDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PurchaseInvoiceId = "PurchaseInvoiceId";
		public static string Prop_PurchaseOrderDetailId = "PurchaseOrderDetailId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_BuyPrice = "BuyPrice";
		public static string Prop_InvoiceQuantity = "InvoiceQuantity";
		public static string Prop_InvoiceAmount = "InvoiceAmount";
		public static string Prop_Remark = "Remark";
		public static string Prop_InWarehouseDetailId = "InWarehouseDetailId";

		#endregion

		#region Private_Variables

		private string _id;
		private string _purchaseInvoiceId;
		private string _purchaseOrderDetailId;
		private string _productId;
		private string _productName;
		private string _productCode;
		private System.Decimal? _buyPrice;
		private int _invoiceQuantity;
		private System.Decimal? _invoiceAmount;
		private string _remark;
		private string _inWarehouseDetailId;


		#endregion

		#region Constructors

		public PurchaseInvoiceDetail()
		{
		}

		public PurchaseInvoiceDetail(
			string p_id,
			string p_purchaseInvoiceId,
			string p_purchaseOrderDetailId,
			string p_productId,
			string p_productName,
			string p_productCode,
			System.Decimal? p_buyPrice,
			int p_invoiceQuantity,
			System.Decimal? p_invoiceAmount,
			string p_remark,
			string p_inWarehouseDetailId)
		{
			_id = p_id;
			_purchaseInvoiceId = p_purchaseInvoiceId;
			_purchaseOrderDetailId = p_purchaseOrderDetailId;
			_productId = p_productId;
			_productName = p_productName;
			_productCode = p_productCode;
			_buyPrice = p_buyPrice;
			_invoiceQuantity = p_invoiceQuantity;
			_invoiceAmount = p_invoiceAmount;
			_remark = p_remark;
			_inWarehouseDetailId = p_inWarehouseDetailId;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("PurchaseInvoiceId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PurchaseInvoiceId
		{
			get { return _purchaseInvoiceId; }
			set
			{
				if ((_purchaseInvoiceId == null) || (value == null) || (!value.Equals(_purchaseInvoiceId)))
				{
                    object oldValue = _purchaseInvoiceId;
					_purchaseInvoiceId = value;
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_PurchaseInvoiceId, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_PurchaseOrderDetailId, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("ProductName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ProductName
		{
			get { return _productName; }
			set
			{
				if ((_productName == null) || (value == null) || (!value.Equals(_productName)))
				{
                    object oldValue = _productName;
					_productName = value;
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_BuyPrice, oldValue, value);
				}
			}

		}

		[Property("InvoiceQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public int InvoiceQuantity
		{
			get { return _invoiceQuantity; }
			set
			{
				if (value != _invoiceQuantity)
				{
                    object oldValue = _invoiceQuantity;
					_invoiceQuantity = value;
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_InvoiceQuantity, oldValue, value);
				}
			}

		}

		[Property("InvoiceAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? InvoiceAmount
		{
			get { return _invoiceAmount; }
			set
			{
				if (value != _invoiceAmount)
				{
                    object oldValue = _invoiceAmount;
					_invoiceAmount = value;
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_InvoiceAmount, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoiceDetail.Prop_InWarehouseDetailId, oldValue, value);
				}
			}

		}

		#endregion
	} // PurchaseInvoiceDetail
}

