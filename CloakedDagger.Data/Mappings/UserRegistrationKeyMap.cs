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
            builder.ToTable("USER_REGISTRATION_KEY")
                .HasKey(urk => urk.Id);

            builder.Property(urk => urk.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(urk => urk.Key)
                .HasColumnName("KEY_VAL")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(urk => urk.UsesRemaining)
                .HasColumnName("USES_REMAINING")
                .IsRequired();
            
            builder.AddActiveEntityProperties();
            builder.AddTrackedEntityProperties();
        }
    }
}