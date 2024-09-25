using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWalletApi.Migrations
{
    /// <inheritdoc />
    public partial class RoleFeeAddAndFewFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "Roles",
                type: "decimal(38,6)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Currencies",
                type: "decimal(38,6)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,0)",
                oldPrecision: 38,
                oldDefaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Roles");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Currencies",
                type: "decimal(38,0)",
                precision: 38,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,6)",
                oldDefaultValue: 0m);
        }
    }
}
