// Business class Skin generated from Skin
// Creator: Ray
// Created Date: [2012-05-13]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Skin")]
	public partial class Skin : ExamModelBase<Skin>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_SkinNo = "SkinNo";
		public static string Prop_ModelNo = "ModelNo";
		public static string Prop_Quantity = "Quantity";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_InWarehouseId = "InWarehouseId";
		public static string Prop_SkinState = "SkinState";

		#endregion

		#region Private_Variables

		private string _id;
		private string _skinNo;
		private string _modelNo;
		private int? _quantity;
		private DateTime? _createTime;
		private string _createId;
		private string _createName;
		private string _inWarehouseId;
		private string _skinState;


		#endregion

		#region Constructors

		public Skin()
		{
		}

		public Skin(
			string p_id,
			string p_skinNo,
			string p_modelNo,
			int? p_quantity,
			DateTime? p_createTime,
			string p_createId,
			string p_createName,
			string p_inWarehouseId,
			string p_skinState)
		{
			_id = p_id;
			_skinNo = p_skinNo;
			_modelNo = p_modelNo;
			_quantity = p_quantity;
			_createTime = p_createTime;
			_createId = p_createId;
			_createName = p_createName;
			_inWarehouseId = p_inWarehouseId;
			_skinState = p_skinState;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("SkinNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SkinNo
		{
			get { return _skinNo; }
			set
			{
				if ((_skinNo == null) || (value == null) || (!value.Equals(_skinNo)))
				{
                    object oldValue = _skinNo;
					_skinNo = value;
					RaisePropertyChanged(Skin.Prop_SkinNo, oldValue, value);
				}
			}

		}

		[Property("ModelNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ModelNo
		{
			get { return _modelNo; }
			set
			{
				if ((_modelNo == null) || (value == null) || (!value.Equals(_modelNo)))
				{
                    object oldValue = _modelNo;
					_modelNo = value;
					RaisePropertyChanged(Skin.Prop_ModelNo, oldValue, value);
				}
			}

		}

		[Property("Quantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Quantity
		{
			get { return _quantity; }
			set
			{
				if (value != _quantity)
				{
                    object oldValue = _quantity;
					_quantity = value;
					RaisePropertyChanged(Skin.Prop_Quantity, oldValue, value);
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
					RaisePropertyChanged(Skin.Prop_CreateTime, oldValue, value);
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
					RaisePropertyChanged(Skin.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Skin.Prop_CreateName, oldValue, value);
				}
			}

		}

		[Property("InWarehouseId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InWarehouseId
		{
			get { return _inWarehouseId; }
			set
			{
				if ((_inWarehouseId == null) || (value == null) || (!value.Equals(_inWarehouseId)))
				{
                    object oldValue = _inWarehouseId;
					_inWarehouseId = value;
					RaisePropertyChanged(Skin.Prop_InWarehouseId, oldValue, value);
				}
			}

		}

		[Property("SkinState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SkinState
		{
			get { return _skinState; }
			set
			{
				if ((_skinState == null) || (value == null) || (!value.Equals(_skinState)))
				{
                    object oldValue = _skinState;
					_skinState = value;
					RaisePropertyChanged(Skin.Prop_SkinState, oldValue, value);
				}
			}

		}

		#endregion
	} // Skin
}

