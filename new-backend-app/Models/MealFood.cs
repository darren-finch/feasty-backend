using System.Text.Json.Serialization;

namespace new_backend.Models;

public class MealFood
{

    // We want cascade delete to happen when a meal is deleted so this is not nullable.
    public virtual long MealId { get; set; }

    // We want cascade delete to happen when a food is deleted so this is not nullable.
    public virtual long FoodId { get; set; }

    [JsonIgnore]
    public virtual Meal Meal { get; set; } = null!;

    public virtual Food BaseFood { get; set; } = null!;

    public virtual double DesiredQuantity { get; set; }
}