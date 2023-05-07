using new_backend.Models;

namespace new_backend.Services;
public interface IMealPlansService
{
    Task<IList<MealPlan>> GetMealPlans();
    Task<MealPlan> GetMealPlanById(long mealPlanId);
    Task<long> SaveMealPlan(MealPlan mealPlan);
    Task DeleteMealPlan(long mealPlanId);
}