using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport_Service.Data;
using Transport_Service.Models.DTOs;
using Transport_Service.Models.Entities;

namespace TransportServiceUnitTests.Fakes
{
    class FakeTransportRepository : ITransportRepository
    {
        public void CreateNewTransport(Transport transport)
        {
            
        }

        public List<Transport> GetAllTransports()
        {
            return new List<Transport>
            {
                new Transport()
                {
                    Id = 1,
                    MinimalWeight = 200,
                    MaximalWeight = 400,
                    Price = 2.4,
                    TransportType = null,
                    TransportTypeId = 1
                },
                new Transport()
                {
                    Id = 2,
                    MinimalWeight = 0,
                    MaximalWeight = 200,
                    Price = 5,
                    TransportType = null,
                    TransportTypeId = 5
                },
                new Transport()
                {
                    Id = 3,
                    MinimalWeight = 401,
                    MaximalWeight = 1000,
                    Price = 8,
                    TransportType = null,
                    TransportTypeId = 1
                },
                new Transport()
                {
                    Id = 4,
                    MinimalWeight = 1001,
                    MaximalWeight = 2000,
                    Price = 11,
                    TransportType = null,
                    TransportTypeId = 1
                }
            };
        }

        public List<AvailableTransportDto> GetAvailableTransportsByProvidedWeight(double weight)
        {
            return new List<AvailableTransportDto>
            {
                new AvailableTransportDto()
                {
                    Id = 1,
                    Price = 2.4,
                    TransportType = null,
                },
                new AvailableTransportDto()
                {
                    Id = 2,
                    Price = 5,
                    TransportType = null,
                }
            };
        }

        public List<Transport> GetTransportsByTransportType(int transportTypeId)
        {
            return new List<Transport>
            {
                new Transport()
                {
                    Id = 1,
                    MinimalWeight = 200,
                    MaximalWeight = 400,
                    Price = 2.4,
                    TransportType = null,
                    TransportTypeId = 1
                },
                new Transport()
                {
                    Id = 3,
                    MinimalWeight = 401,
                    MaximalWeight = 1000,
                    Price = 8,
                    TransportType = null,
                    TransportTypeId = 1
                },
                new Transport()
                {
                    Id = 4,
                    MinimalWeight = 1001,
                    MaximalWeight = 2000,
                    Price = 11,
                    TransportType = null,
                    TransportTypeId = 1
                }
            };
        }

        public List<Transport> GetTransportsByTransportTypeOrderByMinimalWeight(int transportTypeId)
        {
            return new List<Transport>
            {
                new Transport()
                {
                    Id = 1,
                    MinimalWeight = 200,
                    MaximalWeight = 400,
                    Price = 2.4,
                    TransportType = null,
                    TransportTypeId = 1
                },
                new Transport()
                {
                    Id = 3,
                    MinimalWeight = 401,
                    MaximalWeight = 1000,
                    Price = 8,
                    TransportType = null,
                    TransportTypeId = 1
                },
                new Transport()
                {
                    Id = 4,
                    MinimalWeight = 1001,
                    MaximalWeight = 2000,
                    Price = 11,
                    TransportType = null,
                    TransportTypeId = 1
                }
            };
        }

        public void RemoveTransport(Transport transport)
        {
            
        }

        public void UpdateTransport(Transport transport)
        {
            
        }
    }
}
