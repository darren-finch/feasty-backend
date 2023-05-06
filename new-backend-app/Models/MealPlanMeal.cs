using System.Text.Json.Serialization;

namespace new_backend.Models;

public class MealPlanMeal
{
    public virtual long MealPlanId { get; set; }

    [JsonIgnore]
    public virtual MealPlan MealPlan { get; set; }
    public virtual long MealId { get; set; }
    public virtual Meal Meal { get; set; }
}