using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebshop.ConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeTableAnd3Employees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: ["Id", "Name"],
                values: [1, "Toon"]
            );
            migrationBuilder.InsertData(
                table: "Employees",
                columns: ["Id", "Name"],
                values: [2, "Kees"]
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
