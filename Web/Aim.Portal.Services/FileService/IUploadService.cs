using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Aim.Portal.Services
{
    [ServiceContract]
    public interface IUploadService
    {        
        [OperationContract]
        string StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk);

        [OperationContract]
        void CancelUpload(string filename);
    }

}
