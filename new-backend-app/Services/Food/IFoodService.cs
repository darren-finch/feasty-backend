using new_backend.Models;

namespace new_backend.Services
{
    public interface IFoodService
    {
        Task<IList<Food>> GetFoods(string? titleQuery = null);
        Task<Food> GetFoodById(long foodId);

        // Saves the provided food to the database.
        // If the food already exists,
        // it will update all properties of the food except for the id and userId, and return the id of the food.
        // If the food does not exist, 
        // it will create a new food with the userId set to the id of the current user and return the id of the food.
        Task<long> SaveFood(Food food);
        Task DeleteFood(long foodId);
    }
}