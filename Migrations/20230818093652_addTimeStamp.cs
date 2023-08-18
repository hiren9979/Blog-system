using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog_system.Migrations
{
    /// <inheritdoc />
    public partial class addTimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Blogs",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Blogs");
        }
    }
}
