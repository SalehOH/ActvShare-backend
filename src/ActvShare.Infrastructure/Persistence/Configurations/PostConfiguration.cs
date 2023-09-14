using ActvShare.Domain.Posts;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActvShare.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        ConfigurePostsTable(builder);
        ConfigureLikesTable(builder);
        ConfigureRepliesTable(builder);
    }

    private void ConfigurePostsTable(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => PostId.Create(value));

        builder
            .Property(p => p.Content)
            .HasMaxLength(280);


        builder
            .Property(p => p.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));


        builder.OwnsOne(p => p.PostImage, postImageBuilder =>
        {
            postImageBuilder.ToTable("PostsImages");

            postImageBuilder.WithOwner().HasForeignKey("PostId");

            postImageBuilder.HasKey("Id", "PostId");

            postImageBuilder
                .Property(pi => pi.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => PostImageId.Create(value));
        });
    }
    private void ConfigureLikesTable(EntityTypeBuilder<Post> builder)
    {
        builder.OwnsMany(p => p.Likes, likeBuilder =>
        {
            likeBuilder.ToTable("Likes");

            likeBuilder.WithOwner().HasForeignKey("PostId");

            likeBuilder.HasKey("Id", "PostId");

            likeBuilder
                .Property(lb => lb.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => LikeId.Create(value));

            likeBuilder
                .Property(lb => lb.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));
        });
        builder.Metadata.FindNavigation(nameof(Post.Likes))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureRepliesTable(EntityTypeBuilder<Post> builder)
    {
        builder.OwnsMany(p => p.Replies, replyBuilder =>
        {
            replyBuilder.ToTable("Replies");

            replyBuilder.WithOwner().HasForeignKey("PostId");

            replyBuilder.HasKey("Id", "PostId");

            replyBuilder
                .Property(rb => rb.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ReplyId.Create(value));

            replyBuilder
                .Property(rb => rb.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));
            
            replyBuilder
            .Property(rb => rb.Content)
                .HasMaxLength(180);
        });
        builder.Metadata.FindNavigation(nameof(Post.Replies))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

}