// Business class ReturnOrderPart generated from ReturnOrderPart
// Creator: Ray
// Created Date: [2013-07-12]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ReturnOrderPart")]
	public partial class ReturnOrderPart : ExamModelBase<ReturnOrderPart>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_ReturnOrderId = "ReturnOrderId";
		public static string Prop_OrderPartId = "OrderPartId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_Isbn = "Isbn";
		public static string Prop_Count = "Count";
		public static string Prop_Unit = "Unit";
		public static string Prop_ReturnPrice = "ReturnPrice";
		public static string Prop_Amount = "Amount";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _returnOrderId;
		private string _orderPartId;
		private string _productId;
		private string _productCode;
		private string _productName;
		private string _isbn;
		private int? _count;
		private string _unit;
		private System.Decimal? _returnPrice;
		private System.Decimal? _amount;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public ReturnOrderPart()
		{
		}

		public ReturnOrderPart(
			string p_id,
			string p_returnOrderId,
			string p_orderPartId,
			string p_productId,
			string p_productCode,
			string p_productName,
			string p_isbn,
			int? p_count,
			string p_unit,
			System.Decimal? p_returnPrice,
			System.Decimal? p_amount,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_returnOrderId = p_returnOrderId;
			_orderPartId = p_orderPartId;
			_productId = p_productId;
			_productCode = p_productCode;
			_productName = p_productName;
			_isbn = p_isbn;
			_count = p_count;
			_unit = p_unit;
			_returnPrice = p_returnPrice;
			_amount = p_amount;
			_remark = p_remark;
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

		[Property("ReturnOrderId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ReturnOrderId
		{
			get { return _returnOrderId; }
			set
			{
				if ((_returnOrderId == null) || (value == null) || (!value.Equals(_returnOrderId)))
				{
                    object oldValue = _returnOrderId;
					_returnOrderId = value;
					RaisePropertyChanged(ReturnOrderPart.Prop_ReturnOrderId, oldValue, value);
				}
			}

		}

		[Property("OrderPartId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OrderPartId
		{
			get { return _orderPartId; }
			set
			{
				if ((_orderPartId == null) || (value == null) || (!value.Equals(_orderPartId)))
				{
                    object oldValue = _orderPartId;
					_orderPartId = value;
					RaisePropertyChanged(ReturnOrderPart.Prop_OrderPartId, oldValue, value);
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
					RaisePropertyChanged(ReturnOrderPart.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("ProductCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ProductCode
		{
			get { return _productCode; }
			set
			{
				if ((_productCode == null) || (value == null) || (!value.Equals(_productCode)))
				{
                    object oldValue = _productCode;
					_productCode = value;
					RaisePropertyChanged(ReturnOrderPart.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(ReturnOrderPart.Prop_ProductName, oldValue, value);
				}
			}

		}

		[Property("Isbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Isbn
		{
			get { return _isbn; }
			set
			{
				if ((_isbn == null) || (value == null) || (!value.Equals(_isbn)))
				{
                    object oldValue = _isbn;
					_isbn = value;
					RaisePropertyChanged(ReturnOrderPart.Prop_Isbn, oldValue, value);
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
					RaisePropertyChanged(ReturnOrderPart.Prop_Count, oldValue, value);
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
					RaisePropertyChanged(ReturnOrderPart.Prop_Unit, oldValue, value);
				}
			}

		}

		[Property("ReturnPrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ReturnPrice
		{
			get { return _returnPrice; }
			set
			{
				if (value != _returnPrice)
				{
                    object oldValue = _returnPrice;
					_returnPrice = value;
					RaisePropertyChanged(ReturnOrderPart.Prop_ReturnPrice, oldValue, value);
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
					RaisePropertyChanged(ReturnOrderPart.Prop_Amount, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 2000)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(ReturnOrderPart.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(ReturnOrderPart.Prop_CreateId, oldValue, value);
				}
			}

		}

		[Property("CreateName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string CreateName
		{
			get { return _createName; }
			set
			{
				if ((_createName == null) || (value == null) || (!value.Equals(_createName)))
				{
                    object oldValue = _createName;
					_createName = value;
					RaisePropertyChanged(ReturnOrderPart.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(ReturnOrderPart.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // ReturnOrderPart
}

