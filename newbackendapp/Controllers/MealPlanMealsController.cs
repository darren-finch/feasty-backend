using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using new_backend.Exceptions;
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
    public async Task<IActionResult> SaveMealPlanMeal([FromBody] MealPlanMeal mealPlanMeal)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException($"Invalid meal plan meal object.");

        await mealPlanMealService.SaveMealPlanMeal(mealPlanMeal);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMealPlanMeal([FromQuery][BindRequired] long mealId, [FromQuery][BindRequired] long mealPlanId)
    {
        await mealPlanMealService.DeleteMealPlanMeal(mealId, mealPlanId);
        return Ok();
    }
}