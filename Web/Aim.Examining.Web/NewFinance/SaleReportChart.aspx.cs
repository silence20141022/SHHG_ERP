using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using Aim.Data;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.Examining.Model;
using System.Data;

namespace Aim.Examining.Web
{
    public partial class SaleReportChart : ExamListPage
    {
        string sql = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            DateTime dt = System.DateTime.Now.Date;
            DataTable tbl = new DataTable();
            DataColumn dc = new DataColumn("Date");
            tbl.Columns.Add(dc);
            dc = new DataColumn("BorrowAmount");
            tbl.Columns.Add(dc);
            dc = new DataColumn("PayAmount");
            tbl.Columns.Add(dc);//列构建完毕
            //对称式年度对比 比如今年5月份。那么要展示的数据就是去年1-5月和今年的1-5月
            int month = System.DateTime.Now.Month;
            for (int i = 1; i <= month; i++)//去年
            {
                DataRow newRow = tbl.NewRow();
                newRow["Date"] = dt.AddYears(-1).Year + "-" + i.ToString();
                sql = @"select round(sum(TotalMoney)/10000,1) as BorrowAmount from SHHG_AimExamine..DeliveryOrder  where year(CreateTime)='{0}' 
                and month(CreateTime)='{1}' and State='已出库'";
                sql = string.Format(sql, dt.AddYears(-1).Year, i);
                newRow["BorrowAmount"] = DataHelper.QueryValue<decimal>(sql);
                sql = @"select round(sum(Money)/10000,1) as PayAmount from SHHG_AimExamine..PaymentInvoice where year(CreateTime)='{0}' 
                and month(CreateTime)='{1}'";
                sql = string.Format(sql, dt.AddYears(-1).Year, i);
                newRow["PayAmount"] = DataHelper.QueryValue<decimal>(sql);
                tbl.Rows.Add(newRow);
            }
            for (int i = 1; i <= month; i++)//本年度
            {
                DataRow newRow = tbl.NewRow();
                newRow["Date"] = dt.Year + "-" + i.ToString();
                sql = @"select round(sum(TotalMoney)/10000,1) as BorrowAmount from SHHG_AimExamine..DeliveryOrder where year(CreateTime)='{0}'
                and month(CreateTime)='{1}' and State='已出库'";
                sql = string.Format(sql, dt.Year, i);
                newRow["BorrowAmount"] = DataHelper.QueryValue<decimal>(sql);
                sql = @"select round(sum(Money)/10000,1) as PayAmount from SHHG_AimExamine..PaymentInvoice where year(CreateTime)='{0}' 
                and month(CreateTime)='{1}'";
                sql = string.Format(sql, dt.Year, i);
                newRow["PayAmount"] = DataHelper.QueryValue<decimal>(sql);
                tbl.Rows.Add(newRow);
            }
            PageState.Add("DataList", tbl);
        }
    }
}

