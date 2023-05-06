using Microsoft.EntityFrameworkCore;
using new_backend.Data;
using new_backend.Models;

namespace new_backend.Repositories;

public class MealPlanMealsRepository
{
    private readonly FeastyDbContext dbContext;

    public MealPlanMealsRepository(FeastyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<MealPlanMeal?> GetMealPlanMeal(long mealId, long mealPlanId)
    {
        return await dbContext.MealPlanMeals
            .Include(mealPlanMeal => mealPlanMeal.Meal)
            .FirstOrDefaultAsync(mealPlanMeal => mealPlanMeal.MealId == mealId && mealPlanMeal.MealPlanId == mealPlanId);
    }

    public async Task SaveMealPlanMeal(MealPlanMeal mealPlanMeal)
    {
        dbContext.MealPlanMeals.Update(mealPlanMeal);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteMealPlanMeal(MealPlanMeal mealPlanMeal)
    {
        dbContext.MealPlanMeals.Remove(mealPlanMeal);
        await dbContext.SaveChangesAsync();
    }
}