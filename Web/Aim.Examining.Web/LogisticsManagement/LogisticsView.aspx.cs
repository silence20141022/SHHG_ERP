using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using System.Configuration;
using Aim.Data;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Aim.Examining.Web.LogisticsManagement
{
    public partial class LogisticsView : ExamBasePage
    {
        string id = String.Empty;   // 对象id       
        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");
            Logistic ent = Logistic.Find(id);
            SetFormData(ent);
            DataTable tbl = new DataTable();
            DataColumn col = new DataColumn("Code");
            col.DataType = typeof(string);
            tbl.Columns.Add(col);
            col = new DataColumn("Name");
            col.DataType = typeof(string);
            tbl.Columns.Add(col);
            col = new DataColumn("Unit");
            col.DataType = typeof(string);
            tbl.Columns.Add(col);
            col = new DataColumn("OutCount");
            col.DataType = typeof(string);
            tbl.Columns.Add(col);
            col = new DataColumn("Remark");
            col.DataType = typeof(string);
            tbl.Columns.Add(col);
            JArray jsonarray = JsonHelper.GetObject<JArray>(ent.Child);
            foreach (JObject json in jsonarray)
            {
                DataRow dr = tbl.NewRow();
                dr["Code"] = json.Value<string>("Code");
                dr["Name"] = json.Value<string>("Name");
                dr["OutCount"] = json.Value<string>("OutCount");
                dr["Unit"] = json.Value<string>("Unit");
                dr["Remark"] = json.Value<string>("Remark");
                tbl.Rows.Add(dr);
            }
            PageState.Add("DataList", tbl);
        }
    }
}
