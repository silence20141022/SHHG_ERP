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

namespace Aim.Examining.Web.BaseInfo
{
    public partial class SupplierList : ExamListPage
    {
        private IList<Supplier> ents = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "batchdelete":
                    IList<object> idList = RequestData.GetList<object>("IdList");
                    if (idList != null && idList.Count > 0)
                    {
                        foreach (object obj in idList)
                        {
                            bool allowDelete = AllowDelete(obj.ToString());
                            if (allowDelete)
                            {
                                DataHelper.ExecSql("delete SHHG_AimExamine..Supplier where Id='" + obj.ToString() + "'");
                                PageState.Add("Message", "删除成功！");
                            }
                            else
                            {
                                PageState.Add("Message", "该供应商有关联信息不允许删除！");
                                return;
                            }
                        }
                    }
                    break;
                default:
                    DoSelect();
                    break;
            }
        }
        private void DoSelect()
        {
            ents = Supplier.FindAll(SearchCriterion);
            this.PageState.Add("SupplierList", ents);
        }
        private bool AllowDelete(string supplierId)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(supplierId))
            {
                IList<Product> proEnts = Product.FindAll("from Product where SupplierId='" + supplierId + "'"); //有产品的供应商不允许删除
                IList<PurchaseOrder> poEnts = PurchaseOrder.FindAll("from PurchaseOrder where SupplierId='" + supplierId + "'");//有没有做采购单
                IList<InWarehouse> iwEnts = InWarehouse.FindAll("from InWarehouse where SupplierId='" + supplierId + "'");//有没有做入库单
                if (poEnts.Count > 0 || iwEnts.Count > 0 || proEnts.Count > 0)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}

