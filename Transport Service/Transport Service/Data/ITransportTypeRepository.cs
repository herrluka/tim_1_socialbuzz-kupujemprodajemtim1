using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transport_Service.Models.Entities;

namespace Transport_Service.Data
{
    public interface ITransportTypeRepository
    {
        TransportType GetTransportTypeById(int tranportTypeId);
    }
}
