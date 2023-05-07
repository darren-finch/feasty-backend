using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Repositories;

namespace new_backend.Services
{
    public class FoodService : IFoodService
    {
        private IFoodRepository foodRepository;

        private long userId = 1;

        public FoodService(IFoodRepository foodRepository, IAuthManager authManager)
        {
            this.foodRepository = foodRepository;
            this.userId = authManager.GetUserId();
        }

        public async Task<IList<Food>> GetFoods(string? titleQuery = null)
        {
            if (titleQuery == null)
            {
                return await foodRepository.GetAllFoodsForUser(userId);
            }
            else
            {
                return await foodRepository.GetFoodsByTitleForUser(titleQuery, userId);
            }
        }

        public async Task<Food> GetFoodById(long foodId)
        {
            var food = await foodRepository.GetFoodById(foodId);
            if (food == null)
            {
                throw new NotFoundException("Could not find a food with id " + foodId);
            }
            if (food.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to access this food.");
            }

            return food;
        }

        public async Task<long> SaveFood(Food food)
        {
            if (food.Id < 0)
            {
                food.UserId = userId;
                food.Id = await foodRepository.AddFood(food);
            }
            else
            {
                var existingFood = await foodRepository.GetFoodById(food.Id);

                if (existingFood == null)
                {
                    throw new NotFoundException("Could not find food with id " + food.Id + " to update.");
                }

                if (existingFood.UserId != userId)
                {
                    throw new UnauthorizedException("You are not authorized to update this food.");
                }

                existingFood.Title = food.Title;
                existingFood.Quantity = food.Quantity;
                existingFood.Unit = food.Unit;
                existingFood.Calories = food.Calories;
                existingFood.Fats = food.Fats;
                existingFood.Carbs = food.Carbs;
                existingFood.Proteins = food.Proteins;

                await foodRepository.Save();
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
            if (food.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to delete this food.");
            }

            await foodRepository.DeleteFood(food);
        }
    }
}
