// Business class MExamStardard generated from MExamStardard
// Creator: Ray
// Created Date: [2010-11-25]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("MExamStardard")]
	public partial class MExamStardard : ExamModelBase<MExamStardard>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Value = "Value";
		public static string Prop_Text = "Text";

		#endregion

		#region Private_Variables

		private string _id;
		private string _value;
		private string _text;


		#endregion

		#region Constructors

		public MExamStardard()
		{
		}

		public MExamStardard(
			string p_id,
			string p_value,
			string p_text)
		{
			_id = p_id;
			_value = p_value;
			_text = p_text;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Value", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public string Value
		{
			get { return _value; }
			set
			{
				if ((_value == null) || (value == null) || (!value.Equals(_value)))
				{
                    object oldValue = _value;
					_value = value;
					RaisePropertyChanged(MExamStardard.Prop_Value, oldValue, value);
				}
			}

		}

		[Property("Text", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Text
		{
			get { return _text; }
			set
			{
				if ((_text == null) || (value == null) || (!value.Equals(_text)))
				{
                    object oldValue = _text;
					_text = value;
					RaisePropertyChanged(MExamStardard.Prop_Text, oldValue, value);
				}
			}

		}

		#endregion
	} // MExamStardard
}

