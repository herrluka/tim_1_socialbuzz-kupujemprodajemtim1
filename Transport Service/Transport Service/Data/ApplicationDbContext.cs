using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transport_Service.Models.Entities;

namespace Transport_Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
            base(options) { }

        public virtual DbSet<Transport> Transports { get; set; }
        public virtual DbSet<TransportType> TransportTypes { get; set; }
    }
}
