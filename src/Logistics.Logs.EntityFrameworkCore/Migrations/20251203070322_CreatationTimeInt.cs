using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Logs.Migrations
{
    /// <inheritdoc />
    public partial class CreatationTimeInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatationTimeInt",
                table: "EntityAuditLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatationTimeInt",
                table: "EntityAuditLogs");
        }
    }
}
