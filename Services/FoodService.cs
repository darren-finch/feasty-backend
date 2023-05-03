using Microsoft.EntityFrameworkCore;
using new_backend.Data;
using new_backend.Exceptions;
using new_backend.Models;

namespace new_backend.Services
{
    public class FoodService
    {
        private FeastyDbContext dbContext;
        public FoodService(FeastyDbContext dBContext)
        {
            this.dbContext = dBContext;
        }

        public async Task<List<Food>> GetFoods(string? titleQuery = null)
        {
            return await dbContext.Foods.Where(food => titleQuery != null ? food.Title.ToLower().Contains(titleQuery.ToLower()) : true).ToListAsync();
        }

        public async Task<Food> GetFoodById(long foodId)
        {
            var food = await dbContext.Foods.FindAsync(foodId);

            if (food == null)
            {
                throw new NotFoundException("Could not find a food with id " + foodId);
            }

            return food;
        }

        // Returns id of food after saving it to the database. This is an idempotent operation.
        public async Task<long> SaveFood(Food food)
        {
            if (food.Id < 0)
            {
                // This is a new food, so we need to set the id to 0 so that EF Core knows to automatically generate an id for it.
                food.Id = 0;

                dbContext.Foods.Add(food);
            }
            else
            {
                // This is an existing food, so we need to update it.
                var foodToUpdate = dbContext.Foods.Find(food.Id);
                if (foodToUpdate == null)
                {
                    throw new NotFoundException("Could not find food with id " + food.Id + " to update.");
                }

                foodToUpdate.Title = food.Title;
                foodToUpdate.Quantity = food.Quantity;
                foodToUpdate.Unit = food.Unit;
                foodToUpdate.Calories = food.Calories;
                foodToUpdate.Fats = food.Fats;
                foodToUpdate.Carbs = food.Carbs;
                foodToUpdate.Proteins = food.Proteins;
            }

            await dbContext.SaveChangesAsync();

            return food.Id;

        }

        public async Task DeleteFood(long foodId)
        {
            var food = await dbContext.Foods.FindAsync(foodId);

            if (food == null)
            {
                throw new NotFoundException("Could not find food with id " + foodId + " to delete.");
            }

            dbContext.Foods.Remove(food);

            await dbContext.SaveChangesAsync();
        }
    }
}
