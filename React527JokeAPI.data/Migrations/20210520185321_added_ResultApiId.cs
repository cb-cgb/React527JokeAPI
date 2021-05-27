using Microsoft.EntityFrameworkCore.Migrations;

namespace React527JokeAPI.data.Migrations
{
    public partial class added_ResultApiId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultJokeId",
                table: "Jokes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultJokeId",
                table: "Jokes");
        }
    }
}
