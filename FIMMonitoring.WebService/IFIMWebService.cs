using System;
using System.Collections.Generic;
using FIMMonitoring.Domain.ViewModels.WCF;
using System.ServiceModel;

namespace FIMMonitoring.WebService
{
    [ServiceContract]
    public interface IFIMWebService
    {

        [OperationContract]
        List<Guid> SendErrorPack(ErrorPack errors);
    }
    
}
