using CloakedDagger.Common.Entities;
using CloakedDagger.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data
{
    public class CloakedDaggerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public CloakedDaggerDbContext(DbContextOptions<CloakedDaggerDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}