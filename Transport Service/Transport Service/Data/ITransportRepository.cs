using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transport_Service.Models.DTOs;
using Transport_Service.Models.Entities;

namespace Transport_Service.Data
{
    public interface ITransportRepository
    {
        List<Transport> GetAllTransports();
        List<Transport> GetTransportsByTransportType(int transportTypeId);
        void CreateNewTransport(Transport transport);
        List<Transport> GetTransportsByTransportTypeOrderByMinimalWeight(int transportTypeId);
        void UpdateTransport(Transport transport);
        void RemoveTransport(Transport transport);
        List<AvailableTransportDto> GetAvailableTransportsByProvidedWeight(double weight);
    }
}
