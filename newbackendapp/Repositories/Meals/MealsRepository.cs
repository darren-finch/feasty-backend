using Microsoft.EntityFrameworkCore;
using new_backend.Data;
using new_backend.Models;

namespace new_backend.Repositories;

public class MealsRepository : IMealsRepository
{
    private readonly FeastyDbContext dbContext;

    public MealsRepository(FeastyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IList<Meal>> GetAllMealsForUser(long userId)
    {
        return await dbContext.Meals.Where(meal => meal.UserId == userId).Include(meal => meal.MealFoods).ThenInclude(mealFood => mealFood.BaseFood).ToListAsync();
    }

    public async Task<IList<Meal>> GetMealsByTitleForUser(string titleQuery, long userId)
    {
        return await dbContext.Meals.Where(meal => meal.UserId == userId && meal.Title.ToLower().Contains(titleQuery.ToLower())).Include(meal => meal.MealFoods).ThenInclude(mealFood => mealFood.BaseFood).ToListAsync();
    }

    public async Task<Meal?> GetMealById(long mealId)
    {
        return await dbContext.Meals.Include(meal => meal.MealFoods).ThenInclude(mealFood => mealFood.BaseFood).FirstOrDefaultAsync(meal => meal.Id == mealId);
    }

    public async Task<long> AddMeal(Meal meal)
    {
        meal.Id = 0;
        await dbContext.Meals.AddAsync(meal);
        await dbContext.SaveChangesAsync();
        return meal.Id;
    }

    public async Task DeleteMeal(Meal meal)
    {
        dbContext.Meals.Remove(meal);
        await dbContext.SaveChangesAsync();
    }

    public async Task Save()
    {
        await dbContext.SaveChangesAsync();
    }
}