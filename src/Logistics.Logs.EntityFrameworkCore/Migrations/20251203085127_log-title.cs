using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Logs.Migrations
{
    /// <inheritdoc />
    public partial class logtitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "EntityAuditLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "EntityAuditLogs");
        }
    }
}
