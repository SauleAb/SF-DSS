using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SF_DSS.Data.Migrations
{
    /// <inheritdoc />
    public partial class DB003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Messsage",
                table: "Messages",
                newName: "MessageContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageContent",
                table: "Messages",
                newName: "Messsage");
        }
    }
}
