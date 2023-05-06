using Microsoft.AspNetCore.Mvc;
using new_backend.Models;
using new_backend.Services;

namespace new_backend.Controllers;

[Route("meals")]
public class MealsController : Controller
{
    private IMealsService mealsService;

    public MealsController(IMealsService mealsService)
    {
        this.mealsService = mealsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMeals([FromQuery(Name = "title")] string? titleQuery = null)
    {
        return Ok(await mealsService.GetMeals(titleQuery));
    }

    [HttpGet("{mealId}")]
    public async Task<IActionResult> GetMealById(long mealId)
    {
        return Ok(await mealsService.GetMealById(mealId));
    }

    [HttpPut]
    public async Task<IActionResult> SaveMeal([FromBody] Meal meal)
    {
        return Ok(await mealsService.SaveMeal(meal));
    }

    [HttpDelete("{mealId}")]
    public async Task<IActionResult> DeleteMeal(long mealId)
    {
        await mealsService.DeleteMeal(mealId);
        return Ok();
    }
}