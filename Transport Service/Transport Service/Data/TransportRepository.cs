using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transport_Service.Models.DTOs;
using Transport_Service.Models.Entities;

namespace Transport_Service.Data
{
    public class TransportRepository : ITransportRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TransportRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Transport> GetAllTransports()
        {
            return dbContext.Transports.ToList();
        }

        public List<Transport> GetTransportsByTransportType(int transportTypeId)
        {
            return dbContext.Transports.Where(t => t.TransportTypeId == transportTypeId).ToList();
        }

        public void CreateNewTransport(Transport newTransport)
        {
            dbContext.Transports.Add(newTransport);
            dbContext.SaveChangesAsync();
        }

        public List<Transport> GetTransportsByTransportTypeOrderByMinimalWeight(int transportTypeId)
        {
            return dbContext.Transports.OrderBy(t => t.MinimalWeight).Where(t => t.TransportTypeId == transportTypeId).ToList();
        }

        public void UpdateTransport(Transport transport)
        {
            dbContext.Update(transport);
            dbContext.SaveChangesAsync();
        }

        List<Transport> ITransportRepository.GetAllTransports()
        {
            throw new NotImplementedException();
        }

        public void RemoveTransport(Transport transport)
        {
            dbContext.Transports.Remove(transport);
            dbContext.SaveChangesAsync();
        }

        public List<AvailableTransportDto> GetAvailableTransportsByProvidedWeight(double weight)
        {
            var query = from transport in dbContext.Transports
                        join transportType in dbContext.TransportTypes on transport.TransportTypeId equals transportType.Id
                        where transport.MinimalWeight <= weight && weight <= transport.MaximalWeight
                        select new AvailableTransportDto
                        {
                            Id = transport.Id,
                            Price = transport.Price,
                            TransportType = transport.TransportType.Name
                        };

            return query.ToList();
        }
    }
}
