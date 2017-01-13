// Business class OtherInWarehouseDetail generated from OtherInWarehouseDetail
// Creator: Ray
// Created Date: [2012-04-01]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("OtherInWarehouseDetail")]
	public partial class OtherInWarehouseDetail : ExamModelBase<OtherInWarehouseDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_InWarehouseId = "InWarehouseId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductPCN = "ProductPCN";
		public static string Prop_ProductISBN = "ProductISBN";
		public static string Prop_ProductType = "ProductType";
		public static string Prop_InWarehouseState = "InWarehouseState";
		public static string Prop_Quantity = "Quantity";
		public static string Prop_Remark = "Remark";
		public static string Prop_SeriesArray = "SeriesArray";
		public static string Prop_SkinArray = "SkinArray";

		#endregion

		#region Private_Variables

		private string _id;
		private string _inWarehouseId;
		private string _productId;
		private string _productName;
		private string _productCode;
		private string _productPCN;
		private string _productISBN;
		private string _productType;
		private string _inWarehouseState;
		private int? _quantity;
		private string _remark;
		private string _seriesArray;
		private string _skinArray;


		#endregion

		#region Constructors

		public OtherInWarehouseDetail()
		{
		}

		public OtherInWarehouseDetail(
			string p_id,
			string p_inWarehouseId,
			string p_productId,
			string p_productName,
			string p_productCode,
			string p_productPCN,
			string p_productISBN,
			string p_productType,
			string p_inWarehouseState,
			int? p_quantity,
			string p_remark,
			string p_seriesArray,
			string p_skinArray)
		{
			_id = p_id;
			_inWarehouseId = p_inWarehouseId;
			_productId = p_productId;
			_productName = p_productName;
			_productCode = p_productCode;
			_productPCN = p_productPCN;
			_productISBN = p_productISBN;
			_productType = p_productType;
			_inWarehouseState = p_inWarehouseState;
			_quantity = p_quantity;
			_remark = p_remark;
			_seriesArray = p_seriesArray;
			_skinArray = p_skinArray;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("InWarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InWarehouseId
		{
			get { return _inWarehouseId; }
			set
			{
				if ((_inWarehouseId == null) || (value == null) || (!value.Equals(_inWarehouseId)))
				{
                    object oldValue = _inWarehouseId;
					_inWarehouseId = value;
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_InWarehouseId, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_ProductId, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_ProductPCN, oldValue, value);
				}
			}

		}

		[Property("ProductISBN", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ProductISBN
		{
			get { return _productISBN; }
			set
			{
				if ((_productISBN == null) || (value == null) || (!value.Equals(_productISBN)))
				{
                    object oldValue = _productISBN;
					_productISBN = value;
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_ProductISBN, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_ProductType, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_InWarehouseState, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_Quantity, oldValue, value);
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
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("SeriesArray", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string SeriesArray
		{
			get { return _seriesArray; }
			set
			{
				if ((_seriesArray == null) || (value == null) || (!value.Equals(_seriesArray)))
				{
                    object oldValue = _seriesArray;
					_seriesArray = value;
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_SeriesArray, oldValue, value);
				}
			}

		}

		[Property("SkinArray", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string SkinArray
		{
			get { return _skinArray; }
			set
			{
				if ((_skinArray == null) || (value == null) || (!value.Equals(_skinArray)))
				{
                    object oldValue = _skinArray;
					_skinArray = value;
					RaisePropertyChanged(OtherInWarehouseDetail.Prop_SkinArray, oldValue, value);
				}
			}

		}

		#endregion
	} // OtherInWarehouseDetail
}

