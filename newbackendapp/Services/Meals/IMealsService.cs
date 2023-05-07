using new_backend.Models;

namespace new_backend.Services;

public interface IMealsService
{
    Task<IList<Meal>> GetMeals(string? titleQuery = null);

    Task<Meal> GetMealById(long mealId);

    Task<long> SaveMeal(Meal meal);

    Task DeleteMeal(long mealId);
}