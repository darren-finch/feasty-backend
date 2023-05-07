using System.ComponentModel.DataAnnotations;

namespace new_backend.Models;

public class MealPlan
{
    [Required]
    public virtual long Id { get; set; }

    public virtual long UserId { get; set; }

    [Required]
    public virtual string Title { get; set; }

    [Required]
    public virtual int RequiredCalories { get; set; }

    [Required]
    public virtual int RequiredFats { get; set; }

    [Required]
    public virtual int RequiredCarbs { get; set; }

    [Required]
    public virtual int RequiredProteins { get; set; }

    public virtual List<MealPlanMeal> MealPlanMeals { get; set; } = new List<MealPlanMeal>();
}