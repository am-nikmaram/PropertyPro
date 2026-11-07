using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PropertyPro.Infrastructure.Persistence;

#nullable disable

namespace PropertyPro.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20261107000000_AddUserOrganization")]
public partial class AddUserOrganization : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "OrganizationId",
            table: "AspNetUsers",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUsers_OrganizationId",
            table: "AspNetUsers",
            column: "OrganizationId");

        migrationBuilder.AddForeignKey(
            name: "FK_AspNetUsers_Organizations_OrganizationId",
            table: "AspNetUsers",
            column: "OrganizationId",
            principalTable: "Organizations",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AspNetUsers_Organizations_OrganizationId",
            table: "AspNetUsers");

        migrationBuilder.DropIndex(
            name: "IX_AspNetUsers_OrganizationId",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "OrganizationId",
            table: "AspNetUsers");
    }
}
