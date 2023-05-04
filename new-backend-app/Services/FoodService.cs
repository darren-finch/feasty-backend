using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Repositories;

namespace new_backend.Services
{
    public class FoodService : IFoodService
    {
        private IFoodRepository foodRepository;

        public FoodService(IFoodRepository foodRepository)
        {
            this.foodRepository = foodRepository;
        }

        public async Task<IList<Food>> GetFoods(string? titleQuery = null)
        {
            if (titleQuery == null)
            {
                return await foodRepository.GetAllFoodsForCurrentUser();
            }
            else
            {
                return await foodRepository.FindFoodsForCurrentUser(titleQuery);
            }
        }

        public async Task<Food> GetFoodById(long foodId)
        {
            var food = await foodRepository.GetFoodById(foodId);
            if (food == null)
            {
                throw new NotFoundException("Could not find a food with id " + foodId);
            }

            return food;
        }

        public async Task<long> SaveFood(Food food)
        {
            if (food.Id < 0)
            {
                await foodRepository.AddFood(food);
            }
            else
            {
                await foodRepository.UpdateFood(food);
            }

            return food.Id;

        }

        public async Task DeleteFood(long foodId)
        {
            var food = await foodRepository.GetFoodById(foodId);

            if (food == null)
            {
                throw new NotFoundException("Could not find food with id " + foodId + " to delete.");
            }

            await foodRepository.DeleteFood(food);
        }
    }
}
