using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplytekTest.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectCode",
                table: "Projects",
                column: "ProjectCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectCode",
                table: "Projects");
        }
    }
}
