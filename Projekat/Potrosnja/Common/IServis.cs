using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IServis
    {
        [OperationContract]
        List<Load> VratiLoad(string upit);
        [OperationContract]
        Audit VratiAudit(string upit);
        [OperationContract]
        List<Load> readXmlLoad(string upit);
        [OperationContract]
        void AuditToXml(Audit a);
        
    }
}
