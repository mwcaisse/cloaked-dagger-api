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

            builder.ToTable("USER_REGISTRATION_KEY_USE")
                .HasKey(urku => urku.Id);

            builder.Property(urku => urku.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(urku => urku.UserId)
                .HasColumnName("USER_ID")
                .IsRequired();

            builder.Property(urku => urku.UserRegistrationKeyId)
                .HasColumnName("USER_REGISTRATION_KEY_ID")
                .IsRequired();

            builder.HasOne(urku => urku.User)
                .WithMany()
                .HasForeignKey(urku => urku.UserId)
                .IsRequired();

            builder.HasOne(urku => urku.UserRegistrationKey)
                .WithMany(urk => urk.UserRegirationKeyUses)
                .HasForeignKey(urk => urk.UserRegistrationKeyId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties();

        }
    }
}