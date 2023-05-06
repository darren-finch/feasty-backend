using new_backend.Models;

namespace new_backend.Repositories
{
    public interface IFoodRepository
    {
        Task<IList<Food>> GetAllFoodsForUser(long userId);
        Task<IList<Food>> GetFoodsByTitleForUser(string titleQuery, long userId);
        Task<IList<Food>> GetFoodsByIds(IList<long> foodIds);
        Task<Food?> GetFoodById(long foodId);
        Task<long> AddFood(Food food);
        Task DeleteFood(Food food);
        Task Save();
    }
}