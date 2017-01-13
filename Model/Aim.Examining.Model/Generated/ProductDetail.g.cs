// Business class ProductDetail generated from ProductDetails
// Creator: Ray
// Created Date: [2012-02-29]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ProductDetails")]
	public partial class ProductDetail : ExamModelBase<ProductDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PId = "PId";
		public static string Prop_PISBN = "PISBN";
		public static string Prop_PPcn = "PPcn";
		public static string Prop_PCode = "PCode";
		public static string Prop_PName = "PName";
		public static string Prop_GuId = "GuId";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_State = "State";

		#endregion

		#region Private_Variables

		private string _id;
		private string _pId;
		private string _pISBN;
		private string _pPcn;
		private string _pCode;
		private string _pName;
		private string _guId;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private bool? _state;


		#endregion

		#region Constructors

		public ProductDetail()
		{
		}

		public ProductDetail(
			string p_id,
			string p_pId,
			string p_pISBN,
			string p_pPcn,
			string p_pCode,
			string p_pName,
			string p_guId,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			bool? p_state)
		{
			_id = p_id;
			_pId = p_pId;
			_pISBN = p_pISBN;
			_pPcn = p_pPcn;
			_pCode = p_pCode;
			_pName = p_pName;
			_guId = p_guId;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_state = p_state;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(ProductDetail.Prop_PId, oldValue, value);
				}
			}

		}

		[Property("PISBN", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PISBN
		{
			get { return _pISBN; }
			set
			{
				if ((_pISBN == null) || (value == null) || (!value.Equals(_pISBN)))
				{
                    object oldValue = _pISBN;
					_pISBN = value;
					RaisePropertyChanged(ProductDetail.Prop_PISBN, oldValue, value);
				}
			}

		}

		[Property("PPcn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PPcn
		{
			get { return _pPcn; }
			set
			{
				if ((_pPcn == null) || (value == null) || (!value.Equals(_pPcn)))
				{
                    object oldValue = _pPcn;
					_pPcn = value;
					RaisePropertyChanged(ProductDetail.Prop_PPcn, oldValue, value);
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
					RaisePropertyChanged(ProductDetail.Prop_PCode, oldValue, value);
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
					RaisePropertyChanged(ProductDetail.Prop_PName, oldValue, value);
				}
			}

		}

		[Property("GuId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string GuId
		{
			get { return _guId; }
			set
			{
				if ((_guId == null) || (value == null) || (!value.Equals(_guId)))
				{
                    object oldValue = _guId;
					_guId = value;
					RaisePropertyChanged(ProductDetail.Prop_GuId, oldValue, value);
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
					RaisePropertyChanged(ProductDetail.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(ProductDetail.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(ProductDetail.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(ProductDetail.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("State", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public bool? State
		{
			get { return _state; }
			set
			{
				if (value != _state)
				{
                    object oldValue = _state;
					_state = value;
					RaisePropertyChanged(ProductDetail.Prop_State, oldValue, value);
				}
			}

		}

		#endregion
	} // ProductDetail
}

