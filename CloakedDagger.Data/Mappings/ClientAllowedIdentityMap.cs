using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ClientAllowedIdentityMap : IEntityTypeConfiguration<ClientAllowedIdentity>
    {
        public void Configure(EntityTypeBuilder<ClientAllowedIdentity> builder)
        {

            builder.ToTable("CLIENT_ALLOWED_IDENTITY")
                .HasKey(cai => cai.ClientAllowedIdentityId);

            builder.Property(cai => cai.ClientAllowedIdentityId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(cai => cai.ClientId)
                .HasColumnName("CLIENT_ID")
                .IsRequired();

            builder.Property(cai => cai.Identity)
                .IsRequired();

            builder.HasOne(cai => cai.Client)
                .WithMany(c => c.ClientAllowedIdentities)
                .HasForeignKey(cai => cai.ClientId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties();
        }
    }
}