// Business class InAndOutDetail generated from InAndOutDetail
// Creator: Ray
// Created Date: [2012-03-05]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("InAndOutDetail")]
	public partial class InAndOutDetail : ExamModelBase<InAndOutDetail>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_InAndOutId = "InAndOutId";
		public static string Prop_ProductId = "ProductId";
		public static string Prop_ShouldOutQuantity = "ShouldOutQuantity";
		public static string Prop_IOQuantity = "IOQuantity";
		public static string Prop_HaveOut = "HaveOut";

		#endregion

		#region Private_Variables

		private string _id;
		private string _inAndOutId;
		private string _productId;
		private int? _shouldOutQuantity;
		private int? _iOQuantity;
		private int? _haveOut;


		#endregion

		#region Constructors

		public InAndOutDetail()
		{
		}

		public InAndOutDetail(
			string p_id,
			string p_inAndOutId,
			string p_productId,
			int? p_shouldOutQuantity,
			int? p_iOQuantity,
			int? p_haveOut)
		{
			_id = p_id;
			_inAndOutId = p_inAndOutId;
			_productId = p_productId;
			_shouldOutQuantity = p_shouldOutQuantity;
			_iOQuantity = p_iOQuantity;
			_haveOut = p_haveOut;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("InAndOutId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InAndOutId
		{
			get { return _inAndOutId; }
			set
			{
				if ((_inAndOutId == null) || (value == null) || (!value.Equals(_inAndOutId)))
				{
                    object oldValue = _inAndOutId;
					_inAndOutId = value;
					RaisePropertyChanged(InAndOutDetail.Prop_InAndOutId, oldValue, value);
				}
			}

		}

		[Property("ProductId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ProductId
		{
			get { return _productId; }
			set
			{
				if ((_productId == null) || (value == null) || (!value.Equals(_productId)))
				{
                    object oldValue = _productId;
					_productId = value;
					RaisePropertyChanged(InAndOutDetail.Prop_ProductId, oldValue, value);
				}
			}

		}

		[Property("ShouldOutQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? ShouldOutQuantity
		{
			get { return _shouldOutQuantity; }
			set
			{
				if (value != _shouldOutQuantity)
				{
                    object oldValue = _shouldOutQuantity;
					_shouldOutQuantity = value;
					RaisePropertyChanged(InAndOutDetail.Prop_ShouldOutQuantity, oldValue, value);
				}
			}

		}

		[Property("IOQuantity", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? IOQuantity
		{
			get { return _iOQuantity; }
			set
			{
				if (value != _iOQuantity)
				{
                    object oldValue = _iOQuantity;
					_iOQuantity = value;
					RaisePropertyChanged(InAndOutDetail.Prop_IOQuantity, oldValue, value);
				}
			}

		}

		[Property("HaveOut", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? HaveOut
		{
			get { return _haveOut; }
			set
			{
				if (value != _haveOut)
				{
                    object oldValue = _haveOut;
					_haveOut = value;
					RaisePropertyChanged(InAndOutDetail.Prop_HaveOut, oldValue, value);
				}
			}

		}

		#endregion
	} // InAndOutDetail
}

