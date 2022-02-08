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
            builder.ToTable("resource_scope")
                .HasKey(rs => rs.ResourceScopeId);

            builder.Property(rs => rs.ResourceScopeId)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(rs => rs.ResourceId)
                .HasColumnName("resource_id")
                .IsRequired();

            builder.Property(rs => rs.ScopeId)
                .HasColumnName("scope_id")
                .IsRequired();

            builder.HasOne(rs => rs.ResourceEntity)
                .WithMany(r => r.AvailableScopes)
                .HasForeignKey(rs => rs.ResourceId)
                .IsRequired();

            builder.HasOne(rs => rs.ScopeEntity)
                .WithMany(s => s.ResourceScopes)
                .HasForeignKey(rs => rs.ScopeId)
                .IsRequired();

            builder.AddTrackedEntityProperties(true);
        }
    }
}