// Business class OrdersPart generated from OrdersPart
// Creator: Ray
// Created Date: [2015-01-30]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("OrdersPart")]
	public partial class OrdersPart : ExamModelBase<OrdersPart>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_OId = "OId";
		public static string Prop_PId = "PId";
		public static string Prop_PCode = "PCode";
		public static string Prop_PName = "PName";
		public static string Prop_Isbn = "Isbn";
		public static string Prop_Count = "Count";
		public static string Prop_Unit = "Unit";
		public static string Prop_MinSalePrice = "MinSalePrice";
		public static string Prop_SalePrice = "SalePrice";
		public static string Prop_Amount = "Amount";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_IsValid = "IsValid";
		public static string Prop_Guids = "Guids";
		public static string Prop_OutCount = "OutCount";
		public static string Prop_BillingCount = "BillingCount";
		public static string Prop_ReturnCount = "ReturnCount";
		public static string Prop_CustomerOrderNo = "CustomerOrderNo";
		public static string Prop_SaleQuan = "SaleQuan";

		#endregion

		#region Private_Variables

		private string _id;
		private string _oId;
		private string _pId;
		private string _pCode;
		private string _pName;
		private string _isbn;
		private int? _count;
		private string _unit;
		private System.Decimal? _minSalePrice;
		private System.Decimal? _salePrice;
		private System.Decimal? _amount;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private bool? _isValid;
		private string _guids;
		private int? _outCount;
		private int? _billingCount;
		private int? _returnCount;
		private string _customerOrderNo;
		private int? _saleQuan;


		#endregion

		#region Constructors

		public OrdersPart()
		{
		}

		public OrdersPart(
			string p_id,
			string p_oId,
			string p_pId,
			string p_pCode,
			string p_pName,
			string p_isbn,
			int? p_count,
			string p_unit,
			System.Decimal? p_minSalePrice,
			System.Decimal? p_salePrice,
			System.Decimal? p_amount,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			bool? p_isValid,
			string p_guids,
			int? p_outCount,
			int? p_billingCount,
			int? p_returnCount,
			string p_customerOrderNo,
			int? p_saleQuan)
		{
			_id = p_id;
			_oId = p_oId;
			_pId = p_pId;
			_pCode = p_pCode;
			_pName = p_pName;
			_isbn = p_isbn;
			_count = p_count;
			_unit = p_unit;
			_minSalePrice = p_minSalePrice;
			_salePrice = p_salePrice;
			_amount = p_amount;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_isValid = p_isValid;
			_guids = p_guids;
			_outCount = p_outCount;
			_billingCount = p_billingCount;
			_returnCount = p_returnCount;
			_customerOrderNo = p_customerOrderNo;
			_saleQuan = p_saleQuan;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(OrdersPart.Prop_OId, oldValue, value);
				}
			}

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
					RaisePropertyChanged(OrdersPart.Prop_PId, oldValue, value);
				}
			}

		}

		[Property("PCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PCode
		{
			get { return _pCode; }
			set
			{
				if ((_pCode == null) || (value == null) || (!value.Equals(_pCode)))
				{
                    object oldValue = _pCode;
					_pCode = value;
					RaisePropertyChanged(OrdersPart.Prop_PCode, oldValue, value);
				}
			}

		}

		[Property("PName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string PName
		{
			get { return _pName; }
			set
			{
				if ((_pName == null) || (value == null) || (!value.Equals(_pName)))
				{
                    object oldValue = _pName;
					_pName = value;
					RaisePropertyChanged(OrdersPart.Prop_PName, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_Isbn, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_Count, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_Unit, oldValue, value);
				}
			}

		}

		[Property("MinSalePrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? MinSalePrice
		{
			get { return _minSalePrice; }
			set
			{
				if (value != _minSalePrice)
				{
                    object oldValue = _minSalePrice;
					_minSalePrice = value;
					RaisePropertyChanged(OrdersPart.Prop_MinSalePrice, oldValue, value);
				}
			}

		}

		[Property("SalePrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? SalePrice
		{
			get { return _salePrice; }
			set
			{
				if (value != _salePrice)
				{
                    object oldValue = _salePrice;
					_salePrice = value;
					RaisePropertyChanged(OrdersPart.Prop_SalePrice, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_Amount, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("IsValid", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public bool? IsValid
		{
			get { return _isValid; }
			set
			{
				if (value != _isValid)
				{
                    object oldValue = _isValid;
					_isValid = value;
					RaisePropertyChanged(OrdersPart.Prop_IsValid, oldValue, value);
				}
			}

		}

		[Property("Guids", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Guids
		{
			get { return _guids; }
			set
			{
				if ((_guids == null) || (value == null) || (!value.Equals(_guids)))
				{
                    object oldValue = _guids;
					_guids = value;
					RaisePropertyChanged(OrdersPart.Prop_Guids, oldValue, value);
				}
			}

		}

		[Property("OutCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? OutCount
		{
			get { return _outCount; }
			set
			{
				if (value != _outCount)
				{
                    object oldValue = _outCount;
					_outCount = value;
					RaisePropertyChanged(OrdersPart.Prop_OutCount, oldValue, value);
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
					RaisePropertyChanged(OrdersPart.Prop_BillingCount, oldValue, value);
				}
			}

		}

		[Property("ReturnCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? ReturnCount
		{
			get { return _returnCount; }
			set
			{
				if (value != _returnCount)
				{
                    object oldValue = _returnCount;
					_returnCount = value;
					RaisePropertyChanged(OrdersPart.Prop_ReturnCount, oldValue, value);
				}
			}

		}

		[Property("CustomerOrderNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CustomerOrderNo
		{
			get { return _customerOrderNo; }
			set
			{
				if ((_customerOrderNo == null) || (value == null) || (!value.Equals(_customerOrderNo)))
				{
                    object oldValue = _customerOrderNo;
					_customerOrderNo = value;
					RaisePropertyChanged(OrdersPart.Prop_CustomerOrderNo, oldValue, value);
				}
			}

		}

		[Property("SaleQuan", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? SaleQuan
		{
			get { return _saleQuan; }
			set
			{
				if (value != _saleQuan)
				{
                    object oldValue = _saleQuan;
					_saleQuan = value;
					RaisePropertyChanged(OrdersPart.Prop_SaleQuan, oldValue, value);
				}
			}

		}

		#endregion
	} // OrdersPart
}

