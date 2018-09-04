using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstagramClone.Domain.Migrations
{
    public partial class PicturePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Picture",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
