namespace new_backend.Models;

public class Meal
{
    public virtual long Id { get; set; }
    public virtual long UserId { get; set; }
    public virtual string Title { get; set; } = null!;
    public virtual IList<MealFood> MealFoods { get; set; } = new List<MealFood>();

    public Meal()
    {
    }
}