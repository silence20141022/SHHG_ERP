// Business class ContractMain generated from ContractMain
// Creator: Ray
// Created Date: [2011-10-27]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("ContractMain")]
	public partial class ContractMain : ExamModelBase<ContractMain>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_type = "type";
		public static string Prop_content = "content";
		public static string Prop_userid = "userid";
		public static string Prop_username = "username";
		public static string Prop_createdate = "createdate";
		public static string Prop_FileID = "FileID";

		#endregion

		#region Private_Variables

		private string _id;
		private string _type;
		private string _content;
		private string _userid;
		private string _username;
		private DateTime? _createdate;
		private string _fileID;


		#endregion

		#region Constructors

		public ContractMain()
		{
		}

		public ContractMain(
			string p_id,
			string p_type,
			string p_content,
			string p_userid,
			string p_username,
			DateTime? p_createdate,
			string p_fileID)
		{
			_id = p_id;
			_type = p_type;
			_content = p_content;
			_userid = p_userid;
			_username = p_username;
			_createdate = p_createdate;
			_fileID = p_fileID;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(ContractMain.Prop_type, oldValue, value);
				}
			}

		}

		[Property("content", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 2000)]
		public string content
		{
			get { return _content; }
			set
			{
				if ((_content == null) || (value == null) || (!value.Equals(_content)))
				{
                    object oldValue = _content;
					_content = value;
					RaisePropertyChanged(ContractMain.Prop_content, oldValue, value);
				}
			}

		}

		[Property("userid", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string userid
		{
			get { return _userid; }
			set
			{
				if ((_userid == null) || (value == null) || (!value.Equals(_userid)))
				{
                    object oldValue = _userid;
					_userid = value;
					RaisePropertyChanged(ContractMain.Prop_userid, oldValue, value);
				}
			}

		}

		[Property("username", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string username
		{
			get { return _username; }
			set
			{
				if ((_username == null) || (value == null) || (!value.Equals(_username)))
				{
                    object oldValue = _username;
					_username = value;
					RaisePropertyChanged(ContractMain.Prop_username, oldValue, value);
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
					RaisePropertyChanged(ContractMain.Prop_createdate, oldValue, value);
				}
			}

		}

		[Property("FileID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string FileID
		{
			get { return _fileID; }
			set
			{
				if ((_fileID == null) || (value == null) || (!value.Equals(_fileID)))
				{
                    object oldValue = _fileID;
					_fileID = value;
					RaisePropertyChanged(ContractMain.Prop_FileID, oldValue, value);
				}
			}

		}

		#endregion
	} // ContractMain
}

