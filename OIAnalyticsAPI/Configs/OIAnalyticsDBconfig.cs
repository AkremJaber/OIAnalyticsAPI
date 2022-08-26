using Microsoft.EntityFrameworkCore;
using OIAnalyticsAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIAnalyticsAPI.Configs
{
    public class OIAnalyticsDBconfig : DbContext
    {        
        public OIAnalyticsDBconfig(DbContextOptions<OIAnalyticsDBconfig> options) : base(options) { }
        public DbSet<Tenant> CCCTenants { get; set;}
        public DbSet<Person> Person { get; set;}
        public DbSet<TenantsHasPersons> CCCTenantsHasPersons { get; set;}
    }
}
