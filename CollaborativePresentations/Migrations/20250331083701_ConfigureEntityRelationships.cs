using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollaborativePresentations.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureEntityRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Presentations_PresentationId",
                table: "Slides");

            migrationBuilder.DropForeignKey(
                name: "FK_TextBlocks_Slides_SlideId",
                table: "TextBlocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "UserConnections",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections",
                column: "ConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Presentations_PresentationId",
                table: "Slides",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextBlocks_Slides_SlideId",
                table: "TextBlocks",
                column: "SlideId",
                principalTable: "Slides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Presentations_PresentationId",
                table: "Slides");

            migrationBuilder.DropForeignKey(
                name: "FK_TextBlocks_Slides_SlideId",
                table: "TextBlocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "UserConnections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Presentations_PresentationId",
                table: "Slides",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TextBlocks_Slides_SlideId",
                table: "TextBlocks",
                column: "SlideId",
                principalTable: "Slides",
                principalColumn: "Id");
        }
    }
}
