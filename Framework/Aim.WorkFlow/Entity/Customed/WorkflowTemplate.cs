// Business class WorkflowTemplate generated from WorkflowTemplate
// Creator: Ray
// Created Date: [2010-11-19]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;

namespace Aim.WorkFlow
{
	[ActiveRecord("WorkflowTemplate")]
	public partial class WorkflowTemplate : EntityBase<WorkflowTemplate>
	{
		#region Property_Names

		public static string Prop_ID = "ID";
        public static string Prop_Code = "Code";
		public static string Prop_TemplateName = "TemplateName";
		public static string Prop_Category = "Category";
		public static string Prop_Description = "Description";
		public static string Prop_Version = "Version";
		public static string Prop_XAML = "XAML";
		public static string Prop_Config = "Config";
		public static string Prop_Creator = "Creator";
		public static string Prop_LastReviser = "LastReviser";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_UpdateTime = "UpdateTime";
		public static string Prop_Status = "Status";

		#endregion

		#region Private_Variables

		private string _id;
        private string _code;
		private string _templateName;
		private string _category;
		private string _description;
		private string _version;
		private string _xAML;
		private string _config;
		private string _creator;
		private string _lastReviser;
		private DateTime _createTime;
		private DateTime _updateTime;
		private int _status;


		#endregion

		#region Constructors

		public WorkflowTemplate()
		{
		}

		public WorkflowTemplate(
			string p_id,
			string p_templateName,
			string p_category,
			string p_description,
			string p_version,
			string p_xAML,
			string p_config,
			string p_creator,
			string p_lastReviser,
			DateTime p_createTime,
			DateTime p_updateTime,
			int p_status)
		{
			_id = p_id;
			_templateName = p_templateName;
			_category = p_category;
			_description = p_description;
			_version = p_version;
			_xAML = p_xAML;
			_config = p_config;
			_creator = p_creator;
			_lastReviser = p_lastReviser;
			_createTime = p_createTime;
			_updateTime = p_updateTime;
			_status = p_status;
		}

		#endregion

		#region Properties

		[PrimaryKey("ID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string ID
		{
			get { return _id; }
		}

        [Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = false, Length = 100)]
        public string Code
        {
            get { return _code; }
            set
            {
                if ((_templateName == null) || (value == null) || (!value.Equals(_templateName)))
                {
                    object oldValue = _code;
                    _code = value;
                    RaisePropertyChanged(WorkflowTemplate.Prop_Code, oldValue, value);
                }
            }
        }

		[Property("TemplateName", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 100)]
		public string TemplateName
		{
			get { return _templateName; }
			set
			{
				if ((_templateName == null) || (value == null) || (!value.Equals(_templateName)))
				{
                    object oldValue = _templateName;
					_templateName = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_TemplateName, oldValue, value);
				}
			}
		}

		[Property("Category", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Category
		{
			get { return _category; }
			set
			{
				if ((_category == null) || (value == null) || (!value.Equals(_category)))
				{
                    object oldValue = _category;
					_category = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_Category, oldValue, value);
				}
			}
		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_Description, oldValue, value);
				}
			}
		}

		[Property("Version", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
		public string Version
		{
			get { return _version; }
			set
			{
				if ((_version == null) || (value == null) || (!value.Equals(_version)))
				{
                    object oldValue = _version;
					_version = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_Version, oldValue, value);
				}
			}
		}

		[Property("XAML", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, ColumnType = "StringClob")]
		public string XAML
		{
			get { return _xAML; }
			set
			{
				if ((_xAML == null) || (value == null) || (!value.Equals(_xAML)))
				{
                    object oldValue = _xAML;
					_xAML = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_XAML, oldValue, value);
				}
			}
		}

		[Property("Config", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Config
		{
			get { return _config; }
			set
			{
				if ((_config == null) || (value == null) || (!value.Equals(_config)))
				{
                    object oldValue = _config;
					_config = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_Config, oldValue, value);
				}
			}
		}

		[Property("Creator", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 100)]
		public string Creator
		{
			get { return _creator; }
			set
			{
				if ((_creator == null) || (value == null) || (!value.Equals(_creator)))
				{
                    object oldValue = _creator;
					_creator = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_Creator, oldValue, value);
				}
			}
		}

		[Property("LastReviser", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 100)]
		public string LastReviser
		{
			get { return _lastReviser; }
			set
			{
				if ((_lastReviser == null) || (value == null) || (!value.Equals(_lastReviser)))
				{
                    object oldValue = _lastReviser;
					_lastReviser = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_LastReviser, oldValue, value);
				}
			}
		}

		[Property("CreateTime", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public DateTime CreateTime
		{
			get { return _createTime; }
			set
			{
				if (value != _createTime)
				{
                    object oldValue = _createTime;
					_createTime = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_CreateTime, oldValue, value);
				}
			}
		}

		[Property("UpdateTime", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public DateTime UpdateTime
		{
			get { return _updateTime; }
			set
			{
				if (value != _updateTime)
				{
                    object oldValue = _updateTime;
					_updateTime = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_UpdateTime, oldValue, value);
				}
			}
		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public int Status
		{
			get { return _status; }
			set
			{
				if (value != _status)
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(WorkflowTemplate.Prop_Status, oldValue, value);
				}
			}
		}

		#endregion
	} // WorkflowTemplate
}

