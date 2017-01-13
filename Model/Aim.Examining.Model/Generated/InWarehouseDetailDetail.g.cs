// Business class InWarehouseDetailDetail generated from InWarehouseDetailDetail
// Creator: Ray
// Created Date: [2012-03-15]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("InWarehouseDetailDetail")]
	public partial class InWarehouseDetailDetail : ExamModelBase<InWarehouseDetailDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_InWarehouseDetailId = "InWarehouseDetailId";
		public static string Prop_PurchaseOrderDetailId = "PurchaseOrderDetailId";
		public static string Prop_OtherInWarehouseDetailId = "OtherInWarehouseDetailId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_Quantity = "Quantity";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _inWarehouseDetailId;
		private string _purchaseOrderDetailId;
		private string _otherInWarehouseDetailId;
		private string _productId;
		private int? _quantity;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;


		#endregion

		#region Constructors

		public InWarehouseDetailDetail()
		{
		}

		public InWarehouseDetailDetail(
			string p_id,
			string p_inWarehouseDetailId,
			string p_purchaseOrderDetailId,
			string p_otherInWarehouseDetailId,
			string p_productId,
			int? p_quantity,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark)
		{
			_id = p_id;
			_inWarehouseDetailId = p_inWarehouseDetailId;
			_purchaseOrderDetailId = p_purchaseOrderDetailId;
			_otherInWarehouseDetailId = p_otherInWarehouseDetailId;
			_productId = p_productId;
			_quantity = p_quantity;
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

		[Property("InWarehouseDetailId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InWarehouseDetailId
		{
			get { return _inWarehouseDetailId; }
			set
			{
				if ((_inWarehouseDetailId == null) || (value == null) || (!value.Equals(_inWarehouseDetailId)))
				{
                    object oldValue = _inWarehouseDetailId;
					_inWarehouseDetailId = value;
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_InWarehouseDetailId, oldValue, value);
				}
			}

		}

		[Property("PurchaseOrderDetailId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PurchaseOrderDetailId
		{
			get { return _purchaseOrderDetailId; }
			set
			{
				if ((_purchaseOrderDetailId == null) || (value == null) || (!value.Equals(_purchaseOrderDetailId)))
				{
                    object oldValue = _purchaseOrderDetailId;
					_purchaseOrderDetailId = value;
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_PurchaseOrderDetailId, oldValue, value);
				}
			}

		}

		[Property("OtherInWarehouseDetailId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OtherInWarehouseDetailId
		{
			get { return _otherInWarehouseDetailId; }
			set
			{
				if ((_otherInWarehouseDetailId == null) || (value == null) || (!value.Equals(_otherInWarehouseDetailId)))
				{
                    object oldValue = _otherInWarehouseDetailId;
					_otherInWarehouseDetailId = value;
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_OtherInWarehouseDetailId, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("Quantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Quantity
		{
			get { return _quantity; }
			set
			{
				if (value != _quantity)
				{
                    object oldValue = _quantity;
					_quantity = value;
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_Quantity, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetailDetail.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // InWarehouseDetailDetail
}

