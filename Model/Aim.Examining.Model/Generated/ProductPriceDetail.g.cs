// Business class ProductPriceDetail generated from ProductPriceDetail
// Creator: Ray
// Created Date: [2012-03-02]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ProductPriceDetail")]
	public partial class ProductPriceDetail : ExamModelBase<ProductPriceDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_EnumerationID = "EnumerationID";
		public static string Prop_PriceTypeName = "PriceTypeName";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductPCN = "ProductPCN";
		public static string Prop_Price = "Price";

		#endregion

		#region Private_Variables

		private string _id;
		private string _productId;
		private string _enumerationID;
		private string _priceTypeName;
		private string _productName;
		private string _productCode;
		private string _productPCN;
		private System.Decimal? _price;


		#endregion

		#region Constructors

		public ProductPriceDetail()
		{
		}

		public ProductPriceDetail(
			string p_id,
			string p_productId,
			string p_enumerationID,
			string p_priceTypeName,
			string p_productName,
			string p_productCode,
			string p_productPCN,
			System.Decimal? p_price)
		{
			_id = p_id;
			_productId = p_productId;
			_enumerationID = p_enumerationID;
			_priceTypeName = p_priceTypeName;
			_productName = p_productName;
			_productCode = p_productCode;
			_productPCN = p_productPCN;
			_price = p_price;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(ProductPriceDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("EnumerationID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string EnumerationID
		{
			get { return _enumerationID; }
			set
			{
				if ((_enumerationID == null) || (value == null) || (!value.Equals(_enumerationID)))
				{
                    object oldValue = _enumerationID;
					_enumerationID = value;
					RaisePropertyChanged(ProductPriceDetail.Prop_EnumerationID, oldValue, value);
				}
			}

		}

		[Property("PriceTypeName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PriceTypeName
		{
			get { return _priceTypeName; }
			set
			{
				if ((_priceTypeName == null) || (value == null) || (!value.Equals(_priceTypeName)))
				{
                    object oldValue = _priceTypeName;
					_priceTypeName = value;
					RaisePropertyChanged(ProductPriceDetail.Prop_PriceTypeName, oldValue, value);
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
					RaisePropertyChanged(ProductPriceDetail.Prop_ProductName, oldValue, value);
				}
			}

		}

		[Property("ProductCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ProductCode
		{
			get { return _productCode; }
			set
			{
				if ((_productCode == null) || (value == null) || (!value.Equals(_productCode)))
				{
                    object oldValue = _productCode;
					_productCode = value;
					RaisePropertyChanged(ProductPriceDetail.Prop_ProductCode, oldValue, value);
				}
			}

		}

		[Property("ProductPCN", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ProductPCN
		{
			get { return _productPCN; }
			set
			{
				if ((_productPCN == null) || (value == null) || (!value.Equals(_productPCN)))
				{
                    object oldValue = _productPCN;
					_productPCN = value;
					RaisePropertyChanged(ProductPriceDetail.Prop_ProductPCN, oldValue, value);
				}
			}

		}

		[Property("Price", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Price
		{
			get { return _price; }
			set
			{
				if (value != _price)
				{
                    object oldValue = _price;
					_price = value;
					RaisePropertyChanged(ProductPriceDetail.Prop_Price, oldValue, value);
				}
			}

		}

		#endregion
	} // ProductPriceDetail
}

