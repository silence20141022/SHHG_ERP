// Business class CombineSplit generated from CombineSplit
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
	[ActiveRecord("CombineSplit")]
	public partial class CombineSplit : ExamModelBase<CombineSplit>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_CombineSplitNo = "CombineSplitNo";
		public static string Prop_OperateType = "OperateType";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductPcn = "ProductPcn";
		public static string Prop_StockQuantity = "StockQuantity";
		public static string Prop_ProductQuantity = "ProductQuantity";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_CombineSplitUserId = "CombineSplitUserId";
		public static string Prop_CombineSplitUserName = "CombineSplitUserName";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _combineSplitNo;
		private string _operateType;
		private string _productId;
		private string _productName;
		private string _productCode;
		private string _productPcn;
		private int? _stockQuantity;
		private int? _productQuantity;
		private string _warehouseId;
		private string _warehouseName;
		private string _combineSplitUserId;
		private string _combineSplitUserName;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;


		#endregion

		#region Constructors

		public CombineSplit()
		{
		}

		public CombineSplit(
			string p_id,
			string p_combineSplitNo,
			string p_operateType,
			string p_productId,
			string p_productName,
			string p_productCode,
			string p_productPcn,
			int? p_stockQuantity,
			int? p_productQuantity,
			string p_warehouseId,
			string p_warehouseName,
			string p_combineSplitUserId,
			string p_combineSplitUserName,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark)
		{
			_id = p_id;
			_combineSplitNo = p_combineSplitNo;
			_operateType = p_operateType;
			_productId = p_productId;
			_productName = p_productName;
			_productCode = p_productCode;
			_productPcn = p_productPcn;
			_stockQuantity = p_stockQuantity;
			_productQuantity = p_productQuantity;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_combineSplitUserId = p_combineSplitUserId;
			_combineSplitUserName = p_combineSplitUserName;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
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

		[Property("CombineSplitNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CombineSplitNo
		{
			get { return _combineSplitNo; }
			set
			{
				if ((_combineSplitNo == null) || (value == null) || (!value.Equals(_combineSplitNo)))
				{
                    object oldValue = _combineSplitNo;
					_combineSplitNo = value;
					RaisePropertyChanged(CombineSplit.Prop_CombineSplitNo, oldValue, value);
				}
			}

		}

		[Property("OperateType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OperateType
		{
			get { return _operateType; }
			set
			{
				if ((_operateType == null) || (value == null) || (!value.Equals(_operateType)))
				{
                    object oldValue = _operateType;
					_operateType = value;
					RaisePropertyChanged(CombineSplit.Prop_OperateType, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_ProductId, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_ProductPcn, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_StockQuantity, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_ProductQuantity, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_WarehouseId, oldValue, value);
				}
			}

		}

		[Property("WarehouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string WarehouseName
		{
			get { return _warehouseName; }
			set
			{
				if ((_warehouseName == null) || (value == null) || (!value.Equals(_warehouseName)))
				{
                    object oldValue = _warehouseName;
					_warehouseName = value;
					RaisePropertyChanged(CombineSplit.Prop_WarehouseName, oldValue, value);
				}
			}

		}

		[Property("CombineSplitUserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CombineSplitUserId
		{
			get { return _combineSplitUserId; }
			set
			{
				if ((_combineSplitUserId == null) || (value == null) || (!value.Equals(_combineSplitUserId)))
				{
                    object oldValue = _combineSplitUserId;
					_combineSplitUserId = value;
					RaisePropertyChanged(CombineSplit.Prop_CombineSplitUserId, oldValue, value);
				}
			}

		}

		[Property("CombineSplitUserName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CombineSplitUserName
		{
			get { return _combineSplitUserName; }
			set
			{
				if ((_combineSplitUserName == null) || (value == null) || (!value.Equals(_combineSplitUserName)))
				{
                    object oldValue = _combineSplitUserName;
					_combineSplitUserName = value;
					RaisePropertyChanged(CombineSplit.Prop_CombineSplitUserName, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(CombineSplit.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // CombineSplit
}

