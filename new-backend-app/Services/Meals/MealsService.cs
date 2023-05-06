using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Repositories;

namespace new_backend.Services;

public class MealsService : IMealsService
{
    private IFoodRepository foodRepository;
    private IMealsRepository mealsRepository;
    private long userId;

    public MealsService(IFoodRepository foodRepository, IMealsRepository mealRepository, IAuthManager authManager)
    {
        this.foodRepository = foodRepository;
        this.mealsRepository = mealRepository;
        this.userId = authManager.GetUserId();
    }

    public async Task<IList<Meal>> GetMeals(string? titleQuery = null)
    {
        if (titleQuery == null || titleQuery == "")
        {
            return await mealsRepository.GetAllMealsForUser(userId);
        }
        else
        {
            return await mealsRepository.GetMealsByTitleForUser(titleQuery, userId);
        }
    }

    public async Task<Meal> GetMealById(long mealId)
    {
        var meal = await mealsRepository.GetMealById(mealId);
        if (meal == null)
        {
            throw new NotFoundException($"Could not find a meal with id {mealId}");
        }
        if (meal.UserId != userId)
        {
            throw new UnauthorizedException($"You are not authorized to access this meal.");
        }

        return meal;
    }

    public async Task<long> SaveMeal(Meal meal)
    {
        var foodsForMealFoods = await foodRepository.GetFoodsByIds(meal.MealFoods.Select(mealFood => mealFood.FoodId).ToList());
        foreach (var mealFood in meal.MealFoods)
        {
            var food = foodsForMealFoods.FirstOrDefault(food => food.Id == mealFood.FoodId);
            if (food == null)
            {
                throw new NotFoundException($"Could not find a food with id {mealFood.FoodId} for meal food on meal.");
            }
            if (food.UserId != userId)
            {
                throw new UnauthorizedException($"You are not authorized to access this food.");
            }

            // TODO: Test this if it's not too much of a pain
            mealFood.BaseFood = food;
            mealFood.Meal = meal;
        }

        if (meal.Id < 0)
        {
            meal.UserId = userId;
            await mealsRepository.AddMeal(meal);
        }
        else
        {
            var existingMeal = await mealsRepository.GetMealById(meal.Id);
            if (existingMeal == null)
            {
                throw new NotFoundException($"Could not find a meal with id {meal.Id}");
            }
            if (existingMeal.UserId != userId)
            {
                throw new UnauthorizedException($"You are not authorized to access this meal.");
            }

            existingMeal.Title = meal.Title;
            existingMeal.MealFoods = meal.MealFoods;

            await mealsRepository.UpdateMeal(existingMeal);
        }

        return meal.Id;
    }

    public async Task DeleteMeal(long mealId)
    {
        var existingMeal = await mealsRepository.GetMealById(mealId);
        if (existingMeal == null)
        {
            throw new NotFoundException($"Could not find a meal with id {mealId} to delete.");
        }
        if (existingMeal.UserId != userId)
        {
            throw new UnauthorizedException($"You are not authorized to delete this meal.");
        }

        await mealsRepository.DeleteMeal(existingMeal);
    }
}