using Microsoft.AspNetCore.Mvc;
using new_backend.Models;
using new_backend.Services;

namespace new_backend.Controllers
{
    [Route("foods")]
    public class FoodController : Controller
    {
        private IFoodService foodService;

        public FoodController(IFoodService foodService)
        {
            this.foodService = foodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoods([FromQuery(Name = "title")] string? titleQuery)
        {
            return Ok(await foodService.GetFoods(titleQuery));
        }

        [HttpGet("{foodId}")]
        public async Task<IActionResult> GetFoodById(long foodId)
        {
            return Ok(await foodService.GetFoodById(foodId));
        }

        [HttpPut]
        public async Task<IActionResult> SaveFood([FromBody] Food food)
        {
            return Ok(await foodService.SaveFood(food));
        }

        [HttpDelete("{foodId}")]
        public async Task<IActionResult> DeleteFood(long foodId)
        {
            await foodService.DeleteFood(foodId);
            return Ok();
        }
    }
}

