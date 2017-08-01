using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Aim.Examining.Web
{
    public class Common
    {
        public static string GetPageSql(String sql, string orderby, ref int total, int pagesize, int currpage, string direction)
        {
            string tmpsql = "select count(*) from (" + sql + ") t";
            total = DbMgr.ExecuteScalar(tmpsql);
            string order = orderby;
            string pageSql = @"
		    WITH OrderedOrders AS
		    (SELECT *,
		    ROW_NUMBER() OVER (order by {0} {1})as RowNumber
		    FROM ({2}) temp ) 
		    SELECT * 
		    FROM OrderedOrders 
		    WHERE RowNumber between {3} and {4}";
            pageSql = string.Format(pageSql, order, direction, sql, (currpage - 1) * pagesize + 1, currpage * pagesize);
            return pageSql;
        }
        public static JObject Get_UserInfo(string account)
        {
            string result = "";
            string sql = @"select * from app_tb_users where uid ='" + account + "'";
            DataTable dt = DbMgr.GetDataTable(sql);
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string jsonstr = JsonConvert.SerializeObject(dt, iso);
            jsonstr = jsonstr.Replace("[", "").Replace("]", "");
            result = jsonstr;
            return (JObject)JsonConvert.DeserializeObject(result);
        }
        //根据前端传入的dictionary key 返回各key的字典项
        public static string loaddics(string keys)
        {
            string[] strarray = keys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string sql = string.Empty;
            DataTable dt = null;
            List<string> ls = new List<string>();
            foreach (string key in strarray)
            {
                sql = "select * from app_tb_dict where param='" + key + "' order by qorder asc";
                dt = DbMgr.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ls.Add(key + ":" + JsonConvert.SerializeObject(dt));
                }
            } 
            return "{" + string.Join(",", ls.ToArray()) + "}";
        }
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch 
            {               
                return "";
            }
        }
    }
}