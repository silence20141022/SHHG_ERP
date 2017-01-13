using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Aim.Portal.Services
{
    [ServiceContract]
    public interface ICrossDomainService
    {
        [OperationContract]
        [WebGet(UriTemplate = "~/ClientAccessPolicy.xml")]
        System.ServiceModel.Channels.Message ProvidePolicyFile();
    }
}
