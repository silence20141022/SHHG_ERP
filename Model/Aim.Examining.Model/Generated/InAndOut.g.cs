// Business class InAndOut generated from InAndOut
// Creator: Ray
// Created Date: [2012-03-04]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("InAndOut")]
	public partial class InAndOut : ExamModelBase<InAndOut>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_DeliveryOrderId = "DeliveryOrderId";
		public static string Prop_DeliveryOrderNo = "DeliveryOrderNo";
		public static string Prop_InWarehouseId = "InWarehouseId";
		public static string Prop_InWarehouseNo = "InWarehouseNo";
		public static string Prop_CustomerId = "CustomerId";
		public static string Prop_CustomerName = "CustomerName";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _deliveryOrderId;
		private string _deliveryOrderNo;
		private string _inWarehouseId;
		private string _inWarehouseNo;
		private string _customerId;
		private string _customerName;
		private string _supplierId;
		private string _supplierName;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;


		#endregion

		#region Constructors

		public InAndOut()
		{
		}

		public InAndOut(
			string p_id,
			string p_deliveryOrderId,
			string p_deliveryOrderNo,
			string p_inWarehouseId,
			string p_inWarehouseNo,
			string p_customerId,
			string p_customerName,
			string p_supplierId,
			string p_supplierName,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark)
		{
			_id = p_id;
			_deliveryOrderId = p_deliveryOrderId;
			_deliveryOrderNo = p_deliveryOrderNo;
			_inWarehouseId = p_inWarehouseId;
			_inWarehouseNo = p_inWarehouseNo;
			_customerId = p_customerId;
			_customerName = p_customerName;
			_supplierId = p_supplierId;
			_supplierName = p_supplierName;
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
					RaisePropertyChanged(InAndOut.Prop_DeliveryOrderId, oldValue, value);
				}
			}

		}

		[Property("DeliveryOrderNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string DeliveryOrderNo
		{
			get { return _deliveryOrderNo; }
			set
			{
				if ((_deliveryOrderNo == null) || (value == null) || (!value.Equals(_deliveryOrderNo)))
				{
                    object oldValue = _deliveryOrderNo;
					_deliveryOrderNo = value;
					RaisePropertyChanged(InAndOut.Prop_DeliveryOrderNo, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_InWarehouseId, oldValue, value);
				}
			}

		}

		[Property("InWarehouseNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InWarehouseNo
		{
			get { return _inWarehouseNo; }
			set
			{
				if ((_inWarehouseNo == null) || (value == null) || (!value.Equals(_inWarehouseNo)))
				{
                    object oldValue = _inWarehouseNo;
					_inWarehouseNo = value;
					RaisePropertyChanged(InAndOut.Prop_InWarehouseNo, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_CustomerId, oldValue, value);
				}
			}

		}

		[Property("CustomerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CustomerName
		{
			get { return _customerName; }
			set
			{
				if ((_customerName == null) || (value == null) || (!value.Equals(_customerName)))
				{
                    object oldValue = _customerName;
					_customerName = value;
					RaisePropertyChanged(InAndOut.Prop_CustomerName, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_SupplierId, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_SupplierName, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(InAndOut.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // InAndOut
}

