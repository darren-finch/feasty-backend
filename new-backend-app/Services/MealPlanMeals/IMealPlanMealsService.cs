using new_backend.Models;

namespace new_backend.Services;

public interface IMealPlanMealsService
{
    Task SaveMealPlanMeal(MealPlanMeal mealPlanMeal);
    Task DeleteMealPlanMeal(long mealId, long mealPlanId);
}