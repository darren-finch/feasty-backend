using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace new_backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdFromMealFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods");

            migrationBuilder.DropIndex(
                name: "IX_MealFoods_MealId",
                table: "MealFoods");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MealFoods");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "MealFoods",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods",
                columns: new[] { "MealId", "FoodId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "MealFoods",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "MealFoods",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MealFoods_MealId",
                table: "MealFoods",
                column: "MealId");
        }
    }
}
