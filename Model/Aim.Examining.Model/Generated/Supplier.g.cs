// Business class Supplier generated from Supplier
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
	[ActiveRecord("Supplier")]
	public partial class Supplier : ExamModelBase<Supplier>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_SupplierAddress = "SupplierAddress";
		public static string Prop_FixedTelephone = "FixedTelephone";
		public static string Prop_Mobile = "Mobile";
		public static string Prop_Fax = "Fax";
		public static string Prop_ContactPerson = "ContactPerson";
		public static string Prop_Email = "Email";
		public static string Prop_IsDefault = "IsDefault";
		public static string Prop_Bank = "Bank";
		public static string Prop_Account = "Account";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_MailAddress = "MailAddress";
		public static string Prop_MoneyType = "MoneyType";
		public static string Prop_Remark = "Remark";
		public static string Prop_ExchangeRateId = "ExchangeRateId";
		public static string Prop_Symbo = "Symbo";

		#endregion

		#region Private_Variables

		private string _id;
		private string _supplierName;
		private string _supplierAddress;
		private string _fixedTelephone;
		private string _mobile;
		private string _fax;
		private string _contactPerson;
		private string _email;
		private string _isDefault;
		private string _bank;
		private string _account;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _mailAddress;
		private string _moneyType;
		private string _remark;
		private string _exchangeRateId;
		private string _symbo;


		#endregion

		#region Constructors

		public Supplier()
		{
		}

		public Supplier(
			string p_id,
			string p_supplierName,
			string p_supplierAddress,
			string p_fixedTelephone,
			string p_mobile,
			string p_fax,
			string p_contactPerson,
			string p_email,
			string p_isDefault,
			string p_bank,
			string p_account,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_mailAddress,
			string p_moneyType,
			string p_remark,
			string p_exchangeRateId,
			string p_symbo)
		{
			_id = p_id;
			_supplierName = p_supplierName;
			_supplierAddress = p_supplierAddress;
			_fixedTelephone = p_fixedTelephone;
			_mobile = p_mobile;
			_fax = p_fax;
			_contactPerson = p_contactPerson;
			_email = p_email;
			_isDefault = p_isDefault;
			_bank = p_bank;
			_account = p_account;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_mailAddress = p_mailAddress;
			_moneyType = p_moneyType;
			_remark = p_remark;
			_exchangeRateId = p_exchangeRateId;
			_symbo = p_symbo;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("SupplierName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string SupplierName
		{
			get { return _supplierName; }
			set
			{
				if ((_supplierName == null) || (value == null) || (!value.Equals(_supplierName)))
				{
                    object oldValue = _supplierName;
					_supplierName = value;
					RaisePropertyChanged(Supplier.Prop_SupplierName, oldValue, value);
				}
			}

		}

		[Property("SupplierAddress", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string SupplierAddress
		{
			get { return _supplierAddress; }
			set
			{
				if ((_supplierAddress == null) || (value == null) || (!value.Equals(_supplierAddress)))
				{
                    object oldValue = _supplierAddress;
					_supplierAddress = value;
					RaisePropertyChanged(Supplier.Prop_SupplierAddress, oldValue, value);
				}
			}

		}

		[Property("FixedTelephone", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string FixedTelephone
		{
			get { return _fixedTelephone; }
			set
			{
				if ((_fixedTelephone == null) || (value == null) || (!value.Equals(_fixedTelephone)))
				{
                    object oldValue = _fixedTelephone;
					_fixedTelephone = value;
					RaisePropertyChanged(Supplier.Prop_FixedTelephone, oldValue, value);
				}
			}

		}

		[Property("Mobile", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Mobile
		{
			get { return _mobile; }
			set
			{
				if ((_mobile == null) || (value == null) || (!value.Equals(_mobile)))
				{
                    object oldValue = _mobile;
					_mobile = value;
					RaisePropertyChanged(Supplier.Prop_Mobile, oldValue, value);
				}
			}

		}

		[Property("Fax", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Fax
		{
			get { return _fax; }
			set
			{
				if ((_fax == null) || (value == null) || (!value.Equals(_fax)))
				{
                    object oldValue = _fax;
					_fax = value;
					RaisePropertyChanged(Supplier.Prop_Fax, oldValue, value);
				}
			}

		}

		[Property("ContactPerson", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ContactPerson
		{
			get { return _contactPerson; }
			set
			{
				if ((_contactPerson == null) || (value == null) || (!value.Equals(_contactPerson)))
				{
                    object oldValue = _contactPerson;
					_contactPerson = value;
					RaisePropertyChanged(Supplier.Prop_ContactPerson, oldValue, value);
				}
			}

		}

		[Property("Email", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Email
		{
			get { return _email; }
			set
			{
				if ((_email == null) || (value == null) || (!value.Equals(_email)))
				{
                    object oldValue = _email;
					_email = value;
					RaisePropertyChanged(Supplier.Prop_Email, oldValue, value);
				}
			}

		}

		[Property("IsDefault", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string IsDefault
		{
			get { return _isDefault; }
			set
			{
				if ((_isDefault == null) || (value == null) || (!value.Equals(_isDefault)))
				{
                    object oldValue = _isDefault;
					_isDefault = value;
					RaisePropertyChanged(Supplier.Prop_IsDefault, oldValue, value);
				}
			}

		}

		[Property("Bank", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Bank
		{
			get { return _bank; }
			set
			{
				if ((_bank == null) || (value == null) || (!value.Equals(_bank)))
				{
                    object oldValue = _bank;
					_bank = value;
					RaisePropertyChanged(Supplier.Prop_Bank, oldValue, value);
				}
			}

		}

		[Property("Account", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Account
		{
			get { return _account; }
			set
			{
				if ((_account == null) || (value == null) || (!value.Equals(_account)))
				{
                    object oldValue = _account;
					_account = value;
					RaisePropertyChanged(Supplier.Prop_Account, oldValue, value);
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
					RaisePropertyChanged(Supplier.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Supplier.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Supplier.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("MailAddress", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string MailAddress
		{
			get { return _mailAddress; }
			set
			{
				if ((_mailAddress == null) || (value == null) || (!value.Equals(_mailAddress)))
				{
                    object oldValue = _mailAddress;
					_mailAddress = value;
					RaisePropertyChanged(Supplier.Prop_MailAddress, oldValue, value);
				}
			}

		}

		[Property("MoneyType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string MoneyType
		{
			get { return _moneyType; }
			set
			{
				if ((_moneyType == null) || (value == null) || (!value.Equals(_moneyType)))
				{
                    object oldValue = _moneyType;
					_moneyType = value;
					RaisePropertyChanged(Supplier.Prop_MoneyType, oldValue, value);
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
					RaisePropertyChanged(Supplier.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("ExchangeRateId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ExchangeRateId
		{
			get { return _exchangeRateId; }
			set
			{
				if ((_exchangeRateId == null) || (value == null) || (!value.Equals(_exchangeRateId)))
				{
                    object oldValue = _exchangeRateId;
					_exchangeRateId = value;
					RaisePropertyChanged(Supplier.Prop_ExchangeRateId, oldValue, value);
				}
			}

		}

		[Property("Symbo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Symbo
		{
			get { return _symbo; }
			set
			{
				if ((_symbo == null) || (value == null) || (!value.Equals(_symbo)))
				{
                    object oldValue = _symbo;
					_symbo = value;
					RaisePropertyChanged(Supplier.Prop_Symbo, oldValue, value);
				}
			}

		}

		#endregion
	} // Supplier
}

