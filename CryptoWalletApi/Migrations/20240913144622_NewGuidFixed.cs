using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWalletApi.Migrations
{
    /// <inheritdoc />
    public partial class NewGuidFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Wallets",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("6a544486-9c70-4c22-8c2d-ed284bcc329d"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Wallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("6a544486-9c70-4c22-8c2d-ed284bcc329d"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
