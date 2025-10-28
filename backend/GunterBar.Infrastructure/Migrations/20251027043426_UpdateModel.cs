using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GunterBar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SmsVerificationCode",
                table: "Users",
                type: "varchar(6)",
                maxLength: 6,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "SmsVerificationCodeGeneratedAt",
                table: "Users",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsVerificationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SmsVerificationCodeGeneratedAt",
                table: "Users");
        }
    }
}
