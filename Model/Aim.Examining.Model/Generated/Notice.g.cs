// Business class Notice generated from Notices
// Creator: Ray
// Created Date: [2011-11-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Notices")]
	public partial class Notice : ExamModelBase<Notice>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_MsgId = "MsgId";
		public static string Prop_Title = "Title";
		public static string Prop_ReadState = "ReadState";
		public static string Prop_Content = "Content";
		public static string Prop_UserId = "UserId";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _msgId;
		private string _title;
		private string _readState;
		private string _content;
		private string _userId;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public Notice()
		{
		}

		public Notice(
			string p_id,
			string p_msgId,
			string p_title,
			string p_readState,
			string p_content,
			string p_userId,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_msgId = p_msgId;
			_title = p_title;
			_readState = p_readState;
			_content = p_content;
			_userId = p_userId;
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
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("MsgId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string MsgId
		{
			get { return _msgId; }
			set
			{
				if ((_msgId == null) || (value == null) || (!value.Equals(_msgId)))
				{
                    object oldValue = _msgId;
					_msgId = value;
					RaisePropertyChanged(Notice.Prop_MsgId, oldValue, value);
				}
			}

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
					RaisePropertyChanged(Notice.Prop_Title, oldValue, value);
				}
			}

		}

		[Property("ReadState", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public string ReadState
		{
			get { return _readState; }
			set
			{
				if ((_readState == null) || (value == null) || (!value.Equals(_readState)))
				{
                    object oldValue = _readState;
					_readState = value;
					RaisePropertyChanged(Notice.Prop_ReadState, oldValue, value);
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
					RaisePropertyChanged(Notice.Prop_Content, oldValue, value);
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
					RaisePropertyChanged(Notice.Prop_UserId, oldValue, value);
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
					RaisePropertyChanged(Notice.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Notice.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Notice.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // Notice
}

