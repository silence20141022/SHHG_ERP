// Business class InWarehouseDetail generated from InWarehouseDetail
// Creator: Ray
// Created Date: [2015-01-28]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("InWarehouseDetail")]
	public partial class InWarehouseDetail : ExamModelBase<InWarehouseDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_InWarehouseId = "InWarehouseId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ProductCode = "ProductCode";
		public static string Prop_PurchaseOrderDetailId = "PurchaseOrderDetailId";
		public static string Prop_IQuantity = "IQuantity";
		public static string Prop_YiRuQuan = "YiRuQuan";
		public static string Prop_InWarehouseState = "InWarehouseState";
		public static string Prop_Remark = "Remark";
		public static string Prop_SkinArray = "SkinArray";
		public static string Prop_SeriesArray = "SeriesArray";
		public static string Prop_FuKuanDanQuan = "FuKuanDanQuan";
		public static string Prop_KaiPiaoQuan = "KaiPiaoQuan";

		#endregion

		#region Private_Variables

		private string _id;
		private string _inWarehouseId;
		private string _productId;
		private string _productCode;
		private string _purchaseOrderDetailId;
		private int? _iQuantity;
		private int? _yiRuQuan;
		private string _inWarehouseState;
		private string _remark;
		private string _skinArray;
		private string _seriesArray;
		private int? _fuKuanDanQuan;
		private int? _kaiPiaoQuan;


		#endregion

		#region Constructors

		public InWarehouseDetail()
		{
		}

		public InWarehouseDetail(
			string p_id,
			string p_inWarehouseId,
			string p_productId,
			string p_productCode,
			string p_purchaseOrderDetailId,
			int? p_iQuantity,
			int? p_yiRuQuan,
			string p_inWarehouseState,
			string p_remark,
			string p_skinArray,
			string p_seriesArray,
			int? p_fuKuanDanQuan,
			int? p_kaiPiaoQuan)
		{
			_id = p_id;
			_inWarehouseId = p_inWarehouseId;
			_productId = p_productId;
			_productCode = p_productCode;
			_purchaseOrderDetailId = p_purchaseOrderDetailId;
			_iQuantity = p_iQuantity;
			_yiRuQuan = p_yiRuQuan;
			_inWarehouseState = p_inWarehouseState;
			_remark = p_remark;
			_skinArray = p_skinArray;
			_seriesArray = p_seriesArray;
			_fuKuanDanQuan = p_fuKuanDanQuan;
			_kaiPiaoQuan = p_kaiPiaoQuan;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(InWarehouseDetail.Prop_InWarehouseId, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("ProductCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string ProductCode
		{
			get { return _productCode; }
			set
			{
				if ((_productCode == null) || (value == null) || (!value.Equals(_productCode)))
				{
                    object oldValue = _productCode;
					_productCode = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_ProductCode, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetail.Prop_PurchaseOrderDetailId, oldValue, value);
				}
			}

		}

		[Property("IQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? IQuantity
		{
			get { return _iQuantity; }
			set
			{
				if (value != _iQuantity)
				{
                    object oldValue = _iQuantity;
					_iQuantity = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_IQuantity, oldValue, value);
				}
			}

		}

		[Property("YiRuQuan", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? YiRuQuan
		{
			get { return _yiRuQuan; }
			set
			{
				if (value != _yiRuQuan)
				{
                    object oldValue = _yiRuQuan;
					_yiRuQuan = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_YiRuQuan, oldValue, value);
				}
			}

		}

		[Property("InWarehouseState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string InWarehouseState
		{
			get { return _inWarehouseState; }
			set
			{
				if ((_inWarehouseState == null) || (value == null) || (!value.Equals(_inWarehouseState)))
				{
                    object oldValue = _inWarehouseState;
					_inWarehouseState = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_InWarehouseState, oldValue, value);
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
					RaisePropertyChanged(InWarehouseDetail.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("SkinArray", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string SkinArray
		{
			get { return _skinArray; }
			set
			{
				if ((_skinArray == null) || (value == null) || (!value.Equals(_skinArray)))
				{
                    object oldValue = _skinArray;
					_skinArray = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_SkinArray, oldValue, value);
				}
			}

		}

		[Property("SeriesArray", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string SeriesArray
		{
			get { return _seriesArray; }
			set
			{
				if ((_seriesArray == null) || (value == null) || (!value.Equals(_seriesArray)))
				{
                    object oldValue = _seriesArray;
					_seriesArray = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_SeriesArray, oldValue, value);
				}
			}

		}

		[Property("FuKuanDanQuan", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? FuKuanDanQuan
		{
			get { return _fuKuanDanQuan; }
			set
			{
				if (value != _fuKuanDanQuan)
				{
                    object oldValue = _fuKuanDanQuan;
					_fuKuanDanQuan = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_FuKuanDanQuan, oldValue, value);
				}
			}

		}

		[Property("KaiPiaoQuan", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? KaiPiaoQuan
		{
			get { return _kaiPiaoQuan; }
			set
			{
				if (value != _kaiPiaoQuan)
				{
                    object oldValue = _kaiPiaoQuan;
					_kaiPiaoQuan = value;
					RaisePropertyChanged(InWarehouseDetail.Prop_KaiPiaoQuan, oldValue, value);
				}
			}

		}

		#endregion
	} // InWarehouseDetail
}

