// Business class PayBill generated from PayBill
// Creator: Ray
// Created Date: [2015-01-27]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("PayBill")]
	public partial class PayBill : ExamModelBase<PayBill>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_PayBillNo = "PayBillNo";
		public static string Prop_PAmount = "PAmount";
		public static string Prop_SupplierId = "SupplierId";
		public static string Prop_SupplierName = "SupplierName";
		public static string Prop_State = "State";
		public static string Prop_ExamineResult = "ExamineResult";
		public static string Prop_WorkFlowState = "WorkFlowState";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_ActuallyPayAmount = "ActuallyPayAmount";
		public static string Prop_Remark = "Remark";
		public static string Prop_DiscountAmount = "DiscountAmount";
		public static string Prop_Symbo = "Symbo";

		#endregion

		#region Private_Variables

		private string _id;
		private string _payBillNo;
		private System.Decimal? _pAmount;
		private string _supplierId;
		private string _supplierName;
		private string _state;
		private string _examineResult;
		private string _workFlowState;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private System.Decimal? _actuallyPayAmount;
		private string _remark;
		private System.Decimal? _discountAmount;
		private string _symbo;


		#endregion

		#region Constructors

		public PayBill()
		{
		}

		public PayBill(
			string p_id,
			string p_payBillNo,
			System.Decimal? p_pAmount,
			string p_supplierId,
			string p_supplierName,
			string p_state,
			string p_examineResult,
			string p_workFlowState,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			System.Decimal? p_actuallyPayAmount,
			string p_remark,
			System.Decimal? p_discountAmount,
			string p_symbo)
		{
			_id = p_id;
			_payBillNo = p_payBillNo;
			_pAmount = p_pAmount;
			_supplierId = p_supplierId;
			_supplierName = p_supplierName;
			_state = p_state;
			_examineResult = p_examineResult;
			_workFlowState = p_workFlowState;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_actuallyPayAmount = p_actuallyPayAmount;
			_remark = p_remark;
			_discountAmount = p_discountAmount;
			_symbo = p_symbo;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("PayBillNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayBillNo
		{
			get { return _payBillNo; }
			set
			{
				if ((_payBillNo == null) || (value == null) || (!value.Equals(_payBillNo)))
				{
                    object oldValue = _payBillNo;
					_payBillNo = value;
					RaisePropertyChanged(PayBill.Prop_PayBillNo, oldValue, value);
				}
			}

		}

		[Property("PAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? PAmount
		{
			get { return _pAmount; }
			set
			{
				if (value != _pAmount)
				{
                    object oldValue = _pAmount;
					_pAmount = value;
					RaisePropertyChanged(PayBill.Prop_PAmount, oldValue, value);
				}
			}

		}

		[Property("SupplierId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string SupplierId
		{
			get { return _supplierId; }
			set
			{
				if ((_supplierId == null) || (value == null) || (!value.Equals(_supplierId)))
				{
                    object oldValue = _supplierId;
					_supplierId = value;
					RaisePropertyChanged(PayBill.Prop_SupplierId, oldValue, value);
				}
			}

		}

		[Property("SupplierName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SupplierName
		{
			get { return _supplierName; }
			set
			{
				if ((_supplierName == null) || (value == null) || (!value.Equals(_supplierName)))
				{
                    object oldValue = _supplierName;
					_supplierName = value;
					RaisePropertyChanged(PayBill.Prop_SupplierName, oldValue, value);
				}
			}

		}

		[Property("State", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string State
		{
			get { return _state; }
			set
			{
				if ((_state == null) || (value == null) || (!value.Equals(_state)))
				{
                    object oldValue = _state;
					_state = value;
					RaisePropertyChanged(PayBill.Prop_State, oldValue, value);
				}
			}

		}

		[Property("ExamineResult", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ExamineResult
		{
			get { return _examineResult; }
			set
			{
				if ((_examineResult == null) || (value == null) || (!value.Equals(_examineResult)))
				{
                    object oldValue = _examineResult;
					_examineResult = value;
					RaisePropertyChanged(PayBill.Prop_ExamineResult, oldValue, value);
				}
			}

		}

		[Property("WorkFlowState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string WorkFlowState
		{
			get { return _workFlowState; }
			set
			{
				if ((_workFlowState == null) || (value == null) || (!value.Equals(_workFlowState)))
				{
                    object oldValue = _workFlowState;
					_workFlowState = value;
					RaisePropertyChanged(PayBill.Prop_WorkFlowState, oldValue, value);
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
					RaisePropertyChanged(PayBill.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(PayBill.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(PayBill.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("ActuallyPayAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? ActuallyPayAmount
		{
			get { return _actuallyPayAmount; }
			set
			{
				if (value != _actuallyPayAmount)
				{
                    object oldValue = _actuallyPayAmount;
					_actuallyPayAmount = value;
					RaisePropertyChanged(PayBill.Prop_ActuallyPayAmount, oldValue, value);
				}
			}

		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
                    object oldValue = _remark;
					_remark = value;
					RaisePropertyChanged(PayBill.Prop_Remark, oldValue, value);
				}
			}

		}

		[Property("DiscountAmount", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? DiscountAmount
		{
			get { return _discountAmount; }
			set
			{
				if (value != _discountAmount)
				{
                    object oldValue = _discountAmount;
					_discountAmount = value;
					RaisePropertyChanged(PayBill.Prop_DiscountAmount, oldValue, value);
				}
			}

		}

		[Property("Symbo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Symbo
		{
			get { return _symbo; }
			set
			{
				if ((_symbo == null) || (value == null) || (!value.Equals(_symbo)))
				{
                    object oldValue = _symbo;
					_symbo = value;
					RaisePropertyChanged(PayBill.Prop_Symbo, oldValue, value);
				}
			}

		}

		#endregion
	} // PayBill
}

