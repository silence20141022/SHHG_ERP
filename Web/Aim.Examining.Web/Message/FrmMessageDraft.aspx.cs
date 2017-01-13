using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Examining.Model;
using Aim.Portal.Web.UI;
using Aim.Portal.Model;
using Aim.Data;
using System.Configuration;

namespace Aim.Examining.Web.Message
{
    public partial class FrmMessageDraft : ExamBasePage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op") == null ? "c" : RequestData.Get<string>("op");
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");

            MessageDraft ent = null;
            string db = ConfigurationManager.AppSettings["ExamineDB"];

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<MessageDraft>();
                    ent.Type = RequestData["Section"] + "";
                    //ent.Content = RequestData["content"] + "";
                    ent.ReleaseState = "0";
                    ent.DoUpdate();

                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<MessageDraft>();
                    ent.Type = RequestData["Section"] + "";
                    //ent.Content = RequestData["content"] + "";
                    ent.DoCreate();

                    this.SetMessage("新建成功！");
                    break;
                default:
                    if (RequestActionString == "Submit")
                    {
                        //判断是否直接提交
                        if (RequestData["entId"] + "" == "")
                        {
                            ent = this.GetPostedData<MessageDraft>();
                        }
                        else
                        {
                            ent = this.GetMergedData<MessageDraft>();
                        }

                        ent.Type = RequestData["Section"] + "";
                        ent.ReleaseState = "1";
                        if (ent.Id == null)// || DataHelper.ExecSql("select count(id) from " + ConfigurationManager.AppSettings["ExamineDB"] + "..MessageInfo where id='" + ent.Id + "'") + "" == "0"
                        {
                            //添加到info表 (触发器)

                            ent.Content = RequestData["content"] + "";

                            //直接提交
                            ent.DoSave();
                        }
                        else
                        {
                            //添加到info表
                            DataHelper.ExecSql(@"insert into " + db + "..MessageInfo (Id, Title, Content, Importance, Typeid, Type, ReleaseState, ReleaseUser, ReleaseTime, ReadUser, ReadCount, ReadState, IsEnforcementUp, FileID, CreateId, CreateName, CreateTime, ReleaseId,Period,ReleDepartment,ImgPath,RemindDays) " +
                                "select Id, Title, Content, Importance, Typeid, Type, '0', ReleaseUser, ReleaseTime, ReadUser, ReadCount, ReadState, IsEnforcementUp, FileID, CreateId, CreateName, CreateTime, ReleaseId,Period,ReleDepartment,ImgPath,RemindDays from " + db + "..MessageDraft where Id='" + ent.Id + "'");

                            //已保存过的再提交
                            ent.DoUpdate();
                        }
                        //this.SetMessage("已成功提交！");

                        //通知管理员审核
                    }
                    break;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = MessageDraft.Find(id);
                }

                this.SetFormData(ent);
            }

            if (op == "c")
            {
                PageState.Add("ReleDepartment", DataHelper.QueryValue("select " + db + ".dbo.get_DeptName('" + UserInfo.UserID + "')"));
            }

            //重要性
            PageState.Add("ImportanceEnum", SysEnumeration.GetEnumDict("Message.Importance"));

            //栏目类型
            WebPart[] secs = WebPart.FindAll();
            PageState.Add("TypeEnum", GetEnumDict(secs));
        }

        private void AddRole(MessageDraft ent, string roleid, string roletype)
        {
            MessageToUser Mtu;
            string[] roleidArrary = roleid.Split(',');
            foreach (string str in roleidArrary)
            {
                if (str == "")
                    continue;

                Mtu = new MessageToUser();
                Mtu.MsgId = ent.Id;
                Mtu.UserId = str;
                Mtu.CreateId = UserInfo.UserID;
                Mtu.CreateName = UserInfo.Name;
                Mtu.RoleType = roletype;
                Mtu.DoSave();
            }
        }
        #endregion

        public static EasyDictionary GetEnumDict(WebPart[] enums)
        {
            EasyDictionary dict = new EasyDictionary();

            foreach (WebPart item in enums)
            {
                dict.Set(item.Id, item.BlockTitle);
            }

            return dict;
        }
    }
}
