using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Aim.Examining.Web
{
    public partial class FrmImgNews : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnableViewState = false;
                try
                {
                    string sql = @"select top 5 Id,Title,substring(ImgPath,1,len(ImgPath)-1) as ImgPath 
                                from MessageInfo where type='图片新闻' and ReleaseState='1' order by CreateTime desc";
                    SqlDataAdapter dap = new SqlDataAdapter(sql, ConfigurationManager.AppSettings["conStr"]);
                    DataTable dt = new DataTable();
                    dap.Fill(dt);
                    dap.Dispose();

                    DataRow row = null;
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        row = dt.Rows[i];
                        litimg.Text += @"<a href='#'><img src='D:\RW\Files\AppFiles\Portal\Default\" + row["ImgPath"] + "' alt='" + row["Title"] + "' "
                                + "onclick=\"window.open('/Message/FrmMessageView.aspx?Id=" + row["Id"] + "&op=r','asdf','');\" width='350' height='200' />";
                    }

                    row = dt.Rows[dt.Rows.Count - 1];
                    imglast.Src = @"D:\RW\Files\AppFiles\Portal\Default\" + row["ImgPath"];
                    imglast.Alt = row["Title"] + "";
                    imglast.Attributes.Add("onclick", "window.open('/Message/FrmMessageView.aspx?Id=" + row["Id"] + "&op=r','asdf','');");
                }
                catch (Exception ex)
                {
                    litimg.Text = "异常" + ex.Message;
                }
            }
        }
    }
}
