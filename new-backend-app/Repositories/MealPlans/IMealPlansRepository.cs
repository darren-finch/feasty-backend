using new_backend.Models;

public interface IMealPlansRepository
{
    Task<IList<MealPlan>> GetMealPlansForUser(long userId);
    Task<MealPlan?> GetMealPlanById(long id);
    Task<long> AddMealPlan(MealPlan mealPlan);
    Task<long> UpdateMealPlan(MealPlan mealPlan);
    Task DeleteMealPlan(MealPlan mealPlan);
}