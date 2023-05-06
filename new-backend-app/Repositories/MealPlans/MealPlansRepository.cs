using Microsoft.EntityFrameworkCore;
using new_backend.Data;
using new_backend.Models;

namespace new_backend.Repositories;

public class MealPlansRepository : IMealPlansRepository
{
    private readonly FeastyDbContext dbContext;

    public MealPlansRepository(FeastyDbContext appDbContext)
    {
        this.dbContext = appDbContext;
    }

    // Does not include meals.
    public async Task<IList<MealPlan>> GetMealPlansForUser(long userId)
    {
        return await dbContext.MealPlans.Where(mealPlan => mealPlan.UserId == userId).ToListAsync();
    }

    // Includes meal data.
    public async Task<MealPlan?> GetMealPlanById(long mealPlanId)
    {
        return await dbContext.MealPlans.Include(mealPlan => mealPlan.MealPlanMeals)
            .ThenInclude(mealPlanMeal => mealPlanMeal.Meal)
            .ThenInclude(meal => meal.MealFoods)
            .ThenInclude(mealFood => mealFood.BaseFood)
            .FirstOrDefaultAsync(mealPlan => mealPlan.Id == mealPlanId);
    }

    // Does not save meals.
    public async Task<long> AddMealPlan(MealPlan newMealPlan)
    {
        newMealPlan.Id = 0;

        dbContext.MealPlans.Add(newMealPlan);

        await dbContext.SaveChangesAsync();

        return newMealPlan.Id;
    }

    // Does not save meals.
    public async Task<long> UpdateMealPlan(MealPlan updatedMealPlan)
    {
        await dbContext.SaveChangesAsync();
        return updatedMealPlan.Id;
    }

    public async Task DeleteMealPlan(MealPlan mealPlan)
    {
        dbContext.MealPlans.Remove(mealPlan);
        await dbContext.SaveChangesAsync();
    }
}