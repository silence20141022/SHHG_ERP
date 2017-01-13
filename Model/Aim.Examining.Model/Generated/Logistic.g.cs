// Business class Logistic generated from Logistics
// Creator: Ray
// Created Date: [2012-05-15]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;
	
namespace Aim.Examining.Model
{
	[ActiveRecord("Logistics")]
	public partial class Logistic : ExamModelBase<Logistic>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_DeliveryId = "DeliveryId";
		public static string Prop_DeliveryNumber = "DeliveryNumber";
		public static string Prop_Number = "Number";
		public static string Prop_Name = "Name";
		public static string Prop_Price = "Price";
		public static string Prop_Weight = "Weight";
		public static string Prop_Child = "Child";
		public static string Prop_Remark = "Remark";
		public static string Prop_CreateId = "CreateId";
		public static string Prop_CreateName = "CreateName";
		public static string Prop_CreateTime = "CreateTime";
		public static string Prop_CustomerId = "CustomerId";
		public static string Prop_CustomerName = "CustomerName";
		public static string Prop_Receiver = "Receiver";
		public static string Prop_Tel = "Tel";
		public static string Prop_MobilePhone = "MobilePhone";
		public static string Prop_PayType = "PayType";
		public static string Prop_Insured = "Insured";
		public static string Prop_Delivery = "Delivery";
		public static string Prop_Total = "Total";
		public static string Prop_Address = "Address";
		public static string Prop_PayState = "PayState";
		public static string Prop_Volume = "Volume";
		public static string Prop_SendDate = "SendDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _deliveryId;
		private string _deliveryNumber;
		private string _number;
		private string _name;
		private System.Decimal? _price;
		private System.Decimal? _weight;
		private string _child;
		private string _remark;
		private string _createId;
		private string _createName;
		private DateTime? _createTime;
		private string _customerId;
		private string _customerName;
		private string _receiver;
		private string _tel;
		private string _mobilePhone;
		private string _payType;
		private System.Decimal? _insured;
		private System.Decimal? _delivery;
		private System.Decimal? _total;
		private string _address;
		private string _payState;
		private System.Decimal? _volume;
		private DateTime? _sendDate;


		#endregion

		#region Constructors

		public Logistic()
		{
		}

		public Logistic(
			string p_id,
			string p_deliveryId,
			string p_deliveryNumber,
			string p_number,
			string p_name,
			System.Decimal? p_price,
			System.Decimal? p_weight,
			string p_child,
			string p_remark,
			string p_createId,
			string p_createName,
			DateTime? p_createTime,
			string p_customerId,
			string p_customerName,
			string p_receiver,
			string p_tel,
			string p_mobilePhone,
			string p_payType,
			System.Decimal? p_insured,
			System.Decimal? p_delivery,
			System.Decimal? p_total,
			string p_address,
			string p_payState,
			System.Decimal? p_volume,
			DateTime? p_sendDate)
		{
			_id = p_id;
			_deliveryId = p_deliveryId;
			_deliveryNumber = p_deliveryNumber;
			_number = p_number;
			_name = p_name;
			_price = p_price;
			_weight = p_weight;
			_child = p_child;
			_remark = p_remark;
			_createId = p_createId;
			_createName = p_createName;
			_createTime = p_createTime;
			_customerId = p_customerId;
			_customerName = p_customerName;
			_receiver = p_receiver;
			_tel = p_tel;
			_mobilePhone = p_mobilePhone;
			_payType = p_payType;
			_insured = p_insured;
			_delivery = p_delivery;
			_total = p_total;
			_address = p_address;
			_payState = p_payState;
			_volume = p_volume;
			_sendDate = p_sendDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("DeliveryId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 360)]
		public string DeliveryId
		{
			get { return _deliveryId; }
			set
			{
				if ((_deliveryId == null) || (value == null) || (!value.Equals(_deliveryId)))
				{
                    object oldValue = _deliveryId;
					_deliveryId = value;
					RaisePropertyChanged(Logistic.Prop_DeliveryId, oldValue, value);
				}
			}

		}

		[Property("DeliveryNumber", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string DeliveryNumber
		{
			get { return _deliveryNumber; }
			set
			{
				if ((_deliveryNumber == null) || (value == null) || (!value.Equals(_deliveryNumber)))
				{
                    object oldValue = _deliveryNumber;
					_deliveryNumber = value;
					RaisePropertyChanged(Logistic.Prop_DeliveryNumber, oldValue, value);
				}
			}

		}

		[Property("Number", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Number
		{
			get { return _number; }
			set
			{
				if ((_number == null) || (value == null) || (!value.Equals(_number)))
				{
                    object oldValue = _number;
					_number = value;
					RaisePropertyChanged(Logistic.Prop_Number, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(Logistic.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Price", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Price
		{
			get { return _price; }
			set
			{
				if (value != _price)
				{
                    object oldValue = _price;
					_price = value;
					RaisePropertyChanged(Logistic.Prop_Price, oldValue, value);
				}
			}

		}

		[Property("Weight", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Weight
		{
			get { return _weight; }
			set
			{
				if (value != _weight)
				{
                    object oldValue = _weight;
					_weight = value;
					RaisePropertyChanged(Logistic.Prop_Weight, oldValue, value);
				}
			}

		}

		[Property("Child", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Child
		{
			get { return _child; }
			set
			{
				if ((_child == null) || (value == null) || (!value.Equals(_child)))
				{
                    object oldValue = _child;
					_child = value;
					RaisePropertyChanged(Logistic.Prop_Child, oldValue, value);
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
					RaisePropertyChanged(Logistic.Prop_Remark, oldValue, value);
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
					RaisePropertyChanged(Logistic.Prop_CreateId, oldValue, value);
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
					RaisePropertyChanged(Logistic.Prop_CreateName, oldValue, value);
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
					RaisePropertyChanged(Logistic.Prop_CreateTime, oldValue, value);
				}
			}

		}

		[Property("CustomerId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CustomerId
		{
			get { return _customerId; }
			set
			{
				if ((_customerId == null) || (value == null) || (!value.Equals(_customerId)))
				{
                    object oldValue = _customerId;
					_customerId = value;
					RaisePropertyChanged(Logistic.Prop_CustomerId, oldValue, value);
				}
			}

		}

		[Property("CustomerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string CustomerName
		{
			get { return _customerName; }
			set
			{
				if ((_customerName == null) || (value == null) || (!value.Equals(_customerName)))
				{
                    object oldValue = _customerName;
					_customerName = value;
					RaisePropertyChanged(Logistic.Prop_CustomerName, oldValue, value);
				}
			}

		}

		[Property("Receiver", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Receiver
		{
			get { return _receiver; }
			set
			{
				if ((_receiver == null) || (value == null) || (!value.Equals(_receiver)))
				{
                    object oldValue = _receiver;
					_receiver = value;
					RaisePropertyChanged(Logistic.Prop_Receiver, oldValue, value);
				}
			}

		}

		[Property("Tel", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Tel
		{
			get { return _tel; }
			set
			{
				if ((_tel == null) || (value == null) || (!value.Equals(_tel)))
				{
                    object oldValue = _tel;
					_tel = value;
					RaisePropertyChanged(Logistic.Prop_Tel, oldValue, value);
				}
			}

		}

		[Property("MobilePhone", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string MobilePhone
		{
			get { return _mobilePhone; }
			set
			{
				if ((_mobilePhone == null) || (value == null) || (!value.Equals(_mobilePhone)))
				{
                    object oldValue = _mobilePhone;
					_mobilePhone = value;
					RaisePropertyChanged(Logistic.Prop_MobilePhone, oldValue, value);
				}
			}

		}

		[Property("PayType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayType
		{
			get { return _payType; }
			set
			{
				if ((_payType == null) || (value == null) || (!value.Equals(_payType)))
				{
                    object oldValue = _payType;
					_payType = value;
					RaisePropertyChanged(Logistic.Prop_PayType, oldValue, value);
				}
			}

		}

		[Property("Insured", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Insured
		{
			get { return _insured; }
			set
			{
				if (value != _insured)
				{
                    object oldValue = _insured;
					_insured = value;
					RaisePropertyChanged(Logistic.Prop_Insured, oldValue, value);
				}
			}

		}

		[Property("Delivery", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Delivery
		{
			get { return _delivery; }
			set
			{
				if (value != _delivery)
				{
                    object oldValue = _delivery;
					_delivery = value;
					RaisePropertyChanged(Logistic.Prop_Delivery, oldValue, value);
				}
			}

		}

		[Property("Total", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Total
		{
			get { return _total; }
			set
			{
				if (value != _total)
				{
                    object oldValue = _total;
					_total = value;
					RaisePropertyChanged(Logistic.Prop_Total, oldValue, value);
				}
			}

		}

		[Property("Address", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Address
		{
			get { return _address; }
			set
			{
				if ((_address == null) || (value == null) || (!value.Equals(_address)))
				{
                    object oldValue = _address;
					_address = value;
					RaisePropertyChanged(Logistic.Prop_Address, oldValue, value);
				}
			}

		}

		[Property("PayState", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string PayState
		{
			get { return _payState; }
			set
			{
				if ((_payState == null) || (value == null) || (!value.Equals(_payState)))
				{
                    object oldValue = _payState;
					_payState = value;
					RaisePropertyChanged(Logistic.Prop_PayState, oldValue, value);
				}
			}

		}

		[Property("Volume", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Volume
		{
			get { return _volume; }
			set
			{
				if (value != _volume)
				{
                    object oldValue = _volume;
					_volume = value;
					RaisePropertyChanged(Logistic.Prop_Volume, oldValue, value);
				}
			}

		}

		[Property("SendDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? SendDate
		{
			get { return _sendDate; }
			set
			{
				if (value != _sendDate)
				{
                    object oldValue = _sendDate;
					_sendDate = value;
					RaisePropertyChanged(Logistic.Prop_SendDate, oldValue, value);
				}
			}

		}

		#endregion
	} // Logistic
}

