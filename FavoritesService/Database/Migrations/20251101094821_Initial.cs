using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FavoritesService.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCurrencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyId = table.Column<string>(type: "character varying(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrencies_CurrencyId",
                table: "UserCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrencies_UserId",
                table: "UserCurrencies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrencies_UserId_CurrencyId",
                table: "UserCurrencies",
                columns: new[] { "UserId", "CurrencyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCurrencies");
        }
    }
}
