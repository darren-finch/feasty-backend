using Microsoft.AspNetCore.Mvc;
using new_backend.Models;
using new_backend.Services;

namespace new_backend.Controllers;

[Route("mealplans")]
public class MealPlansController : Controller
{
    private readonly IMealPlansService mealPlansService;

    public MealPlansController(IMealPlansService mealPlansService)
    {
        this.mealPlansService = mealPlansService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMealPlans()
    {
        return Ok(await mealPlansService.GetMealPlans());
    }

    [HttpGet("{mealPlanId}")]
    public async Task<IActionResult> GetMealPlanById(long mealPlanId)
    {
        return Ok(await mealPlansService.GetMealPlanById(mealPlanId));
    }

    [HttpPut]
    public async Task<IActionResult> SaveMealPlan([FromBody] MealPlan mealPlan)
    {
        return Ok(await mealPlansService.SaveMealPlan(mealPlan));
    }

    [HttpDelete("{mealPlanId}")]
    public async Task<IActionResult> DeleteMealPlan(long mealPlanId)
    {
        await mealPlansService.DeleteMealPlan(mealPlanId);
        return Ok();
    }
}