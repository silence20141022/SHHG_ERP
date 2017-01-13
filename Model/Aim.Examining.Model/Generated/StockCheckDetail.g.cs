// Business class StockCheckDetail generated from StockCheckDetail
// Creator: Ray
// Created Date: [2012-07-28]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("StockCheckDetail")]
	public partial class StockCheckDetail : ExamModelBase<StockCheckDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_StockCheckId = "StockCheckId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductPcn = "ProductPcn";
		public static string Prop_StockCheckQuantity = "StockCheckQuantity";
		public static string Prop_StockQuantity = "StockQuantity";
		public static string Prop_StockCheckResult = "StockCheckResult";
		public static string Prop_StockCheckState = "StockCheckState";

		#endregion

		#region Private_Variables

		private string _id;
		private string _stockCheckId;
		private string _productId;
		private string _productName;
		private string _productCode;
		private string _productPcn;
		private int? _stockCheckQuantity;
		private int? _stockQuantity;
		private string _stockCheckResult;
		private string _stockCheckState;


		#endregion

		#region Constructors

		public StockCheckDetail()
		{
		}

		public StockCheckDetail(
			string p_id,
			string p_stockCheckId,
			string p_productId,
			string p_productName,
			string p_productCode,
			string p_productPcn,
			int? p_stockCheckQuantity,
			int? p_stockQuantity,
			string p_stockCheckResult,
			string p_stockCheckState)
		{
			_id = p_id;
			_stockCheckId = p_stockCheckId;
			_productId = p_productId;
			_productName = p_productName;
			_productCode = p_productCode;
			_productPcn = p_productPcn;
			_stockCheckQuantity = p_stockCheckQuantity;
			_stockQuantity = p_stockQuantity;
			_stockCheckResult = p_stockCheckResult;
			_stockCheckState = p_stockCheckState;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("StockCheckId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string StockCheckId
		{
			get { return _stockCheckId; }
			set
			{
				if ((_stockCheckId == null) || (value == null) || (!value.Equals(_stockCheckId)))
				{
                    object oldValue = _stockCheckId;
					_stockCheckId = value;
					RaisePropertyChanged(StockCheckDetail.Prop_StockCheckId, oldValue, value);
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
					RaisePropertyChanged(StockCheckDetail.Prop_ProductId, oldValue, value);
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
					RaisePropertyChanged(StockCheckDetail.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(StockCheckDetail.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(StockCheckDetail.Prop_ProductPcn, oldValue, value);
				}
			}

		}

		[Property("StockCheckQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? StockCheckQuantity
		{
			get { return _stockCheckQuantity; }
			set
			{
				if (value != _stockCheckQuantity)
				{
                    object oldValue = _stockCheckQuantity;
					_stockCheckQuantity = value;
					RaisePropertyChanged(StockCheckDetail.Prop_StockCheckQuantity, oldValue, value);
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
					RaisePropertyChanged(StockCheckDetail.Prop_StockQuantity, oldValue, value);
				}
			}

		}

		[Property("StockCheckResult", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string StockCheckResult
		{
			get { return _stockCheckResult; }
			set
			{
				if ((_stockCheckResult == null) || (value == null) || (!value.Equals(_stockCheckResult)))
				{
                    object oldValue = _stockCheckResult;
					_stockCheckResult = value;
					RaisePropertyChanged(StockCheckDetail.Prop_StockCheckResult, oldValue, value);
				}
			}

		}

		[Property("StockCheckState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string StockCheckState
		{
			get { return _stockCheckState; }
			set
			{
				if ((_stockCheckState == null) || (value == null) || (!value.Equals(_stockCheckState)))
				{
                    object oldValue = _stockCheckState;
					_stockCheckState = value;
					RaisePropertyChanged(StockCheckDetail.Prop_StockCheckState, oldValue, value);
				}
			}

		}

		#endregion
	} // StockCheckDetail
}

