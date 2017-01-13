// Business class CollectionToUser generated from CollectionToUser
// Creator: Ray
// Created Date: [2011-11-04]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("CollectionToUser")]
	public partial class CollectionToUser : ExamModelBase<CollectionToUser>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_UserId = "UserId";
		public static string Prop_MsgId = "MsgId";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";

		#endregion

		#region Private_Variables

		private string _id;
		private string _userId;
		private string _msgId;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;


		#endregion

		#region Constructors

		public CollectionToUser()
		{
		}

		public CollectionToUser(
			string p_id,
			string p_userId,
			string p_msgId,
			string p_createId,
			string p_createName,
			DateTime? p_createTime)
		{
			_id = p_id;
			_userId = p_userId;
			_msgId = p_msgId;
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
					RaisePropertyChanged(CollectionToUser.Prop_UserId, oldValue, value);
				}
			}

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
					RaisePropertyChanged(CollectionToUser.Prop_MsgId, oldValue, value);
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
					RaisePropertyChanged(CollectionToUser.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(CollectionToUser.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(CollectionToUser.Prop_CreateTime, oldValue, value);
				}
			}

		}

		#endregion
	} // CollectionToUser
}

