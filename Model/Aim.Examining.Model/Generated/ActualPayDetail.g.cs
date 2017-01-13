// Business class ActualPayDetail generated from ActualPayDetail
// Creator: Ray
// Created Date: [2012-04-10]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ActualPayDetail")]
	public partial class ActualPayDetail : ExamModelBase<ActualPayDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PayBillId = "PayBillId";
		public static string Prop_ActualPayAmount = "ActualPayAmount";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";

		#endregion

		#region Private_Variables

		private string _id;
		private string _payBillId;
		private System.Decimal? _actualPayAmount;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;


		#endregion

		#region Constructors

		public ActualPayDetail()
		{
		}

		public ActualPayDetail(
			string p_id,
			string p_payBillId,
			System.Decimal? p_actualPayAmount,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark)
		{
			_id = p_id;
			_payBillId = p_payBillId;
			_actualPayAmount = p_actualPayAmount;
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

		[Property("PayBillId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string PayBillId
		{
			get { return _payBillId; }
			set
			{
				if ((_payBillId == null) || (value == null) || (!value.Equals(_payBillId)))
				{
                    object oldValue = _payBillId;
					_payBillId = value;
					RaisePropertyChanged(ActualPayDetail.Prop_PayBillId, oldValue, value);
				}
			}

		}

		[Property("ActualPayAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ActualPayAmount
		{
			get { return _actualPayAmount; }
			set
			{
				if (value != _actualPayAmount)
				{
                    object oldValue = _actualPayAmount;
					_actualPayAmount = value;
					RaisePropertyChanged(ActualPayDetail.Prop_ActualPayAmount, oldValue, value);
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
					RaisePropertyChanged(ActualPayDetail.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(ActualPayDetail.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(ActualPayDetail.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(ActualPayDetail.Prop_Remark, oldValue, value);
				}
			}

		}

		#endregion
	} // ActualPayDetail
}

