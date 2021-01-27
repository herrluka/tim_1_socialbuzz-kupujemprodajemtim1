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
        public TransportType GetTransportTypeById(int tranportTypeId)
        {
            return dbContext.TransportTypes.FirstOrDefault(transportType => transportType.Id == tranportTypeId);
        }
    }
}
