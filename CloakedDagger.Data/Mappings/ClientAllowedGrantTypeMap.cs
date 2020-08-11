using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ClientAllowedGrantTypeMap : IEntityTypeConfiguration<ClientAllowedGrantType>
    {
        public void Configure(EntityTypeBuilder<ClientAllowedGrantType> builder)
        {
            builder.ToTable("CLIENT_ALLOWED_GRANT_TYPE")
                .HasKey(cagt => cagt.ClientAllowedGrantTypeId);

            builder.Property(cagt => cagt.ClientAllowedGrantTypeId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(cagt => cagt.ClientId)
                .HasColumnName("CLIENT_ID")
                .IsRequired();

            builder.Property(cagt => cagt.GrantType)
                .HasColumnName("GRANT_TYPE")
                .IsRequired();

            builder.HasOne(cagt => cagt.Client)
                .WithMany(c => c.ClientAllowedGrantTypes)
                .HasForeignKey(cagt => cagt.ClientId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties();
        }
    }
}