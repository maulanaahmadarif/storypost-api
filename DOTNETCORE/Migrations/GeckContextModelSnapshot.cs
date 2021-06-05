﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using geckserver.Data;

namespace geckserver.Migrations
{
    [DbContext(typeof(StoryPostV2Context))]
    partial class StoryPostV2ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("geckserver.Data.ListMenu", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("deleted_at");

                    b.Property<string>("MenuName")
                        .HasColumnType("text")
                        .HasColumnName("menu_name");

                    b.Property<long?>("UserRoleId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_role_id");

                    b.HasKey("Id");

                    b.HasIndex("UserRoleId");

                    b.HasIndex(new[] { "Id", "UserRoleId" }, "list_menu_index");

                    b.ToTable("ListMenu");
                });

            modelBuilder.Entity("geckserver.Data.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<long?>("FromId")
                        .HasColumnType("bigint")
                        .HasColumnName("from_id");

                    b.Property<long?>("PostDataId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_data_id");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("text")
                        .HasColumnName("thumbnail");

                    b.Property<int?>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.Property<int?>("Viewed")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostDataId");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Id", "UserId", "PostDataId", "FromId" }, "notif_index");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("geckserver.Data.PostCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("IsReported")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("slug");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Slug" }, "PostCategory_index_3")
                        .IsUnique()
                        .HasFilter("[slug] IS NOT NULL");

                    b.HasIndex(new[] { "Id", "Slug" }, "post_category_index");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("geckserver.Data.PostDatum", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Caption")
                        .HasColumnType("text")
                        .HasColumnName("caption");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Location")
                        .HasColumnType("text")
                        .HasColumnName("location");

                    b.Property<long?>("PostCategoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_category_id");

                    b.Property<string>("Slug")
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("PostCategoryId");

                    b.HasIndex(new[] { "Id", "PostCategoryId", "UserId" }, "post_data_index");

                    b.ToTable("PostData");
                });

            modelBuilder.Entity("geckserver.Data.PostImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Path")
                        .HasColumnType("text")
                        .HasColumnName("path");

                    b.Property<long?>("PostDataId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_data_id");

                    b.HasKey("Id");

                    b.HasIndex("PostDataId");

                    b.HasIndex(new[] { "Id", "PostDataId" }, "post_image_index");

                    b.ToTable("PostImage");
                });

            modelBuilder.Entity("geckserver.Data.PostLike", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("deleted_at");

                    b.Property<long?>("PostDataId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_data_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("PostDataId");

                    b.HasIndex(new[] { "UserId", "PostDataId" }, "post_like_index");

                    b.ToTable("PostLike");
                });

            modelBuilder.Entity("geckserver.Data.PostReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<int?>("IsDecline")
                        .HasColumnType("int")
                        .HasColumnName("isDecline");

                    b.Property<int?>("IsEmailed")
                        .HasColumnType("int")
                        .HasColumnName("isEmailed");

                    b.Property<int?>("IsRemove")
                        .HasColumnType("int")
                        .HasColumnName("isRemove");

                    b.Property<long?>("PostDataId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_data_id");

                    b.Property<string>("Reason")
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<string>("StatusDesc")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("status_desc");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("PostDataId");

                    b.ToTable("PostReports");
                });

            modelBuilder.Entity("geckserver.Data.PostTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("PostDataId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_data_id");

                    b.Property<string>("TagData")
                        .HasColumnType("text")
                        .HasColumnName("tagData");

                    b.HasKey("Id");

                    b.HasIndex("PostDataId");

                    b.HasIndex(new[] { "Id", "PostDataId" }, "post_tag_index");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("geckserver.Data.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("Confirmed")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Facebook")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Instagram")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("IsBanned")
                        .HasColumnType("int");

                    b.Property<int?>("IsDownload")
                        .HasColumnType("int");

                    b.Property<int?>("IsEmailBanned")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Nip")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("NIP");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Phone")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Picture")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<long?>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<string>("Secretcode")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("status");

                    b.Property<string>("Twitter")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Username")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "Id" }, "user_index");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("geckserver.Data.UserAcitivity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime")
                        .HasColumnName("last_login");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Id", "UserId" }, "user_activity_index");

                    b.ToTable("UserAcitivity");
                });

            modelBuilder.Entity("geckserver.Data.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("deleted_at");

                    b.Property<string>("RoleName")
                        .HasColumnType("text")
                        .HasColumnName("role_name");

                    b.HasKey("Id");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("geckserver.Data.ListMenu", b =>
                {
                    b.HasOne("geckserver.Data.UserRole", "UserRole")
                        .WithMany("ListMenus")
                        .HasForeignKey("UserRoleId")
                        .HasConstraintName("FK__ListMenu__user_r__74794A92");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("geckserver.Data.Notification", b =>
                {
                    b.HasOne("geckserver.Data.PostDatum", "PostData")
                        .WithMany("Notifications")
                        .HasForeignKey("PostDataId")
                        .HasConstraintName("FK__Notificat__post___7C1A6C5A");

                    b.HasOne("geckserver.Data.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Notificat__user___7A3223E8");

                    b.Navigation("PostData");

                    b.Navigation("User");
                });

            modelBuilder.Entity("geckserver.Data.PostDatum", b =>
                {
                    b.HasOne("geckserver.Data.PostCategory", "PostCategory")
                        .WithMany("PostData")
                        .HasForeignKey("PostCategoryId")
                        .HasConstraintName("FK__PostData__post_c__756D6ECB");

                    b.Navigation("PostCategory");
                });

            modelBuilder.Entity("geckserver.Data.PostImage", b =>
                {
                    b.HasOne("geckserver.Data.PostDatum", "PostData")
                        .WithMany("PostImages")
                        .HasForeignKey("PostDataId")
                        .HasConstraintName("FK__PostImage__post___76619304");

                    b.Navigation("PostData");
                });

            modelBuilder.Entity("geckserver.Data.PostLike", b =>
                {
                    b.HasOne("geckserver.Data.PostDatum", "PostData")
                        .WithMany("PostLikes")
                        .HasForeignKey("PostDataId")
                        .HasConstraintName("FK__PostLike__post_d__7849DB76");

                    b.Navigation("PostData");
                });

            modelBuilder.Entity("geckserver.Data.PostReport", b =>
                {
                    b.HasOne("geckserver.Data.PostDatum", "PostData")
                        .WithMany("PostReports")
                        .HasForeignKey("PostDataId")
                        .HasConstraintName("FK__PostRepor__post___7B264821");

                    b.Navigation("PostData");
                });

            modelBuilder.Entity("geckserver.Data.PostTag", b =>
                {
                    b.HasOne("geckserver.Data.PostDatum", "PostData")
                        .WithMany("PostTags")
                        .HasForeignKey("PostDataId")
                        .HasConstraintName("FK__PostTag__post_da__7755B73D");

                    b.Navigation("PostData");
                });

            modelBuilder.Entity("geckserver.Data.User", b =>
                {
                    b.HasOne("geckserver.Data.UserRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__Users__role_id__7D0E9093");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("geckserver.Data.UserAcitivity", b =>
                {
                    b.HasOne("geckserver.Data.User", "User")
                        .WithMany("UserAcitivities")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserAciti__user___793DFFAF");

                    b.Navigation("User");
                });

            modelBuilder.Entity("geckserver.Data.PostCategory", b =>
                {
                    b.Navigation("PostData");
                });

            modelBuilder.Entity("geckserver.Data.PostDatum", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("PostImages");

                    b.Navigation("PostLikes");

                    b.Navigation("PostReports");

                    b.Navigation("PostTags");
                });

            modelBuilder.Entity("geckserver.Data.User", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("UserAcitivities");
                });

            modelBuilder.Entity("geckserver.Data.UserRole", b =>
                {
                    b.Navigation("ListMenus");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
