using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transport_Service.Models.Entities;

namespace Transport_Service.Data
{
    public class TransportTypeRepository : ITransportTypeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TransportTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<TransportType> GetAllTransportTypes()
        {
            return dbContext.TransportTypes.ToList();
        }

        public TransportType GetTransportTypeById(int tranportTypeId)
        {
            return dbContext.TransportTypes.FirstOrDefault(transportType => transportType.Id == tranportTypeId);
        }

        public void CreateNewTransportType(TransportType transportType)
        {
            dbContext.TransportTypes.Add(transportType);
        }

        public void RemoveTransportType(TransportType transportType)
        {
            dbContext.TransportTypes.Remove(transportType);
        }
    }
}
