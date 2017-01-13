// Business class AssemblyOrDi generated from AssemblyOrDis
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
	[ActiveRecord("AssemblyOrDis")]
	public partial class AssemblyOrDi : ExamModelBase<AssemblyOrDi>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Product1 = "Product1";
		public static string Prop_Product2 = "Product2";
		public static string Prop_Op = "Op";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_PName = "PName";
		public static string Prop_PCode = "PCode";
		public static string Prop_Count = "Count";

		#endregion

		#region Private_Variables

		private string _id;
		private string _product1;
		private string _product2;
		private string _op;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _pName;
		private string _pCode;
		private string _count;


		#endregion

		#region Constructors

		public AssemblyOrDi()
		{
		}

		public AssemblyOrDi(
			string p_id,
			string p_product1,
			string p_product2,
			string p_op,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_pName,
			string p_pCode,
			string p_count)
		{
			_id = p_id;
			_product1 = p_product1;
			_product2 = p_product2;
			_op = p_op;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_pName = p_pName;
			_pCode = p_pCode;
			_count = p_count;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Product1", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Product1
		{
			get { return _product1; }
			set
			{
				if ((_product1 == null) || (value == null) || (!value.Equals(_product1)))
				{
                    object oldValue = _product1;
					_product1 = value;
					RaisePropertyChanged(AssemblyOrDi.Prop_Product1, oldValue, value);
				}
			}

		}

		[Property("Product2", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Product2
		{
			get { return _product2; }
			set
			{
				if ((_product2 == null) || (value == null) || (!value.Equals(_product2)))
				{
                    object oldValue = _product2;
					_product2 = value;
					RaisePropertyChanged(AssemblyOrDi.Prop_Product2, oldValue, value);
				}
			}

		}

		[Property("Op", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string Op
		{
			get { return _op; }
			set
			{
				if ((_op == null) || (value == null) || (!value.Equals(_op)))
				{
                    object oldValue = _op;
					_op = value;
					RaisePropertyChanged(AssemblyOrDi.Prop_Op, oldValue, value);
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
					RaisePropertyChanged(AssemblyOrDi.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(AssemblyOrDi.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(AssemblyOrDi.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(AssemblyOrDi.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("PName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string PName
		{
			get { return _pName; }
			set
			{
				if ((_pName == null) || (value == null) || (!value.Equals(_pName)))
				{
                    object oldValue = _pName;
					_pName = value;
					RaisePropertyChanged(AssemblyOrDi.Prop_PName, oldValue, value);
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
					RaisePropertyChanged(AssemblyOrDi.Prop_PCode, oldValue, value);
				}
			}

		}

		[Property("Count", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string Count
		{
			get { return _count; }
			set
			{
				if ((_count == null) || (value == null) || (!value.Equals(_count)))
				{
                    object oldValue = _count;
					_count = value;
					RaisePropertyChanged(AssemblyOrDi.Prop_Count, oldValue, value);
				}
			}

		}

		#endregion
	} // AssemblyOrDi
}

