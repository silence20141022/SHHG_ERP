// Business class CorrespondBill generated from CorrespondBill
// Creator: Ray
// Created Date: [2013-03-24]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("CorrespondBill")]
	public partial class CorrespondBill : ExamModelBase<CorrespondBill>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_CorrespondBillNo = "CorrespondBillNo";
		public static string Prop_DeliveryOrderId = "DeliveryOrderId";
		public static string Prop_CustomerId = "CustomerId";
		public static string Prop_CustomerName = "CustomerName";
		public static string Prop_InWarehouseId = "InWarehouseId";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_BorrowAmount = "BorrowAmount";
		public static string Prop_PayAmount = "PayAmount";
		public static string Prop_Arrearage = "Arrearage";
		public static string Prop_InvoiceNo = "InvoiceNo";
		public static string Prop_InvoiceAmount = "InvoiceAmount";
		public static string Prop_BillType = "BillType";
		public static string Prop_AmountType = "AmountType";
		public static string Prop_OperateDate = "OperateDate";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _correspondBillNo;
		private string _deliveryOrderId;
		private string _customerId;
		private string _customerName;
		private string _inWarehouseId;
		private string _supplierId;
		private string _supplierName;
		private System.Decimal? _borrowAmount;
		private System.Decimal? _payAmount;
		private System.Decimal? _arrearage;
		private string _invoiceNo;
		private System.Decimal? _invoiceAmount;
		private string _billType;
		private string _amountType;
		private DateTime? _operateDate;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public CorrespondBill()
		{
		}

		public CorrespondBill(
			string p_id,
			string p_correspondBillNo,
			string p_deliveryOrderId,
			string p_customerId,
			string p_customerName,
			string p_inWarehouseId,
			string p_supplierId,
			string p_supplierName,
			System.Decimal? p_borrowAmount,
			System.Decimal? p_payAmount,
			System.Decimal? p_arrearage,
			string p_invoiceNo,
			System.Decimal? p_invoiceAmount,
			string p_billType,
			string p_amountType,
			DateTime? p_operateDate,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_correspondBillNo = p_correspondBillNo;
			_deliveryOrderId = p_deliveryOrderId;
			_customerId = p_customerId;
			_customerName = p_customerName;
			_inWarehouseId = p_inWarehouseId;
			_supplierId = p_supplierId;
			_supplierName = p_supplierName;
			_borrowAmount = p_borrowAmount;
			_payAmount = p_payAmount;
			_arrearage = p_arrearage;
			_invoiceNo = p_invoiceNo;
			_invoiceAmount = p_invoiceAmount;
			_billType = p_billType;
			_amountType = p_amountType;
			_operateDate = p_operateDate;
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

		[Property("CorrespondBillNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CorrespondBillNo
		{
			get { return _correspondBillNo; }
			set
			{
				if ((_correspondBillNo == null) || (value == null) || (!value.Equals(_correspondBillNo)))
				{
                    object oldValue = _correspondBillNo;
					_correspondBillNo = value;
					RaisePropertyChanged(CorrespondBill.Prop_CorrespondBillNo, oldValue, value);
				}
			}

		}

		[Property("DeliveryOrderId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string DeliveryOrderId
		{
			get { return _deliveryOrderId; }
			set
			{
				if ((_deliveryOrderId == null) || (value == null) || (!value.Equals(_deliveryOrderId)))
				{
                    object oldValue = _deliveryOrderId;
					_deliveryOrderId = value;
					RaisePropertyChanged(CorrespondBill.Prop_DeliveryOrderId, oldValue, value);
				}
			}

		}

		[Property("CustomerId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CustomerId
		{
			get { return _customerId; }
			set
			{
				if ((_customerId == null) || (value == null) || (!value.Equals(_customerId)))
				{
                    object oldValue = _customerId;
					_customerId = value;
					RaisePropertyChanged(CorrespondBill.Prop_CustomerId, oldValue, value);
				}
			}

		}

		[Property("CustomerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string CustomerName
		{
			get { return _customerName; }
			set
			{
				if ((_customerName == null) || (value == null) || (!value.Equals(_customerName)))
				{
                    object oldValue = _customerName;
					_customerName = value;
					RaisePropertyChanged(CorrespondBill.Prop_CustomerName, oldValue, value);
				}
			}

		}

		[Property("InWarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InWarehouseId
		{
			get { return _inWarehouseId; }
			set
			{
				if ((_inWarehouseId == null) || (value == null) || (!value.Equals(_inWarehouseId)))
				{
                    object oldValue = _inWarehouseId;
					_inWarehouseId = value;
					RaisePropertyChanged(CorrespondBill.Prop_InWarehouseId, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_SupplierId, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_SupplierName, oldValue, value);
				}
			}

		}

		[Property("BorrowAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? BorrowAmount
		{
			get { return _borrowAmount; }
			set
			{
				if (value != _borrowAmount)
				{
                    object oldValue = _borrowAmount;
					_borrowAmount = value;
					RaisePropertyChanged(CorrespondBill.Prop_BorrowAmount, oldValue, value);
				}
			}

		}

		[Property("PayAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PayAmount
		{
			get { return _payAmount; }
			set
			{
				if (value != _payAmount)
				{
                    object oldValue = _payAmount;
					_payAmount = value;
					RaisePropertyChanged(CorrespondBill.Prop_PayAmount, oldValue, value);
				}
			}

		}

		[Property("Arrearage", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Arrearage
		{
			get { return _arrearage; }
			set
			{
				if (value != _arrearage)
				{
                    object oldValue = _arrearage;
					_arrearage = value;
					RaisePropertyChanged(CorrespondBill.Prop_Arrearage, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_InvoiceNo, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_InvoiceAmount, oldValue, value);
				}
			}

		}

		[Property("BillType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string BillType
		{
			get { return _billType; }
			set
			{
				if ((_billType == null) || (value == null) || (!value.Equals(_billType)))
				{
                    object oldValue = _billType;
					_billType = value;
					RaisePropertyChanged(CorrespondBill.Prop_BillType, oldValue, value);
				}
			}

		}

		[Property("AmountType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string AmountType
		{
			get { return _amountType; }
			set
			{
				if ((_amountType == null) || (value == null) || (!value.Equals(_amountType)))
				{
                    object oldValue = _amountType;
					_amountType = value;
					RaisePropertyChanged(CorrespondBill.Prop_AmountType, oldValue, value);
				}
			}

		}

		[Property("OperateDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? OperateDate
		{
			get { return _operateDate; }
			set
			{
				if (value != _operateDate)
				{
                    object oldValue = _operateDate;
					_operateDate = value;
					RaisePropertyChanged(CorrespondBill.Prop_OperateDate, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(CorrespondBill.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // CorrespondBill
}

