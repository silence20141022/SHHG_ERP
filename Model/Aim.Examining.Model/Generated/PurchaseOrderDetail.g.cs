// Business class PurchaseOrderDetail generated from PurchaseOrderDetail
// Creator: Ray
// Created Date: [2015-01-27]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PurchaseOrderDetail")]
	public partial class PurchaseOrderDetail : ExamModelBase<PurchaseOrderDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PurchaseOrderId = "PurchaseOrderId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_Name = "Name";
		public static string Prop_Code = "Code";
		public static string Prop_PCN = "PCN";
		public static string Prop_BuyPrice = "BuyPrice";
		public static string Prop_Quantity = "Quantity";
		public static string Prop_Amount = "Amount";
		public static string Prop_PayState = "PayState";
		public static string Prop_InvoiceState = "InvoiceState";
		public static string Prop_InWarehouseState = "InWarehouseState";
		public static string Prop_DelieveGoodsDate = "DelieveGoodsDate";
		public static string Prop_ExpectedArrivalDate = "ExpectedArrivalDate";
		public static string Prop_InvoiceNo = "InvoiceNo";
		public static string Prop_RuKuDanQuan = "RuKuDanQuan";
		public static string Prop_FuKuanDanQuan = "FuKuanDanQuan";

		#endregion

		#region Private_Variables

		private string _id;
		private string _purchaseOrderId;
		private string _productId;
		private string _name;
		private string _code;
		private string _pCN;
		private System.Decimal? _buyPrice;
		private int? _quantity;
		private System.Decimal? _amount;
		private string _payState;
		private string _invoiceState;
		private string _inWarehouseState;
		private DateTime? _delieveGoodsDate;
		private DateTime? _expectedArrivalDate;
		private string _invoiceNo;
		private int? _ruKuDanQuan;
		private int? _fuKuanDanQuan;


		#endregion

		#region Constructors

		public PurchaseOrderDetail()
		{
		}

		public PurchaseOrderDetail(
			string p_id,
			string p_purchaseOrderId,
			string p_productId,
			string p_name,
			string p_code,
			string p_pCN,
			System.Decimal? p_buyPrice,
			int? p_quantity,
			System.Decimal? p_amount,
			string p_payState,
			string p_invoiceState,
			string p_inWarehouseState,
			DateTime? p_delieveGoodsDate,
			DateTime? p_expectedArrivalDate,
			string p_invoiceNo,
			int? p_ruKuDanQuan,
			int? p_fuKuanDanQuan)
		{
			_id = p_id;
			_purchaseOrderId = p_purchaseOrderId;
			_productId = p_productId;
			_name = p_name;
			_code = p_code;
			_pCN = p_pCN;
			_buyPrice = p_buyPrice;
			_quantity = p_quantity;
			_amount = p_amount;
			_payState = p_payState;
			_invoiceState = p_invoiceState;
			_inWarehouseState = p_inWarehouseState;
			_delieveGoodsDate = p_delieveGoodsDate;
			_expectedArrivalDate = p_expectedArrivalDate;
			_invoiceNo = p_invoiceNo;
			_ruKuDanQuan = p_ruKuDanQuan;
			_fuKuanDanQuan = p_fuKuanDanQuan;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("PurchaseOrderId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PurchaseOrderId
		{
			get { return _purchaseOrderId; }
			set
			{
				if ((_purchaseOrderId == null) || (value == null) || (!value.Equals(_purchaseOrderId)))
				{
                    object oldValue = _purchaseOrderId;
					_purchaseOrderId = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_PurchaseOrderId, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrderDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("PCN", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PCN
		{
			get { return _pCN; }
			set
			{
				if ((_pCN == null) || (value == null) || (!value.Equals(_pCN)))
				{
                    object oldValue = _pCN;
					_pCN = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_PCN, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrderDetail.Prop_BuyPrice, oldValue, value);
				}
			}

		}

		[Property("Quantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Quantity
		{
			get { return _quantity; }
			set
			{
				if (value != _quantity)
				{
                    object oldValue = _quantity;
					_quantity = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_Quantity, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrderDetail.Prop_Amount, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrderDetail.Prop_PayState, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrderDetail.Prop_InvoiceState, oldValue, value);
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
					RaisePropertyChanged(PurchaseOrderDetail.Prop_InWarehouseState, oldValue, value);
				}
			}

		}

		[Property("DelieveGoodsDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? DelieveGoodsDate
		{
			get { return _delieveGoodsDate; }
			set
			{
				if (value != _delieveGoodsDate)
				{
                    object oldValue = _delieveGoodsDate;
					_delieveGoodsDate = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_DelieveGoodsDate, oldValue, value);
				}
			}

		}

		[Property("ExpectedArrivalDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? ExpectedArrivalDate
		{
			get { return _expectedArrivalDate; }
			set
			{
				if (value != _expectedArrivalDate)
				{
                    object oldValue = _expectedArrivalDate;
					_expectedArrivalDate = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_ExpectedArrivalDate, oldValue, value);
				}
			}

		}

		[Property("InvoiceNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InvoiceNo
		{
			get { return _invoiceNo; }
			set
			{
				if ((_invoiceNo == null) || (value == null) || (!value.Equals(_invoiceNo)))
				{
                    object oldValue = _invoiceNo;
					_invoiceNo = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_InvoiceNo, oldValue, value);
				}
			}

		}

		[Property("RuKuDanQuan", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? RuKuDanQuan
		{
			get { return _ruKuDanQuan; }
			set
			{
				if (value != _ruKuDanQuan)
				{
                    object oldValue = _ruKuDanQuan;
					_ruKuDanQuan = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_RuKuDanQuan, oldValue, value);
				}
			}

		}

		[Property("FuKuanDanQuan", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? FuKuanDanQuan
		{
			get { return _fuKuanDanQuan; }
			set
			{
				if (value != _fuKuanDanQuan)
				{
                    object oldValue = _fuKuanDanQuan;
					_fuKuanDanQuan = value;
					RaisePropertyChanged(PurchaseOrderDetail.Prop_FuKuanDanQuan, oldValue, value);
				}
			}

		}

		#endregion
	} // PurchaseOrderDetail
}

