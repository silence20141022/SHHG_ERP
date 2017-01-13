// Business class TransactionHi generated from TransactionHis
// Creator: Ray
// Created Date: [2012-02-15]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("TransactionHis")]
	public partial class TransactionHi : ExamModelBase<TransactionHi>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_CId = "CId";
		public static string Prop_CCode = "CCode";
		public static string Prop_CName = "CName";
		public static string Prop_Number = "Number";
		public static string Prop_TransactionTime = "TransactionTime";
		public static string Prop_Child = "Child";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_RtnOrOut = "RtnOrOut";

		#endregion

		#region Private_Variables

		private string _id;
		private string _cId;
		private string _cCode;
		private string _cName;
		private string _number;
		private DateTime? _transactionTime;
		private string _child;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _rtnOrOut;


		#endregion

		#region Constructors

		public TransactionHi()
		{
		}

		public TransactionHi(
			string p_id,
			string p_cId,
			string p_cCode,
			string p_cName,
			string p_number,
			DateTime? p_transactionTime,
			string p_child,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_rtnOrOut)
		{
			_id = p_id;
			_cId = p_cId;
			_cCode = p_cCode;
			_cName = p_cName;
			_number = p_number;
			_transactionTime = p_transactionTime;
			_child = p_child;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_rtnOrOut = p_rtnOrOut;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(TransactionHi.Prop_CId, oldValue, value);
				}
			}

		}

		[Property("CCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string CCode
		{
			get { return _cCode; }
			set
			{
				if ((_cCode == null) || (value == null) || (!value.Equals(_cCode)))
				{
                    object oldValue = _cCode;
					_cCode = value;
					RaisePropertyChanged(TransactionHi.Prop_CCode, oldValue, value);
				}
			}

		}

		[Property("CName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string CName
		{
			get { return _cName; }
			set
			{
				if ((_cName == null) || (value == null) || (!value.Equals(_cName)))
				{
                    object oldValue = _cName;
					_cName = value;
					RaisePropertyChanged(TransactionHi.Prop_CName, oldValue, value);
				}
			}

		}

		[Property("Number", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Number
		{
			get { return _number; }
			set
			{
				if ((_number == null) || (value == null) || (!value.Equals(_number)))
				{
                    object oldValue = _number;
					_number = value;
					RaisePropertyChanged(TransactionHi.Prop_Number, oldValue, value);
				}
			}

		}

		[Property("TransactionTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? TransactionTime
		{
			get { return _transactionTime; }
			set
			{
				if (value != _transactionTime)
				{
                    object oldValue = _transactionTime;
					_transactionTime = value;
					RaisePropertyChanged(TransactionHi.Prop_TransactionTime, oldValue, value);
				}
			}

		}

		[Property("Child", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Child
		{
			get { return _child; }
			set
			{
				if ((_child == null) || (value == null) || (!value.Equals(_child)))
				{
                    object oldValue = _child;
					_child = value;
					RaisePropertyChanged(TransactionHi.Prop_Child, oldValue, value);
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
					RaisePropertyChanged(TransactionHi.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(TransactionHi.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(TransactionHi.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(TransactionHi.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("RtnOrOut", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string RtnOrOut
		{
			get { return _rtnOrOut; }
			set
			{
				if ((_rtnOrOut == null) || (value == null) || (!value.Equals(_rtnOrOut)))
				{
                    object oldValue = _rtnOrOut;
					_rtnOrOut = value;
					RaisePropertyChanged(TransactionHi.Prop_RtnOrOut, oldValue, value);
				}
			}

		}

		#endregion
	} // TransactionHi
}

