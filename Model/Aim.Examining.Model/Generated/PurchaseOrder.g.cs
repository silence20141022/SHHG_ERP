// Business class PurchaseOrder generated from PurchaseOrder
// Creator: Ray
// Created Date: [2015-01-24]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PurchaseOrder")]
	public partial class PurchaseOrder : ExamModelBase<PurchaseOrder>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PurchaseOrderNo = "PurchaseOrderNo";
		public static string Prop_SupplierBillNo = "SupplierBillNo";
		public static string Prop_PriceType = "PriceType";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_PurchaseOrderAmount = "PurchaseOrderAmount";
		public static string Prop_OrderState = "OrderState";
		public static string Prop_PayState = "PayState";
		public static string Prop_InWarehouseState = "InWarehouseState";
		public static string Prop_InvoiceState = "InvoiceState";
		public static string Prop_ProductType = "ProductType";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_TransportationMode = "TransportationMode";
		public static string Prop_OrderDate = "OrderDate";
		public static string Prop_PurchaseType = "PurchaseType";
		public static string Prop_GoodsResource = "GoodsResource";
		public static string Prop_MoneyType = "MoneyType";
		public static string Prop_Symbo = "Symbo";

		#endregion

		#region Private_Variables

		private string _id;
		private string _purchaseOrderNo;
		private string _supplierBillNo;
		private string _priceType;
		private string _supplierId;
		private string _supplierName;
		private string _warehouseId;
		private System.Decimal? _purchaseOrderAmount;
		private string _orderState;
		private string _payState;
		private string _inWarehouseState;
		private string _invoiceState;
		private string _productType;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _transportationMode;
		private DateTime? _orderDate;
		private string _purchaseType;
		private string _goodsResource;
		private string _moneyType;
		private string _symbo;


		#endregion

		#region Constructors

		public PurchaseOrder()
		{
		}

		public PurchaseOrder(
			string p_id,
			string p_purchaseOrderNo,
			string p_supplierBillNo,
			string p_priceType,
			string p_supplierId,
			string p_supplierName,
			string p_warehouseId,
			System.Decimal? p_purchaseOrderAmount,
			string p_orderState,
			string p_payState,
			string p_inWarehouseState,
			string p_invoiceState,
			string p_productType,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_transportationMode,
			DateTime? p_orderDate,
			string p_purchaseType,
			string p_goodsResource,
			string p_moneyType,
			string p_symbo)
		{
			_id = p_id;
			_purchaseOrderNo = p_purchaseOrderNo;
			_supplierBillNo = p_supplierBillNo;
			_priceType = p_priceType;
			_supplierId = p_supplierId;
			_supplierName = p_supplierName;
			_warehouseId = p_warehouseId;
			_purchaseOrderAmount = p_purchaseOrderAmount;
			_orderState = p_orderState;
			_payState = p_payState;
			_inWarehouseState = p_inWarehouseState;
			_invoiceState = p_invoiceState;
			_productType = p_productType;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_transportationMode = p_transportationMode;
			_orderDate = p_orderDate;
			_purchaseType = p_purchaseType;
			_goodsResource = p_goodsResource;
			_moneyType = p_moneyType;
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

		[Property("PurchaseOrderNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PurchaseOrderNo
		{
			get { return _purchaseOrderNo; }
			set
			{
				if ((_purchaseOrderNo == null) || (value == null) || (!value.Equals(_purchaseOrderNo)))
				{
                    object oldValue = _purchaseOrderNo;
					_purchaseOrderNo = value;
					RaisePropertyChanged(PurchaseOrder.Prop_PurchaseOrderNo, oldValue, value);
				}
			}

		}

		[Property("SupplierBillNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SupplierBillNo
		{
			get { return _supplierBillNo; }
			set
			{
				if ((_supplierBillNo == null) || (value == null) || (!value.Equals(_supplierBillNo)))
				{
                    object oldValue = _supplierBillNo;
					_supplierBillNo = value;
					RaisePropertyChanged(PurchaseOrder.Prop_SupplierBillNo, oldValue, value);
				}
			}

		}

		[Property("PriceType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PriceType
		{
			get { return _priceType; }
			set
			{
				if ((_priceType == null) || (value == null) || (!value.Equals(_priceType)))
				{
                    object oldValue = _priceType;
					_priceType = value;
					RaisePropertyChanged(PurchaseOrder.Prop_PriceType, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_SupplierId, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_SupplierName, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_WarehouseId, oldValue, value);
				}
			}

		}

		[Property("PurchaseOrderAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PurchaseOrderAmount
		{
			get { return _purchaseOrderAmount; }
			set
			{
				if (value != _purchaseOrderAmount)
				{
                    object oldValue = _purchaseOrderAmount;
					_purchaseOrderAmount = value;
					RaisePropertyChanged(PurchaseOrder.Prop_PurchaseOrderAmount, oldValue, value);
				}
			}

		}

		[Property("OrderState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OrderState
		{
			get { return _orderState; }
			set
			{
				if ((_orderState == null) || (value == null) || (!value.Equals(_orderState)))
				{
                    object oldValue = _orderState;
					_orderState = value;
					RaisePropertyChanged(PurchaseOrder.Prop_OrderState, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_PayState, oldValue, value);
				}
			}

		}

		[Property("InWarehouseState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InWarehouseState
		{
			get { return _inWarehouseState; }
			set
			{
				if ((_inWarehouseState == null) || (value == null) || (!value.Equals(_inWarehouseState)))
				{
                    object oldValue = _inWarehouseState;
					_inWarehouseState = value;
					RaisePropertyChanged(PurchaseOrder.Prop_InWarehouseState, oldValue, value);
				}
			}

		}

		[Property("InvoiceState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InvoiceState
		{
			get { return _invoiceState; }
			set
			{
				if ((_invoiceState == null) || (value == null) || (!value.Equals(_invoiceState)))
				{
                    object oldValue = _invoiceState;
					_invoiceState = value;
					RaisePropertyChanged(PurchaseOrder.Prop_InvoiceState, oldValue, value);
				}
			}

		}

		[Property("ProductType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ProductType
		{
			get { return _productType; }
			set
			{
				if ((_productType == null) || (value == null) || (!value.Equals(_productType)))
				{
                    object oldValue = _productType;
					_productType = value;
					RaisePropertyChanged(PurchaseOrder.Prop_ProductType, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(PurchaseOrder.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("TransportationMode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string TransportationMode
		{
			get { return _transportationMode; }
			set
			{
				if ((_transportationMode == null) || (value == null) || (!value.Equals(_transportationMode)))
				{
                    object oldValue = _transportationMode;
					_transportationMode = value;
					RaisePropertyChanged(PurchaseOrder.Prop_TransportationMode, oldValue, value);
				}
			}

		}

		[Property("OrderDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? OrderDate
		{
			get { return _orderDate; }
			set
			{
				if (value != _orderDate)
				{
                    object oldValue = _orderDate;
					_orderDate = value;
					RaisePropertyChanged(PurchaseOrder.Prop_OrderDate, oldValue, value);
				}
			}

		}

		[Property("PurchaseType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PurchaseType
		{
			get { return _purchaseType; }
			set
			{
				if ((_purchaseType == null) || (value == null) || (!value.Equals(_purchaseType)))
				{
                    object oldValue = _purchaseType;
					_purchaseType = value;
					RaisePropertyChanged(PurchaseOrder.Prop_PurchaseType, oldValue, value);
				}
			}

		}

		[Property("GoodsResource", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string GoodsResource
		{
			get { return _goodsResource; }
			set
			{
				if ((_goodsResource == null) || (value == null) || (!value.Equals(_goodsResource)))
				{
                    object oldValue = _goodsResource;
					_goodsResource = value;
					RaisePropertyChanged(PurchaseOrder.Prop_GoodsResource, oldValue, value);
				}
			}

		}

		[Property("MoneyType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string MoneyType
		{
			get { return _moneyType; }
			set
			{
				if ((_moneyType == null) || (value == null) || (!value.Equals(_moneyType)))
				{
                    object oldValue = _moneyType;
					_moneyType = value;
					RaisePropertyChanged(PurchaseOrder.Prop_MoneyType, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrder.Prop_Symbo, oldValue, value);
				}
			}

		}

		#endregion
	} // PurchaseOrder
}

