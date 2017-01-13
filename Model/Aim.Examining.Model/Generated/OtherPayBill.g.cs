// Business class OtherPayBill generated from OtherPayBill
// Creator: Ray
// Created Date: [2012-04-26]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("OtherPayBill")]
	public partial class OtherPayBill : ExamModelBase<OtherPayBill>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PayBillNo = "PayBillNo";
		public static string Prop_PayState = "PayState";
		public static string Prop_PayType = "PayType";
		public static string Prop_LogisticsCompanyName = "LogisticsCompanyName";
		public static string Prop_ShouldPayAmount = "ShouldPayAmount";
		public static string Prop_AcctualPayAmount = "AcctualPayAmount";
		public static string Prop_InterfaceArray = "InterfaceArray";
		public static string Prop_ModifyUserId = "ModifyUserId";
		public static string Prop_InvoiceNo = "InvoiceNo";
		public static string Prop_InvoiceAmount = "InvoiceAmount";
		public static string Prop_PayUserId = "PayUserId";
		public static string Prop_PayUserName = "PayUserName";
		public static string Prop_PayTime = "PayTime";
		public static string Prop_ModifyTime = "ModifyTime";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _payBillNo;
		private string _payState;
		private string _payType;
		private string _logisticsCompanyName;
		private System.Decimal? _shouldPayAmount;
		private System.Decimal? _acctualPayAmount;
		private string _interfaceArray;
		private string _modifyUserId;
		private string _invoiceNo;
		private System.Decimal? _invoiceAmount;
		private string _payUserId;
		private string _payUserName;
		private DateTime? _payTime;
		private DateTime? _modifyTime;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public OtherPayBill()
		{
		}

		public OtherPayBill(
			string p_id,
			string p_payBillNo,
			string p_payState,
			string p_payType,
			string p_logisticsCompanyName,
			System.Decimal? p_shouldPayAmount,
			System.Decimal? p_acctualPayAmount,
			string p_interfaceArray,
			string p_modifyUserId,
			string p_invoiceNo,
			System.Decimal? p_invoiceAmount,
			string p_payUserId,
			string p_payUserName,
			DateTime? p_payTime,
			DateTime? p_modifyTime,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_payBillNo = p_payBillNo;
			_payState = p_payState;
			_payType = p_payType;
			_logisticsCompanyName = p_logisticsCompanyName;
			_shouldPayAmount = p_shouldPayAmount;
			_acctualPayAmount = p_acctualPayAmount;
			_interfaceArray = p_interfaceArray;
			_modifyUserId = p_modifyUserId;
			_invoiceNo = p_invoiceNo;
			_invoiceAmount = p_invoiceAmount;
			_payUserId = p_payUserId;
			_payUserName = p_payUserName;
			_payTime = p_payTime;
			_modifyTime = p_modifyTime;
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

		[Property("PayBillNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayBillNo
		{
			get { return _payBillNo; }
			set
			{
				if ((_payBillNo == null) || (value == null) || (!value.Equals(_payBillNo)))
				{
                    object oldValue = _payBillNo;
					_payBillNo = value;
					RaisePropertyChanged(OtherPayBill.Prop_PayBillNo, oldValue, value);
				}
			}

		}

		[Property("PayState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayState
		{
			get { return _payState; }
			set
			{
				if ((_payState == null) || (value == null) || (!value.Equals(_payState)))
				{
                    object oldValue = _payState;
					_payState = value;
					RaisePropertyChanged(OtherPayBill.Prop_PayState, oldValue, value);
				}
			}

		}

		[Property("PayType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayType
		{
			get { return _payType; }
			set
			{
				if ((_payType == null) || (value == null) || (!value.Equals(_payType)))
				{
                    object oldValue = _payType;
					_payType = value;
					RaisePropertyChanged(OtherPayBill.Prop_PayType, oldValue, value);
				}
			}

		}

		[Property("LogisticsCompanyName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string LogisticsCompanyName
		{
			get { return _logisticsCompanyName; }
			set
			{
				if ((_logisticsCompanyName == null) || (value == null) || (!value.Equals(_logisticsCompanyName)))
				{
                    object oldValue = _logisticsCompanyName;
					_logisticsCompanyName = value;
					RaisePropertyChanged(OtherPayBill.Prop_LogisticsCompanyName, oldValue, value);
				}
			}

		}

		[Property("ShouldPayAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ShouldPayAmount
		{
			get { return _shouldPayAmount; }
			set
			{
				if (value != _shouldPayAmount)
				{
                    object oldValue = _shouldPayAmount;
					_shouldPayAmount = value;
					RaisePropertyChanged(OtherPayBill.Prop_ShouldPayAmount, oldValue, value);
				}
			}

		}

		[Property("AcctualPayAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? AcctualPayAmount
		{
			get { return _acctualPayAmount; }
			set
			{
				if (value != _acctualPayAmount)
				{
                    object oldValue = _acctualPayAmount;
					_acctualPayAmount = value;
					RaisePropertyChanged(OtherPayBill.Prop_AcctualPayAmount, oldValue, value);
				}
			}

		}

		[Property("InterfaceArray", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string InterfaceArray
		{
			get { return _interfaceArray; }
			set
			{
				if ((_interfaceArray == null) || (value == null) || (!value.Equals(_interfaceArray)))
				{
                    object oldValue = _interfaceArray;
					_interfaceArray = value;
					RaisePropertyChanged(OtherPayBill.Prop_InterfaceArray, oldValue, value);
				}
			}

		}

		[Property("ModifyUserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ModifyUserId
		{
			get { return _modifyUserId; }
			set
			{
				if ((_modifyUserId == null) || (value == null) || (!value.Equals(_modifyUserId)))
				{
                    object oldValue = _modifyUserId;
					_modifyUserId = value;
					RaisePropertyChanged(OtherPayBill.Prop_ModifyUserId, oldValue, value);
				}
			}

		}

		[Property("InvoiceNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InvoiceNo
		{
			get { return _invoiceNo; }
			set
			{
				if ((_invoiceNo == null) || (value == null) || (!value.Equals(_invoiceNo)))
				{
                    object oldValue = _invoiceNo;
					_invoiceNo = value;
					RaisePropertyChanged(OtherPayBill.Prop_InvoiceNo, oldValue, value);
				}
			}

		}

		[Property("InvoiceAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? InvoiceAmount
		{
			get { return _invoiceAmount; }
			set
			{
				if (value != _invoiceAmount)
				{
                    object oldValue = _invoiceAmount;
					_invoiceAmount = value;
					RaisePropertyChanged(OtherPayBill.Prop_InvoiceAmount, oldValue, value);
				}
			}

		}

		[Property("PayUserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PayUserId
		{
			get { return _payUserId; }
			set
			{
				if ((_payUserId == null) || (value == null) || (!value.Equals(_payUserId)))
				{
                    object oldValue = _payUserId;
					_payUserId = value;
					RaisePropertyChanged(OtherPayBill.Prop_PayUserId, oldValue, value);
				}
			}

		}

		[Property("PayUserName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayUserName
		{
			get { return _payUserName; }
			set
			{
				if ((_payUserName == null) || (value == null) || (!value.Equals(_payUserName)))
				{
                    object oldValue = _payUserName;
					_payUserName = value;
					RaisePropertyChanged(OtherPayBill.Prop_PayUserName, oldValue, value);
				}
			}

		}

		[Property("PayTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? PayTime
		{
			get { return _payTime; }
			set
			{
				if (value != _payTime)
				{
                    object oldValue = _payTime;
					_payTime = value;
					RaisePropertyChanged(OtherPayBill.Prop_PayTime, oldValue, value);
				}
			}

		}

		[Property("ModifyTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? ModifyTime
		{
			get { return _modifyTime; }
			set
			{
				if (value != _modifyTime)
				{
                    object oldValue = _modifyTime;
					_modifyTime = value;
					RaisePropertyChanged(OtherPayBill.Prop_ModifyTime, oldValue, value);
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
					RaisePropertyChanged(OtherPayBill.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(OtherPayBill.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(OtherPayBill.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(OtherPayBill.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // OtherPayBill
}

