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
    public partial class PurchaseCorrespondBillChart : ExamListPage
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
            DataColumn dc = new DataColumn("Date"); tbl.Columns.Add(dc);
            dc = new DataColumn("BorrowAmount"); tbl.Columns.Add(dc);
            dc = new DataColumn("PayAmount"); tbl.Columns.Add(dc);//列构建完毕
            for (int i = 11; i >= 0; i--)
            {
                DataRow newRow = tbl.NewRow();
                newRow["Date"] = dt.AddMonths(-i).Month;
                sql = @"select sum(BorrowAmount) as BorrowAmount,sum(PayAmount) as PayAmount from SHHG_AimExamine..CorrespondBill 
                where year(OperateDate)='{0}' and month(OperateDate)='{1}' and BillType='采购对账'";
                sql = string.Format(sql, dt.AddMonths(-i).Year, dt.AddMonths(-i).Month);
                IList<EasyDictionary> eDics = DataHelper.QueryDictList(sql);
                newRow["BorrowAmount"] = eDics[0].Get<string>("BorrowAmount");
                newRow["PayAmount"] = eDics[0].Get<string>("PayAmount");
                tbl.Rows.Add(newRow);
            }
            PageState.Add("DataList", tbl);
        }
    }
}

