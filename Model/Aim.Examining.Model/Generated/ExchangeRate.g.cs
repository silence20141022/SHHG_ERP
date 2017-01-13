// Business class ExchangeRate generated from ExchangeRate
// Creator: Ray
// Created Date: [2012-03-11]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ExchangeRate")]
	public partial class ExchangeRate : ExamModelBase<ExchangeRate>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_MoneyType = "MoneyType";
		public static string Prop_Rate = "Rate";
		public static string Prop_Symbo = "Symbo";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_ModifyDate = "ModifyDate";
		public static string Prop_ModifyUserId = "ModifyUserId";
		public static string Prop_ModifyName = "ModifyName";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _moneyType;
		private System.Decimal? _rate;
		private string _symbo;
		private DateTime? _createTime;
		private string _createId;
		private string _createName;
		private DateTime? _modifyDate;
		private string _modifyUserId;
		private string _modifyName;
		private string _remark;


		#endregion

		#region Constructors

		public ExchangeRate()
		{
		}

		public ExchangeRate(
			string p_id,
			string p_moneyType,
			System.Decimal? p_rate,
			string p_symbo,
			DateTime? p_createTime,
			string p_createId,
			string p_createName,
			DateTime? p_modifyDate,
			string p_modifyUserId,
			string p_modifyName,
			string p_remark)
		{
			_id = p_id;
			_moneyType = p_moneyType;
			_rate = p_rate;
			_symbo = p_symbo;
			_createTime = p_createTime;
			_createId = p_createId;
			_createName = p_createName;
			_modifyDate = p_modifyDate;
			_modifyUserId = p_modifyUserId;
			_modifyName = p_modifyName;
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

		[Property("MoneyType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string MoneyType
		{
			get { return _moneyType; }
			set
			{
				if ((_moneyType == null) || (value == null) || (!value.Equals(_moneyType)))
				{
                    object oldValue = _moneyType;
					_moneyType = value;
					RaisePropertyChanged(ExchangeRate.Prop_MoneyType, oldValue, value);
				}
			}

		}

		[Property("Rate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Rate
		{
			get { return _rate; }
			set
			{
				if (value != _rate)
				{
                    object oldValue = _rate;
					_rate = value;
					RaisePropertyChanged(ExchangeRate.Prop_Rate, oldValue, value);
				}
			}

		}

		[Property("Symbo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Symbo
		{
			get { return _symbo; }
			set
			{
				if ((_symbo == null) || (value == null) || (!value.Equals(_symbo)))
				{
                    object oldValue = _symbo;
					_symbo = value;
					RaisePropertyChanged(ExchangeRate.Prop_Symbo, oldValue, value);
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
					RaisePropertyChanged(ExchangeRate.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(ExchangeRate.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(ExchangeRate.Prop_CreateName, oldValue, value);
				}
			}

		}

		[Property("ModifyDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? ModifyDate
		{
			get { return _modifyDate; }
			set
			{
				if (value != _modifyDate)
				{
                    object oldValue = _modifyDate;
					_modifyDate = value;
					RaisePropertyChanged(ExchangeRate.Prop_ModifyDate, oldValue, value);
				}
			}

		}

		[Property("ModifyUserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ModifyUserId
		{
			get { return _modifyUserId; }
			set
			{
				if ((_modifyUserId == null) || (value == null) || (!value.Equals(_modifyUserId)))
				{
                    object oldValue = _modifyUserId;
					_modifyUserId = value;
					RaisePropertyChanged(ExchangeRate.Prop_ModifyUserId, oldValue, value);
				}
			}

		}

		[Property("ModifyName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ModifyName
		{
			get { return _modifyName; }
			set
			{
				if ((_modifyName == null) || (value == null) || (!value.Equals(_modifyName)))
				{
                    object oldValue = _modifyName;
					_modifyName = value;
					RaisePropertyChanged(ExchangeRate.Prop_ModifyName, oldValue, value);
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
					RaisePropertyChanged(ExchangeRate.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // ExchangeRate
}

