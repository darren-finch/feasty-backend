using Microsoft.AspNetCore.Mvc;
using new_backend.Models;
using new_backend.Services;

namespace new_backend.Controllers;

[Route("mealplanmeals")]
public class MealPlanMealsController : Controller
{
    private readonly IMealPlanMealsService mealPlanMealService;

    public MealPlanMealsController(IMealPlanMealsService mealPlanMealService)
    {
        this.mealPlanMealService = mealPlanMealService;
    }

    [HttpPut]
    public async Task<IActionResult> SaveMealPlanMeal(MealPlanMeal mealPlanMeal)
    {
        await mealPlanMealService.SaveMealPlanMeal(mealPlanMeal);
        return Ok();
    }

    [HttpDelete("{mealPlanMealId}")]
    public async Task<IActionResult> DeleteMealPlanMeal(long mealId, long mealPlanId)
    {
        await mealPlanMealService.DeleteMealPlanMeal(mealId, mealPlanId);
        return Ok();
    }
}