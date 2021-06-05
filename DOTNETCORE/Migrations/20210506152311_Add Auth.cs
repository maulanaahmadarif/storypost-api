using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace geckserver.Migrations
{
    public partial class AddAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            // migrationBuilder.CreateTable(
            //     name: "PostCategory",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //         Name = table.Column<string>(type: "text", nullable: false),
            //         IsReported = table.Column<int>(type: "int", nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
            //         DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PostCategory", x => x.id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "UserRole",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         role_name = table.Column<string>(type: "text", nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_UserRole", x => x.id);
            //     });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.CreateTable(
            //     name: "PostData",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         user_id = table.Column<int>(type: "int", nullable: true),
            //         post_category_id = table.Column<long>(type: "bigint", nullable: true),
            //         slug = table.Column<string>(type: "text", nullable: true),
            //         location = table.Column<string>(type: "text", nullable: true),
            //         caption = table.Column<string>(type: "text", nullable: true),
            //         created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //         updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PostData", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__PostData__post_c__756D6ECB",
            //             column: x => x.post_category_id,
            //             principalTable: "PostCategory",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ListMenu",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         menu_name = table.Column<string>(type: "text", nullable: true),
            //         user_role_id = table.Column<long>(type: "bigint", nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ListMenu", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__ListMenu__user_r__74794A92",
            //             column: x => x.user_role_id,
            //             principalTable: "UserRole",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Users",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //         Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         Phone = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         Facebook = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         Twitter = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         Instagram = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         Picture = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //         NIP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //         Account = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //         status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //         CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
            //         UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
            //         DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
            //         Confirmed = table.Column<int>(type: "int", nullable: true),
            //         Secretcode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //         IsBanned = table.Column<int>(type: "int", nullable: true),
            //         IsDownload = table.Column<int>(type: "int", nullable: true),
            //         IsEmailBanned = table.Column<int>(type: "int", nullable: true),
            //         RoleId = table.Column<long>(type: "bigint", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Users", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK__Users__role_id__7D0E9093",
            //             column: x => x.RoleId,
            //             principalTable: "UserRole",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "PostImage",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         post_data_id = table.Column<long>(type: "bigint", nullable: true),
            //         path = table.Column<string>(type: "text", nullable: true),
            //         created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PostImage", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__PostImage__post___76619304",
            //             column: x => x.post_data_id,
            //             principalTable: "PostData",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "PostLike",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         user_id = table.Column<int>(type: "int", nullable: true),
            //         post_data_id = table.Column<long>(type: "bigint", nullable: true),
            //         created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //         deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PostLike", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__PostLike__post_d__7849DB76",
            //             column: x => x.post_data_id,
            //             principalTable: "PostData",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "PostReports",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         post_data_id = table.Column<long>(type: "bigint", nullable: true),
            //         user_id = table.Column<long>(type: "bigint", nullable: true),
            //         reason = table.Column<string>(type: "text", nullable: true),
            //         created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //         status_desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //         isDecline = table.Column<int>(type: "int", nullable: true),
            //         isRemove = table.Column<int>(type: "int", nullable: true),
            //         isEmailed = table.Column<int>(type: "int", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PostReports", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__PostRepor__post___7B264821",
            //             column: x => x.post_data_id,
            //             principalTable: "PostData",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "PostTag",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         tagData = table.Column<string>(type: "text", nullable: true),
            //         post_data_id = table.Column<long>(type: "bigint", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_PostTag", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__PostTag__post_da__7755B73D",
            //             column: x => x.post_data_id,
            //             principalTable: "PostData",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Notifications",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         user_id = table.Column<long>(type: "bigint", nullable: true),
            //         post_data_id = table.Column<long>(type: "bigint", nullable: true),
            //         from_id = table.Column<long>(type: "bigint", nullable: true),
            //         type = table.Column<int>(type: "int", nullable: true),
            //         thumbnail = table.Column<string>(type: "text", nullable: true),
            //         Viewed = table.Column<int>(type: "int", nullable: true),
            //         created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //         updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Notifications", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__Notificat__post___7C1A6C5A",
            //             column: x => x.post_data_id,
            //             principalTable: "PostData",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK__Notificat__user___7A3223E8",
            //             column: x => x.user_id,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "UserAcitivity",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         user_id = table.Column<long>(type: "bigint", nullable: true),
            //         last_login = table.Column<DateTime>(type: "datetime", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_UserAcitivity", x => x.id);
            //         table.ForeignKey(
            //             name: "FK__UserAciti__user___793DFFAF",
            //             column: x => x.user_id,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            // migrationBuilder.CreateIndex(
            //     name: "IX_ListMenu_user_role_id",
            //     table: "ListMenu",
            //     column: "user_role_id");

            // migrationBuilder.CreateIndex(
            //     name: "list_menu_index",
            //     table: "ListMenu",
            //     columns: new[] { "id", "user_role_id" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Notifications_post_data_id",
            //     table: "Notifications",
            //     column: "post_data_id");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Notifications_user_id",
            //     table: "Notifications",
            //     column: "user_id");

            // migrationBuilder.CreateIndex(
            //     name: "notif_index",
            //     table: "Notifications",
            //     columns: new[] { "id", "user_id", "post_data_id", "from_id" });

            // migrationBuilder.CreateIndex(
            //     name: "post_category_index",
            //     table: "PostCategory",
            //     columns: new[] { "id", "slug" });

            // migrationBuilder.CreateIndex(
            //     name: "PostCategory_index_3",
            //     table: "PostCategory",
            //     column: "slug",
            //     unique: true,
            //     filter: "[slug] IS NOT NULL");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PostData_post_category_id",
            //     table: "PostData",
            //     column: "post_category_id");

            // migrationBuilder.CreateIndex(
            //     name: "post_data_index",
            //     table: "PostData",
            //     columns: new[] { "id", "post_category_id", "user_id" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_PostImage_post_data_id",
            //     table: "PostImage",
            //     column: "post_data_id");

            // migrationBuilder.CreateIndex(
            //     name: "post_image_index",
            //     table: "PostImage",
            //     columns: new[] { "id", "post_data_id" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_PostLike_post_data_id",
            //     table: "PostLike",
            //     column: "post_data_id");

            // migrationBuilder.CreateIndex(
            //     name: "post_like_index",
            //     table: "PostLike",
            //     columns: new[] { "user_id", "post_data_id" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_PostReports_post_data_id",
            //     table: "PostReports",
            //     column: "post_data_id");

            // migrationBuilder.CreateIndex(
            //     name: "IX_PostTag_post_data_id",
            //     table: "PostTag",
            //     column: "post_data_id");

            // migrationBuilder.CreateIndex(
            //     name: "post_tag_index",
            //     table: "PostTag",
            //     columns: new[] { "id", "post_data_id" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserAcitivity_user_id",
            //     table: "UserAcitivity",
            //     column: "user_id");

            // migrationBuilder.CreateIndex(
            //     name: "user_activity_index",
            //     table: "UserAcitivity",
            //     columns: new[] { "id", "user_id" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Users_RoleId",
            //     table: "Users",
            //     column: "RoleId");

            // migrationBuilder.CreateIndex(
            //     name: "user_index",
            //     table: "Users",
            //     column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            // migrationBuilder.DropTable(
            //     name: "ListMenu");

            // migrationBuilder.DropTable(
            //     name: "Notifications");

            // migrationBuilder.DropTable(
            //     name: "PostImage");

            // migrationBuilder.DropTable(
            //     name: "PostLike");

            // migrationBuilder.DropTable(
            //     name: "PostReports");

            // migrationBuilder.DropTable(
            //     name: "PostTag");

            // migrationBuilder.DropTable(
            //     name: "UserAcitivity");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            // migrationBuilder.DropTable(
            //     name: "PostData");

            // migrationBuilder.DropTable(
            //     name: "Users");

            // migrationBuilder.DropTable(
            //     name: "PostCategory");

            // migrationBuilder.DropTable(
            //     name: "UserRole");
        }
    }
}
