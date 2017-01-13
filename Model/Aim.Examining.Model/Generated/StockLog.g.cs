// Business class StockLog generated from StockLog
// Creator: Ray
// Created Date: [2013-02-01]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("StockLog")]
	public partial class StockLog : ExamModelBase<StockLog>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_InOrOutDetailId = "InOrOutDetailId";
		public static string Prop_InOrOutBillNo = "InOrOutBillNo";
		public static string Prop_OperateType = "OperateType";
		public static string Prop_WarehouseId = "WarehouseId";
		public static string Prop_WarehouseName = "WarehouseName";
		public static string Prop_StockQuantity = "StockQuantity";
		public static string Prop_Quantity = "Quantity";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductPcn = "ProductPcn";
		public static string Prop_ProductIsbn = "ProductIsbn";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _inOrOutDetailId;
		private string _inOrOutBillNo;
		private string _operateType;
		private string _warehouseId;
		private string _warehouseName;
		private int? _stockQuantity;
		private int? _quantity;
		private string _productId;
		private string _productName;
		private string _productCode;
		private string _productPcn;
		private string _productIsbn;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public StockLog()
		{
		}

		public StockLog(
			string p_id,
			string p_inOrOutDetailId,
			string p_inOrOutBillNo,
			string p_operateType,
			string p_warehouseId,
			string p_warehouseName,
			int? p_stockQuantity,
			int? p_quantity,
			string p_productId,
			string p_productName,
			string p_productCode,
			string p_productPcn,
			string p_productIsbn,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_inOrOutDetailId = p_inOrOutDetailId;
			_inOrOutBillNo = p_inOrOutBillNo;
			_operateType = p_operateType;
			_warehouseId = p_warehouseId;
			_warehouseName = p_warehouseName;
			_stockQuantity = p_stockQuantity;
			_quantity = p_quantity;
			_productId = p_productId;
			_productName = p_productName;
			_productCode = p_productCode;
			_productPcn = p_productPcn;
			_productIsbn = p_productIsbn;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("InOrOutDetailId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InOrOutDetailId
		{
			get { return _inOrOutDetailId; }
			set
			{
				if ((_inOrOutDetailId == null) || (value == null) || (!value.Equals(_inOrOutDetailId)))
				{
                    object oldValue = _inOrOutDetailId;
					_inOrOutDetailId = value;
					RaisePropertyChanged(StockLog.Prop_InOrOutDetailId, oldValue, value);
				}
			}

		}

		[Property("InOrOutBillNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InOrOutBillNo
		{
			get { return _inOrOutBillNo; }
			set
			{
				if ((_inOrOutBillNo == null) || (value == null) || (!value.Equals(_inOrOutBillNo)))
				{
                    object oldValue = _inOrOutBillNo;
					_inOrOutBillNo = value;
					RaisePropertyChanged(StockLog.Prop_InOrOutBillNo, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_OperateType, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_WarehouseId, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_WarehouseName, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_StockQuantity, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_Quantity, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_ProductId, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_ProductCode, oldValue, value);
				}
			}

		}

		[Property("ProductPcn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string ProductPcn
		{
			get { return _productPcn; }
			set
			{
				if ((_productPcn == null) || (value == null) || (!value.Equals(_productPcn)))
				{
                    object oldValue = _productPcn;
					_productPcn = value;
					RaisePropertyChanged(StockLog.Prop_ProductPcn, oldValue, value);
				}
			}

		}

		[Property("ProductIsbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string ProductIsbn
		{
			get { return _productIsbn; }
			set
			{
				if ((_productIsbn == null) || (value == null) || (!value.Equals(_productIsbn)))
				{
                    object oldValue = _productIsbn;
					_productIsbn = value;
					RaisePropertyChanged(StockLog.Prop_ProductIsbn, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(StockLog.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // StockLog
}

