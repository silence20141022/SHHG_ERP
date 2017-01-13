// Business class ChangeOrderPart generated from ChangeOrderPart
// Creator: Ray
// Created Date: [2012-05-24]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ChangeOrderPart")]
	public partial class ChangeOrderPart : ExamModelBase<ChangeOrderPart>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PId = "PId";
		public static string Prop_OId = "OId";
		public static string Prop_ONumber = "ONumber";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_ProductName = "ProductName";
		public static string Prop_Isbn = "Isbn";
		public static string Prop_Count = "Count";
		public static string Prop_Unit = "Unit";
		public static string Prop_ReturnPrice = "ReturnPrice";
		public static string Prop_Amount = "Amount";
		public static string Prop_Reason = "Reason";
		public static string Prop_Remark = "Remark";
		public static string Prop_BillingCount = "BillingCount";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _pId;
		private string _oId;
		private string _oNumber;
		private string _productId;
		private string _productCode;
		private string _productName;
		private string _isbn;
		private int? _count;
		private string _unit;
		private System.Decimal? _returnPrice;
		private System.Decimal? _amount;
		private string _reason;
		private string _remark;
		private int? _billingCount;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public ChangeOrderPart()
		{
		}

		public ChangeOrderPart(
			string p_id,
			string p_pId,
			string p_oId,
			string p_oNumber,
			string p_productId,
			string p_productCode,
			string p_productName,
			string p_isbn,
			int? p_count,
			string p_unit,
			System.Decimal? p_returnPrice,
			System.Decimal? p_amount,
			string p_reason,
			string p_remark,
			int? p_billingCount,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_pId = p_pId;
			_oId = p_oId;
			_oNumber = p_oNumber;
			_productId = p_productId;
			_productCode = p_productCode;
			_productName = p_productName;
			_isbn = p_isbn;
			_count = p_count;
			_unit = p_unit;
			_returnPrice = p_returnPrice;
			_amount = p_amount;
			_reason = p_reason;
			_remark = p_remark;
			_billingCount = p_billingCount;
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

		[Property("PId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PId
		{
			get { return _pId; }
			set
			{
				if ((_pId == null) || (value == null) || (!value.Equals(_pId)))
				{
                    object oldValue = _pId;
					_pId = value;
					RaisePropertyChanged(ChangeOrderPart.Prop_PId, oldValue, value);
				}
			}

		}

		[Property("OId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OId
		{
			get { return _oId; }
			set
			{
				if ((_oId == null) || (value == null) || (!value.Equals(_oId)))
				{
                    object oldValue = _oId;
					_oId = value;
					RaisePropertyChanged(ChangeOrderPart.Prop_OId, oldValue, value);
				}
			}

		}

		[Property("ONumber", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ONumber
		{
			get { return _oNumber; }
			set
			{
				if ((_oNumber == null) || (value == null) || (!value.Equals(_oNumber)))
				{
                    object oldValue = _oNumber;
					_oNumber = value;
					RaisePropertyChanged(ChangeOrderPart.Prop_ONumber, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_ProductId, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_ProductName, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_Isbn, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_Count, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_Unit, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_ReturnPrice, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_Amount, oldValue, value);
				}
			}

		}

		[Property("Reason", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Reason
		{
			get { return _reason; }
			set
			{
				if ((_reason == null) || (value == null) || (!value.Equals(_reason)))
				{
                    object oldValue = _reason;
					_reason = value;
					RaisePropertyChanged(ChangeOrderPart.Prop_Reason, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("BillingCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? BillingCount
		{
			get { return _billingCount; }
			set
			{
				if (value != _billingCount)
				{
                    object oldValue = _billingCount;
					_billingCount = value;
					RaisePropertyChanged(ChangeOrderPart.Prop_BillingCount, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(ChangeOrderPart.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // ChangeOrderPart
}

