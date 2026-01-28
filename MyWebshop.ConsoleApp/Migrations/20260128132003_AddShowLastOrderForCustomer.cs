using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebshop.ConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddShowLastOrderForCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR ALTER PROCEDURE dbo.ShowLastOrderForCustomer
                @customerId AS int
            AS
            BEGIN
	            SELECT TOP 1 *
	            FROM Orders AS o
	            WHERE o.CustomerId = @customerId
	            ORDER BY o.OrderDate DESC;
            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.ShowLastOrderForCustomer");
        }
    }
}
