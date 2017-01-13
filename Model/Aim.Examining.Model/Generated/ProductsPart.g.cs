// Business class ProductsPart generated from ProductsPart
// Creator: Ray
// Created Date: [2012-02-10]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ProductsPart")]
	public partial class ProductsPart : ExamModelBase<ProductsPart>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PId = "PId";
		public static string Prop_PIsbn = "PIsbn";
		public static string Prop_CId = "CId";
		public static string Prop_CIsbn = "CIsbn";
		public static string Prop_CCount = "CCount";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _pId;
		private string _pIsbn;
		private string _cId;
		private string _cIsbn;
		private int? _cCount;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public ProductsPart()
		{
		}

		public ProductsPart(
			string p_id,
			string p_pId,
			string p_pIsbn,
			string p_cId,
			string p_cIsbn,
			int? p_cCount,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_pId = p_pId;
			_pIsbn = p_pIsbn;
			_cId = p_cId;
			_cIsbn = p_cIsbn;
			_cCount = p_cCount;
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
					RaisePropertyChanged(ProductsPart.Prop_PId, oldValue, value);
				}
			}

		}

		[Property("PIsbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PIsbn
		{
			get { return _pIsbn; }
			set
			{
				if ((_pIsbn == null) || (value == null) || (!value.Equals(_pIsbn)))
				{
                    object oldValue = _pIsbn;
					_pIsbn = value;
					RaisePropertyChanged(ProductsPart.Prop_PIsbn, oldValue, value);
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
					RaisePropertyChanged(ProductsPart.Prop_CId, oldValue, value);
				}
			}

		}

		[Property("CIsbn", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string CIsbn
		{
			get { return _cIsbn; }
			set
			{
				if ((_cIsbn == null) || (value == null) || (!value.Equals(_cIsbn)))
				{
                    object oldValue = _cIsbn;
					_cIsbn = value;
					RaisePropertyChanged(ProductsPart.Prop_CIsbn, oldValue, value);
				}
			}

		}

		[Property("CCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? CCount
		{
			get { return _cCount; }
			set
			{
				if (value != _cCount)
				{
                    object oldValue = _cCount;
					_cCount = value;
					RaisePropertyChanged(ProductsPart.Prop_CCount, oldValue, value);
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
					RaisePropertyChanged(ProductsPart.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(ProductsPart.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(ProductsPart.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(ProductsPart.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // ProductsPart
}

