using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace geckserver.Data
{
    public partial class StoryPostV2Context : DbContext
    {

        public StoryPostV2Context(DbContextOptions<StoryPostV2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<DynamicPage> DynamicPages { get; set; }
        public virtual DbSet<EventPost> EventPosts { get; set; }
        public virtual DbSet<EventPostDetail> EventPostDetails { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<ListMenu> ListMenus { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
        public virtual DbSet<PostData> PostData { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        public virtual DbSet<PostLike> PostLikes { get; set; }
        public virtual DbSet<PostReport> PostReports { get; set; }
        public virtual DbSet<PostTag> PostTags { get; set; }
        public virtual DbSet<TagData> TagData { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAcitivity> UserAcitivities { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                                                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                .AddJsonFile("appsettings.json")
                                                .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("AzureConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DynamicPage>(entity =>
            {
                entity.ToTable("DynamicPage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_at");

                entity.Property(e => e.Ordering).HasColumnName("ordering");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<EventPost>(entity =>
            {
                entity.ToTable("EventPost");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_at");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("endDate");

                entity.Property(e => e.PostCategoryId).HasColumnName("post_category_id");

                entity.Property(e => e.PrizePool)
                    .HasColumnType("text")
                    .HasColumnName("prizePool");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("startDate");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Winner).HasColumnName("winner");

                entity.HasOne(d => d.PostCategory)
                    .WithMany(p => p.EventPosts)
                    .HasForeignKey(d => d.PostCategoryId)
                    .HasConstraintName("FK__EventPost__post___00AA174D");
            });

            modelBuilder.Entity<EventPostDetail>(entity =>
            {
                entity.ToTable("EventPostDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventPostId).HasColumnName("event_post_id");

                entity.Property(e => e.PostDataId).HasColumnName("post_data_id");

                entity.HasOne(d => d.EventPost)
                    .WithMany(p => p.EventPostDetails)
                    .HasForeignKey(d => d.EventPostId)
                    .HasConstraintName("FK__EventPost__event__7EC1CEDB");

                entity.HasOne(d => d.PostData)
                    .WithMany(p => p.EventPostDetails)
                    .HasForeignKey(d => d.PostDataId)
                    .HasConstraintName("FK__EventPost__post___7FB5F314");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.Subject)
                    .HasMaxLength(255)
                    .HasColumnName("subject");
            });

            modelBuilder.Entity<ListMenu>(entity =>
            {
                entity.ToTable("ListMenu");

                entity.HasIndex(e => new { e.Id, e.UserRoleId }, "list_menu_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_at");

                entity.Property(e => e.MenuName)
                    .HasColumnType("text")
                    .HasColumnName("menu_name");

                entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.ListMenus)
                    .HasForeignKey(d => d.UserRoleId)
                    .HasConstraintName("FK__ListMenu__user_r__74794A92");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.UserId, e.PostDataId, e.FromId }, "notif_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.FromId).HasColumnName("from_id");

                entity.Property(e => e.PostDataId).HasColumnName("post_data_id");

                entity.Property(e => e.Thumbnail)
                    .HasColumnType("text")
                    .HasColumnName("thumbnail");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.PostData)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.PostDataId)
                    .HasConstraintName("FK__Notificat__post___7C1A6C5A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__user___7A3223E8");
            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.ToTable("PostCategory");

                entity.HasIndex(e => e.Slug, "PostCategory_index_3")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.Slug }, "post_category_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .HasColumnName("slug");

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("uid");
            });

            modelBuilder.Entity<PostData>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("caption");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_at");

                entity.Property(e => e.IsEvent).HasColumnName("isEvent");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("location");

                entity.Property(e => e.PostCategoryId).HasColumnName("post_category_id");

                entity.Property(e => e.Title)
                    .HasColumnType("text")
                    .HasColumnName("title");

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("uid");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.WeeklyId).HasColumnName("weekly_id");

                entity.HasOne(d => d.PostCategory)
                    .WithMany(p => p.PostData)
                    .HasForeignKey(d => d.PostCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostData__post_c__756D6ECB");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostData)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostData__user_i__3FD07829");
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.ToTable("PostImage");

                entity.HasIndex(e => new { e.Id, e.PostDataId }, "post_image_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_at");

                entity.Property(e => e.Path)
                    .HasColumnType("text")
                    .HasColumnName("path");

                entity.Property(e => e.PostDataId).HasColumnName("post_data_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.HasOne(d => d.PostData)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.PostDataId)
                    .HasConstraintName("FK__PostImage__post___76619304");
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.ToTable("PostLike");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_at");

                entity.Property(e => e.PostDataId).HasColumnName("post_data_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.PostData)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.PostDataId)
                    .HasConstraintName("FK__PostLike__post_d__7849DB76");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostLike__user_d");
            });

            modelBuilder.Entity<PostReport>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsDecline).HasColumnName("isDecline");

                entity.Property(e => e.IsEmailed).HasColumnName("isEmailed");

                entity.Property(e => e.IsRemove).HasColumnName("isRemove");

                entity.Property(e => e.PostDataId).HasColumnName("post_data_id");

                entity.Property(e => e.Reason)
                    .HasColumnType("text")
                    .HasColumnName("reason");

                entity.Property(e => e.StatusDesc)
                    .HasMaxLength(255)
                    .HasColumnName("status_desc");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.PostData)
                    .WithMany(p => p.PostReports)
                    .HasForeignKey(d => d.PostDataId)
                    .HasConstraintName("FK__PostRepor__post___7B264821");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostReports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PostRepor__user___68D28DBC");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.ToTable("PostTag");

                entity.HasIndex(e => new { e.Id, e.PostDataId }, "post_tag_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostDataId).HasColumnName("post_data_id");

                entity.Property(e => e.TagDataId).HasColumnName("tag_data_id");

                entity.HasOne(d => d.PostData)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.PostDataId)
                    .HasConstraintName("FK__PostTag__post_da__7755B73D");

                entity.HasOne(d => d.TagData)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.TagDataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostTag__tag_dat__42ACE4D4");
            });

            modelBuilder.Entity<TagData>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TagName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("tagName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Id, "user_index");

                entity.Property(e => e.Account).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Facebook)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Instagram)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nip)
                    .HasMaxLength(255)
                    .HasColumnName("NIP");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Secretcode).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasColumnName("status");

                entity.Property(e => e.Twitter)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Username).HasMaxLength(255);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__role_id__7D0E9093");
            });

            modelBuilder.Entity<UserAcitivity>(entity =>
            {
                entity.ToTable("UserAcitivity");

                entity.HasIndex(e => new { e.Id, e.UserId }, "user_activity_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("last_login");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAcitivities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserAciti__user___793DFFAF");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_at");

                entity.Property(e => e.RoleName)
                    .HasColumnType("text")
                    .HasColumnName("role_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
