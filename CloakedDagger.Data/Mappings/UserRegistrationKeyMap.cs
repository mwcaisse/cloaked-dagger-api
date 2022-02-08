using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class UserRegistrationKeyMap : IEntityTypeConfiguration<UserRegistrationKeyEntity>
    {
        public void Configure(EntityTypeBuilder<UserRegistrationKeyEntity> builder)
        {
            builder.ToTable("user_registration_key")
                .HasKey(urk => urk.Id);

            builder.Property(urk => urk.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(urk => urk.Key)
                .HasColumnName("key_val")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(urk => urk.UsesRemaining)
                .HasColumnName("uses_remaining")
                .IsRequired();
            
            builder.AddActiveEntityProperties(true);
            builder.AddTrackedEntityProperties(true);
        }
    }
}