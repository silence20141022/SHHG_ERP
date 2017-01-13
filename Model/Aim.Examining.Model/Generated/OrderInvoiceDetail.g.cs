// Business class OrderInvoiceDetail generated from OrderInvoiceDetail
// Creator: Ray
// Created Date: [2013-04-16]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("OrderInvoiceDetail")]
	public partial class OrderInvoiceDetail : ExamModelBase<OrderInvoiceDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_OrderInvoiceId = "OrderInvoiceId";
		public static string Prop_OrderDetailId = "OrderDetailId";
		public static string Prop_SaleOrderId = "SaleOrderId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_Unit = "Unit";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_Count = "Count";
		public static string Prop_SalePrice = "SalePrice";
		public static string Prop_Amount = "Amount";
		public static string Prop_InvoiceCount = "InvoiceCount";
		public static string Prop_HaveInvoiceCount = "HaveInvoiceCount";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _orderInvoiceId;
		private string _orderDetailId;
		private string _saleOrderId;
		private string _productId;
		private string _unit;
		private string _productName;
		private string _productCode;
		private int? _count;
		private System.Decimal? _salePrice;
		private System.Decimal? _amount;
		private int? _invoiceCount;
		private int? _haveInvoiceCount;
		private string _remark;


		#endregion

		#region Constructors

		public OrderInvoiceDetail()
		{
		}

		public OrderInvoiceDetail(
			string p_id,
			string p_orderInvoiceId,
			string p_orderDetailId,
			string p_saleOrderId,
			string p_productId,
			string p_unit,
			string p_productName,
			string p_productCode,
			int? p_count,
			System.Decimal? p_salePrice,
			System.Decimal? p_amount,
			int? p_invoiceCount,
			int? p_haveInvoiceCount,
			string p_remark)
		{
			_id = p_id;
			_orderInvoiceId = p_orderInvoiceId;
			_orderDetailId = p_orderDetailId;
			_saleOrderId = p_saleOrderId;
			_productId = p_productId;
			_unit = p_unit;
			_productName = p_productName;
			_productCode = p_productCode;
			_count = p_count;
			_salePrice = p_salePrice;
			_amount = p_amount;
			_invoiceCount = p_invoiceCount;
			_haveInvoiceCount = p_haveInvoiceCount;
			_remark = p_remark;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("OrderInvoiceId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OrderInvoiceId
		{
			get { return _orderInvoiceId; }
			set
			{
				if ((_orderInvoiceId == null) || (value == null) || (!value.Equals(_orderInvoiceId)))
				{
                    object oldValue = _orderInvoiceId;
					_orderInvoiceId = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_OrderInvoiceId, oldValue, value);
				}
			}

		}

		[Property("OrderDetailId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OrderDetailId
		{
			get { return _orderDetailId; }
			set
			{
				if ((_orderDetailId == null) || (value == null) || (!value.Equals(_orderDetailId)))
				{
                    object oldValue = _orderDetailId;
					_orderDetailId = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_OrderDetailId, oldValue, value);
				}
			}

		}

		[Property("SaleOrderId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string SaleOrderId
		{
			get { return _saleOrderId; }
			set
			{
				if ((_saleOrderId == null) || (value == null) || (!value.Equals(_saleOrderId)))
				{
                    object oldValue = _saleOrderId;
					_saleOrderId = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_SaleOrderId, oldValue, value);
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
					RaisePropertyChanged(OrderInvoiceDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("Unit", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string Unit
		{
			get { return _unit; }
			set
			{
				if ((_unit == null) || (value == null) || (!value.Equals(_unit)))
				{
                    object oldValue = _unit;
					_unit = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_Unit, oldValue, value);
				}
			}

		}

		[Property("ProductName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string ProductName
		{
			get { return _productName; }
			set
			{
				if ((_productName == null) || (value == null) || (!value.Equals(_productName)))
				{
                    object oldValue = _productName;
					_productName = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(OrderInvoiceDetail.Prop_ProductCode, oldValue, value);
				}
			}

		}

		[Property("Count", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Count
		{
			get { return _count; }
			set
			{
				if (value != _count)
				{
                    object oldValue = _count;
					_count = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_Count, oldValue, value);
				}
			}

		}

		[Property("SalePrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? SalePrice
		{
			get { return _salePrice; }
			set
			{
				if (value != _salePrice)
				{
                    object oldValue = _salePrice;
					_salePrice = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_SalePrice, oldValue, value);
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
					RaisePropertyChanged(OrderInvoiceDetail.Prop_Amount, oldValue, value);
				}
			}

		}

		[Property("InvoiceCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? InvoiceCount
		{
			get { return _invoiceCount; }
			set
			{
				if (value != _invoiceCount)
				{
                    object oldValue = _invoiceCount;
					_invoiceCount = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_InvoiceCount, oldValue, value);
				}
			}

		}

		[Property("HaveInvoiceCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? HaveInvoiceCount
		{
			get { return _haveInvoiceCount; }
			set
			{
				if (value != _haveInvoiceCount)
				{
                    object oldValue = _haveInvoiceCount;
					_haveInvoiceCount = value;
					RaisePropertyChanged(OrderInvoiceDetail.Prop_HaveInvoiceCount, oldValue, value);
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
					RaisePropertyChanged(OrderInvoiceDetail.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // OrderInvoiceDetail
}

