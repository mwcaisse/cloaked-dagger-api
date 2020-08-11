using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ScopeMap : IEntityTypeConfiguration<Scope>
    {
        public void Configure(EntityTypeBuilder<Scope> builder)
        {

            builder.ToTable("SCOPE")
                .HasKey(s => s.ScopeId);

            builder.Property(s => s.ScopeId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                .HasColumnName("NAME")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(s => s.Description)
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(1000);

            builder.AddActiveEntityProperties();
            builder.AddTrackedEntityProperties();
        }
    }
}