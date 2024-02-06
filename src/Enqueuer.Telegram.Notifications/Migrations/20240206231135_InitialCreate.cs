using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enqueuer.Telegram.Notifications.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    code = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_languages", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "chat_configurations",
                columns: table => new
                {
                    chat_id = table.Column<long>(type: "INTEGER", nullable: false),
                    notifications_language_code = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_configurations", x => x.chat_id);
                    table.ForeignKey(
                        name: "fk_chat_configurations_available_languages_notifications_language_code",
                        column: x => x.notifications_language_code,
                        principalTable: "languages",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "localized_messages",
                columns: table => new
                {
                    language_code = table.Column<string>(type: "TEXT", nullable: false),
                    key = table.Column<string>(type: "TEXT", nullable: false),
                    value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localized_messages", x => new { x.language_code, x.key });
                    table.ForeignKey(
                        name: "fk_localized_messages_available_languages_language_code",
                        column: x => x.language_code,
                        principalTable: "languages",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_chat_configurations_notifications_language_code",
                table: "chat_configurations",
                column: "notifications_language_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_configurations");

            migrationBuilder.DropTable(
                name: "localized_messages");

            migrationBuilder.DropTable(
                name: "languages");
        }
    }
}
