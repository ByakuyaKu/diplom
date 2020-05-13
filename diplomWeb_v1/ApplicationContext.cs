using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.AHP.Entities;

namespace diplomWeb_v1
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Criteria> Criterias { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated(); 
        }
    }
}
