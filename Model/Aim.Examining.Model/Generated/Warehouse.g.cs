// Business class Warehouse generated from Warehouse
// Creator: Ray
// Created Date: [2012-02-09]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Warehouse")]
	public partial class Warehouse : ExamModelBase<Warehouse>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_ManagerId = "ManagerId";
		public static string Prop_ManagerName = "ManagerName";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_Remark = "Remark";
		public static string Prop_WareSeat = "WareSeat";
		public static string Prop_Images = "Images";

		#endregion

		#region Private_Variables

		private string _id;
		private string _code;
		private string _name;
		private string _managerId;
		private string _managerName;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _remark;
		private string _wareSeat;
		private string _images;


		#endregion

		#region Constructors

		public Warehouse()
		{
		}

		public Warehouse(
			string p_id,
			string p_code,
			string p_name,
			string p_managerId,
			string p_managerName,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_remark,
			string p_wareSeat,
			string p_images)
		{
			_id = p_id;
			_code = p_code;
			_name = p_name;
			_managerId = p_managerId;
			_managerName = p_managerName;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_remark = p_remark;
			_wareSeat = p_wareSeat;
			_images = p_images;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(Warehouse.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(Warehouse.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("ManagerId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ManagerId
		{
			get { return _managerId; }
			set
			{
				if ((_managerId == null) || (value == null) || (!value.Equals(_managerId)))
				{
                    object oldValue = _managerId;
					_managerId = value;
					RaisePropertyChanged(Warehouse.Prop_ManagerId, oldValue, value);
				}
			}

		}

		[Property("ManagerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string ManagerName
		{
			get { return _managerName; }
			set
			{
				if ((_managerName == null) || (value == null) || (!value.Equals(_managerName)))
				{
                    object oldValue = _managerName;
					_managerName = value;
					RaisePropertyChanged(Warehouse.Prop_ManagerName, oldValue, value);
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
					RaisePropertyChanged(Warehouse.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Warehouse.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Warehouse.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 2000)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(Warehouse.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("WareSeat", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string WareSeat
		{
			get { return _wareSeat; }
			set
			{
				if ((_wareSeat == null) || (value == null) || (!value.Equals(_wareSeat)))
				{
                    object oldValue = _wareSeat;
					_wareSeat = value;
					RaisePropertyChanged(Warehouse.Prop_WareSeat, oldValue, value);
				}
			}

		}

		[Property("Images", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1000)]
		public string Images
		{
			get { return _images; }
			set
			{
				if ((_images == null) || (value == null) || (!value.Equals(_images)))
				{
                    object oldValue = _images;
					_images = value;
					RaisePropertyChanged(Warehouse.Prop_Images, oldValue, value);
				}
			}

		}

		#endregion
	} // Warehouse
}

