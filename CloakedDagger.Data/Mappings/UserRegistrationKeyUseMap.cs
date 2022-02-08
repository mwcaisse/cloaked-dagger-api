using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class UserRegistrationKeyUseMap : IEntityTypeConfiguration<UserRegistrationKeyUseEntity> 
    {
        public void Configure(EntityTypeBuilder<UserRegistrationKeyUseEntity> builder)
        {

            builder.ToTable("user_registration_key_use")
                .HasKey(urku => urku.Id);

            builder.Property(urku => urku.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(urku => urku.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(urku => urku.UserRegistrationKeyId)
                .HasColumnName("user_registration_key_id")
                .IsRequired();

            builder.HasOne(urku => urku.User)
                .WithMany()
                .HasForeignKey(urku => urku.UserId)
                .IsRequired();

            builder.HasOne(urku => urku.UserRegistrationKey)
                .WithMany(urk => urk.UserRegirationKeyUses)
                .HasForeignKey(urk => urk.UserRegistrationKeyId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties(true);

        }
    }
}