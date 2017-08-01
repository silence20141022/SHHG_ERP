using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Aim.Examining.Web.BaseInfo
{
    public partial class customerlist : System.Web.UI.Page
    {
        int total = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string where = string.Empty;
            string sql = string.Empty;
            DataTable dt = null;
            switch (action)
            {
                case "load":
                    if (!string.IsNullOrEmpty(Request["name"]))
                    {
                        where += " and name like '%" + Request["name"].Trim() + "%'";
                    } 
                    sql = "select * from customers where 1=1 " + where;
                    int pagesize = Convert.ToInt32(Request["limit"]);//start limit  page
                    int currpage = Convert.ToInt32(Request["page"]);
                    dt = DbMgr.GetDataTable(Common.GetPageSql(sql, "createtime", ref total, pagesize, currpage, "desc"));
                    IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
                    iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                    string result = JsonConvert.SerializeObject(dt, iso);
                    Response.Write("{total:" + total + ",rows:" + result + "}");
                    Response.End();
                    break;
                case "loaduser":
                    sql = "select UserID,Name from "+ConfigurationManager.AppSettings["basedb"]+"..sysuser order by createdate asc";
                    dt = DbMgr.GetDataTable(sql);
                    Response.Write(JsonConvert.SerializeObject(dt));
                    Response.End();
                    break;
                case "save":
                    try
                    {
                        JObject jo = (JObject)JsonConvert.DeserializeObject(Request["formdata"]); 
                        int id;
                        if (string.IsNullOrEmpty(jo.Value<string>("id")))
                        {
                            sql = @"insert into customers (Name,SimpleName,Bank,AccountNum,AccountName,TariffNum,CreditAmount,AccountValidity,
                                    Address,Tel,OpenTime,MagUser,Remark,MagId) values ()";
                        }
                        else
                        {
                            //ud.updateUser(jo);//这个是返回影响的行数
                            //id = jo.Value<int>("id");
                        }
                       
                       // return id;
                    }
                    catch
                    {
                       // return 0;
                    }
                    break;
            }
        }
    }
}