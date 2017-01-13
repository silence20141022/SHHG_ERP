// Business class tblMail generated from tblMail
// Creator: Ray
// Created Date: [2011-11-15]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("tblMail")]
	public partial class tblMail : ExamModelBase<tblMail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Title = "Title";
		public static string Prop_Content = "Content";
		public static string Prop_acceptMail = "acceptMail";
		public static string Prop_ccMail = "ccMail";
		public static string Prop_filePath = "filePath";
		public static string Prop_state = "state";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_sendTime = "sendTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _title;
		private string _content;
		private string _acceptMail;
		private string _ccMail;
		private string _filePath;
		private string _state;
		private DateTime? _createTime;
		private string _createName;
		private DateTime? _sendTime;


		#endregion

		#region Constructors

		public tblMail()
		{
		}

		public tblMail(
			string p_id,
			string p_title,
			string p_content,
			string p_acceptMail,
			string p_ccMail,
			string p_filePath,
			string p_state,
			DateTime? p_createTime,
			string p_createName,
			DateTime? p_sendTime)
		{
			_id = p_id;
			_title = p_title;
			_content = p_content;
			_acceptMail = p_acceptMail;
			_ccMail = p_ccMail;
			_filePath = p_filePath;
			_state = p_state;
			_createTime = p_createTime;
			_createName = p_createName;
			_sendTime = p_sendTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(tblMail.Prop_Title, oldValue, value);
				}
			}

		}

		[Property("Content", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Content
		{
			get { return _content; }
			set
			{
				if ((_content == null) || (value == null) || (!value.Equals(_content)))
				{
                    object oldValue = _content;
					_content = value;
					RaisePropertyChanged(tblMail.Prop_Content, oldValue, value);
				}
			}

		}

		[Property("acceptMail", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string acceptMail
		{
			get { return _acceptMail; }
			set
			{
				if ((_acceptMail == null) || (value == null) || (!value.Equals(_acceptMail)))
				{
                    object oldValue = _acceptMail;
					_acceptMail = value;
					RaisePropertyChanged(tblMail.Prop_acceptMail, oldValue, value);
				}
			}

		}

		[Property("ccMail", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string ccMail
		{
			get { return _ccMail; }
			set
			{
				if ((_ccMail == null) || (value == null) || (!value.Equals(_ccMail)))
				{
                    object oldValue = _ccMail;
					_ccMail = value;
					RaisePropertyChanged(tblMail.Prop_ccMail, oldValue, value);
				}
			}

		}

		[Property("filePath", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string filePath
		{
			get { return _filePath; }
			set
			{
				if ((_filePath == null) || (value == null) || (!value.Equals(_filePath)))
				{
                    object oldValue = _filePath;
					_filePath = value;
					RaisePropertyChanged(tblMail.Prop_filePath, oldValue, value);
				}
			}

		}

		[Property("state", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public string state
		{
			get { return _state; }
			set
			{
				if ((_state == null) || (value == null) || (!value.Equals(_state)))
				{
                    object oldValue = _state;
					_state = value;
					RaisePropertyChanged(tblMail.Prop_state, oldValue, value);
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
					RaisePropertyChanged(tblMail.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(tblMail.Prop_CreateName, oldValue, value);
				}
			}

		}

		[Property("sendTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? sendTime
		{
			get { return _sendTime; }
			set
			{
				if (value != _sendTime)
				{
                    object oldValue = _sendTime;
					_sendTime = value;
					RaisePropertyChanged(tblMail.Prop_sendTime, oldValue, value);
				}
			}

		}

		#endregion
	} // tblMail
}

