using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace new_backend.Models;

public class MealPlanMeal
{
    [Required]
    public virtual long MealPlanId { get; set; }

    [Required]
    public virtual long MealId { get; set; }

    [JsonIgnore]
    public virtual MealPlan MealPlan { get; set; }

    public virtual Meal Meal { get; set; }
}