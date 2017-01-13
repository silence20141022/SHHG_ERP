// Business class FeedBack generated from FeedBack
// Creator: Ray
// Created Date: [2011-10-08]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("FeedBack")]
	public partial class FeedBack : ExamModelBase<FeedBack>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Title = "Title";
		public static string Prop_FeedBackContent = "FeedBackContent";
		public static string Prop_ImageUrl = "ImageUrl";
		public static string Prop_UserId = "UserId";
		public static string Prop_UserName = "UserName";
		public static string Prop_Department = "Department";
		public static string Prop_Email = "Email";
		public static string Prop_Telephone = "Telephone";
		public static string Prop_CreateDate = "CreateDate";
		public static string Prop_Solution = "Solution";
		public static string Prop_SolutionDate = "SolutionDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _title;
		private string _feedBackContent;
		private string _imageUrl;
		private string _userId;
		private string _userName;
		private string _department;
		private string _email;
		private string _telephone;
		private DateTime? _createDate;
		private string _solution;
		private DateTime? _solutionDate;


		#endregion

		#region Constructors

		public FeedBack()
		{
		}

		public FeedBack(
			string p_id,
			string p_title,
			string p_feedBackContent,
			string p_imageUrl,
			string p_userId,
			string p_userName,
			string p_department,
			string p_email,
			string p_telephone,
			DateTime? p_createDate,
			string p_solution,
			DateTime? p_solutionDate)
		{
			_id = p_id;
			_title = p_title;
			_feedBackContent = p_feedBackContent;
			_imageUrl = p_imageUrl;
			_userId = p_userId;
			_userName = p_userName;
			_department = p_department;
			_email = p_email;
			_telephone = p_telephone;
			_createDate = p_createDate;
			_solution = p_solution;
			_solutionDate = p_solutionDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
		public string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(FeedBack.Prop_Title, oldValue, value);
				}
			}

		}

		[Property("FeedBackContent", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string FeedBackContent
		{
			get { return _feedBackContent; }
			set
			{
				if ((_feedBackContent == null) || (value == null) || (!value.Equals(_feedBackContent)))
				{
                    object oldValue = _feedBackContent;
					_feedBackContent = value;
					RaisePropertyChanged(FeedBack.Prop_FeedBackContent, oldValue, value);
				}
			}

		}

		[Property("ImageUrl", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string ImageUrl
		{
			get { return _imageUrl; }
			set
			{
				if ((_imageUrl == null) || (value == null) || (!value.Equals(_imageUrl)))
				{
                    object oldValue = _imageUrl;
					_imageUrl = value;
					RaisePropertyChanged(FeedBack.Prop_ImageUrl, oldValue, value);
				}
			}

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
					RaisePropertyChanged(FeedBack.Prop_UserId, oldValue, value);
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
					RaisePropertyChanged(FeedBack.Prop_UserName, oldValue, value);
				}
			}

		}

		[Property("Department", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Department
		{
			get { return _department; }
			set
			{
				if ((_department == null) || (value == null) || (!value.Equals(_department)))
				{
                    object oldValue = _department;
					_department = value;
					RaisePropertyChanged(FeedBack.Prop_Department, oldValue, value);
				}
			}

		}

		[Property("Email", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Email
		{
			get { return _email; }
			set
			{
				if ((_email == null) || (value == null) || (!value.Equals(_email)))
				{
                    object oldValue = _email;
					_email = value;
					RaisePropertyChanged(FeedBack.Prop_Email, oldValue, value);
				}
			}

		}

		[Property("Telephone", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Telephone
		{
			get { return _telephone; }
			set
			{
				if ((_telephone == null) || (value == null) || (!value.Equals(_telephone)))
				{
                    object oldValue = _telephone;
					_telephone = value;
					RaisePropertyChanged(FeedBack.Prop_Telephone, oldValue, value);
				}
			}

		}

		[Property("CreateDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? CreateDate
		{
			get { return _createDate; }
			set
			{
				if (value != _createDate)
				{
                    object oldValue = _createDate;
					_createDate = value;
					RaisePropertyChanged(FeedBack.Prop_CreateDate, oldValue, value);
				}
			}

		}

		[Property("Solution", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Solution
		{
			get { return _solution; }
			set
			{
				if ((_solution == null) || (value == null) || (!value.Equals(_solution)))
				{
                    object oldValue = _solution;
					_solution = value;
					RaisePropertyChanged(FeedBack.Prop_Solution, oldValue, value);
				}
			}

		}

		[Property("SolutionDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? SolutionDate
		{
			get { return _solutionDate; }
			set
			{
				if (value != _solutionDate)
				{
                    object oldValue = _solutionDate;
					_solutionDate = value;
					RaisePropertyChanged(FeedBack.Prop_SolutionDate, oldValue, value);
				}
			}

		}

		#endregion
	} // FeedBack
}

