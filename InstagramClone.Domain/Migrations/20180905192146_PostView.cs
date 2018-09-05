using Microsoft.EntityFrameworkCore.Migrations;

namespace InstagramClone.Domain.Migrations
{
    public partial class PostView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Posts",
                newName: "PictureView");

            migrationBuilder.AddColumn<string>(
                name: "PicturePreview",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePreview",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PictureView",
                table: "Posts",
                newName: "Picture");
        }
    }
}
