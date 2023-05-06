using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Repositories;

namespace new_backend.Services;

public class MealPlanMealsService : IMealPlanMealsService
{
    private readonly IMealsRepository mealsRepository;
    private readonly IMealPlansRepository mealPlansRepository;
    private readonly IMealPlanMealsRepository mealPlanMealRepository;

    private long userId;

    public MealPlanMealsService(IMealsRepository mealsRepository, IMealPlansRepository mealPlansRepository, IMealPlanMealsRepository mealPlanMealRepository, IAuthManager authManager)
    {
        this.mealsRepository = mealsRepository;
        this.mealPlansRepository = mealPlansRepository;
        this.mealPlanMealRepository = mealPlanMealRepository;
        this.userId = authManager.GetUserId();
    }

    public async Task SaveMealPlanMeal(MealPlanMeal mealPlanMeal)
    {
        var existingMealPlanMeal = await mealPlanMealRepository.GetMealPlanMeal(mealPlanMeal.MealId, mealPlanMeal.MealPlanId);

        if (existingMealPlanMeal != null)
        {
            return;
        }

        var existingMeal = await mealsRepository.GetMealById(mealPlanMeal.MealId);
        var existingMealPlan = await mealPlansRepository.GetMealPlanById(mealPlanMeal.MealPlanId);

        if (existingMeal == null)
        {
            throw new NotFoundException("Could not find a meal with the given meal id.");
        }

        if (existingMealPlan == null)
        {
            throw new NotFoundException("Could not find a meal plan with the given meal plan id.");
        }

        if (existingMeal.UserId != userId)
        {
            throw new UnauthorizedException("You do not have permission to add this meal to your meal plan.");
        }

        if (existingMealPlan.UserId != userId)
        {
            throw new UnauthorizedException("You do not have permission to add your meal to this meal plan.");
        }

        await mealPlanMealRepository.SaveMealPlanMeal(mealPlanMeal);
    }

    public async Task DeleteMealPlanMeal(long mealId, long mealPlanId)
    {
        var existingMealPlanMeal = await mealPlanMealRepository.GetMealPlanMeal(mealId, mealPlanId);

        if (existingMealPlanMeal == null)
        {
            throw new NotFoundException($"Could not find a meal plan meal with meal id {mealId} and meal plan id {mealPlanId}.");
        }

        if (existingMealPlanMeal.Meal.UserId != userId)
        {
            throw new UnauthorizedException("You do not have permission to add this meal to your meal plan.");
        }

        if (existingMealPlanMeal.MealPlan.UserId != userId)
        {
            throw new UnauthorizedException("You do not have permission to add your meal to this meal plan.");
        }

        await mealPlanMealRepository.DeleteMealPlanMeal(existingMealPlanMeal);
    }
}