// Business class ContractMoney generated from ContractMoney
// Creator: Ray
// Created Date: [2011-10-24]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ContractMoney")]
	public partial class ContractMoney : ExamModelBase<ContractMoney>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_cid = "cid";
		public static string Prop_Amount = "Amount";
		public static string Prop_createdate = "createdate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _cid;
		private System.Decimal? _amount;
		private DateTime? _createdate;


		#endregion

		#region Constructors

		public ContractMoney()
		{
		}

		public ContractMoney(
			string p_id,
			string p_cid,
			System.Decimal? p_amount,
			DateTime? p_createdate)
		{
			_id = p_id;
			_cid = p_cid;
			_amount = p_amount;
			_createdate = p_createdate;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("cid", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string cid
		{
			get { return _cid; }
			set
			{
				if ((_cid == null) || (value == null) || (!value.Equals(_cid)))
				{
                    object oldValue = _cid;
					_cid = value;
					RaisePropertyChanged(ContractMoney.Prop_cid, oldValue, value);
				}
			}

		}

		[Property("Amount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Amount
		{
			get { return _amount; }
			set
			{
				if (value != _amount)
				{
                    object oldValue = _amount;
					_amount = value;
					RaisePropertyChanged(ContractMoney.Prop_Amount, oldValue, value);
				}
			}

		}

		[Property("createdate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? createdate
		{
			get { return _createdate; }
			set
			{
				if (value != _createdate)
				{
                    object oldValue = _createdate;
					_createdate = value;
					RaisePropertyChanged(ContractMoney.Prop_createdate, oldValue, value);
				}
			}

		}

		#endregion
	} // ContractMoney
}

