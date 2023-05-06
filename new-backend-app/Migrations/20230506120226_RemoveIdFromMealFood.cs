using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace new_backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdFromMealFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MealFoods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "MealFoods",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
