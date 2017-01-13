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
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data;

namespace Aim.Examining.Web
{
    public partial class FrmDeliveryProduct : ExamBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["Id"];

            if (!String.IsNullOrEmpty(id))
            {
                DeliveryOrder ent = DeliveryOrder.TryFind(id);
                if (ent != null)
                {
                    this.SetFormData(ent);
                }
            }
        }
    }
}

