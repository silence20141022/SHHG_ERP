// Business class Compressor generated from Compressor
// Creator: Ray
// Created Date: [2012-03-31]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Compressor")]
	public partial class Compressor : ExamModelBase<Compressor>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_SeriesNo = "SeriesNo";
		public static string Prop_SkinId = "SkinId";
		public static string Prop_ModelNo = "ModelNo";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_InWarehouseId = "InWarehouseId";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_DeliveryOrderId = "DeliveryOrderId";
		public static string Prop_CustomerId = "CustomerId";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _seriesNo;
		private string _skinId;
		private string _modelNo;
		private string _productId;
		private string _inWarehouseId;
		private string _supplierId;
		private string _deliveryOrderId;
		private string _customerId;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _state;
		private string _remark;


		#endregion

		#region Constructors

		public Compressor()
		{
		}

		public Compressor(
			string p_id,
			string p_seriesNo,
			string p_skinId,
			string p_modelNo,
			string p_productId,
			string p_inWarehouseId,
			string p_supplierId,
			string p_deliveryOrderId,
			string p_customerId,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_state,
			string p_remark)
		{
			_id = p_id;
			_seriesNo = p_seriesNo;
			_skinId = p_skinId;
			_modelNo = p_modelNo;
			_productId = p_productId;
			_inWarehouseId = p_inWarehouseId;
			_supplierId = p_supplierId;
			_deliveryOrderId = p_deliveryOrderId;
			_customerId = p_customerId;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_state = p_state;
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

		[Property("SeriesNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SeriesNo
		{
			get { return _seriesNo; }
			set
			{
				if ((_seriesNo == null) || (value == null) || (!value.Equals(_seriesNo)))
				{
                    object oldValue = _seriesNo;
					_seriesNo = value;
					RaisePropertyChanged(Compressor.Prop_SeriesNo, oldValue, value);
				}
			}

		}

		[Property("SkinId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string SkinId
		{
			get { return _skinId; }
			set
			{
				if ((_skinId == null) || (value == null) || (!value.Equals(_skinId)))
				{
                    object oldValue = _skinId;
					_skinId = value;
					RaisePropertyChanged(Compressor.Prop_SkinId, oldValue, value);
				}
			}

		}

		[Property("ModelNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ModelNo
		{
			get { return _modelNo; }
			set
			{
				if ((_modelNo == null) || (value == null) || (!value.Equals(_modelNo)))
				{
                    object oldValue = _modelNo;
					_modelNo = value;
					RaisePropertyChanged(Compressor.Prop_ModelNo, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_ProductId, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_InWarehouseId, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_SupplierId, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_DeliveryOrderId, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_CustomerId, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_State, oldValue, value);
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
					RaisePropertyChanged(Compressor.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // Compressor
}

