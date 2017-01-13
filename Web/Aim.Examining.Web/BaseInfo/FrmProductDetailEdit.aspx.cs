using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aim.Data;
using Aim.Portal;
using Aim.Portal.Model;
using Aim.Portal.Web;
using Aim.Portal.Web.UI;
using Aim.Examining.Model;
using Castle.ActiveRecord;
using System.Web.Script.Serialization;

namespace Aim.Examining.Web.PurchaseManagement
{
    public partial class FrmProductDetailEdit : ExamBasePage
    {

        string Pid = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Pid = RequestData.Get<string>("Pid");

            ProductDetail ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Insert:
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<ProductDetail>();

                    //判断是否存在
                    if (ProductDetail.FindAllByProperty("GuId", ent.GuId).FirstOrDefault<ProductDetail>() != null)
                    {
                        PageState.Add("error", "此编号已存在！");
                        break;
                    }

                    ent.State = true;
                    ent.DoCreate();
                    this.SetMessage("新建成功！");
                    break;
            }

            if (RequestData.Get<string>("op") == "c")
            {
                if (!String.IsNullOrEmpty(Pid))
                {
                    Product pro = Product.TryFind(Pid);
                    if (pro != null)
                    {
                        ent = new ProductDetail { PId = pro.Id, PCode = pro.Code, PISBN = pro.Isbn, PName = pro.Name, PPcn = pro.Pcn };
                        this.SetFormData(ent);
                    }
                }
            }
        }
    }
}

