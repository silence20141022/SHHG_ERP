// Business class OrderDetail_FenGongSi generated from OrderDetail_FenGongSi
// Creator: Ray
// Created Date: [2015-01-29]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("OrderDetail_FenGongSi")]
	public partial class OrderDetail_FenGongSi : ExamModelBase<OrderDetail_FenGongSi>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Order_FenGongSi_Id = "Order_FenGongSi_Id";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_Name = "Name";
		public static string Prop_Code = "Code";
		public static string Prop_Pcn = "Pcn";
		public static string Prop_Quantity = "Quantity";
		public static string Prop_PurchasePrice = "PurchasePrice";
		public static string Prop_SecondPrice = "SecondPrice";
		public static string Prop_OrderPart_Id = "OrderPart_Id";
		public static string Prop_Amount = "Amount";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _order_FenGongSi_Id;
		private string _productId;
		private string _name;
		private string _code;
		private string _pcn;
		private int? _quantity;
		private System.Decimal? _purchasePrice;
		private System.Decimal? _secondPrice;
		private string _orderPart_Id;
		private System.Decimal? _amount;
		private string _remark;


		#endregion

		#region Constructors

		public OrderDetail_FenGongSi()
		{
		}

		public OrderDetail_FenGongSi(
			string p_id,
			string p_order_FenGongSi_Id,
			string p_productId,
			string p_name,
			string p_code,
			string p_pcn,
			int? p_quantity,
			System.Decimal? p_purchasePrice,
			System.Decimal? p_secondPrice,
			string p_orderPart_Id,
			System.Decimal? p_amount,
			string p_remark)
		{
			_id = p_id;
			_order_FenGongSi_Id = p_order_FenGongSi_Id;
			_productId = p_productId;
			_name = p_name;
			_code = p_code;
			_pcn = p_pcn;
			_quantity = p_quantity;
			_purchasePrice = p_purchasePrice;
			_secondPrice = p_secondPrice;
			_orderPart_Id = p_orderPart_Id;
			_amount = p_amount;
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

		[Property("Order_FenGongSi_Id", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string Order_FenGongSi_Id
		{
			get { return _order_FenGongSi_Id; }
			set
			{
				if ((_order_FenGongSi_Id == null) || (value == null) || (!value.Equals(_order_FenGongSi_Id)))
				{
                    object oldValue = _order_FenGongSi_Id;
					_order_FenGongSi_Id = value;
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_Order_FenGongSi_Id, oldValue, value);
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
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Pcn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Pcn
		{
			get { return _pcn; }
			set
			{
				if ((_pcn == null) || (value == null) || (!value.Equals(_pcn)))
				{
                    object oldValue = _pcn;
					_pcn = value;
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_Pcn, oldValue, value);
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
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_Quantity, oldValue, value);
				}
			}

		}

		[Property("PurchasePrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PurchasePrice
		{
			get { return _purchasePrice; }
			set
			{
				if (value != _purchasePrice)
				{
                    object oldValue = _purchasePrice;
					_purchasePrice = value;
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_PurchasePrice, oldValue, value);
				}
			}

		}

		[Property("SecondPrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? SecondPrice
		{
			get { return _secondPrice; }
			set
			{
				if (value != _secondPrice)
				{
                    object oldValue = _secondPrice;
					_secondPrice = value;
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_SecondPrice, oldValue, value);
				}
			}

		}

		[Property("OrderPart_Id", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OrderPart_Id
		{
			get { return _orderPart_Id; }
			set
			{
				if ((_orderPart_Id == null) || (value == null) || (!value.Equals(_orderPart_Id)))
				{
                    object oldValue = _orderPart_Id;
					_orderPart_Id = value;
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_OrderPart_Id, oldValue, value);
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
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_Amount, oldValue, value);
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
					RaisePropertyChanged(OrderDetail_FenGongSi.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // OrderDetail_FenGongSi
}

