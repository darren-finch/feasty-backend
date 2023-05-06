using new_backend.Models;

namespace new_backend.Repositories;

public interface IMealsRepository
{
    Task<IList<Meal>> GetAllMealsForUser(long userId);
    Task<IList<Meal>> GetMealsByTitleForUser(string titleQuery, long userId);
    Task<Meal?> GetMealById(long mealId);
    Task<long> AddMeal(Meal meal);
    Task<long> UpdateMeal(Meal meal);
    Task DeleteMeal(Meal meal);
}