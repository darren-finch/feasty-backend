using System.ComponentModel.DataAnnotations;

namespace new_backend.Models;

public class Meal
{
    [Required]
    public virtual long Id { get; set; }
    [Required]
    public virtual long UserId { get; set; }
    [Required]
    public virtual string Title { get; set; } = null!;

    public virtual IList<MealFood> MealFoods { get; set; } = new List<MealFood>();

    public Meal()
    {
    }
}