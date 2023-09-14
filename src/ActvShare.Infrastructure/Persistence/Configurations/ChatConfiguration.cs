using ActvShare.Domain.Chats;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActvShare.Infrastructure.Persistence.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        ConfigureChatTable(builder);
        ConfigureMessagesTable(builder);
    }

    private void ConfigureChatTable(EntityTypeBuilder<Chat> builder)
    {
       builder.ToTable("Chats");

        builder.HasKey(p => p.Id);

        builder
            .Property(c => c.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ChatId.Create(value));

        builder
            .Property(c => c.user1)
            .IsRequired()
            .HasConversion(
                 id => id.Value,
                 value => UserId.Create(value));

        builder
            .Property(c => c.user2)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
        
    }
        
    private void ConfigureMessagesTable(EntityTypeBuilder<Chat> builder)
    {
        builder.OwnsMany(c => c.Messages, messagesBuilder =>
        {
            messagesBuilder.ToTable("ChatMessages");

            messagesBuilder
                .WithOwner()
                .HasForeignKey("ChatId");

            messagesBuilder
                .HasKey("Id", "ChatId");

            messagesBuilder
                .Property(mb => mb.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ChatMessageId.Create(value));

            messagesBuilder
                .Property(mb => mb.SenderId)                
                .IsRequired()
                .HasConversion(
                     id => id.Value,
                     value => UserId.Create(value));

            messagesBuilder
                .Property(mb => mb.Content)
                .HasMaxLength(120);
        });

        builder.Metadata.FindNavigation(nameof(Chat.Messages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}