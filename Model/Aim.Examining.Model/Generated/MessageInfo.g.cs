// Business class MessageInfo generated from MessageInfo
// Creator: Ray
// Created Date: [2011-11-17]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("MessageInfo")]
	public partial class MessageInfo : ExamModelBase<MessageInfo>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Title = "Title";
		public static string Prop_Content = "Content";
		public static string Prop_Importance = "Importance";
		public static string Prop_Typeid = "Typeid";
		public static string Prop_Type = "Type";
		public static string Prop_ReleaseState = "ReleaseState";
		public static string Prop_ReleaseUser = "ReleaseUser";
		public static string Prop_ReleaseTime = "ReleaseTime";
		public static string Prop_ReadUser = "ReadUser";
		public static string Prop_ReadCount = "ReadCount";
		public static string Prop_ReadState = "ReadState";
		public static string Prop_IsEnforcementUp = "IsEnforcementUp";
		public static string Prop_FileID = "FileID";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_ReleaseId = "ReleaseId";
		public static string Prop_Period = "Period";
		public static string Prop_ReleDepartment = "ReleDepartment";
		public static string Prop_ImgPath = "ImgPath";
		public static string Prop_RemindDays = "RemindDays";

		#endregion

		#region Private_Variables

		private string _id;
		private string _title;
		private string _content;
		private string _importance;
		private string _typeid;
		private string _type;
		private string _releaseState;
		private string _releaseUser;
		private string _releaseTime;
		private string _readUser;
		private int? _readCount;
		private string _readState;
		private string _isEnforcementUp;
		private string _fileID;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _releaseId;
		private DateTime? _period;
		private string _releDepartment;
		private string _imgPath;
		private int? _remindDays;


		#endregion

		#region Constructors

		public MessageInfo()
		{
		}

		public MessageInfo(
			string p_id,
			string p_title,
			string p_content,
			string p_importance,
			string p_typeid,
			string p_type,
			string p_releaseState,
			string p_releaseUser,
			string p_releaseTime,
			string p_readUser,
			int? p_readCount,
			string p_readState,
			string p_isEnforcementUp,
			string p_fileID,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_releaseId,
			DateTime? p_period,
			string p_releDepartment,
			string p_imgPath,
			int? p_remindDays)
		{
			_id = p_id;
			_title = p_title;
			_content = p_content;
			_importance = p_importance;
			_typeid = p_typeid;
			_type = p_type;
			_releaseState = p_releaseState;
			_releaseUser = p_releaseUser;
			_releaseTime = p_releaseTime;
			_readUser = p_readUser;
			_readCount = p_readCount;
			_readState = p_readState;
			_isEnforcementUp = p_isEnforcementUp;
			_fileID = p_fileID;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_releaseId = p_releaseId;
			_period = p_period;
			_releDepartment = p_releDepartment;
			_imgPath = p_imgPath;
			_remindDays = p_remindDays;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(MessageInfo.Prop_Title, oldValue, value);
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
					RaisePropertyChanged(MessageInfo.Prop_Content, oldValue, value);
				}
			}

		}

		[Property("Importance", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string Importance
		{
			get { return _importance; }
			set
			{
				if ((_importance == null) || (value == null) || (!value.Equals(_importance)))
				{
                    object oldValue = _importance;
					_importance = value;
					RaisePropertyChanged(MessageInfo.Prop_Importance, oldValue, value);
				}
			}

		}

		[Property("Typeid", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string Typeid
		{
			get { return _typeid; }
			set
			{
				if ((_typeid == null) || (value == null) || (!value.Equals(_typeid)))
				{
                    object oldValue = _typeid;
					_typeid = value;
					RaisePropertyChanged(MessageInfo.Prop_Typeid, oldValue, value);
				}
			}

		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 30)]
		public string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(MessageInfo.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("ReleaseState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string ReleaseState
		{
			get { return _releaseState; }
			set
			{
				if ((_releaseState == null) || (value == null) || (!value.Equals(_releaseState)))
				{
                    object oldValue = _releaseState;
					_releaseState = value;
					RaisePropertyChanged(MessageInfo.Prop_ReleaseState, oldValue, value);
				}
			}

		}

		[Property("ReleaseUser", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string ReleaseUser
		{
			get { return _releaseUser; }
			set
			{
				if ((_releaseUser == null) || (value == null) || (!value.Equals(_releaseUser)))
				{
                    object oldValue = _releaseUser;
					_releaseUser = value;
					RaisePropertyChanged(MessageInfo.Prop_ReleaseUser, oldValue, value);
				}
			}

		}

		[Property("ReleaseTime", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string ReleaseTime
		{
			get { return _releaseTime; }
			set
			{
				if ((_releaseTime == null) || (value == null) || (!value.Equals(_releaseTime)))
				{
                    object oldValue = _releaseTime;
					_releaseTime = value;
					RaisePropertyChanged(MessageInfo.Prop_ReleaseTime, oldValue, value);
				}
			}

		}

		[Property("ReadUser", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string ReadUser
		{
			get { return _readUser; }
			set
			{
				if ((_readUser == null) || (value == null) || (!value.Equals(_readUser)))
				{
                    object oldValue = _readUser;
					_readUser = value;
					RaisePropertyChanged(MessageInfo.Prop_ReadUser, oldValue, value);
				}
			}

		}

		[Property("ReadCount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? ReadCount
		{
			get { return _readCount; }
			set
			{
				if (value != _readCount)
				{
                    object oldValue = _readCount;
					_readCount = value;
					RaisePropertyChanged(MessageInfo.Prop_ReadCount, oldValue, value);
				}
			}

		}

		[Property("ReadState", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string ReadState
		{
			get { return _readState; }
			set
			{
				if ((_readState == null) || (value == null) || (!value.Equals(_readState)))
				{
                    object oldValue = _readState;
					_readState = value;
					RaisePropertyChanged(MessageInfo.Prop_ReadState, oldValue, value);
				}
			}

		}

		[Property("IsEnforcementUp", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 10)]
		public string IsEnforcementUp
		{
			get { return _isEnforcementUp; }
			set
			{
				if ((_isEnforcementUp == null) || (value == null) || (!value.Equals(_isEnforcementUp)))
				{
                    object oldValue = _isEnforcementUp;
					_isEnforcementUp = value;
					RaisePropertyChanged(MessageInfo.Prop_IsEnforcementUp, oldValue, value);
				}
			}

		}

		[Property("FileID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 2000)]
		public string FileID
		{
			get { return _fileID; }
			set
			{
				if ((_fileID == null) || (value == null) || (!value.Equals(_fileID)))
				{
                    object oldValue = _fileID;
					_fileID = value;
					RaisePropertyChanged(MessageInfo.Prop_FileID, oldValue, value);
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
					RaisePropertyChanged(MessageInfo.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(MessageInfo.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(MessageInfo.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("ReleaseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ReleaseId
		{
			get { return _releaseId; }
			set
			{
				if ((_releaseId == null) || (value == null) || (!value.Equals(_releaseId)))
				{
                    object oldValue = _releaseId;
					_releaseId = value;
					RaisePropertyChanged(MessageInfo.Prop_ReleaseId, oldValue, value);
				}
			}

		}

		[Property("Period", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? Period
		{
			get { return _period; }
			set
			{
				if (value != _period)
				{
                    object oldValue = _period;
					_period = value;
					RaisePropertyChanged(MessageInfo.Prop_Period, oldValue, value);
				}
			}

		}

		[Property("ReleDepartment", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string ReleDepartment
		{
			get { return _releDepartment; }
			set
			{
				if ((_releDepartment == null) || (value == null) || (!value.Equals(_releDepartment)))
				{
                    object oldValue = _releDepartment;
					_releDepartment = value;
					RaisePropertyChanged(MessageInfo.Prop_ReleDepartment, oldValue, value);
				}
			}

		}

		[Property("ImgPath", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string ImgPath
		{
			get { return _imgPath; }
			set
			{
				if ((_imgPath == null) || (value == null) || (!value.Equals(_imgPath)))
				{
                    object oldValue = _imgPath;
					_imgPath = value;
					RaisePropertyChanged(MessageInfo.Prop_ImgPath, oldValue, value);
				}
			}

		}

		[Property("RemindDays", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? RemindDays
		{
			get { return _remindDays; }
			set
			{
				if (value != _remindDays)
				{
                    object oldValue = _remindDays;
					_remindDays = value;
					RaisePropertyChanged(MessageInfo.Prop_RemindDays, oldValue, value);
				}
			}

		}

		#endregion
	} // MessageInfo
}

