﻿// <auto-generated />
using System;
using ActvShare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ActvShare.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230913235049_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ActvShare.Domain.Chats.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("user1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("user2")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Chats", (string)null);
                });

            modelBuilder.Entity("ActvShare.Domain.Posts.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasMaxLength(280)
                        .HasColumnType("nvarchar(280)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("ActvShare.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("ActvShare.Domain.Chats.Chat", b =>
                {
                    b.OwnsMany("ActvShare.Domain.Chats.Entities.ChatMessage", "Messages", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("ChatId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Content")
                                .IsRequired()
                                .HasMaxLength(120)
                                .HasColumnType("nvarchar(120)");

                            b1.Property<Guid>("SenderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("SentAt")
                                .HasColumnType("datetime2");

                            b1.HasKey("Id", "ChatId");

                            b1.HasIndex("ChatId");

                            b1.ToTable("ChatMessages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ChatId");
                        });

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ActvShare.Domain.Posts.Post", b =>
                {
                    b.OwnsMany("ActvShare.Domain.Posts.Entities.Like", "Likes", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("PostId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("datetime2");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id", "PostId");

                            b1.HasIndex("PostId");

                            b1.ToTable("Likes", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.OwnsOne("ActvShare.Domain.Posts.Entities.PostImage", "PostImage", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("PostId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("ContentType")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<long>("FileSize")
                                .HasColumnType("bigint");

                            b1.Property<string>("OriginalFileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("StoredFileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("UploadedAt")
                                .HasColumnType("datetime2");

                            b1.HasKey("Id", "PostId");

                            b1.HasIndex("PostId")
                                .IsUnique();

                            b1.ToTable("PostsImages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.OwnsMany("ActvShare.Domain.Posts.Entities.Reply", "Replies", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("PostId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Content")
                                .IsRequired()
                                .HasMaxLength(180)
                                .HasColumnType("nvarchar(180)");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("UpdatedAt")
                                .HasColumnType("datetime2");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id", "PostId");

                            b1.HasIndex("PostId");

                            b1.ToTable("Replies", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.Navigation("Likes");

                    b.Navigation("PostImage");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("ActvShare.Domain.Users.User", b =>
                {
                    b.OwnsMany("ActvShare.Domain.Users.Entities.Follow", "Follows", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("FollowedUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id", "UserId");

                            b1.HasIndex("UserId");

                            b1.ToTable("Follows", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsMany("ActvShare.Domain.Users.Entities.Notification", "Notifications", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("IsRead")
                                .HasColumnType("bit");

                            b1.Property<string>("Message")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.HasKey("Id", "UserId");

                            b1.HasIndex("UserId");

                            b1.ToTable("Notifications", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("ActvShare.Domain.Users.Entities.ProfileImage", "ProfileImage", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("ContentType")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<long>("FileSize")
                                .HasColumnType("bigint");

                            b1.Property<string>("OriginalFileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("StoredFileName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("UploadedAt")
                                .HasColumnType("datetime2");

                            b1.HasKey("Id", "UserId");

                            b1.HasIndex("UserId")
                                .IsUnique();

                            b1.ToTable("ProfileImages", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Follows");

                    b.Navigation("Notifications");

                    b.Navigation("ProfileImage")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
