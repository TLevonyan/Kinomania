using Microsoft.EntityFrameworkCore.Migrations;

namespace Kinomania.Data.Migrations
{
    public partial class ChangedFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersGenres");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FilmsGenres");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ActorsFilms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersGenres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FilmsGenres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ActorsFilms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
