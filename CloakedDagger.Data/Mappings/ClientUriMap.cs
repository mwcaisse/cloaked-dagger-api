using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ClientUriMap : IEntityTypeConfiguration<ClientUri>
    {
        public void Configure(EntityTypeBuilder<ClientUri> builder)
        {

            builder.ToTable("CLIENT_URI")
                .HasKey(cu => cu.ClientUriId);

            builder.Property(cu => cu.ClientUriId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(cu => cu.ClientId)
                .HasColumnName("CLIENT_ID")
                .IsRequired();
            
            builder.Property(cu => cu.Type)
                .HasColumnName("TYPE")
                .IsRequired();

            builder.Property(cu => cu.Uri)
                .HasColumnName("URI")
                .IsRequired()
                .HasMaxLength(5000);

            builder.HasOne(cu => cu.Client)
                .WithMany(c => c.ClientUris)
                .HasForeignKey(cu => cu.ClientId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties();
        }
    }
}