using Enqueuer.Identity.Persistence.Procedures;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enqueuer.Identity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckAccessFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CheckUserAccessProcedure.Definition);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION check_user_access");
        }
    }
}
