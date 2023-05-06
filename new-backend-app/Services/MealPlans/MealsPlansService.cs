using new_backend.Exceptions;
using new_backend.Models;

namespace new_backend.Services;

public class MealPlansService : IMealPlansService
{
    private readonly IMealPlansRepository mealPlansRepository;

    private long userId;

    public MealPlansService(IMealPlansRepository mealPlansRepository, IAuthManager authManager)
    {
        this.mealPlansRepository = mealPlansRepository;
        this.userId = authManager.GetUserId();
    }

    public async Task<IList<MealPlan>> GetMealPlans()
    {
        return await mealPlansRepository.GetMealPlansForUser(userId);
    }

    public async Task<MealPlan> GetMealPlanById(long mealPlanId)
    {
        var mealPlan = await mealPlansRepository.GetMealPlanById(mealPlanId);
        if (mealPlan == null)
        {
            throw new NotFoundException($"Could not find meal plan with id {mealPlanId}");
        }
        if (mealPlan.UserId != userId)
        {
            throw new UnauthorizedException($"You do not have access to meal plan with id {mealPlanId}");
        }

        return mealPlan;
    }

    public async Task<long> SaveMealPlan(MealPlan newMealPlan)
    {
        if (newMealPlan.Id < 0)
        {
            newMealPlan.UserId = userId;
            return await mealPlansRepository.AddMealPlan(newMealPlan);
        }
        else
        {
            var existingMealPlan = await mealPlansRepository.GetMealPlanById(newMealPlan.Id);

            if (existingMealPlan == null)
            {
                throw new NotFoundException($"Could not find meal plan with id {newMealPlan.Id} to update.");
            }

            if (existingMealPlan.UserId != userId)
            {
                throw new UnauthorizedException($"You do not have authorization to update meal plan with id {newMealPlan.Id}");
            }

            existingMealPlan.Title = newMealPlan.Title;
            existingMealPlan.RequiredCalories = newMealPlan.RequiredCalories;
            existingMealPlan.RequiredFats = newMealPlan.RequiredFats;
            existingMealPlan.RequiredCarbs = newMealPlan.RequiredCarbs;
            existingMealPlan.RequiredProteins = newMealPlan.RequiredProteins;

            return await mealPlansRepository.UpdateMealPlan(newMealPlan);
        }
    }

    public async Task DeleteMealPlan(long mealPlanId)
    {
        var existingMealPlan = await mealPlansRepository.GetMealPlanById(mealPlanId);

        if (existingMealPlan == null)
        {
            throw new NotFoundException($"Could not find meal plan with id {mealPlanId} to delete.");
        }

        if (existingMealPlan.UserId != userId)
        {
            throw new UnauthorizedException($"You do not have authorization to delete meal plan with id {mealPlanId}");
        }

        await mealPlansRepository.DeleteMealPlan(existingMealPlan);
    }
}