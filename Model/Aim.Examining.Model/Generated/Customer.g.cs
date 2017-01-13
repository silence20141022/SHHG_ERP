// Business class Customer generated from Customers
// Creator: Ray
// Created Date: [2013-05-01]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Customers")]
	public partial class Customer : ExamModelBase<Customer>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Code = "Code";
		public static string Prop_SimpleName = "SimpleName";
		public static string Prop_Name = "Name";
		public static string Prop_Bank = "Bank";
		public static string Prop_AccountNum = "AccountNum";
		public static string Prop_AccountName = "AccountName";
		public static string Prop_TariffNum = "TariffNum";
		public static string Prop_Province = "Province";
		public static string Prop_UnitType = "UnitType";
		public static string Prop_Contact = "Contact";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_City = "City";
		public static string Prop_Address = "Address";
		public static string Prop_ZipCode = "ZipCode";
		public static string Prop_Level = "Level";
		public static string Prop_Importance = "Importance";
		public static string Prop_Website = "Website";
		public static string Prop_OpenTime = "OpenTime";
		public static string Prop_CreditAmount = "CreditAmount";
		public static string Prop_AccountValidity = "AccountValidity";
		public static string Prop_WarnDays = "WarnDays";
		public static string Prop_BuyPrice = "BuyPrice";
		public static string Prop_SalePrice = "SalePrice";
		public static string Prop_MinSalePrice = "MinSalePrice";
		public static string Prop_MinCount = "MinCount";
		public static string Prop_WarnInterval = "WarnInterval";
		public static string Prop_BackCash = "BackCash";
		public static string Prop_PreDeposit = "PreDeposit";
		public static string Prop_MagId = "MagId";
		public static string Prop_MagUser = "MagUser";
		public static string Prop_Attachment = "Attachment";
		public static string Prop_Tel = "Tel";
		public static string Prop_PreInvoice = "PreInvoice";

		#endregion

		#region Private_Variables

		private string _id;
		private string _code;
		private string _simpleName;
		private string _name;
		private string _bank;
		private string _accountNum;
		private string _accountName;
		private string _tariffNum;
		private string _province;
		private string _unitType;
		private string _contact;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _city;
		private string _address;
		private string _zipCode;
		private string _level;
		private string _importance;
		private string _website;
		private DateTime? _openTime;
		private int? _creditAmount;
		private int? _accountValidity;
		private int? _warnDays;
		private System.Decimal? _buyPrice;
		private System.Decimal? _salePrice;
		private System.Decimal? _minSalePrice;
		private int? _minCount;
		private int? _warnInterval;
		private System.Decimal? _backCash;
		private System.Decimal? _preDeposit;
		private string _magId;
		private string _magUser;
		private string _attachment;
		private string _tel;
		private System.Decimal? _preInvoice;


		#endregion

		#region Constructors

		public Customer()
		{
		}

		public Customer(
			string p_id,
			string p_code,
			string p_simpleName,
			string p_name,
			string p_bank,
			string p_accountNum,
			string p_accountName,
			string p_tariffNum,
			string p_province,
			string p_unitType,
			string p_contact,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_city,
			string p_address,
			string p_zipCode,
			string p_level,
			string p_importance,
			string p_website,
			DateTime? p_openTime,
			int? p_creditAmount,
			int? p_accountValidity,
			int? p_warnDays,
			System.Decimal? p_buyPrice,
			System.Decimal? p_salePrice,
			System.Decimal? p_minSalePrice,
			int? p_minCount,
			int? p_warnInterval,
			System.Decimal? p_backCash,
			System.Decimal? p_preDeposit,
			string p_magId,
			string p_magUser,
			string p_attachment,
			string p_tel,
			System.Decimal? p_preInvoice)
		{
			_id = p_id;
			_code = p_code;
			_simpleName = p_simpleName;
			_name = p_name;
			_bank = p_bank;
			_accountNum = p_accountNum;
			_accountName = p_accountName;
			_tariffNum = p_tariffNum;
			_province = p_province;
			_unitType = p_unitType;
			_contact = p_contact;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_city = p_city;
			_address = p_address;
			_zipCode = p_zipCode;
			_level = p_level;
			_importance = p_importance;
			_website = p_website;
			_openTime = p_openTime;
			_creditAmount = p_creditAmount;
			_accountValidity = p_accountValidity;
			_warnDays = p_warnDays;
			_buyPrice = p_buyPrice;
			_salePrice = p_salePrice;
			_minSalePrice = p_minSalePrice;
			_minCount = p_minCount;
			_warnInterval = p_warnInterval;
			_backCash = p_backCash;
			_preDeposit = p_preDeposit;
			_magId = p_magId;
			_magUser = p_magUser;
			_attachment = p_attachment;
			_tel = p_tel;
			_preInvoice = p_preInvoice;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(Customer.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("SimpleName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string SimpleName
		{
			get { return _simpleName; }
			set
			{
				if ((_simpleName == null) || (value == null) || (!value.Equals(_simpleName)))
				{
                    object oldValue = _simpleName;
					_simpleName = value;
					RaisePropertyChanged(Customer.Prop_SimpleName, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Bank", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Bank
		{
			get { return _bank; }
			set
			{
				if ((_bank == null) || (value == null) || (!value.Equals(_bank)))
				{
                    object oldValue = _bank;
					_bank = value;
					RaisePropertyChanged(Customer.Prop_Bank, oldValue, value);
				}
			}

		}

		[Property("AccountNum", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 30)]
		public string AccountNum
		{
			get { return _accountNum; }
			set
			{
				if ((_accountNum == null) || (value == null) || (!value.Equals(_accountNum)))
				{
                    object oldValue = _accountNum;
					_accountNum = value;
					RaisePropertyChanged(Customer.Prop_AccountNum, oldValue, value);
				}
			}

		}

		[Property("AccountName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string AccountName
		{
			get { return _accountName; }
			set
			{
				if ((_accountName == null) || (value == null) || (!value.Equals(_accountName)))
				{
                    object oldValue = _accountName;
					_accountName = value;
					RaisePropertyChanged(Customer.Prop_AccountName, oldValue, value);
				}
			}

		}

		[Property("TariffNum", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string TariffNum
		{
			get { return _tariffNum; }
			set
			{
				if ((_tariffNum == null) || (value == null) || (!value.Equals(_tariffNum)))
				{
                    object oldValue = _tariffNum;
					_tariffNum = value;
					RaisePropertyChanged(Customer.Prop_TariffNum, oldValue, value);
				}
			}

		}

		[Property("Province", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Province
		{
			get { return _province; }
			set
			{
				if ((_province == null) || (value == null) || (!value.Equals(_province)))
				{
                    object oldValue = _province;
					_province = value;
					RaisePropertyChanged(Customer.Prop_Province, oldValue, value);
				}
			}

		}

		[Property("UnitType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string UnitType
		{
			get { return _unitType; }
			set
			{
				if ((_unitType == null) || (value == null) || (!value.Equals(_unitType)))
				{
                    object oldValue = _unitType;
					_unitType = value;
					RaisePropertyChanged(Customer.Prop_UnitType, oldValue, value);
				}
			}

		}

		[Property("Contact", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Contact
		{
			get { return _contact; }
			set
			{
				if ((_contact == null) || (value == null) || (!value.Equals(_contact)))
				{
                    object oldValue = _contact;
					_contact = value;
					RaisePropertyChanged(Customer.Prop_Contact, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 3000)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(Customer.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("City", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string City
		{
			get { return _city; }
			set
			{
				if ((_city == null) || (value == null) || (!value.Equals(_city)))
				{
                    object oldValue = _city;
					_city = value;
					RaisePropertyChanged(Customer.Prop_City, oldValue, value);
				}
			}

		}

		[Property("Address", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Address
		{
			get { return _address; }
			set
			{
				if ((_address == null) || (value == null) || (!value.Equals(_address)))
				{
                    object oldValue = _address;
					_address = value;
					RaisePropertyChanged(Customer.Prop_Address, oldValue, value);
				}
			}

		}

		[Property("ZipCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 6)]
		public string ZipCode
		{
			get { return _zipCode; }
			set
			{
				if ((_zipCode == null) || (value == null) || (!value.Equals(_zipCode)))
				{
                    object oldValue = _zipCode;
					_zipCode = value;
					RaisePropertyChanged(Customer.Prop_ZipCode, oldValue, value);
				}
			}

		}

		[Property("Level", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Level
		{
			get { return _level; }
			set
			{
				if ((_level == null) || (value == null) || (!value.Equals(_level)))
				{
                    object oldValue = _level;
					_level = value;
					RaisePropertyChanged(Customer.Prop_Level, oldValue, value);
				}
			}

		}

		[Property("Importance", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Importance
		{
			get { return _importance; }
			set
			{
				if ((_importance == null) || (value == null) || (!value.Equals(_importance)))
				{
                    object oldValue = _importance;
					_importance = value;
					RaisePropertyChanged(Customer.Prop_Importance, oldValue, value);
				}
			}

		}

		[Property("Website", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Website
		{
			get { return _website; }
			set
			{
				if ((_website == null) || (value == null) || (!value.Equals(_website)))
				{
                    object oldValue = _website;
					_website = value;
					RaisePropertyChanged(Customer.Prop_Website, oldValue, value);
				}
			}

		}

		[Property("OpenTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? OpenTime
		{
			get { return _openTime; }
			set
			{
				if (value != _openTime)
				{
                    object oldValue = _openTime;
					_openTime = value;
					RaisePropertyChanged(Customer.Prop_OpenTime, oldValue, value);
				}
			}

		}

		[Property("CreditAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? CreditAmount
		{
			get { return _creditAmount; }
			set
			{
				if (value != _creditAmount)
				{
                    object oldValue = _creditAmount;
					_creditAmount = value;
					RaisePropertyChanged(Customer.Prop_CreditAmount, oldValue, value);
				}
			}

		}

		[Property("AccountValidity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? AccountValidity
		{
			get { return _accountValidity; }
			set
			{
				if (value != _accountValidity)
				{
                    object oldValue = _accountValidity;
					_accountValidity = value;
					RaisePropertyChanged(Customer.Prop_AccountValidity, oldValue, value);
				}
			}

		}

		[Property("WarnDays", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? WarnDays
		{
			get { return _warnDays; }
			set
			{
				if (value != _warnDays)
				{
                    object oldValue = _warnDays;
					_warnDays = value;
					RaisePropertyChanged(Customer.Prop_WarnDays, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_BuyPrice, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_SalePrice, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_MinSalePrice, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_MinCount, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_WarnInterval, oldValue, value);
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
					RaisePropertyChanged(Customer.Prop_BackCash, oldValue, value);
				}
			}

		}

		[Property("PreDeposit", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PreDeposit
		{
			get { return _preDeposit; }
			set
			{
				if (value != _preDeposit)
				{
                    object oldValue = _preDeposit;
					_preDeposit = value;
					RaisePropertyChanged(Customer.Prop_PreDeposit, oldValue, value);
				}
			}

		}

		[Property("MagId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string MagId
		{
			get { return _magId; }
			set
			{
				if ((_magId == null) || (value == null) || (!value.Equals(_magId)))
				{
                    object oldValue = _magId;
					_magId = value;
					RaisePropertyChanged(Customer.Prop_MagId, oldValue, value);
				}
			}

		}

		[Property("MagUser", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string MagUser
		{
			get { return _magUser; }
			set
			{
				if ((_magUser == null) || (value == null) || (!value.Equals(_magUser)))
				{
                    object oldValue = _magUser;
					_magUser = value;
					RaisePropertyChanged(Customer.Prop_MagUser, oldValue, value);
				}
			}

		}

		[Property("Attachment", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Attachment
		{
			get { return _attachment; }
			set
			{
				if ((_attachment == null) || (value == null) || (!value.Equals(_attachment)))
				{
                    object oldValue = _attachment;
					_attachment = value;
					RaisePropertyChanged(Customer.Prop_Attachment, oldValue, value);
				}
			}

		}

		[Property("Tel", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Tel
		{
			get { return _tel; }
			set
			{
				if ((_tel == null) || (value == null) || (!value.Equals(_tel)))
				{
                    object oldValue = _tel;
					_tel = value;
					RaisePropertyChanged(Customer.Prop_Tel, oldValue, value);
				}
			}

		}

		[Property("PreInvoice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PreInvoice
		{
			get { return _preInvoice; }
			set
			{
				if (value != _preInvoice)
				{
                    object oldValue = _preInvoice;
					_preInvoice = value;
					RaisePropertyChanged(Customer.Prop_PreInvoice, oldValue, value);
				}
			}

		}

		#endregion
	} // Customer
}

