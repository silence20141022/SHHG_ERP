// Business class Product generated from Products
// Creator: Ray
// Created Date: [2013-05-25]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Products")]
	public partial class Product : ExamModelBase<Product>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Isbn = "Isbn";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_BuyPrice = "BuyPrice";
		public static string Prop_SalePrice = "SalePrice";
		public static string Prop_MinSalePrice = "MinSalePrice";
		public static string Prop_MinCount = "MinCount";
		public static string Prop_WarnInterval = "WarnInterval";
		public static string Prop_BackCash = "BackCash";
		public static string Prop_Pcn = "Pcn";
		public static string Prop_MakeArea = "MakeArea";
		public static string Prop_ProMsgId = "ProMsgId";
		public static string Prop_ProMsg = "ProMsg";
		public static string Prop_Weight = "Weight";
		public static string Prop_IsProxy = "IsProxy";
		public static string Prop_Unit = "Unit";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_ProductType = "ProductType";
		public static string Prop_WarnTime = "WarnTime";
		public static string Prop_FirstSkinIsbn = "FirstSkinIsbn";
		public static string Prop_FirstSkinCapacity = "FirstSkinCapacity";
		public static string Prop_SecondSkinIsbn = "SecondSkinIsbn";
		public static string Prop_SecondSkinCapacity = "SecondSkinCapacity";
		public static string Prop_CostPrice = "CostPrice";

		#endregion

		#region Private_Variables

		private string _id;
		private string _code;
		private string _name;
		private string _isbn;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private System.Decimal? _buyPrice;
		private System.Decimal? _salePrice;
		private System.Decimal? _minSalePrice;
		private int? _minCount;
		private int? _warnInterval;
		private System.Decimal? _backCash;
		private string _pcn;
		private string _makeArea;
		private string _proMsgId;
		private string _proMsg;
		private decimal? _weight;
		private string _isProxy;
		private string _unit;
		private string _supplierId;
		private string _supplierName;
		private string _productType;
		private DateTime? _warnTime;
		private string _firstSkinIsbn;
		private int? _firstSkinCapacity;
		private string _secondSkinIsbn;
		private int? _secondSkinCapacity;
		private System.Decimal? _costPrice;


		#endregion

		#region Constructors

		public Product()
		{
		}

		public Product(
			string p_id,
			string p_code,
			string p_name,
			string p_isbn,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			System.Decimal? p_buyPrice,
			System.Decimal? p_salePrice,
			System.Decimal? p_minSalePrice,
			int? p_minCount,
			int? p_warnInterval,
			System.Decimal? p_backCash,
			string p_pcn,
			string p_makeArea,
			string p_proMsgId,
			string p_proMsg,
			int? p_weight,
			string p_isProxy,
			string p_unit,
			string p_supplierId,
			string p_supplierName,
			string p_productType,
			DateTime? p_warnTime,
			string p_firstSkinIsbn,
			int? p_firstSkinCapacity,
			string p_secondSkinIsbn,
			int? p_secondSkinCapacity,
			System.Decimal? p_costPrice)
		{
			_id = p_id;
			_code = p_code;
			_name = p_name;
			_isbn = p_isbn;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_buyPrice = p_buyPrice;
			_salePrice = p_salePrice;
			_minSalePrice = p_minSalePrice;
			_minCount = p_minCount;
			_warnInterval = p_warnInterval;
			_backCash = p_backCash;
			_pcn = p_pcn;
			_makeArea = p_makeArea;
			_proMsgId = p_proMsgId;
			_proMsg = p_proMsg;
			_weight = p_weight;
			_isProxy = p_isProxy;
			_unit = p_unit;
			_supplierId = p_supplierId;
			_supplierName = p_supplierName;
			_productType = p_productType;
			_warnTime = p_warnTime;
			_firstSkinIsbn = p_firstSkinIsbn;
			_firstSkinCapacity = p_firstSkinCapacity;
			_secondSkinIsbn = p_secondSkinIsbn;
			_secondSkinCapacity = p_secondSkinCapacity;
			_costPrice = p_costPrice;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(Product.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(Product.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Isbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Isbn
		{
			get { return _isbn; }
			set
			{
				if ((_isbn == null) || (value == null) || (!value.Equals(_isbn)))
				{
                    object oldValue = _isbn;
					_isbn = value;
					RaisePropertyChanged(Product.Prop_Isbn, oldValue, value);
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
					RaisePropertyChanged(Product.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(Product.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Product.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Product.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("BuyPrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? BuyPrice
		{
			get { return _buyPrice; }
			set
			{
				if (value != _buyPrice)
				{
                    object oldValue = _buyPrice;
					_buyPrice = value;
					RaisePropertyChanged(Product.Prop_BuyPrice, oldValue, value);
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
					RaisePropertyChanged(Product.Prop_SalePrice, oldValue, value);
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
					RaisePropertyChanged(Product.Prop_MinSalePrice, oldValue, value);
				}
			}

		}

		[Property("MinCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? MinCount
		{
			get { return _minCount; }
			set
			{
				if (value != _minCount)
				{
                    object oldValue = _minCount;
					_minCount = value;
					RaisePropertyChanged(Product.Prop_MinCount, oldValue, value);
				}
			}

		}

		[Property("WarnInterval", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? WarnInterval
		{
			get { return _warnInterval; }
			set
			{
				if (value != _warnInterval)
				{
                    object oldValue = _warnInterval;
					_warnInterval = value;
					RaisePropertyChanged(Product.Prop_WarnInterval, oldValue, value);
				}
			}

		}

		[Property("BackCash", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? BackCash
		{
			get { return _backCash; }
			set
			{
				if (value != _backCash)
				{
                    object oldValue = _backCash;
					_backCash = value;
					RaisePropertyChanged(Product.Prop_BackCash, oldValue, value);
				}
			}

		}

		[Property("Pcn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Pcn
		{
			get { return _pcn; }
			set
			{
				if ((_pcn == null) || (value == null) || (!value.Equals(_pcn)))
				{
                    object oldValue = _pcn;
					_pcn = value;
					RaisePropertyChanged(Product.Prop_Pcn, oldValue, value);
				}
			}

		}

		[Property("MakeArea", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string MakeArea
		{
			get { return _makeArea; }
			set
			{
				if ((_makeArea == null) || (value == null) || (!value.Equals(_makeArea)))
				{
                    object oldValue = _makeArea;
					_makeArea = value;
					RaisePropertyChanged(Product.Prop_MakeArea, oldValue, value);
				}
			}

		}

		[Property("ProMsgId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ProMsgId
		{
			get { return _proMsgId; }
			set
			{
				if ((_proMsgId == null) || (value == null) || (!value.Equals(_proMsgId)))
				{
                    object oldValue = _proMsgId;
					_proMsgId = value;
					RaisePropertyChanged(Product.Prop_ProMsgId, oldValue, value);
				}
			}

		}

		[Property("ProMsg", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string ProMsg
		{
			get { return _proMsg; }
			set
			{
				if ((_proMsg == null) || (value == null) || (!value.Equals(_proMsg)))
				{
                    object oldValue = _proMsg;
					_proMsg = value;
					RaisePropertyChanged(Product.Prop_ProMsg, oldValue, value);
				}
			}

		}

		[Property("Weight", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public decimal? Weight
		{
			get { return _weight; }
			set
			{
				if (value != _weight)
				{
                    object oldValue = _weight;
					_weight = value;
					RaisePropertyChanged(Product.Prop_Weight, oldValue, value);
				}
			}

		}

		[Property("IsProxy", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string IsProxy
		{
			get { return _isProxy; }
			set
			{
				if ((_isProxy == null) || (value == null) || (!value.Equals(_isProxy)))
				{
                    object oldValue = _isProxy;
					_isProxy = value;
					RaisePropertyChanged(Product.Prop_IsProxy, oldValue, value);
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
					RaisePropertyChanged(Product.Prop_Unit, oldValue, value);
				}
			}

		}

		[Property("SupplierId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string SupplierId
		{
			get { return _supplierId; }
			set
			{
				if ((_supplierId == null) || (value == null) || (!value.Equals(_supplierId)))
				{
                    object oldValue = _supplierId;
					_supplierId = value;
					RaisePropertyChanged(Product.Prop_SupplierId, oldValue, value);
				}
			}

		}

		[Property("SupplierName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string SupplierName
		{
			get { return _supplierName; }
			set
			{
				if ((_supplierName == null) || (value == null) || (!value.Equals(_supplierName)))
				{
                    object oldValue = _supplierName;
					_supplierName = value;
					RaisePropertyChanged(Product.Prop_SupplierName, oldValue, value);
				}
			}

		}

		[Property("ProductType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ProductType
		{
			get { return _productType; }
			set
			{
				if ((_productType == null) || (value == null) || (!value.Equals(_productType)))
				{
                    object oldValue = _productType;
					_productType = value;
					RaisePropertyChanged(Product.Prop_ProductType, oldValue, value);
				}
			}

		}

		[Property("WarnTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? WarnTime
		{
			get { return _warnTime; }
			set
			{
				if (value != _warnTime)
				{
                    object oldValue = _warnTime;
					_warnTime = value;
					RaisePropertyChanged(Product.Prop_WarnTime, oldValue, value);
				}
			}

		}

		[Property("FirstSkinIsbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string FirstSkinIsbn
		{
			get { return _firstSkinIsbn; }
			set
			{
				if ((_firstSkinIsbn == null) || (value == null) || (!value.Equals(_firstSkinIsbn)))
				{
                    object oldValue = _firstSkinIsbn;
					_firstSkinIsbn = value;
					RaisePropertyChanged(Product.Prop_FirstSkinIsbn, oldValue, value);
				}
			}

		}

		[Property("FirstSkinCapacity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? FirstSkinCapacity
		{
			get { return _firstSkinCapacity; }
			set
			{
				if (value != _firstSkinCapacity)
				{
                    object oldValue = _firstSkinCapacity;
					_firstSkinCapacity = value;
					RaisePropertyChanged(Product.Prop_FirstSkinCapacity, oldValue, value);
				}
			}

		}

		[Property("SecondSkinIsbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SecondSkinIsbn
		{
			get { return _secondSkinIsbn; }
			set
			{
				if ((_secondSkinIsbn == null) || (value == null) || (!value.Equals(_secondSkinIsbn)))
				{
                    object oldValue = _secondSkinIsbn;
					_secondSkinIsbn = value;
					RaisePropertyChanged(Product.Prop_SecondSkinIsbn, oldValue, value);
				}
			}

		}

		[Property("SecondSkinCapacity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? SecondSkinCapacity
		{
			get { return _secondSkinCapacity; }
			set
			{
				if (value != _secondSkinCapacity)
				{
                    object oldValue = _secondSkinCapacity;
					_secondSkinCapacity = value;
					RaisePropertyChanged(Product.Prop_SecondSkinCapacity, oldValue, value);
				}
			}

		}

		[Property("CostPrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? CostPrice
		{
			get { return _costPrice; }
			set
			{
				if (value != _costPrice)
				{
                    object oldValue = _costPrice;
					_costPrice = value;
					RaisePropertyChanged(Product.Prop_CostPrice, oldValue, value);
				}
			}

		}

		#endregion
	} // Product
}

