using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWarsAPI.Migrations
{
    /// <inheritdoc />
    public partial class StarWars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Faction = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HomeWorld = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Species = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
