using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport_Service.Data;
using Transport_Service.Models.Entities;

namespace TransportServiceUnitTests.Fakes
{
    class FakeTransportTypeRepository : ITransportTypeRepository
    {
        public void CreateNewTransportType(TransportType transportType)
        {
            throw new NotImplementedException();
        }

        public List<TransportType> GetAllTransportTypes()
        {
            throw new NotImplementedException();
        }

        public TransportType GetTransportTypeById(int tranportTypeId)
        {
            return new TransportType
            {
                Id = 1,
                Name = "Water"
            };
        }

        public void RemoveTransportType(TransportType transportType)
        {
            throw new NotImplementedException();
        }
    }
}
