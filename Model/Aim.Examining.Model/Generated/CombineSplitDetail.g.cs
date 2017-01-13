// Business class CombineSplitDetail generated from CombineSplitDetail
// Creator: Ray
// Created Date: [2012-06-26]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("CombineSplitDetail")]
	public partial class CombineSplitDetail : ExamModelBase<CombineSplitDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_CombineSplitId = "CombineSplitId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductPcn = "ProductPcn";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductQuantity = "ProductQuantity";
		public static string Prop_StockQuantity = "StockQuantity";

		#endregion

		#region Private_Variables

		private string _id;
		private string _combineSplitId;
		private string _productId;
		private string _productPcn;
		private string _productCode;
		private string _productName;
		private int? _productQuantity;
		private int? _stockQuantity;


		#endregion

		#region Constructors

		public CombineSplitDetail()
		{
		}

		public CombineSplitDetail(
			string p_id,
			string p_combineSplitId,
			string p_productId,
			string p_productPcn,
			string p_productCode,
			string p_productName,
			int? p_productQuantity,
			int? p_stockQuantity)
		{
			_id = p_id;
			_combineSplitId = p_combineSplitId;
			_productId = p_productId;
			_productPcn = p_productPcn;
			_productCode = p_productCode;
			_productName = p_productName;
			_productQuantity = p_productQuantity;
			_stockQuantity = p_stockQuantity;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("CombineSplitId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CombineSplitId
		{
			get { return _combineSplitId; }
			set
			{
				if ((_combineSplitId == null) || (value == null) || (!value.Equals(_combineSplitId)))
				{
                    object oldValue = _combineSplitId;
					_combineSplitId = value;
					RaisePropertyChanged(CombineSplitDetail.Prop_CombineSplitId, oldValue, value);
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
					RaisePropertyChanged(CombineSplitDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("ProductPcn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string ProductPcn
		{
			get { return _productPcn; }
			set
			{
				if ((_productPcn == null) || (value == null) || (!value.Equals(_productPcn)))
				{
                    object oldValue = _productPcn;
					_productPcn = value;
					RaisePropertyChanged(CombineSplitDetail.Prop_ProductPcn, oldValue, value);
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
					RaisePropertyChanged(CombineSplitDetail.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(CombineSplitDetail.Prop_ProductName, oldValue, value);
				}
			}

		}

		[Property("ProductQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? ProductQuantity
		{
			get { return _productQuantity; }
			set
			{
				if (value != _productQuantity)
				{
                    object oldValue = _productQuantity;
					_productQuantity = value;
					RaisePropertyChanged(CombineSplitDetail.Prop_ProductQuantity, oldValue, value);
				}
			}

		}

		[Property("StockQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? StockQuantity
		{
			get { return _stockQuantity; }
			set
			{
				if (value != _stockQuantity)
				{
                    object oldValue = _stockQuantity;
					_stockQuantity = value;
					RaisePropertyChanged(CombineSplitDetail.Prop_StockQuantity, oldValue, value);
				}
			}

		}

		#endregion
	} // CombineSplitDetail
}

