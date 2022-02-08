using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ClientMap : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {

            builder.ToTable("client")
                .HasKey(c => c.ClientId);

            builder.Property(c => c.ClientId)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            builder.Property(c => c.Secret)
                .HasColumnName("secret")
                .IsRequired()
                .HasMaxLength(2000);
            
            builder.AddActiveEntityProperties(true);
            builder.AddTrackedEntityProperties(true);

        }
    }
}