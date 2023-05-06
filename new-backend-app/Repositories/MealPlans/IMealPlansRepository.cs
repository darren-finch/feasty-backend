using new_backend.Models;

public interface IMealPlansRepository
{
    Task<MealPlan?> GetMealPlanById(long id);
}