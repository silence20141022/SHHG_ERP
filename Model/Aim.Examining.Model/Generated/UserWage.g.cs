// Business class UserWage generated from UserWage
// Creator: Ray
// Created Date: [2012-03-02]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("UserWage")]
	public partial class UserWage : ExamModelBase<UserWage>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_UserId = "UserId";
		public static string Prop_UserName = "UserName";
		public static string Prop_DeptName = "DeptName";
		public static string Prop_WorkNo = "WorkNo";
		public static string Prop_LoginName = "LoginName";
		public static string Prop_Wage = "Wage";
		public static string Prop_Bonus = "Bonus";
		public static string Prop_Total = "Total";
		public static string Prop_Remark = "Remark";
		public static string Prop_Stage = "Stage";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _userId;
		private string _userName;
		private string _deptName;
		private string _workNo;
		private string _loginName;
		private System.Decimal? _wage;
		private System.Decimal? _bonus;
		private System.Decimal? _total;
		private string _remark;
		private string _stage;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public UserWage()
		{
		}

		public UserWage(
			string p_id,
			string p_userId,
			string p_userName,
			string p_deptName,
			string p_workNo,
			string p_loginName,
			System.Decimal? p_wage,
			System.Decimal? p_bonus,
			System.Decimal? p_total,
			string p_remark,
			string p_stage,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_userId = p_userId;
			_userName = p_userName;
			_deptName = p_deptName;
			_workNo = p_workNo;
			_loginName = p_loginName;
			_wage = p_wage;
			_bonus = p_bonus;
			_total = p_total;
			_remark = p_remark;
			_stage = p_stage;
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

		[Property("UserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string UserId
		{
			get { return _userId; }
			set
			{
				if ((_userId == null) || (value == null) || (!value.Equals(_userId)))
				{
                    object oldValue = _userId;
					_userId = value;
					RaisePropertyChanged(UserWage.Prop_UserId, oldValue, value);
				}
			}

		}

		[Property("UserName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string UserName
		{
			get { return _userName; }
			set
			{
				if ((_userName == null) || (value == null) || (!value.Equals(_userName)))
				{
                    object oldValue = _userName;
					_userName = value;
					RaisePropertyChanged(UserWage.Prop_UserName, oldValue, value);
				}
			}

		}

		[Property("DeptName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string DeptName
		{
			get { return _deptName; }
			set
			{
				if ((_deptName == null) || (value == null) || (!value.Equals(_deptName)))
				{
                    object oldValue = _deptName;
					_deptName = value;
					RaisePropertyChanged(UserWage.Prop_DeptName, oldValue, value);
				}
			}

		}

		[Property("WorkNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string WorkNo
		{
			get { return _workNo; }
			set
			{
				if ((_workNo == null) || (value == null) || (!value.Equals(_workNo)))
				{
                    object oldValue = _workNo;
					_workNo = value;
					RaisePropertyChanged(UserWage.Prop_WorkNo, oldValue, value);
				}
			}

		}

		[Property("LoginName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string LoginName
		{
			get { return _loginName; }
			set
			{
				if ((_loginName == null) || (value == null) || (!value.Equals(_loginName)))
				{
                    object oldValue = _loginName;
					_loginName = value;
					RaisePropertyChanged(UserWage.Prop_LoginName, oldValue, value);
				}
			}

		}

		[Property("Wage", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Wage
		{
			get { return _wage; }
			set
			{
				if (value != _wage)
				{
                    object oldValue = _wage;
					_wage = value;
					RaisePropertyChanged(UserWage.Prop_Wage, oldValue, value);
				}
			}

		}

		[Property("Bonus", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Bonus
		{
			get { return _bonus; }
			set
			{
				if (value != _bonus)
				{
                    object oldValue = _bonus;
					_bonus = value;
					RaisePropertyChanged(UserWage.Prop_Bonus, oldValue, value);
				}
			}

		}

		[Property("Total", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Total
		{
			get { return _total; }
			set
			{
				if (value != _total)
				{
                    object oldValue = _total;
					_total = value;
					RaisePropertyChanged(UserWage.Prop_Total, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1000)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(UserWage.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("Stage", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Stage
		{
			get { return _stage; }
			set
			{
				if ((_stage == null) || (value == null) || (!value.Equals(_stage)))
				{
                    object oldValue = _stage;
					_stage = value;
					RaisePropertyChanged(UserWage.Prop_Stage, oldValue, value);
				}
			}

		}

		[Property("CreateId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CreateId
		{
			get { return _createId; }
			set
			{
				if ((_createId == null) || (value == null) || (!value.Equals(_createId)))
				{
                    object oldValue = _createId;
					_createId = value;
					RaisePropertyChanged(UserWage.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(UserWage.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(UserWage.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // UserWage
}

