using System;
using System.IO;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Aim.Portal.Services
{
    public class CrossDomainService : ICrossDomainService
    {
        #region ICrossDomainService Members

        System.ServiceModel.Channels.Message ICrossDomainService.ProvidePolicyFile()
        {
            FileStream filestream = File.Open(@"ClientAcessPolicy.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//此处访问xml地址
            XmlReader reader = XmlReader.Create(filestream);

            System.ServiceModel.Channels.Message result = System.ServiceModel.Channels.Message.CreateMessage(MessageVersion.None, "", reader);
            System.Console.WriteLine("it start...");

            return result;
        }

        #endregion
    }
}
