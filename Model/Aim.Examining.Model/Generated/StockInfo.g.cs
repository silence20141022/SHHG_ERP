// Business class StockInfo generated from StockInfo
// Creator: Ray
// Created Date: [2012-02-24]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("StockInfo")]
	public partial class StockInfo : ExamModelBase<StockInfo>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_StockQuantity = "StockQuantity";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _productId;
		private string _productCode;
		private string _productName;
		private string _warehouseId;
		private string _warehouseName;
		private int? _stockQuantity;
		private string _remark;


		#endregion

		#region Constructors

		public StockInfo()
		{
		}

		public StockInfo(
			string p_id,
			string p_productId,
			string p_productCode,
			string p_productName,
			string p_warehouseId,
			string p_warehouseName,
			int? p_stockQuantity,
			string p_remark)
		{
			_id = p_id;
			_productId = p_productId;
			_productCode = p_productCode;
			_productName = p_productName;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_stockQuantity = p_stockQuantity;
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
					RaisePropertyChanged(StockInfo.Prop_ProductId, oldValue, value);
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
					RaisePropertyChanged(StockInfo.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(StockInfo.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(StockInfo.Prop_WarehouseId, oldValue, value);
				}
			}

		}

		[Property("WarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string WarehouseName
		{
			get { return _warehouseName; }
			set
			{
				if ((_warehouseName == null) || (value == null) || (!value.Equals(_warehouseName)))
				{
                    object oldValue = _warehouseName;
					_warehouseName = value;
					RaisePropertyChanged(StockInfo.Prop_WarehouseName, oldValue, value);
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
					RaisePropertyChanged(StockInfo.Prop_StockQuantity, oldValue, value);
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
					RaisePropertyChanged(StockInfo.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // StockInfo
}

