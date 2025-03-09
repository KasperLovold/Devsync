using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSync.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "UserModel",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "UserModel",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserModel",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserModel",
                type: "character varying(524)",
                maxLength: 524,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "UserModel",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "UserModel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "UserModel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "UserModel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "UserModel",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "UserModel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "UserModel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "UserModel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "UserModel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "UserModel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "UserModel",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "UserModel");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserModel",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(524)",
                oldMaxLength: 524,
                oldNullable: true);
        }
    }
}
