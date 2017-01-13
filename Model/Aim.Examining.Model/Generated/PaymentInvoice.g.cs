// Business class PaymentInvoice generated from PaymentInvoice
// Creator: Ray
// Created Date: [2014-05-11]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PaymentInvoice")]
	public partial class PaymentInvoice : ExamModelBase<PaymentInvoice>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_BillType = "BillType";
		public static string Prop_CorrespondState = "CorrespondState";
		public static string Prop_CorrespondInvoice = "CorrespondInvoice";
		public static string Prop_Name = "Name";
		public static string Prop_Money = "Money";
		public static string Prop_CId = "CId";
		public static string Prop_CName = "CName";
		public static string Prop_CollectionType = "CollectionType";
		public static string Prop_PayType = "PayType";
		public static string Prop_ReceivablesTime = "ReceivablesTime";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";
		public static string Prop_CorrespondAmount = "CorrespondAmount";

		#endregion

		#region Private_Variables

		private string _id;
		private string _billType;
		private string _correspondState;
		private string _correspondInvoice;
		private string _name;
		private System.Decimal? _money;
		private string _cId;
		private string _cName;
		private string _collectionType;
		private string _payType;
		private DateTime? _receivablesTime;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;
		private System.Decimal? _correspondAmount;


		#endregion

		#region Constructors

		public PaymentInvoice()
		{
		}

		public PaymentInvoice(
			string p_id,
			string p_billType,
			string p_correspondState,
			string p_correspondInvoice,
			string p_name,
			System.Decimal? p_money,
			string p_cId,
			string p_cName,
			string p_collectionType,
			string p_payType,
			DateTime? p_receivablesTime,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark,
			System.Decimal? p_correspondAmount)
		{
			_id = p_id;
			_billType = p_billType;
			_correspondState = p_correspondState;
			_correspondInvoice = p_correspondInvoice;
			_name = p_name;
			_money = p_money;
			_cId = p_cId;
			_cName = p_cName;
			_collectionType = p_collectionType;
			_payType = p_payType;
			_receivablesTime = p_receivablesTime;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_remark = p_remark;
			_correspondAmount = p_correspondAmount;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("BillType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string BillType
		{
			get { return _billType; }
			set
			{
				if ((_billType == null) || (value == null) || (!value.Equals(_billType)))
				{
                    object oldValue = _billType;
					_billType = value;
					RaisePropertyChanged(PaymentInvoice.Prop_BillType, oldValue, value);
				}
			}

		}

		[Property("CorrespondState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CorrespondState
		{
			get { return _correspondState; }
			set
			{
				if ((_correspondState == null) || (value == null) || (!value.Equals(_correspondState)))
				{
                    object oldValue = _correspondState;
					_correspondState = value;
					RaisePropertyChanged(PaymentInvoice.Prop_CorrespondState, oldValue, value);
				}
			}

		}

		[Property("CorrespondInvoice", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 4000)]
		public string CorrespondInvoice
		{
			get { return _correspondInvoice; }
			set
			{
				if ((_correspondInvoice == null) || (value == null) || (!value.Equals(_correspondInvoice)))
				{
                    object oldValue = _correspondInvoice;
					_correspondInvoice = value;
					RaisePropertyChanged(PaymentInvoice.Prop_CorrespondInvoice, oldValue, value);
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
					RaisePropertyChanged(PaymentInvoice.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Money", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Money
		{
			get { return _money; }
			set
			{
				if (value != _money)
				{
                    object oldValue = _money;
					_money = value;
					RaisePropertyChanged(PaymentInvoice.Prop_Money, oldValue, value);
				}
			}

		}

		[Property("CId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CId
		{
			get { return _cId; }
			set
			{
				if ((_cId == null) || (value == null) || (!value.Equals(_cId)))
				{
                    object oldValue = _cId;
					_cId = value;
					RaisePropertyChanged(PaymentInvoice.Prop_CId, oldValue, value);
				}
			}

		}

		[Property("CName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string CName
		{
			get { return _cName; }
			set
			{
				if ((_cName == null) || (value == null) || (!value.Equals(_cName)))
				{
                    object oldValue = _cName;
					_cName = value;
					RaisePropertyChanged(PaymentInvoice.Prop_CName, oldValue, value);
				}
			}

		}

		[Property("CollectionType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CollectionType
		{
			get { return _collectionType; }
			set
			{
				if ((_collectionType == null) || (value == null) || (!value.Equals(_collectionType)))
				{
                    object oldValue = _collectionType;
					_collectionType = value;
					RaisePropertyChanged(PaymentInvoice.Prop_CollectionType, oldValue, value);
				}
			}

		}

		[Property("PayType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PayType
		{
			get { return _payType; }
			set
			{
				if ((_payType == null) || (value == null) || (!value.Equals(_payType)))
				{
                    object oldValue = _payType;
					_payType = value;
					RaisePropertyChanged(PaymentInvoice.Prop_PayType, oldValue, value);
				}
			}

		}

		[Property("ReceivablesTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? ReceivablesTime
		{
			get { return _receivablesTime; }
			set
			{
				if (value != _receivablesTime)
				{
                    object oldValue = _receivablesTime;
					_receivablesTime = value;
					RaisePropertyChanged(PaymentInvoice.Prop_ReceivablesTime, oldValue, value);
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
					RaisePropertyChanged(PaymentInvoice.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(PaymentInvoice.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(PaymentInvoice.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(PaymentInvoice.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("CorrespondAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? CorrespondAmount
		{
			get { return _correspondAmount; }
			set
			{
				if (value != _correspondAmount)
				{
                    object oldValue = _correspondAmount;
					_correspondAmount = value;
					RaisePropertyChanged(PaymentInvoice.Prop_CorrespondAmount, oldValue, value);
				}
			}

		}

		#endregion
	} // PaymentInvoice
}

