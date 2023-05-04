using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using new_backend.Data;
using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Services;

namespace new_backend.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private FeastyDbContext dbContext;
        private long userId;

        public FoodRepository(FeastyDbContext dbContext, IAuthManager authManager, ILogger<FoodRepository> logger)
        {
            this.dbContext = dbContext;

            this.userId = authManager.GetUserId();
        }

        private long GetUserIdNumberFromAuthName(string authName)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < authName.Length; i++)
            {
                if (char.IsDigit(authName[i]))
                {
                    sb.Append(authName[i]);
                }
            }

            return long.Parse(sb.ToString().Substring(0, Math.Min(10, sb.Length)));
        }

        public async Task<IList<Food>> FindFoodsForCurrentUser(string titleQuery)
        {
            return await dbContext.Foods.Where(food => food.Title.ToLower().Contains(titleQuery.ToLower()) && food.UserId == userId).ToListAsync();
        }

        public async Task<IList<Food>> GetAllFoodsForCurrentUser()
        {
            return await dbContext.Foods.Where(food => food.UserId == userId).ToListAsync();
        }

        public async Task<Food?> GetFoodById(long foodId)
        {
            var food = await dbContext.Foods.FindAsync(foodId);
            if (food != null && food.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to access this food.");
            }
            return food;
        }

        public async Task<long> AddFood(Food food)
        {
            food.UserId = userId;

            // This is a new food, so we need to set the id to 0 so that EF Core knows to automatically generate an id for it.
            food.Id = 0;

            dbContext.Foods.Add(food);
            await dbContext.SaveChangesAsync();

            return food.Id;
        }

        // Assumes that the food is being tracked by EF Core.
        public async Task<long> UpdateFood(Food food)
        {
            var trackedFood = await dbContext.Foods.FindAsync(food.Id);

            if (trackedFood == null)
            {
                throw new NotFoundException("Could not find food with id " + food.Id + " to update.");
            }

            if (trackedFood.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to update this food.");
            }

            trackedFood.Title = food.Title;
            trackedFood.Quantity = food.Quantity;
            trackedFood.Unit = food.Unit;
            trackedFood.Calories = food.Calories;
            trackedFood.Fats = food.Fats;
            trackedFood.Carbs = food.Carbs;
            trackedFood.Proteins = food.Proteins;

            await dbContext.SaveChangesAsync();

            return food.Id;
        }

        public async Task DeleteFood(Food food)
        {
            if (food.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to delete this food.");
            }

            dbContext.Foods.Remove(food);

            await dbContext.SaveChangesAsync();
        }
    }
}