// Business class DelieryOrderPart generated from DelieryOrderPart
// Creator: Ray
// Created Date: [2013-08-01]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("DelieryOrderPart")]
	public partial class DelieryOrderPart : ExamModelBase<DelieryOrderPart>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_DId = "DId";
		public static string Prop_PId = "PId";
		public static string Prop_PCode = "PCode";
		public static string Prop_PName = "PName";
		public static string Prop_Isbn = "Isbn";
		public static string Prop_Count = "Count";
		public static string Prop_Guids = "Guids";
		public static string Prop_Unit = "Unit";
		public static string Prop_State = "State";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_OutCount = "OutCount";
		public static string Prop_InvoiceNo = "InvoiceNo";
		public static string Prop_CostPrice = "CostPrice";
		public static string Prop_CostAmount = "CostAmount";

		#endregion

		#region Private_Variables

		private string _id;
		private string _dId;
		private string _pId;
		private string _pCode;
		private string _pName;
		private string _isbn;
		private int? _count;
		private string _guids;
		private string _unit;
		private string _state;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _productId;
		private int? _outCount;
		private string _invoiceNo;
		private System.Decimal? _costPrice;
		private System.Decimal? _costAmount;


		#endregion

		#region Constructors

		public DelieryOrderPart()
		{
		}

		public DelieryOrderPart(
			string p_id,
			string p_dId,
			string p_pId,
			string p_pCode,
			string p_pName,
			string p_isbn,
			int? p_count,
			string p_guids,
			string p_unit,
			string p_state,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_productId,
			int? p_outCount,
			string p_invoiceNo,
			System.Decimal? p_costPrice,
			System.Decimal? p_costAmount)
		{
			_id = p_id;
			_dId = p_dId;
			_pId = p_pId;
			_pCode = p_pCode;
			_pName = p_pName;
			_isbn = p_isbn;
			_count = p_count;
			_guids = p_guids;
			_unit = p_unit;
			_state = p_state;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_productId = p_productId;
			_outCount = p_outCount;
			_invoiceNo = p_invoiceNo;
			_costPrice = p_costPrice;
			_costAmount = p_costAmount;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("DId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string DId
		{
			get { return _dId; }
			set
			{
				if ((_dId == null) || (value == null) || (!value.Equals(_dId)))
				{
                    object oldValue = _dId;
					_dId = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_DId, oldValue, value);
				}
			}

		}

		[Property("PId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PId
		{
			get { return _pId; }
			set
			{
				if ((_pId == null) || (value == null) || (!value.Equals(_pId)))
				{
                    object oldValue = _pId;
					_pId = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_PId, oldValue, value);
				}
			}

		}

		[Property("PCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PCode
		{
			get { return _pCode; }
			set
			{
				if ((_pCode == null) || (value == null) || (!value.Equals(_pCode)))
				{
                    object oldValue = _pCode;
					_pCode = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_PCode, oldValue, value);
				}
			}

		}

		[Property("PName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string PName
		{
			get { return _pName; }
			set
			{
				if ((_pName == null) || (value == null) || (!value.Equals(_pName)))
				{
                    object oldValue = _pName;
					_pName = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_PName, oldValue, value);
				}
			}

		}

		[Property("Isbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Isbn
		{
			get { return _isbn; }
			set
			{
				if ((_isbn == null) || (value == null) || (!value.Equals(_isbn)))
				{
                    object oldValue = _isbn;
					_isbn = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_Isbn, oldValue, value);
				}
			}

		}

		[Property("Count", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Count
		{
			get { return _count; }
			set
			{
				if (value != _count)
				{
                    object oldValue = _count;
					_count = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_Count, oldValue, value);
				}
			}

		}

		[Property("Guids", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Guids
		{
			get { return _guids; }
			set
			{
				if ((_guids == null) || (value == null) || (!value.Equals(_guids)))
				{
                    object oldValue = _guids;
					_guids = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_Guids, oldValue, value);
				}
			}

		}

		[Property("Unit", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string Unit
		{
			get { return _unit; }
			set
			{
				if ((_unit == null) || (value == null) || (!value.Equals(_unit)))
				{
                    object oldValue = _unit;
					_unit = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_Unit, oldValue, value);
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
					RaisePropertyChanged(DelieryOrderPart.Prop_State, oldValue, value);
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
					RaisePropertyChanged(DelieryOrderPart.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(DelieryOrderPart.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(DelieryOrderPart.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(DelieryOrderPart.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(DelieryOrderPart.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("OutCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? OutCount
		{
			get { return _outCount; }
			set
			{
				if (value != _outCount)
				{
                    object oldValue = _outCount;
					_outCount = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_OutCount, oldValue, value);
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
					RaisePropertyChanged(DelieryOrderPart.Prop_InvoiceNo, oldValue, value);
				}
			}

		}

		[Property("CostPrice", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? CostPrice
		{
			get { return _costPrice; }
			set
			{
				if (value != _costPrice)
				{
                    object oldValue = _costPrice;
					_costPrice = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_CostPrice, oldValue, value);
				}
			}

		}

		[Property("CostAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? CostAmount
		{
			get { return _costAmount; }
			set
			{
				if (value != _costAmount)
				{
                    object oldValue = _costAmount;
					_costAmount = value;
					RaisePropertyChanged(DelieryOrderPart.Prop_CostAmount, oldValue, value);
				}
			}

		}

		#endregion
	} // DelieryOrderPart
}

