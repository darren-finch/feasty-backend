using Microsoft.EntityFrameworkCore;
using new_backend.Models;

namespace new_backend.Data
{
    public class FeastyDbContext : DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealFood> MealFoods { get; set; }
        public DbSet<MealPlanMeal> MealPlanMeals { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }

        public FeastyDbContext(DbContextOptions<FeastyDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MealFood>()
                .HasKey(mealFood => new { mealFood.MealId, mealFood.FoodId });

            modelBuilder.Entity<MealFood>()
                .HasOne(mealFood => mealFood.Meal)
                .WithMany(meal => meal.MealFoods)
                .HasForeignKey(mealFood => mealFood.MealId);

            modelBuilder.Entity<MealFood>()
                .HasOne(mealFood => mealFood.BaseFood)
                .WithMany()
                .HasForeignKey(mealFood => mealFood.FoodId);



            modelBuilder.Entity<MealPlanMeal>()
                .HasKey(mealPlanMeal => new { mealPlanMeal.MealId, mealPlanMeal.MealPlanId });

            modelBuilder.Entity<MealPlanMeal>()
                .HasOne(mealPlanMeal => mealPlanMeal.Meal)
                .WithMany()
                .HasForeignKey(mealPlanMeal => mealPlanMeal.MealId);

            modelBuilder.Entity<MealPlanMeal>()
                .HasOne(mealPlanMeal => mealPlanMeal.MealPlan)
                .WithMany(mealPlan => mealPlan.MealPlanMeals)
                .HasForeignKey(mealPlanMeal => mealPlanMeal.MealPlanId);
        }
    }
}

