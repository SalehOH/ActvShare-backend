using ActvShare.Domain.Users;
using ActvShare.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActvShare.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureFollowsTable(builder);
        ConfigureNotificationTable(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder
            .Property(u => u.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder
            .Property(u => u.Name)
            .HasMaxLength(50);

        // Add these lines
        builder
            .Property(u => u.Username);

        builder
            .Property(u => u.Email);

        builder
            .Property(u => u.Password);


        builder
            .Property(u => u.RefreshToken);



        builder.OwnsOne(u => u.ProfileImage, profileBuilder =>
            {
                profileBuilder.ToTable("ProfileImages");

                profileBuilder.WithOwner().HasForeignKey("UserId");

                profileBuilder.HasKey("Id", "UserId");

                profileBuilder
                    .Property(pi => pi.Id)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => ProfileImageId.Create(value));
            });

    }

    private void ConfigureFollowsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Follows, followBuilder =>
        {
            followBuilder.ToTable("Follows");

            followBuilder
                .WithOwner()
                .HasForeignKey("UserId");

            followBuilder
                .HasKey("Id", "UserId");

            followBuilder
                .Property(f => f.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => FollowId.Create(value));

            followBuilder
                .Property(f => f.FollowedUserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));
        });

        builder.Metadata.FindNavigation(nameof(User.Follows))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    private void ConfigureNotificationTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Notifications, notificationBuilder =>
        {
            notificationBuilder.ToTable("Notifications");

            notificationBuilder
                .WithOwner()
                .HasForeignKey("UserId");

            notificationBuilder
                .HasKey("Id", "UserId");

            notificationBuilder
                .Property(n => n.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => NotificationId.Create(value));

            notificationBuilder
                .Property(n => n.Message)
                .HasMaxLength(100);

            notificationBuilder
                .Property(n => n.IsRead);

            builder.Metadata.FindNavigation(nameof(User.Notifications))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

        });
    }
}