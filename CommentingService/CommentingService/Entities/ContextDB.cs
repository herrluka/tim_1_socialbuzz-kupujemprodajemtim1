using Microsoft.Extensions.Configuration;
using CommentingService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Entities
{
    public class ContextDB : DbContext
    {
        private readonly IConfiguration configuration;

        public ContextDB(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Comments> Comment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //this is empty method
        }
    
}
}
