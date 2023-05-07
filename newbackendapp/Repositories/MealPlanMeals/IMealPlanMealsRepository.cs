using new_backend.Models;

namespace new_backend.Repositories;

public interface IMealPlanMealsRepository
{
    Task<MealPlanMeal?> GetMealPlanMeal(long mealId, long mealPlanId);
    Task SaveMealPlanMeal(MealPlanMeal mealPlanMeal);
    Task DeleteMealPlanMeal(MealPlanMeal mealPlanMeal);
}