using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: true),
                    filepath = table.Column<string>(name: "file_path", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    avatarid = table.Column<int>(name: "avatar_id", type: "integer", nullable: true),
                    lastseen = table.Column<string>(name: "last_seen", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Avatars_avatar_id",
                        column: x => x.avatarid,
                        principalTable: "Avatars",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid1 = table.Column<int>(name: "userid_1", type: "integer", nullable: false),
                    userid2 = table.Column<int>(name: "userid_2", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Users_userid_1",
                        column: x => x.userid1,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_Users_userid_2",
                        column: x => x.userid2,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messeges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chatid = table.Column<int>(name: "chat_id", type: "integer", nullable: false),
                    messege = table.Column<string>(type: "text", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    sentdate = table.Column<string>(name: "sent_date", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messeges", x => x.id);
                    table.ForeignKey(
                        name: "FK_Messeges_Chats_chat_id",
                        column: x => x.chatid,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messeges_Users_user_id",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_user_id",
                table: "Avatars",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_userid_1",
                table: "Chats",
                column: "userid_1");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_userid_2",
                table: "Chats",
                column: "userid_2");

            migrationBuilder.CreateIndex(
                name: "IX_Messeges_chat_id",
                table: "Messeges",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messeges_user_id",
                table: "Messeges",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_avatar_id",
                table: "Users",
                column: "avatar_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Users_user_id",
                table: "Avatars",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_Users_user_id",
                table: "Avatars");

            migrationBuilder.DropTable(
                name: "Messeges");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Avatars");
        }
    }
}
