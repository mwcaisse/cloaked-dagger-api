using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ClientAllowedScopeMap : IEntityTypeConfiguration<ClientAllowedScope>
    {
        public void Configure(EntityTypeBuilder<ClientAllowedScope> builder)
        {

            builder.ToTable("CLIENT_ALLOWED_SCOPE")
                .HasKey(cas => cas.ClientAllowedScopeId);

            builder.Property(cas => cas.ClientId)
                .HasColumnName("CLIENT_ID")
                .IsRequired();

            builder.Property(cas => cas.ScopeId)
                .HasColumnName("SCOPE_ID")
                .IsRequired();

            builder.HasOne(cas => cas.Client)
                .WithMany(c => c.ClientAllowedScopes)
                .HasForeignKey(cas => cas.ClientId)
                .IsRequired();

            builder.HasOne(cas => cas.Scope)
                .WithMany(s => s.ClientAllowedScopes)
                .HasForeignKey(cas => cas.ScopeId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties();
        }
    }
}