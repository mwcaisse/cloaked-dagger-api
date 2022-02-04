using CloakedDagger.Common.Entities;
using CloakedDagger.Data.Mappings;
using CloakedDagger.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data
{
    public class CloakedDaggerDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<RoleEntity> Roles { get; set; }
        
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        
        public DbSet<UserRegistrationKeyEntity> UserRegistrationKeys { get; set; }
        
        public DbSet<UserRegistrationKeyUseEntity> UserRegistrationKeyUses { get; set; }
        
        public DbSet<UserEmailVerificationRequestEntity> UserEmailVerificationRequests { get; set; }

        public DbSet<ClientEventEntity> ClientEvents { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }

        public DbSet<ResourceEntity> Resources { get; set; }
        
        public DbSet<ResourceScopeEntity> ResourceScopes { get; set; }
        
        public DbSet<ScopeEntity> Scopes { get; set; }
        
        public DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public CloakedDaggerDbContext(DbContextOptions<CloakedDaggerDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new UserRegistrationKeyMap());
            modelBuilder.ApplyConfiguration(new UserRegistrationKeyUseMap());
            modelBuilder.ApplyConfiguration(new UserEmailVerificationRequestMap());

            modelBuilder.ApplyConfiguration(new ClientMap());
            modelBuilder.ApplyConfiguration(new ClientEventEntityMap());
            
            modelBuilder.ApplyConfiguration(new ResourceMap());
            modelBuilder.ApplyConfiguration(new ResourceScopeMap());
            
            modelBuilder.ApplyConfiguration(new ScopeMap());

            modelBuilder.ApplyConfiguration(new PersistedGrantMap());
        }
    }
}