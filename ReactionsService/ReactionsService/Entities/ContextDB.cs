using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReactionsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Entities
{
    public class ContextDB : DbContext
    {
        private readonly IConfiguration configuration;

        public ContextDB(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Reactions> Reactions { get; set; }

        public DbSet<TypeOfReaction> Type_Of_Reaction { get; set; }
        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reactions>()
                 .HasData(new
                 {
                     ReactionID = Guid.Parse("6a411c13-a195-48f7-8dbd-67596c3974c0"),
                     ProductID = 2,
                     TypeOfReactionID = 1,
                     UserID = 3

                 });
        }
    }
}
