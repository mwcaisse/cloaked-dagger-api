using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {

            builder.ToTable("CLIENT")
                .HasKey(c => c.ClientId);

            builder.Property(c => c.ClientId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .HasColumnName("NAME")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.Description)
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(1000);

            builder.Property(c => c.Secret)
                .HasColumnName("SECRET")
                .IsRequired()
                .HasMaxLength(2000);
            
            builder.AddActiveEntityProperties();
            builder.AddTrackedEntityProperties();

        }
    }
}