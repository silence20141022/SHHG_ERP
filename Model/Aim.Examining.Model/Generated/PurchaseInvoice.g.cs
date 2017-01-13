// Business class PurchaseInvoice generated from PurchaseInvoice
// Creator: Ray
// Created Date: [2015-02-03]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PurchaseInvoice")]
	public partial class PurchaseInvoice : ExamModelBase<PurchaseInvoice>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_InvoiceNo = "InvoiceNo";
		public static string Prop_InvoiceAmount = "InvoiceAmount";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_Symbo = "Symbo";
		public static string Prop_State = "State";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _invoiceNo;
		private System.Decimal? _invoiceAmount;
		private string _supplierId;
		private string _supplierName;
		private string _symbo;
		private string _state;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;


		#endregion

		#region Constructors

		public PurchaseInvoice()
		{
		}

		public PurchaseInvoice(
			string p_id,
			string p_invoiceNo,
			System.Decimal? p_invoiceAmount,
			string p_supplierId,
			string p_supplierName,
			string p_symbo,
			string p_state,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark)
		{
			_id = p_id;
			_invoiceNo = p_invoiceNo;
			_invoiceAmount = p_invoiceAmount;
			_supplierId = p_supplierId;
			_supplierName = p_supplierName;
			_symbo = p_symbo;
			_state = p_state;
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
					RaisePropertyChanged(PurchaseInvoice.Prop_InvoiceNo, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoice.Prop_InvoiceAmount, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoice.Prop_SupplierId, oldValue, value);
				}
			}

		}

		[Property("SupplierName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SupplierName
		{
			get { return _supplierName; }
			set
			{
				if ((_supplierName == null) || (value == null) || (!value.Equals(_supplierName)))
				{
                    object oldValue = _supplierName;
					_supplierName = value;
					RaisePropertyChanged(PurchaseInvoice.Prop_SupplierName, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoice.Prop_Symbo, oldValue, value);
				}
			}

		}

		[Property("State", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string State
		{
			get { return _state; }
			set
			{
				if ((_state == null) || (value == null) || (!value.Equals(_state)))
				{
                    object oldValue = _state;
					_state = value;
					RaisePropertyChanged(PurchaseInvoice.Prop_State, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoice.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoice.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoice.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(PurchaseInvoice.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // PurchaseInvoice
}

