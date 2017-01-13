// Business class WarehouseExchangeDetail generated from WarehouseExchangeDetail
// Creator: Ray
// Created Date: [2012-05-19]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("WarehouseExchangeDetail")]
	public partial class WarehouseExchangeDetail : ExamModelBase<WarehouseExchangeDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_WarehouseExchangeId = "WarehouseExchangeId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductIsbn = "ProductIsbn";
		public static string Prop_ProductPcn = "ProductPcn";
		public static string Prop_ExchangeQuantity = "ExchangeQuantity";

		#endregion

		#region Private_Variables

		private string _id;
		private string _warehouseExchangeId;
		private string _productId;
		private string _productName;
		private string _productCode;
		private string _productIsbn;
		private string _productPcn;
		private int? _exchangeQuantity;


		#endregion

		#region Constructors

		public WarehouseExchangeDetail()
		{
		}

		public WarehouseExchangeDetail(
			string p_id,
			string p_warehouseExchangeId,
			string p_productId,
			string p_productName,
			string p_productCode,
			string p_productIsbn,
			string p_productPcn,
			int? p_exchangeQuantity)
		{
			_id = p_id;
			_warehouseExchangeId = p_warehouseExchangeId;
			_productId = p_productId;
			_productName = p_productName;
			_productCode = p_productCode;
			_productIsbn = p_productIsbn;
			_productPcn = p_productPcn;
			_exchangeQuantity = p_exchangeQuantity;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("WarehouseExchangeId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string WarehouseExchangeId
		{
			get { return _warehouseExchangeId; }
			set
			{
				if ((_warehouseExchangeId == null) || (value == null) || (!value.Equals(_warehouseExchangeId)))
				{
                    object oldValue = _warehouseExchangeId;
					_warehouseExchangeId = value;
					RaisePropertyChanged(WarehouseExchangeDetail.Prop_WarehouseExchangeId, oldValue, value);
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
					RaisePropertyChanged(WarehouseExchangeDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("ProductName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ProductName
		{
			get { return _productName; }
			set
			{
				if ((_productName == null) || (value == null) || (!value.Equals(_productName)))
				{
                    object oldValue = _productName;
					_productName = value;
					RaisePropertyChanged(WarehouseExchangeDetail.Prop_ProductName, oldValue, value);
				}
			}

		}

		[Property("ProductCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string ProductCode
		{
			get { return _productCode; }
			set
			{
				if ((_productCode == null) || (value == null) || (!value.Equals(_productCode)))
				{
                    object oldValue = _productCode;
					_productCode = value;
					RaisePropertyChanged(WarehouseExchangeDetail.Prop_ProductCode, oldValue, value);
				}
			}

		}

		[Property("ProductIsbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ProductIsbn
		{
			get { return _productIsbn; }
			set
			{
				if ((_productIsbn == null) || (value == null) || (!value.Equals(_productIsbn)))
				{
                    object oldValue = _productIsbn;
					_productIsbn = value;
					RaisePropertyChanged(WarehouseExchangeDetail.Prop_ProductIsbn, oldValue, value);
				}
			}

		}

		[Property("ProductPcn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ProductPcn
		{
			get { return _productPcn; }
			set
			{
				if ((_productPcn == null) || (value == null) || (!value.Equals(_productPcn)))
				{
                    object oldValue = _productPcn;
					_productPcn = value;
					RaisePropertyChanged(WarehouseExchangeDetail.Prop_ProductPcn, oldValue, value);
				}
			}

		}

		[Property("ExchangeQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? ExchangeQuantity
		{
			get { return _exchangeQuantity; }
			set
			{
				if (value != _exchangeQuantity)
				{
                    object oldValue = _exchangeQuantity;
					_exchangeQuantity = value;
					RaisePropertyChanged(WarehouseExchangeDetail.Prop_ExchangeQuantity, oldValue, value);
				}
			}

		}

		#endregion
	} // WarehouseExchangeDetail
}

