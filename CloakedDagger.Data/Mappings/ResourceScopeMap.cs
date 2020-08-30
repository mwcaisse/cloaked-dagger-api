using System.Runtime.Versioning;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ResourceScopeMap : IEntityTypeConfiguration<ResourceScopeEntity>
    {
        public void Configure(EntityTypeBuilder<ResourceScopeEntity> builder)
        {
            builder.ToTable("RESOURCE_SCOPE")
                .HasKey(rs => rs.ResourceScopeId);

            builder.Property(rs => rs.ResourceScopeId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(rs => rs.ResourceId)
                .HasColumnName("RESOURCE_ID")
                .IsRequired();

            builder.Property(rs => rs.ScopeId)
                .HasColumnName("SCOPE_ID")
                .IsRequired();

            builder.HasOne(rs => rs.ResourceEntity)
                .WithMany(r => r.AvailableScopes)
                .HasForeignKey(rs => rs.ResourceId)
                .IsRequired();

            builder.HasOne(rs => rs.ScopeEntity)
                .WithMany(s => s.ResourceScopes)
                .HasForeignKey(rs => rs.ScopeId)
                .IsRequired();

            builder.AddTrackedEntityProperties();
        }
    }
}