using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop legacy User table if it exists
            migrationBuilder.Sql(@"IF OBJECT_ID(N'dbo.[User]', N'U') IS NOT NULL DROP TABLE dbo.[User];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op: recreating the original User table schema is not provided here.
            // If you need to roll back, recreate the table and sequence manually or remove this migration.
        }
    }
}
