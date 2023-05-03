using Microsoft.AspNetCore.Mvc;
using new_backend.Models;
using new_backend.Services;

namespace new_backend.Controllers
{
    public class FoodController : Controller
    {
        private FoodService foodService;

        public FoodController(FoodService foodService)
        {
            this.foodService = foodService;
        }

        [HttpGet("/api/foods")]
        public async Task<IActionResult> GetFoods([FromQuery] string? title = null)
        {
            return Ok(await foodService.GetFoods(title));
        }

        [HttpGet("/api/foods/{foodId}")]
        public async Task<IActionResult> GetFoodById(long foodId)
        {
            return Ok(await foodService.GetFoodById(foodId));
        }

        [HttpPut("/api/foods")]
        public async Task<IActionResult> SaveFood([FromBody] Food food)
        {
            return Ok(await foodService.SaveFood(food));
        }

        [HttpDelete("/api/foods/{foodId}")]
        public async Task<IActionResult> DeleteFood(long foodId)
        {
            await foodService.DeleteFood(foodId);
            return Ok();
        }
    }
}

