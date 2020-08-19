using System.Runtime.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;
using ResourceScope = CloakedDagger.Common.Entities.ResourceScope;

namespace CloakedDagger.Data.Mappings
{
    public class ResourceScopeMap : IEntityTypeConfiguration<ResourceScope>
    {
        public void Configure(EntityTypeBuilder<ResourceScope> builder)
        {
            builder.ToTable("RESOURCE")
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

            builder.HasOne(rs => rs.Resource)
                .WithMany(r => r.AvailableScopes)
                .HasForeignKey(rs => rs.ResourceId)
                .IsRequired();

            builder.HasOne(rs => rs.Scope)
                .WithMany(s => s.ResourceScopes)
                .HasForeignKey(rs => rs.ScopeId)
                .IsRequired();

            builder.AddTrackedEntityProperties();
        }
    }
}