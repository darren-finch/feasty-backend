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

        public async Task<IList<Food>> GetAllFoodsForUser(long userId)
        {
            return await dbContext.Foods.Where(food => food.UserId == userId).ToListAsync();
        }

        public async Task<IList<Food>> GetFoodsByTitleForUser(string titleQuery, long userId)
        {
            return await dbContext.Foods.Where(food => food.Title.ToLower().Contains(titleQuery.ToLower()) && food.UserId == userId).ToListAsync();
        }

        public async Task<Food?> GetFoodById(long foodId)
        {
            var food = await dbContext.Foods.FindAsync(foodId);
            return food;
        }

        public async Task<long> AddFood(Food food)
        {
            // This is a new food, so we need to set the id to 0 so that EF Core knows to automatically generate an id for it.
            food.Id = 0;

            dbContext.Foods.Add(food);
            await dbContext.SaveChangesAsync();

            return food.Id;
        }

        // Assumes that the food is being tracked by EF Core so it just saves the changes.
        public async Task<long> UpdateFood(Food food)
        {
            await dbContext.SaveChangesAsync();

            return food.Id;
        }

        public async Task DeleteFood(Food food)
        {
            dbContext.Foods.Remove(food);

            await dbContext.SaveChangesAsync();
        }
    }
}