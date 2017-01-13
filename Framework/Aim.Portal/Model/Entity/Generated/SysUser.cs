// Business class SysUser generated from SysUser
// Creator: Ray
// Created Date: [2010-04-10]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Aim.Data;

namespace Aim.Portal.Model
{
    [ActiveRecord("SysUser")]
    public partial class SysUser : EntityBase<SysUser>, INotifyPropertyChanged
    {

        #region Property_Names

        public static string Prop_UserID = "UserID";
        public static string Prop_LoginName = "LoginName";
        public static string Prop_WorkNo = "WorkNo";
        public static string Prop_Password = "Password";
        public static string Prop_Name = "Name";
        public static string Prop_Email = "Email";
        public static string Prop_Remark = "Remark";
        public static string Prop_Status = "Status";
        public static string Prop_LastLogIP = "LastLogIP";
        public static string Prop_LastLogDate = "LastLogDate";
        public static string Prop_SortIndex = "SortIndex";
        public static string Prop_LastModifiedDate = "LastModifiedDate";
        public static string Prop_CreateDate = "CreateDate";
        public static string Prop_Wage = "Wage";

        #endregion

        #region Private_Variables

        private string _userid;
        private string _loginName;
        private string _workNo;
        private string _password;
        private string _name;
        private string _email;
        private string _remark;
        private int? _status;
        private string _lastLogIP;
        private DateTime? _lastLogDate;
        private int? _sortIndex;
        private DateTime? _lastModifiedDate;
        private DateTime? _createDate;
        private System.Decimal? _wage;

        #endregion

        #region Constructors

        public SysUser()
        {
        }

        public SysUser(
            string p_userid,
            string p_loginName,
            string p_workNo,
            string p_password,
            string p_name,
            string p_email,
            string p_remark,
            byte? p_status,
            string p_lastLogIP,
            DateTime? p_lastLogDate,
            int? p_sortIndex,
            DateTime? p_lastModifiedDate,
            DateTime? p_createDate,
            System.Decimal? p_wage)
        {
            _userid = p_userid;
            _loginName = p_loginName;
            _workNo = p_workNo;
            _password = p_password;
            _name = p_name;
            _email = p_email;
            _remark = p_remark;
            _status = p_status;
            _lastLogIP = p_lastLogIP;
            _lastLogDate = p_lastLogDate;
            _sortIndex = p_sortIndex;
            _lastModifiedDate = p_lastModifiedDate;
            _createDate = p_createDate;
            _wage = p_wage;
        }

        #endregion

        #region Properties

        [PrimaryKey("UserID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(AimIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public string UserID
        {
            get { return _userid; }
        }

        [Property("LoginName", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
        public string LoginName
        {
            get { return _loginName; }
            set
            {
                if ((_loginName == null) || (value == null) || (!value.Equals(_loginName)))
                {
                    _loginName = value;
                    NotifyPropertyChanged(SysUser.Prop_LoginName);
                }
            }
        }

        [Property("WorkNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 16)]
        public string WorkNo
        {
            get { return _workNo; }
            set
            {
                if ((_workNo == null) || (value == null) || (!value.Equals(_workNo)))
                {
                    _workNo = value;
                    NotifyPropertyChanged(SysUser.Prop_WorkNo);
                }
            }
        }

        [JsonIgnore]
        [Property("Password", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 32)]
        public string Password
        {
            get { return _password; }
            set
            {
                if ((_password == null) || (value == null) || (!value.Equals(_password)))
                {
                    _password = value;
                    NotifyPropertyChanged(SysUser.Prop_Password);
                }
            }
        }

        [Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
        public string Name
        {
            get { return _name; }
            set
            {
                if ((_name == null) || (value == null) || (!value.Equals(_name)))
                {
                    _name = value;
                    NotifyPropertyChanged(SysUser.Prop_Name);
                }
            }
        }

        [Property("Email", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
        public string Email
        {
            get { return _email; }
            set
            {
                if ((_email == null) || (value == null) || (!value.Equals(_email)))
                {
                    _email = value;
                    NotifyPropertyChanged(SysUser.Prop_Email);
                }
            }
        }

        [Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
        public string Remark
        {
            get { return _remark; }
            set
            {
                if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
                {
                    _remark = value;
                    NotifyPropertyChanged(SysUser.Prop_Remark);
                }
            }
        }

        [Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public int? Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    NotifyPropertyChanged(SysUser.Prop_Status);
                }
            }
        }

        [Property("LastLogIP", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string LastLogIP
        {
            get { return _lastLogIP; }
            set
            {
                if ((_lastLogIP == null) || (value == null) || (!value.Equals(_lastLogIP)))
                {
                    _lastLogIP = value;
                    NotifyPropertyChanged(SysUser.Prop_LastLogIP);
                }
            }
        }

        [Property("LastLogDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public DateTime? LastLogDate
        {
            get { return _lastLogDate; }
            set
            {
                if (value != _lastLogDate)
                {
                    _lastLogDate = value;
                    NotifyPropertyChanged(SysUser.Prop_LastLogDate);
                }
            }
        }

        [Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public int? SortIndex
        {
            get { return _sortIndex; }
            set
            {
                if (value != _sortIndex)
                {
                    _sortIndex = value;
                    NotifyPropertyChanged(SysUser.Prop_SortIndex);
                }
            }
        }

        [Property("LastModifiedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public DateTime? LastModifiedDate
        {
            get { return _lastModifiedDate; }
            set
            {
                if (value != _lastModifiedDate)
                {
                    _lastModifiedDate = value;
                    NotifyPropertyChanged(SysUser.Prop_LastModifiedDate);
                }
            }
        }

        [Property("CreateDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public DateTime? CreateDate
        {
            get { return _createDate; }
            set
            {
                if (value != _createDate)
                {
                    _createDate = value;
                    NotifyPropertyChanged(SysUser.Prop_CreateDate);
                }
            }
        }


        [Property("Wage", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public System.Decimal? Wage
        {
            get { return _wage; }
            set
            {
                if (value != _wage)
                {
                    object oldValue = _wage;
                    _wage = value;
                    RaisePropertyChanged(SysUser.Prop_Wage, oldValue, value);
                }
            }

        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            PropertyChangedEventHandler localPropertyChanged = PropertyChanged;
            if (localPropertyChanged != null)
            {
                localPropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

    } // SysUser
}

