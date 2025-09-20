using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvc_apps.Migrations
{
    /// <inheritdoc />
    public partial class machinestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MachineStatusTag",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineStatusTag",
                table: "Machines");
        }
    }
}
