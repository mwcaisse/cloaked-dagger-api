using CloakedDagger.Common.Entities;
using CloakedDagger.Data.Mappings;
using CloakedDagger.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data
{
    public class CloakedDaggerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<Client> Clients { get; set; }
        
        public DbSet<ClientAllowedScope> ClientAllowedScopes { get; set; }
        
        public DbSet<ClientAllowedGrantType> ClientAllowedGrantTypes { get; set; }
        
        public DbSet<ClientAllowedIdentity> ClientAllowedIdentities { get; set; }
        
        public DbSet<ClientUri> ClientUris { get; set; }
        
        public DbSet<Resource> Resources { get; set; }
        
        public DbSet<ResourceScope> ResourceScopes { get; set; }
        
        public DbSet<Scope> Scopes { get; set; }

        public CloakedDaggerDbContext(DbContextOptions<CloakedDaggerDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());

            modelBuilder.ApplyConfiguration(new ClientMap());
            modelBuilder.ApplyConfiguration(new ClientAllowedGrantTypeMap());
            modelBuilder.ApplyConfiguration(new ClientAllowedIdentityMap());
            modelBuilder.ApplyConfiguration(new ClientAllowedScopeMap());
            modelBuilder.ApplyConfiguration(new ClientUriMap());
            
            modelBuilder.ApplyConfiguration(new ResourceMap());
            modelBuilder.ApplyConfiguration(new ResourceScopeMap());
            
            modelBuilder.ApplyConfiguration(new ScopeMap());
            
            
            
        }
    }
}