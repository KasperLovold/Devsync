using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSync.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMember_ProjectId",
                table: "ProjectMember");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProjectMember");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember",
                columns: new[] { "ProjectId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProjectMember",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMember",
                table: "ProjectMember",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMember_ProjectId",
                table: "ProjectMember",
                column: "ProjectId");
        }
    }
}
