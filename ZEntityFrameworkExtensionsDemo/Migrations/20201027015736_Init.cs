using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEntityFrameworkExtensionsDemo.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChickenBreeds",
                columns: table => new
                {
                    ChickenBreedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    PrimaryColor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChickenBreeds", x => x.ChickenBreedId);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChickenCoops",
                columns: table => new
                {
                    ChickenCoopId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChickenCoops", x => x.ChickenCoopId);
                    table.ForeignKey(
                        name: "FK_ChickenCoops_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chickens",
                columns: table => new
                {
                    ChickenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsAdoptable = table.Column<bool>(nullable: false),
                    ChickenBreedId = table.Column<int>(nullable: false),
                    ChickenCoopId = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chickens", x => x.ChickenId);
                    table.ForeignKey(
                        name: "FK_Chickens_ChickenBreeds_ChickenBreedId",
                        column: x => x.ChickenBreedId,
                        principalTable: "ChickenBreeds",
                        principalColumn: "ChickenBreedId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chickens_ChickenCoops_ChickenCoopId",
                        column: x => x.ChickenCoopId,
                        principalTable: "ChickenCoops",
                        principalColumn: "ChickenCoopId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chickens_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChickenCoops_OwnerId",
                table: "ChickenCoops",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Chickens_ChickenBreedId",
                table: "Chickens",
                column: "ChickenBreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Chickens_ChickenCoopId",
                table: "Chickens",
                column: "ChickenCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Chickens_OwnerId",
                table: "Chickens",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chickens");

            migrationBuilder.DropTable(
                name: "ChickenBreeds");

            migrationBuilder.DropTable(
                name: "ChickenCoops");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
