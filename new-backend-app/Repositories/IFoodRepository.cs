using new_backend.Models;

namespace new_backend.Repositories
{
    public interface IFoodRepository
    {
        Task<IList<Food>> GetAllFoodsForCurrentUser();
        Task<IList<Food>> FindFoodsForCurrentUser(string titleQuery);
        Task<Food?> GetFoodById(long foodId);
        Task<long> AddFood(Food food);
        Task<long> UpdateFood(Food food);
        Task DeleteFood(Food food);
    }
}