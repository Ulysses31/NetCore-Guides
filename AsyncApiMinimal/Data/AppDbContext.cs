using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncApiMinimal.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncApiMinimal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options)
        {
        }

        public DbSet<BatchProcesses> BatchProcesses => Set<BatchProcesses>();

        public DbSet<BatchProcessItems> BatchProcessItems => Set<BatchProcessItems>();

        public DbSet<DomainUsers> DomainUsers => Set<DomainUsers>();
    }
}
