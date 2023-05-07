using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace new_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMealPlanMealsAndMealPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    RequiredCalories = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredFats = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredCarbs = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredProteins = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPlanMeals",
                columns: table => new
                {
                    MealPlanId = table.Column<long>(type: "INTEGER", nullable: false),
                    MealId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlanMeals", x => new { x.MealId, x.MealPlanId });
                    table.ForeignKey(
                        name: "FK_MealPlanMeals_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPlanMeals_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanMeals_MealPlanId",
                table: "MealPlanMeals",
                column: "MealPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealPlanMeals");

            migrationBuilder.DropTable(
                name: "MealPlans");
        }
    }
}
