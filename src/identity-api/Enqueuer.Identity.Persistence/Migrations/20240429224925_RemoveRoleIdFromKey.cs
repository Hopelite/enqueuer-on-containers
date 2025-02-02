using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enqueuer.Identity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoleIdFromKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_resource_roles",
                table: "user_resource_roles");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_resource_roles",
                table: "user_resource_roles",
                columns: new[] { "user_id", "resource_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_resource_roles",
                table: "user_resource_roles");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_resource_roles",
                table: "user_resource_roles",
                columns: new[] { "user_id", "role_id", "resource_id" });
        }
    }
}
